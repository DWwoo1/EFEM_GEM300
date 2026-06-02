using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Define.DefineConstant;
using Define.DefineEnumBase.ThreadTimer;
using Define.DefineEnumBase.Common;
using Define.DefineEnumProject.Task;
using DefineTask = Define.DefineEnumProject.Task;
using Define.DefineEnumProject.Map;

using RunningMain_;
using RunningTask_;
using ThreadTimer_;

using Interrupt_;
using Alarm_;

using RegisteredInstances_;

using DesignPattern_.Observer_;
using FrameOfSystem3.Work;

namespace FrameOfSystem3.Task
{
    // 2020.05.20 by yjlee [ADD] Use the alias for the instances.
    extern alias AlarmInstance;

    /// <summary>
    /// 2020.05.18 by yjlee [ADD] This class controls the tasks and operates them.
    /// </summary>
    class TaskOperator : RunningMain
    {
        #region Singleton
        private TaskOperator() : base(Define.DefineConstant.FilePath.FILEPATH_LOG_MAIN)
        {
        }
        static private TaskOperator m_Instance = new TaskOperator();
        static public TaskOperator GetInstance() { return m_Instance; }
        #endregion

        #region Constant
        const string NAMESPACE_TASK = "FrameOfSystem3.Task.";
        #endregion

        #region <FILED>

        #region Init & Exit
        Functional.Initializer m_pInitializer = null;
        #endregion

        /// <summary>
        /// 2020.05.12 by yjlee [ADD] Declare the delegates to pass to the Dll.
        /// </summary>
        #region Thread Timer
        private deleCallbackFunction delegateThreadTimerForMain = null;
        private deleCallbackFunction delegateThreadTimerForObserver = null;
        #endregion

        #region Instances for Obserber
        private Subject subjectEquipmentState = null;
        private Subject subjectAlarm = null;
        #endregion

        #region Alarm
        private int m_nIndexOfBuzzerOutput = -1;
        private int m_nIndexOfGeneratedAlarm = -1;
        #endregion

        #region Common
        FrameOfSystem3.Config.ConfigDevice m_InstanceOfDevice = null;
        FrameOfSystem3.Config.ConfigAlarm.TaskAlarmInformation m_instancOfTaskInformation = null;
        #endregion

        private Dictionary<string, RunningTask> m_dicOfTask = new Dictionary<string, RunningTask>();

        // 2020.12.01 by jhchoo [ADD] Recovery 객체 모음
        #region Recovery
        private Dictionary<EN_TASK_LIST, RecoveryData> m_dicRecovery = new Dictionary<EN_TASK_LIST, RecoveryData>();
        #endregion

		#region for Task Functions
        private TickCounter_.TickCounter _efemWaitingTimeCounter = new TickCounter_.TickCounter();
        private static Recipe.Recipe _recipe = Recipe.Recipe.GetInstance();
        private Alarm _alarm = Alarm.GetInstance();
        #endregion /for Task Functions

        #endregion </FIELD>

        #region Inherit Interface
        /// <summary>
        /// 2020.05.18 by yjlee [ADD] This function will be called to initialize the equipment.
        /// </summary>
        protected override bool DoUndefinedSequence()
        {
            return m_pInitializer.DoInitializeSequence();
        }

        /// <summary>
        /// 2020.05.19 by yjlee [ADD] Monitor the 'UNDEFINED' state to update the equipment state to 'PAUSE'.
        ///     - if the function return 'false', the equipment state can't become to 'PAUSE'.
        /// </summary>
        protected override bool MonitorUndefined()
        {
            return true;
        }

        /// <summary>
        /// 2020.05.19 by yjlee [ADD] Monitor the 'PAUSE' state to check whether an error occur or not.
        ///     - if the function return 'false', the equipment state will be change to 'UNDEFINED'.
        ///     - Don't add the blocking sequence here.
        /// </summary>
        protected override bool MonitorPause()
        {
            return true;
        }

        /// <summary>
        /// 2020.05.19 by yjlee [ADD] Monitor the 'IDLE' state to check whether an error occur or not.
        ///     - if the function return 'false', the equipment state will be change to 'PUASE'.
        ///     - Don't add the blocking sequence here.
        /// </summary>
        protected override bool MonitorIdle()
        {
            return true;
        }

        #region RunMode
        /// <summary>
        /// 2020.12.03 by yjlee [ADD] If the run mode is changed, it will be called.
        ///     - Don't add the blocking sequence here.
        /// </summary>
        protected override void ChangeRunMode(RUN_MODE enRunMode)
        {
            //bool bVisionSkipped     = false;
			Log.LogManager.GetInstance().WriteProcessLog("GENERAL", string.Format("CHANGE_RUN_MODE:{0}", enRunMode.ToString()));
            switch (enRunMode)
            {
                case RUN_MODE.AUTO:
                    //bVisionSkipped  = false;
                    break;

                case RUN_MODE.DRY_RUN:
                    //bVisionSkipped  = true;
                    break;

                case RUN_MODE.SIMULATION:
                    //bVisionSkipped  = true;
                    break;
            }

            // Vision_.Vision.GetInstance().SetRunMode(bVisionSkipped);
        }
        #endregion

        #region Alarm
        /// <summary>
        /// 2021.02.16 by yjlee [ADD] Get the task information when an alarm occur in the system.
        /// </summary>

        protected override void GetTaskAlarmInformartion(ref TaskAlarmData[] arTaskAlarmData)
        {
            foreach (TaskAlarmData pTaskData in arTaskAlarmData)
            {
                m_instancOfTaskInformation.UpdateTaskInformation(pTaskData);
            }
        }

        /// <summary>
        /// 2020.06.09 by yjlee [ADD] Show an alarm message.
        /// </summary>
        protected override void ShowAlarmMessage()
        {
            Views.Functional.Form_AlarmMessage.GetInstance().SetAlarmMessageForm(true);
        }
        /// <summary>
        /// 2020.06.09 by yjlee [ADD] Hide an alarm message.
        /// </summary>
        protected override void HideAlarmMessage()
        {
            Views.Functional.Form_AlarmMessage.GetInstance().SetAlarmMessageForm(false);
        }

        /// <summary>
        /// 2021.04.27 by yjlee [ADD] Start a buzzer action.
        /// </summary>
        protected override void StartBuzzer()
        {
            m_nIndexOfGeneratedAlarm = -1;

            if (false == Alarm_.Alarm.GetInstance().GetGeneratedAlarm(ref m_nIndexOfGeneratedAlarm)
                || false == Alarm_.Alarm.GetInstance().GetParameter(m_nIndexOfGeneratedAlarm, Alarm_.PARAM_LIST.OUTPUT_BUZZER, ref m_nIndexOfBuzzerOutput))
                return;

            DigitalIO_.DigitalIO.GetInstance().WriteOutput(m_nIndexOfBuzzerOutput, true);
        }

        /// <summary>
        /// 2021.04.27 by yjlee [ADD] Stop a buzzer action.
        /// </summary>
        protected override void StopBuzzer()
        {
            DigitalIO_.DigitalIO.GetInstance().WriteOutput(m_nIndexOfBuzzerOutput, false);
        }
        #endregion

        #endregion

        #region <METHOD>

        #region Init
        /// <summary>
        /// 2020.05.18 by yjlee [ADD] Initialize the instances to operator the machine.
        /// </summary>
        private void InitializeMainInstances()
        {
            // 1. Registers the subject of the task operator.
            subjectEquipmentState = EquipmentState_.EquipmentState.GetInstance();
            subjectAlarm = Alarm_.Alarm.GetInstance();

            RegisterSubject(subjectEquipmentState);
            RegisterSubject(subjectAlarm);

            // 2. Initialize the thread timer.
            ThreadTimer.GetInstance().Init(false);

            // 3. Add the instances to the thread timer.
            delegateThreadTimerForObserver = new deleCallbackFunction(DesignPattern_.Observer_.NotifyThread.GetInstance().Run);
            ThreadTimer.GetInstance().AddTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_OBSERVER
                , ThreadTimerInterval.THREADTIMER_INTERVAL_OBSERVER
                , delegateThreadTimerForObserver);
            ThreadTimer.GetInstance().StartTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_OBSERVER);

