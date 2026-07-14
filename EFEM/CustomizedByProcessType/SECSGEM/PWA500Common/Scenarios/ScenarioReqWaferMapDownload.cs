using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    // TODO : [     dwlim    ] ScenarioReqWaferMapDownload.cs 추가
    public class ScenarioReqWaferMapDownloadParamValues : ScenarioParamValues
    {
        public ScenarioReqWaferMapDownloadParamValues(List<string> values, /*bool useEventHandling,*/ WaferMapData mapData) : base(values)
        {
            WaferDataToHandling = mapData;
            //UseEventHandling = useEventHandling;
        }
        public readonly WaferMapData WaferDataToHandling;
        //public readonly bool UseEventHandling;
    }

    public class ScenarioReqWaferMapDownload : AutoScenarioBase
    {
        #region <Constructor>
        public ScenarioReqWaferMapDownload(string name, long streamToSend, long funcToSend, bool useRemoteCommandConfirmation, uint timeOut = 10000)
            : base(name, timeOut)
        {
            _messageFormatToSend = new List<SemiObject>();

            ReceiveStream = streamToSend;
            ReceiveFunction = funcToSend + 1;

            StreamToSend = streamToSend;
            FunctionToSend = funcToSend;

            //_eventId = eventId;
            //_variables = variables;
        }
        #endregion </Constructor>

        #region <Fields>
        ScenarioReqWaferMapDownloadParamValues _paramValue = null;
        private List<SemiObject> _messageFormatToSend = null;

        //private long _eventId;
        //private List<long> _variables;

        private const string ObjectSpecFieldName = "OBJSPEC";
        private const string ObjectTypeFieldName = "OBTYPE";
        private const string ObjectIdFieldName = "OBJID";

        private const string ObjectTypeFieldValue = "WaferMap";             //임시

        private const string AttributeIdFieldName = "ATTRID";
        private const string AttributeDataFieldName = "ATTRDATA";
        private const string AttributeRelationshipFieldName = "ATTRRELN";

        private const string AttributeIdFieldValue_SubsType = "SubstrateType";       //임시
        private const string AttributeIdFieldValue_MapData = "MapData";            //임시
        private const string AttributeIdFieldValue_Orientation = "Orientation";             //임시
        private const string AttributeDataFieldValue_Wafer = "Wafer";             //임시
        private const string AttributeOriginLocationFieldValue = "UpperLeft";
        private const string AttributeLayoutIdFieldValueWafer = "Die";
        private const string AttributeNullBinFieldValue = ".";
        private const string AttributeOverlayMapNameFieldValue = "DownloadCoreWaferMap";
        private const string AttributeReferenceDeviceFieldValue_First = "FirstDevice";
        private const string AttributeReferenceDeviceFieldValue_FDI = "FDI";
        private const int AttributeRelationshipFieldValue = 0;
        private const int AttributeDataFieldValue_0 = 0;             //임시

        private const int FieldIndexObjectName = 2;
        private const int FieldIndexObjectId = 4;
        private const int FieldIndexAttributeCarrierId = 11;
        private const int FieldIndexAttributeWaferId = 14;
        #endregion </Fields>

        #region <Types>
        private enum EN_SCENARIO_SEQ
        {
            INIT = 0,
            SEND_SECSMESSAGE_MAPDATA_REQUEST = 100,
            WAIT_FOR_PERMISSION = 200,
            FINISH,
        }
        #endregion </Types>

        #region <Properties>
        public long StreamToSend { get; private set; }
        public long FunctionToSend { get; private set; }
        public List<SemiObject> ReceiveMessageFormat { get; set; }
        #endregion </Properties>

        #region <Methods>
        public override EN_SCENARIO_RESULT ExecuteScenario()
        {
            switch (_seqNum)
            {
                case (int)EN_SCENARIO_SEQ.INIT:
                    {
                        Activate = true;
                        InitFlags();
                        if (_paramValue == null)
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                        _seqNum = (int)EN_SCENARIO_SEQ.SEND_SECSMESSAGE_MAPDATA_REQUEST;
                        break;
                    }
                case (int)EN_SCENARIO_SEQ.SEND_SECSMESSAGE_MAPDATA_REQUEST:
                    {
                        if (false == SendSecsMessageReqMapDownload())
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }
                        if (false == _gemHandler.SendUserDefinedSecsMessage(StreamToSend, FunctionToSend, _messageFormatToSend))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                        SetTickCount(TimeOut);
                        _seqNum = (int)EN_SCENARIO_SEQ.WAIT_FOR_PERMISSION;
                        break;
                    }
                case (int)EN_SCENARIO_SEQ.WAIT_FOR_PERMISSION:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }

                        switch (Permission)
                        {
                            case EN_SCENARIO_PERMISSION_RESULT.OK:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                            case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                            default:
                                break;
                        }
                        break;
                    }
                    
                default:
                    return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
            }

            return EN_SCENARIO_RESULT.PROCEED;
        }
        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValue = paramValues as ScenarioReqWaferMapDownloadParamValues;
        }
        public override bool UpdateReceiveMessage(List<SemiObject> listOfReceive)
        {
            ReceiveMessageFormat = listOfReceive;

            // OBJID
            if (!(ReceiveMessageFormat[3] is SemiObjectAscii materialId))
                return false;

            // ATTRID
            if (!(ReceiveMessageFormat[6] is SemiObjectAscii attrid))
                return false;

            // ATTRDATA
            if (!(ReceiveMessageFormat[7] is SemiObjectAscii attrdata))
                return false;

            SemiObjectAscii resultAsciiObjectForXml = ReceiveMessageFormat[7] as SemiObjectAscii;
			string[] xmlDatas = resultAsciiObjectForXml.GetValues();
            string xmlData = resultAsciiObjectForXml.GetValue();
            MapData mapdata = new MapData();
            MapDataControl xmlControl = new MapDataControl();

            mapdata = xmlControl.DeserializeMapData(xmlData);

            if (null == mapdata)
                return false;

            // IDTYP
            if (false == xmlControl.HasOnlyOneSubstrate(mapdata, out MapDataSubstrate mapDataSubstrate))
                return false;

            if (false == xmlControl.HasOnlyOneSubstrateMap(mapdata, out MapDataSubstrateMap mapDataSubstrateMap))
                return false;

            // FNLOC
            if (false == xmlControl.GetAngle(mapDataSubstrateMap, out double angle))
                return false;

            //if (false == fnLoc.GetValue().Equals((ushort)_paramValue.WaferDataToHandling.Angle))
            //    return false;

            // ORLOC
            if (false == xmlControl.GetOriginLocation(mapDataSubstrateMap, out string originLocation))
                return false;

            if (!(AttributeOriginLocationFieldValue == originLocation))
                return false;

            // RPOSEL pass(5)

            // RefXYList(List : 6)
            //if (false == xmlControl.FindReferenceDeviceByName(mapDataSubstrateMap, AttributeReferenceDeviceFieldValue_FDI, out MapDataReferenceDevice refDevice_FDI))
            //    return false;

            // DUTMS pass(8)
            // XDIES pass? (9)
            // YDIES pass? (10)

            // ROWCT
            // COLCT
            if (false == xmlControl.FindLayoutById(mapdata, AttributeLayoutIdFieldValueWafer, out MapDataLayout mapDataLayout))
                return false;

            // PRODCT
            //if (!(_receiveMessageFormat[13] is SemiObjectUInt2 processDies))
            //    return false;
            //_paramValue.WaferDataToHandling.CountOfProcessDies = processDies.GetValue();
            if (false == xmlControl.GetDieCount(mapDataSubstrate, out int dieCount))
                return false;

            // BCEQU
            //if (!(_receiveMessageFormat[14] is SemiObjectAscii bceQu))
            //    return false;

            // NULBC
            // Map Data
            string binCodeMap = xmlControl.GetBinCodeMapByOverlayMapName(mapdata, AttributeOverlayMapNameFieldValue);
            if (null == binCodeMap)
                return false;

            // MLCL pass (16)

            // STRP
            //if (false == xmlControl.FindReferenceDeviceByName(mapDataSubstrateMap, AttributeReferenceDeviceFieldValue_First, out MapDataReferenceDevice refDevice_First))
            //    return false;

            _paramValue.WaferDataToHandling.WaferId = materialId.GetValue();
            _paramValue.WaferDataToHandling.Angle = angle;
            _paramValue.WaferDataToHandling.IndexOfRefX = /*refDevice_FDI.AttributeCoordinates.LogicalCoordinateX*/0;
            _paramValue.WaferDataToHandling.IndexOfRefY = /*refDevice_FDI.AttributeCoordinates.LogicalCoordinateY*/0;
            _paramValue.WaferDataToHandling.CountOfCol = mapDataLayout.AttributeDimension.LogicalCoordinateX;
            _paramValue.WaferDataToHandling.CountOfRow = mapDataLayout.AttributeDimension.LogicalCoordinateY;
            _paramValue.WaferDataToHandling.CountOfProcessDies = dieCount;
            _paramValue.WaferDataToHandling.MapData = binCodeMap;
            _paramValue.WaferDataToHandling.IndexOfStartingX = /*refDevice_First.AttributeCoordinates.LogicalCoordinateX*/0;
            _paramValue.WaferDataToHandling.IndexOfStartingX = /*refDevice_First.AttributeCoordinates.LogicalCoordinateY*/0;

            Permission = EN_SCENARIO_PERMISSION_RESULT.OK;
            return true;
        }

        private bool SendSecsMessageReqMapDownload()
        {
            if (_paramValue == null)
                return false;

            if (!(_paramValue is ScenarioReqWaferMapDownloadParamValues value))
                return false;

            _messageFormatToSend.Clear();

            string objTypeName = "SubstrateMap";

            _messageFormatToSend = new List<SemiObject>();
            _messageFormatToSend.Add(new SemiObjectList(5));
            _messageFormatToSend.Add(new SemiObjectAscii(ObjectSpecFieldName, string.Empty));
            _messageFormatToSend.Add(new SemiObjectAscii(ObjectTypeFieldName, objTypeName));
            _messageFormatToSend.Add(new SemiObjectList(1));
            _messageFormatToSend.Add(new SemiObjectAscii(ObjectIdFieldName, value.WaferDataToHandling.WaferId));
            _messageFormatToSend.Add(new SemiObjectList(2));
            _messageFormatToSend.Add(new SemiObjectList(3));
            _messageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldValue_SubsType));
            _messageFormatToSend.Add(new SemiObjectAscii(AttributeDataFieldName, AttributeDataFieldValue_Wafer));
            _messageFormatToSend.Add(new SemiObjectUInt(AttributeRelationshipFieldName, AttributeRelationshipFieldValue));
            // 2026.06.10 dwlim [ADD] Core Map Download 요청 시 각도 추가
            _messageFormatToSend.Add(new SemiObjectList(3));
            _messageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldValue_Orientation));
            _messageFormatToSend.Add(new SemiObjectInt2(AttributeDataFieldName, AttributeDataFieldValue_0));
            _messageFormatToSend.Add(new SemiObjectUInt(AttributeRelationshipFieldName, AttributeRelationshipFieldValue));
            _messageFormatToSend.Add(new SemiObjectList(1));
            // 2026.06.10 dwlim [END]
            _messageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldValue_MapData));

            Receiving = true;
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
