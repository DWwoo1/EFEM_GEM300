using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;

using Define.DefineEnumProject.Modbus;

namespace FrameOfSystem3.Controller.ModbusShared
{
    #region Modbus FunctionCode
    public enum ModbusFunctionCode : byte
    {
        ReadCoils = 1,             // FC1
        ReadDiscreteInputs = 2,    // FC2
        ReadHoldingRegisters = 3,  // FC3
        ReadInputRegisters = 4,    // FC4
        WriteSingleCoil = 5,       // FC5
        WriteSingleRegister = 6    // FC6
        // FC15/16 필요 시 확장
    }
    #endregion

    #region Poll Handle
    public interface IPollHandle : IDisposable
    {
        Guid Id { get; }
    }

    internal sealed class PollHandle : IPollHandle
    {
        private readonly Action _dispose;
        public Guid Id { get; }

        public PollHandle(Action dispose, Guid id)
        {
            _dispose = dispose ?? throw new ArgumentNullException(nameof(dispose));
            Id = id;
        }

        public void Dispose() => _dispose();
    }
    #endregion

    #region Shared Client Key (NO readonly struct)
    internal sealed class ModbusClientKey : IEquatable<ModbusClientKey>
    {
        public string Ip { get; }
        public int Port { get; }
        public EN_MODBUS_SERVER_PROTOCOL Protocol { get; }

        public int IoTimeoutMs { get; }
        public int ReconnectDelayMs { get; }
        public int MaxReconnectAttempts { get; }

        public bool KeepAliveEnabled { get; }
        public bool DebugSafeMode { get; }
        public int DebugGapResetMs { get; }

        public ModbusClientKey(
            string ip, int port, EN_MODBUS_SERVER_PROTOCOL protocol,
            int ioTimeoutMs, int reconnectDelayMs, int maxReconnectAttempts,
            bool keepAliveEnabled, bool debugSafeMode, int debugGapResetMs)
        {
            Ip = (ip ?? "").Trim();
            Port = port;
            Protocol = protocol;

            IoTimeoutMs = ioTimeoutMs;
            ReconnectDelayMs = reconnectDelayMs;
            MaxReconnectAttempts = maxReconnectAttempts;

            KeepAliveEnabled = keepAliveEnabled;
            DebugSafeMode = debugSafeMode;
            DebugGapResetMs = debugGapResetMs;
        }

        public bool Equals(ModbusClientKey other)
        {
            if (ReferenceEquals(other, null)) return false;

            return string.Equals(Ip, other.Ip, StringComparison.OrdinalIgnoreCase)
                && Port == other.Port
                && Protocol == other.Protocol
                && IoTimeoutMs == other.IoTimeoutMs
                && ReconnectDelayMs == other.ReconnectDelayMs
                && MaxReconnectAttempts == other.MaxReconnectAttempts
                && KeepAliveEnabled == other.KeepAliveEnabled
                && DebugSafeMode == other.DebugSafeMode
                && DebugGapResetMs == other.DebugGapResetMs;
        }

        public override bool Equals(object obj) => Equals(obj as ModbusClientKey);

        public override int GetHashCode()
        {
            unchecked
            {
                int h = StringComparer.OrdinalIgnoreCase.GetHashCode(Ip);
                h = (h * 397) ^ Port;
                h = (h * 397) ^ (int)Protocol;
                h = (h * 397) ^ IoTimeoutMs;
                h = (h * 397) ^ ReconnectDelayMs;
                h = (h * 397) ^ MaxReconnectAttempts;
                h = (h * 397) ^ (KeepAliveEnabled ? 1 : 0);
                h = (h * 397) ^ (DebugSafeMode ? 1 : 0);
                h = (h * 397) ^ DebugGapResetMs;
                return h;
            }
        }
    }

    #endregion

    #region ModbusTcpClient Registry (shared by IP/Port policy)
    internal sealed class ModbusTcpClientRegistry
    {
        private sealed class Entry
        {
            public ModbusTcpClient Client;
            public int RefCount;
        }

        private static readonly ModbusTcpClientRegistry _instance = new ModbusTcpClientRegistry();
        public static ModbusTcpClientRegistry Instance => _instance;