            delegateThreadTimerForMain = new deleCallbackFunction(Task.TaskOperator.GetInstance().Execute);
            ThreadTimer.GetInstance().AddTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MAIN
                , ThreadTimerInterval.THREADTIMER_INTERVAL_MAIN
                , delegateThreadTimerForMain);
            ThreadTimer.GetInstance().StartTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MAIN);
        }
        /// <summary>
        /// 2020.05.18 by yjlee [ADD] Wait to be complete for the initialization.
        /// </summary>
        private void WaitForInitializing()
        {
            while (false == m_pInitializer.IsInitializationEnd()) ;
        }
        #endregion

        #region Interrupt Actions
        /// <summary>
        /// 2020.05.12 by yjlee [ADD] There are functions for the Interrupt actions.
        /// </summary>
        private async void InterruptStart(int nParam)
        {
            if (false == (await ProcessBeforeAutorun()))
                return;

            SetOperation(RunningMain_.OPERATION_EQUIPMENT.RUN); 
        }
        private void InterruptStop(int nParam) { SetOperation(RunningMain_.OPERATION_EQUIPMENT.STOP); }
        // private void InterruptReset(int nParam) { Alarm_.Alarm.GetInstance().SetAlarmResult(AlarmInstance::Alarm_.ALARM_RESULT.RESET);}

        // 2021.07.25. by shkim. [MOD] 인터럽트 Reset Action (Motion Position Clear)하도록 수정 (기존에는 작동안함)

        /// <summary>
        /// 2021.07.25. by shkim. [MOD] 인터럽트 Reset Action (Motion Position Clear)하도록 수정 (기존에는 작동안함)
        /// 2021.09.06. by shkim. [MOD] 인터럽트 Reset Action에 Motion Position Clear +  알람 리셋 하도록 수정
        /// </summary>
        /// <param name="nParam"></param>
        private void InterruptReset(int nParam)
        {
            DoInteruptActionReset();
            Alarm_.Alarm.GetInstance().SetAlarmResult(AlarmInstance::Alarm_.ALARM_RESULT.RESET);
        }
        private void InterruptAlarm(int nParam) { Alarm_.Alarm.GetInstance().GenerateAlarm(0, 0, nParam, false); }

        #endregion

        #endregion </METHOD>

        #region <INTERFACE>

        #region Init & Exit
        /// <summary>
        /// 2020.05.18 by yjlee [ADD] Initialize the instances for the equipment.
        /// </summary>
        public void Init()
        {
            m_pInitializer = new Functional.Initializer();

            m_pInitializer.Init(new DelegateForInterruptAction(InterruptStart)
                , new DelegateForInterruptAction(InterruptStop)
                , new DelegateForInterruptAction(InterruptReset)
                , new DelegateForInterruptAction(InterruptAlarm));

            // 2020.05.18 by yjlee [ADD] Initialize the instances of the taskoperator.
            InitializeMainInstances();

            // 2020.05.18 by yjlee [ADD] Wait to be complete for the initializing.
            WaitForInitializing();
			// 2024.06.14 by junho [ADD] Initialize 완료 event 추가
			if (InitializeFinished != null)
				InitializeFinished();
        }

        /// <summary>
        /// 2020.05.18 by yjlee [ADD] Release the resources of the instances.
        /// </summary>
        public void Exit()
        {
            // 2021.08.18. by shkim [ADD] 설비 종료시 설비 상태 UNDEFINED로 변경 후 종료
            RequestExitProgram();
            m_pInitializer.Exit();
        }
        #endregion

        #region Initialize of instances
        /// <summary>
        /// 2020.06.15 by yjlee [ADD] Initialize the task.
        /// </summary>
        public bool InitializeTask()
        {
            RunningTask.SetRecipeInstance(RecipeManager_.RecipeManager.GetInstance());

            // 2022.04.06 by junho [ADD] Debug mode에서 Sequence time out 증가
            int debugCoefficient = 1;
            if (System.Diagnostics.Debugger.IsAttached) debugCoefficient = 100;

            RunningTask.SetTimeoutOfSequence(Define.DefineConstant.Task.TIMEOUT_SEQUENCE * debugCoefficient);

            string[] arTaskName = null;
            int[] arInstanceOfTask = null;
            string[] arClassName = null;
            bool bInitializeTask = true;

            m_InstanceOfDevice = FrameOfSystem3.Config.ConfigDevice.GetInstance();
            m_instancOfTaskInformation = FrameOfSystem3.Config.ConfigAlarm.TaskAlarmInformation.GetInstance();

            if (FrameOfSystem3.Config.ConfigTask.GetInstance().GetListOfTask(ref arTaskName)
                && FrameOfSystem3.Config.ConfigTask.GetInstance().GetListOfIndexOfInstance(ref arInstanceOfTask)
                && FrameOfSystem3.Config.ConfigTask.GetInstance().GetListOfClassName(ref arClassName))
            {
                for (int nIndex = 0, nEndOfIndex = arClassName.Length
                    ; nIndex < nEndOfIndex; ++nIndex)
                {
                    int nIndexOfInstance = arInstanceOfTask[nIndex];

                    // 2020.07.29 by yjlee [ADD] Can't use the '0' as the instance or a task has the same instance number.
                    if (1 > nIndexOfInstance) { return false; }

                    var typeClass = Type.GetType(NAMESPACE_TASK + arClassName[nIndex]);

                    // 2020.07.29 by yjlee [ADD] Can't find the class file in the project.
                    if (null == typeClass) { return false; }

                    object[] arParam = new object[] { nIndexOfInstance, arTaskName[nIndex] };

                    var instanceOfClass = Activator.CreateInstance(typeClass, arParam);

                    RunningTask pTask = instanceOfClass as RunningTask;

                    #region Device
                    int nCountOfDevice = 0;
                    int[] arDevice = null;

                    #region Motion
                    /// 2020.11.16 by twkang [ADD] Motion 인스턴스를 추가한다.
                    m_InstanceOfDevice.GetIndexesOfDevice(arTaskName[nIndex], Config.ConfigDevice.EN_TYPE_DEVICE.MOTION, ref nCountOfDevice, ref arDevice);
                    for (int nIndexOfKey = 0; nIndexOfKey < nCountOfDevice; ++nIndexOfKey)
                    {
                        int nTargetIndex = -1;
                        RegisteredMotion pMotion = null;

                        if (m_InstanceOfDevice.GetDeviceTargetIndex(arTaskName[nIndex], Config.ConfigDevice.EN_TYPE_DEVICE.MOTION, arDevice[nIndexOfKey], ref nTargetIndex))
                        {
                            if (nTargetIndex > -1)
                            {
                                bInitializeTask &= RegisteredInstanceManager.GetInstance().AddMotionInstance(nIndexOfInstance, nTargetIndex, ref pMotion);
                                bInitializeTask &= pTask.AddInstanceOfMotion(arDevice[nIndexOfKey], ref pMotion);
                            }
                        }
                    }
                    #endregion

                    #region Cylinder
                    /// 2020.11.16 by twkang [ADD] Cylinder 인스턴스를 추가한다.
                    m_InstanceOfDevice.GetIndexesOfDevice(arTaskName[nIndex], Config.ConfigDevice.EN_TYPE_DEVICE.CYLINDER, ref nCountOfDevice, ref arDevice);
                    for (int nIndexOfKey = 0; nIndexOfKey < nCountOfDevice; ++nIndexOfKey)
                    {
                        int nTargetIndex = -1;
                        RegisteredCylinder pCylinder = null;

                        if (m_InstanceOfDevice.GetDeviceTargetIndex(arTaskName[nIndex], Config.ConfigDevice.EN_TYPE_DEVICE.CYLINDER, arDevice[nIndexOfKey], ref nTargetIndex))
                        {
                            if (nTargetIndex > -1)
                            {
                                bInitializeTask &= RegisteredInstanceManager.GetInstance().AddCylinderInstance(nIndexOfInstance, nTargetIndex, ref pCylinder);
                                bInitializeTask &= pTask.AddInstanceOfCylinder(arDevice[nIndexOfKey], ref pCylinder);
                            }
                        }
                    }
                    #endregion

                    #region Digital IO
                    /// 2020.11.16 by twkang [ADD] Digital Input 인스턴스를 추가한다.
                    m_InstanceOfDevice.GetIndexesOfDevice(arTaskName[nIndex], Config.ConfigDevice.EN_TYPE_DEVICE.DIGITAL_INPUT, ref nCountOfDevice, ref arDevice);
                    for (int nIndexOfKey = 0; nIndexOfKey < nCountOfDevice; ++nIndexOfKey)
                    {
                        int nTargetIndex = -1;
                        RegisteredDigitalInput pDigitalInput = null;

                        if (m_InstanceOfDevice.GetDeviceTargetIndex(arTaskName[nIndex], Config.ConfigDevice.EN_TYPE_DEVICE.DIGITAL_INPUT, arDevice[nIndexOfKey], ref nTargetIndex))
                        {
                            if (nTargetIndex > -1)
                            {
                                bInitializeTask &= RegisteredInstanceManager.GetInstance().AddDigitalInputInstance(nIndexOfInstance, nTargetIndex, ref pDigitalInput);
                                bInitializeTask &= pTask.AddInstanceOfDigitalInput(arDevice[nIndexOfKey], ref pDigitalInput);
                            }
                        }
                    }

                    /// 2020.11.16 by twkang [ADD] Digital Output 인스턴스를 추가한다.
                    m_InstanceOfDevice.GetIndexesOfDevice(arTaskName[nIndex], Config.ConfigDevice.EN_TYPE_DEVICE.DIGITAL_OUTPUT, ref nCountOfDevice, ref arDevice);
                    for (int nIndexOfKey = 0; nIndexOfKey < nCountOfDevice; ++nIndexOfKey)
                    {
                        int nTargetIndex = -1;
                        RegisteredDigitalOutput pDigitalOutput = null;

                        if (m_InstanceOfDevice.GetDeviceTargetIndex(arTaskName[nIndex], Config.ConfigDevice.EN_TYPE_DEVICE.DIGITAL_OUTPUT, arDevice[nIndexOfKey], ref nTargetIndex))
                        {
                            if (nTargetIndex > -1)
                            {
                                bInitializeTask &= RegisteredInstanceManager.GetInstance().AddDigitalOutputInstance(nIndexOfInstance, nTargetIndex, ref pDigitalOutput);
                                bInitializeTask &= pTask.AddInstanceOfDigitalOutput(arDevice[nIndexOfKey], ref pDigitalOutput);
                            }
                        }
                    }
                    #endregion

                    #region Analog IO
                    /// 2020.11.16 by twkang [ADD] Analog Input 인스턴스를 추가한다.
                    m_InstanceOfDevice.GetIndexesOfDevice(arTaskName[nIndex], Config.ConfigDevice.EN_TYPE_DEVICE.ANALOG_INPUT, ref nCountOfDevice, ref arDevice);

                    for (int nIndexOfKey = 0; nIndexOfKey < nCountOfDevice; ++nIndexOfKey)
                    {
                        int nTargetIndex = -1;
                        RegisteredAnalogInput pAnalogInput = null;

                        if (m_InstanceOfDevice.GetDeviceTargetIndex(arTaskName[nIndex], Config.ConfigDevice.EN_TYPE_DEVICE.ANALOG_INPUT, arDevice[nIndexOfKey], ref nTargetIndex))
                        {
                            if (nTargetIndex > -1)
                            {
                                bInitializeTask &= RegisteredInstanceManager.GetInstance().AddAnalogInputInstance(nIndexOfInstance, nTargetIndex, ref pAnalogInput);
                                bInitializeTask &= pTask.AddInstanceOfAnalogInput(arDevice[nIndexOfKey], ref pAnalogInput);
                            }
                        }
                    }

                    /// 2020.11.16 by twkang [ADD] Analog Output 인스턴스를 추가한다.
                    m_InstanceOfDevice.GetIndexesOfDevice(arTaskName[nIndex], Config.ConfigDevice.EN_TYPE_DEVICE.ANALOG_OUTPUT, ref nCountOfDevice, ref arDevice);
                    for (int nIndexOfKey = 0; nIndexOfKey < nCountOfDevice; ++nIndexOfKey)
                    {
                        int nTargetIndex = -1;
                        RegisteredAnalogOutput pAnalogOutput = null;

                        if (m_InstanceOfDevice.GetDeviceTargetIndex(arTaskName[nIndex], Config.ConfigDevice.EN_TYPE_DEVICE.ANALOG_OUTPUT, arDevice[nIndexOfKey], ref nTargetIndex))
                        {
                            if (nTargetIndex > -1)
                            {
                                bInitializeTask &= RegisteredInstanceManager.GetInstance().AddAnalogOutputInstance(nIndexOfInstance, nTargetIndex, ref pAnalogOutput);
                                bInitializeTask &= pTask.AddInstanceOfAnalogOutput(arDevice[nIndexOfKey], ref pAnalogOutput);
                            }
                        }
                    }
                    #endregion

                    #endregion

                    #region Alarm
                    // 2020.07.29 by yjlee [ADD] Add an alarm instance of the task.
                    RegisteredAlarm pAlarmInstance = null;

                    if (false == RegisteredInstanceManager.GetInstance().AddAlarmInstance(nIndexOfInstance
                        , ref pAlarmInstance))
                    {
                        return false;
                    }

                    pTask.AddInstanceOfAlarm(ref pAlarmInstance);
                    #endregion

                    AddTask(pTask);

                    m_dicOfTask.Add(arTaskName[nIndex], pTask);
                }

                return bInitializeTask;
            }

            return false;
        }
        #endregion

        #region Task Instance
        /// <summary>
        /// 2021.05.27 by yjlee [ADD] Get the task instance.
        /// </summary>
        public bool GetTaskInstance(string strTaskName, ref RunningTask pTask)
        {
            if (m_dicOfTask.ContainsKey(strTaskName))
            {
                pTask = m_dicOfTask[strTaskName];

                return true;
            }

            return false;
        }
        #endregion

        #region EquipmentSate
        public bool IsRunningMode()
        {
            EquipmentState_.EQUIPMENT_STATE current = EquipmentState_.EquipmentState.GetInstance().GetState();
            switch (current)
            {
                case EquipmentState_.EQUIPMENT_STATE.INITIALIZE:
                case EquipmentState_.EQUIPMENT_STATE.READY:
                case EquipmentState_.EQUIPMENT_STATE.EXECUTING:
                case EquipmentState_.EQUIPMENT_STATE.SETUP:
                case EquipmentState_.EQUIPMENT_STATE.FINISHING:
                    return true;
                default:
                    return false;
            }
        }
        public bool IsEquipmentStateFinishing()
        {
            return EquipmentState_.EquipmentState.GetInstance().GetState() == EquipmentState_.EQUIPMENT_STATE.FINISHING;
        }
        public bool IsEquipmentFinishing(bool bCheckAlarm = true)
        {
            bool bIsFinishing = EquipmentState_.EquipmentState.GetInstance().GetState() == EquipmentState_.EQUIPMENT_STATE.FINISHING;

            int nAlarmState = (int)Alarm_.Alarm.GetInstance().GetAlarmState();

            return (bIsFinishing ||
                (bCheckAlarm && (nAlarmState == (int)AlarmInstance.Alarm_.ALARM_STATE.WARNING || nAlarmState == (int)AlarmInstance.Alarm_.ALARM_STATE.ERROR)));
        }
        #endregion

        // 2020.12.01 by jhchoo [ADD] Recovery Method
        #region Recovery
        public void AddRecoveryData(EN_TASK_LIST eTaskItem, ref RecoveryData tRecovery)
        {
            if (m_dicRecovery.ContainsKey(eTaskItem))
            {
                m_dicRecovery[eTaskItem] = tRecovery;
                return;
            }

            m_dicRecovery.Add(eTaskItem, tRecovery);
        }
        public bool GetRecoveryData(EN_TASK_LIST eTaskItem, out RecoveryData tRecovery)
        {
            tRecovery = null;

            if (m_dicRecovery.ContainsKey(eTaskItem))
            {
                tRecovery = m_dicRecovery[eTaskItem];
                return true;
            }

            return false;
        }
        public RecoveryData GetRecoveryData(EN_TASK_LIST targetTask)
        {
            if (m_dicRecovery.ContainsKey(targetTask))
                return m_dicRecovery[targetTask];

            return null;
        }
        public bool GetPortStatus(EN_TASK_LIST eTaskItem, int nPortNumber, out EN_PORT_STATUS enPortStatus)
        {
            RecoveryData tRecovery = null;
            enPortStatus = EN_PORT_STATUS.EMPTY;

            if (false == m_dicRecovery.ContainsKey(eTaskItem))
                return false;

            tRecovery = m_dicRecovery[eTaskItem];

            tRecovery.GetPortStatus(nPortNumber, out enPortStatus);

            return true;
        }
        public bool GetPortStatus(string strTaskItem, int nPortNumber, out EN_PORT_STATUS enPortStatus)
        {
            RecoveryData tRecovery = null;
            enPortStatus = EN_PORT_STATUS.EMPTY;

            EN_TASK_LIST eTaskItem = EN_TASK_LIST.Global;

            if (false == Enum.TryParse(strTaskItem, out eTaskItem))
                return false;

            if (false == m_dicRecovery.ContainsKey(eTaskItem))
                return false;

            tRecovery = m_dicRecovery[eTaskItem];

            tRecovery.GetPortStatus(nPortNumber, out enPortStatus);

            return true;
        }
        #endregion

        #region Task Functions

        #region Is Machine mode
        public bool IsAutoRunMode()
        {
            return (EquipmentState_.EquipmentState.GetInstance().GetState() == EquipmentState_.EQUIPMENT_STATE.EXECUTING);
        }
        public bool IsManualMode()
        {
            return (EquipmentState_.EquipmentState.GetInstance().GetState() == EquipmentState_.EQUIPMENT_STATE.SETUP);
        }
        public bool IsExecutingMode()
        {
            EquipmentState_.EQUIPMENT_STATE current = EquipmentState_.EquipmentState.GetInstance().GetState();
            switch (current)
            {
                case EquipmentState_.EQUIPMENT_STATE.EXECUTING:
                case EquipmentState_.EQUIPMENT_STATE.SETUP:
                    return true;
                default:
                    return false;
            }
        }
        public bool IsEntry()
        {
            EquipmentState_.EQUIPMENT_STATE current = EquipmentState_.EquipmentState.GetInstance().GetState();
            switch (current)
            {
                case EquipmentState_.EQUIPMENT_STATE.READY:
                    return true;
                default:
                    return false;
            }
        }
        public bool IsIdleMode(bool isIdleOnly = false)
        {
            switch (EquipmentState_.EquipmentState.GetInstance().GetState())
            {
                case EquipmentState_.EQUIPMENT_STATE.IDLE:
                    return true;

                case EquipmentState_.EQUIPMENT_STATE.PAUSE:
                    return false == isIdleOnly;
            }
            return false;
        }
        public bool IsInitializeMode()
        {
            switch (EquipmentState_.EquipmentState.GetInstance().GetState())
            {
                case EquipmentState_.EQUIPMENT_STATE.INITIALIZE:
                    return true;
            }
            return false;
        }
        public bool IsFinishingMode()
        {
            // 2022.01.24 by junho [ADD] check alarm
            bool isFinishing = EquipmentState_.EquipmentState.GetInstance().GetState() == EquipmentState_.EQUIPMENT_STATE.FINISHING;

            bool isAlarm = IsAlarmState();
            return isFinishing || isAlarm;
        }
        public bool IsAlarmState()
        {
            //bool isAlarm = Alarm_.Alarm.GetInstance().GetGeneratedAlarm(ref alarmNo);

            // return (int)Alarm_.Alarm.GetInstance().GetAlarmState() > 1;

            // by shkim. 숫자보단 의미있는 Enum으로...
            return (int)Alarm_.Alarm.GetInstance().GetAlarmState() > (int)Alarm_.ALARM_STATE.NOTICE;
        }
        public bool IsPauseMode()
        {
            EquipmentState_.EQUIPMENT_STATE current = EquipmentState_.EquipmentState.GetInstance().GetState();
            switch (current)
            {
                case EquipmentState_.EQUIPMENT_STATE.UNDEFINED:
                case EquipmentState_.EQUIPMENT_STATE.PAUSE:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsDryRunOrSimulationMode()
        {
            switch (GetRunMode())
            {
                case RUN_MODE.DRY_RUN:
                case RUN_MODE.SIMULATION:
                    return true;
                default:
                    return false;
            }
        }
        public bool IsDryRunMode()
        {
            RUN_MODE mode = GetRunMode();
            return mode == RUN_MODE.DRY_RUN;
        }
        public bool IsSimulationMode()
        {
            RUN_MODE mode = GetRunMode();
            return mode == RUN_MODE.SIMULATION;
        }
        #endregion

        #region <TASK_DEVICE>
        /// <summary>
        /// 2022.04.14. by shkim. [ADD] 각 Task에서 SpeedContents를 이용해 등록된 Motion의 Speed값 받아오기 (리스트 모션 등에 활용)
        /// </summary>
        public bool GetSpeedOfTaskMotionByRunMode(string sTaskName, int nRegisteredMotionIndex, int nSpeedContents, ref double dSpeed)
        {
            int nMotionTargetIndex = -1;

            if (false == GetTargetIndexOfDeviceByRegisteredIndex(sTaskName, TaskDevice_.TYPE_DEVICE.MOTION, nRegisteredMotionIndex, ref nMotionTargetIndex))
            {
                return false;
            }

            if (false == Config.ConfigMotionSpeed.GetInstance().GetSpeedParameter(nMotionTargetIndex,
                (Config.ConfigMotion.EN_SPEED_CONTENT)nSpeedContents,
                Config.ConfigMotionSpeed.EN_PARAM_SPEED.VELOCITY,
                ref dSpeed))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 2022.04.15. by shkim. [ADD] 각 Task에 등록된 디바이스 RegisteredIndex로 Target Index
        /// </summary>
        /// <param name="sTaskName"></param>
        /// <param name="enDeviceType"></param>
        /// <param name="nRegisteredMotionIndex"></param>
        /// <param name="nTargetIndex"></param>
        /// <returns></returns>
        public bool GetTargetIndexOfDeviceByRegisteredIndex(string sTaskName, TaskDevice_.TYPE_DEVICE enDeviceType, int nRegisteredMotionIndex, ref int nTargetIndex)
        {
            return TaskDevice_.TaskDevice.GetInstance().GetDeviceTargetIndex(sTaskName,
                enDeviceType,
                nRegisteredMotionIndex,
                ref nTargetIndex);
        }

        /// <summary>
        /// 2022.01.18 by junho [ADD] Check machine is not working.
        /// 설비 상태가 Execute 상태여도 작업을 안하고 있을 수 있다.
        /// </summary>
        public bool IsMachineWait()
        {
            TaskData tData;
            foreach (EN_TASK_LIST task in Enum.GetValues(typeof(EN_TASK_LIST)))
            {
                if (task.Equals(EN_TASK_LIST.Global))
                    continue;
                
                tData = null;
                if (false == GetTaskInformation((int)task, ref tData))
                    return false;

                string strTaskAction = tData.strRunningAction;
                if (false == string.IsNullOrEmpty(strTaskAction))
                    return false;
            }

            return true;
        }

        public Config.ConfigMotion.EN_SPEED_CONTENT GetConfigSpeedContentByOperation(bool isCustomSpeed = false)
        {
            if (isCustomSpeed)
            {
                return Config.ConfigMotion.EN_SPEED_CONTENT.CUSTOM_1;
            }

            bool bIsRunMode = (GetOperationOfEquipment() == RunningMain_.OPERATION_EQUIPMENT.RUN);
            return bIsRunMode ? Config.ConfigMotion.EN_SPEED_CONTENT.RUN : Config.ConfigMotion.EN_SPEED_CONTENT.MANUAL;
        }
        public Motion_.MOTION_SPEED_CONTENT GetMotionSpeedContentByOperation(bool isCustomSpeed = false)
        {
            if (isCustomSpeed)
            {
                return Motion_.MOTION_SPEED_CONTENT.CUSTOM_1;
            }

            bool bIsRunMode = (GetOperationOfEquipment() == RunningMain_.OPERATION_EQUIPMENT.RUN);
            var motionSpeedContent = bIsRunMode ? Motion_.MOTION_SPEED_CONTENT.RUN : Motion_.MOTION_SPEED_CONTENT.MANUAL;
            return (Motion_.MOTION_SPEED_CONTENT)motionSpeedContent;
        }
        public List<Motion_.MOTION_SPEED_CONTENT> GetMotionSpeedContentByOperation(List<bool> isCustomSpeeds)
        {
            var result = new List<Motion_.MOTION_SPEED_CONTENT>();
            foreach (bool isCustomSpeed in isCustomSpeeds)
            {
                if (isCustomSpeed)
                    result.Add(Motion_.MOTION_SPEED_CONTENT.CUSTOM_1);
                else
                {
                    bool bIsRunMode = (GetOperationOfEquipment() == RunningMain_.OPERATION_EQUIPMENT.RUN);
                    var motionSpeedContent = bIsRunMode ? Motion_.MOTION_SPEED_CONTENT.RUN : Motion_.MOTION_SPEED_CONTENT.MANUAL;
                    result.Add(motionSpeedContent);
                }
            }

            return result;
        }
        #endregion

        #region Simple Recipe controll

        #region process
        public double GetProcessValue(string taskName, string parameterName, double defaultValue)
        {
            return _recipe.GetValue(taskName, parameterName, 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
        }
        public int GetProcessValue(string taskName, string parameterName, int defaultValue)
        {
            return _recipe.GetValue(taskName, parameterName, 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
        }
        public bool GetProcessValue(string taskName, string parameterName, bool defaultValue)
        {
            return _recipe.GetValue(taskName, parameterName, 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
        }
        public string GetProcessValue(string taskName, string parameterName, string defaultValue)
        {
            return _recipe.GetValue(taskName, parameterName, 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
        }

        public bool SetProcessValue(string taskName, string parameterName, string setValue)
        {
            return _recipe.SetValue(taskName, parameterName, 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, setValue);
        }

        #endregion

        #region equipment
        public double GetEquipmentValue(Recipe.PARAM_EQUIPMENT equipItem, double defaultValue)
        {
            return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT, equipItem.ToString(), 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
        }
        public int GetEquipmentValue(Recipe.PARAM_EQUIPMENT equipItem, int defaultValue)
        {
            return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT, equipItem.ToString(), 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
        }
        public bool GetEquipmentValue(Recipe.PARAM_EQUIPMENT equipItem, bool defaultValue)
        {
            return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT, equipItem.ToString(), 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
        }
        public string GetEquipmentValue(Recipe.PARAM_EQUIPMENT equipItem, string defaultValue)
        {
            return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT, equipItem.ToString(), 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
        }

        public bool SetEquipmentValue(Recipe.PARAM_EQUIPMENT equipItem, string setValue)
        {
            return _recipe.SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT, equipItem.ToString(), 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, setValue);
        }
        #endregion

        #region common
        public double GetCommonValue(Recipe.PARAM_COMMON commonItem, double defaultValue)
        {
            return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.COMMON, commonItem.ToString(), 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
        }
        public int GetCommonValue(Recipe.PARAM_COMMON commonItem, int defaultValue)
        {
            return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.COMMON, commonItem.ToString(), 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
        }
        public bool GetCommonValue(Recipe.PARAM_COMMON commonItem, bool defaultValue)
        {
            return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.COMMON, commonItem.ToString(), 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
        }
        public string GetCommonValue(Recipe.PARAM_COMMON commonItem, string defaultValue)
        {
            return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.COMMON, commonItem.ToString(), 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
        }

        public bool SetCommonValue(Recipe.PARAM_COMMON commonItem, string setValue)
        {
            return _recipe.SetValue(Recipe.EN_RECIPE_TYPE.COMMON, commonItem.ToString(), 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE, setValue);
        }
        #endregion

        #endregion /Simple Recipe controll

        public new bool SetOperation(OPERATION_EQUIPMENT enOperation)
        {
            Log.LogManager.GetInstance().WriteProcessLog("GENERAL", "OPERATION", enOperation.ToString());
            return base.SetOperation(enOperation);
        }
        public bool GetManualActionName(out string actionName)
        {
            actionName = "";
            if (GetOperationOfEquipment() != RunningMain_.OPERATION_EQUIPMENT.MANUAL)
                return false;

            var operationName = new string[][] { };
            if (false == GetManualOperation(ref operationName))
                return false;

            if (operationName.Length < 1 || operationName[0].Length < 1)
                return false;

            actionName = operationName[0][0];
            return true;
        }
        public bool GetManualActionName(out string[][] result)
        {
            result = new string[][] { };
            if (GetOperationOfEquipment() != RunningMain_.OPERATION_EQUIPMENT.MANUAL)
                return false;

            if (false == GetManualOperation(ref result))
                return false;

            return true;
        }
        public bool GetManualActionName(out List<string> taskName, out List<List<string>> actionName)
        {
            string[] arTaskName = new string[] { };
            string[][] arActionName = new string[][] { };
            if (GetOperationOfEquipment() != RunningMain_.OPERATION_EQUIPMENT.MANUAL
                || false == GetManualOperation(ref arTaskName, ref arActionName))
            {
                taskName = null;
                actionName = null;
                return false;
            }

            taskName = arTaskName.ToList();

            actionName = new List<List<string>>();
            for (int i = 0; i < arActionName.Length; ++i)
            {
                actionName.Add(new List<string>());
                actionName[i] = arActionName[i].ToList();
            }

            return true;
        }
        public void SetUiOperation(string[] taskName, string[] actionName)
        {
            if (taskName.Length != actionName.Length) return;

            string message = "";
            for (int i = 0; i < taskName.Length; ++i)
            {
                if (i == 0) message += string.Format("Do you want This Action START?\n{0}_{1}", taskName[i], actionName[i]);
                else message += string.Format(" -> {0}_{1}", taskName[i], actionName[i]);
            }

            var messageBox = Views.Functional.Form_MessageBox.GetInstance();
            if (false == messageBox.ShowMessage(message))
                return;

            SetOperation(ref taskName, ref actionName);
        }
        public ALARM_RESULT GetAlarmResult()
        {
            return _alarm.GetAlarmResult();
        }
		/// <summary>
		/// Entry에서 사용 가능한 Manual mode인지 확인
		/// </summary>
		public bool IsManualOperation()
		{
			List<string> taskNameList;
			List<List<string>> actionNameList;
			if (false == GetManualActionName(out taskNameList, out actionNameList))
				return false;

			return true;
		}

		#endregion </INTERFACE>
		
		// 2024.06.14 by junho [ADD] Initialize 완료 event 추가
		public static event System.Action InitializeFinished = null;
		#endregion </project only>

        #region <EFEM Only>

        #region <Robot Only>
        //private readonly Dictionary<int, Queue<EFEM.Defines.AtmRobot.RobotWorkingInfo>> _workingInfos
        //	= new Dictionary<int, Queue<EFEM.Defines.AtmRobot.RobotWorkingInfo>>();

        //public bool SetInformationToWork(int robotIndex, bool picking, string targetLocation, int slot, string substrateName,
        //	EFEM.Defines.AtmRobot.RobotArmTypes armType = EFEM.Defines.AtmRobot.RobotArmTypes.All, bool additional = false)
        //      {
        //	EFEM.Defines.AtmRobot.RobotWorkingInfo workingInfo = new EFEM.Defines.AtmRobot.RobotWorkingInfo();
        //	if (armType.Equals(EFEM.Defines.AtmRobot.RobotArmTypes.All))
        //          {
        //		// 비어있는 암을 가져온다.
        //		List<EFEM.Defines.AtmRobot.RobotArmTypes> availableArms = new List<EFEM.Defines.AtmRobot.RobotArmTypes>();
        //		if (false == EFEM.Modules.AtmRobotManager.Instance.GetAvailableArm(robotIndex, picking, ref availableArms))
        //			return false;

        //		// 일단은..
        //		workingInfo.ActionArm = availableArms[0];
        //	}
        //	else
        //          {
        //		workingInfo.ActionArm = armType;
        //	}

        //	workingInfo.Target = targetLocation;
        //	workingInfo.Slot = slot;
        //	workingInfo.SubstrateName = substrateName;

        //	if (false == _workingInfos.ContainsKey(robotIndex))
        //          {
        //		_workingInfos.Add(robotIndex, new Queue<EFEM.Defines.AtmRobot.RobotWorkingInfo>());
        //	}

        //	if (false == additional)
        //          {
        //		_workingInfos[robotIndex].Clear();

        //	}

        //	// 작업할 정보를 추가
        //	_workingInfos[robotIndex].Enqueue(workingInfo);

        //	return true;
        //}
        //public bool GetWorkingInfo(int robotIndex, ref EFEM.Defines.AtmRobot.RobotWorkingInfo workingInfo)
        //      {
        //	if (false == _workingInfos.ContainsKey(robotIndex))
        //		return false;

        //	// 작업할 정보를 반환
        //	workingInfo = _workingInfos[robotIndex].Dequeue();

        //	return true;
        //      }
        #endregion </Robot Only>

        #region <EFEM Simul Only>
        // 외부에서 True -> Simul 에>서 확인 후 업데이트 및 False
        public System.Collections.Concurrent.ConcurrentDictionary<int, bool> SimulLoadPortPlaced = new System.Collections.Concurrent.ConcurrentDictionary<int, bool>();        // False -> True Flag : 안착됨
        public System.Collections.Concurrent.ConcurrentDictionary<int, bool> SimulLoadPortRemoved = new System.Collections.Concurrent.ConcurrentDictionary<int, bool>();        // False -> True Flag : 제거됨
        public System.Collections.Concurrent.ConcurrentDictionary<int, bool> SimulLoadPortLoadClicked = new System.Collections.Concurrent.ConcurrentDictionary<int, bool>();        // False -> True Flag : 버튼 눌림
        public System.Collections.Concurrent.ConcurrentDictionary<int, bool> SimulLoadPortUnloadClicked = new System.Collections.Concurrent.ConcurrentDictionary<int, bool>();        // False -> True Flag : 버튼 눌림

        public void TriggerLoadPortPlacedForSimul(int portId)
        {
            SimulLoadPortPlaced[portId] = true;
        }

        public void TriggerLoadPortRemovedForSimul(int portId)
        {
            SimulLoadPortRemoved[portId] = true;
        }

        public void TriggerLoadPortLoadButtonClickedForSimul(int portId)
        {
            SimulLoadPortLoadClicked[portId] = true;
        }

        public void TriggerLoadPortUnloadButtonClickedForSimul(int portId)
        {
            SimulLoadPortUnloadClicked[portId] = true;
        }

        public int SimulSubstrateSizeType { get; private set; } = 8;
        public void SetSubstrateSizeFor500(int size)
        {
            SimulSubstrateSizeType = size;
        }
        #endregion </EFEM Simul Only>

        #region <ETC>
        public bool RemoteCommandError = false;
        private const string TitleWarning = "WARNING MESSAGE";
        public async System.Threading.Tasks.Task<bool> ProcessBeforeAutorun()
        {
            var messageBox = Views.Functional.Form_MessageBox.GetInstance();
            var indexOfBuzzer = (Alarm_.Alarm.GetInstance().GetParameter(m_nIndexOfGeneratedAlarm, Alarm_.PARAM_LIST.OUTPUT_BUZZER, ref m_nIndexOfBuzzerOutput) ? m_nIndexOfBuzzerOutput : -1);

            var scenarioOperator = SECSGEM.ScenarioOperator.Instance;
            var gemCommunicator = SECSGEM.Communicator.SecsGemHandler.Instance;
            bool useSecsGem = scenarioOperator.UseScenario;
            
            #region <SECSGEM Status>
            if (useSecsGem)
            {
                // 호스트 연결 상태 확인
                SECSGEM.DefineSecsGem.EN_COMM_STATE commState = gemCommunicator.GetCommState();
                if (false == commState.Equals(SECSGEM.DefineSecsGem.EN_COMM_STATE.COMMUNICATING))
                {
                    messageBox.ShowWarningMessage("SECS/GEM을 사용하지만 서버와 연결되지 않았습니다.", Views.Functional.Form_MessageBox.EN_STYLE.OkCancel, TitleWarning, indexOfBuzzer, true);
                    return false;
                }

                // 컨트롤 스테이터스 확인
                SECSGEM.DefineSecsGem.EN_CONTROL_STATE controlState = gemCommunicator.GetControlState();
                if (false == controlState.Equals(SECSGEM.DefineSecsGem.EN_CONTROL_STATE.REMOTE))
                {
                    string message = string.Format("SECS/GEM Control State가 Remote상태가 아닙니다. 계속 할까요? 현재 : {0}", controlState.ToString());
                    if (false == messageBox.ShowWarningMessage(message, Views.Functional.Form_MessageBox.EN_STYLE.YesNo, TitleWarning, indexOfBuzzer, true))
                    {
                        return false;
                    }
                }

                if (gemCommunicator.MaintenanceMode)
                {
                    if (false == messageBox.ShowWarningMessage("[PM모드] 활성화 되어있습니다. 작업을 진행할까요?", Views.Functional.Form_MessageBox.EN_STYLE.YesNo, TitleWarning, indexOfBuzzer, true))
                    {
                        return false;
                    }
                }

                var useDownloadingRecipe = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.COMMON,
                    Recipe.PARAM_COMMON.UseDownloadingRecipe.ToString(), true);
                if (false == useDownloadingRecipe)
                {
                    string message = "레시피 다운로드 옵션이 꺼져있습니다. 계속 할까요?";
                    if (false == messageBox.ShowWarningMessage(message, Views.Functional.Form_MessageBox.EN_STYLE.YesNo, TitleWarning, indexOfBuzzer, true))
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (false == messageBox.ShowWarningMessage("SECS/GEM이 Off 상태 입니다. 계속 진행할까요?", Views.Functional.Form_MessageBox.EN_STYLE.OkCancel, TitleWarning, indexOfBuzzer, true))
                    return false;
            }
            #endregion </SECSGEM Status>

            return true;
        }

        public bool IsProcessModuleConnected()
        {
            bool connected = true;

            if (AppConfigManager.Instance.ProcessModuleSimulation)
                return true;

            EFEM.Defines.ProcessModule.NetworkInformation networkInformation = new EFEM.Defines.ProcessModule.NetworkInformation();
            for (int i = 0; i < EFEM.Modules.ProcessModuleGroup.Instance.Count; ++i)
            {
                if (EFEM.Modules.ProcessModuleGroup.Instance.GetCommunicationInfo(i, ref networkInformation))
                {
                    foreach (var item in networkInformation.ServiceInfo)
                    {
                        connected &= item.Value.ConnectionStatus;
                    }

                    foreach (var item in networkInformation.ClientInfo)
                    {
                        connected &= item.Value.ConnectionStatus;
                    }
                }
            }

            return connected;
        }
        public bool GetTaskAlarmData(string taskName, out string actionName, out int occurredStep)
        {
            actionName = string.Empty;
            occurredStep = -1;

            return m_instancOfTaskInformation.GetTaskInformation(taskName, ref actionName, ref occurredStep);
        }
        enum TaskType
        {
            LoadPort,
            Robot,
            Global
        }
        public void MakeTaskAlarms()
        {
            List<int> codes = new List<int>();

            // 0. System
            var systemAlarms = Enum.GetValues(typeof(Define.DefineEnumProject.Common.EN_SYSTEM_ALARM));

            foreach (Define.DefineEnumProject.Common.EN_SYSTEM_ALARM en in systemAlarms)
            {
                codes.Add((int)en);
            }

            int[] currentAlarmList = new int[1];
            if (Config.ConfigAlarm.GetInstance().GetListOfItem(ref currentAlarmList))
            {
                for (int i = 0; i < currentAlarmList.Length; ++i)
                {
                    int alarmCode = 0;
                    if (Config.ConfigAlarm.GetInstance().GetParameter(currentAlarmList[i], Config.ConfigAlarm.EN_PARAM_ALARM.ALARMCODE, ref alarmCode))
                    {
                        codes.Add(alarmCode);
                        Config.ConfigAlarm.GetInstance().RemoveItem(currentAlarmList[i]);
                    }
                }
            }

            var tasks = (EN_TASK_LIST[])Enum.GetValues(typeof(EN_TASK_LIST));
            foreach (var item in tasks)
            {
                int taskIndex = ((int)item + 1) * 10000;

                bool pass = false;
                Array alarms = null;
                switch (item)
                {
                    //case EN_TASK_LIST.LoadPort1:
                    //case EN_TASK_LIST.LoadPort2:
                    //case EN_TASK_LIST.LoadPort3:
                    //case EN_TASK_LIST.LoadPort4:
                    //case EN_TASK_LIST.LoadPort5:
                    //case EN_TASK_LIST.LoadPort6:
                    //    {
                    //        alarms = Enum.GetValues(typeof(Define.DefineEnumProject.Task.LoadPort.EN_ALARM));
                    //    }
                    //    break;
                    case EN_TASK_LIST.AtmRobot:
                        {
                            alarms = Enum.GetValues(typeof(Define.DefineEnumProject.Task.AtmRobot.EN_ALARM));
                        }
                        break;
                    case EN_TASK_LIST.Global:
                        {
                            alarms = Enum.GetValues(typeof(Define.DefineEnumProject.Task.Global.ALARM_GLOBAL));
                        }
                        break;
                    default:
                        if (item.ToString().Contains("LoadPort"))
                        {
                            alarms = Enum.GetValues(typeof(Define.DefineEnumProject.Task.LoadPort.EN_ALARM));
                        }
                        else
                        {
                            pass = true;
                        }
                        break;
                }

                if (pass)
                    continue;

                if (alarms != null)
                {
                    foreach (var kvp in alarms)
                    {
                        codes.Add((int)kvp + taskIndex);
                    }
                }
            }

            //codes.Sort();

            int length = codes.Count;
            int[] alarmCodes = codes.ToArray();
            int[] messageCodes = new int[length];
            string[] grades = new string[length];
            int[] solutions = new int[length];
            int[] buzzers = new int[length];

            int offset = 0;
#if PWA500W
            offset = 2;
#endif

            for (int i = 0; i < length; ++i)
            {
                if (alarmCodes[i] == (int)Define.DefineEnumProject.Common.EN_SYSTEM_ALARM.EMO_ALARM)
                {
                    grades[i] = "ERROR";
                    messageCodes[i] = alarmCodes[i];
                    solutions[i] = -1;
                    continue;
                }

                TaskType taskType;

                if (alarmCodes[i] >= ((int)EN_TASK_LIST.LoadPort1 + 1) * 10000 && alarmCodes[i] < ((int)EN_TASK_LIST.AtmRobot + 1) * 10000)
                    taskType = TaskType.LoadPort;
                else if (alarmCodes[i] >= ((int)EN_TASK_LIST.AtmRobot + 1) * 10000 && alarmCodes[i] < ((int)EN_TASK_LIST.Global + 1) * 10000)
                    taskType = TaskType.Robot;
                else
                    taskType = TaskType.Global;

                int pureCodeOnly = alarmCodes[i] % 10000;

                // Task Alarms
                if (alarmCodes[i] >= 10000)
                {
                    int code = -1;
                    messageCodes[i] = -1;
                    solutions[i] = -1;
                    switch (taskType)
                    {
                        case TaskType.LoadPort:
                            {
                                code = alarmCodes[i] % 10000 + 10000;

                                // grade
                                if (pureCodeOnly.Equals((int)Define.DefineEnumProject.Task.LoadPort.EN_ALARM.LOADPORT_HAS_NOT_BEEN_INITIALIZED) ||
                                    pureCodeOnly.Equals((int)Define.DefineEnumProject.Task.LoadPort.EN_ALARM.LOADPORT_HAS_ALARM) ||
                                    pureCodeOnly.Equals((int)Define.DefineEnumProject.Task.LoadPort.EN_ALARM.LOADPORT_PLACEMENT_ERROR) ||
                                    pureCodeOnly.Equals((int)Define.DefineEnumProject.Task.LoadPort.EN_ALARM.LOADPORT_CARRIER_OUT_ERROR))
                                {
                                    grades[i] = "ERROR";
                                }
                                else
                                {
                                    grades[i] = "WARNING";
                                }
                            }
                            break;
                        case TaskType.Robot:
                            {
                                int taskIndex = (int)EN_TASK_LIST.AtmRobot + 1 + offset;
                                code = alarmCodes[i] % 10000 + (10000 * taskIndex);

                                if (Enum.TryParse(pureCodeOnly.ToString(), out Define.DefineEnumProject.Task.AtmRobot.EN_ALARM alarmCode))
                                {
                                    switch (alarmCode)
                                    {
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_DATA_INVALID:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_LOADING_DATA_INVALID:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_LOADING_DATA_INVALID:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_DATA_INVALID + (1000 * taskIndex);
                                            }
                                            break;

                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_SENDING_FAILED:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_LOADING_SENDING_FAILED:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_LOADING_SENDING_FAILED:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_SENDING_FAILED + (1000 * taskIndex);
                                            }
                                            break;

                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_SENDING_COMPLETED_TIMEOUT_ACK:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_LOADING_SENDING_COMPLETED_TIMEOUT_ACK:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_LOADING_SENDING_COMPLETED_TIMEOUT_ACK:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_SENDING_COMPLETED_TIMEOUT_ACK + (1000 * taskIndex);
                                            }
                                            break;

                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_SENDING_COMPLETED_BUT_NACK:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_LOADING_SENDING_COMPLETED_BUT_NACK:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_LOADING_SENDING_COMPLETED_BUT_NACK:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_SENDING_COMPLETED_BUT_NACK + (1000 * taskIndex);
                                            }
                                            break;

                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_LOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_LOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT + (1000 * taskIndex);
                                            }
                                            break;
                                            
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_RECEIVING_RESPONSE_DATA_TIMEOUT:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_LOADING_RECEIVING_RESPONSE_DATA_TIMEOUT:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_LOADING_RECEIVING_RESPONSE_DATA_TIMEOUT:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_RECEIVING_RESPONSE_DATA_TIMEOUT + (1000 * taskIndex);
                                            }
                                            break;

                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_RECEIVING_COMPLETED_BUT_ERROR:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_LOADING_RECEIVING_COMPLETED_BUT_ERROR:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_LOADING_RECEIVING_COMPLETED_BUT_ERROR:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_RECEIVING_COMPLETED_BUT_ERROR + (1000 * taskIndex);
                                            }
                                            break;

                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_LOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID:                                            
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_LOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_LOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID + (1000 * taskIndex);
                                            }
                                            break;
                                            
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_LOADING_SMEMA_TIMEOUT:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_LOADING_SMEMA_TIMEOUT + (1000 * taskIndex);
                                            }
                                            break;

                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_DATA_INVALID:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_UNLOADING_DATA_INVALID:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_UNLOADING_DATA_INVALID:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_DATA_INVALID + (1000 * taskIndex);
                                            }
                                            break;

                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_SENDING_FAILED:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_UNLOADING_SENDING_FAILED:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_UNLOADING_SENDING_FAILED:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_SENDING_FAILED + (1000 * taskIndex);
                                            }
                                            break;

                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_SENDING_COMPLETED_TIMEOUT_ACK:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_UNLOADING_SENDING_COMPLETED_TIMEOUT_ACK:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_UNLOADING_SENDING_COMPLETED_TIMEOUT_ACK:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_SENDING_COMPLETED_TIMEOUT_ACK + (1000 * taskIndex);
                                            }
                                            break;

                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_SENDING_COMPLETED_BUT_NACK:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_UNLOADING_SENDING_COMPLETED_BUT_NACK:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_UNLOADING_SENDING_COMPLETED_BUT_NACK:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_SENDING_COMPLETED_BUT_NACK + (1000 * taskIndex);
                                            }
                                            break;
                                        
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_UNLOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_UNLOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_RESPONSE_MESSAGE_TIMEOUT + (1000 * taskIndex);
                                            }
                                            break;
                                        
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_RESPONSE_DATA_TIMEOUT:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_UNLOADING_RECEIVING_RESPONSE_DATA_TIMEOUT:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_UNLOADING_RECEIVING_RESPONSE_DATA_TIMEOUT:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_RESPONSE_DATA_TIMEOUT + (1000 * taskIndex);
                                            }
                                            break;
                                        
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_ERROR:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_UNLOADING_RECEIVING_COMPLETED_BUT_ERROR:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_UNLOADING_RECEIVING_COMPLETED_BUT_ERROR:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_ERROR + (1000 * taskIndex);
                                            }
                                            break;
                                        
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_ACTION_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID:
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_BEFORE_UNLOADING_RECEIVING_COMPLETED_BUT_DATA_INVALID + (1000 * taskIndex);
                                            }
                                            break;
                                          
                                        case DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_UNLOADING_SMEMA_TIMEOUT:
                                            {
                                                messageCodes[i] = (int)DefineTask.AtmRobot.EN_ALARM.INTERFACE_AFTER_UNLOADING_SMEMA_TIMEOUT + (1000 * taskIndex);
                                            }
                                            break;
                                            
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_IS_NOT_INITIALIZED:
                                            {
                                                messageCodes[i] = code / 10000 * 1000 + code % 100;
                                                solutions[i] = code / 10000 * 10000 + code % 100;
                                            }
                                            break;
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_INITIALIZING_FAILED:
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_ALARM_CLEARING_FAILED:
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_PICKING_ACTION_FAILED:
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_PLACING_ACTION_FAILED:
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_APPROACH_LOADING_FAILED:                                            
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_LOADING_FAILED:                                            
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_APPROACH_UNLOADING_FAILED:                                            
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_UNLOADING_FAILED:
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO:
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_HAS_NO_AVAILABLE_ARM:
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_DOES_NOT_HAVE_CARRIER:
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_BEFORE_PICK:
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_AFTER_PICK:
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_BEFORE_PLACE:
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_AFTER_PLACE:
                                        case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_CONTROLLER_NOT_CONNECTED:
                                            {
                                                int temporaryCode = (int)DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_ALARM_CLEARING_FAILED + (10000 * taskIndex);
                                                messageCodes[i] = code / 10000 * 1000 + code % 100;
                                                solutions[i] = temporaryCode / 10000 * 10000 + temporaryCode % 100;
                                            }
                                            break;
                                        //case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_CANNOT_GET_WORKING_INFO:
                                        //    break;
                                        //case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_HAS_NO_AVAILABLE_ARM:
                                        //    break;
                                        //case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_DOES_NOT_HAVE_CARRIER:
                                        //    break;
                                        //case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_BEFORE_PICK:
                                        //    break;
                                        //case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_AFTER_PICK:
                                        //    break;
                                        //case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_BEFORE_PLACE:
                                        //    break;
                                        //case DefineTask.AtmRobot.EN_ALARM.ATM_ROBOT_SECSGEM_ERROR_AFTER_PLACE:
                                        //    break;
                                        default:
                                            code = alarmCodes[i] % 10000 + 10000;
                                            break;
                                    }
                                }
                                // grade
                                if (pureCodeOnly.Equals((int)Define.DefineEnumProject.Task.AtmRobot.EN_ALARM.ATM_ROBOT_IS_NOT_INITIALIZED) ||
                                    pureCodeOnly.Equals(0))
                                {
                                    grades[i] = "ERROR";
                                }
                                else
                                {
                                    grades[i] = "WARNING";
                                }
                            }
                            break;
                        case TaskType.Global:
                            {
                                int taskIndex = (int)EN_TASK_LIST.Global + 1 + offset;

                                if (pureCodeOnly.Equals(0))
                                {
                                    code = alarmCodes[i] % 10000 + 10000;
                                }
                                else
                                {
                                    code = alarmCodes[i] % 10000 + (10000 * taskIndex);
                                }
                                // grade
                                if (pureCodeOnly.Equals((int)Define.DefineEnumProject.Task.Global.ALARM_GLOBAL.DOOR_UNLOCKED) ||
                                    pureCodeOnly.Equals(0))
                                {
                                    grades[i] = "ERROR";
                                }
                                else
                                {
                                    grades[i] = "WARNING";
                                }

                            }
                            break;

                        default:
                            break;
                    }

                    // message code
                    if (messageCodes[i] == -1)
                    {
                        messageCodes[i] = code / 10000 * 1000 + code % 100;
                    }

                    // solution
                    if (solutions[i] == -1)
                    {
                        solutions[i] = code / 10000 * 10000 + code % 100;
                    }
                }
                else
                {
                    // message code
                    messageCodes[i] = alarmCodes[i] + 100000;

                    // solution
                    solutions[i] = alarmCodes[i] + 1000000;
                }

                // buzzer
                buzzers[i] = -1;
            }

            Config.ConfigAlarm.GetInstance().AddAlarmItemArray(length, alarmCodes, messageCodes, grades, solutions, buzzers);
        }
        public bool IsExiting
        {
            get; set;
        }
        public EFEM.Defines.LoadPort.LoadPortLoadingMode TargetLoadingMode { get; set; }
        public bool IsTypeEqualsAtmRobot(EN_TASK_LIST targetTask)
        {
            string taskName = targetTask.ToString();
            if (false == m_dicOfTask.ContainsKey(taskName))
                return false;

            switch (targetTask)
            {
                case EN_TASK_LIST.AtmRobot:
                    return true;
                
                default:
                    return false;
            }
        }
        #endregion </ETC>

        #endregion </EFEM Only>
    }
}