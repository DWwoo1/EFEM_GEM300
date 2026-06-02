using System;
using System.IO;

using AnalogIO_;

using FrameOfSystem3.Functional;
using Define.DefineEnumProject.Modbus;
using FrameOfSystem3.Controller.ModbusShared;

namespace FrameOfSystem3.Controller.AnalogIO
{

    public class NewCrevisModbusAnalogIOController : AnalogIOController
    {
        private ModbusTcpClient _client;
        private ModbusClientKey _clientKey;

        private IPollHandle _aiPollHandle;
        private IPollHandle _aoPollHandle;

        // Crevis: 아날로그는 1채널 = 1 word(ushort)
        private const int _countOfChannelPerModule = 1;

        private byte _unitId;

        private ushort _aiStart;
        private ushort _aiQty;
        private ushort _aoStart;
        private ushort _aoQty;

        #region Analog INI (independent)
        private const string SECTION_NAME = "MODBUS_ANALOG_CONFIG";
        private const string INI_NAME = "ModbusAnalog.ini";

        private const string KEY_CLIENT_ID = "CLIENT_ID";
        private const string KEY_PROTOCOL_TYPE = "PROTOCOL_TYPE(TCP/SERIAL)";
        private const string KEY_SERVER_IP = "MODBUS_SERVER_IP";
        private const string KEY_SERVER_PORT = "MODBUS_SERVER_PORT";

        private const string KEY_AI_COUNT = "ANALOG_INPUT_CHANNEL_COUNT";
        private const string KEY_AI_START = "ANALOG_INPUT_CHANNEL_START_ADDRESS";

        private const string KEY_AO_COUNT = "ANALOG_OUTPUT_CHANNEL_COUNT";
        private const string KEY_AO_START = "ANALOG_OUTPUT_CHANNEL_START_ADDRESS";

        private const string KEY_POLL_INTERVAL_MS = "ANALOG_POLL_INTERVAL_MS"; // default 100

        private const string KEY_KEEPALIVE_ENABLE = "KEEPALIVE_ENABLE";
        private const string KEY_DEBUG_SAFE_MODE = "DEBUG_SAFE_MODE";
        private const string KEY_DEBUG_GAP_RESET_MS = "DEBUG_GAP_RESET_MS";

        private int _clientId;
        private EN_MODBUS_SERVER_PROTOCOL _protocolType;
        private string _serverIp;
        private int _serverPort;

        private int _analogInputModuleCount;
        private int _analogInputModuleStartingAddress;

        private int _analogOutputModuleCount;
        private int _analogOutputModuleStartingAddress;

        private int _pollIntervalMs; // default 100

        private int _keepAliveEnable;   // 0/1
        private int _debugSafeMode;     // 0/1
        private int _debugGapResetMs;   // ms
        #endregion

        public override bool InitController()
        {
            ReadConfigFile();

            if (_protocolType != EN_MODBUS_SERVER_PROTOCOL.TCP)
                return false;

            _unitId = (byte)_clientId;

            _aiStart = checked((ushort)_analogInputModuleStartingAddress);
            _aiQty = checked((ushort)(_analogInputModuleCount * _countOfChannelPerModule));

            _aoStart = checked((ushort)_analogOutputModuleStartingAddress);
            _aoQty = checked((ushort)(_analogOutputModuleCount * _countOfChannelPerModule));

            _clientKey = new ModbusClientKey(
                _serverIp, _serverPort, _protocolType,
                ioTimeoutMs: 500,
                reconnectDelayMs: 1000,
                maxReconnectAttempts: 3,
                keepAliveEnabled: _keepAliveEnable != 0,
                debugSafeMode: _debugSafeMode != 0,
                debugGapResetMs: _debugGapResetMs);

            _client = ModbusTcpClientRegistry.Instance.Acquire(_clientKey);

            // AI/AO 둘 다 Holding Register(FC3)로 모니터링
            if (_analogInputModuleCount > 0)
                _aiPollHandle = _client.AddHoldingRegistersPoll(_unitId, _aiStart, _aiQty, _pollIntervalMs);

            if (_analogOutputModuleCount > 0)
                _aoPollHandle = _client.AddHoldingRegistersPoll(_unitId, _aoStart, _aoQty, _pollIntervalMs);

            return true;
        }

