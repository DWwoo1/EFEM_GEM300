using System;
using System.Collections.Generic;

using FrameOfSystem3.Recipe;
using FrameOfSystem3.SECSGEM.DefineSecsGem;
using ScenarioLogger;

namespace FrameOfSystem3.SECSGEM.Communicator
{
    internal sealed class SecsGemRuntime : ISecsGemRuntime
    {
        #region <Constructors>
        #endregion </Constructors>

        #region <Types>
        private sealed class UnformattedRecipeControlBundle
        {
            public deleReqUPloadingUnformattedRecipeControl UploadingFunc;
            public deleReqDownloadingUnformattedRecipeControl DownloadingFunc;
            public deleReqUPloadingUnformattedRecipeAck UploadingAck;
        }

        private sealed class FormattedRecipeControlBundle
        {
            public deleReqUploadingFormattedRecipe UploadingFunc;
            public deleReqDownloadingFormattedRecipe DownloadingFunc;
        }
        #endregion </Types>

        #region <Fields>
        private SecsGem _gemDriver = null;
        private string _cfgPath = string.Empty;
        private string _recipePath = string.Empty;

        private Recipe.Recipe _recipe = Recipe.Recipe.GetInstance();
        private AsyncLogger _asyncLogger = new AsyncLogger();
        private readonly LogArchiveManager _logArchiveManager = new LogArchiveManager();
        private PeriodicLogArchiveService _logArchiveService;

        private readonly List<deleHandlerString> _displayLogHandlers = new List<deleHandlerString>();
        private readonly List<deleHandlerString> _terminalMessageHandlers = new List<deleHandlerString>();
        private readonly List<deleDisplayOperatorCallForm> _operatorCallHandlers = new List<deleDisplayOperatorCallForm>();
        private readonly List<deleHandlerVoid> _connectionHandlers = new List<deleHandlerVoid>();
        private readonly List<deleHandlerString> _controlStateHandlers = new List<deleHandlerString>();
        private readonly List<deleRemoteCommand> _remoteCommandHandlers = new List<deleRemoteCommand>();
        private readonly List<deleChangeEquipmentParameters> _equipmentParameterHandlers = new List<deleChangeEquipmentParameters>();
        private readonly List<deleRecvClientToClientMessage> _clientToClientHandlers = new List<deleRecvClientToClientMessage>();
        private readonly List<deleSecsMessageReceived> _secsMessageHandlers = new List<deleSecsMessageReceived>();
        private readonly List<deleRecipeControlGrant> _recipeControlGrantHandlers = new List<deleRecipeControlGrant>();
        private readonly List<deleRecipeFileIsDeleted> _recipeFileDeletedHandlers = new List<deleRecipeFileIsDeleted>();

        private readonly List<UnformattedRecipeControlBundle> _unformattedRecipeBundles = new List<UnformattedRecipeControlBundle>();
        private readonly List<FormattedRecipeControlBundle> _formattedRecipeBundles = new List<FormattedRecipeControlBundle>();
        #endregion </Fields>

        #region <Properties>
        public bool IsConnect
        {
            get
            {
                if (_gemDriver == null) return false;
                return _gemDriver.Connect;
            }
        }
        public bool MaintenanceMode { get; set; }
        public bool IsExitingRequested { get; set; }
        #endregion </Properties>