        private readonly object _sync = new object();
        private readonly Dictionary<ModbusClientKey, Entry> _entries = new Dictionary<ModbusClientKey, Entry>();

        private ModbusTcpClientRegistry() { }

        public ModbusTcpClient Acquire(ModbusClientKey key)
        {
            lock (_sync)
            {
                // class key라서 Equals 기반 검색
                foreach (var kv in _entries)
                {
                    if (kv.Key.Equals(key))
                    {
                        kv.Value.RefCount++;
                        return kv.Value.Client;
                    }
                }

                if (key.Protocol != EN_MODBUS_SERVER_PROTOCOL.TCP)
                    throw new NotSupportedException("Only TCP is supported for ModbusTcpClient.");

                var client = new ModbusTcpClient(
                    host: key.Ip,
                    port: key.Port,
                    ioTimeoutMs: key.IoTimeoutMs,
                    reconnectDelayMs: key.ReconnectDelayMs,
                    maxReconnectAttempts: key.MaxReconnectAttempts,
                    keepAliveEnabled: key.KeepAliveEnabled,
                    debugSafeMode: key.DebugSafeMode,
                    debugGapResetMs: key.DebugGapResetMs);

                client.Start();

                _entries.Add(key, new Entry { Client = client, RefCount = 1 });
                return client;
            }
        }

        public void Release(ModbusClientKey key)
        {
            ModbusTcpClient toDispose = null;

            lock (_sync)
            {
                ModbusClientKey foundKey = null;
                Entry foundEntry = null;

                foreach (var kv in _entries)
                {
                    if (kv.Key.Equals(key))
                    {
                        foundKey = kv.Key;
                        foundEntry = kv.Value;
                        break;
                    }
                }
                if (foundEntry == null) return;

                foundEntry.RefCount--;
                if (foundEntry.RefCount <= 0)
                {
                    toDispose = foundEntry.Client;
                    _entries.Remove(foundKey);
                }
            }

            toDispose?.Dispose();
        }
    }
    #endregion

    #region ModbusTcpClient (1ms IO thread, serialized, Poll interval supported)
    public sealed class ModbusTcpClient : IDisposable
    {
        public event Action Connected;
        public event Action<Exception> Disconnected;

        private readonly string _host;
        private readonly int _port;

        private readonly int _ioTimeoutMs;
        private readonly int _reconnectDelayMs;
        private readonly int _maxReconnectAttempts;
        private readonly bool _keepAliveEnabled;
        private readonly bool _debugSafeMode;
        private readonly int _debugGapResetMs;

        private TcpClient _tcpClient;
        private NetworkStream _stream;

        private readonly Thread _ioThread;
        private volatile bool _stopRequested;

        private readonly List<PollJob> _pollJobs = new List<PollJob>();
        private readonly ConcurrentQueue<WriteRequest> _writeQueue = new ConcurrentQueue<WriteRequest>();

        private readonly ConcurrentDictionary<CacheKey, CacheEntry<byte[]>> _bitCache = new ConcurrentDictionary<CacheKey, CacheEntry<byte[]>>();

        private readonly ConcurrentDictionary<CacheKey, CacheEntry<ushort[]>> _regCache =
            new ConcurrentDictionary<CacheKey, CacheEntry<ushort[]>>();

        private volatile bool _isConnected;
        private ushort _transactionId;

        private readonly Stopwatch _sw = Stopwatch.StartNew();
        private long _nextTickMs;
        private long _lastLoopMs;

        public ModbusTcpClient(string host, int port, int ioTimeoutMs, int reconnectDelayMs, int maxReconnectAttempts,
             bool keepAliveEnabled, bool debugSafeMode, int debugGapResetMs)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _port = port;
            _ioTimeoutMs = ioTimeoutMs;
            _reconnectDelayMs = reconnectDelayMs;
            _maxReconnectAttempts = maxReconnectAttempts;

            _keepAliveEnabled = keepAliveEnabled;
            _debugSafeMode = debugSafeMode;
            _debugGapResetMs = debugGapResetMs <= 0 ? 2000 : debugGapResetMs;

