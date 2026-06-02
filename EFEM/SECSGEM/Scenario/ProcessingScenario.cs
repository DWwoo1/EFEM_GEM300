using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

using TickCounter_;

using FrameOfSystem3.Recipe;
using FrameOfSystem3.Functional;
using FrameOfSystem3.SECSGEM.Scenario;
using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Trace;

namespace FrameOfSystem3.SECSGEM
{
    public abstract class ProcessingScenario
    {
        #region <Variables>
        protected Recipe.Recipe _recipe = Recipe.Recipe.GetInstance();
        private Communicator.SecsGemHandler _gemHandler = Communicator.SecsGemHandler.Instance;

        protected PARAM_RANGE _paramRange = PARAM_RANGE.GetInstance();
        protected readonly ConcurrentDictionary<EN_SCENARIO, ScenarioBaseClass> ScenarioList = new ConcurrentDictionary<EN_SCENARIO, ScenarioBaseClass>();

        private readonly AutoScenarioRuntimeManager _autoScenarioRuntimeManager;

        protected string _recipePath;

        protected EN_CONTROL_STATE _controlState = EN_CONTROL_STATE.OFFLINE;

        protected readonly Dictionary<long, List<StatusVariable>> ReportList = new Dictionary<long, List<StatusVariable>>();
        protected readonly Dictionary<string, StatusVariable> StatusVariableList = new Dictionary<string, StatusVariable>();
        protected readonly Dictionary<string, CollectionEvent> CollectionEventList = new Dictionary<string, CollectionEvent>();
        protected readonly Dictionary<string, EquipmentConstant> EquipmentConstantList = new Dictionary<string, EquipmentConstant>();// key : Name;

        private Trace.TraceRuntimeManager _traceRuntimeManager = null;
        #endregion </Variables>

        #region <Constructor>
        protected ProcessingScenario()
        {
            _autoScenarioRuntimeManager = new AutoScenarioRuntimeManager(this);
            UseLogging = true;
        }
        #endregion </Constructor>

        public bool UseLogging { get; protected set; }

        #region <External Methods>
        public ScenarioBaseClass GetInstanceScenario(EN_SCENARIO scenario)
        {
            if (false == ScenarioList.ContainsKey(scenario))
                return null;

            return ScenarioList[scenario];
        }

        public List<string> GetScenarioList()
        {
            List<string> scenarioList = new List<string>();
            foreach (var item in ScenarioList)
            {
                scenarioList.Add(item.Key.ToString());
            }

            return scenarioList;
        }

        public Dictionary<long, List<StatusVariable>> GetReportList()
        {
            return new Dictionary<long, List<StatusVariable>>(ReportList);
        }

        public Dictionary<string, StatusVariable> GetStatusVariableList()
        {
            return new Dictionary<string, StatusVariable>(StatusVariableList);
        }

        public Dictionary<string, CollectionEvent> GetCollectionEventList()
        {
            return new Dictionary<string, CollectionEvent>(CollectionEventList);
        }

        public bool IsScenarioRunning(EN_SCENARIO scenario)
        {
            if (false == ScenarioList.ContainsKey(scenario))
                return false;

            return ScenarioList[scenario].Activate;
        }

        public void InitScenarioAll()
        {
            foreach (var item in ScenarioList)
            {
                SetScenarioActivation(item.Key, false);
            }
        }

        public void SetScenarioActivation(EN_SCENARIO scenario, bool activated)
        {
            if (false == ScenarioList.ContainsKey(scenario))
                return;

            ScenarioList[scenario].Activate = activated;
        }

        public bool IsAutoScenario(EN_SCENARIO scenario)
        {
            return _autoScenarioRuntimeManager.IsAutoScenario(scenario);
        }

        public bool EnqueueAutoScenarioByUpdate(
            string sender,
            EN_SCENARIO scenario,
            Dictionary<string, string> scenarioParams,
            Dictionary<string, string> additionalParams = null)
        {
            return _autoScenarioRuntimeManager.EnqueueAutoScenarioByUpdate(
                sender,
                UseLogging,
                scenario,
                scenarioParams,
                additionalParams);
        }

