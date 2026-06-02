using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario.Common
{
    public class ScenarioProceedWithCarrierParamValues : ScenarioParamValues
    {
        public ScenarioProceedWithCarrierParamValues(List<string> values) : base(values)
        {

        }
    }

    public class ScenarioProceedWithCarrier : ScenarioSendEventThenHandlingSecsMessage
    {
        #region <Constructors>
        public ScenarioProceedWithCarrier(long eventId, List<long> variables,
            long streamToReceive, long funcToReceive, bool useRemoteCommandConfirmation, uint timeOut = 10000)
            : base(ActionProceedWithCarrier, eventId, variables, streamToReceive, funcToReceive, useRemoteCommandConfirmation, timeOut)
        {
            ContentMapData = new Dictionary<int, Tuple<string, string>>();
            ContentStatus = new Dictionary<int, uint>();
        }
        #endregion </Constructors>

        #region <Fields>
        private const string ActionProceedWithCarrier = "ProceedWithCarrier";
        private const string ActionCancelCarrier = "CancelCarrier";

        private const string FieldNameCarrierActionAck = "CAACK";
        //private const string FieldNameDataId = "DATAID";
        //private const string FieldNameCarrierAction = "CARRIERACTION";
        //private const string FieldNameCarrierId = "CARRIERID";
        //private const string FieldNamePortId = "PTN";

        private const int FieldIndexDataId = 1;
        private const int FieldIndexActionName = 2;
        private const int FieldIndexCarrierId = 3;
        private const int FieldIndexPortId = 4;
        private const int FieldIndexCapacity = 8;
        private const int FieldIndexSubCount = 11;

        private const int FieldIndexOffsetEachSlots = 3;
        private const int FieldIndexCarrierAttributeDataList = 16;      // 여기부터 Capacity * 3만큼 더해야함
        private const int FieldIndexSubstrateLotIdStart = 18;
        private const int FieldIndexSubstrateSubstrateIdStart = 19;

        private const int FieldIndexOffsetSlotStatusFromSubList = 4;        // Sublist + 4 위치부터 SlotStatus

        private readonly Dictionary<int, Tuple<string, string>> ContentMapData = null;      // Slot 별 LotId, SubId
        private readonly Dictionary<int, uint> ContentStatus = null;         // Slot 별 Status

        private bool _isCancelCarrier;
        private uint _dataId;
        private int _portId;
        private string _carrierId;
        private int _capacity;
        private int _substrateCount;
        #endregion </Fields>

        #region <Properties>        
        #endregion </Properties>

        #region <Methods>
        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValue = paramValues as ScenarioProceedWithCarrierParamValues;
        }

        public override Dictionary<string, string> GetResultData()
        {
            Dictionary<string, string> resultData = new Dictionary<string, string>();
            resultData["IsCancelCarrier"] = _isCancelCarrier.ToString();
            resultData["Capacity"] = _capacity.ToString();
            for (int i = 1; i <= _capacity; ++i)
            {
                string keyLotId = string.Format("LotId_{0}", i);
                resultData[keyLotId] = ContentMapData[i].Item1;
                
                string keyName = string.Format("Name_{0}", i);
                resultData[keyName] = ContentMapData[i].Item2;

                string keyStatus = string.Format("Status_{0}", i);
                resultData[keyStatus] = ContentStatus[i].ToString();
            }

            return resultData;
        }
        protected override bool UpdateReceivedSecsMessage(List<SemiObject> listOfReceive)
        {
            _receiveMessageFormat = listOfReceive;

            if (!(_receiveMessageFormat[FieldIndexActionName] is SemiObjectAscii actionNameData))
                return false;

            // ProceedWithCarrier or CancelCarrier
            string actionName = actionNameData.GetValue();
            if (false == actionName.Equals(ActionProceedWithCarrier) &&
                false == actionName.Equals(ActionCancelCarrier))
                return false;

            if (!(_receiveMessageFormat[FieldIndexDataId] is SemiObjectUInt4 dataId))
                return false;
            _dataId = dataId.GetValue();

            if (!(_receiveMessageFormat[FieldIndexCarrierId] is SemiObjectAscii carrierId))
                return false;
            _carrierId = carrierId.GetValue();
            if (_paramValue != null)
            {
                // 보고한 LotId와 받은 LotId가 다르면 리턴(동시 발생 시 정합성을 위해 추가)
                if (false == _carrierId.Equals(_paramValue.VariableValues[1]))
                    return false;
            }

            if (!(_receiveMessageFormat[FieldIndexPortId] is SemiObjectBinary portId))
                return false;
            
            _portId = portId.GetValue();
            _isCancelCarrier = false;

            if (actionName.Equals(ActionProceedWithCarrier))        // ProceedWithCarrier
            {
                if (!(_receiveMessageFormat[FieldIndexCapacity] is SemiObjectBinary capacity))
                    return false;

                _capacity = capacity.GetValue();

                if (!(_receiveMessageFormat[FieldIndexSubCount] is SemiObjectBinary substrateCount))
                    return false;

                _substrateCount = substrateCount.GetValue();

                int lastIndex = FieldIndexCarrierAttributeDataList;
                for (int i = 0; i < _capacity; ++i)
                {
                    int fieldIndexLotId = 16 + i * FieldIndexOffsetEachSlots;
                    int fieldIndexSubstrateId = 17 + i * FieldIndexOffsetEachSlots;
                    lastIndex = fieldIndexSubstrateId;

                    if (!(_receiveMessageFormat[fieldIndexLotId] is SemiObjectAscii lotId))
                        continue;

                    if (!(_receiveMessageFormat[fieldIndexSubstrateId] is SemiObjectAscii subId))
                        continue;

                    ContentMapData[i + 1] = new Tuple<string, string>(lotId.GetValue(), subId.GetValue());
                }

                int startingSlotStatus = lastIndex + FieldIndexOffsetSlotStatusFromSubList;
                for (int i = 0; i < _capacity; ++i)
                {
                    int fieldIndex = startingSlotStatus + i;
                    if (!(_receiveMessageFormat[fieldIndex] is SemiObjectUInt status))
                        continue;

                    ContentStatus[i + 1] = status.GetValue();
                }
            }
            else if (actionName.Equals(ActionCancelCarrier))  // CancelCarrier
            {
                _isCancelCarrier = true;
            }

            return true;
            //Permission = EN_SCENARIO_PERMISSION_RESULT.OK;
        }
        protected override bool MakeMessageToSend()
        {
            MessageFormatToSend = new List<SemiObject>();
            MessageFormatToSend.Add(new SemiObjectList(2));
            MessageFormatToSend.Add(new SemiObjectUInt(FieldNameCarrierActionAck, (byte)CarrierActionAck.Ok));
            MessageFormatToSend.Add(new SemiObjectList(0));

            return true;
        }
        #endregion </Methods>
    }
}