            _ioThread = new Thread(IoLoop)
            {
                IsBackground = true,
                Name = "ModbusTcpClient.IoThread"
            };
        }

        public void Start() => _ioThread.Start();

        public void Dispose()
        {
            _stopRequested = true;

            try 
            {
                _ioThread.Join(1000); 
            }
            catch { }
            
            CloseSocket();
        }

        #region Poll registration (intervalMs)
        public IPollHandle AddDiscreteInputsPoll(byte unitId, ushort startAddress, ushort quantity, int intervalMs)
            => AddPoll(unitId, ModbusFunctionCode.ReadDiscreteInputs, startAddress, quantity, intervalMs);

        public IPollHandle AddCoilsPoll(byte unitId, ushort startAddress, ushort quantity, int intervalMs)
            => AddPoll(unitId, ModbusFunctionCode.ReadCoils, startAddress, quantity, intervalMs);

        public IPollHandle AddHoldingRegistersPoll(byte unitId, ushort startAddress, ushort quantity, int intervalMs)
            => AddPoll(unitId, ModbusFunctionCode.ReadHoldingRegisters, startAddress, quantity, intervalMs);

        private IPollHandle AddPoll(byte unitId, ModbusFunctionCode fc, ushort startAddress, ushort quantity, int intervalMs)
        {
            if (intervalMs < 1) intervalMs = 1;

            var job = new PollJob(Guid.NewGuid(), unitId, fc, startAddress, quantity, intervalMs, _sw.ElapsedMilliseconds);

            lock (_pollJobs)
            {
                _pollJobs.Add(job);
            }

            return new PollHandle(() =>
            {
                lock (_pollJobs)
                {
                    _pollJobs.RemoveAll(j => j.Id == job.Id);
                }
            }, job.Id);
        }

        private sealed class PollJob
        {
            public Guid Id { get; }
            public byte UnitId { get; }
            public ModbusFunctionCode FunctionCode { get; }
            public ushort StartAddress { get; }
            public ushort Quantity { get; }

            public int IntervalMs { get; }
            public long NextDueMs { get; set; }

            public PollJob(Guid id, byte unitId, ModbusFunctionCode fc, ushort start, ushort qty, int intervalMs, long nowMs)
            {
                Id = id;
                UnitId = unitId;
                FunctionCode = fc;
                StartAddress = start;
                Quantity = qty;
                IntervalMs = intervalMs;
                NextDueMs = nowMs; // 즉시 첫 실행
            }
        }
        #endregion

        #region Cache read (Non-blocking)
        public bool TryGetCachedDiscreteInputs(byte unitId, ushort startAddress, ushort quantity, out bool[] values, out DateTime timestamp)
            => TryGetBitCache(unitId, ModbusFunctionCode.ReadDiscreteInputs, startAddress, quantity, out values, out timestamp);

        public bool TryGetCachedCoils(byte unitId, ushort startAddress, ushort quantity, out bool[] values, out DateTime timestamp)
            => TryGetBitCache(unitId, ModbusFunctionCode.ReadCoils, startAddress, quantity, out values, out timestamp);

        public bool TryGetCachedHoldingRegisters(byte unitId, ushort startAddress, ushort quantity, out ushort[] registers, out DateTime timestamp)
            => TryGetRegCache(unitId, ModbusFunctionCode.ReadHoldingRegisters, startAddress, quantity, out registers, out timestamp);

        private bool TryGetBitCache(byte unitId, ModbusFunctionCode fc, ushort start, ushort qty, out bool[] bits, out DateTime ts)
        {
            var key = new CacheKey(unitId, fc, start, qty);
            if (_bitCache.TryGetValue(key, out var entry))
            {
                bits = (bool[])entry.Value.Clone();
                ts = entry.Timestamp;
                return true;
            }
            bits = null;
            ts = default(DateTime);
            return false;
        }

        public bool TryGetCachedDiscreteInputsPacked(byte unitId, ushort startAddress, ushort quantity,
            out byte[] packed, out DateTime timestamp)
        {
            return TryGetBitCachePacked(unitId, ModbusFunctionCode.ReadDiscreteInputs, startAddress, quantity, out packed, out timestamp);
        }