        public virtual EN_SCENARIO_RESULT EnqueueAutoScenario(
            string sender,
            EN_SCENARIO scenario,
            Dictionary<string, string> scenarioParams,
            deleAutoScenarioCompleted callback,
            Dictionary<string, string> additionalParams = null)
        {
            return _autoScenarioRuntimeManager.EnqueueAutoScenario(
                sender,
                UseLogging,
                scenario,
                scenarioParams,
                callback,
                additionalParams);
        }

        public Dictionary<string, string> ConsumeScenarioResultData(
            string sender,
            EN_SCENARIO scenario)
        {
            return _autoScenarioRuntimeManager.ConsumeScenarioResultData(sender, scenario);
        }

        public Dictionary<string, string> GetLastScenarioResultData(
            string sender,
            EN_SCENARIO scenario)
        {
            return _autoScenarioRuntimeManager.GetLastScenarioResultData(sender, scenario);
        }

        public EN_SCENARIO_RESULT GetAutoScenarioExecutionState(
            string sender,
            EN_SCENARIO scenario)
        {
            return _autoScenarioRuntimeManager.GetAutoScenarioExecutionState(sender, scenario);
        }

        public EN_SCENARIO_RESULT ExecuteScenario(EN_SCENARIO scenario)
        {
            if (false == ScenarioList.ContainsKey(scenario))
                return EN_SCENARIO_RESULT.PROCEED;

            return ScenarioList[scenario].ExecuteScenario();
        }

        public virtual void ProcessAutoScenarioQueue()
        {
            _autoScenarioRuntimeManager.ProcessAutoScenarioQueue();
        }

        public void InitScenarioResultData(EN_SCENARIO scenario)
        {
            if (false == ScenarioList.ContainsKey(scenario))
                return;

            ScenarioList[scenario].InitResultData();
        }

        public int GetScenarioStep(EN_SCENARIO scenario)
        {
            if (false == ScenarioList.ContainsKey(scenario))
                return DefineSecsGem.Contants.SCENARIO_STEP_END;

            return ScenarioList[scenario].Step;
        }

        public bool UpdateTraceData(ref Dictionary<long, string> dataToUpdate)
        {
            if (_traceRuntimeManager == null)
                return false;

            Dictionary<long, string> changedValues;
            if (false == _traceRuntimeManager.TryGetDelta(out changedValues))
                return false;

            dataToUpdate = changedValues;
            return true;
        }
        #endregion </External Methods>

        #region <Virtual Methods>
        public virtual bool Init(string recipePath, string configPath, Dictionary<string, StatusVariable> statusVariableList,
                Dictionary<long, List<StatusVariable>> reportList, Dictionary<string, CollectionEvent> collectionEventList)
        {
            if (statusVariableList != null)
            {
                foreach (var item in statusVariableList)
                {
                    StatusVariableList[item.Key] = item.Value;
                }
            }

            if (reportList != null)
            {
                foreach (var item in reportList)
                {
                    ReportList[item.Key] = item.Value;
                }
            }

            if (collectionEventList != null)
            {
                foreach (var item in collectionEventList)
                {
                    CollectionEventList[item.Key] = item.Value;
                }
            }

            _recipePath = recipePath;

            _gemHandler.LinkConnection(UpdateVariablesAll);
            _gemHandler.LinkControlState(ControlStateChanged);
            _gemHandler.LinkRemoteCommand(CallBackRemoteCommand);
            _gemHandler.LinkEquipmentParameterChangeRequest(EquipmentParameterChangeRequested);
            _gemHandler.LinkClientToClientMessage(CallBackClientToClientMessage);
            _gemHandler.LinkSecsMessageReceived(SecsMessageReceived);
            _gemHandler.LinkRecipeControlGrant(CheckingRecipeControlGrant);
            _gemHandler.LinkFormattedRecipeControls(UploadingFormattedRecipeReceived, DownloadingFormattedRecipeReceived);
            _gemHandler.LinkUnFormattedRecipeControls(UploadingUnFormattedRecipeReceived, DownloadingUnFormattedRecipeReceived, UploadingUnFormattedRecipeAckReceived);
            _gemHandler.LinkRecipeFileIsDeleted(RecipeFileIsDeleted);
            _gemHandler.LinkTerminalMessageWithProcessingScenario(OnTerminalMessageReceived);

            MakeScenarioByConfigFiles(configPath);
            MakeCustomScenario();

            #region <Trace Data Scenario>
            Trace.ITraceDataProvider traceProvider = CreateTraceDataProvider();
            Trace.ITraceRecoveryStore recoveryStore = CreateTraceRecoveryStore();

            if (traceProvider != null)
            {
                _traceRuntimeManager = new Trace.TraceRuntimeManager(traceProvider, recoveryStore);
                _traceRuntimeManager.Initialize();
            }
            #endregion </Trace Data Scenario>

            return true;
        }

