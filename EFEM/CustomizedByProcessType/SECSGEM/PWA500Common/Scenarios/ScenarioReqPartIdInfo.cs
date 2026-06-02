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
    public class ScenarioReqPartIdInfoParamValues : ScenarioParamValues
    {
        public ScenarioReqPartIdInfoParamValues(List<string> values) : base(values)
        {

        }
    }

    public class ScenarioReqPartIdInfo : ScenarioSendEventThenHandlingSecsMessage
    {
        #region <Constructors>
        public ScenarioReqPartIdInfo(string name, long eventId, List<long> variables,
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
        private const string ObjectAckFieldName = "OBJACK";

        private const int FieldIndexObjectName = 2;
        private const int FieldIndexObjectId = 4;
        private const int FieldIndexAttributeMergedLotId = 8;
        private const int FieldIndexAttributePartId = 11;


        private string _lotId;
        private string _partId;
        private string _stepSeq;
        private string _lotType;
        private string _recipeId;
        private int _lotQty;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>
        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValue = paramValues as ScenarioReqPartIdInfoParamValues;
        }
        public override Dictionary<string, string> GetResultData()
        {
            Dictionary<string, string> resultData = new Dictionary<string, string>
            {
                [LotInfoKeys.KeyResultLotId] = _lotId,
                [LotInfoKeys.KeyResultPartId] = _partId
            };

            return resultData;
        }
        protected override bool UpdateReceivedSecsMessage(List<SemiObject> listOfReceive)
        {
            _receiveMessageFormat = listOfReceive;

            if (!(_receiveMessageFormat[FieldIndexObjectName] is SemiObjectAscii objName))
                return false;

            if (false == objName.GetValue().Equals(NameToReceive))
                return false;
           
            if (_paramValue != null)
            {
                if (!(_receiveMessageFormat[FieldIndexObjectId] is SemiObjectAscii objId))      // LotId
                    return false;

                return true;
            }

            return false;
        }

        protected override bool MakeMessageToSend()
        {
            if (!(_receiveMessageFormat[FieldIndexObjectId] is SemiObjectAscii objId))      // LotId
                return false;
            string objIdName = objId.GetValue();
            _lotId = objId.GetValue();

            MessageFormatToSend = new List<SemiObject>();
            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectList(1));
            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(ObjectIdFieldName, objIdName));

            MessageFormatToSend.Add(new SemiObjectList(2));

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNameLotId));        // 병합된 LotId
            if (!(_receiveMessageFormat[FieldIndexAttributeMergedLotId] is SemiObjectAscii lotId))
                return false;
            MessageFormatToSend.Add(lotId);
            _lotId = lotId.GetValue();

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectAscii(AttributeIdFieldName, AttributeIdFieldNamePartId));
            if (!(_receiveMessageFormat[FieldIndexAttributePartId] is SemiObjectAscii partId))
                return false;
            MessageFormatToSend.Add(partId);
            _partId = partId.GetValue();

            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectUInt(ObjectAckFieldName, 0));
            MessageFormatToSend.Add(new SemiObjectList(0));

            return true;
        }
        #endregion </Methods>
    }
}