        public bool TryGetCachedCoilsPacked(byte unitId, ushort startAddress, ushort quantity,
            out byte[] packed, out DateTime timestamp)
        {
            return TryGetBitCachePacked(unitId, ModbusFunctionCode.ReadCoils, startAddress, quantity, out packed, out timestamp);
        }
        public bool TryGetCachedBitsMask8(byte unitId, ModbusFunctionCode fc, ushort pollStart, ushort pollQuantity,
            int bitOffsetFromPollStart, out uint mask, out DateTime timestamp)
        {
            var key = new CacheKey(unitId, fc, pollStart, pollQuantity);
            if (_bitCache.TryGetValue(key, out var entry))
            {
                var packed = entry.Value; // 내부 캐시 참조(읽기 전용으로만 사용할 것)
                timestamp = entry.Timestamp;

                int totalBits = pollQuantity;
                return TryExtract8BitsAsUInt(packed, bitOffsetFromPollStart, totalBits, out mask);
            }

            mask = 0;
            timestamp = default(DateTime);
            return false;
        }

        private bool TryGetBitCachePacked(byte unitId, ModbusFunctionCode fc, ushort start, ushort qty,
            out byte[] packed, out DateTime ts)
        {
            var key = new CacheKey(unitId, fc, start, qty);
            if (_bitCache.TryGetValue(key, out var entry))
            {
                // 방어적 복사(호출자가 packed를 바꿔도 캐시 오염 방지)
                packed = (byte[])entry.Value.Clone();
                ts = entry.Timestamp;
                return true;
            }
            packed = null;
            ts = default(DateTime);
            return false;
        }

        private bool TryGetRegCache(byte unitId, ModbusFunctionCode fc, ushort start, ushort qty, out ushort[] regs, out DateTime ts)
        {
            var key = new CacheKey(unitId, fc, start, qty);
            if (_regCache.TryGetValue(key, out var entry))
            {
                regs = (ushort[])entry.Value.Clone();
                ts = entry.Timestamp;
                return true;
            }
            regs = null;
            ts = default(DateTime);
            return false;
        }

        public bool TryPatchCachedCoilPacked(byte unitId, ushort pollStartAddress, ushort pollQuantity, ushort coilAddress, bool value)
        {
            if (coilAddress < pollStartAddress) return false;
            int bitIndex = coilAddress - pollStartAddress;
            if (bitIndex < 0 || bitIndex >= pollQuantity) return false;

            var key = new CacheKey(unitId, ModbusFunctionCode.ReadCoils, pollStartAddress, pollQuantity);
            if (!_bitCache.TryGetValue(key, out var entry)) return false;

            // copy-on-write (안전)
            var newPacked = (byte[])entry.Value.Clone();

            int byteIndex = bitIndex / 8;
            int bitInByte = bitIndex % 8;
            byte mask = (byte)(1 << bitInByte);

            if (value) newPacked[byteIndex] = (byte)(newPacked[byteIndex] | mask);
            else newPacked[byteIndex] = (byte)(newPacked[byteIndex] & ~mask);

            _bitCache[key] = CacheEntry<byte[]>.Now(newPacked);
            return true;
        }

        #endregion

        #region Write enqueue
        public void WriteSingleCoil(byte unitId, ushort address, bool value, Action<bool, Exception> completed = null)
        {
            _writeQueue.Enqueue(WriteRequest.SingleCoil(unitId, address, value, completed));
        }

        public void WriteSingleRegister(byte unitId, ushort address, ushort value, Action<bool, Exception> completed = null)
        {
            _writeQueue.Enqueue(WriteRequest.SingleRegister(unitId, address, value, completed));
        }

        private sealed class WriteRequest
        {
            public ModbusFunctionCode FunctionCode { get; }
            public byte UnitId { get; }
            public ushort Address { get; }
            public bool CoilValue { get; }
            public ushort RegisterValue { get; }
            public Action<bool, Exception> Completed { get; }

            private WriteRequest(ModbusFunctionCode fc, byte uid, ushort addr, bool coilVal, ushort regVal, Action<bool, Exception> completed)
            {
                FunctionCode = fc;
                UnitId = uid;
                Address = addr;
                CoilValue = coilVal;
                RegisterValue = regVal;
                Completed = completed;
            }