        #region <Methods>
        public bool Initialize(SecsGem driver, string cfgPath, string recipePath)
        {
            if (driver == null)
                return false;

            if (_gemDriver != null)
            {
                Exit();
            }

            EnsureLogger();

            _cfgPath = cfgPath;
            _recipePath = recipePath;

            IsExitingRequested = false;
            MaintenanceMode = false;
            _gemDriver = driver;

            _gemDriver.CallbackLogging += WriteLog;
            _gemDriver.OnWriteTerminalLog += WriteTerminalLog;

            bool initialized = false;

            try
            {
                initialized = _gemDriver.Init(string.Format(@"{0}\{1}", _cfgPath, DefineSecsGem.PATH.FILE_NAME_CFG));

                if (!initialized)
                {
                    _gemDriver.CallbackLogging -= WriteLog;
                    _gemDriver.OnWriteTerminalLog -= WriteTerminalLog;

                    WriteLog("Communicator Initialize 실패");
                    _gemDriver = null;
                    return false;
                }

                BindCachedHandlers();

                _gemDriver.SetRecipePath(_recipePath);

                _logArchiveService = new PeriodicLogArchiveService(
                    _logArchiveManager,
                    WriteLog,
                    PATH.FILEPATH_LOG,
                    2,
                    TimeSpan.FromDays(1));

                _logArchiveService.Start();

                WriteLog("Communicator Initialize 성공");
                return true;
            }
            catch (Exception ex)
            {
                if (_logArchiveService != null)
                {
                    _logArchiveService.Stop(TimeSpan.FromSeconds(5));
                    _logArchiveService = null;
                }

                if (_gemDriver != null)
                {
                    UnbindCachedHandlers();
                    _gemDriver.CallbackLogging -= WriteLog;
                    _gemDriver.OnWriteTerminalLog -= WriteTerminalLog;
                    _gemDriver = null;
                }

                WriteLog(string.Format("Communicator initialization Failed : {0}, {1}", ex.Message, ex.StackTrace));
                return false;
            }
        }
        private void BindCachedHandlers()
        {
            if (_gemDriver == null)
                return;

            foreach (var handler in _terminalMessageHandlers)
                _gemDriver.CallbackTerminalMessage += handler;

            foreach (var handler in _operatorCallHandlers)
                _gemDriver.CallbackOperatorCall += handler;

            foreach (var handler in _connectionHandlers)
                _gemDriver.CallbackUpdateVariables += handler;

            foreach (var handler in _controlStateHandlers)
                _gemDriver.CallbackControlState += handler;

            foreach (var handler in _remoteCommandHandlers)
                _gemDriver.CallbackRemoteCommand += handler;

            foreach (var handler in _equipmentParameterHandlers)
                _gemDriver.CallbackChangeSystemParameter += handler;

            foreach (var handler in _clientToClientHandlers)
                _gemDriver.CallBackClientToClientMessageReceived += handler;

            foreach (var handler in _secsMessageHandlers)
                _gemDriver.CallbackSecsMessageReceived += handler;

            foreach (var handler in _recipeControlGrantHandlers)
                _gemDriver.CallbackCheckingRecipeControlGrant += handler;

            foreach (var bundle in _unformattedRecipeBundles)
            {
                if (bundle.UploadingFunc != null) _gemDriver.CallbackUploadingUnformattedRecipe += bundle.UploadingFunc;
                if (bundle.DownloadingFunc != null) _gemDriver.CallbackDownloadingUnformattedRecipe += bundle.DownloadingFunc;
                if (bundle.UploadingAck != null) _gemDriver.CallbackUploadingUnformattedRecipeAck += bundle.UploadingAck;
            }

            foreach (var bundle in _formattedRecipeBundles)
            {
                if (bundle.UploadingFunc != null) _gemDriver.CallbackReqUploadingFormattedRecipe += bundle.UploadingFunc;
                if (bundle.DownloadingFunc != null) _gemDriver.CallbackReqDownloadingFormattedRecipe += bundle.DownloadingFunc;
            }

            foreach (var handler in _recipeFileDeletedHandlers)
                _gemDriver.CallbackRecipeFileIsDeleted += handler;
        }
        private void UnbindCachedHandlers()
        {
            if (_gemDriver == null)
                return;

            foreach (var handler in _terminalMessageHandlers)
                _gemDriver.CallbackTerminalMessage -= handler;

            foreach (var handler in _operatorCallHandlers)
                _gemDriver.CallbackOperatorCall -= handler;

            foreach (var handler in _connectionHandlers)
                _gemDriver.CallbackUpdateVariables -= handler;

            foreach (var handler in _controlStateHandlers)
                _gemDriver.CallbackControlState -= handler;

            foreach (var handler in _remoteCommandHandlers)
                _gemDriver.CallbackRemoteCommand -= handler;

            foreach (var handler in _equipmentParameterHandlers)
                _gemDriver.CallbackChangeSystemParameter -= handler;

            foreach (var handler in _clientToClientHandlers)
                _gemDriver.CallBackClientToClientMessageReceived -= handler;

            foreach (var handler in _secsMessageHandlers)
                _gemDriver.CallbackSecsMessageReceived -= handler;

            foreach (var handler in _recipeControlGrantHandlers)
                _gemDriver.CallbackCheckingRecipeControlGrant -= handler;

            foreach (var bundle in _unformattedRecipeBundles)
            {
                if (bundle.UploadingFunc != null) _gemDriver.CallbackUploadingUnformattedRecipe -= bundle.UploadingFunc;
                if (bundle.DownloadingFunc != null) _gemDriver.CallbackDownloadingUnformattedRecipe -= bundle.DownloadingFunc;
                if (bundle.UploadingAck != null) _gemDriver.CallbackUploadingUnformattedRecipeAck -= bundle.UploadingAck;
            }

            foreach (var bundle in _formattedRecipeBundles)
            {
                if (bundle.UploadingFunc != null) _gemDriver.CallbackReqUploadingFormattedRecipe -= bundle.UploadingFunc;
                if (bundle.DownloadingFunc != null) _gemDriver.CallbackReqDownloadingFormattedRecipe -= bundle.DownloadingFunc;
            }

            foreach (var handler in _recipeFileDeletedHandlers)
                _gemDriver.CallbackRecipeFileIsDeleted -= handler;
        }
        private bool IsUnavailable()
        {
            return _gemDriver == null || IsExitingRequested;
        }
        public void MakeGemSpecification(string configDirectory, out Dictionary<string, StatusVariable> statusVariableList, out Dictionary<long, List<StatusVariable>> reportList, out Dictionary<string, CollectionEvent> collectionEventList)
        {
            statusVariableList = new Dictionary<string, StatusVariable>();
            reportList = new Dictionary<long, List<StatusVariable>>();
            collectionEventList = new Dictionary<string, CollectionEvent>();

            if (_gemDriver == null)
                return;

            _gemDriver.MakeGemSpecification(configDirectory, ref statusVariableList, ref reportList, ref collectionEventList);
        }

