using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using DesignPattern_.Observer_;
using EquipmentState_;
using FrameOfSystem3.Recipe;
using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario;
using FrameOfSystem3.SECSGEM.SecsGemSDK.Gem300;

using TickCounter_;

namespace FrameOfSystem3.SECSGEM
{
    public class ScenarioOperator : IObserver
    {
        #region <Constructors>
        protected ScenarioOperator()
            : this(Communicator.SecsGemHandler.Instance)
        {
        }
        protected ScenarioOperator(Communicator.SecsGemHandler gemCommunicator)
        {
            _gemCommunicator = gemCommunicator ?? Communicator.SecsGemHandler.Instance;
        }
        protected ScenarioOperator(
            Communicator.SecsGemHandler gemCommunicator,
            Func<ProcessingScenario> scenarioFactory,
            Func<SecsGem> driverFactory,
            string cfgPath,
            string recipePath)
        {
            if (gemCommunicator == null)
            {
                throw new ArgumentNullException("gemCommunicator");
            }

            if (scenarioFactory == null)
            {
                throw new ArgumentNullException("scenarioFactory");
            }

            if (driverFactory == null)
            {
                throw new ArgumentNullException("driverFactory");
            }

            _gemCommunicator = gemCommunicator;
            _scenarioFactory = scenarioFactory;
            _driverFactory = driverFactory;
            _cfgPath = cfgPath;
            _recipePath = recipePath;
        }
        protected ScenarioOperator(
                Func<ProcessingScenario> scenarioFactory,
                Func<SecsGem> driverFactory,
                string cfgPath,
                string recipePath)
            : this(
                Communicator.SecsGemHandler.Instance,
                scenarioFactory,
                driverFactory,
                cfgPath,
                recipePath)
        {
        }
        #endregion </Constructors>

        #region <Fields>
        private bool _initialized = false;
        private EquipmentState _subjectEquipmentState = null;

        private string _previousEquipmentState = string.Empty;
        private readonly Communicator.SecsGemHandler _gemCommunicator;
        private ProcessingScenario _scenario = null;

        private IGem300ScenarioService _gem300Service = null;

        private static readonly object _syncRoot = new object();
        private Func<ProcessingScenario> _scenarioFactory;
        private Func<SecsGem> _driverFactory;

        private string _recipePath;
        private string _cfgPath;
        private SecsGem _driver;

        private Dictionary<long, string> _traceDataToUpdate = new Dictionary<long, string>();

        private bool _isExiting = false;
        private static ScenarioOperator _instance = null;
        private bool _isUse = false;
        #endregion </Fields>

        #region <Properties>
        public static ScenarioOperator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ScenarioOperator();
                }