            public static WriteRequest SingleCoil(byte uid, ushort addr, bool val, Action<bool, Exception> completed)
                => new WriteRequest(ModbusFunctionCode.WriteSingleCoil, uid, addr, val, 0, completed);

            public static WriteRequest SingleRegister(byte uid, ushort addr, ushort val, Action<bool, Exception> completed)
                => new WriteRequest(ModbusFunctionCode.WriteSingleRegister, uid, addr, false, val, completed);
        }
        #endregion

        #region IO loop (1ms tick)
        private void IoLoop()
        {
            _nextTickMs = _sw.ElapsedMilliseconds;
            _lastLoopMs = _nextTickMs;

            while (!_stopRequested)
            {
                long nowMs = _sw.ElapsedMilliseconds;
                long gapMs = nowMs - _lastLoopMs;
                _lastLoopMs = nowMs;

                // ✅ 디버그 안전 모드: 긴 정지 감지 후 강제 리셋
                // 목적: half-read/버퍼에 남은 응답/Transaction mismatch 등 중간 상태를 통째로 버리고 재연결로 정합성 회복
                if (_debugSafeMode && _isConnected && gapMs > _debugGapResetMs)
                {
                    ForceResetForDebugGap(gapMs);
                    WaitNextTick();
                    continue;
                }

                if (!_isConnected)
                {
                    TryConnectWithRetry();
                    WaitNextTick();
                    continue;
                }

                try
                {
                    // Write 우선(직렬화)
                    ProcessWrites();

                    // Due된 Poll만 수행(직렬화)
                    ProcessPollReadsDueOnly();

                    WaitNextTick();
                }
                catch (Exception ex)
                {
                    // 종료 중이면 조용히 나감(불필요한 disconnect 로그/이벤트 최소화)
                    if (_stopRequested)
                        break;

                    HandleDisconnect(ex);
                }
            }

            // 종료 정리
            CloseSocket();
        }

        private void ForceResetForDebugGap(long gapMs)
        {
            try
            {
                _isConnected = false;
                CloseSocket();

                // 이벤트를 남기고 싶으면 Disconnected를 호출. 원치 않으면 주석 처리.
                SafeInvokeDisconnected(new IOException($"Debug gap reset. gapMs={gapMs}, threshold={_debugGapResetMs}"));
            }
            catch { }
        }

        private void WaitNextTick()
        {
            _nextTickMs += 1;

            while (!_stopRequested)
            {
                long now = _sw.ElapsedMilliseconds;
                long remaining = _nextTickMs - now;
                if (remaining <= 0) break;

                if (remaining <= 1) Thread.SpinWait(50);
                else Thread.Sleep(0);
            }
        }

        private void TryConnectWithRetry()
        {
            for (int attempt = 1; attempt <= _maxReconnectAttempts && !_stopRequested; attempt++)
            {
                try
                {
                    ConnectSocket();
                    _isConnected = true;
                    SafeInvokeConnected();
                    return;
                }
                catch
                {
                    CloseSocket();
                    if (_stopRequested) return;
                    Thread.Sleep(_reconnectDelayMs);
                }
            }

            _isConnected = false;
        }

        private void HandleDisconnect(Exception ex)
        {
            if (_isConnected)
            {
                _isConnected = false;
                CloseSocket();
                SafeInvokeDisconnected(ex);
            }
        }

        private void SafeInvokeConnected()
        {
            try { Connected?.Invoke(); } catch { }
        }

        private void SafeInvokeDisconnected(Exception ex)
        {
            try { Disconnected?.Invoke(ex); } catch { }
        }

        private void ConnectSocket()
        {
            _tcpClient = new TcpClient();
            _tcpClient.NoDelay = true;

            // ✅ KeepAlive ON
            if (_keepAliveEnabled)
            {
                try
                {
                    _tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                }
                catch
                {
                    // keepalive 설정 실패는 치명적이지 않으니 무시(환경/권한 이슈 대비)
                }
            }

            _tcpClient.ReceiveTimeout = _ioTimeoutMs;
            _tcpClient.SendTimeout = _ioTimeoutMs;

            _tcpClient.Connect(_host, _port);
            _stream = _tcpClient.GetStream();
        }

