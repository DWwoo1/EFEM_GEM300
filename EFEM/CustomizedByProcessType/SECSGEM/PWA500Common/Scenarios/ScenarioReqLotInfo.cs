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
    public class ScenarioReqLotInfoParamValues : ScenarioParamValues
    {
        public ScenarioReqLotInfoParamValues(List<string> values) : base(values)
        {

        }
    }

    public class ScenarioReqLotInfo : ScenarioSendEventThenHandlingSecsMessage
    {
        #region <Constructors>
        public ScenarioReqLotInfo(string name, long eventId, List<long> variables,
            long streamToReceive, long funcToReceive, bool useRemoteCommandConfirmation, uint timeOut = 10000)
            : base(name, eventId, variables, streamToReceive, funcToReceive, useRemoteCommandConfirmation, timeOut)
        {
        }
        #endregion </Constructors>

        #region <Fields>
        private const string ObjectIdFieldName = "OBJID";
        private const string AttributeIdFieldName = "ATTRID";
        private const string AttributeIdFieldNameLotId = "LOTID";
        private const string AttributeIdFieldNamePartId = "PARTID";
        private const string AttributeIdFieldNameLotType = "LOTTYPE";
        private const string AttributeIdFieldNameStepSeq = "STEPSEQ";
        private const string AttributeIdFieldNameQty = "QTY";
        private const string AttributeIdFieldNameRecipeId = "RECIPEID";
        private const string ObjectAckFieldName = "OBJACK";

        private const int ObjectNameFieldIndex = 2;
        private const int ObjectIdFieldIndex = 4;
        private const int AttributeFieldIndexPartId = 8;
        private const int AttributeFieldIndexLotType = 11;
        private const int AttributeFieldIndexStepSeq = 14;
        private const int AttributeFieldIndexQty = 17;
        private const int AttributeFieldIndexRecipeId = 20;

        private string _lotId = string.Empty;
        private string _partId = string.Empty;
        private string _stepSeq = string.Empty;
        private string _lotType = string.Empty;
        private string _recipeId = string.Empty;
        private int _lotQty = 0;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>
        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValue = paramValues as ScenarioReqLotInfoParamValues;
        }
        public override Dictionary<string, string> GetResultData()
        {
            Dictionary<string, string> resultData = new Dictionary<string, string>
            {
                ["LotId"] = _lotId,
                ["PartId"] = _partId,
                ["StepSeq"] = _stepSeq,
                ["LotType"] = _lotType,
                ["RecipeId"] = _recipeId,
                ["LotQty"] = _lotQty.ToString()
            };

            return resultData;
        }
        protected override bool UpdateReceivedSecsMessage(List<SemiObject> listOfReceive)
        {
            _receiveMessageFormat = listOfReceive;

            if (!(_receiveMessageFormat[ObjectNameFieldIndex] is SemiObjectAscii objName))
                return false;

            if (false == objName.GetValue().Equals(NameToReceive))
                return false;

            if (_paramValue != null)
            {
                if (!(_receiveMessageFormat[ObjectIdFieldIndex] is SemiObjectAscii objId))      // LotId
                    return false;

                // 보고한 LotId와 받은 LotId가 다르면 리턴(동시 발생 시 정합성을 위해 추가)
                string lotId = objId.GetValue();
                if (false == _paramValue.VariableValues[0].Equals(lotId))
                    return false;

                _lotId = lotId;

                return true;
            }

            return false;
        }

        protected override bool MakeMessageToSend()
        {
            if (!(_receiveMessageFormat[ObjectIdFieldIndex] is SemiObjectAscii objId))      // LotId
                return false;
            string objIdName = objId.GetValue();
            _lotId = objId.GetValue();

            MessageFormatToSend = new List<SemiObject>();
            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectList(1));
            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(ObjectIdFieldName, objIdName));

            MessageFormatToSend.Add(new SemiObjectList(5));

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNamePartId));
            if (!(_receiveMessageFormat[11] is SemiObjectAscii partId))
                return false;
            MessageFormatToSend.Add(partId);
            _partId = partId.GetValue();

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameLotType));
            if (!(_receiveMessageFormat[14] is SemiObjectAscii lotType))
                return false;
            MessageFormatToSend.Add(lotType);
            _lotType = lotType.GetValue();

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameStepSeq));
            if (!(_receiveMessageFormat[17] is SemiObjectAscii stepSeq))
                return false;
            MessageFormatToSend.Add(stepSeq);
            _stepSeq = stepSeq.GetValue();

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameQty));
            if (!(_receiveMessageFormat[20] is SemiObjectAscii qty))
                return false;
            MessageFormatToSend.Add(qty);
            int.TryParse(qty.GetValue(), out _lotQty);

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameRecipeId));
            if (!(_receiveMessageFormat[23] is SemiObjectAscii recipeId))
                return false;
            MessageFormatToSend.Add(recipeId);
            _recipeId = recipeId.GetValue();

            /*
            if (!(_receiveMessageFormat[AttributeFieldIndexPartId] is SemiObjectAscii partId))
                return false;
            MessageFormatToSend.Add(partId);
            _partId = partId.GetValue();

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameLotType));
            if (!(_receiveMessageFormat[AttributeFieldIndexLotType] is SemiObjectAscii lotType))
                return false;
            MessageFormatToSend.Add(lotType);
            _lotType = lotType.GetValue();

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameStepSeq));
            if (!(_receiveMessageFormat[AttributeFieldIndexStepSeq] is SemiObjectAscii stepSeq))
                return false;
            MessageFormatToSend.Add(stepSeq);
            _stepSeq = stepSeq.GetValue();

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameQty));
            if (!(_receiveMessageFormat[AttributeFieldIndexQty] is SemiObjectAscii qty))
                return false;
            MessageFormatToSend.Add(qty);
            int.TryParse(qty.GetValue(), out _lotQty);

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameRecipeId));
            if (!(_receiveMessageFormat[AttributeFieldIndexRecipeId] is SemiObjectAscii recipeId))
                return false;
            MessageFormatToSend.Add(recipeId);
            _recipeId = recipeId.GetValue();
*/
            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectUInt(ObjectAckFieldName, 0));
            MessageFormatToSend.Add(new SemiObjectList(0));

            return true;
        }
        #endregion </Methods>
    }
}
