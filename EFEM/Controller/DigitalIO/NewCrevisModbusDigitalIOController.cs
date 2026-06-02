using System;
using System.IO;

using DigitalIO_;

using FrameOfSystem3.Functional;
using Define.DefineEnumProject.Modbus;
using FrameOfSystem3.Controller.ModbusShared;

namespace FrameOfSystem3.Controller.DigitalIO
{
    public class NewCrevisModbusDigitalIOController : DigitalIOController
    {
        private ModbusTcpClient _client;
        private ModbusClientKey _clientKey;

        private IPollHandle _diPollHandle;
        private IPollHandle _doPollHandle;

        private const int _countOfChannelPerModule = 8;

        private byte _unitId;

        private ushort _diStart;
        private ushort _diQty;
        private ushort _doStart;
        private ushort _doQty;

        #region Digital INI (independent)
        private const string SECTION_NAME = "MODBUS_DIGITAL_CONFIG";
        private const string INI_NAME = "ModbusDigital.ini";

        private const string KEY_CLIENT_ID = "CLIENT_ID";
        private const string KEY_PROTOCOL_TYPE = "PROTOCOL_TYPE(TCP/SERIAL)";
        private const string KEY_SERVER_IP = "MODBUS_SERVER_IP";
        private const string KEY_SERVER_PORT = "MODBUS_SERVER_PORT";

        private const string KEY_DI_COUNT = "DIGITAL_INPUT_CHANNEL_COUNT";
        private const string KEY_DI_START = "DIGITAL_INPUT_CHANNEL_START_ADDRESS";

        private const string KEY_DO_COUNT = "DIGITAL_OUTPUT_CHANNEL_COUNT";
        private const string KEY_DO_START = "DIGITAL_OUTPUT_CHANNEL_START_ADDRESS";

        private const string KEY_POLL_INTERVAL_MS = "DIGITAL_POLL_INTERVAL_MS"; // default 10

        private const string KEY_KEEPALIVE_ENABLE = "KEEPALIVE_ENABLE";
        private const string KEY_DEBUG_SAFE_MODE = "DEBUG_SAFE_MODE";
        private const string KEY_DEBUG_GAP_RESET_MS = "DEBUG_GAP_RESET_MS";

        private int _clientId;
        private EN_MODBUS_SERVER_PROTOCOL _protocolType;
        private string _serverIp;
        private int _serverPort;

        private int _digitalInputModuleCount;
        private int _digitalInputModuleStartingAddress;

        private int _digitalOutputModuleCount;
        private int _digitalOutputModuleStartingAddress;

        private int _pollIntervalMs; // default 10

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

            _diStart = checked((ushort)_digitalInputModuleStartingAddress);
            _diQty = checked((ushort)(_digitalInputModuleCount * _countOfChannelPerModule));

            _doStart = checked((ushort)_digitalOutputModuleStartingAddress);
            _doQty = checked((ushort)(_digitalOutputModuleCount * _countOfChannelPerModule));

            _clientKey = new ModbusClientKey(
                _serverIp, _serverPort, _protocolType,
                ioTimeoutMs: 500,
                reconnectDelayMs: 1000,
                maxReconnectAttempts: 3,
                keepAliveEnabled: _keepAliveEnable != 0,
                debugSafeMode: _debugSafeMode != 0,
                debugGapResetMs: _debugGapResetMs);

            _client = ModbusTcpClientRegistry.Instance.Acquire(_clientKey);

            if (_digitalInputModuleCount > 0)
                _diPollHandle = _client.AddDiscreteInputsPoll(_unitId, _diStart, _diQty, _pollIntervalMs);

            if (_digitalOutputModuleCount > 0)
                _doPollHandle = _client.AddCoilsPoll(_unitId, _doStart, _doQty, _pollIntervalMs);

            return true;
        }

        public override void ExitController()
        {
            try { _diPollHandle?.Dispose(); } catch { }
            try { _doPollHandle?.Dispose(); } catch { }

            _diPollHandle = null;
            _doPollHandle = null;

            if (_client != null)
            {
                _client = null;
                ModbusTcpClientRegistry.Instance.Release(_clientKey);
            }
        }

        public override int GetCountOfInputModule() => _digitalInputModuleCount;
        public override int GetCountOfOutputModule() => _digitalOutputModuleCount;

        public override int GetCountOfInputChannel(ref int nInputModule) => _countOfChannelPerModule;
        public override int GetCountOfOutputChannel(ref int nOutputMoudle) => _countOfChannelPerModule;

