using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario;
using FrameOfSystem3.SECSGEM.Scenario.Common.Auto;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    public class ScenarioReqWaferIdAssignParamValues : ScenarioParamValues
    {
        public ScenarioReqWaferIdAssignParamValues(List<string> values) : base(values)
        {

        }
    }

    public class ScenarioReqWaferIdAssign : ScenarioSendEventThenHandlingSecsMessage
    {
        #region <Constructors>
        public ScenarioReqWaferIdAssign(string name, long eventId, List<long> variables,
            long eventId2, List<long> variables2,
            long streamToReceive, long funcToReceive, bool useRemoteCommandConfirmation, uint timeOut = 10000)
            : base(name, eventId, variables, streamToReceive, funcToReceive, useRemoteCommandConfirmation, timeOut)
        {
            EventIdConfirm = eventId2;

            if (variables2 != null)
            {
                StatusVariableForConfirm = new List<long>(variables2);
            }
        }
        #endregion </Constructors>

        #region <Fields>
        private const string ObjectIdFieldName = "OBJID";
        private const string AttributeIdFieldName = "ATTRID";
        private const string AttributeIdFieldNameMergedLotId = "LOTID";
        private const string AttributeIdFieldNameSourceWaferId = "SOURCE_WAFERID";      // Ring Id
        private const string AttributeIdFieldNameWaferId = "WAFERID";
        private const string ObjectAckFieldName = "OBJACK";

        private const int FieldIndexObjectName = 2;
        private const int FieldIndexObjectId = 4;
        private const int FieldIndexAttributeMergedLotId = 8;
        //private const int FieldIndexAttributeSourceWaferId = 11;
        private const int FieldIndexAttributeWaferId = 11;

        private readonly long EventIdConfirm;
        private readonly List<long> StatusVariableForConfirm;

        private new ScenarioReqWaferIdAssignParamValues _paramValue;
        private string _assignedWaferId = string.Empty;
        #endregion </Fields>

        #region <Properties>
        public long EventIdConfirmation 
        { 
            get
            {
                return EventIdConfirm;
            }
        }

        #endregion </Properties>

        #region <Methods>
        public override EN_SCENARIO_RESULT ExecuteScenario()
        {
            switch (_seqNum)
            {
                case (int)EN_SCENARIO_SEQ.INIT:
                    {
                        Receiving = true;
                        _seqNum = (int)EN_SCENARIO_SEQ.SEND_EVENT;
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.SEND_EVENT:
                    {
                        _gemHandler.SendEvent(EventId, Variables.ToArray(), _paramValue.VariableValues);
                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.SEND_EVENT + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        if (false == _gemHandler.IsSendingEventCompleted(EventId))
                            break;

                        switch (Permission)     // Receive SecsMessage에 대한 Permission 이다.
                        {
                            case EN_SCENARIO_PERMISSION_RESULT.OK:
                                Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
                                ++_seqNum;
                                break;
                            case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);

                            default:
                                break;
                        }
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.SEND_EVENT + 2:
                    {
                        if (false == UseRemoteCommandConfirmation)
                        {
                            _seqNum = (int)EN_SCENARIO_SEQ.AFTER_PERMISSION;
                        }
                        else
                        {
                            SetTickCount(TimeOut);                            
                            _seqNum = (int)EN_SCENARIO_SEQ.WAIT_FOR_PERMISSION;
                        }
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.WAIT_FOR_PERMISSION:
                    if (IsTickOver(true))
                    {
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                    }

                    switch (Permission)         // RemoteCommande에 대한 Permission 이다. 
                    {
                        case EN_SCENARIO_PERMISSION_RESULT.OK:
                            if (EventIdConfirm < 0)
                            {
                                // Confirmation Event 사용하지 않음
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                            }

                            Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
                            _seqNum = (int)EN_SCENARIO_SEQ.AFTER_PERMISSION;
                            break;

                        case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.AFTER_PERMISSION:
                    {
                        // 데이터가 같은데, Assign wafer id만 추가됐다.
                        List<string> variables = new List<string>(_paramValue.VariableValues)
                        {
                            _assignedWaferId
                        };
                        
                        _gemHandler.SendEvent(EventIdConfirm, StatusVariableForConfirm.ToArray(), variables.ToArray());
                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.AFTER_PERMISSION + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        if (false == _gemHandler.IsSendingEventCompleted(EventId))
                            break;

                        switch (Permission)         // RemoteCommande에 대한 Permission 이다. 
                        {
                            case EN_SCENARIO_PERMISSION_RESULT.OK:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                                //break;

                            case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);

                        }
                        //return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                    }
                    break;

                default:
                    return EN_SCENARIO_RESULT.ERROR;
            }

            return EN_SCENARIO_RESULT.PROCEED;
        }

        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValue = paramValues as ScenarioReqWaferIdAssignParamValues;
        }
        protected override bool UpdateReceivedSecsMessage(List<SemiObject> listOfReceive)
        {
            _receiveMessageFormat = listOfReceive;

            if (!(_receiveMessageFormat[FieldIndexObjectName] is SemiObjectAscii objName))
                return false;

            if (false == objName.GetValue().Equals(NameToReceive))
                return false;

            return true;
        }
        protected override bool MakeMessageToSend()
        { 
            if (!(_receiveMessageFormat[FieldIndexObjectId] is SemiObjectAscii objId))
                return false;
            string objIdName = objId.GetValue();

            MessageFormatToSend = new List<SemiObject>();
            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectList(1));
            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(ObjectIdFieldName, objIdName));

            if (EventIdConfirm >= 0)
            {
                MessageFormatToSend.Add(new SemiObjectList(2));

                MessageFormatToSend.Add(new SemiObjectList(2));
                MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameMergedLotId));        // 병합된 LotId
                if (!(_receiveMessageFormat[FieldIndexAttributeMergedLotId] is SemiObjectAscii lotId))
                    return false;
                MessageFormatToSend.Add(lotId);

                // 2024.09.26. jhlim [DEL] : 서버측과 협의 후 제거
                //MessageFormatToSend.Add(new SemiObjectList(2));
                //MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameSourceWaferId));
                //if (!(_receiveMessageFormat[FieldIndexAttributeSourceWaferId] is SemiObjectAscii sourceWaferId))
                //    return false;
                //MessageFormatToSend.Add(sourceWaferId);
                // 2024.09.26. jhlim [END]

                MessageFormatToSend.Add(new SemiObjectList(2));
                MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameWaferId));
                if (!(_receiveMessageFormat[FieldIndexAttributeWaferId] is SemiObjectAscii waferId))
                    return false;
                MessageFormatToSend.Add(waferId);
                _assignedWaferId = waferId.GetValue();
            }
            else
            {
                MessageFormatToSend.Add(new SemiObjectList(1));

                MessageFormatToSend.Add(new SemiObjectList(2));
                MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameWaferId));
                if (!(_receiveMessageFormat[FieldIndexAttributeMergedLotId] is SemiObjectAscii waferId))
                    return false;
                MessageFormatToSend.Add(waferId);
                _assignedWaferId = waferId.GetValue();
            }

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectUInt(ObjectAckFieldName, 0));
            MessageFormatToSend.Add(new SemiObjectList(0));

            return true;
        }
        public override Dictionary<string, string> GetResultData()
        {
            Dictionary<string, string> resultData = new Dictionary<string, string>();
            resultData["SubstrateId"] = _assignedWaferId;

            return resultData;
        }
        #endregion </Methods>
    }
}
