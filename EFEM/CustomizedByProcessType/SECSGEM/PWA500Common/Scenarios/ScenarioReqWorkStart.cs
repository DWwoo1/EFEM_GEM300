using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    public class ScenarioReqWorkStartParamValues : ScenarioParamValues
    {
        public ScenarioReqWorkStartParamValues(List<string> values, bool useEventHandling, WaferMapData mapData) : base(values)
        {
            WaferDataToHandling = mapData;
            UseEventHandling = useEventHandling;
        }

        public readonly WaferMapData WaferDataToHandling;
        public readonly bool UseEventHandling;
    }

    public class ScenarioReqWorkStart : AutoScenarioBase
    {
        #region <Constructors>
        public ScenarioReqWorkStart(string name, long eventId, List<long> variables,
            long streamToHandling, long funcToSetupRequest, long funcToDataRequest, uint timeOut = 10000)
            : base(name, timeOut)
        {
            _receiveMessageFormat = new List<SemiObject>();
            MessageFormatToSend = new List<SemiObject>();

            ReceiveStream = streamToHandling;
            ReceiveFunction = funcToSetupRequest;
            FunctionToSendWaferMapSetup = funcToSetupRequest;
            FunctionToSendWaferMapData = funcToDataRequest;

            FunctionToReceivedWaferMapSetup = funcToSetupRequest + 1;
            FunctionToReceivedWaferMapData = funcToDataRequest + 1;

            _eventId = eventId;
            _variables = new List<long>(variables);
        }
        #endregion </Constructors>

        #region <Fields>
        private ScenarioReqWorkStartParamValues _paramValue = null;
        private List<SemiObject> _receiveMessageFormat = null;
        private readonly List<SemiObject> MessageFormatToSend = null;

        private long _eventId;
        private List<long> _variables;
        private readonly long FunctionToSendWaferMapSetup;
        private readonly long FunctionToSendWaferMapData;

        private const string AttributeNameOfMaterialId = "MID";
        private const string AttributeNameOfIdType = "IDTYP";
        private const string AttributeNameOfMapFormatType = "MAPFT";
        private const string AttributeNameOfFlatNotchLocation = "FNLOC";
        private const string AttributeNameOfFilmFrameLocation = "FFROT";
        private const string AttributeNameOfOriginLocation = "ORLOC";
        private const string AttributeNameOfProcessAccess = "PRAXI";
        private const string AttributeNameOfBinCodeEquivalents = "BCEQU";
        private const string AttributeNameOfNullBinCode = "NULBC";

        private const ushort AttributeValueFilmFrameLocation = 0;
        private const string AttributeValueBinCodeEquivalents = "012345678ab";
        private const string AttributeValueNullBinCode = ".";

        //private string _substrateId;
        //private double _angle;
        //private int _countRow;
        //private int _countCol;
        //private int _qty;
        //private string _mapData;
        #endregion </Fields>

        #region <Types>
        enum MessageTypeToSend
        {
            MapSetupData,       // 12, 3
            MapData,            // 12, 15
        }
        enum ScenarioSeq
        {
            INIT = 0,
            SEND_EVENT = 100,
            SEND_SECSMESSAGE_MAPDATA_SETUP_REQUEST = 200,
            SEND_SECSMESSAGE_MAPDATA_REQUEST = 300,
            FINISH = 1000,
        }
        #endregion </Types>

        #region <Properties>
        public long FunctionToReceivedWaferMapSetup { get; private set; }
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
        //            _paramValue = new ScenarioReqWorkStartParamValues(null, new WaferMapData());
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
                        if (_paramValue != null && false == _paramValue.UseEventHandling)
                        {
                            _seqNum = (int)ScenarioSeq.SEND_SECSMESSAGE_MAPDATA_SETUP_REQUEST;
                        }
                        else
                        {
                            _seqNum = (int)ScenarioSeq.SEND_EVENT;
                        }
                    }
                    break;

                case (int)ScenarioSeq.SEND_EVENT:
                    {
                        _gemHandler.SendEvent(_eventId, _variables.ToArray(), _paramValue.VariableValues);
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

                        _seqNum = (int)ScenarioSeq.SEND_SECSMESSAGE_MAPDATA_SETUP_REQUEST;
                    }
                    break;

                case (int)ScenarioSeq.SEND_SECSMESSAGE_MAPDATA_SETUP_REQUEST:
                    {
                        if (false == SendSecsMessageMapData(MessageTypeToSend.MapSetupData))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }
                        _gemHandler.SendUserDefinedSecsMessage(ReceiveStream, FunctionToSendWaferMapSetup, MessageFormatToSend);
                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)ScenarioSeq.SEND_SECSMESSAGE_MAPDATA_SETUP_REQUEST + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        switch (Permission)
                        {
                            case EN_SCENARIO_PERMISSION_RESULT.OK:
                                {
                                    // 2024.12.31. jhlim [ADD] 플래그 초기화 누락
                                    Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
                                    _seqNum = (int)ScenarioSeq.SEND_SECSMESSAGE_MAPDATA_REQUEST;
                                }
                                break;

                            case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                    }
                    break;
                
                case (int)ScenarioSeq.SEND_SECSMESSAGE_MAPDATA_REQUEST:
                    {
                        if (false == SendSecsMessageMapData(MessageTypeToSend.MapData))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                        _gemHandler.SendUserDefinedSecsMessage(ReceiveStream, FunctionToSendWaferMapData, MessageFormatToSend);
                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)ScenarioSeq.SEND_SECSMESSAGE_MAPDATA_REQUEST + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

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
            _paramValue = paramValues as ScenarioReqWorkStartParamValues;
        }

        public bool UpdateReceivedSecsMessage(long function, List<SemiObject> listOfReceive)
        {
            _receiveMessageFormat = listOfReceive;

            if (function == FunctionToReceivedWaferMapSetup)
            {
                if (_receiveMessageFormat.Count != 17)
                    return false;
               
                // MID
                if (!(_receiveMessageFormat[1] is SemiObjectAscii materialId))
                    return false;
                
                _paramValue.WaferDataToHandling.WaferId = materialId.GetValue();
                
                #region 임시
                //_paramValue.WaferDataToHandling.WaferId = "CoreSubstrate1";
                #endregion

                //if (false == materialId.GetValue().Equals(_paramValue.WaferDataToHandling.WaferId))
                //    return false;

                // IDTYP
                if (!(_receiveMessageFormat[2] is SemiObjectBinary idType))
                    return false;
                //if (false == idType.GetValue().Equals((byte)MapTypes.WaferId))
                //    return false;

                // FNLOC
                if (!(_receiveMessageFormat[3] is SemiObjectUInt2 fnLoc))
                    return false;
                _paramValue.WaferDataToHandling.Angle = fnLoc.GetValue();
                //if (false == fnLoc.GetValue().Equals((ushort)_paramValue.WaferDataToHandling.Angle))
                //    return false;

                // ORLOC
                if (!(_receiveMessageFormat[4] is SemiObjectBinary orLoc))
                    return false;
                //if (false == orLoc.GetValue().Equals((byte)MapTypes.WaferId))
                //    return false;

                // RPOSEL pass(5)

                // RefXYList(List : 6)
                if (_receiveMessageFormat[7] is SemiObjectInt2 ||
                    _receiveMessageFormat[7] is SemiObjectUInt2)
                {
                    if(_receiveMessageFormat[7] is SemiObjectInt2)
                    {
                        var refXy = _receiveMessageFormat[7] as SemiObjectInt2;

                        _paramValue.WaferDataToHandling.IndexOfRefX = refXy.GetValues()[0];
                        _paramValue.WaferDataToHandling.IndexOfRefY = refXy.GetValues()[1];
                    }
                    else
                    {
                        var refXy = _receiveMessageFormat[7] as SemiObjectUInt2;

                        _paramValue.WaferDataToHandling.IndexOfRefX = refXy.GetValues()[0];
                        _paramValue.WaferDataToHandling.IndexOfRefY = refXy.GetValues()[1];
                    }
                }
                else
                    return false;

                // DUTMS pass(8)
                // XDIES pass? (9)
                // YDIES pass? (10)

                // ROWCT
                //if (!(_receiveMessageFormat[11] is SemiObjectUInt2 rowCount))
                //    return false;
                //_paramValue.WaferDataToHandling.CountOfRow = rowCount.GetValue();
                if (_receiveMessageFormat[11] is SemiObjectUInt2 ||
                    _receiveMessageFormat[11] is SemiObjectUInt4)
                {
                    if (_receiveMessageFormat[11] is SemiObjectUInt2)
                    {
                        var rowCount = _receiveMessageFormat[11] as SemiObjectUInt2;

                        _paramValue.WaferDataToHandling.CountOfRow = rowCount.GetValue();
                    }
                    else
                    {
                        var rowCount = _receiveMessageFormat[11] as SemiObjectUInt4;

                        _paramValue.WaferDataToHandling.CountOfRow = (int)rowCount.GetValue();
                    }
                }
                else
                    return false;

                // COLCT
                //if (!(_receiveMessageFormat[12] is SemiObjectUInt2 colCount))
                //    return false;
                //_paramValue.WaferDataToHandling.CountOfCol = colCount.GetValue();
                if (_receiveMessageFormat[12] is SemiObjectUInt2 ||
                    _receiveMessageFormat[12] is SemiObjectUInt4)
                {
                    if (_receiveMessageFormat[12] is SemiObjectUInt2)
                    {
                        var colCount = _receiveMessageFormat[12] as SemiObjectUInt2;

                        _paramValue.WaferDataToHandling.CountOfCol = colCount.GetValue();
                    }
                    else
                    {
                        var colCount = _receiveMessageFormat[12] as SemiObjectUInt4;

                        _paramValue.WaferDataToHandling.CountOfCol = (int)colCount.GetValue();
                    }
                }
                else
                    return false;

                // PRODCT
                //if (!(_receiveMessageFormat[13] is SemiObjectUInt2 processDies))
                //    return false;
                //_paramValue.WaferDataToHandling.CountOfProcessDies = processDies.GetValue();
                if (_receiveMessageFormat[13] is SemiObjectUInt2 ||
                    _receiveMessageFormat[13] is SemiObjectUInt4)
                {
                    if (_receiveMessageFormat[13] is SemiObjectUInt2)
                    {
                        var processDies = _receiveMessageFormat[13] as SemiObjectUInt2;

                        _paramValue.WaferDataToHandling.CountOfProcessDies = processDies.GetValue();
                    }
                    else
                    {
                        var processDies = _receiveMessageFormat[13] as SemiObjectUInt4;

                        _paramValue.WaferDataToHandling.CountOfProcessDies = (int)processDies.GetValue();
                    }
                }
                else
                    return false;

                // BCEQU
                if (!(_receiveMessageFormat[14] is SemiObjectAscii bceQu))
                    return false;
                
                //if (false == bceQu.GetValue().Equals(AttributeValueBinCodeEquivalents))
                //    return false;

                // NULBC
                if (!(_receiveMessageFormat[15] is SemiObjectAscii nulBc))
                    return false;

                //if (false == nulBc.GetValue().Equals(AttributeValueNullBinCode))
                //    return false;
                // MLCL pass (16)
            }
            else
            {
                if (_receiveMessageFormat.Count != 5)
                    return false;

                // MID
                if (!(_receiveMessageFormat[1] is SemiObjectAscii materialId))
                    return false;
                if (false == materialId.GetValue().Equals(_paramValue.WaferDataToHandling.WaferId))
                    return false;

                // IDTYP
                if (!(_receiveMessageFormat[2] is SemiObjectBinary idType))
                    return false;
                if (false == idType.GetValue().Equals((byte)MapTypes.WaferId))
                    return false;

                // STRPxy
                if (!(_receiveMessageFormat[3] is SemiObjectInt2 startingXy))
                    return false;
                if (startingXy.GetValues().Length != 2)
                    return false;
                _paramValue.WaferDataToHandling.IndexOfStartingX = startingXy.GetValues()[0];
                _paramValue.WaferDataToHandling.IndexOfStartingY = startingXy.GetValues()[1];

                // BINLT
                if (!(_receiveMessageFormat[4] is SemiObjectAscii binValue))
                    return false;

                string mapData = binValue.GetValue().Replace(AttributeValueNullBinCode, _paramValue.WaferDataToHandling.NullBinCode);
                _paramValue.WaferDataToHandling.MapData = mapData;// binValue.GetValue();                
            }

            Permission = EN_SCENARIO_PERMISSION_RESULT.OK;
            return true;
        }
        
        private bool SendSecsMessageMapData(MessageTypeToSend messageType)
        {
            MessageFormatToSend.Clear();
            switch (messageType)
            {
                case MessageTypeToSend.MapSetupData:
                    {
                        MessageFormatToSend.Add(new SemiObjectList(9));
                        MessageFormatToSend.Add(new SemiObjectAscii(AttributeNameOfMaterialId, _paramValue.WaferDataToHandling.WaferId));
                        MessageFormatToSend.Add(new SemiObjectBinary(AttributeNameOfIdType, (byte)MapTypes.WaferId));
                        MessageFormatToSend.Add(new SemiObjectBinary(AttributeNameOfMapFormatType, (byte)MapDataFormatTypes.ArrayFormat));
                        MessageFormatToSend.Add(new SemiObjectUInt2(AttributeNameOfFlatNotchLocation, (ushort)_paramValue.WaferDataToHandling.Angle));
                        MessageFormatToSend.Add(new SemiObjectUInt2(AttributeNameOfFilmFrameLocation, AttributeValueFilmFrameLocation));
                        MessageFormatToSend.Add(new SemiObjectBinary(AttributeNameOfOriginLocation, (byte)OriginLocationTypes.UpperLeft));
                        MessageFormatToSend.Add(new SemiObjectBinary(AttributeNameOfProcessAccess, 0));
                        MessageFormatToSend.Add(new SemiObjectAscii(AttributeNameOfBinCodeEquivalents, AttributeValueBinCodeEquivalents));                       
                        MessageFormatToSend.Add(new SemiObjectAscii(AttributeNameOfNullBinCode, AttributeValueNullBinCode));
                    }
                    break;
                case MessageTypeToSend.MapData:
                    {
                        MessageFormatToSend.Add(new SemiObjectList(2));
                        MessageFormatToSend.Add(new SemiObjectAscii(AttributeNameOfMaterialId, _paramValue.WaferDataToHandling.WaferId));
                        MessageFormatToSend.Add(new SemiObjectBinary(AttributeNameOfIdType, (byte)MapTypes.WaferId));
                    }
                    break;
                default:
                    return false;
            }

            
            Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
            return true;
        }
        public override Dictionary<string, string> GetResultData()
        {
            Dictionary<string, string> resultData = new Dictionary<string, string>();
            resultData[RequestDownloadMapFileKeys.KeyResultSubstrateId] = _paramValue.WaferDataToHandling.WaferId;
            resultData[RequestDownloadMapFileKeys.KeyResultAngle] = _paramValue.WaferDataToHandling.Angle.ToString();
            resultData[RequestDownloadMapFileKeys.KeyResultCountRow] = _paramValue.WaferDataToHandling.CountOfRow.ToString();
            resultData[RequestDownloadMapFileKeys.KeyResultCountCol] = _paramValue.WaferDataToHandling.CountOfCol.ToString();
            resultData[RequestDownloadMapFileKeys.KeyResultQty] = _paramValue.WaferDataToHandling.CountOfProcessDies.ToString();
            resultData[RequestDownloadMapFileKeys.KeyResultStartingX] = _paramValue.WaferDataToHandling.IndexOfStartingX.ToString();
            resultData[RequestDownloadMapFileKeys.KeyResultStartingY] = _paramValue.WaferDataToHandling.IndexOfStartingY.ToString();
            resultData[RequestDownloadMapFileKeys.KeyResultReferenceX] = _paramValue.WaferDataToHandling.IndexOfRefX.ToString();
            resultData[RequestDownloadMapFileKeys.KeyResultReferenceY] = _paramValue.WaferDataToHandling.IndexOfRefY.ToString();
            resultData[RequestDownloadMapFileKeys.KeyResultMapData] = _paramValue.WaferDataToHandling.MapData;
            
            return resultData;
        }
        #endregion </Methods>
    }
}