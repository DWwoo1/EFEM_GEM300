using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    public class ScenarioUploadBinDataParamValues : ScenarioParamValues
    {
        public ScenarioUploadBinDataParamValues(List<string> values, bool useEventHandling, string dataPathToUpload, WaferMapData mapData) : base(values)
        {
            UseEventHandling = useEventHandling;
            WaferDataToHandling = mapData;
            DataPathToUpload = dataPathToUpload;
        }

        public readonly bool UseEventHandling;
        public readonly string DataPathToUpload;
        public readonly WaferMapData WaferDataToHandling;
    }

    public class ScenarioUploadBinData : AutoScenarioBase
    {
        #region <Constructors>
        public ScenarioUploadBinData(string name, long eventId, List<long> variables,
            long vidPmsBody,
            long streamToHandling,
            long funcToSetupDataSend, long funcToDataTransmitInquire, long funcToDataSend,
            EN_ITEM_FORMAT formatForUint,
            uint timeOut = 10000)
            : base(name, timeOut)
        {
            _receiveMessageFormat = new List<SemiObject>();
            MessageFormatToSend = new List<SemiObject>();

            ReceiveStream = streamToHandling;
            ReceiveFunction = funcToSetupDataSend;

            FunctionToSendWaferMapDataSetup = funcToSetupDataSend;
            FunctionToSendWaferMapTransmitInquire = funcToDataTransmitInquire;
            FunctionToSendWaferMapData = funcToDataSend;

            FunctionToReceivedWaferMapDataSetup = FunctionToSendWaferMapDataSetup + 1;
            FunctionToReceivedWaferMapTransmitInquire = FunctionToSendWaferMapTransmitInquire + 1;
            FunctionToReceivedWaferMapData = FunctionToSendWaferMapData + 1;

            _eventId = eventId;
            _variables = new List<long>(variables);

            VidPmsBody = vidPmsBody;

            ItemFormatForUint = formatForUint;
        }
        #endregion </Constructors>

        #region <Fields>
        private ScenarioUploadBinDataParamValues _paramValue = null;
        private List<SemiObject> _receiveMessageFormat = null;
        private readonly List<SemiObject> MessageFormatToSend = null;

        private readonly long VidPmsBody;

        private readonly EN_ITEM_FORMAT ItemFormatForUint;

        private long _eventId;
        private List<long> _variables;
        private readonly long FunctionToSendWaferMapDataSetup;
        private readonly long FunctionToSendWaferMapTransmitInquire;
        private readonly long FunctionToSendWaferMapData;

        private const string AttributeNameOfMaterialId = "MID";
        private const string AttributeNameOfIdType = "IDTYP";
        private const string AttributeNameOfMapFormatType = "MAPFT";
        private const string AttributeNameOfFlatNotchLocation = "FNLOC";
        private const string AttributeNameOfFilmFrameLocation = "FFROT";
        private const string AttributeNameOfOriginLocation = "ORLOC";
        private const string AttributeNameOfReferencePointSelect = "RPSEL";
        private const string AttributeNameOfReferenceXY = "REFXY";
        private const string AttributeNameOfDieUnitsOfMeasure = "DUTMS";
        private const string AttributeNameOfXAxisDieSize = "XDIES";
        private const string AttributeNameOfYAxisDieSize = "YDIES";
        private const string AttributeNameOfCountRow = "ROWCT";
        private const string AttributeNameOfCountCol = "COLCT";
        private const string AttributeNameOfProcessDieCount = "PRDCT";
        private const string AttributeNameOfNullBinCode = "NULBC";
        private const string AttributeNameOfProcessAccess = "PRAXI";
        private const string AttributeNameOfMessageLength = "MLCL";
        private const string AttributeNameOfStartingPointXY = "STRPxy";

        private const string AttributeNameOfBinCodeList = "BINLT";

        private const ushort AttributeValueFilmFrameLocation = 0;
        private const ushort AttributeValueMessageLength = 0;
        private const byte AttributeValueReferencePointSelect = 0;
        private const string AttributeValueBinCodeEquivalents = "0123456789DEFGHINOPQRSTUVXYabcdefghimxyz";
        private const string AttributeValueDieUnitsOfMeasure = "mm";
        private const string AttributeValueNullBinCode = " ";
        private const byte AttributeValueProcessAccess = 2;
        #endregion </Fields>

        #region <Types>
        enum MessageTypeToSend
        {
            MapSetupData,           // 12, 1
            MapTransmitInquire,     // 12, 5
            MapDataSend             // 12, 9
        }
        enum ScenarioSeq
        {
            INIT = 0,
            SEND_SECSMESSAGE_MAP_SETUP_DATA = 100,
            SEND_SECSMESSAGE_MAP_TRANSMIT_INQUIRE = 200,
            SEND_SECSMESSAGE_MAP_DATA = 300,
            
            UPDATE_BINARY_VARIABLE = 400,
            SEND_EVENT = 500,
            
            FINISH = 1000,
        }
        #endregion </Types>

        #region <Properties>
        public long FunctionToReceivedWaferMapDataSetup { get; private set; }
        public long FunctionToReceivedWaferMapTransmitInquire { get; private set; }
        public long FunctionToReceivedWaferMapData { get; private set; }
        public long EventId
        {
            get
            {
                return _eventId;
            }
        }
        //public WaferMapData WaferData
        //{
        //    get
        //    {
        //        if (_paramValue == null)
        //        {
        //            _paramValue = new ScenarioUploadBinDataParamValues(null, new WaferMapData());
        //        }

        //        return _paramValue.WaferDataToHandling;
        //    }
        //}
        #endregion </Properties>

        #region <Methods>
        public override EN_SCENARIO_RESULT ExecuteScenario()
        {
            switch (_seqNum)
            {
                case (int)ScenarioSeq.INIT:
                    {
                        //Activate = true;
                        //InitFlags();
                        Receiving = true;

                        if (FrameOfSystem3.Task.TaskOperator.GetInstance().IsSimulationMode())
                        {
                            _seqNum = (int)ScenarioSeq.UPDATE_BINARY_VARIABLE;
                        }
                        else
                        {
                            _seqNum = (int)ScenarioSeq.SEND_SECSMESSAGE_MAP_SETUP_DATA;
                        }
                    }
                    break;

                case (int)ScenarioSeq.SEND_SECSMESSAGE_MAP_SETUP_DATA:
                    {
                        if (false == SendSecsMessageMapData(MessageTypeToSend.MapSetupData))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                        
                        _gemHandler.SendUserDefinedSecsMessage(ReceiveStream, FunctionToSendWaferMapDataSetup, MessageFormatToSend);
                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)ScenarioSeq.SEND_SECSMESSAGE_MAP_SETUP_DATA + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        switch (Permission)
                        {
                            case EN_SCENARIO_PERMISSION_RESULT.OK:
                                Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
                                _seqNum = (int)ScenarioSeq.SEND_SECSMESSAGE_MAP_TRANSMIT_INQUIRE;
                                break;

                            case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                    }
                    break;

                case (int)ScenarioSeq.SEND_SECSMESSAGE_MAP_TRANSMIT_INQUIRE:
                    {
                        if (false == SendSecsMessageMapData(MessageTypeToSend.MapTransmitInquire))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                        _gemHandler.SendUserDefinedSecsMessage(ReceiveStream, FunctionToSendWaferMapTransmitInquire, MessageFormatToSend);
                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)ScenarioSeq.SEND_SECSMESSAGE_MAP_TRANSMIT_INQUIRE + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        switch (Permission)
                        {
                            case EN_SCENARIO_PERMISSION_RESULT.OK:
                                Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
                                _seqNum = (int)ScenarioSeq.SEND_SECSMESSAGE_MAP_DATA;
                                break;

                            case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                    }
                    break;

                case (int)ScenarioSeq.SEND_SECSMESSAGE_MAP_DATA:
                    {
                        if (false == SendSecsMessageMapData(MessageTypeToSend.MapDataSend))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                        _gemHandler.SendUserDefinedSecsMessage(ReceiveStream, FunctionToSendWaferMapData, MessageFormatToSend);
                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)ScenarioSeq.SEND_SECSMESSAGE_MAP_DATA + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        switch (Permission)
                        {
                            case EN_SCENARIO_PERMISSION_RESULT.OK:
                                {
                                    Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
                                    _seqNum = (int)ScenarioSeq.UPDATE_BINARY_VARIABLE;
                                }
                                break;

                            case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                    }
                    break;

                case (int)ScenarioSeq.UPDATE_BINARY_VARIABLE:
                    {
                        SemiObjectBinary binaryItemToPms;
                        string pmsFilePath = _paramValue.DataPathToUpload;
                        if (string.IsNullOrEmpty(pmsFilePath))
                        {
                            binaryItemToPms = new SemiObjectBinary("PMS", new byte[] { 0x00 });
                        }
                        else
                        {
                            if (false == File.Exists(pmsFilePath))
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);

                            byte[] pmsBodies = File.ReadAllBytes(pmsFilePath);
                            binaryItemToPms = new SemiObjectBinary("PMS", pmsBodies);
                        }
                        _gemHandler.UpdateVariable(VidPmsBody, new List<SemiObject>() { binaryItemToPms });

                        SetTickCount(100);

                        _seqNum = (int)ScenarioSeq.SEND_EVENT;
                    }
                    break;

                case (int)ScenarioSeq.SEND_EVENT:
                    {
                        if (false == IsTickOver(false))
                            break;

                        List<long> variableIdsToUpdate = new List<long>();
                        List<string> variablesToUpdate = new List<string>();
                       
                        // Body 는 위에서 업데이트 했기 때문에 제외한다.
                        for (int i = 0; i < _variables.Count; ++i)
                        {
                            if (_variables[i] == VidPmsBody)
                            {
                                if (FrameOfSystem3.Task.TaskOperator.GetInstance().IsSimulationMode())
                                {
                                    _paramValue.VariableValues[i] = "0";
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            variableIdsToUpdate.Add(_variables[i]);
                            variablesToUpdate.Add(_paramValue.VariableValues[i]);
                        }

                        _gemHandler.SendEvent(_eventId, variableIdsToUpdate.ToArray(), variablesToUpdate.ToArray());

                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)ScenarioSeq.SEND_EVENT + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        if (false == _gemHandler.IsSendingEventCompleted(_eventId))
                            break;

                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)ScenarioSeq.SEND_EVENT + 2:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        // RCMD 대기
                        switch (Permission)
                        {
                            case EN_SCENARIO_PERMISSION_RESULT.OK:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);

                            case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                    }
                    break;

                default:
                    return EN_SCENARIO_RESULT.ERROR;
            }

            return EN_SCENARIO_RESULT.PROCEED;
        }

        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValue = paramValues as ScenarioUploadBinDataParamValues;
        }

        public bool UpdateReceivedSecsMessage(long function, List<SemiObject> listOfReceive)
        {
            if (listOfReceive.Count != 1)
                return false;

            if (!(listOfReceive[0] is SemiObjectBinary ack))
                return false;

            if (function == FunctionToReceivedWaferMapDataSetup)
            {
                Permission = ack.GetValue() == 0 ?
                    EN_SCENARIO_PERMISSION_RESULT.OK : EN_SCENARIO_PERMISSION_RESULT.ERROR;

                return true;

            }
            else if (function == FunctionToReceivedWaferMapTransmitInquire)
            {
                Permission = ack.GetValue() == 0 ?
                    EN_SCENARIO_PERMISSION_RESULT.OK : EN_SCENARIO_PERMISSION_RESULT.ERROR;

                //Permission = EN_SCENARIO_PERMISSION_RESULT.OK;
                return true;

            }
            else if (function == FunctionToReceivedWaferMapData)
            {
                Permission = ack.GetValue() == 0 ?
                    EN_SCENARIO_PERMISSION_RESULT.OK : EN_SCENARIO_PERMISSION_RESULT.ERROR;

                //Permission = EN_SCENARIO_PERMISSION_RESULT.OK;
                return true;

            }

            return false;
        }

        private bool SendSecsMessageMapData(MessageTypeToSend messageType)
        {
            if (_paramValue == null || _paramValue.WaferDataToHandling == null)
                return false;

            MessageFormatToSend.Clear();
            switch (messageType)
            {
                case MessageTypeToSend.MapSetupData:
                    {
                        MessageFormatToSend.Add(new SemiObjectList(15));
                        MessageFormatToSend.Add(new SemiObjectAscii(AttributeNameOfMaterialId, _paramValue.WaferDataToHandling.WaferId));
                        MessageFormatToSend.Add(new SemiObjectBinary(AttributeNameOfIdType, (byte)MapTypes.WaferId));
                        MessageFormatToSend.Add(new SemiObjectUInt2(AttributeNameOfFlatNotchLocation, (ushort)_paramValue.WaferDataToHandling.Angle));
                        MessageFormatToSend.Add(new SemiObjectUInt2(AttributeNameOfFilmFrameLocation, AttributeValueFilmFrameLocation));
                        MessageFormatToSend.Add(new SemiObjectBinary(AttributeNameOfOriginLocation, (byte)OriginLocationTypes.UpperLeft));
                        MessageFormatToSend.Add(new SemiObjectUInt(AttributeNameOfReferencePointSelect, AttributeValueReferencePointSelect));
                        MessageFormatToSend.Add(new SemiObjectList(1));
                        MessageFormatToSend.Add(new SemiObjectInt2(AttributeNameOfReferenceXY,
                            new short[] 
                            {
                                (short)_paramValue.WaferDataToHandling.IndexOfRefX,
                                (short)_paramValue.WaferDataToHandling.IndexOfRefY 
                            }));
                        MessageFormatToSend.Add(new SemiObjectAscii(AttributeNameOfDieUnitsOfMeasure, AttributeValueDieUnitsOfMeasure));

                        // 2025.07.25. jhlim [MOD] XDIES, YDIES, ROW, COL, DIECOUNT 자료형이 BIN과 W가 다름
                        MessageFormatToSend.Add(AddSendingMessageByType(AttributeNameOfXAxisDieSize, _paramValue.WaferDataToHandling.SizeOfDieX));
                        MessageFormatToSend.Add(AddSendingMessageByType(AttributeNameOfYAxisDieSize, _paramValue.WaferDataToHandling.SizeOfDieY));
                        MessageFormatToSend.Add(AddSendingMessageByType(AttributeNameOfCountRow, _paramValue.WaferDataToHandling.CountOfRow));
                        MessageFormatToSend.Add(AddSendingMessageByType(AttributeNameOfCountCol, _paramValue.WaferDataToHandling.CountOfCol));
                        //MessageFormatToSend.Add(new SemiObjectUInt2(AttributeNameOfXAxisDieSize, (ushort)_paramValue.WaferDataToHandling.SizeOfDieX));
                        //MessageFormatToSend.Add(new SemiObjectUInt2(AttributeNameOfYAxisDieSize, (ushort)_paramValue.WaferDataToHandling.SizeOfDieY));
                        //MessageFormatToSend.Add(new SemiObjectUInt2(AttributeNameOfCountRow, (ushort)_paramValue.WaferDataToHandling.CountOfRow));
                        //MessageFormatToSend.Add(new SemiObjectUInt2(AttributeNameOfCountCol, (ushort)_paramValue.WaferDataToHandling.CountOfCol));

                        //
                        MessageFormatToSend.Add(new SemiObjectAscii(AttributeNameOfNullBinCode, _paramValue.WaferDataToHandling.NullBinCode));
                       
                        MessageFormatToSend.Add(AddSendingMessageByType(AttributeNameOfProcessDieCount, _paramValue.WaferDataToHandling.CountOfProcessDies));
                        //MessageFormatToSend.Add(new SemiObjectUInt2(AttributeNameOfProcessDieCount, (ushort)_paramValue.WaferDataToHandling.CountOfProcessDies));
                        // 2025.07.25. jhlim [END]

                        //
                        MessageFormatToSend.Add(new SemiObjectBinary(AttributeNameOfProcessAccess, AttributeValueProcessAccess));
                    }
                    break;
                case MessageTypeToSend.MapTransmitInquire:
                    {
                        MessageFormatToSend.Add(new SemiObjectList(4));
                        MessageFormatToSend.Add(new SemiObjectAscii(AttributeNameOfMaterialId, _paramValue.WaferDataToHandling.WaferId));
                        MessageFormatToSend.Add(new SemiObjectBinary(AttributeNameOfIdType, (byte)MapTypes.WaferId));
                        MessageFormatToSend.Add(new SemiObjectBinary(AttributeNameOfMapFormatType, (byte)MapDataFormatTypes.ArrayFormat));
                        MessageFormatToSend.Add(AddSendingMessageByType(AttributeNameOfMessageLength, AttributeValueMessageLength));
                        //
                    }
                    break;
                case MessageTypeToSend.MapDataSend:
                    {
                        MessageFormatToSend.Add(new SemiObjectList(4));
                        MessageFormatToSend.Add(new SemiObjectAscii(AttributeNameOfMaterialId, _paramValue.WaferDataToHandling.WaferId));
                        MessageFormatToSend.Add(new SemiObjectBinary(AttributeNameOfIdType, (byte)MapTypes.WaferId));
                        MessageFormatToSend.Add(new SemiObjectInt2(AttributeNameOfStartingPointXY,
                            new short[] 
                            {
                                (short)_paramValue.WaferDataToHandling.IndexOfStartingX,
                                (short)_paramValue.WaferDataToHandling.IndexOfStartingY 
                            }));
                        MessageFormatToSend.Add(new SemiObjectAscii(AttributeNameOfBinCodeList, _paramValue.WaferDataToHandling.MapData));
                    }
                    break;

                default:
                    return false;
            }


            Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
            return true;
        }

        private SemiObject AddSendingMessageByType(string name, int value)
        {
            switch (ItemFormatForUint)
            {
                case EN_ITEM_FORMAT.UINT4:
                    return new SemiObjectUInt4(name, (uint)value);

                default:
                    return new SemiObjectUInt2(name, (ushort)value);
            }
        }
        #endregion </Methods>
    }
}