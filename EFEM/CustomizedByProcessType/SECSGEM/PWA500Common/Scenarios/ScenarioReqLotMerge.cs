using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.DefineSecsGem.PWA500BIN_TP;
using FrameOfSystem3.SECSGEM.Scenario.Common;

namespace FrameOfSystem3.SECSGEM.Scenario.Scenario.PWA500BIN_TP
{
    public class ScenarioReqLotMergeParamValues : ScenarioParamValues
    {
        public ScenarioReqLotMergeParamValues(List<string> values) : base(values)
        {

        }
    }

    public class ScenarioReqLotMerge : ScenarioSendEventThenHandlingSecsMessage
    {
        #region <Constructors>
        public ScenarioReqLotMerge(string name, long eventId, List<long> variables,
            long streamToReceive, long funcToReceive, bool useRemoteCommandConfirmation, uint timeOut = 10000)
            : base(name, eventId, variables, streamToReceive, funcToReceive, useRemoteCommandConfirmation, timeOut)
        {
        }
        #endregion </Constructors>

        #region <Fields>
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

        private string _mergedLotId;
        #endregion </Fields>

        #region <Methods>
        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValue = paramValues as ScenarioReqLotMergeParamValues;
        }
        protected override bool UpdateReceivedSecsMessage(List<SemiObject> listOfReceive)
        {
            _receiveMessageFormat = listOfReceive;

            // TODO : 받는 형식 검증 필요
            if (!(_receiveMessageFormat[FieldIndexObjectName] is SemiObjectAscii objName))
                return false;

            if (false == objName.GetValue().Equals(NameToReceive))
                return false;

            if (_paramValue != null)
            {
                if (!(_receiveMessageFormat[FieldIndexAttributeMergedLotId] is SemiObjectAscii recvlotId))
                    return false;

                // 보고한 LotId와 받은 LotId가 다르면 리턴(동시 발생 시 정합성을 위해 추가)
                string lotId = recvlotId.GetValue();
                if (false == _paramValue.VariableValues[0].Equals(lotId))
                    return false;

                return true;
            }

            return false;
        }
        protected override bool MakeMessageToSend()
        { 
            if (!(_receiveMessageFormat[FieldIndexObjectId] is SemiObjectAscii objId))
                return false;
            string objIdName = objId.GetValue();

            // TODO : 보내는 형식 검증 필요
            MessageFormatToSend = new List<SemiObject>();
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

            _mergedLotId = lotId.GetValue();

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

            return true;
        }

        public override Dictionary<string, string> GetResultData()
        {
            var resultData = new Dictionary<string, string>
            {
                ["LotId"] = _mergedLotId
            };

            return resultData;
        }
        #endregion </Methods>
    }
}