        public virtual void Exit() 
        {
            if (_traceRuntimeManager != null)
            {
                _traceRuntimeManager.SaveRecovery();
            }
        }

        public virtual bool AddECVList(Dictionary<string, EquipmentConstant> equipmentConstantList)
        {
            if (equipmentConstantList != null)
            {
                foreach (var item in equipmentConstantList)
                {
                    EquipmentConstantList[item.Key] = item.Value;
                }
            }

            return true;
        }

        protected virtual void MakeScenario(EN_SCENARIO typeOfScenario, ScenarioBaseClass scenario)
        {
            ScenarioList.TryAdd(typeOfScenario, scenario);
        }

        public virtual bool SendClientToClientMessage(string device, string messageName, string sendingType, string scenarioName, string[] contentNames, string[] messages, EN_MESSAGE_RESULT result, bool useLogging)
        {
            return _gemHandler.SendClientToClientMessage(device, messageName, sendingType, scenarioName, contentNames, messages, result, useLogging);
        }

        public virtual EN_PPGRANT CheckingRecipeControlGrant(string recipeName)
        {
            var state = EquipmentState_.EquipmentState.GetInstance().GetState();
            switch (state)
            {
                case EquipmentState_.EQUIPMENT_STATE.EXECUTING:
                    break;

                case EquipmentState_.EQUIPMENT_STATE.FINISHING:
                case EquipmentState_.EQUIPMENT_STATE.INITIALIZE:
                case EquipmentState_.EQUIPMENT_STATE.READY:
                case EquipmentState_.EQUIPMENT_STATE.SETUP:
                    return EN_PPGRANT.BUSY;

                case EquipmentState_.EQUIPMENT_STATE.IDLE:
                case EquipmentState_.EQUIPMENT_STATE.PAUSE:
                    return EN_PPGRANT.OK;

                default:
                    return EN_PPGRANT.BUSY;
            }

            return EN_PPGRANT.BUSY;
        }

        public virtual void ExecuteReportAlarm(int alarmId, EN_GEM_ALARM_STATE state)
        {
            if (state.Equals(EN_GEM_ALARM_STATE.OCCURED))
            {
                _gemHandler.SetAlarm(alarmId);
            }
            else
            {
                _gemHandler.ClearAlarm(alarmId);
            }
        }

        public virtual void EquipmentParameterChangeRequested(string[] ecNameList, string[] valueList)
        {
            for (int i = 0; i < ecNameList.Length; ++i)
            {
                string ecName = ecNameList[i];
                if (EquipmentConstantList != null && EquipmentConstantList.ContainsKey(ecName))
                {
                    PARAM_COMMON enCommonParam;
                    if (Enum.TryParse(ecName, out enCommonParam))
                    {
                        _recipe.SetValue(EN_RECIPE_TYPE.COMMON, enCommonParam.ToString(),
                            0, EN_RECIPE_PARAM_TYPE.VALUE, valueList[i]);
                    }

                    PARAM_EQUIPMENT enEquipmentParam;
                    if (Enum.TryParse(ecName, out enEquipmentParam))
                    {
                        _recipe.SetValue(EN_RECIPE_TYPE.EQUIPMENT, enEquipmentParam.ToString(),
                            0, EN_RECIPE_PARAM_TYPE.VALUE, valueList[i]);
                    }
                }
            }
        }

        protected virtual void OnTerminalMessageReceived(string message)
        {
        }

        protected virtual void OnAutoScenarioCompleted(
            AutoScenarioRequest request,
            EN_SCENARIO_RESULT result,
            Dictionary<string, string> resultData)
        {
        }