        private void CloseSocket()
        {
            try { _stream?.Close(); } catch { }
            try { _tcpClient?.Close(); } catch { }
            _stream = null;
            _tcpClient = null;
        }

        private void ProcessWrites()
        {
            while (_writeQueue.TryDequeue(out var wr))
            {
                if (!_isConnected) return;

                try
                {
                    switch (wr.FunctionCode)
                    {
                        case ModbusFunctionCode.WriteSingleCoil:
                            WriteSingleCoilInternal(wr.UnitId, wr.Address, wr.CoilValue);
                            wr.Completed?.Invoke(true, null);
                            break;

                        case ModbusFunctionCode.WriteSingleRegister:
                            WriteSingleRegisterInternal(wr.UnitId, wr.Address, wr.RegisterValue);
                            wr.Completed?.Invoke(true, null);
                            break;

                        default:
                            throw new NotSupportedException("Unsupported write function.");
                    }
                }
                catch (Exception ex)
                {
                    wr.Completed?.Invoke(false, ex);
                    throw;
                }
            }
        }

        private void ProcessPollReadsDueOnly()
        {
            PollJob[] snapshot;
            lock (_pollJobs) { snapshot = _pollJobs.ToArray(); }

            long nowMs = _sw.ElapsedMilliseconds;

            for (int i = 0; i < snapshot.Length; i++)
            {
                var job = snapshot[i];
                if (!_isConnected) return;

                if (nowMs < job.NextDueMs)
                    continue;

                // 다음 due 계산
                job.NextDueMs = nowMs + job.IntervalMs;

                // read 사이에도 write 즉시성 유지
                if (_writeQueue.Count > 0) ProcessWrites();

                switch (job.FunctionCode)
                {
                    case ModbusFunctionCode.ReadDiscreteInputs:
                    case ModbusFunctionCode.ReadCoils:
                        {
                            var packed = ReadBitsPackedInternal(job.UnitId, job.FunctionCode, job.StartAddress, job.Quantity);
                            _bitCache[new CacheKey(job.UnitId, job.FunctionCode, job.StartAddress, job.Quantity)] =
                                CacheEntry<byte[]>.Now(packed);
                        }
                        break;

                    case ModbusFunctionCode.ReadHoldingRegisters:
                        {
                            var regs = ReadRegistersInternal(job.UnitId, job.FunctionCode, job.StartAddress, job.Quantity);
                            _regCache[new CacheKey(job.UnitId, job.FunctionCode, job.StartAddress, job.Quantity)] =
                                CacheEntry<ushort[]>.Now(regs);
                        }
                        break;
                }
            }
        }
        #endregion

        #region Modbus TCP protocol
        private ushort NextTransactionId() { unchecked { return ++_transactionId; } }

