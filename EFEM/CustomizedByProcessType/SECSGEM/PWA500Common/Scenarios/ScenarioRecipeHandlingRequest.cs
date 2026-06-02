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
    public class RecipeHandlingRequestParamValues : ScenarioParamValues
    {
        public RecipeHandlingRequestParamValues(string targetDevice,
            string messageName,
            Dictionary<string, string> messages,
            bool useCommunicationToPM,
            List<string> values, string recipeId, string recipePath) : base(values)
        {
            RecipeId = recipeId;
            RecipePath = recipePath;

            TargetDevice = targetDevice;
            MessageName = messageName;

            if (messages != null)
            {
                _contentNames = messages.Keys.ToArray();
                _messages = messages.Values.ToArray();
            }
            else
            {
                _contentNames = new string[0] { };
                _messages = new string[0] { };
            }

            UseCommunicationToPM = useCommunicationToPM;
        }

        #region <Fields>
        private string[] _contentNames = null;
        private string[] _messages = null;
        #endregion </Fields>

        #region <Properties>
        public string RecipeId { get; private set; }
        public string RecipePath { get; private set; }
        public string MessageName { get; private set; }
        public string TargetDevice { get; private set; }
        public string[] ContentNames { get { return _contentNames; } }
        public string[] Messages { get { return _messages; } }
        public bool UseCommunicationToPM { get; private set; }
        #endregion </Properties>
    }

    // Recipe Upload/Download를 위한 이벤트 전송 후 처리를 기다리는 시나리오
    // 이벤트 전송 후 서버에서 Upload(S7F5) or Download(S7F3) 전송하는 시나리오이다.
    public class ScenarioRecipeHandlingRequest : AutoScenarioBase
    {
        #region <Constructor>
        public ScenarioRecipeHandlingRequest(string name,
            bool isUpload,
            long eventId,
            List<long> variableIds = null,
            bool useLogging = false,
            uint timeOut = 10000)
            : base(name, timeOut)
        {
            if (variableIds != null)
            {
                _statusVariableIds = new long[variableIds.Count];

                for (int i = 0; i < variableIds.Count; ++i)
                {
                    _statusVariableIds[i] = variableIds[i];
                }
            }

            EventId = eventId;
            UseLogging = useLogging;
            HandlingType = isUpload ? HandlingTypes.Upload : HandlingTypes.Download;
        }
        #endregion </Constructor>

        #region <Fields>
        private long[] _statusVariableIds = null;
        private RecipeHandlingRequestParamValues _paramValues = null;
        private readonly bool UseLogging;
        private readonly HandlingTypes HandlingType;
        #endregion </Fields>

        #region <Properties>
        public long EventId { get; private set; }
        #endregion </Properties>

        #region <Type>
        private enum HandlingTypes
        {
            Upload,
            Download
        }
        private enum EN_SCENARIO_SEQ
        {
            INIT = 0,
            EXECUTE_BEFORE_HANDLING = 100,
            SEND_EVENT = 200,
            WAIT_FOR_HANDLING_COMPLETION = 300,
            EXECUTE_AFTER_HANDLING = 400,
            FINISH,
        }
        #endregion </Type>

        #region <Methods>
        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValues = paramValues as RecipeHandlingRequestParamValues;
        }

        public override EN_SCENARIO_RESULT ExecuteScenario()
        {
            switch (_seqNum)
            {
                case (int)EN_SCENARIO_SEQ.INIT:
                    //Activate = true;
                    //InitFlags();
                    
                    if (_paramValues == null)
                    {
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                    }

                    if (false == _paramValues.UseCommunicationToPM)
                    {
                        _seqNum = (int)EN_SCENARIO_SEQ.SEND_EVENT;
                    }
                    else
                    {
                        switch (HandlingType)
                        {
                            case HandlingTypes.Upload:
                                _seqNum = (int)EN_SCENARIO_SEQ.EXECUTE_BEFORE_HANDLING;
                                break;
                            case HandlingTypes.Download:
                                _seqNum = (int)EN_SCENARIO_SEQ.SEND_EVENT;
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.EXECUTE_BEFORE_HANDLING:
                    {
                        if (false == _gemHandler.SendClientToClientMessage(_paramValues.TargetDevice,
                                _paramValues.MessageName,
                                DefinesForClientToClientMessage.VALUE_MESSAGE_TYPE_SEND,
                                Name,
                                _paramValues.ContentNames,
                                _paramValues.Messages,
                                EN_MESSAGE_RESULT.OK,
                                UseLogging))
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.EXECUTE_BEFORE_HANDLING + 1:
                    {
                        if (IsTickOver(true))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }

                        switch (Permission)
                        {
                            case EN_SCENARIO_PERMISSION_RESULT.OK:
                                Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
                                _seqNum = (int)EN_SCENARIO_SEQ.SEND_EVENT;
                                break;

                            case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);

                            default:
                                break;
                        }
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.SEND_EVENT:
                    if (EventId < 0)
                    {
                        SetTickCount(TimeOut);
                        _seqNum = (int)EN_SCENARIO_SEQ.WAIT_FOR_HANDLING_COMPLETION;
                    }
                    else
                    {
                        ++_seqNum;
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.SEND_EVENT + 1:
                    _gemHandler.SendEvent(EventId, _statusVariableIds, _paramValues.VariableValues);
                    SetTickCount(TimeOut);
                    ++_seqNum; break;

                case (int)EN_SCENARIO_SEQ.SEND_EVENT + 2:
                    if (IsTickOver(true))
                    {
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                    }

                    if (false == _gemHandler.IsSendingEventCompleted(EventId))
                        break;

                    SetTickCount(TimeOut);
                    _seqNum = (int)EN_SCENARIO_SEQ.WAIT_FOR_HANDLING_COMPLETION;
                    break;

                case (int)EN_SCENARIO_SEQ.WAIT_FOR_HANDLING_COMPLETION:
                    if (IsTickOver(true))
                    {
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                    }

                    switch (Permission)
                    {
                        case EN_SCENARIO_PERMISSION_RESULT.OK:
                            if (false == _paramValues.UseCommunicationToPM)
                            {
                                return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);
                            }
                            else
                            {
                                switch (HandlingType)
                                {
                                    case HandlingTypes.Upload:
                                        return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);

                                    case HandlingTypes.Download:
                                        Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
                                        _seqNum = (int)EN_SCENARIO_SEQ.EXECUTE_AFTER_HANDLING;
                                        break;
                                }
                            }
                            break;

                        case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        default:
                            break;
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.EXECUTE_AFTER_HANDLING:
                    {
                        string path = Path.GetDirectoryName(_paramValues.RecipePath);
                        if (false == Directory.Exists(path) ||
                            false == File.Exists(_paramValues.RecipePath))
                        {
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        }
                        string recipeBody = string.Empty;
                        using (StreamReader sr = new StreamReader(_paramValues.RecipePath))
                        {
                            recipeBody = sr.ReadToEnd();
                        }
                        var paramToSend = new Dictionary<string, string>
                        {
                            [RecipeHandlingKeys.KeyRecipeId] = _paramValues.RecipeId,
                            [RecipeHandlingKeys.KeyRecipeBody] = recipeBody
                        };

                        if (false == _gemHandler.SendClientToClientMessage(_paramValues.TargetDevice,
                                _paramValues.MessageName,
                                DefinesForClientToClientMessage.VALUE_MESSAGE_TYPE_SEND,
                                Name,
                                paramToSend.Keys.ToArray(),
                                paramToSend.Values.ToArray(),
                                EN_MESSAGE_RESULT.OK,
                                UseLogging))
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);

                        SetTickCount(TimeOut);
                        ++_seqNum;
                    }
                    break;

                case (int)EN_SCENARIO_SEQ.EXECUTE_AFTER_HANDLING + 1:
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
                    }
                    break;

                default:
                    return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
            }

            return EN_SCENARIO_RESULT.PROCEED;
        }
        #endregion </Methods>
    }
}