        public override void ExitController()
        {
            try { _aiPollHandle?.Dispose(); } catch { }
            try { _aoPollHandle?.Dispose(); } catch { }

            _aiPollHandle = null;
            _aoPollHandle = null;

            if (_client != null)
            {
                _client = null;
                ModbusTcpClientRegistry.Instance.Release(_clientKey);
            }
        }

        public override int GetCountOfInputModule() => _analogInputModuleCount;
        public override int GetCountOfOutputModule() => _analogOutputModuleCount;

        public override int GetCountOfInputChannel(ref int nInputModule) => _countOfChannelPerModule;
        public override int GetCountOfOutputChannel(ref int nOutputMoudle) => _countOfChannelPerModule;

        public override void WriteOutput(ref int nOutputChannel, ref int nCount)
        {
            if (_client == null) return;

            // unsigned로: 상위 int -> ushort로 캐스팅(0~65535만 유효)
            ushort value = checked((ushort)nCount);
            ushort addr = checked((ushort)(_analogOutputModuleStartingAddress + nOutputChannel));

            _client.WriteSingleRegister(_unitId, addr, value);
        }

        public override void ReadInputAll(ref int nInputModule, ref int nCountOfChannel, ref int[] arCount)
        {
            nCountOfChannel = _countOfChannelPerModule;
            if (_client == null) { arCount[0] = 0; return; }

            ushort addr = checked((ushort)(_analogInputModuleStartingAddress + nInputModule));

            // 여기서 “한 채널만” 즉시 읽어오는 게 아니라, Poll 캐시에서 읽기
            // 폴링을 전체로 등록했기 때문에 offset 계산
            if (!_client.TryGetCachedHoldingRegisters(_unitId, _aiStart, _aiQty, out var regs, out _))
            {
                arCount[0] = 0;
                return;
            }

            int offset = addr - _aiStart;
            if (offset < 0 || offset >= regs.Length) { arCount[0] = 0; return; }

            arCount[0] = (int)regs[offset]; // unsigned -> int
        }

        public override void ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel, ref int[] arCount)
        {
            nCountOfChannel = _countOfChannelPerModule;
            if (_client == null) { arCount[0] = 0; return; }

            ushort addr = checked((ushort)(_analogOutputModuleStartingAddress + nOutputModule));

            if (!_client.TryGetCachedHoldingRegisters(_unitId, _aoStart, _aoQty, out var regs, out _))
            {
                arCount[0] = 0;
                return;
            }

            int offset = addr - _aoStart;
            if (offset < 0 || offset >= regs.Length) { arCount[0] = 0; return; }

            arCount[0] = (int)regs[offset]; // unsigned -> int
        }

        // 아래 ListTable 관련은 기존처럼 NotImplemented 유지
        public override void SetOutputListTable(ref int nOutputChannel, ref int nCountOfLoop, ref int nSizeOfPattern, ref int[] arPattern) { throw new NotImplementedException(); }
        public override void SetOutputListTableInterval(ref int nOutputChannel, ref double dInterval) { throw new NotImplementedException(); }
        public override void StartOutputListTable(ref int[] arChannel, ref int nSize) { throw new NotImplementedException(); }
        public override void StopOutputListTable(ref int[] arChannel, ref int nSize) { throw new NotImplementedException(); }
        public override void ResetOutputListTable(ref int nOutputChannel) { throw new NotImplementedException(); }
        public override void GetOutputListTable(ref int nOutputChannel, ref int nLoopCount, ref int nPatternSize, ref int[] arPattern) { throw new NotImplementedException(); }
        public override void GetOutputListTableInterval(ref int nOutputChannel, ref double dblInterval) { throw new NotImplementedException(); }
        public override void GetOutputListTableStatus(ref int nOutputChannel, ref int nPatternIndex, ref int nCountOfLoop, ref uint uInBusy) { throw new NotImplementedException(); }

