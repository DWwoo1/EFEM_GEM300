using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    public class ScenarioFormattedRecipeDownloadRequestParamValues : ScenarioParamValues
    {
        public ScenarioFormattedRecipeDownloadRequestParamValues(List<string> values) : base(values)
        {
            //UseEventHandling = useEventHandling;
        }
        //public readonly bool UseEventHandling;
    }

    public class ScenarioFormattedRecipeDownloadRequest : AutoScenarioBase
    {
        #region <Constructor>
        public ScenarioFormattedRecipeDownloadRequest(string name, long streamToSend, long funcToSend, uint timeOut = 10000)
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
        ScenarioFormattedRecipeDownloadRequestParamValues _paramValue = null;
        private List<SemiObject> _messageFormatToSend = null;

        private const string ProcessProgramFieldName = "PPID";

        private const string ModelTypeFieldName = "MDLN";
        private const string SoftwareRevisionFieldName = "SOFTREV";
        private const string CommandCodeFieldName = "CCODE";
        private const string ProcessParameterFieldName = "PPARM";

        private const int FieldIndexListForRecipeWrapper = 4;
        private const int FieldIndexRecipe = 5;
        private const int FieldIndexCCODE = 6;
        private const int FieldIndexPPARM = 8;

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
                        if (false == SendSecsMessageReqRecipeDownload())
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
            _paramValue = paramValues as ScenarioFormattedRecipeDownloadRequestParamValues;
        }
        public override bool UpdateReceiveMessage(List<SemiObject> listOfReceive)
        {
            ReceiveMessageFormat = listOfReceive;

            // PPID - PPID Process Program ID
            if (!(ReceiveMessageFormat[1] is SemiObjectAscii ppid))
                return false;
            // MDLN - Equipment Model Type
            if (!(ReceiveMessageFormat[2] is SemiObjectAscii mdln))
                return false;
            // SOFTREV - Software Revision
            if (!(ReceiveMessageFormat[3] is SemiObjectAscii softrev))
                return false;

            Permission = EN_SCENARIO_PERMISSION_RESULT.OK;
            return true;
        }

        private bool SendSecsMessageReqRecipeDownload()
        {
            if (_paramValue == null)
                return false;

            if (!(_paramValue is ScenarioFormattedRecipeDownloadRequestParamValues value))
                return false;

            _messageFormatToSend.Clear();

            _messageFormatToSend = new List<SemiObject>();
            _messageFormatToSend.Add(new SemiObjectAscii(ProcessProgramFieldName, string.Empty));

            Receiving = true;
            return true;
        }
        public override Dictionary<string, string> GetResultData()
        {
            Dictionary<string, string> resultData = new Dictionary<string, string>();
            SemiObjectList resultRecipe = ReceiveMessageFormat[FieldIndexListForRecipeWrapper] as SemiObjectList;
            long listCount = resultRecipe.GetValue();

            for (int i = 0; i < (int)listCount; i++)
            {
                resultData.Add(ReceiveMessageFormat[FieldIndexCCODE + (i * 3)].GetValueString(), ReceiveMessageFormat[FieldIndexCCODE + (i * 3) + 2].GetValueString());
            }
            return resultData;
        }
        #endregion </Methods>
    }
}