        internal void RaiseAutoScenarioCompleted(
            AutoScenarioRequest request,
            EN_SCENARIO_RESULT result,
            Dictionary<string, string> resultData)
        {
            OnAutoScenarioCompleted(request, result, resultData);
        }
        #endregion </Virtual Methods>

        #region <Abstract Methods>

        #region <Callback Message 관련>
        public abstract bool RemoteCommandReceived(string rcmdName, string[] cpNames, string[] cpValues, ref long[] results);
        public abstract bool ClientToClientMessageReceived(string device, string messageName, string sendingType, string scenarioName, string[] contentNames, string[] messages, EN_MESSAGE_RESULT result, ref bool useLogging);
        public abstract bool SecsMessageReceived(UserDefinedSecsMessage receivedSecsMessage, ref UserDefinedSecsMessage secsMessageToSend);
        #endregion </Callback Message 관련>

        #region <Variable 관련>
        public abstract void UpdateVariablesAll();
        public abstract bool UpdateECVParameter(string strECVName, string strValue);
        #endregion </Variable 관련>

        #region <Status 관련>
        public abstract void ControlStateChanged(string state);
        public abstract void EquipmentstateChanged(string state);
        #endregion </Status 관련>

        #region <Scenario 관련>
        protected abstract void MakeCustomScenario();
        protected abstract ITraceDataProvider CreateTraceDataProvider();
        protected virtual ITraceRecoveryStore CreateTraceRecoveryStore()
        {
            return null;
        }
        protected abstract void MakeScenarioByConfigFiles(string configPath);
        public abstract bool IsScenarioEnabled(EN_SCENARIO scenario);
        public abstract List<string> GetScenarioParameterList(EN_SCENARIO scenario);
        public abstract Dictionary<string, string> GetScenarioResultData(EN_SCENARIO scenario);
        public abstract bool UpdateScenarioParams(string scenarioName, Dictionary<string, string> param);
        #endregion </Scenario 관련>

        #region <UnFormatted Recipe>
        public abstract bool UploadingUnFormattedRecipeReceived(string recipeName, ref string recipeFullPath);
        public abstract EN_ACK7 DownloadingUnFormattedRecipeReceived(string recipeName, string recipeFullPath);
        public abstract void UploadingUnFormattedRecipeAckReceived(string recipeName, EN_ACK7 recipeUploadAck);
        public abstract void RecipeFileIsDeleted(string[] deletedFileList);
        #endregion </UnFormatted Recipe>

        #region <Formatted Recipe>
        public abstract bool UploadingFormattedRecipeReceived(string recipeName, out Dictionary<string, SemiObject[]> recipeBodies);
        public abstract bool DownloadingFormattedRecipeReceived(string recipeName, Dictionary<string, string[]> recipeBodies);
        #endregion </Formatted Recipe>

        #region <주기호출>
        public abstract void Execute();
        #endregion </주기호출>

        #endregion </Abstract Methods>

        #region <Protected/Private Methods>
        #region <Interface with Gem Driver>
        protected void SetControlState(EN_CONTROL_STATE state)
        {
            _gemHandler.SetControlState(state);
        }

        protected EN_CONTROL_STATE GetControlState()
        {
            return _gemHandler.GetControlState();
        }

        protected void SendEvent(long nEventID, long[] arrVids, string[] arrVidValues)
        {
            _gemHandler.SendEvent(nEventID, arrVids, arrVidValues);
        }

        protected void SendSecsMessage(long stream, long function, List<SemiObject> messageStructure)
        {
            _gemHandler.SendUserDefinedSecsMessage(stream, function, messageStructure);
        }

        protected bool IsSendingEventCompleted(long nEventID)
        {
            return _gemHandler.IsSendingEventCompleted(nEventID);
        }

        protected void UpdateVariable(long[] ids, string[] values)
        {
            _gemHandler.UpdateVariables(ids, values);
        }

        protected void UpdateEquipmentConstants(long[] ids, string[] values)
        {
            _gemHandler.UpdateECVParameters(ids, values);
        }