        private bool[] ReadBitsInternal(byte unitId, ModbusFunctionCode fc, ushort startAddress, ushort quantity)
        {
            ushort tid = NextTransactionId();

            byte[] pdu = new byte[5];
            pdu[0] = (byte)fc;
            WriteUInt16BE(pdu, 1, startAddress);
            WriteUInt16BE(pdu, 3, quantity);

            SendRequest(tid, unitId, pdu);

            byte[] resp = ReceiveResponse(tid, unitId);
            ValidateFunction(resp, fc);

            int expectedByteCount = ((quantity - 1) / 8) + 1;
            int byteCount = resp[1];
            if (byteCount != expectedByteCount)
                throw new IOException("Unexpected bit byteCount.");

            bool[] bits = new bool[quantity];
            int dataOffset = 2;

            for (int i = 0; i < quantity; i++)
            {
                int byteIndex = i / 8;
                int bitIndex = i % 8;
                byte b = resp[dataOffset + byteIndex];
                bits[i] = ((b >> bitIndex) & 0x01) == 1;
            }

            return bits;
        }
        private byte[] ReadBitsPackedInternal(byte unitId, ModbusFunctionCode fc, ushort startAddress, ushort quantity)
        {
            if (fc != ModbusFunctionCode.ReadDiscreteInputs && fc != ModbusFunctionCode.ReadCoils)
                throw new ArgumentException("Invalid FC for bit read.");

            ushort tid = NextTransactionId();

            byte[] pdu = new byte[5];
            pdu[0] = (byte)fc;
            WriteUInt16BE(pdu, 1, startAddress);
            WriteUInt16BE(pdu, 3, quantity);

            SendRequest(tid, unitId, pdu);

            byte[] resp = ReceiveResponse(tid, unitId);
            ValidateFunction(resp, fc);

            int expectedByteCount = ((quantity - 1) / 8) + 1;
            int byteCount = resp[1];
            if (byteCount != expectedByteCount)
                throw new IOException("Unexpected bit byteCount.");

            // resp: [FC][ByteCount][Data...]
            byte[] packed = new byte[byteCount];
            Buffer.BlockCopy(resp, 2, packed, 0, byteCount);
            return packed;
        }
        private ushort[] ReadRegistersInternal(byte unitId, ModbusFunctionCode fc, ushort startAddress, ushort quantity)
        {
            ushort tid = NextTransactionId();

            byte[] pdu = new byte[5];
            pdu[0] = (byte)fc;
            WriteUInt16BE(pdu, 1, startAddress);
            WriteUInt16BE(pdu, 3, quantity);

            SendRequest(tid, unitId, pdu);

            byte[] resp = ReceiveResponse(tid, unitId);
            ValidateFunction(resp, fc);

            int byteCount = resp[1];
            if (byteCount != quantity * 2)
                throw new IOException("Unexpected register byteCount.");

            ushort[] regs = new ushort[quantity];
            int offset = 2;

            for (int i = 0; i < quantity; i++)
            {
                regs[i] = ReadUInt16BE(resp, offset);
                offset += 2;
            }

            return regs;
        }

        private void WriteSingleCoilInternal(byte unitId, ushort address, bool value)
        {
            ushort tid = NextTransactionId();

            byte[] pdu = new byte[5];
            pdu[0] = (byte)ModbusFunctionCode.WriteSingleCoil;
            WriteUInt16BE(pdu, 1, address);
            WriteUInt16BE(pdu, 3, value ? (ushort)0xFF00 : (ushort)0x0000);

            SendRequest(tid, unitId, pdu);

            byte[] resp = ReceiveResponse(tid, unitId);
            ValidateFunction(resp, ModbusFunctionCode.WriteSingleCoil);

            ushort echoedAddr = ReadUInt16BE(resp, 1);
            ushort echoedVal = ReadUInt16BE(resp, 3);
            ushort expected = value ? (ushort)0xFF00 : (ushort)0x0000;

            if (echoedAddr != address || echoedVal != expected)
                throw new IOException("WriteSingleCoil echo mismatch.");
        }

        private void WriteSingleRegisterInternal(byte unitId, ushort address, ushort value)
        {
            ushort tid = NextTransactionId();

            byte[] pdu = new byte[5];
            pdu[0] = (byte)ModbusFunctionCode.WriteSingleRegister;
            WriteUInt16BE(pdu, 1, address);
            WriteUInt16BE(pdu, 3, value);

            SendRequest(tid, unitId, pdu);

            byte[] resp = ReceiveResponse(tid, unitId);
            ValidateFunction(resp, ModbusFunctionCode.WriteSingleRegister);

            ushort echoedAddr = ReadUInt16BE(resp, 1);
            ushort echoedVal = ReadUInt16BE(resp, 3);

            if (echoedAddr != address || echoedVal != value)
                throw new IOException("WriteSingleRegister echo mismatch.");
        }

        private void SendRequest(ushort transactionId, byte unitId, byte[] pdu)
        {
            if (_stream == null) throw new InvalidOperationException("Not connected.");

            ushort len = (ushort)(1 + pdu.Length);

            byte[] mbap = new byte[7];
            WriteUInt16BE(mbap, 0, transactionId);
            WriteUInt16BE(mbap, 2, 0);
            WriteUInt16BE(mbap, 4, len);
            mbap[6] = unitId;

            _stream.Write(mbap, 0, mbap.Length);
            _stream.Write(pdu, 0, pdu.Length);
        }

