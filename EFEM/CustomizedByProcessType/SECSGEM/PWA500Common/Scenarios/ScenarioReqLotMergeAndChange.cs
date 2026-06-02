using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    public class ScenarioReqLotMergeAndChangeParamValues : ScenarioParamValues
    {
        public ScenarioReqLotMergeAndChangeParamValues(string lotIdFromFirstSubstrate, string carrierId, List<string> valuesForMerge, List<string> valuesForChange) : base(valuesForMerge)
        {
            LotIdFromFirstSubstrate = lotIdFromFirstSubstrate;
            CarrierId = carrierId;
            if (valuesForChange != null)
            {
                ValuesForChange = new List<string>(valuesForChange);
            }
        }
        public string LotIdFromFirstSubstrate { get; private set; }
        public string CarrierId { get; private set; }
        public List<string> ValuesForChange { get; private set; }
    }

    public class ScenarioReqLotMergeAndChange : AutoScenarioBase
    {
        #region <Constructors>
        public ScenarioReqLotMergeAndChange(string name,
            Dictionary<string, ObjectNames> objectNames,
            long eventIdForMerge,
            List<long> variablesForMerge,
            long eventIdForChange,
            List<long> variablesForChange,
            uint timeOut = 10000)
            : base(name, timeOut)
        {
            _receiveMessageFormat = new List<SemiObject>();
            MessageFormatToSend = new List<SemiObject>();

            ReceiveStream = 14;
            ReceiveFunction = 3;

            StreamToSend = 14;
            FunctionToSend = 4;

            ObjectNameList = new Dictionary<string, ObjectNames>(objectNames);

            EventIdForMerge = eventIdForMerge;
            VariablesForMerge = new List<long>(variablesForMerge);
            
            EventIdForChange = eventIdForChange;
            if (EventIdForChange >= 0)
            {
                VariablesForChange = new List<long>(variablesForChange);
            }
        }
        #endregion </Constructors>

        #region <Fields>
        private ScenarioReqLotMergeAndChangeParamValues _paramValue = null;
        private List<SemiObject> _receiveMessageFormat = null;        
        private readonly Dictionary<string, ObjectNames> ObjectNameList = null;

        private readonly long EventIdForMerge;
        private readonly long EventIdForChange;
        private readonly List<long> VariablesForMerge = null;
        private readonly List<long> VariablesForChange = null;

        private const string ObjectIdFieldName = "OBJID";
        private const string AttributeIdFieldName = "ATTRID";
        private const string AttributeIdFieldNameMergedLotId = "LOTID";
        private const string AttributeIdFieldNameCarrierId = "CARRIERID";
        private const string AttributeIdFieldNameWaferId = "WAFERID";
        private const string ObjectAckFieldName = "OBJACK";
        private const int FieldIndexObjectName = 2;
        private const int FieldIndexObjectId = 4;
        private const int FieldIndexAttributeMergedLotId = 8;
        private const int FieldIndexAttributeCarrierId = 11;
        private const int FieldIndexAttributeWaferId = 14;

        private string _resultLotId;
        #endregion </Fields>

        #region <Types>
        enum ScenarioSeq
        {
            INIT = 0,
            SEND_EVENT_MERGE = 100,
            WAIT_FOR_COMPLETION_TO_MERGE = 200,
            WAIT_FOR_REMOTE_COMMAND_RESULT_FOR_MERGE = 300,
            SEND_EVENT_CHANGE = 400,
            WAIT_FOR_COMPLETION_TO_CHANGE = 500,
            WAIT_FOR_REMOTE_COMMAND_RESULT_FOR_CHANGE = 600,

            FINISH = 1000,
        }
        #endregion </Types>

        #region <Properties>
        public long EventIdMerge
        {
            get
            {
                return EventIdForMerge;
            }
        }
        public long EventIdChange
        {
            get
            {
                return EventIdForChange;
            }
        }
        public List<SemiObject> MessageFormatToSend { get; private set; }
        public long StreamToSend { get; private set; }
        public long FunctionToSend { get; private set; }
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
                        if (_paramValue == null)
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                        else
                        {
                            _seqNum = (int)ScenarioSeq.SEND_EVENT_MERGE;
                        }
                    }
                    break;

                case (int)ScenarioSeq.SEND_EVENT_MERGE:
                    {
                        if (false == UseTargetObjectScenario(ObjectNames.LOT_MERGE))
                        {
                            _seqNum = (int)ScenarioSeq.SEND_EVENT_CHANGE;
                        }
                        else
                        {
                            _gemHandler.SendEvent(EventIdMerge, VariablesForMerge.ToArray(), _paramValue.VariableValues);
                            SetTickCount(TimeOut);
                            ++_seqNum;
                        }
                    }
                    break;

                case (int)ScenarioSeq.SEND_EVENT_MERGE + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        if (false == _gemHandler.IsSendingEventCompleted(EventIdMerge))
                            break;
                        
                        _seqNum = (int)ScenarioSeq.WAIT_FOR_COMPLETION_TO_MERGE;                        
                    }
                    break;

                case (int)ScenarioSeq.WAIT_FOR_COMPLETION_TO_MERGE:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        switch (Permission)
                        {
                            case EN_SCENARIO_PERMISSION_RESULT.OK:
                                SetTickCount(TimeOut);
                                Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
                                _seqNum = (int)ScenarioSeq.WAIT_FOR_REMOTE_COMMAND_RESULT_FOR_MERGE;
                                break;

                            case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                    }
                    break;

                case (int)ScenarioSeq.WAIT_FOR_REMOTE_COMMAND_RESULT_FOR_MERGE:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        switch (Permission)
                        {
                            case EN_SCENARIO_PERMISSION_RESULT.OK:
                                Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
                                _seqNum = (int)ScenarioSeq.SEND_EVENT_CHANGE;
                                break;

                            case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                    }
                    break;

                case (int)ScenarioSeq.SEND_EVENT_CHANGE:
                    {
                        if (false == UseTargetObjectScenario(ObjectNames.LOT_MERGE))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                        }
                        else
                        {
                            if(EventIdChange < 0 || VariablesForChange == null)
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);

                            _gemHandler.SendEvent(EventIdChange, VariablesForChange.ToArray(), _paramValue.ValuesForChange.ToArray());
                            SetTickCount(TimeOut);
                            ++_seqNum;
                        }
                    }
                    break;

                case (int)ScenarioSeq.SEND_EVENT_CHANGE + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        if (false == _gemHandler.IsSendingEventCompleted(EventIdChange))
                            break;

                        _seqNum = (int)ScenarioSeq.WAIT_FOR_COMPLETION_TO_CHANGE;
                    }
                    break;

                case (int)ScenarioSeq.WAIT_FOR_COMPLETION_TO_CHANGE:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        switch (Permission)
                        {
                            case EN_SCENARIO_PERMISSION_RESULT.OK:
                                SetTickCount(TimeOut);
                                Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
                                _seqNum = (int)ScenarioSeq.WAIT_FOR_REMOTE_COMMAND_RESULT_FOR_CHANGE;
                                break;

                            case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                    }
                    break;

                case (int)ScenarioSeq.WAIT_FOR_REMOTE_COMMAND_RESULT_FOR_CHANGE:
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
            _paramValue = paramValues as ScenarioReqLotMergeAndChangeParamValues;
        }

        public sealed override bool UpdateReceiveMessage(List<SemiObject> listOfReceive)
        {
            ObjectNames objectName;
            if (UpdateReceivedSecsMessage(listOfReceive, out objectName))
            {
                if (MakeSecsMessageToSend(objectName))
                {
                    Permission = EN_SCENARIO_PERMISSION_RESULT.OK;
                }
                else
                {
                    Permission = EN_SCENARIO_PERMISSION_RESULT.ERROR;
                }

                return true;
            }

            return false;
        }

        public bool UpdateReceivedSecsMessage(List<SemiObject> listOfReceive, out ObjectNames objectName)
        {
            _receiveMessageFormat = listOfReceive;
            objectName = ObjectNames.LOT_MERGE;

            if (!(_receiveMessageFormat[FieldIndexObjectName] is SemiObjectAscii objName))
                return false;

            if (false == ObjectNameList.TryGetValue(objName.GetValue(), out objectName))
                return false;

            if (_paramValue != null)
            {
                if (!(_receiveMessageFormat[FieldIndexAttributeMergedLotId] is SemiObjectAscii recvlotId))
                    return false;

                // 보고한 LotId와 받은 LotId가 다르면 리턴(동시 발생 시 정합성을 위해 추가)
                string lotId = recvlotId.GetValue();

                switch (objectName)
                {
                    case ObjectNames.LOT_MERGE:
                        {
                            if (false == _paramValue.LotIdFromFirstSubstrate.Equals(lotId))
                                return false;
                        }
                        break;
                    case ObjectNames.LOT_ID_CHANGE:
                        // 할게 없다.
                        break;
                    default:
                        break;
                }                

                _resultLotId = lotId;
                
                return true;
            }

            return false;            
        }       
        private bool MakeSecsMessageToSend(ObjectNames objectName)
        {
            if (!(_receiveMessageFormat[FieldIndexObjectId] is SemiObjectAscii objId))
                return false;
            string objIdName = objId.GetValue();

            MessageFormatToSend.Clear();
            switch (objectName)
            {
                case ObjectNames.LOT_MERGE:
                    {
                        MessageFormatToSend.Add(new SemiObjectList(2));
                        MessageFormatToSend.Add(new SemiObjectList(1));
                        MessageFormatToSend.Add(new SemiObjectList(2));
                        MessageFormatToSend.Add(new SemiObjectAscii(ObjectIdFieldName, objIdName));

                        MessageFormatToSend.Add(new SemiObjectList(2));

                        MessageFormatToSend.Add(new SemiObjectList(2));
                        MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameMergedLotId));        // 병합된 LotId
                        if (!(_receiveMessageFormat[FieldIndexAttributeMergedLotId] is SemiObjectAscii lotId))
                            return false;
                        MessageFormatToSend.Add(lotId);

                        MessageFormatToSend.Add(new SemiObjectList(2));
                        MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameCarrierId));
                        if (!(_receiveMessageFormat[FieldIndexAttributeCarrierId] is SemiObjectAscii carrierId))
                            return false;
                        MessageFormatToSend.Add(carrierId);

                        //MessageFormatToSend.Add(new SemiObjectList(2));
                        //MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameWaferId));
                        //if (!(_receiveMessageFormat[FieldIndexAttributeWaferId] is SemiObjectAscii waferId))
                        //    return false;
                        //MessageFormatToSend.Add(waferId);

                        MessageFormatToSend.Add(new SemiObjectList(2));
                        MessageFormatToSend.Add(new SemiObjectUInt(ObjectAckFieldName, 0));
                        MessageFormatToSend.Add(new SemiObjectList(0));
                    }
                    break;
                case ObjectNames.LOT_ID_CHANGE:
                    {
                        MessageFormatToSend.Add(new SemiObjectList(2));
                        MessageFormatToSend.Add(new SemiObjectList(1));
                        MessageFormatToSend.Add(new SemiObjectList(2));
                        MessageFormatToSend.Add(new SemiObjectAscii(ObjectIdFieldName, objIdName));

                        MessageFormatToSend.Add(new SemiObjectList(2));

                        MessageFormatToSend.Add(new SemiObjectList(2));
                        MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameMergedLotId));        // 병합된 LotId
                        if (!(_receiveMessageFormat[FieldIndexAttributeMergedLotId] is SemiObjectAscii lotId))
                            return false;
                        MessageFormatToSend.Add(lotId);
                        
                        MessageFormatToSend.Add(new SemiObjectList(2));
                        MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameCarrierId));
                        if (!(_receiveMessageFormat[FieldIndexAttributeCarrierId] is SemiObjectAscii carrierId))
                            return false;
                        MessageFormatToSend.Add(carrierId);

                        //MessageFormatToSend.Add(new SemiObjectList(2));
                        //MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameWaferId));
                        //if (!(_receiveMessageFormat[FieldIndexAttributeWaferId] is SemiObjectAscii waferId))
                        //    return false;
                        //MessageFormatToSend.Add(waferId);

                        MessageFormatToSend.Add(new SemiObjectList(2));
                        MessageFormatToSend.Add(new SemiObjectUInt(ObjectAckFieldName, 0));
                        MessageFormatToSend.Add(new SemiObjectList(0));
                    }
                    break;

                default:
                    return false;
            }
           
            return true;
        }
        private bool UseTargetObjectScenario(ObjectNames objectName)
        {
            foreach (var item in ObjectNameList)
            {
                if (item.Value.Equals(objectName))
                    return true;
            }

            return false;
        }
        public override Dictionary<string, string> GetResultData()
        {
            var resultData = new Dictionary<string, string>
            {
                [LotMergeKeys.KeyResultLotId] = _resultLotId
            };

            return resultData;
        }
        #endregion </Methods>
    }
}