        public void MakeGemECVSpecification(string configDirectory, out Dictionary<string, EquipmentConstant> equipmentConstantList)
        {
            equipmentConstantList = new Dictionary<string, EquipmentConstant>();

            if (_gemDriver == null)
                return;

            _gemDriver.MakeGemECVSpecification(configDirectory, ref equipmentConstantList);
        }
        public void Exit()
        {
            IsExitingRequested = true;

            var driver = _gemDriver;
            if (driver == null)
                return;

            try
            {
                if (_logArchiveService != null)
                {
                    _logArchiveService.Stop(TimeSpan.FromSeconds(5));
                    _logArchiveService = null;
                }

                WriteLog("Communicator Initialize 종료");

                UnbindCachedHandlers();

                driver.CallbackLogging -= WriteLog;
                driver.OnWriteTerminalLog -= WriteTerminalLog;

                driver.Close();
            }
            finally
            {
                _gemDriver = null;
                MaintenanceMode = false;

                if (_asyncLogger != null)
                {
                    _asyncLogger.Exit();
                    _asyncLogger = null;
                }
            }
        }
        public bool Reset()
        {
            if (_gemDriver == null)
                return false;

            if (string.IsNullOrWhiteSpace(_cfgPath))
                return false;

            EnsureLogger();

            try
            {
                if (_logArchiveService != null)
                {
                    _logArchiveService.Stop(TimeSpan.FromSeconds(5));
                    _logArchiveService = null;
                }

                UnbindCachedHandlers();

                _gemDriver.CallbackLogging -= WriteLog;
                _gemDriver.OnWriteTerminalLog -= WriteTerminalLog;

                _gemDriver.Close();

                IsExitingRequested = false;
                MaintenanceMode = false;

                _gemDriver.CallbackLogging += WriteLog;
                _gemDriver.OnWriteTerminalLog += WriteTerminalLog;

                bool initialized = _gemDriver.Init(string.Format(@"{0}\{1}", _cfgPath, DefineSecsGem.PATH.FILE_NAME_CFG));
                if (!initialized)
                {
                    _gemDriver.CallbackLogging -= WriteLog;
                    _gemDriver.OnWriteTerminalLog -= WriteTerminalLog;

                    IsExitingRequested = true;
                    WriteLog("Communicator Reset 실패");
                    return false;
                }

                BindCachedHandlers();

                _gemDriver.SetRecipePath(_recipePath);

                _logArchiveService = new PeriodicLogArchiveService(
                    _logArchiveManager,
                    WriteLog,
                    PATH.FILEPATH_LOG,
                    2,
                    TimeSpan.FromDays(1));

                _logArchiveService.Start();

                WriteLog("Communicator Reset 성공");
                return true;
            }
            catch (Exception ex)
            {
                IsExitingRequested = true;

                try
                {
                    _gemDriver.CallbackLogging -= WriteLog;
                    _gemDriver.OnWriteTerminalLog -= WriteTerminalLog;
                }
                catch 
                {
                    
                }
                
                WriteLog(string.Format("Communicator reset Failed : {0}, {1}", ex.Message, ex.StackTrace));
                return false;
            }
        }
        private void EnsureLogger()
        {
            if (_asyncLogger != null)
                return;

            _asyncLogger = new AsyncLogger();

            foreach (var handler in _displayLogHandlers)
                _asyncLogger.CallbackDisplayLog += handler;
        }
        #region <Delegate>
        public void AttachDisplayLog(deleHandlerString pFunc)
        {
            if (pFunc == null)
                return;

            if (_displayLogHandlers.Contains(pFunc))
                return;

            _displayLogHandlers.Add(pFunc);

            if (_asyncLogger != null)
                _asyncLogger.CallbackDisplayLog += pFunc;
        }