                return _instance;
            }
        }
        public bool UseScenario
        {
            get
            {
                return _isUse;
            }
        }
        #endregion </Properties>

        #region External Interface

        #region Initialize & Exit
        public static void ConfigureDeferred(
            Func<ProcessingScenario> scenarioFactory,
            Func<SecsGem> driverFactory,
            string cfgPath,
            string recipePath)
        {
            lock (_syncRoot)
            {
                _instance = new ScenarioOperator(
                    scenarioFactory,
                    driverFactory,
                    cfgPath,
                    recipePath);
            }
        }
        private void BuildGem300Services(SecsGem300 driver)
        {
            if (_gem300Service != null)
            {
                _gem300Service.AttachDriver(driver);
            }
        }
        private void ReleaseGem300Services()
        {
            if (_gem300Service != null)
            {
                _gem300Service.DetachDriver();
            }
        }
        public void AttachGem300Service(IGem300ScenarioService gem300Service)
        {
            if (gem300Service == null)
                throw new ArgumentNullException("gem300Service");

            _gem300Service = gem300Service;

            // Form 또는 Gem300Service가 Initialize 이후 늦게 붙는 경우 보정
            if (_initialized)
            {
                var gem300Driver = _driver as SecsGem300;
                if (gem300Driver != null)
                {
                    BuildGem300Services(gem300Driver);
                }
            }
        }
        public bool Initialize()
        {
            if (_initialized)
            {
                return true;
            }

            if (_scenario == null)
            {
                if (_scenarioFactory == null)
                {
                    throw new InvalidOperationException(
                        "Scenario factory is not configured.");
                }

                _scenario = _scenarioFactory();
            }

            if (_driver == null)
            {
                if (_driverFactory == null)
                {
                    throw new InvalidOperationException(
                        "Driver factory is not configured.");
                }

                _driver = _driverFactory();
            }

            bool result = Initialize(
                _scenario,
                _driver,
                _cfgPath,
                _recipePath);

            if (result)
            {
                _initialized = true;
            }

            return result;
        }

        public virtual bool Initialize(
                ProcessingScenario scenario,
                SecsGem driver,
                string cfgPath,
                string recipePath)
        {
            _scenario = scenario;
            if (null == _scenario)
                return false;

            if (false == _gemCommunicator.Initialize(driver, cfgPath, recipePath))
                return false;

            Dictionary<string, StatusVariable> statusVariableList;
            Dictionary<long, List<StatusVariable>> reportList;
            Dictionary<string, CollectionEvent> collectionEventList;
            _gemCommunicator.MakeGemSpecification(
                cfgPath, 
                out statusVariableList,
                out reportList,
                out collectionEventList);

            _initialized = _scenario.Init(recipePath, string.Format(@"{0}\Config", cfgPath), statusVariableList, reportList, collectionEventList);

            //24.09.27 by wdw [ADD] EQUIPMNET CONSTANT 추가
            Dictionary<string, EquipmentConstant> equipmentConstantList;
            _gemCommunicator.MakeGemECVSpecification(cfgPath, out equipmentConstantList);
            _scenario.AddECVList(equipmentConstantList);

            if (_initialized)
            {
                RegisterSubject(EquipmentState.GetInstance());
                UpdateVariableItems();

                if (driver is SecsGem300 gem && _gem300Service != null)
                {
                    BuildGem300Services(gem);
                }
            }

            _isUse = Recipe.Recipe.GetInstance().GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.UseSecsGem.ToString(), false);

            return _initialized;
        }

        public virtual void Exit()
        {
            _isExiting = true;

            if (_scenario != null)
                _scenario.Exit();

            ReleaseGem300Services();

            _gemCommunicator.Exit();
        }
        public virtual bool Reset()
        {
            bool result = _gemCommunicator.Reset();
            if (result)
                _isExiting = false;

            return result;
        }

        public bool SetUse(bool trigger)
        {
            if (trigger && false == IsHostConnected(false))
            {
                _isUse = false;
                return false;
            }

            _isUse = trigger;
            return true;
        }
        #endregion

        #region Execute Scenario
        public void InitScenarioAll()
        {
            if (_scenario == null)
                return;

            _scenario.InitScenarioAll();
        }
        public void InitScenarioResultData(EN_SCENARIO scenario)
        {
            if (_scenario == null)
                return;

            if (_scenario.GetInstanceScenario(scenario) == null)
                return;

            _scenario.InitScenarioResultData(scenario);
        }

        public bool IsScenarioRegistered(EN_SCENARIO scenario)
        {
            if (_scenario == null)
                return false;

            return (_scenario.GetInstanceScenario(scenario) != null);
        }
        public void SetScenarioActivation(EN_SCENARIO scenario, bool activation)
        {
            if (_scenario == null)
                return;

            if (_scenario.GetInstanceScenario(scenario) == null)
                return;

            _scenario.SetScenarioActivation(scenario, activation);
        }
        public bool UpdateScenarioParam(
            string sender,
            EN_SCENARIO scenario,
            Dictionary<string, string> values)
        {
            if (_scenario == null)
                return false;

            if (_scenario.GetInstanceScenario(scenario) == null)
                return true;

            if (_scenario.IsAutoScenario(scenario))
            {
                return _scenario.EnqueueAutoScenarioByUpdate(sender, scenario, values);
            }

            _scenario.SetScenarioActivation(scenario, false);
            return _scenario.UpdateScenarioParams(scenario.ToString(), values);
        }

        public EN_SCENARIO_RESULT ExecuteScenario(
            string sender,
            EN_SCENARIO scenario)
        {
            if (_scenario == null)
                return EN_SCENARIO_RESULT.ERROR;

            if (_scenario.GetInstanceScenario(scenario) == null || false == _isUse)
                return EN_SCENARIO_RESULT.COMPLETED;

            if (_scenario.IsAutoScenario(scenario))
            {
                return _scenario.GetAutoScenarioExecutionState(sender, scenario);
            }

            if (false == _scenario.IsScenarioRunning(scenario))
            {
                InitScenarioResultData(scenario);
                _scenario.SetScenarioActivation(scenario, true);
            }

            return _scenario.ExecuteScenario(scenario);
        }

        public EN_SCENARIO_RESULT EnqueueAutoScenario(
            string sender,
            EN_SCENARIO scenario,
            Dictionary<string, string> values,
            deleAutoScenarioCompleted callback)
        {
            if (_scenario == null)
                return EN_SCENARIO_RESULT.ERROR;

            if (_scenario.GetInstanceScenario(scenario) == null || false == _isUse)
                return EN_SCENARIO_RESULT.COMPLETED;

            return _scenario.EnqueueAutoScenario(sender, scenario, values, callback);
        }
        #endregion

        #region Update State
        public void UpdateState(string strState)
        {
            if (_scenario == null) return;

            if (false == _previousEquipmentState.Equals(strState))
            {
                _previousEquipmentState = String.Format("{0}", strState);
                _scenario.EquipmentstateChanged(strState);
            }
        }
        #endregion

        #region Update Variable Items
        public void UpdateVariableItems()
        {
            if (_scenario == null) return;

            if (false == _initialized) return;

            if (false == _isUse)
                return;

            _scenario.UpdateVariablesAll();
        }
        #endregion

        #region ECID Parameter
        public void UpdateCommonParameters(PARAM_COMMON enParam, string strValue)
        {
            if (_scenario == null) return;

            _gemCommunicator.UpdateECVParameter((long)enParam + PARAM_RANGE.GetInstance().ECID_COMMON_START, strValue);
        }

        public void UpdateMachineParameters(PARAM_EQUIPMENT enParam, string strValue)
        {
            if (_scenario == null) return;

            _gemCommunicator.UpdateECVParameter((long)enParam + PARAM_RANGE.GetInstance().ECID_EQUIP_START, strValue);
        }

        public bool UpdateECVParameters(string strECIDName, string strValue)
        {
            if (_scenario == null) return false;

            return _scenario.UpdateECVParameter(strECIDName, strValue);
        }
        #endregion

        #region <Alarms>
        public void ExecuteAlarmScenario(int alarmId, EN_GEM_ALARM_STATE state)
        {
            if (_scenario == null)
                return;

            if (false == UseScenario || _gemCommunicator.MaintenanceMode)
                return;

            _scenario.ExecuteReportAlarm(alarmId, state);
        }
        #endregion </Alarms>

        #region <Scenario Config>
        public List<string> GetScenarioList()
        {
            if (_scenario == null)
                return null;

            return _scenario.GetScenarioList();
        }

        public List<string> GetScenarioParameterList(EN_SCENARIO scenario)
        {
            if (_scenario == null)
                return null;

            if (false == _scenario.IsScenarioEnabled(scenario))
                return null;

            return _scenario.GetScenarioParameterList(scenario);
        }

        public Dictionary<string, string> GetScenarioResultData(
                    string sender,
                    EN_SCENARIO scenario)
        {
            if (_scenario == null)
                return null;

            if (_scenario.GetInstanceScenario(scenario) == null)
                return null;

            if (_scenario.IsAutoScenario(scenario))
            {
                return _scenario.ConsumeScenarioResultData(sender, scenario);
            }

            return _scenario.GetScenarioResultData(scenario);
        }

        public Dictionary<string, string> GetLastScenarioResultData(
            string sender,
            EN_SCENARIO scenario)
        {
            if (_scenario == null)
                return null;

            if (_scenario.GetInstanceScenario(scenario) == null)
                return null;

            if (_scenario.IsAutoScenario(scenario))
            {
                return _scenario.GetLastScenarioResultData(sender, scenario);
            }

            return _scenario.GetScenarioResultData(scenario);
        }

        public int GetScenarioStep(EN_SCENARIO scenario)
        {
            if (_scenario == null)
                return DefineSecsGem.Contants.SCENARIO_STEP_END;

            if (_scenario.GetInstanceScenario(scenario) == null)
                return DefineSecsGem.Contants.SCENARIO_STEP_END;

            //ScenarioParamValues values = new ScenarioParamValues(arValues.ToList());

            if (false == _scenario.IsScenarioRunning(scenario))
            {
                return DefineSecsGem.Contants.SCENARIO_STEP_END;
            }

            return _scenario.GetScenarioStep(scenario);
        }

        public Dictionary<long, List<StatusVariable>> GetReportList()
        {
            if (_scenario == null)
                return null;

            return _scenario.GetReportList();
        }
        public Dictionary<string, StatusVariable> GetStatusVariableList()
        {
            if (_scenario == null)
                return null;

            return _scenario.GetStatusVariableList();
        }
        public Dictionary<string, CollectionEvent> GetCollectionEventList()
        {
            if (_scenario == null)
                return null;

            return _scenario.GetCollectionEventList();
        }

        #endregion <Scenario Config>

        #endregion

        #region Internal Interface

        #region Connect 여부 확인
        private bool IsHostConnected(bool bDoGenerateAlarm = true)
        {
            if (false == _isUse)
                return true;

            if (false == _initialized || false == _gemCommunicator.IsConnect)
            {
                if (true == bDoGenerateAlarm)
                {
                    //Alarm_.Alarm.GetInstance().GenerateAlarm(0, 0, (int)Define.DefineEnumProject.Alarm.EN_SYSTEM_ALARM.HOST_CONNECTION_ALARM, false);
                }
                return false;
            }

            // 추후 이벤트 사용 여부를 설정해서 여기서 패스할 지 정해서 넘겨야하나??

            return true;
        }
        #endregion

        #endregion

        #region <Observer>
        // 설비 상태를 받아오기 위해 옵저버를 등록한다.
        public void RegisterSubject(Subject pSubject)
        {
            if (true == _initialized)
            {
                if (pSubject is EquipmentState)
                {
                    _subjectEquipmentState = pSubject as EquipmentState;

                    // 2022.08.19 by Thienvv [MOD] when state of Load is change, then update state of all Devices
                    //UpdateUnitState(m_instEqpInfo.UnitStatus); // 2022.08.31 by Thienvv [DEL]
                    UpdateState(_subjectEquipmentState.GetState().ToString());
                }

                pSubject.Attach(this);
            }
        }
        public void UpdateObserver(Subject pSubject)
        {
            if (true == _initialized)
            {
                if (pSubject is EquipmentState)
                {
                    _subjectEquipmentState = pSubject as EquipmentState;

                    UpdateState(_subjectEquipmentState.GetState().ToString());
                    //UpdateUnitState(m_subjectEquipmentState.GetState().ToString()); // 2022.08.31 by Thienvv [DEL]
                }
            }
        }
        #endregion </Observer>

        #region <Execute>
        public virtual void Execute()
        {
            if (false == _initialized)
                return;

            if (_isExiting)
                return;

            if (_scenario == null)
                return;

            if (false == IsHostConnected(false))
                return;

            _scenario.Execute();
            _scenario.ProcessAutoScenarioQueue();

            if (_isUse)
            {
                if (_scenario.UpdateTraceData(ref _traceDataToUpdate))
                {
                    if (_traceDataToUpdate != null && _traceDataToUpdate.Count > 0)
                    {
                        _gemCommunicator.UpdateVariables(_traceDataToUpdate.Keys.ToArray(), _traceDataToUpdate.Values.ToArray());
                    }
                }
            }

            _gemCommunicator.Execute();
        }
        #endregion </Execute>
    }

    namespace Communicator
    {
        using FrameOfSystem3.SECSGEM.DefineSecsGem;
        using System.Collections.Generic;

        public class SecsGemHandler
        {
            #region <Constructors>
            private SecsGemHandler()
            {
                _runtime = new SecsGemRuntime();
            }
            #endregion </Constructors>

            #region <Fields>
            private static SecsGemHandler _instance = null;
            private readonly ISecsGemRuntime _runtime;
            #endregion </Fields>

            #region <Properties>
            public bool IsConnect
            {
                get
                {
                    return _runtime.IsConnect;
                }
            }

            public bool MaintenanceMode
            {
                get
                {
                    return _runtime.MaintenanceMode;
                }
                set
                {
                    _runtime.MaintenanceMode = value;
                }
            }

            public bool IsExitingRequested
            {
                get
                {
                    return _runtime.IsExitingRequested;
                }
                set
                {
                    _runtime.IsExitingRequested = value;
                }
            }
            public static SecsGemHandler Instance
            {
                get
                {
                    if (_instance == null)
                        _instance = new SecsGemHandler();

                    return _instance;
                }
            }
            #endregion </Properties>

            #region <Methods>

            #region <Delegate>
            public void AttachDisplayLog(deleHandlerString pFunc)
            {
                if (_runtime == null)
                    return;

                _runtime.AttachDisplayLog(pFunc);
            }

            public void LinkTerminalMessage(deleHandlerString pFunc)
            {
                if (_runtime == null)
                    return;

                _runtime.LinkTerminalMessage(pFunc);
            }

            public void LinkShowOperatorCall(deleDisplayOperatorCallForm pFunc)
            {
                if (_runtime == null)
                    return;

                _runtime.LinkShowOperatorCall(pFunc);
            }

            public void LinkConnection(deleHandlerVoid pFunc)
            {
                if (_runtime == null)
                    return;

                _runtime.LinkConnection(pFunc);
            }

            public void LinkControlState(deleHandlerString pFunc)
            {
                if (_runtime == null)
                    return;

                _runtime.LinkControlState(pFunc);
            }

            public void LinkRemoteCommand(deleRemoteCommand pFunc)
            {
                if (_runtime == null)
                    return;

                _runtime.LinkRemoteCommand(pFunc);
            }
            public void LinkEquipmentParameterChangeRequest(deleChangeEquipmentParameters pFunc)
            {
                if (_runtime == null)
                    return;

                _runtime.LinkEquipmentParameterChangeRequest(pFunc);
            }
            public void LinkClientToClientMessage(deleRecvClientToClientMessage pFunc)
            {
                if (_runtime == null)
                    return;

                _runtime.LinkClientToClientMessage(pFunc);
            }

            public void LinkSecsMessageReceived(deleSecsMessageReceived pFunc)
            {
                if (_runtime == null)
                    return;

                _runtime.LinkSecsMessageReceived(pFunc);
            }

            public void LinkRecipeControlGrant(deleRecipeControlGrant pFunc)
            {
                if (_runtime == null)
                    return;

                _runtime.LinkRecipeControlGrant(pFunc);
            }

            public void LinkUnFormattedRecipeControls(
                deleReqUPloadingUnformattedRecipeControl pUploadingFunc,
                deleReqDownloadingUnformattedRecipeControl pDownloadingFunc,
                deleReqUPloadingUnformattedRecipeAck pUploadingAck)
            {
                if (_runtime == null)
                    return;

                _runtime.LinkUnFormattedRecipeControls(
                    pUploadingFunc,
                    pDownloadingFunc,
                    pUploadingAck);
            }

            public void LinkFormattedRecipeControls(
                deleReqUploadingFormattedRecipe pUploadingFunc,
                deleReqDownloadingFormattedRecipe pDownloadingFunc)
            {
                if (_runtime == null)
                    return;

                _runtime.LinkFormattedRecipeControls(
                    pUploadingFunc,
                    pDownloadingFunc);
            }

            public void LinkRecipeFileIsDeleted(deleRecipeFileIsDeleted pFunc)
            {
                if (_runtime == null)
                    return;

                _runtime.LinkRecipeFileIsDeleted(pFunc);
            }
            public void LinkTerminalMessageWithProcessingScenario(deleHandlerString pFunc)
            {
                if (_runtime == null)
                    return;

                _runtime.LinkTerminalMessageWithProcessingScenario(pFunc);
            }
            #endregion </Delegate>

            #region <Init/Exit>
            public bool Initialize(SecsGem driver, string cfgPath, string recipePath)
            {
                return _runtime.Initialize(driver, cfgPath, recipePath);
            }

            public void MakeGemSpecification(
                string configDirectory,
                out Dictionary<string, StatusVariable> statusVariableList,
                out Dictionary<long, List<StatusVariable>> reportList,
                out Dictionary<string, CollectionEvent> collectionEventList)
            {
                _runtime.MakeGemSpecification(
                    configDirectory,
                    out statusVariableList,
                    out reportList,
                    out collectionEventList);
            }

            public void MakeGemECVSpecification(
                string configDirectory,
                out Dictionary<string, EquipmentConstant> equipmentConstantList)
            {
                _runtime.MakeGemECVSpecification(
                    configDirectory,
                    out equipmentConstantList);
            }

            public void Exit()
            {
                _runtime.Exit();
            }
            public bool Reset()
            {
                return _runtime.Reset();
            }
            #endregion </Init/Exit>

            #region <State>
            public void SetControlState(EN_CONTROL_STATE controlState)
            {
                _runtime.SetControlState(controlState);
            }

            public EN_CONTROL_STATE GetControlState()
            {
                return _runtime.GetControlState();
            }

            public void SetCommStateEnable()
            {
                _runtime.SetCommStateToEnable();
            }

            public void SetCommStateDisabled()
            {
                _runtime.SetCommStateToDisable();
            }

            public EN_COMM_STATE GetCommState()
            {
                return _runtime.GetCommState();
            }
            #endregion </State>

            #region <Alarm>
            public void SetAlarm(int alarmId)
            {
                _runtime.SetAlarm(alarmId);
            }
            public void ClearAlarm(int alarmId)
            {
                _runtime.ClearAlarm(alarmId);
            }
            #endregion </Alarm>

            #region <UserDefinedMessage>
            public bool SendClientToClientMessage(
                string device,
                string messageName,
                string sendingType,
                string scenarioName,
                string[] contentNames,
                string[] messages,
                EN_MESSAGE_RESULT result,
                bool useLogging)
            {
                return _runtime.SendClientToClientMessage(
                    device,
                    messageName,
                    sendingType,
                    scenarioName,
                    contentNames,
                    messages,
                    result,
                    useLogging);
            }
            #endregion </UserDefinedMessage>

            #region <Send Event>
            public void SendEvent(
                long eventID,
                long[] vids,
                string[] vidValues,
                bool useCheckSecondaryAck = true)
            {
                _runtime.SendEvent(eventID, vids, vidValues, useCheckSecondaryAck);
            }

            public bool IsSendingEventCompleted(long eventId)
            {
                return _runtime.IsSendingEventCompleted(eventId);
            }
            #endregion </SendEvent>

            #region <Send SecsMessage>
            public bool SendUserDefinedSecsMessage(long stream, long function, List<SemiObject> structure)
            {
                return _runtime.SendUserDefinedSecsMessage(stream, function, structure);
            }
            #endregion </Send SecsMessage>

            #region <CallBack>
            public void ShowOperatorCallingMessage(string message)
            {
                _runtime.ShowOperatorCallingMessage(message);
            }
            #endregion </CallBack>

            #region <Logging>
            public void WriteTerminalLog(string message)
            {
                _runtime.WriteTerminalLog(message);
            }
            public void WriteLog(string message)
            {
                if (string.IsNullOrWhiteSpace(message))
                {

                }
                _runtime.WriteLog(message);
            }
            public void WriteScenarioLog(string message)
            {
                _runtime.WriteScenarioLog(message);
            }
            #endregion </Logging>

            #region <ECID>
            public void UpdateECVParameter(long ecid, string value)
            {
                _runtime.UpdateECVParameter(ecid, value);
            }

            public void UpdateECVParameters(long[] ecids, string[] values)
            {
                _runtime.UpdateECVParameters(ecids, values);
            }
            public void UpdateECVParameters(Dictionary<string, string> ecidValues)
            {
                _runtime.UpdateECVParameters(ecidValues);
            }
            #endregion </ECID>

            #region <VID>
            public void UpdateVariable(long vid, List<SemiObject> value)
            {
                _runtime.UpdateVariable(vid, value);
            }
            public void UpdateVariable(long vid, string value)
            {
                _runtime.UpdateVariable(vid, value);
            }
            public void UpdateVariables(long[] vids, string[] values)
            {
                _runtime.UpdateVariables(vids, values);
            }
            #endregion </VID>

            #region Recipe
            public void SendRecipeUploadInquire(string recipeName)
            {
                _runtime.SendRecipeUploadInquire(recipeName);
            }
            public void SendRecipeUploadUnFormatted(string recipeName)
            {
                _runtime.SendRecipeUploadUnFormatted(recipeName);
            }
            public void SendRecipeDownloadUnFormatted(string recipeName)
            {
                _runtime.SendRecipeDownloadUnFormatted(recipeName);
            }
            #endregion

            #region <Gathering>
            public void Execute()
            {
                _runtime.Execute();
            }
            #endregion </Gathering>

            #endregion </Methods>
        }
    }
}