        private byte[] ReceiveResponse(ushort expectedTid, byte expectedUnitId)
        {
            if (_stream == null) throw new InvalidOperationException("Not connected.");

            byte[] mbap = ReadExact(_stream, 7);

            ushort tid = ReadUInt16BE(mbap, 0);
            ushort pid = ReadUInt16BE(mbap, 2);
            ushort len = ReadUInt16BE(mbap, 4);
            byte uid = mbap[6];

            if (pid != 0) throw new IOException("Invalid protocol id.");
            if (tid != expectedTid) throw new IOException("TransactionId mismatch.");
            if (uid != expectedUnitId) throw new IOException("UnitId mismatch.");
            if (len < 2) throw new IOException("Invalid length.");

            int pduLen = len - 1;
            return ReadExact(_stream, pduLen);
        }

        private void ValidateFunction(byte[] pdu, ModbusFunctionCode expectedFc)
        {
            byte fc = pdu[0];

            if ((fc & 0x80) != 0)
            {
                byte exCode = pdu.Length > 1 ? pdu[1] : (byte)0xFF;
                throw new IOException($"Modbus exception. fc=0x{fc:X2}, ex=0x{exCode:X2}");
            }

            if (fc != (byte)expectedFc)
                throw new IOException("FunctionCode mismatch.");
        }

        private static byte[] ReadExact(NetworkStream stream, int length)
        {
            byte[] buffer = new byte[length];
            int offset = 0;

            while (offset < length)
            {
                int read = stream.Read(buffer, offset, length - offset);
                if (read <= 0) throw new IOException("Remote closed.");
                offset += read;
            }

            return buffer;
        }

        private static void WriteUInt16BE(byte[] buffer, int offset, ushort value)
        {
            buffer[offset] = (byte)(value >> 8);
            buffer[offset + 1] = (byte)(value & 0xFF);
        }

        private static ushort ReadUInt16BE(byte[] buffer, int offset)
        {
            return (ushort)((buffer[offset] << 8) | buffer[offset + 1]);
        }
        #endregion

        #region Cache key/entry (NO readonly struct)
        private sealed class CacheKey : IEquatable<CacheKey>
        {
            public byte UnitId { get; }
            public ModbusFunctionCode FunctionCode { get; }
            public ushort StartAddress { get; }
            public ushort Quantity { get; }

            public CacheKey(byte unitId, ModbusFunctionCode fc, ushort start, ushort qty)
            {
                UnitId = unitId;
                FunctionCode = fc;
                StartAddress = start;
                Quantity = qty;
            }

            public bool Equals(CacheKey other)
            {
                if (ReferenceEquals(other, null)) return false;
                return UnitId == other.UnitId
                    && FunctionCode == other.FunctionCode
                    && StartAddress == other.StartAddress
                    && Quantity == other.Quantity;
            }

            public override bool Equals(object obj) => Equals(obj as CacheKey);

            public override int GetHashCode()
            {
                unchecked
                {
                    int h = UnitId;
                    h = (h * 397) ^ (int)FunctionCode;
                    h = (h * 397) ^ StartAddress.GetHashCode();
                    h = (h * 397) ^ Quantity.GetHashCode();
                    return h;
                }
            }
        }

        private sealed class CacheEntry<T>
        {
            public T Value { get; }
            public DateTime Timestamp { get; }

            public CacheEntry(T value, DateTime timestamp)
            {
                Value = value;
                Timestamp = timestamp;
            }

            public static CacheEntry<T> Now(T value) => new CacheEntry<T>(value, DateTime.UtcNow);
        }
        #endregion

        #region <ETC>
        private static bool TryExtract8BitsAsUInt(byte[] packed, int bitOffset, int totalBits, out uint mask)
        {
            mask = 0;
            if (bitOffset < 0 || bitOffset + 7 >= totalBits) return false;

            for (int i = 0; i < 8; i++)
            {
                int bitIndex = bitOffset + i;
                int byteIndex = bitIndex / 8;
                int bitInByte = bitIndex % 8;

                if (((packed[byteIndex] >> bitInByte) & 0x01) != 0)
                    mask |= (1u << i);
            }
            return true;
        }
        #endregion </ETC>
    }
    #endregion
}