        public void LinkTerminalMessage(deleHandlerString pFunc)
        {
            if (pFunc == null)
                return;

            _terminalMessageHandlers.Add(pFunc);

            if (_gemDriver != null)
                _gemDriver.CallbackTerminalMessage += pFunc;
        }

        public void LinkShowOperatorCall(deleDisplayOperatorCallForm pFunc)
        {
            if (pFunc == null)
                return;

            _operatorCallHandlers.Add(pFunc);

            if (_gemDriver != null)
                _gemDriver.CallbackOperatorCall += pFunc;
        }

        public void LinkConnection(deleHandlerVoid pFunc)
        {
            if (pFunc == null)
                return;

            _connectionHandlers.Add(pFunc);

            if (_gemDriver != null)
                _gemDriver.CallbackUpdateVariables += pFunc;
        }

        public void LinkControlState(deleHandlerString pFunc)
        {
            if (pFunc == null)
                return;

            _controlStateHandlers.Add(pFunc);

            if (_gemDriver != null)
                _gemDriver.CallbackControlState += pFunc;
        }

        public void LinkRemoteCommand(deleRemoteCommand pFunc)
        {
            if (pFunc == null)
                return;

            _remoteCommandHandlers.Add(pFunc);

            if (_gemDriver != null)
                _gemDriver.CallbackRemoteCommand += pFunc;
        }
        public void LinkEquipmentParameterChangeRequest(deleChangeEquipmentParameters pFunc)
        {
            if (pFunc == null)
                return;

            _equipmentParameterHandlers.Add(pFunc);

            if (_gemDriver != null)
                _gemDriver.CallbackChangeSystemParameter += pFunc;
        }
        public void LinkClientToClientMessage(deleRecvClientToClientMessage pFunc)
        {
            if (pFunc == null)
                return;

            _clientToClientHandlers.Add(pFunc);

            if (_gemDriver != null)
                _gemDriver.CallBackClientToClientMessageReceived += pFunc;
        }

        public void LinkSecsMessageReceived(deleSecsMessageReceived pFunc)
        {
            if (pFunc == null)
                return;

            _secsMessageHandlers.Add(pFunc);

            if (_gemDriver != null)
                _gemDriver.CallbackSecsMessageReceived += pFunc;
        }

        public void LinkRecipeControlGrant(deleRecipeControlGrant pFunc)
        {
            if (pFunc == null)
                return;

            _recipeControlGrantHandlers.Add(pFunc);

            if (_gemDriver != null)
                _gemDriver.CallbackCheckingRecipeControlGrant += pFunc;
        }