        protected void UpdateEquipmentConstants(string[] ecidNames, string[] values)
        {
            var ecidValues = new Dictionary<string, string>();
            for (int i = 0; i < ecidNames.Length; ++i)
            {
                ecidValues[ecidNames[i]] = values[i];
            }
            _gemHandler.UpdateECVParameters(ecidValues);
        }
        protected bool PushCurrentTraceSnapshotToHost()
        {
            if (_traceRuntimeManager == null)
                return false;

            Dictionary<long, string> snapshot;
            if (false == _traceRuntimeManager.TryGetCurrentSnapshot(out snapshot))
                return false;

            if (snapshot == null || snapshot.Count <= 0)
                return false;

            UpdateVariable(snapshot.Keys.ToArray(), snapshot.Values.ToArray());

            return true;
        }
        #endregion </Interface with Gem Driver>

        #region <Callback Wrapper>
        private bool CallBackClientToClientMessage(string device, string messageName, string sendingType, string scenarioName, string[] contentNames, string[] messages, EN_MESSAGE_RESULT result)
        {
            bool useLogging = true;

            bool resultCallback = ClientToClientMessageReceived(device, messageName, sendingType, scenarioName, contentNames, messages, result, ref useLogging);
            if (useLogging)
            {
                string messageOfLogging = String.Empty;
                string message = String.Empty;
                if (contentNames != null)
                {
                    int count = contentNames.Length;
                    for (int i = 0; i < count; ++i)
                    {
                        message = String.Format(" [{0} : {1}] ", contentNames[i], messages[i]);

                        messageOfLogging = String.Format("{0},{1}", messageOfLogging, message);
                    }
                }

                if (messageOfLogging.Length > 1 && messageOfLogging.Substring(0, 1).Equals(","))
                    messageOfLogging = messageOfLogging.Remove(0, 1);

                _gemHandler.WriteLog(String.Format("Received Client Message > TargetDevice : {0}, MessageName : {1}, Type : {2}, Scenario : {3}, Content : {4}, Result : {5}",
                    device, messageName, sendingType, scenarioName, messageOfLogging, result.ToString()));
            }

            return resultCallback;
        }

        private bool CallBackRemoteCommand(string rcmdName, string[] cpNames, string[] cpValues, ref long[] results)
        {
            return RemoteCommandReceived(rcmdName, cpNames, cpValues, ref results);
        }
        #endregion </Callback Wrapper>

        #region <Scenario>
        protected void UpdateScenarioParams(EN_SCENARIO scenario, ScenarioParamValues values)
        {
            if (false == ScenarioList.ContainsKey(scenario))
                return;

            ScenarioList[scenario].UpdateParamValues(values);
        }
        #endregion </Scenario>

        #region <Recipe>
        protected void UpdateScenarioPermission(EN_SCENARIO scenario, bool result)
        {
            if (false == ScenarioList.ContainsKey(scenario))
                return;

            ScenarioList[scenario].UpdatePermission(result);
        }

        protected string AddExtensionToFileName(string fileName)
        {
            string ex = System.IO.Path.GetExtension(fileName);
            string extension = Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE;

            if (String.IsNullOrEmpty(ex) || false == ex.Equals(extension))
            {
                fileName += Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE;
            }

            return fileName;
        }

        protected bool LoadRecipe(string recipeName)
        {
            string ppid = AddExtensionToFileName(recipeName);
            _gemHandler.WriteLog(string.Format("> target file {0}\\{1}", _recipePath, ppid));

            if (false == FunctionsETC.FileExistCheck(_recipePath, ppid))
            {
                _gemHandler.WriteLog("> file not found");
                return false;
            }

            string path = _recipePath;
            string strErrorMsg = string.Empty;
            if (false == _recipe.LoadProcessRecipe(ref path, ref ppid, ref strErrorMsg))
            {
                _gemHandler.WriteLog(string.Format("> recipe load fail : {0}" + strErrorMsg));
                return false;
            }
            return true;
        }
        #endregion </Recipe>

        #region <Logging>
        public void WriteLog(string logToWrite)
        {
            _gemHandler.WriteLog(logToWrite);
        }
        public void WriteScenarioRuntimeLog(string logToWrite)
        {
            _gemHandler.WriteScenarioLog(logToWrite);
        }
        #endregion </Logging>

        #endregion </Internal Methods>
    }
}