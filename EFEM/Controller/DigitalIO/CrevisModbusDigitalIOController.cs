using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using DigitalIO_;

using FrameOfSystem3.Functional;
using Define.DefineEnumProject.Modbus;
using FrameOfSystem3.ExternalDevice.Socket;
using FrameOfSystem3.ExternalDevice.Modbus;

namespace FrameOfSystem3.Controller.DigitalIO
{
    public class CrevisModbusDigitalIOController : DigitalIOController
    {
        #region <Constructors>
        #endregion </Constructors>

        #region <Fields>
        private ModbusTCPClient _modbusClient = null;

        // byte 단위이며, 크레비스는 1채널 당 1바이트 = 8 bit이다.
        private const int _countOfChannelPerModule = 8;

        #region <Config>
        private const string SECTION_NAME = "MODBUS_CONFIG";
        private const string KEY_CLIENT_ID = "CLIENT_ID";
        private const string KEY_PROTOCOL_TYPE = "PROTOCOL_TYPE(TCP/SERIAL)";

        private const string KEY_DIGITAL_INPUT_CHANNEL_COUNT = "DIGITAL_INPUT_CHANNEL_COUNT";
        private const string KEY_DIGITAL_INPUT_CHANNEL_START_ADDRESS = "DIGITAL_INPUT_CHANNEL_START_ADDRESS";

        private const string KEY_DIGITAL_OUTPUT_CHANNEL_COUNT = "DIGITAL_OUTPUT_CHANNEL_COUNT";
        private const string KEY_DIGITAL_OUTPUT_CHANNEL_START_ADDRESS = "DIGITAL_OUTPUT_CHANNEL_START_ADDRESS";

        private int _clientId;
        private EN_MODBUS_SERVER_PROTOCOL _protocolType;
        private int _digitalInputModuleCount;
        private int _digitalInputModuleStartingAddress;
        private int _digitalOutputModuleCount;
        private int _digitalOutputModuleStartingAddress;
        #endregion </Config>

        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>
        public override bool InitController()
        {
            ReadConfigFile();

            _modbusClient = ModbusTCPClient.GetInstance((int)Define.DefineEnumProject.Socket.EN_SOCKET_INDEX.MODBUS);

            ModbusTCPClientDevice client
                = new ModbusTCPClientDevice((byte)_clientId, EN_MODBUS_SERVER_PROTOCOL.TCP);
            _modbusClient.AddDevice(client);
            if (_digitalInputModuleCount > 0)
            {
                int count = _digitalInputModuleCount * _countOfChannelPerModule;
                _modbusClient.AddMornitorInputStatus(_clientId,
                    _digitalInputModuleStartingAddress,
                    count);
            }

            if (_digitalOutputModuleCount > 0)
            {
                int count = _digitalOutputModuleCount * _countOfChannelPerModule;
                _modbusClient.AddMornitorCoilStatus(_clientId,
                    _digitalOutputModuleStartingAddress,
                    count);
            }

            return _modbusClient.Init();
        }

        public override void ExitController()
        {
            _modbusClient.Close();
        }

        public override int GetCountOfInputModule()
        {
            return _digitalInputModuleCount;
        }

        public override int GetCountOfOutputModule()
        {
            return _digitalOutputModuleCount;
        }

        public override int GetCountOfInputChannel(ref int nInputModule)
        {
            return _countOfChannelPerModule;
        }

        public override int GetCountOfOutputChannel(ref int nOutputMoudle)
        {
            return _countOfChannelPerModule;
        }

        public override void WriteOutput(ref int nOutputChannel, ref bool bPulse)
        {
            _modbusClient.SetCoilStatus(_clientId, _digitalOutputModuleStartingAddress + nOutputChannel, bPulse);    
        }

        public override uint ReadInputAll(ref int nInputModule, ref int nCountOfChannel)
        {
            return _modbusClient.GetInputStates(_clientId, nInputModule);
        }

        public override uint ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel)
        {
            return _modbusClient.GetOutputStates(_clientId, nOutputModule);
        }

        private void ReadConfigFile()
        {
            string path = string.Format(@"{0}\ModbusConfig", Define.DefineConstant.FilePath.FILEPATH_EXE);
            if (false == Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fullName = string.Format(@"{0}\Modbus.ini", path);
            if (false == File.Exists(fullName))
            {
                WriteConfigFile();
                return;
            }

            IniControl ini = new IniControl(fullName);
            bool saveFlag = false;

            _clientId = ini.GetInt(SECTION_NAME, KEY_CLIENT_ID, -1);
            if (_clientId < 0)
            {
                saveFlag = true;
                _clientId = 1;
            }

            string protocolType = ini.GetString(SECTION_NAME, KEY_PROTOCOL_TYPE, "TCP");
            if (false == Enum.TryParse(protocolType, out _protocolType))
            {
                saveFlag = true;
                _protocolType = EN_MODBUS_SERVER_PROTOCOL.TCP;
            }

            _digitalInputModuleCount = ini.GetInt(SECTION_NAME, KEY_DIGITAL_INPUT_CHANNEL_COUNT, -1);
            if (_digitalInputModuleCount < 0)
            {
                saveFlag = true;
                _digitalInputModuleCount = 1;
            }

            _digitalInputModuleStartingAddress = ini.GetInt(SECTION_NAME, KEY_DIGITAL_INPUT_CHANNEL_START_ADDRESS, -1);
            if (_digitalInputModuleStartingAddress < 0)
            {
                saveFlag = true;
                _digitalInputModuleStartingAddress = 0;
            }

            _digitalOutputModuleCount = ini.GetInt(SECTION_NAME, KEY_DIGITAL_OUTPUT_CHANNEL_COUNT, -1);
            if (_digitalOutputModuleCount < 0)
            {
                saveFlag = true;
                _digitalOutputModuleCount = 1;
            }

            _digitalOutputModuleStartingAddress = ini.GetInt(SECTION_NAME, KEY_DIGITAL_OUTPUT_CHANNEL_START_ADDRESS, -1);
            if (_digitalOutputModuleStartingAddress < 0)
            {
                saveFlag = true;
                _digitalOutputModuleStartingAddress = 0;
            }

            if (saveFlag)
            {
                WriteConfigFile();
            }
        }

        private void WriteConfigFile()
        {
            string path = string.Format(@"{0}\ModbusConfig", Define.DefineConstant.FilePath.FILEPATH_EXE);
            if (false == Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fullName = string.Format(@"{0}\Modbus.ini", path);
            IniControl ini = new IniControl(fullName);

            ini.WriteInt(SECTION_NAME, KEY_CLIENT_ID, _clientId);
            ini.WriteString(SECTION_NAME, KEY_PROTOCOL_TYPE, _protocolType.ToString());

            ini.WriteInt(SECTION_NAME, KEY_DIGITAL_INPUT_CHANNEL_COUNT, _digitalInputModuleCount);
            ini.WriteInt(SECTION_NAME, KEY_DIGITAL_INPUT_CHANNEL_START_ADDRESS, _digitalInputModuleStartingAddress);

            ini.WriteInt(SECTION_NAME, KEY_DIGITAL_OUTPUT_CHANNEL_COUNT, _digitalOutputModuleCount);
            ini.WriteInt(SECTION_NAME, KEY_DIGITAL_OUTPUT_CHANNEL_START_ADDRESS, _digitalOutputModuleStartingAddress);

        }
        #endregion </Methods>

    }
}