        public void LinkUnFormattedRecipeControls(
            deleReqUPloadingUnformattedRecipeControl pUploadingFunc,
            deleReqDownloadingUnformattedRecipeControl pDownloadingFunc,
            deleReqUPloadingUnformattedRecipeAck pUploadingAck)
        {
            _unformattedRecipeBundles.Add(new UnformattedRecipeControlBundle
            {
                UploadingFunc = pUploadingFunc,
                DownloadingFunc = pDownloadingFunc,
                UploadingAck = pUploadingAck
            });

            if (_gemDriver != null)
            {
                if (pUploadingFunc != null) _gemDriver.CallbackUploadingUnformattedRecipe += pUploadingFunc;
                if (pDownloadingFunc != null) _gemDriver.CallbackDownloadingUnformattedRecipe += pDownloadingFunc;
                if (pUploadingAck != null) _gemDriver.CallbackUploadingUnformattedRecipeAck += pUploadingAck;
            }
        }
        public void LinkFormattedRecipeControls(
            deleReqUploadingFormattedRecipe pUploadingFunc,
            deleReqDownloadingFormattedRecipe pDownloadingFunc)
        {
            _formattedRecipeBundles.Add(new FormattedRecipeControlBundle
            {
                UploadingFunc = pUploadingFunc,
                DownloadingFunc = pDownloadingFunc
            });

            if (_gemDriver != null)
            {
                if (pUploadingFunc != null) _gemDriver.CallbackReqUploadingFormattedRecipe += pUploadingFunc;
                if (pDownloadingFunc != null) _gemDriver.CallbackReqDownloadingFormattedRecipe += pDownloadingFunc;
            }
        }
        public void LinkRecipeFileIsDeleted(deleRecipeFileIsDeleted pFunc)
        {
            if (pFunc == null)
                return;

            _recipeFileDeletedHandlers.Add(pFunc);

            if (_gemDriver != null)
                _gemDriver.CallbackRecipeFileIsDeleted += pFunc;
        }
        public void LinkTerminalMessageWithProcessingScenario(deleHandlerString pFunc)
        {
            LinkTerminalMessage(pFunc);
        }
        #endregion </Delegate>

        #region <State>
        public void SetControlState(EN_CONTROL_STATE enControlState)
        {
            if (IsUnavailable())
                return;

            _gemDriver.SetInitControlState(enControlState);
            _gemDriver.SetControlState(enControlState);

            WriteLog(string.Format("SetControl state : {0}", enControlState.ToString()));
        }

        public EN_CONTROL_STATE GetControlState()
        {
            if (IsUnavailable())
                return EN_CONTROL_STATE.OFFLINE;

            return _gemDriver.GetControlState();
        }

        public void SetCommStateToEnable()
        {
            if (IsUnavailable())
                return;

            _gemDriver.SetCommStateEnabled();
        }

        public void SetCommStateToDisable()
        {
            if (IsUnavailable())
                return;

            _gemDriver.SetCommStateDisabled();
        }

        public EN_COMM_STATE GetCommState()
        {
            if (IsUnavailable())
                return EN_COMM_STATE.DISABLED;

            return _gemDriver.GetCommState();
        }
        #endregion </State>

        #region <Alarm>
        public void SetAlarm(int nAlarm)
        {
            if (IsUnavailable())
                return;

            if (MaintenanceMode)
                return;

            _gemDriver.SetAlarm(nAlarm);
        }
        public void ClearAlarm(int nAlarm)
        {
            if (IsUnavailable())
                return;

            if (MaintenanceMode)
                return;

            _gemDriver.ClearAlarm(nAlarm);
        }
        #endregion </Alarm>

        #region <UserDefinedMessage>
        public bool SendClientToClientMessage(string device, string messageName, string sendingType, string scenarioName, string[] contentNames, string[] messages, EN_MESSAGE_RESULT result, bool useLogging)
        {
            if (IsUnavailable())
                return false;

            bool sendingResult = _gemDriver.SendClientToClientMessage(device, messageName, sendingType, scenarioName, contentNames, messages, result);

            if (useLogging)
            {
                string messageOfLogging = string.Empty;
                string message = string.Empty;
                if (contentNames != null)
                {
                    int count = contentNames.Length;
                    for (int i = 0; i < count; ++i)
                    {
                        message = string.Format(" [{0} : {1}] ", contentNames[i], messages[i]);

                        messageOfLogging = string.Format("{0},{1}", messageOfLogging, message);
                    }
                }

                if (messageOfLogging.Length > 1 && messageOfLogging.Substring(0, 1).Equals(","))
                    messageOfLogging = messageOfLogging.Remove(0, 1);

                WriteLog(string.Format("Send Client Message > TargetDevice : {0}, MessageName : {1}, Type : {2}, Scenario : {3}, Content : {4}, Result : {5}", device, messageName, sendingType, scenarioName, messageOfLogging, result.ToString()));
            }

            return sendingResult;
        }
        #endregion </UserDefinedMessage>