        private void ReadConfigFile()
        {
            string path = string.Format(@"{0}\ModbusConfig", Define.DefineConstant.FilePath.FILEPATH_EXE);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string fullName = Path.Combine(path, INI_NAME);

            IniControl ini = new IniControl(fullName);
            bool save = false;

            _clientId = ini.GetInt(SECTION_NAME, KEY_CLIENT_ID, -1);
            if (_clientId < 0) { _clientId = 1; save = true; }

            string protocolType = ini.GetString(SECTION_NAME, KEY_PROTOCOL_TYPE, "TCP");
            if (!Enum.TryParse(protocolType, out _protocolType)) { _protocolType = EN_MODBUS_SERVER_PROTOCOL.TCP; save = true; }

            _serverIp = ini.GetString(SECTION_NAME, KEY_SERVER_IP, "");
            if (string.IsNullOrWhiteSpace(_serverIp)) { _serverIp = "127.0.0.1"; save = true; }

            _serverPort = ini.GetInt(SECTION_NAME, KEY_SERVER_PORT, -1);
            if (_serverPort <= 0) { _serverPort = 502; save = true; }

            _analogInputModuleCount = ini.GetInt(SECTION_NAME, KEY_AI_COUNT, -1);
            if (_analogInputModuleCount < 0) { _analogInputModuleCount = 1; save = true; }

            _analogInputModuleStartingAddress = ini.GetInt(SECTION_NAME, KEY_AI_START, -1);
            if (_analogInputModuleStartingAddress < 0) { _analogInputModuleStartingAddress = 0; save = true; }

            _analogOutputModuleCount = ini.GetInt(SECTION_NAME, KEY_AO_COUNT, -1);
            if (_analogOutputModuleCount < 0) { _analogOutputModuleCount = 0; save = true; }

            _analogOutputModuleStartingAddress = ini.GetInt(SECTION_NAME, KEY_AO_START, -1);
            if (_analogOutputModuleStartingAddress < 0) { _analogOutputModuleStartingAddress = 0; save = true; }

            _pollIntervalMs = ini.GetInt(SECTION_NAME, KEY_POLL_INTERVAL_MS, -1);
            if (_pollIntervalMs <= 0) { _pollIntervalMs = 100; save = true; }

            _keepAliveEnable = ini.GetInt(SECTION_NAME, KEY_KEEPALIVE_ENABLE, 1); // 기본 ON
            _debugSafeMode = ini.GetInt(SECTION_NAME, KEY_DEBUG_SAFE_MODE, 0);   // 기본 OFF
            _debugGapResetMs = ini.GetInt(SECTION_NAME, KEY_DEBUG_GAP_RESET_MS, 2000);
            if (_debugGapResetMs <= 0) _debugGapResetMs = 2000;

            if (save) WriteConfigFile(fullName);
        }

        private void WriteConfigFile(string fullName)
        {
            IniControl ini = new IniControl(fullName);

            ini.WriteInt(SECTION_NAME, KEY_CLIENT_ID, _clientId);
            ini.WriteString(SECTION_NAME, KEY_PROTOCOL_TYPE, _protocolType.ToString());
            ini.WriteString(SECTION_NAME, KEY_SERVER_IP, _serverIp);
            ini.WriteInt(SECTION_NAME, KEY_SERVER_PORT, _serverPort);

            ini.WriteInt(SECTION_NAME, KEY_AI_COUNT, _analogInputModuleCount);
            ini.WriteInt(SECTION_NAME, KEY_AI_START, _analogInputModuleStartingAddress);

            ini.WriteInt(SECTION_NAME, KEY_AO_COUNT, _analogOutputModuleCount);
            ini.WriteInt(SECTION_NAME, KEY_AO_START, _analogOutputModuleStartingAddress);

            ini.WriteInt(SECTION_NAME, KEY_POLL_INTERVAL_MS, _pollIntervalMs);
        }
    }
}