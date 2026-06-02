using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using AnalogIO_;

using FrameOfSystem3.Functional;
using Define.DefineEnumProject.Modbus;
using FrameOfSystem3.ExternalDevice.Socket;
using FrameOfSystem3.ExternalDevice.Modbus;

namespace FrameOfSystem3.Controller.AnalogIO
{
    public class CrevisModbusAnalogIOController : AnalogIOController
    {
        #region <Constructors>
        //public ModbusAnalogIOController(int index)
        //{
        //}
        #endregion </Constructors>

        #region <Fields>
        private ModbusTCPClient _modbusClient = null;

        // word 단위이며, 크레비스는 1채널 당 2바이트 = 1 word이다.
        private const int _countOfChannelPerModule = 1;

        #region <Config>
        private const string SECTION_NAME = "MODBUS_CONFIG";
        private const string KEY_CLIENT_ID = "CLIENT_ID";
        private const string KEY_PROTOCOL_TYPE = "PROTOCOL_TYPE(TCP/SERIAL)";

        private const string KEY_ANALOG_INPUT_CHANNEL_COUNT = "ANALOG_INPUT_CHANNEL_COUNT";
        private const string KEY_ANALOG_INPUT_CHANNEL_START_ADDRESS = "ANALOG_INPUT_CHANNEL_START_ADDRESS";

        private const string KEY_ANALOG_OUTPUT_CHANNEL_COUNT = "ANALOG_OUTPUT_CHANNEL_COUNT";
        private const string KEY_ANALOG_OUTPUT_CHANNEL_START_ADDRESS = "ANALOG_OUTPUT_CHANNEL_START_ADDRESS";

        private int _clientId;
        private EN_MODBUS_SERVER_PROTOCOL _protocolType;
        private int _analogInputModuleCount;
        private int _analogInputModuleStartingAddress;
        private int _analogOutputModuleCount;
        private int _analogOutputModuleStartingAddress;
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
            if (_analogInputModuleCount > 0)
            {
                int count = _analogInputModuleCount * _countOfChannelPerModule;
                _modbusClient.AddMornitorHoldingRegister(_clientId,
                    _analogInputModuleStartingAddress,
                    count);
            }

            if (_analogOutputModuleCount > 0)
            {
                int count = _analogOutputModuleCount * _countOfChannelPerModule;
                _modbusClient.AddMornitorHoldingRegister(_clientId,
                    _analogOutputModuleStartingAddress,
                    count);
            }

            _modbusClient.Init();
            
            return true;
        }

        public override void ExitController()
        {
            _modbusClient.Close();
        }

        public override int GetCountOfInputModule()
        {
            return _analogInputModuleCount;
        }

        public override int GetCountOfOutputModule()
        {
            return _analogOutputModuleCount;
        }

        public override int GetCountOfInputChannel(ref int nInputModule)
        {
            return _countOfChannelPerModule;
        }

        public override int GetCountOfOutputChannel(ref int nOutputMoudle)
        {
            return _countOfChannelPerModule;
        }

        public override void WriteOutput(ref int nOutputChannel, ref int nCount)
        {
            _modbusClient.SetHoldingRegister(_clientId, _analogOutputModuleStartingAddress + nOutputChannel, nCount);
            //throw new NotImplementedException();
            //_modbusClient.SetCoilStatus(_clientId, nOutputChannel, bPulse);

        }

        public override void ReadInputAll(ref int nInputModule, ref int nCountOfChannel, ref int[] arCount)
        {
            arCount[0] = _modbusClient.GetushortHoldingRegister(_clientId, _analogInputModuleStartingAddress + nInputModule);
            //for (int i = 0; i < _analogInputModuleCount; ++i)
            //{
            //    //arCount[i] = _modbusClient.GetushortHoldingRegister(_clientId, i);
            //    arCount[i] = _modbusClient.GetushortInputRegister(_clientId, i);
            //}
                
            //throw new NotImplementedException();
            //return _modbusClient.GetInputStates(_clientId, nInputModule);
        }

        public override void ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel, ref int[] arCount)
        {
            arCount[0] = _modbusClient.GetushortHoldingRegister(_clientId, _analogOutputModuleStartingAddress + nOutputModule);
            //throw new NotImplementedException();
            //return _modbusClient.GetOutputStates(_clientId, nOutputModule);
        }

        public override void SetOutputListTable(ref int nOutputChannel, ref int nCountOfLoop, ref int nSizeOfPattern, ref int[] arPattern)
        {
            throw new NotImplementedException();
        }

        public override void SetOutputListTableInterval(ref int nOutputChannel, ref double dInterval)
        {
            throw new NotImplementedException();
        }

        public override void StartOutputListTable(ref int[] arChannel, ref int nSize)
        {
            throw new NotImplementedException();
        }

        public override void StopOutputListTable(ref int[] arChannel, ref int nSize)
        {
            throw new NotImplementedException();
        }

        public override void ResetOutputListTable(ref int nOutputChannel)
        {
            throw new NotImplementedException();
        }

        public override void GetOutputListTable(ref int nOutputChannel, ref int nLoopCount, ref int nPatternSize, ref int[] arPattern)
        {
            throw new NotImplementedException();
        }

        public override void GetOutputListTableInterval(ref int nOutputChannel, ref double dblInterval)
        {
            throw new NotImplementedException();
        }

        public override void GetOutputListTableStatus(ref int nOutputChannel, ref int nPatternIndex, ref int nCountOfLoop, ref uint uInBusy)
        {
            throw new NotImplementedException();
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

            _analogInputModuleCount = ini.GetInt(SECTION_NAME, KEY_ANALOG_INPUT_CHANNEL_COUNT, -1);
            if (_analogInputModuleCount < 0)
            {
                saveFlag = true;
                _analogInputModuleCount = 1;
            }

            _analogInputModuleStartingAddress = ini.GetInt(SECTION_NAME, KEY_ANALOG_INPUT_CHANNEL_START_ADDRESS, -1);
            if (_analogInputModuleStartingAddress < 0)
            {
                saveFlag = true;
                _analogInputModuleStartingAddress = 0;
            }

            _analogOutputModuleCount = ini.GetInt(SECTION_NAME, KEY_ANALOG_OUTPUT_CHANNEL_COUNT, -1);
            if (_analogOutputModuleCount < 0)
            {
                saveFlag = true;
                _analogOutputModuleCount = 1;
            }

            _analogOutputModuleStartingAddress = ini.GetInt(SECTION_NAME, KEY_ANALOG_OUTPUT_CHANNEL_START_ADDRESS, -1);
            if (_analogOutputModuleStartingAddress < 0)
            {
                saveFlag = true;
                _analogOutputModuleStartingAddress = 0;
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

            ini.WriteInt(SECTION_NAME, KEY_ANALOG_INPUT_CHANNEL_COUNT, _analogInputModuleCount);
            ini.WriteInt(SECTION_NAME, KEY_ANALOG_INPUT_CHANNEL_START_ADDRESS, _analogInputModuleStartingAddress);

            ini.WriteInt(SECTION_NAME, KEY_ANALOG_OUTPUT_CHANNEL_COUNT, _analogOutputModuleCount);
            ini.WriteInt(SECTION_NAME, KEY_ANALOG_OUTPUT_CHANNEL_START_ADDRESS, _analogOutputModuleStartingAddress);

        }
        #endregion </Methods>

    }
}