        #region <Send Event>
        public void SendEvent(long eventID, long[] vids, string[] vidValues, bool useCheckSecondaryAck = true)
        {
            if (IsUnavailable())
                return;

            if (vids == null)
                vids = new long[0];

            if (vidValues == null)
                vidValues = new string[0];

            if (_gemDriver == null) return;

            if (vids != null && vidValues != null &&
                vids.Length != vidValues.Length)
            {
                string log = string.Format("[EQ ==> XGEM] Vid 개수 오류! >> CEID : {0}, Vid 수 : {1}, Value 수 : {2}", eventID, vids.Length, vidValues.Length);

                WriteLog(log);

                int length = Math.Min(vids.Length, vidValues.Length);

                long[] vidsTemp = new long[length];
                string[] valuesTemp = new string[length];

                Array.Copy(vids, vidsTemp, length);
                Array.Copy(vidValues, valuesTemp, length);

                _gemDriver.SendEvent(eventID, vidsTemp, valuesTemp, useCheckSecondaryAck);
            }
            else
            {
                _gemDriver.SendEvent(eventID, vids, vidValues, useCheckSecondaryAck);
            }
        }

        public bool IsSendingEventCompleted(long nEventID)
        {
            if (IsUnavailable())
                return false;

            return _gemDriver.IsEventDone(nEventID);
        }
        #endregion </SendEvent>

        #region <Send SecsMessage>
        public bool SendUserDefinedSecsMessage(long stream, long function, List<SemiObject> structure)
        {
            if (IsUnavailable())
                return false;

            return _gemDriver.SendUserDefinedSecsMessage(stream, function, structure);
        }
        #endregion </Send SecsMessage>

        #region <CallBack>
        public void ShowOperatorCallingMessage(string strMessage)
        {
            if (IsUnavailable())
                return;

            _gemDriver.ShowOperatorCall(EN_OPCALL_LEVEL.WARNING, "OPERATOR", true, strMessage);
        }

        // 2024.10.11. jhlim [DEL] ProcessingScenario로 이동
        //public void EquipmentParameterChanged(long[] arrIDs, string[] arrValues)
        //{
        //	if (_gemDriver == null) return;

        //	var paramRange = PARAM_RANGE.GetInstance();
        //	for (int i = 0; i < arrIDs.Length; ++i)
        //	{
        //                 if (EquipmentConstantList != null
        //                     && EquipmentConstantList.ContainsKey(arrIDs[i]))
        //                 {
        //                      PARAM_COMMON enCommonParam;
        //                     if(Enum.TryParse(EquipmentConstantList[arrIDs[i]].Name, out enCommonParam))
        //                     {
        //                             _recipe.SetValue(EN_RECIPE_TYPE.COMMON, enCommonParam.ToString(),
        //                                 0, EN_RECIPE_PARAM_TYPE.VALUE, arrValues[i]);
        //                     }

        //                     PARAM_EQUIPMENT enEquipmentParam;
        //                     if (Enum.TryParse(EquipmentConstantList[arrIDs[i]].Name, out enEquipmentParam))
        //                     {
        //                         _recipe.SetValue(EN_RECIPE_TYPE.EQUIPMENT, enEquipmentParam.ToString(),
        //                             0, EN_RECIPE_PARAM_TYPE.VALUE, arrValues[i]);
        //                     }
        //                 }
        //                 else
        //                 {
        //                     if (arrIDs[i] >= paramRange.ECID_START && arrIDs[i] <= paramRange.ECID_END)
        //                     {
        //                         // Common
        //                         if (arrIDs[i] >= paramRange.ECID_COMMON_START &&
        //                             arrIDs[i] <= paramRange.ECID_COMMON_END)
        //                         {
        //                             int nIndex = (int)arrIDs[i] - paramRange.ECID_COMMON_START;
        //                             PARAM_COMMON enParam = (PARAM_COMMON)nIndex;

