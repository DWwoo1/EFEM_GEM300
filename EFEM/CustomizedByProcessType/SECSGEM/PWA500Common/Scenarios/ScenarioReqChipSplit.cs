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
    public class ScenarioReqChipSplitParamValues : ScenarioParamValues
    {
        public ScenarioReqChipSplitParamValues(List<string> values) : base(values)
        {

        }
    }

    public class ScenarioReqChipSplit : ScenarioSendEventThenHandlingSecsMessage
    {
        #region <Constructors>
        public ScenarioReqChipSplit(string name, long eventId, List<long> variables,
            long streamToReceive, long funcToReceive, string attributeId, bool useRemoteCommandConfirmation, uint timeOut = 10000)
            : base(name, eventId, variables, streamToReceive, funcToReceive, useRemoteCommandConfirmation, timeOut)
        {
            _attributeId = attributeId;
        }
        #endregion </Constructors>

        #region <Fields>
        private const string ObjectIdFieldName = "OBJID";
        private const string AttributeIdFieldName = "ATTRID";
        private const string ObjectAckFieldName = "OBJACK";

        private const int ObjectNameFieldIndex = 2;
        private const int ObjectIdFieldIndex = 4;
        private const int AttributeFieldIndexLotId = 8;
        private const int AttributeFieldIndexWaferId = 11;

        private string _attributeId;
        private string _newLotId;
        #endregion </Fields>

        #region <Methods>
        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValue = paramValues as ScenarioReqChipSplitParamValues;
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

            MessageFormatToSend.Add(new SemiObjectList(1));

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, _attributeId));
            if (!(_receiveMessageFormat[AttributeFieldIndexLotId] is SemiObjectAscii lotId))
                return false;
            MessageFormatToSend.Add(lotId);
            
            // 2024.09.26.jhlim [MOD] objId에서 읽어오도록 변경
            _newLotId = objIdName;
            //_newLotId = lotId.GetValue();
            // 2024.09.26.jhlim [END]

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectUInt(ObjectAckFieldName, 0));
            MessageFormatToSend.Add(new SemiObjectList(0));

            return true;
        }

        public override Dictionary<string, string> GetResultData()
        {
            Dictionary<string, string> resultData = new Dictionary<string, string>();
            resultData[AssignBinLotIdKeys.KeyLotId] = _newLotId;

            return resultData;
        }
        #endregion </Methods>
    }
}
