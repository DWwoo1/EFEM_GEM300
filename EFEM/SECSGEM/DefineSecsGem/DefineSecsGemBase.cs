using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3.SECSGEM.DefineSecsGem
{
    #region <Class>
    public static class PATH
    {
        public static readonly string FILE_PATH_CFG = System.Environment.CurrentDirectory + @"\SecsGem\";
        public static readonly string FILEPATH_LOG = Define.DefineConstant.FilePath.FILEPATH_LOG + @"\SecsGem\";

        public static readonly string FILE_NAME_CFG = "Eq.Cfg";
    }

    public static class Contants
    {
        public static readonly int SCENARIO_STEP_END = 1000;
    }

    public class UserDefinedSecsMessage
    {
        #region <Constuctor>
        public UserDefinedSecsMessage(long stream, long function)
        {
            Stream = stream;
            Function = function;
        }
        #endregion </Constuctor>

        #region <Fields>
        private List<SemiObject> _listItemFormat = new List<SemiObject>();
        #endregion </Fields>

        #region <Properties>
        public string Name { get; private set; }
        public long Stream { get; private set; }
        public long Function { get; private set; }

        public List<SemiObject> ListItemFormat { get { return _listItemFormat; } }
        #endregion </Properties>

        #region <Methods>
        public void GetStructure(ref List<SemiObject> item)
        {
            if (item == null || _listItemFormat == null)
                return;

            item.Clear();
            for (int i = 0; i < _listItemFormat.Count; ++i)
                item.Add(_listItemFormat[i]);
        }
        public void SetStructure(List<SemiObject> item)
        {
            if (item == null || _listItemFormat == null)
                return;

            _listItemFormat.Clear();
            for (int i = 0; i < item.Count; ++i)
                _listItemFormat.Add(item[i]);
        }

        public string GetValueAsStringFromStructure(string nameToGet, int nTarget = 0)
        {
            for (int i = 0; i < _listItemFormat.Count; ++i)
            {
                if (_listItemFormat[i].Name.Equals(nameToGet))
                {
                    return _listItemFormat[i].GetTargetValueString(nTarget);
                }
            }

            return String.Empty;
        }
        #endregion </Methods>
    }

    #region <SemiObject>
    public abstract class SemiObject
    {
        #region <Properties>
        public EN_ITEM_FORMAT Format { get; protected set; }

        public string Name { get; protected set; }
        #endregion </Properties>

        #region Method
        public abstract string GetValueStringAll();
        public abstract string GetValueString();
        public abstract string GetTargetValueString(int nTarget);
        #endregion
    }

    public abstract class ObjectValue<T> : SemiObject
    {
        #region <Constuctor>
        protected ObjectValue(EN_ITEM_FORMAT format, string name, params T[] value)
        {
            Format = format;
            Name = name;

            _value = new T[value.Length];
            Array.Copy(value, _value, value.Length);
        }
        #endregion </Constuctor>

        #region <Fields>
        protected T[] _value;
        #endregion </Fields>

        #region <Methods>

        //#region <String Returns>
        //public virtual string GetValueString()
        //{
        //    return _value.ToString();
        //}
        //#endregion </String Returns>

        #region <Value>
        public void SetValue(T newValue)
        {
            _value[0] = newValue;
        }

        public void SetValues(T[] newValue)
        {
            _value = new T[newValue.Length];
            Array.Copy(newValue, _value, newValue.Length);
        }

        public T GetValue()
        {
            return _value[0];
        }
        public override string GetValueStringAll()
        {
            if (_value == null)
                return string.Empty;

            string messageToReturn = string.Empty;
            for (int i = 0; i < _value.Length; ++i)
            {
                if (string.IsNullOrEmpty(messageToReturn))
                {
                    messageToReturn = _value[i].ToString();
                }
                else
                {
                    messageToReturn = string.Format("{0} {1}", messageToReturn, _value[i].ToString());
                }
            }

            return messageToReturn;
        }

        public override string GetValueString()
        {
            if (_value == null)
                return string.Empty;

            return _value[0].ToString();
        }
        public T GetTargetValue(int nTarget)
        {
            return _value[nTarget];
        }
        public override string GetTargetValueString(int nTarget)
        {
            return _value[nTarget].ToString();
        }
        public T[] GetValues()
        {
            return _value;
        }
        #endregion </Value>

        #region <Item Format>
        public void SetItemFormat(EN_ITEM_FORMAT format)
        {
            Format = format;
        }

        public EN_ITEM_FORMAT GetItemFormat()
        {
            return Format;
        }
        #endregion </Item Format>

        #endregion </Methods>
    }

    #region <DataType에 따른 상속>
    public class SemiObjectList : ObjectValue<long>
    {
        public SemiObjectList(long count)
            : base(EN_ITEM_FORMAT.LIST, "List", count)
        {
            //Count = count;
        }

        //public int Count { get; private set; }
    }

    public class SemiObjectAscii : ObjectValue<string>
    {
        public SemiObjectAscii(string name, string value)
            : base(EN_ITEM_FORMAT.ASCII, name, string.IsNullOrEmpty(value) ? string.Empty : value)
        {
        }
    }

    public class SemiObjectBinary : ObjectValue<byte>
    {
        public SemiObjectBinary(string name, params byte[] value)
            : base(EN_ITEM_FORMAT.BINARY, name, value)
        {
        }
    }

    public class SemiObjectBool : ObjectValue<bool>
    {
        public SemiObjectBool(string name, params bool[] value)
            : base(EN_ITEM_FORMAT.BOOL, name, value)
        {
        }
        public SemiObjectBool(string name, params byte[] value)
            : base(EN_ITEM_FORMAT.BOOL, name)
        {
            bool[] convertValue = new bool[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                convertValue[i] = value[i] == '1';
            }

            SetValues(convertValue);
        }
    }

    public class SemiObjectFloat4 : ObjectValue<float>
    {
        public SemiObjectFloat4(string name, params float[] value)
            : base(EN_ITEM_FORMAT.FLOAT4, name, value)
        {
        }
    }

    public class SemiObjectFloat8 : ObjectValue<double>
    {
        public SemiObjectFloat8(string name, params double[] value)
            : base(EN_ITEM_FORMAT.FLOAT8, name, value)
        {
        }
    }

    public class SemiObjectInt : ObjectValue<sbyte>
    {
        public SemiObjectInt(string name, params sbyte[] value)
            : base(EN_ITEM_FORMAT.INT, name, value)
        {
        }
    }

    public class SemiObjectInt2 : ObjectValue<short>
    {
        public SemiObjectInt2(string name, params short[] value)
            : base(EN_ITEM_FORMAT.INT2, name, value)
        {
        }
    }

    public class SemiObjectInt4 : ObjectValue<int>
    {
        public SemiObjectInt4(string name, params int[] value)
            : base(EN_ITEM_FORMAT.INT4, name, value)
        {
        }
    }

    public class SemiObjectInt8 : ObjectValue<long>
    {
        public SemiObjectInt8(string name, params long[] value)
            : base(EN_ITEM_FORMAT.INT8, name, value)
        {
        }
    }

    public class SemiObjectUInt : ObjectValue<byte>
    {
        public SemiObjectUInt(string name, params byte[] value)
            : base(EN_ITEM_FORMAT.UINT, name, value)
        {
        }
    }

    public class SemiObjectUInt2 : ObjectValue<ushort>
    {
        public SemiObjectUInt2(string name, params ushort[] value)
            : base(EN_ITEM_FORMAT.UINT2, name, value)
        {
        }
    }

    public class SemiObjectUInt4 : ObjectValue<uint>
    {
        public SemiObjectUInt4(string name, params uint[] value)
            : base(EN_ITEM_FORMAT.UINT4, name, value)
        {
        }
    }

    public class SemiObjectUInt8 : ObjectValue<ulong>
    {
        public SemiObjectUInt8(string name, params ulong[] value)
            : base(EN_ITEM_FORMAT.UINT8, name, value)
        {
        }
    }
    #endregion

    #endregion </SemiObject>

    public static class DefinesForClientToClientMessage
    {
        public const string VALUE_MESSAGE_TYPE_SEND = "S";
        public const string VALUE_MESSAGE_TYPE_ACK = "A";
    }

    public class WaferMapData
    {
        #region <Fields>
        private const string AttributeNameOfMaterialId = "MID";
        private const string AttributeNameOfIdType = "IDTYP";
        private const string AttributeNameOfMapFormatType = "MAPFT";
        private const string AttributeNameOfFlatNotchLocation = "FNLOC";
        private const string AttributeNameOfFilmFrameLocation = "FFROT";
        private const string AttributeNameOfOriginLocation = "ORLOC";
        private const string AttributeNameOfProcessAccess = "PRAXI";
        private const string AttributeNameOfBinCodeEquivalents = "BCEQU";
        private const string AttributeNameOfNullBinCode = "NULBC";
        private const string AttributeNameOfReferenceX = "RefX";
        private const string AttributeNameOfReferenceY = "RefY";
        private const string AttributeNameOfStartingX = "StartingX";
        private const string AttributeNameOfStartingY = "StartingY";
        private const string AttributeNameOfCountRow = "CountRow";
        private const string AttributeNameOfCountCol = "CountCol";
        private const string AttributeNameOfXAxisDieSize = "XDIES";
        private const string AttributeNameOfYAxisDieSize = "YDIES";
        private const string AttributeNameOfCountProcessDies = "CountProcessDies";
        private const string AttributeNameOfMapData = "MapData";
        #endregion </Fields>

        #region <Properties>
        public string WaferId { get; set; }
        public double Angle { get; set; }
        public int IndexOfRefX { get; set; }
        public int IndexOfRefY { get; set; }
        public int SizeOfDieX { get; set; }
        public int SizeOfDieY { get; set; }
        public int IndexOfStartingX { get; set; }
        public int IndexOfStartingY { get; set; }
        public int CountOfRow { get; set; }
        public int CountOfCol { get; set; }
        public int CountOfProcessDies { get; set; }
        public string NullBinCode { get; set; }
        public string MapData { get; set; }
        #endregion </Properties>

        public Dictionary<string, string> GetDataAll()
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { AttributeNameOfMaterialId, WaferId },
                { AttributeNameOfFlatNotchLocation, Angle.ToString() },
                { AttributeNameOfReferenceX, IndexOfRefX.ToString() },
                { AttributeNameOfReferenceY, IndexOfRefY.ToString() },
                { AttributeNameOfStartingX, IndexOfStartingX.ToString() },
                { AttributeNameOfStartingY, IndexOfStartingY.ToString() },
                { AttributeNameOfCountRow, CountOfRow.ToString() },
                { AttributeNameOfCountCol, CountOfCol.ToString() },
                { AttributeNameOfXAxisDieSize, SizeOfDieX.ToString() },
                { AttributeNameOfYAxisDieSize, SizeOfDieY.ToString() },
                { AttributeNameOfCountProcessDies, CountOfProcessDies.ToString() },
                { AttributeNameOfNullBinCode, NullBinCode },
                { AttributeNameOfMapData, MapData }
            };

            return data;
        }
    }

    public sealed class AutoScenarioRequest
    {
        public string Sender { get; set; }
        public EN_SCENARIO Scenario { get; set; }
        public Dictionary<string, string> ScenarioParams { get; set; }
        public Dictionary<string, string> AdditionalParams { get; set; }
        public deleAutoScenarioCompleted Callback { get; set; }
        public EN_SCENARIO_RESULT QueueState { get; set; }
        public bool UseLogging { get; set; }
    }

    public class QueuedScenarioInfo
    {
        public EN_SCENARIO Scenario { get; set; }
        public Dictionary<string, string> ScenarioParams { get; set; }
        public Dictionary<string, string> AdditionalParams { get; set; }
    }

    public class StatusVariable
    {
        public StatusVariable(long id, string name)
        {
            Id = id;
            Name = name;
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public EN_ITEM_FORMAT ItemFormat { get; set; }
    }

    public class CollectionEvent
    {
        public CollectionEvent(long id, Dictionary<long, StatusVariable> variables)
        {
            Id = id;

            Variables = new Dictionary<long, StatusVariable>(variables);
        }
        public bool CustomScenario { get; set; }
        public long Id { get; set; }
        public Dictionary<long, StatusVariable> Variables { get; set; }
        public List<long> VariableIds
        {
            get
            {
                if (Variables == null)
                    return null;

                return Variables.Keys.ToList();
            }
        }
    }

    public class EquipmentConstant
    {
        public EquipmentConstant(long id, string name)
        {
            Id = id;
            Name = name;
            Value = string.Empty;
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public EN_ITEM_FORMAT ItemFormat { get; private set; }
        public string Value { get; set; }
        public void SetRange<T>(T min, T max)
        {

        }
    }
    #endregion </Class>

    #region <Enum>
    public enum EN_SCENARIO
    {
        FdcUpdate,
        RecipeChanged,
        RecipeParameterChanged,

        SCENARIO_REQ_LOT_INFO_CORE_1,
        SCENARIO_REQ_LOT_INFO_CORE_2,
        SCENARIO_REQ_LOT_INFO_CORE_3,               // W Only
        SCENARIO_REQ_LOT_INFO_EMPTY_TAPE,

        SCENARIO_PORT_STATUS_LOAD_1,
        SCENARIO_PORT_STATUS_LOAD_2,
        SCENARIO_PORT_STATUS_LOAD_3,
        SCENARIO_PORT_STATUS_LOAD_4,
        SCENARIO_PORT_STATUS_LOAD_5,                // BIN Only
        SCENARIO_PORT_STATUS_LOAD_6,                // BIN Only

        SCENARIO_PORT_STATUS_UNLOAD_1,
        SCENARIO_PORT_STATUS_UNLOAD_2,
        SCENARIO_PORT_STATUS_UNLOAD_3,
        SCENARIO_PORT_STATUS_UNLOAD_4,
        SCENARIO_PORT_STATUS_UNLOAD_5,              // BIN Only
        SCENARIO_PORT_STATUS_UNLOAD_6,              // BIN Only

        SCENARIO_EQUIPMENT_START,
        SCENARIO_EQUIPMENT_END,
        SCENARIO_ERROR_START,
        SCENARIO_ERROR_STOP,

        SCENARIO_PROCESS_START,
        SCENARIO_PROCESS_END,

        SCENARIO_CARRIER_LOAD,
        SCENARIO_CARRIER_UNLOAD,

        SCENARIO_RFID_READ_CORE_1,
        SCENARIO_RFID_READ_CORE_2,
        SCENARIO_RFID_READ_CORE_3,                  // W Only
        SCENARIO_RFID_READ_EMPTY_TAPE,
        SCENARIO_RFID_READ_BIN_1,                   // BIN Only
        SCENARIO_RFID_READ_BIN_2,                   // BIN Only
        SCENARIO_RFID_READ_BIN_3,                   // BIN Only

        SCENARIO_REQ_SLOT_INFO_CORE_1,
        SCENARIO_REQ_SLOT_INFO_CORE_2,
        SCENARIO_REQ_SLOT_INFO_CORE_3,              // W Only
        SCENARIO_REQ_SLOT_INFO_EMPTY_TAPE,

        SCENARIO_REQ_RECIPE_DOWNLOAD,
        SCENARIO_REQ_RECIPE_UPLOAD,
        SCENARIO_RECIPE_DOWNLOAD_BY_HOST,               // 2026.07.09 dwlim [ADD] Host, PM 중 누가 요청했냐에 따라 처리순서 다름
        SCENARIO_RECIPE_UPLOAD_BY_HOST,                 // 2026.07.09 dwlim [ADD] Host, PM 중 누가 요청했냐에 따라 처리순서 다름

        SCENARIO_REQ_TRACK_IN,
        SCENARIO_REQ_CORE_WAFER_TRACK_OUT,
        SCENARIO_REQ_LOT_MATCH,
        SCENARIO_REQ_BIN_WAFER_TRACK_OUT,
        SCENARIO_SLOT_WAFER_MAPPING_CORE_1,
        SCENARIO_SLOT_WAFER_MAPPING_CORE_2,
        SCENARIO_SLOT_WAFER_MAPPING_CORE_3,         // W Only
        SCENARIO_SLOT_WAFER_MAPPING_EMPTY_TAPE,
        SCENARIO_SLOT_WAFER_MAPPING_BIN_1,          // BIN Only
        SCENARIO_SLOT_WAFER_MAPPING_BIN_2,          // BIN Only
        SCENARIO_SLOT_WAFER_MAPPING_BIN_3,          // BIN Only
        SCENARIO_REQ_LOT_MERGE_CORE_1,
        SCENARIO_REQ_LOT_MERGE_CORE_2,
        SCENARIO_REQ_LOT_MERGE_CORE_3,              // W Only
        SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_1,
        SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_2, // BIN Only
        SCENARIO_REQ_LOT_ID_MERGE_AND_CHANGE_BIN_3, // BIN Only

        // 2026.06.22. jhlim [ADD] ADS Move 시나리오 추가
        SCENARIO_ADS_MOVE_FLAG_1,
        SCENARIO_ADS_MOVE_FLAG_2,
        SCENARIO_SCRAP_BIN_CHIP,
        SCENARIO_UPLOAD_BIN_SCRAP_INFO,     // Client to Client Message
        SCENARIO_SCRAP_CORE_CHIP,
        SCENARIO_UPLOAD_WORK_RESULT,

        SCENARIO_WORK_START,
        SCENARIO_WORK_END,
        SCENARIO_REQ_CORE_WAFER_SPLIT,
        SCENARIO_REQ_CORE_WAFER_SPLIT_LAST,
        SCENARIO_CORE_WAFER_DETACH_START,
        SCENARIO_CORE_WAFER_DETACH_END,
        SCENARIO_REQ_CORE_CHIP_SPLIT_FIRST,
        SCENARIO_REQ_CORE_CHIP_SPLIT,
        SCENARIO_REQ_CORE_CHIP_FULL_SPLIT_FIRST,    // BIN Only
        SCENARIO_REQ_CORE_CHIP_FULL_SPLIT,          // BIN Only
        SCENARIO_REQ_CORE_CHIP_MERGE,

        SCENARIO_BIN_WAFER_ID_READ,
        SCENARIO_BIN_WORK_END,

        SCENARIO_BIN_PART_ID_INFO_REQ,
        SCENARIO_BIN_DATA_UPLOAD,

        SCENARIO_REQ_BIN_WAFER_ID_ASSIGN,
        SCENARIO_REQ_CORE_WAFER_ID,
        SCENARIO_BIN_SORTING_START_1,
        SCENARIO_BIN_SORTING_END_1,
        SCENARIO_BIN_SORTING_START_2,               // BIN Only
        SCENARIO_BIN_SORTING_END_2,                 // BIN Only
        SCENARIO_BIN_SORTING_START_3,               // BIN Only
        SCENARIO_BIN_SORTING_END_3,                 // BIN Only

        #region 추후 구현
        SCENARIO_REQ_COLLET_CHANGE_1,
        SCENARIO_REQ_COLLET_CHANGE_2,
        SCENARIO_REQ_HOOD_CHANGE,
        #endregion

        SCENARIO_REQ_UPLOAD_BINFILE,
        SCENARIO_ASSIGN_SUBSTRATE_ID,

        SCENARIO_PICK_UP_END,
        SCENARIO_PLACE_END,

        // GEM300 Only
        /*
         * SCENARIO_EQUIPMENT_START(있음)
         * SCENARIO_EQUIPMENT_END(있음)
         * SCENARIO_PROCESS_START(있음)
         * SCENARIO_PROCESS_END(있음)
         * SCENARIO_ERROR_START(있음)
         * SCENARIO_ERROR_STOP(있음)
         * SCENARIO_LOT_START(추가)
         * SCENARIO_LOT_END(추가)
         * SCENARIO_WAFER_START(추가)
         * SCENARIO_WAFER_END(추가)
         * SCENARIO_CHAMBER_START(추가)
         * SCENARIO_CHAMBER_END(추가)
         * SCENARIO_CORE_MAP_UPLOAD(추가)
         * SCENARIO_CORE_WAFER_ID_REQ(있음:SCENARIO_REQ_CORE_WAFER_ID, 일단 미구현)
         * SCENARIO_STEP_START(추가)
         * SCENARIO_STEP_END(추가)
         * SCENARIO_CORE_WAFER_DETACH_START(있음)
         * SCENARIO_CORE_WAFER_DETACH_END(있음)
         * SCENARIO_BIN_WAFER_RING_ID_READ(있음:SCENARIO_BIN_WAFER_ID_READ)
         * SCENARIO_BIN_DATA_UPLOAD(있음:일단 미구현)      // 구현중
         * SCENARIO_BIN_MAP_UPLOAD(추가)
         * SCENARIO_BIN_SORTING_START(있음)
         * SCENARIO_BIN_SORTING_END(있음)
         * SCENARIO_REQ_UPLOAD_BINFILE(PM에 전달용)
         * SCENARIO_ASSIGN_SUBSTRATE_ID(PM에 전달용)
         */
        SCENARIO_LOT_START,
        SCENARIO_LOT_END,
        SCENARIO_WAFER_START,
        SCENARIO_WAFER_END,
        SCENARIO_CHAMBER_START,
        SCENARIO_CHAMBER_END,
        SCENARIO_CORE_MAP_DOWNLOAD,     // 2026.05.14. dwlim [ADD] GEM300이 아닐때에는 Work End에 붙어있었는데, GEM300일 때에는 쓰지않게 되어 추가
        SCENARIO_CORE_MAP_UPLOAD,
        SCENARIO_STEP_START,
        SCENARIO_STEP_END,
        SCENARIO_BIN_MAP_UPLOAD,
        SCENARIO_BIN_WAFER_END,
    }
    public enum EN_GEM_ALARM_STATE
    {
        CLEARED = 0,
        OCCURED = 1,
    }
    public enum EN_MESSAGE_RESULT
    {
        NG = 0,
        OK = 1,
    }
    public enum EN_AUTO_SCENARIO_STATE
    {
        IDLE,
        WAITING,
        DEQUEUED,
        RUNNING,
        COMPLETED,
        ERROR,
        TIMEOUT_ERROR,
    }
    public enum EN_SCENARIO_RESULT
    {
        PROCEED,
        COMPLETED,
        ERROR,
        TIMEOUT_ERROR,

        // 추가
        WAITING,
    }
    public enum EN_COMM_STATE
    {
        DISABLED = 1,
        WAIT_CR_FROM_HOST,
        WAIT_DELAY,
        WAIT_CRA,
        COMMUNICATING,
    }
    public enum EN_SCENARIO_SEQ
    {
        INIT = 0,
        SEND_EVENT = 100,
        WAIT_FOR_PERMISSION = 200,
        AFTER_PERMISSION = 300,
        FINISH,
    }
    public enum EN_SETTING_CONTROL_STATE
    {
        OFFLINE = 1,
        //HOST_OFFLINE = 3,       // Host Offline은 설정할 수 없다.
        LOCAL = 4,
        REMOTE = 5,
    }
    public enum EN_CONTROL_STATE
    {
        OFFLINE = 1,
        ATTEMP_ONLINE = 2,
        HOST_OFFLINE = 3,
        LOCAL = 4,
        REMOTE = 5
    }
    public enum EN_ITEM_FORMAT
    {
        LIST = 0,
        ASCII,
        BINARY,
        BOOL,
        UINT,
        UINT2,
        UINT4,
        UINT8,
        INT,
        INT2,
        INT4,
        INT8,
        FLOAT4,
        FLOAT8,
    }
    public enum EN_XEIC_DEVICE_SIGNAL_ACK
    {
        NOT_COMPLETE,
        OK,
        NG,
    }
    public enum EN_REMOTE_COMMAND_RESULT
    {
        INIT,
        OK,
        ERROR
    }
    public enum EN_SCENARIO_PERMISSION_RESULT
    {
        OK,
        PROCEED,
        ERROR
    }
    public enum EN_PPGRANT
    {
        OK = 0,
        ALREADY_HAVE,
        NO_SPACE,
        INVALID_PPID,
        BUSY,
    }
    public enum EN_ACK7
    {
        OK = 0,             // Accepted
        PERMISSION = 1,     // Permission not granted
        LENGTH = 2,         // Length error
        OVERFLOW = 3,       // Matrix overflow
        NOT_FOUND = 4,      // PPID not found
        UNSUPPORTED = 5,    // Mode unsupported
        PERFORM_LATER = 6   // Command will be performed with completion signaled later
    }
    public enum EN_OPCALL_LEVEL
    {
        INFO = 1,
        WARNING = 2,
        ERROR = 3,
        DOWN = 4,
        ETC = 5,
    }
    public enum EAC                     // Equipment Acknowledge Code
    {
        OK = 0,
        CONSTANTS_DOES_NOT_EXIST,       // 1 - one or more constants does not exist
        BUSY,                           // 2 - busy
        OUT_OF_RANGE,                   // 3 - one or more values out of range
    }
    public enum EN_CPACK_TYPE
    {
        OK = 0,
        UNKNOWN_CPNAME = 1,
        ILLEGAL_VALUE_FOR_CPVAL = 2,
        ILLEGAL_FORMAT_FOR_CPVAL = 3,
    }
    public enum CarrierActionAck
    {
        Ok = 0,
        InvalidCommand = 1,
        CannotPerformNow = 2,
        InvalidDataOrArgument = 3,
        InitiatedForAsynchronousCompletion = 4,
        RejectedByInvalidState = 5,
        CommandPerformedWithErrors = 6
    }
    public enum MapTypes : byte
    {
        WaferId = 0,
        WaferCassetteId,
        FilmFrameId
    }
    public enum MapDataFormatTypes : byte
    {
        RowFormat = 0,
        ArrayFormat,
        CoordinateFormat
    }
    public enum OriginLocationTypes : byte
    {
        CenterDieOfWafer = 0,
        UpperRight,
        UpperLeft,
        LowerLeft,
        LowerRight
    }
    public enum ScenarioTypes
    {
        SendingEventScenario,
        ClientToClientCommunicationScenario,
        Custom
    }
    public enum LogTypes
    {
        History,
        Terminal,
        Scenario
    }
    public enum EN_AUTO_SCENARIO_LOG_PHASE
    {
        NONE = 0,

        ENQUEUE,
        DEQUEUE,
        DEQUEUE_REQUEUE,
        START,

        PREPARE_FAIL,

        COMPLETE,
        COMPLETE_ERROR,
        COMPLETE_TIMEOUT,

        CALLBACK_ERROR,
    }
    public enum ScenarioSenders
    {
        Auto,
        Manual
    }
    #endregion </Enum>

    #region <Events>
    // Connections
    public delegate void deleHandlerVoid();

    // Terminal Message
    public delegate void deleHandlerString(string message);

    // RemoteCommand
    public delegate bool deleRemoteCommand(string rcmdName, string[] cpNames, string[] cpValues, ref long[] results);

    // Received Signal
    public delegate bool deleRecvClientToClientMessage(string device, string messageName, string sendingType, string scenarioName, string[] contentNames, string[] messages, EN_MESSAGE_RESULT result);

    // Up/Downloading Recipe
    public delegate bool deleReqRecipeControl(string recipeName);

    // Variables
    public delegate void deleChangeEquipmentParameters(string[] ecNames, string[] values);


    public delegate bool deleDisplayOperatorCallForm(EN_OPCALL_LEVEL level, string operatorId, bool usingBuzzer, string message);


    public delegate bool deleSecsMessageReceived(UserDefinedSecsMessage receivedSecsMessage, ref UserDefinedSecsMessage secsMessageToSend);


    public delegate EN_PPGRANT deleRecipeControlGrant(string recipeName);

    public delegate void deleAutoScenarioCompleted(
        string sender,
        EN_SCENARIO scenario,
        Dictionary<string, string> scenarioParams,
        Dictionary<string, string> resultData,
        EN_SCENARIO_RESULT result);


    #region <Recipe Control>
    public delegate bool deleReqUPloadingUnformattedRecipeControl(string recipeName, ref string recipeFullPath);
    public delegate EN_ACK7 deleReqDownloadingUnformattedRecipeControl(string recipeName, string recipeFullPath);

    //24.09.20 by wdw [ADD] Scenerio S7F4 Ack 확인 
    public delegate void deleReqUPloadingUnformattedRecipeAck(string recipeName, EN_ACK7 Ack);

    // RecipeName, <CCode, PParam>
    public delegate bool deleReqUploadingFormattedRecipe(string recipeName, out Dictionary<string, SemiObject[]> recipeBodies);

    public delegate bool deleReqDownloadingFormattedRecipe(string recipeName, Dictionary<string, string[]> recipeBodies);

    public delegate void deleRecipeFileIsDeleted(string[] recipeFiles);
    #endregion </Recipe Control>

    #endregion </Events>
}