        public override void WriteOutput(ref int nOutputChannel, ref bool bPulse)
        {
            if (_client == null) return;

            ushort coilAddr = checked((ushort)(_digitalOutputModuleStartingAddress + nOutputChannel));
            bool value = bPulse;

            _client.WriteSingleCoil(_unitId, coilAddr, value, (ok, ex) =>
            {
                if (!ok) return;
                _client.TryPatchCachedCoilPacked(_unitId, _doStart, _doQty, coilAddr, value);
            });
        }

        public override uint ReadInputAll(ref int nInputModule, ref int nCountOfChannel)
        {
            nCountOfChannel = 8;
            if (_client == null) return 0;
            if (nInputModule < 0 || nInputModule >= _digitalInputModuleCount) return 0;

            int bitOffset = nInputModule * 8; // pollStart 기준 offset
            if (_client.TryGetCachedBitsMask8(
                    _unitId,
                    ModbusFunctionCode.ReadDiscreteInputs,
                    _diStart,
                    _diQty,
                    bitOffset,
                    out uint mask,
                    out _))
            {
                return mask;
            }
            return 0;
        }


        public override uint ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel)
        {
            nCountOfChannel = 8;
            if (_client == null) return 0;
            if (nOutputModule < 0 || nOutputModule >= _digitalOutputModuleCount) return 0;

            int bitOffset = nOutputModule * 8;
            if (_client.TryGetCachedBitsMask8(
                    _unitId,
                    ModbusFunctionCode.ReadCoils,
                    _doStart,
                    _doQty,
                    bitOffset,
                    out uint mask,
                    out _))
            {
                return mask;
            }
            return 0;
        }


        private static uint Pack8Bits(bool[] all, int baseIndex)
        {
            uint mask = 0;
            for (int ch = 0; ch < 8; ch++)
            {
                if (all[baseIndex + ch])
                    mask |= (1u << ch);
            }
            return mask;
        }

        private void ReadConfigFile()
        {
            string path = string.Format(@"{0}\ModbusConfig", Define.DefineConstant.FilePath.FILEPATH_EXE);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string fullName = Path.Combine(path, INI_NAME);

            if (!File.Exists(fullName))
            {
                _clientId = 1;
                _protocolType = EN_MODBUS_SERVER_PROTOCOL.TCP;
                _serverIp = "127.0.0.1";
                _serverPort = 502;

                _digitalInputModuleCount = 1;
                _digitalInputModuleStartingAddress = 0;
                _digitalOutputModuleCount = 1;
                _digitalOutputModuleStartingAddress = 0;

                _pollIntervalMs = 10;

                WriteConfigFile(fullName);
                return;
            }

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

            _digitalInputModuleCount = ini.GetInt(SECTION_NAME, KEY_DI_COUNT, -1);
            if (_digitalInputModuleCount < 0) { _digitalInputModuleCount = 1; save = true; }

            _digitalInputModuleStartingAddress = ini.GetInt(SECTION_NAME, KEY_DI_START, -1);
            if (_digitalInputModuleStartingAddress < 0) { _digitalInputModuleStartingAddress = 0; save = true; }

            _digitalOutputModuleCount = ini.GetInt(SECTION_NAME, KEY_DO_COUNT, -1);
            if (_digitalOutputModuleCount < 0) { _digitalOutputModuleCount = 1; save = true; }

            _digitalOutputModuleStartingAddress = ini.GetInt(SECTION_NAME, KEY_DO_START, -1);
            if (_digitalOutputModuleStartingAddress < 0) { _digitalOutputModuleStartingAddress = 0; save = true; }

            _pollIntervalMs = ini.GetInt(SECTION_NAME, KEY_POLL_INTERVAL_MS, -1);
            if (_pollIntervalMs <= 0) { _pollIntervalMs = 10; save = true; }

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

            ini.WriteInt(SECTION_NAME, KEY_DI_COUNT, _digitalInputModuleCount);
            ini.WriteInt(SECTION_NAME, KEY_DI_START, _digitalInputModuleStartingAddress);

            ini.WriteInt(SECTION_NAME, KEY_DO_COUNT, _digitalOutputModuleCount);
            ini.WriteInt(SECTION_NAME, KEY_DO_START, _digitalOutputModuleStartingAddress);

            ini.WriteInt(SECTION_NAME, KEY_POLL_INTERVAL_MS, _pollIntervalMs);
        }
    }
}