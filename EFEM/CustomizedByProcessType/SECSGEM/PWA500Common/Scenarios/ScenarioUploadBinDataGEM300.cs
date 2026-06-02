using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    public class ScenarioUploadBinDataGEM300ParamValues : ScenarioParamValues
    {
        public ScenarioUploadBinDataGEM300ParamValues(List<string> values, bool useEventHandling, string dataPathToUpload) : base(values)
        {
            UseEventHandling = useEventHandling;
            DataPathToUpload = dataPathToUpload;
        }

        public readonly bool UseEventHandling;
        public readonly string DataPathToUpload;
    }

    public class ScenarioUploadBinDataGEM300 : AutoScenarioBase
    {
        #region <Constructors>
        public ScenarioUploadBinDataGEM300(string name, long eventId, List<long> variables,
            long vidPmsBody,
            EN_ITEM_FORMAT formatForUint,
            uint timeOut = 10000)
            : base(name, timeOut)
        {
            _receiveMessageFormat = new List<SemiObject>();
            MessageFormatToSend = new List<SemiObject>();

            _eventId = eventId;
            _variables = new List<long>(variables);

            VidPmsBody = vidPmsBody;

            ItemFormatForUint = formatForUint;
        }
        #endregion </Constructors>

        #region <Fields>
        private ScenarioUploadBinDataGEM300ParamValues _paramValue = null;
        private List<SemiObject> _receiveMessageFormat = null;
        private readonly List<SemiObject> MessageFormatToSend = null;

        private readonly long VidPmsBody;

        private readonly EN_ITEM_FORMAT ItemFormatForUint;

        private long _eventId;
        private List<long> _variables;
        //private readonly long FunctionToSendWaferMapDataSetup;
        //private readonly long FunctionToSendWaferMapTransmitInquire;
        //private readonly long FunctionToSendWaferMapData;

        //private const string AttributeNameOfMaterialId = "MID";
        //private const string AttributeNameOfIdType = "IDTYP";
        //private const string AttributeNameOfMapFormatType = "MAPFT";
        //private const string AttributeNameOfFlatNotchLocation = "FNLOC";
        //private const string AttributeNameOfFilmFrameLocation = "FFROT";
        //private const string AttributeNameOfOriginLocation = "ORLOC";
        //private const string AttributeNameOfReferencePointSelect = "RPSEL";
        //private const string AttributeNameOfReferenceXY = "REFXY";
        //private const string AttributeNameOfDieUnitsOfMeasure = "DUTMS";
        //private const string AttributeNameOfXAxisDieSize = "XDIES";
        //private const string AttributeNameOfYAxisDieSize = "YDIES";
        //private const string AttributeNameOfCountRow = "ROWCT";
        //private const string AttributeNameOfCountCol = "COLCT";
        //private const string AttributeNameOfProcessDieCount = "PRDCT";
        //private const string AttributeNameOfNullBinCode = "NULBC";
        //private const string AttributeNameOfProcessAccess = "PRAXI";
        //private const string AttributeNameOfMessageLength = "MLCL";
        //private const string AttributeNameOfStartingPointXY = "STRPxy";

        //private const string AttributeNameOfBinCodeList = "BINLT";

        //private const ushort AttributeValueFilmFrameLocation = 0;
        //private const ushort AttributeValueMessageLength = 0;
        //private const byte AttributeValueReferencePointSelect = 0;
        //private const string AttributeValueBinCodeEquivalents = "0123456789DEFGHINOPQRSTUVXYabcdefghimxyz";
        //private const string AttributeValueDieUnitsOfMeasure = "mm";
        //private const string AttributeValueNullBinCode = " ";
        //private const byte AttributeValueProcessAccess = 2;
        #endregion </Fields>

        #region <Types>
        enum ScenarioSeq
        {
            INIT = 0,
            UPDATE_BINARY_VARIABLE = 400,
            SEND_EVENT = 500,

            FINISH = 1000,
        }
        #endregion </Types>

        #region <Properties>
        public long EventId
        {
            get
            {
                return _eventId;
            }
        }
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

                        _seqNum = (int)ScenarioSeq.UPDATE_BINARY_VARIABLE;
                    }
                    break;

                case (int)ScenarioSeq.UPDATE_BINARY_VARIABLE:
                    {
                        SemiObjectBinary binaryItemToPms;
                        string pmsFilePath = _paramValue.DataPathToUpload;
                        if (string.IsNullOrEmpty(pmsFilePath))
                        {
                            binaryItemToPms = new SemiObjectBinary("PMS", new byte[] { 0x00 });
                        }
                        else
                        {
                            if (false == File.Exists(pmsFilePath))
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);

                            byte[] pmsBodies = File.ReadAllBytes(pmsFilePath);
                            binaryItemToPms = new SemiObjectBinary("PMS", pmsBodies);
                        }
                        _gemHandler.UpdateVariable(VidPmsBody, new List<SemiObject>() { binaryItemToPms });

                        SetTickCount(100);

                        _seqNum = (int)ScenarioSeq.SEND_EVENT;
                    }
                    break;

                case (int)ScenarioSeq.SEND_EVENT:
                    {
                        if (false == IsTickOver(false))
                            break;

                        List<long> variableIdsToUpdate = new List<long>();
                        List<string> variablesToUpdate = new List<string>();

                        // Body 는 위에서 업데이트 했기 때문에 제외한다.
                        for (int i = 0; i < _variables.Count; ++i)
                        {
                            if (_variables[i] == VidPmsBody)
                            {
                                //if (FrameOfSystem3.Task.TaskOperator.GetInstance().IsSimulationMode())
                                //{
                                //    _paramValue.VariableValues[i] = "0";
                                //}
                                //else
                                {
                                    continue;
                                }
                            }

                            variableIdsToUpdate.Add(_variables[i]);
                            variablesToUpdate.Add(_paramValue.VariableValues[i]);
                        }

                        _gemHandler.SendEvent(_eventId, variableIdsToUpdate.ToArray(), variablesToUpdate.ToArray());

                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)ScenarioSeq.SEND_EVENT + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        if (false == _gemHandler.IsSendingEventCompleted(_eventId))
                            break;

                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)ScenarioSeq.SEND_EVENT + 2:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                        }

                        return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                    }
                    break;

                default:
                    return EN_SCENARIO_RESULT.ERROR;
            }

            return EN_SCENARIO_RESULT.PROCEED;
        }

        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValue = paramValues as ScenarioUploadBinDataGEM300ParamValues;
        }

        #endregion </Methods>
    }
}