        //                             _recipe.SetValue(EN_RECIPE_TYPE.COMMON, enParam.ToString(),
        //                                 0, EN_RECIPE_PARAM_TYPE.VALUE, arrValues[i]);
        //                         }

        //                         // Equip
        //                         if (arrIDs[i] >= paramRange.ECID_EQUIP_START &&
        //                             arrIDs[i] <= paramRange.ECID_EQUIP_END)
        //                         {
        //                             int nIndex = (int)arrIDs[i] - paramRange.ECID_EQUIP_START;

        //                             PARAM_EQUIPMENT enParam = (PARAM_EQUIPMENT)nIndex;

        //                             _recipe.SetValue(EN_RECIPE_TYPE.EQUIPMENT, enParam.ToString(), 0,
        //                                 EN_RECIPE_PARAM_TYPE.VALUE, arrValues[i]);
        //                         }
        //                     }
        //                 }
        //	}
        //}
        // 2024.10.11. jhlim [END]
        #endregion </CallBack>

        #region <Logging>
        public void WriteTerminalLog(string log)
        {
            if (_asyncLogger == null)
                return;

            _asyncLogger.EnqueueLog(LogTypes.Terminal, log);
        }

        public void WriteLog(string strLog)
        {
            if (_asyncLogger == null)
                return;

            _asyncLogger.EnqueueLog(LogTypes.History, strLog);
        }
        public void WriteScenarioLog(string strLog)
        {
            if (_asyncLogger == null)
                return;

            _asyncLogger.EnqueueLog(LogTypes.Scenario, strLog);
        }
        #endregion </Logging>

        #region <ECID>
        public void UpdateECVParameter(long nID, string strValue)
        {
            if (IsUnavailable())
                return;

            long[] arrIDs = { nID };
            string[] arrValues = { strValue };

            _gemDriver.UpdateECV(arrIDs, arrValues);
        }

        public void UpdateECVParameters(long[] arrIDs, string[] arrValues)
        {
            if (IsUnavailable())
                return;

            _gemDriver.UpdateECV(arrIDs, arrValues);
        }
        public void UpdateECVParameters(Dictionary<string, string> ecidValues)
        {
            if (IsUnavailable())
                return;

            _gemDriver.UpdateEquipmentConstants(ecidValues);
        }
        #endregion </ECID>

        #region <VID>
        public void UpdateVariable(long vid, List<SemiObject> value)
        {
            if (IsUnavailable())
                return;

            _gemDriver.UpdateVariable(vid, value);
        }
        public void UpdateVariable(long nID, string strValue)
        {
            if (IsUnavailable())
                return;

            long[] arrIDs = { nID };
            string[] arrValues = { strValue };

            _gemDriver.UpdateVariables(arrIDs, arrValues);
        }
        public void UpdateVariables(long[] arrIDs, string[] arrValues)
        {
            if (IsUnavailable())
                return;

            _gemDriver.UpdateVariables(arrIDs, arrValues);
        }
        #endregion </VID>

        #region Recipe
        public void SendRecipeUploadInquire(string recipeName)
        {
            if (IsUnavailable())
                return;

            _gemDriver.ReqUploadingRecipeInquire(recipeName);
        }
        public void SendRecipeUploadUnFormatted(string recipeName)
        {
            if (IsUnavailable())
                return;

            _gemDriver.ReqUploadingUnformattedRecipe(recipeName);
        }
        public void SendRecipeDownloadUnFormatted(string recipeName)
        {
            if (IsUnavailable())
                return;

            _gemDriver.ReqDownloadingUnformattedRecipe(recipeName);
        }
        #endregion

        #region <Gathering>
        public void Execute()
        {
            if (IsUnavailable())
                return;

            if (IsExitingRequested)
                return;

            _gemDriver.Execute();
        }
        #endregion </Gathering>

        #endregion </Methods>
    }
}