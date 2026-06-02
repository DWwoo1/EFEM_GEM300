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
    public class ScenarioReqWaferSplitFromLotParamValues : ScenarioParamValues
    {
        public ScenarioReqWaferSplitFromLotParamValues(List<string> values) : base(values)
        {

        }
    }

    public class ScenarioReqWaferSplitFromLot : ScenarioSendEventThenHandlingSecsMessage
    {
        #region <Constructors>
        public ScenarioReqWaferSplitFromLot(string name, long eventId, List<long> variables,
            long streamToReceive, long funcToReceive, bool useRemoteCommandConfirmation, uint timeOut = 10000)
            : base(name, eventId, variables, streamToReceive, funcToReceive, useRemoteCommandConfirmation, timeOut)
        {
        }
        #endregion </Constructors>

        #region <Fields>
        private const string ObjectIdFieldName = "OBJID";
        private const string AttributeIdFieldName = "ATTRID";
        private const string AttributeIdFieldNamePartId = "PARTID";
        private const string AttributeIdFieldNameLotType = "LOTTYPE";
        private const string AttributeIdFieldNameStepSeq = "STEPSEQ";
        private const string AttributeIdFieldNameQty = "QTY";
        private const string AttributeIdFieldNameRecipeId = "RECIPEID";
        private const string ObjectAckFieldName = "OBJACK";

        private const int ObjectNameFieldIndex = 2;
        private const int ObjectIdFieldIndex = 4;
        private const int AttributeFieldIndexPartId = 11;
        private const int AttributeFieldIndexLotType = 13;
        private const int AttributeFieldIndexStepSeq = 17;
        private const int AttributeFieldIndexQty = 20;
        private const int AttributeFieldIndexRecipeId = 23;

        private string _receivedPartId;
        private string _splittedLotId;
        private string _splittedQty;
        #endregion </Fields>

        #region <Methods>
        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValue = paramValues as ScenarioReqWaferSplitFromLotParamValues;
        }
        protected override bool UpdateReceivedSecsMessage(List<SemiObject> listOfReceive)
        {
            _receiveMessageFormat = listOfReceive;

            if (!(_receiveMessageFormat[ObjectNameFieldIndex] is SemiObjectAscii objName))
                return false;

            if (false == objName.GetValue().Equals(NameToReceive))
                return false;

            return true;
        }
        protected override bool MakeMessageToSend()
        {
            if (!(_receiveMessageFormat[ObjectIdFieldIndex] is SemiObjectAscii objId))      // LotId
                return false;
            string objIdName = objId.GetValue();

            MessageFormatToSend = new List<SemiObject>();
            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectList(1));
            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(ObjectIdFieldName, objIdName));
            _splittedLotId = objIdName;

            MessageFormatToSend.Add(new SemiObjectList(5));

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNamePartId));
            //if (!(_receiveMessageFormat[11/*AttributeFieldIndexPartId*/] is SemiObjectAscii partId))
            if (!(_receiveMessageFormat[AttributeFieldIndexPartId] is SemiObjectAscii partId))
                return false;
            MessageFormatToSend.Add(partId);
            _receivedPartId = partId.GetValue();

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameLotType));
            if (!(_receiveMessageFormat[AttributeFieldIndexLotType] is SemiObjectAscii lotType))
                return false;
            MessageFormatToSend.Add(lotType);

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameStepSeq));
            if (!(_receiveMessageFormat[AttributeFieldIndexStepSeq] is SemiObjectAscii stepSeq))
                return false;
            MessageFormatToSend.Add(stepSeq);

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameQty));
            //if (!(_receiveMessageFormat[20/*AttributeFieldIndexQty*/] is SemiObjectAscii qty))
            if (!(_receiveMessageFormat[AttributeFieldIndexQty] is SemiObjectAscii qty))
                return false;
            MessageFormatToSend.Add(qty);
            _splittedQty = qty.GetValue();

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameRecipeId));
            //if (!(_receiveMessageFormat[23/*AttributeFieldIndexRecipeId*/] is SemiObjectAscii recipeId))
            if (!(_receiveMessageFormat[AttributeFieldIndexRecipeId] is SemiObjectAscii recipeId))
                return false;
            MessageFormatToSend.Add(recipeId);

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectUInt(ObjectAckFieldName, 0));
            MessageFormatToSend.Add(new SemiObjectList(0));

            return true;
        }

        public override Dictionary<string, string> GetResultData()
        {
            Dictionary<string, string> resultData = new Dictionary<string, string>();
            resultData[AssignSubstrateLotIdKeys.KeyResultLotId] = _splittedLotId;
            resultData[AssignSubstrateLotIdKeys.KeyResultQty] = _splittedQty;
            resultData[AssignSubstrateLotIdKeys.KeyResultPartId] = _receivedPartId;
            
            return resultData;
        }
        #endregion </Methods>
    }
}
