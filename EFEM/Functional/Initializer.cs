using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Define.DefineConstant;
using Define.DefineEnumBase.ThreadTimer;
using Define.DefineEnumBase.Initialize;

using ThreadTimer_;

using Account_;
using Alarm_;

using Motion_;

using Cylinder_;

using DigitalIO_;
using AnalogIO_;

using Socket_;
using Serial_;
using Trigger_;
using Interrupt_;
using TaskDevice_;

using Vision_;
using RegisteredInstances_;

using DesignPattern_.Observer_;

namespace FrameOfSystem3.Functional
{
    /// <summary>
    /// 2020.05.13 by yjlee [ADD] Initialize the instances of the dll.
    /// </summary>
    public class Initializer
    {
        #region Variables
        // 2022.01.17. [ADD] PROGRESS BAR DEBUG 모드 확인용 (기본값 false)
        private static bool m_bShowProgressWhenAttachedDebuger = false;

        private EN_INITIALIZATION_STEP m_enInitializeStep = EN_INITIALIZATION_STEP.INIT_START;

        #region Delegate
        /// <summary>
        /// 2020.05.12 by yjlee [ADD] Declare the delegates to pass to the Dll.
        /// </summary>
        #region Thread Timer
        private deleCallbackFunction delegateThreadTimerForFileIO = null;
        private deleCallbackFunction delegateThreadTimerForDigitalIO = null;
        private deleCallbackFunction delegateThreadTimerForAnalogIO = null;
        private deleCallbackFunction delegateThreadTimerForMotion = null;
        private deleCallbackFunction delegateThreadTimerForMotionGathering = null;
        private deleCallbackFunction delegateThreadTimerForCommunication = null;
        private deleCallbackFunction delegateThreadTimerForETC = null;
        #endregion

        #region Cylinder
        private DelegateForReadingIO delegateCylinderForReadingInput = null;
        private DelegateForReadingIO delegateCylinderForReadingOutput = null;
        private DelegateForWritingIO delegateCylinderForWritingOutput = null;
        #endregion

        #region Interrupt
        private DelegateForReadingInput delegateInterruptForReadingInput = null;
        private DelegateForWriteDigitalOutput delegateInterruptForWriteOutput = null;
        private DelegateForInterruptAction delegateInterruptForActionStart = null;
        private DelegateForInterruptAction delegateInterruptForActionStop = null;
        private DelegateForInterruptAction delegateInterruptForActionReset = null;
        private DelegateForInterruptAction delegateInterruptForActionAlarm = null;
        #endregion

        #region Trigger
        private Trigger_.DelegateForWritingOutput delegateTriggerForWritingOutput = null;
        private Trigger_.DelegateForIsOutputTransitionComplete delegateForIsOutputTransitionComplete = null;
        #endregion

        #endregion

        #region Instances for Obserber
        private Subject subjectEquipmentState = null;
        private Subject subjectAlarm = null;
        #endregion

        #region for Form progress
        FrameOfSystem3.Views.Functional.Form_Progress m_Progress = null;
        System.Timers.Timer m_timerForProgressForm = null;
        #endregion

        #region Instance for Interfaces
        RegisteredInterfaces m_pRegisteredInterface = null;
        #endregion Instance for Interfaces

        #region DLL instances
        private Motion_.Motion m_instanceMotion = null;
        private Socket m_instanceSocket = null;
        private Serial m_instanceSerial = null;
        private Vision_.Vision m_instanceVision = null;
        private Interrupt m_instanceInterrupt = null;
        private Cylinder m_instanceCylinder = null;
        private Trigger m_instanceTrigger = null;
        private RegisteredInstanceManager m_instanceRegisteredManager = null;
        #endregion

        #region Controller
        Vision_.VisionController m_visionController = null;
        AnalogIOController[] m_arAnalogIOController = null;
        DigitalIOController[] m_arDigitalIOController = null;
        MotionController[] m_arMotionController = null;       // 2023.02.13. jhlim [MOD] 멀티 컨트롤러 사용을 위해 배열로 변경
        #endregion
        #endregion Variables

        #region Contructor & Destructor
        public Initializer() { }

        #endregion

        #region Internal Interface
        /// <summary>
        /// 2020.05.12 by yjlee [ADD] Register the event of the observer.
        /// </summary>
        private void RegisterObserverEvent()
        {
            subjectEquipmentState = EquipmentState_.EquipmentState.GetInstance();
            subjectAlarm = Alarm_.Alarm.GetInstance();

            Interrupt.GetInstance().RegisterSubject(subjectEquipmentState);

            Trigger.GetInstance().RegisterSubject(subjectEquipmentState);
            Trigger.GetInstance().RegisterSubject(subjectAlarm);

            Config.ConfigAlarm.GetInstance().RegisterSubject(subjectAlarm);
        }

        /// <summary>
        /// 2020.10.07 by yjlee [ADD] Get the instances of the dlls.
        /// </summary>
        private void GetDllInstance()
        {
            m_instanceMotion = Motion_.Motion.GetInstance();
            m_instanceSocket = Socket_.Socket.GetInstance();
            m_instanceSerial = Serial_.Serial.GetInstance();
            m_instanceVision = Vision_.Vision.GetInstance();
            m_instanceInterrupt = Interrupt_.Interrupt.GetInstance();
            m_instanceCylinder = Cylinder.GetInstance();
            m_instanceTrigger = Trigger.GetInstance();
            m_instanceRegisteredManager = RegisteredInstanceManager.GetInstance();
        }

        /// <summary>
        /// 2020.05.12 by yjlee [ADD] Set the thread timer to run.
        /// </summary>
        private bool SetThreadTimer()
        {
            delegateThreadTimerForFileIO = new deleCallbackFunction(FileIOManager_.FileIOManager.GetInstance().Execute);
            ThreadTimer.GetInstance().AddTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_FILEIO
                , ThreadTimerInterval.THREADTIMER_INTERVAL_FILEIO
                , delegateThreadTimerForFileIO);
            ThreadTimer.GetInstance().StartTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_FILEIO);

            delegateThreadTimerForDigitalIO = new deleCallbackFunction(DigitalIO.GetInstance().Execute);
            ThreadTimer.GetInstance().AddTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_DIGITALIO
                , ThreadTimerInterval.THREADTIMER_INTERVAL_DIGITALIO
                , delegateThreadTimerForDigitalIO);
            ThreadTimer.GetInstance().StartTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_DIGITALIO);

            delegateThreadTimerForAnalogIO = new deleCallbackFunction(AnalogIO.GetInstance().Execute);
            ThreadTimer.GetInstance().AddTimer((int)EN_THREADTIMER_INDEX.THERADTIMER_INDEX_ANALOGIO
                , ThreadTimerInterval.THREADTIMER_INTERVAL_ANALOGIO
                , delegateThreadTimerForAnalogIO);
            ThreadTimer.GetInstance().StartTimer((int)EN_THREADTIMER_INDEX.THERADTIMER_INDEX_ANALOGIO);

            delegateThreadTimerForMotion = new deleCallbackFunction(Motion_.Motion.GetInstance().Execute);
            ThreadTimer.GetInstance().AddTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MOTION
                , ThreadTimerInterval.THREADTIMER_INTERVAL_MOTION
                , delegateThreadTimerForMotion);
            ThreadTimer.GetInstance().StartTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MOTION);

            delegateThreadTimerForMotionGathering = new deleCallbackFunction(ExecuteForMotionGathering);
            ThreadTimer.GetInstance().AddTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MOTION_GATHERING
                , ThreadTimerInterval.THREADTIMER_INTERVAL_MOTION_GATHERING
                , delegateThreadTimerForMotionGathering);
            ThreadTimer.GetInstance().StartTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_MOTION_GATHERING);

            delegateThreadTimerForCommunication = new deleCallbackFunction(ExecuteForCommunication);
            ThreadTimer.GetInstance().AddTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_COMMUNICATION
                , ThreadTimerInterval.THREADTIMER_INTERVAL_COMMUNICATION
                , delegateThreadTimerForCommunication);
            ThreadTimer.GetInstance().StartTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_COMMUNICATION);

            // 2024.06.14. jhlim [ADD] etc timer는 아래서 설정한다.
            //delegateThreadTimerForETC = new deleCallbackFunction(ExecuteForETC);
            //ThreadTimer.GetInstance().AddTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_ETC_INSTANCES
            //    , ThreadTimerInterval.THREADTIMER_INTERVAL_ETC_INSTANCES
            //    , delegateThreadTimerForETC);
            //ThreadTimer.GetInstance().StartTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_ETC_INSTANCES);

            return true;
        }

        private void StartETCThreadTimer()
        {
            delegateThreadTimerForETC = new deleCallbackFunction(ExecuteForETC);
            ThreadTimer.GetInstance().AddTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_ETC_INSTANCES
                , ThreadTimerInterval.THREADTIMER_INTERVAL_ETC_INSTANCES
                , delegateThreadTimerForETC);
            ThreadTimer.GetInstance().StartTimer((int)EN_THREADTIMER_INDEX.THREADTIMER_INDEX_ETC_INSTANCES);
        }
        #region Execute
        /// <summary>
        /// 2020.05.21 by yjlee [ADD] Execute to gather the data for the motion.
        /// </summary>
        private void ExecuteForMotionGathering()
        {
            var enControllerState = Motion_.CONTROLLER_STATE.STOP;
            m_instanceMotion.ExecuteForGathering(ref enControllerState);
        }

        /// <summary>
        /// 2020.05.21 by yjlee [ADD] Execute to communicate the external devices.
        /// </summary>
        private void ExecuteForCommunication()
        {
            m_instanceSocket.Execute();
            m_instanceSerial.Execute();
            m_instanceVision.Execute();
        }

        /// <summary>
        /// 2020.05.21 by yjlee [ADD] Execute for the ETC Instances.
        /// </summary>
        private void ExecuteForETC()
        {
            m_instanceInterrupt.Execute();
            m_instanceCylinder.Execute();
            m_instanceTrigger.Execute();
            Scheduler.GetInstance().Excute();
            EquipmentProperty.EquipmentProperty.GetInstance().Execute();
            EquipmentMonitor.RAM_Metrics.GetInstance().Execute();

            if (false == FrameOfSystem3.Task.TaskOperator.GetInstance().IsExiting)
            {
                SECSGEM.ScenarioOperator.Instance.Execute();
                EFEM.Modules.LoadPortManager.Instance.Execute();
                EFEM.Modules.AtmRobotManager.Instance.Execute();
                EFEM.Modules.RFIDManager.Instance.ExecuteAll();
                EFEM.Modules.ProcessModuleGroup.Instance.ExecuteAll();
                ExternalDevice.Serial.FanFilterUnit.FanFilterUnitManager.Instance.Execute();    
                
                ExternalDevice.Socket.ModbusTCPClient.GetInstance((int)Define.DefineEnumProject.Socket.EN_SOCKET_INDEX.MODBUS).Execute();
            }

        }
        #endregion

        #region Progress Form
        private bool ShowProgressForm()
        {
            if (!m_bShowProgressWhenAttachedDebuger == System.Diagnostics.Debugger.IsAttached)
            {
                return true;
            }
            else if (null != m_Progress && false != m_Progress.IsFormLoad())
            {
                m_Progress.SetEndStep(Enum.GetValues(typeof(EN_INITIALIZATION_STEP)).Length);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 2020.05.13 by yjlee [ADD] Initialize the progress form.
        /// </summary>
        private void InitProgressForm()
        {
            if (!m_bShowProgressWhenAttachedDebuger == System.Diagnostics.Debugger.IsAttached)
            {
                return;
            }

            #region Init Timer
            m_timerForProgressForm = new System.Timers.Timer();
            m_timerForProgressForm.BeginInit();
            m_timerForProgressForm.Elapsed += new System.Timers.ElapsedEventHandler(CallbackFunctionForTimer);
            m_timerForProgressForm.AutoReset = false;
            m_timerForProgressForm.Interval = InitializationProgressForm.INTERVAL_CHECKING_INIT_STATE;
            m_timerForProgressForm.EndInit();
            #endregion

            m_timerForProgressForm.Start();
        }

        /// <summary>
        /// 2020.05.13 by yjlee [ADD] Release the resources.
        /// </summary>
        private void ExitProgressForm()
        {
            if (null == m_timerForProgressForm)
            {
                return;
            }

            string temp = Enum.GetValues(typeof(EN_INITIALIZATION_STEP)).Length.ToString();
            m_Progress.EnqueueResult(true, ref temp);

            m_timerForProgressForm.Dispose();
            m_timerForProgressForm = null;
        }

        /// <summary>
        /// 2020.05.13 by yjlee [ADD] It will be called by the timer routine.
        /// </summary>
        private void CallbackFunctionForTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            m_Progress = new Views.Functional.Form_Progress(InitializationProgressForm.INTERVAL_CHECKING_QUEUE_OF_PROGRESS);
            m_Progress.ShowDialog();

            m_Progress.Dispose();
            m_Progress = null;
        }
        #endregion

        private void UpdateEquipmentProperty()
        {
            if (EquipmentProperty.RawMaterialPortManager.GetInstance().GetRawMaterialExist())
                EquipmentProperty.EquipmentProperty.GetInstance().SetValue(EquipmentProperty.EN_EQUIPMENT_PROPERTY_LIST.MATERIAL_EXIST, EquipmentProperty.EN_MATERIAL_EXIST_VALUES.EXIST);
            else
                EquipmentProperty.EquipmentProperty.GetInstance().SetValue(EquipmentProperty.EN_EQUIPMENT_PROPERTY_LIST.MATERIAL_EXIST, EquipmentProperty.EN_MATERIAL_EXIST_VALUES.EMPTY);
        }
        #endregion

        #region External Interface
        /// <summary>
        /// 2020.02.05 by yjlee [ADD] Initialize the software.
        /// </summary>
        public void Init(DelegateForInterruptAction delegateStart
            , DelegateForInterruptAction delegateStop
            , DelegateForInterruptAction delegateReset
            , DelegateForInterruptAction delegateAlarm)
        {
            m_enInitializeStep = EN_INITIALIZATION_STEP.INIT_START;

            // 2020.05.18 by yjlee [ADD] Set an interrupt actions.
            delegateInterruptForActionStart = delegateStart;
            delegateInterruptForActionStop = delegateStop;
            delegateInterruptForActionReset = delegateReset;
            delegateInterruptForActionAlarm = delegateAlarm;

            InitProgressForm();
        }
        /// <summary>
        /// 2020.02.05 by yjlee [ADD] Exit the software.
        /// 2021.08.18. by shkim [MOD] Task Thread가 정지되기 전에 Recipe 인스턴스를 소멸시키면 Exception 발생하여 위치 변경
        /// </summary>
        public void Exit()
        {
            // Save 이후 인스턴스 소멸동작이 아닌
            // 클래스 내부에서 사용하는 인스턴스들을 유지하고, Process Recipe Save 동작만 하도록 변경
            ShutDown();

            #region Recipe
            Recipe.Recipe.GetInstance().SaveProcessRecipe();
            #endregion

            #region Logging
            Log.LogManager.GetInstance().Exit();
            //Log.LogWriter.GetInstance().Deactivate(); // 2025.05.29 by junho [DEL] windows event 받아서 deactivate하도록 개선 (종료시 항상 exception 발생현상 개선)
            #endregion

            #region Config
            Account_.Account.GetInstance().Exit();
            Alarm.GetInstance().Exit();
            Socket.GetInstance().Exit();
            Serial.GetInstance().Exit();
            Cylinder.GetInstance().Exit();
            Interrupt.GetInstance().Exit();
            Trigger.GetInstance().Exit();
            AnalogIO_.AnalogIO.GetInstance().Exit();
            DigitalIO_.DigitalIO.GetInstance().Exit();
            Motion_.Motion.GetInstance().Exit();
            Vision_.Vision.GetInstance().Exit();
            Language_.Language.GetInstance().Exit();
            JogManager_.JogManager.GetInstance().Exit();
            #endregion

            #region Task
            RegisteredInstances_.RegisteredInstanceManager.GetInstance().Exit();
            TaskAction_.TaskActionFlow.GetInstance().Exit();
            TaskAction_.TaskActionManager.GetInstance().Exit();
            TaskDevice_.TaskDevice.GetInstance().Exit();
            #endregion

            #region Recipe
            RecipeManager_.RecipeManager.GetInstance().Exit();
            #endregion

            #region File Management
            FileBorn_.FileBorn.GetInstance().Exit();
            FileComposite_.FileComposite.GetInstance().Exit();
            FileIOManager_.FileIOManager.GetInstance().Exit();
            #endregion

            #region Thread Timer
            ThreadTimer.GetInstance().Exit();
            #endregion
        }
        /// <summary>
        /// 2020.05.12 by yjlee [ADD] Initialize the instances of the DLL.
        /// </summary>
        public bool DoInitializeSequence()
        {
            bool bResult = false;
            string strContentsResult = null;

            switch (m_enInitializeStep)
            {
                case EN_INITIALIZATION_STEP.INIT_START:
                    if (false == ShowProgressForm()) { return false; }
                    strContentsResult = "The System is being Start... ";
                    break;

                #region Observer
                case EN_INITIALIZATION_STEP.INIT_OBSERVER_START:
                    strContentsResult = "The observers are being attached to the subjects... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_OBSERVER_END:
                    RegisterObserverEvent();
                    bResult = true;
                    break;
                #endregion

                #region File IO
                case EN_INITIALIZATION_STEP.INIT_FILEIO_START:
                    strContentsResult = "The file I/O is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_FILEIO_END:
                    Log.LogWriter.GetInstance().Activate();
                    bResult = FileIOManager_.FileIOManager.GetInstance().Init();
                    break;
                #endregion

                #region Account
                case EN_INITIALIZATION_STEP.INIT_ACCOUNT_START:
                    strContentsResult = "The accout is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_ACCOUNT_END:
                    bResult = Account_.Account.GetInstance().Init(System.Diagnostics.Debugger.IsAttached);

                    break;
                #endregion

                #region Alarm
                case EN_INITIALIZATION_STEP.INIT_ALARM_START:
                    strContentsResult = "The alarm is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_ALARM_END:
                    bResult = Alarm_.Alarm.GetInstance().Init();
                    break;
                #endregion

                #region Socket
                case EN_INITIALIZATION_STEP.INIT_SOCKET_START:
                    strContentsResult = "The socket communication is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_SOCKET_END:
                    bResult = Socket_.Socket.GetInstance().Init();
                    break;
                #endregion

                #region Serial
                case EN_INITIALIZATION_STEP.INIT_SERIAL_START:
                    strContentsResult = "The serial communication is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_SERIAL_END:
                    bResult = Serial.GetInstance().Init();
                    break;
                #endregion

                #region Analog IO
                case EN_INITIALIZATION_STEP.INIT_ANALOG_IO_START:
                    strContentsResult = "The analog I/O is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_ANALOG_IO_END:
                    {
                        m_arAnalogIOController = new AnalogIOController[1];
                        Define.DefineEnumProject.AppConfig.EN_ANALOG_IO_CONTROLLER controllerName
                            = Work.AppConfigManager.Instance.ControllerAnalog;

                        switch (controllerName)
                        {
                            case Define.DefineEnumProject.AppConfig.EN_ANALOG_IO_CONTROLLER.CREVIS_MODBUS_TCP:
                                m_arAnalogIOController[0] = new FrameOfSystem3.Controller.AnalogIO.CrevisModbusAnalogIOController();
                                break;
                            default:
                                m_arAnalogIOController[0] = null;
                                break;
                        }

                        bResult = AnalogIO.GetInstance().Init(ref m_arAnalogIOController);
                        if (m_arAnalogIOController[0] == null)
                        {
                            bResult = true;
                        }

                        bResult = true;
                    }
                    break;
                #endregion

                #region Digital IO
                case EN_INITIALIZATION_STEP.INIT_DIGITAL_IO_START:
                    strContentsResult = "The digital I/O is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_DIGITAL_IO_END:
                    {
                        m_arDigitalIOController = new DigitalIOController[1];
                        Define.DefineEnumProject.AppConfig.EN_DIGITAL_IO_CONTROLLER controllerName
                            = Work.AppConfigManager.Instance.ControllerDigital;

                        switch (controllerName)
                        {
                            case Define.DefineEnumProject.AppConfig.EN_DIGITAL_IO_CONTROLLER.CREVIS_MODBUS_TCP:
                                m_arDigitalIOController[0] = new FrameOfSystem3.Controller.DigitalIO.CrevisModbusDigitalIOController();
                                break;
                            default:
                                m_arDigitalIOController[0] = null;
                                break;
                        }

                        bResult = DigitalIO.GetInstance().Init(ref m_arDigitalIOController);
                        if (m_arDigitalIOController[0] == null)
                            bResult = true;
                    }
                    break;
                #endregion

                #region Cylinder
                case EN_INITIALIZATION_STEP.INIT_CYLINDER_START:
                    strContentsResult = "The cylinder is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_CYLINDER_END:
                    delegateCylinderForReadingInput = new DelegateForReadingIO(DigitalIO.GetInstance().ReadInput);
                    delegateCylinderForReadingOutput = new DelegateForReadingIO(DigitalIO.GetInstance().ReadOutput);
                    delegateCylinderForWritingOutput = new DelegateForWritingIO(DigitalIO.GetInstance().WriteOutput);

                    bResult = Cylinder.GetInstance().Init(delegateCylinderForReadingInput
                        , delegateCylinderForReadingOutput
                        , delegateCylinderForWritingOutput);
                    break;
                #endregion

                #region Interrupt
                case EN_INITIALIZATION_STEP.INIT_INTERRUPT_START:
                    strContentsResult = "The interrupt is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_INTERRUPT_END:
                    delegateInterruptForReadingInput = new DelegateForReadingInput(DigitalIO.GetInstance().ReadInput);
                    delegateInterruptForWriteOutput = new DelegateForWriteDigitalOutput(DigitalIO.GetInstance().WriteOutput);

                    bResult = Interrupt.GetInstance().Init(delegateInterruptForReadingInput
                        , delegateInterruptForWriteOutput
                        , delegateInterruptForActionStart
                        , delegateInterruptForActionStop
                        , delegateInterruptForActionReset
                        , delegateInterruptForActionAlarm);
                    break;
                #endregion

                #region Trigger
                case EN_INITIALIZATION_STEP.INIT_TRIGGER_START:
                    strContentsResult = "The trigger is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_TRIGGER_END:
                    delegateTriggerForWritingOutput = new DelegateForWritingOutput(DigitalIO.GetInstance().WriteOutput);
                    delegateForIsOutputTransitionComplete = (int index, ref bool currentIOState, bool defaultState) =>
                    {
                        return DigitalIO.GetInstance().IsTransitionComplete(index, false, ref currentIOState, defaultState);
                    };

                    bResult = Trigger.GetInstance().Init(delegateTriggerForWritingOutput, delegateForIsOutputTransitionComplete);
                    break;
                #endregion

                #region Motion
                case EN_INITIALIZATION_STEP.INIT_MOTION_START:
                    strContentsResult = "The motion is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_MOTION_END:
                    {
                        m_arMotionController = new MotionController[1];
                        Define.DefineEnumProject.AppConfig.EN_MOTION_CONTROLLER controllerName = Work.AppConfigManager.Instance.ControllerMotion;
                        switch (controllerName)
                        {
                            default:
                                m_arMotionController[0] = null;
                                break;
                        }

                        bResult = Motion_.Motion.GetInstance().Init(ref m_arMotionController, Define.DefineConstant.Motion.INTERVAL_CHECKING_CONNECTION);

                        bResult = true;
                    }
                    break;
                #endregion

                #region Langauge
                case EN_INITIALIZATION_STEP.INIT_LANGUAGE_START:
                    strContentsResult = "The language is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_LANGUAGE_END:
                    {
                        var language = Language_.Language.GetInstance();
                        bResult = language.Init();
                        if (bResult)
                        {
                            language.SetLanguage(Work.AppConfigManager.Instance.Language);
                        }
                    }
                    break;
                #endregion

                #region TaskDevice
                case EN_INITIALIZATION_STEP.INIT_TASK_DEVICE_START:
                    strContentsResult = "The Task Device is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_TASK_DEVICE_END:
                    bResult = TaskDevice.GetInstance().Init();
                    break;
                #endregion

                #region Registered Instances
                case EN_INITIALIZATION_STEP.INIT_REGISTERED_INSTANCES_START:
                    strContentsResult = "The registered manager is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_REGISTERED_INSTANCES_END:
                    Interface.RegisteredInterface pInterface = new Interface.RegisteredInterface();
                    m_pRegisteredInterface = pInterface as RegisteredInterfaces;

                    bResult = RegisteredInstanceManager.GetInstance().Init(m_pRegisteredInterface);
                    break;
                #endregion

                #region Thread Timer
                case EN_INITIALIZATION_STEP.INIT_THREADTIMER_START:
                    strContentsResult = "The ThreadTimer is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_THREADTIMER_END:
                    GetDllInstance();

                    bResult = SetThreadTimer();
                    break;
                #endregion

                #region Recipe
                case EN_INITIALIZATION_STEP.INIT_RECIPE_START:
                    strContentsResult = "The instance of the recipe is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_RECIPE_END:
                    bResult = RecipeManager_.RecipeManager.GetInstance().Init();
                    break;
                #endregion

                #region Config Files
                case EN_INITIALIZATION_STEP.INIT_CONFIG_INSTANCES_START:
                    strContentsResult = "The system makes the instances for the device configurations... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_CONFIG_INSTANCES_END:
                    Functional.Storage.GetInstance().Init();
                    FileBorn_.FileBorn.GetInstance().Init();

                    bResult = true;
                    bResult &= Config.ConfigTask.GetInstance().Init();
                    bResult &= Config.ConfigDigitalIO.GetInstance().Init();
                    bResult &= Config.ConfigAnalogIO.GetInstance().Init();
                    bResult &= Config.ConfigCylinder.GetInstance().Init();
                    bResult &= Config.ConfigSocket.GetInstance().Init();
                    bResult &= Config.ConfigSerial.GetInstance().Init();
                    bResult &= Config.ConfigInterrupt.GetInstance().Init();
                    bResult &= Config.ConfigTrigger.GetInstance().Init();
                    bResult &= Config.ConfigLanguage.GetInstance().Init();
                    bResult &= Config.ConfigAlarm.GetInstance().Init();
                    bResult &= Config.ConfigMotion.GetInstance().Init();
                    bResult &= Config.ConfigMotionSpeed.GetInstance().Init();
                    bResult &= Config.ConfigJog.GetInstance().Init();
                    bResult &= Config.ConfigDevice.GetInstance().Init();
                    bResult &= Config.ConfigPort.GetInstance().Init();
                    bResult &= Config.ConfigDynamicLink.GetInstance().Init();
                    bResult &= Config.ConfigFlow.GetInstance().Init();
                    bResult &= Config.ConfigTool.GetInstance().Init();          // 2021.09.27 by jhchoo [ADD]
                    bResult &= Config.ConfigWCF.GetInstance().Init();           // 2024.02.01 by jhlee [ADD]
                    bResult &= Account.CAccount.GetInstance().Init();
                    break;
                #endregion

                #region Task
                case EN_INITIALIZATION_STEP.INIT_TASK_START:
                    strContentsResult = "The system makes the instances of the task... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_TASK_END:
                    bResult = Task.TaskOperator.GetInstance().InitializeTask();
                    break;
                #endregion

                #region Load Recipe
                case EN_INITIALIZATION_STEP.LOAD_RECIPE_START:
                    strContentsResult = "The system is loading the file of the recipe... ";
                    break;

                case EN_INITIALIZATION_STEP.LOAD_RECIPE_END:
                    bResult = Recipe.Recipe.GetInstance().Init();

                    // 2025.06.12 by junho [ADD] Previous value storage 기능 추가
                    Recipe.PreviousValueStorage.Instance.Init();
                    break;
                #endregion

                #region Vision
                //case EN_INITIALIZATION_STEP.INIT_VISION_START:
                //    strContentsResult       = "The vision is being initialized... ";
                //    break;

                //case EN_INITIALIZATION_STEP.INIT_VISION_END:
                //   Vision_.Vision vision = Vision_.Vision.GetInstance();
                //    bResult = vision.Init(new Controller.Vision.ProtecVisionController((int)Define.DefineEnumProject.Socket.EN_SOCKET_INDEX.VISION), Define.DefineConstant.Vision.COUNT_CAM);
                //    if(bResult)
                //    {
                //        FrameOfSystem3.Task.TaskOperator.GetInstance().AddDelegateSetOperation(new RunningMain_.RunningMain.DelegateWithSetOperation(vision.ResetVision));

                //        // check here : vision algorithm assine
                //        //vision.AddResultParsingDelegate((int)EN_CAMERA_LIST.ALIGN, (int)EN_VISION_ALGORITHM.FLUX_SUBJECT_1st, VisionResultParser_BP5000IR.ALIGN_MATCHING_DB);
                //    }
                //    break;
                #endregion

                #region LOG
                case EN_INITIALIZATION_STEP.INIT_LOG_START:
                    strContentsResult = "The log instance is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_LOG_END:
                    {
                        var logManager = Log.LogManager.GetInstance();
                        bResult = logManager.Init();
                        logManager.CaptionMaterialType = "WAFER";   // TODO : Material type 설정 (나중에 해도 됨)
                        logManager.RegisterGetLotIdFunction(new Log.LogManager.DeleGetLotId(() => { return Log.LogManager.EMPTY_DATA; }));  // TODO : lot id 반환 함수 등록 (나중에 해도 됨)
                        logManager.RegisterGetMaterialIdFromTaskName(new Log.LogManager.DeleGetMaterialIdFromTaskName((taskName) => { return string.Format("{0}_{1}", taskName, Log.LogManager.EMPTY_DATA); }));  // TODO : 자재 id 반환 함수 등록 (나중에 해도 됨)
                    }
                    break;
                #endregion

                #region INTERLOCK
                case EN_INITIALIZATION_STEP.INIT_INTERLOCK_START:
                    strContentsResult = "The Interlock instance is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_INTERLOCK_END:
                    bResult = Config.ConfigInterlock.GetInstance().Init();
                    break;
                #endregion

                #region SCHEDULER
                case EN_INITIALIZATION_STEP.INIT_SCHEDULER_START:
                    strContentsResult = "The Scheduler is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_SCHEDULER_END:
                    //                     Scheduler.Schedule DeleteLog = new Scheduler.Schedule();
                    //                     DeleteLog.Hour = 0;
                    //                     DeleteLog.delFunction = new Scheduler.delGenerateFunction(FunctionsETC.DeleteLogFile);
                    //                     Scheduler.GetInstance().AddSchedule("DeleteFile", DeleteLog);

                    //                     Scheduler.Schedule BackUpFile = new Scheduler.Schedule();
                    //                     BackUpFile.Hour = 0;
                    //                     BackUpFile.delFunction = new Scheduler.delGenerateFunction(FunctionsETC.ImportantFileBackup);
                    //                     Scheduler.GetInstance().AddSchedule("BackUpFile", BackUpFile);

                    bResult = Scheduler.GetInstance().Init();
                    break;
                #endregion

                #region EQUIPMENT PROPERTY
                case EN_INITIALIZATION_STEP.INIT_EQUIPMENT_PROPERTY_START:
                    strContentsResult = "The Equipment Property is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_EQUIPMENT_PROPERTY_END:
                    EquipmentProperty.EquipmentProperty.GetInstance().delegateUpdateProperty = new EquipmentProperty.DelegateUpdateProperty(UpdateEquipmentProperty);
                    bResult = true;
                    break;
                #endregion

                #region RAM Metrics
                case EN_INITIALIZATION_STEP.INIT_RAM_METRICS_START:
                    strContentsResult = "The RAM Metrics is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_RAM_METRICS_END:
                    EquipmentMonitor.RAM_Metrics.GetInstance().Init();
                    bResult = true;
                    break;
                #endregion

                #region FTP
                case EN_INITIALIZATION_STEP.INIT_FTP_START:
                    strContentsResult = "The FTP is being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_FTP_END:
                    #region FTP
                    Config.ConfigFTP.GetInstance().Init();
                    #endregion
                    bResult = true;
                    break;
                #endregion

                #region <Init EFEM Modules>
                case EN_INITIALIZATION_STEP.INIT_EFEM_MODULES_START:
                    strContentsResult = "The EFEM modules are being initialized... ";
                    break;
                case EN_INITIALIZATION_STEP.INIT_EFEM_MODULES_END:
                    {
                        SECSGEM.IGem300ScenarioService gem300Service = new SECSGEM.Gem300ScenarioService();

                        ConfigureScenarioOperator(gem300Service);

                        BuildJobManager(gem300Service);

                        ExecuteBeforeInitialization(gem300Service);

                        BuildJobBinder();

                        RecoverJobManager();

                        int i;

                        BuildLoadPorts(gem300Service);

                        BuildRobots();

                        BuildProcessModules();

                        StartETCThreadTimer();

                        bResult = true;
                    }
                    break;
                #endregion </Init EFEM Modules>

                #region <External Devices>
                case EN_INITIALIZATION_STEP.INIT_EXTERNAL_DEVICE_START:
                    strContentsResult = "The external devices are being initialized... ";
                    break;
                case EN_INITIALIZATION_STEP.INIT_EXTERNAL_DEVICE_END:
                    {
                        #region <Modbus>
                        // 2024.11.06. jhlim [DEL] 필요한가??
                        //ExternalDevice.Socket.ModbusTCPClient.GetInstance((int)Define.DefineEnumProject.Socket.EN_SOCKET_INDEX.MODBUS).Init();
                        #endregion </Modbus>

                        #region <Fan Filter Unit>
                        bResult &= ExternalDevice.Serial.FanFilterUnit.FanFilterUnitManager.Instance.Activate();
                        ExternalDevice.Serial.FanFilterUnit.FanFilterUnitController controller
                            = new ExternalDevice.Serial.FanFilterUnit.Bluecord.FanFilterUnitControllerBluecord((int)Define.DefineEnumProject.Serial.EN_SERIAL_INDEX.FFU,
                            Define.DefineEnumBase.Common.EN_CONNECTION_TYPE.SERIAL,
                            Work.AppConfigManager.Instance.UseDifferentialPressureMode,
                            Work.AppConfigManager.Instance.CountFanFilterUnit);
                        ExternalDevice.Serial.FanFilterUnit.FanFilterUnitManager.Instance.AddController(controller);
                        #endregion </Fan Filter Unit>

                        bResult = true;
                    }
                    break;

                #endregion </External Devices>

                #region <Init EFEM Module Information>
                case EN_INITIALIZATION_STEP.INIT_EFEM_MODULE_INFORMATION_START:
                    strContentsResult = "The EFEM module informations are being initialized... ";
                    break;

                case EN_INITIALIZATION_STEP.INIT_EFEM_MODULE_INFORMATION_END:
                    {
                        ExecuteAfterInitialization();

                        // Language 설정
                        string language = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT, Recipe.PARAM_EQUIPMENT.MachineLanguage.ToString(), Config.ConfigLanguage.EN_PARAM_LANGUAGE.ENGLISH.ToString());
                        if (Enum.TryParse(language, out Config.ConfigLanguage.EN_PARAM_LANGUAGE targetLanguage))
                        {
                            Config.ConfigLanguage.GetInstance().SetLanguage(targetLanguage);
                        }

                        bResult = true;

                    }
                    break;
                #endregion </Init EFEM Module Information>

                case EN_INITIALIZATION_STEP.INIT_END:
                    ExitProgressForm();
                    return true;
            }

            //Release 모드이다.
            if (m_bShowProgressWhenAttachedDebuger == System.Diagnostics.Debugger.IsAttached)
            {
                int nInitializeStep = (int)m_enInitializeStep;

                if (0 != nInitializeStep % 2 && nInitializeStep > 2)
                {
                    m_Progress.EnqueueResult(false, ref bResult);
                }
                else
                {
                    m_Progress.EnqueueResult(true, ref strContentsResult);
                }
            }

            ++m_enInitializeStep;

            return false;
        }
        /// <summary>
        /// 2020.05.18 by yjlee [ADD] Check whether the initialization sequence is end or not.
        /// </summary>
        public bool IsInitializationEnd()
        {
            return m_enInitializeStep == EN_INITIALIZATION_STEP.INIT_END
                && null == m_Progress;
        }

        #endregion

        #region <EFEM Only>
        EFEM.Jobs.Repository.IRemovedBindingTargetRepository _removedTargetRepository;
        EFEM.Jobs.Binding.IJobBindingTargetIndex _jobBindingTargetIndex;

        private void BuildJobManager(SECSGEM.IGem300ScenarioService gem300Service)
        {
            //EFEM.Jobs.Repository.IOrderedRepository<EFEM.Jobs.Domain.ControlJob, string> controlJobRepository =
            //    new EFEM.Jobs.Repository.InMemoryRepository<EFEM.Jobs.Domain.ControlJob, string>();

            //EFEM.Jobs.Repository.IOrderedRepository<EFEM.Jobs.Domain.ProcessJob, string> processJobRepository =
            //    new EFEM.Jobs.Repository.InMemoryRepository<EFEM.Jobs.Domain.ProcessJob, string>();

            //EFEM.Jobs.Repository.IJobRelationRepository relationRepository =
            //    new EFEM.Jobs.Repository.InMemoryJobRelationRepository();

            EFEM.Jobs.Repository.IOrderedRepository<EFEM.Jobs.Domain.ControlJob, string> controlJobRepository =
                new EFEM.Jobs.Repository.JsonOrderedJobRepository<EFEM.Jobs.Domain.ControlJob>(
            System.IO.Path.Combine(
                EFEM.Defines.Common.RecoveryFileDefines.JobRecoveryFilePath,
                "ControlJob"),
                new EFEM.Jobs.Repository.ControlJobRecoveryJsonAdapter());

            EFEM.Jobs.Repository.IOrderedRepository<EFEM.Jobs.Domain.ProcessJob, string> processJobRepository =
                new EFEM.Jobs.Repository.JsonOrderedJobRepository<EFEM.Jobs.Domain.ProcessJob>(
                    System.IO.Path.Combine(
                        EFEM.Defines.Common.RecoveryFileDefines.JobRecoveryFilePath,
                        "ProcessJob"),
                    new EFEM.Jobs.Repository.ProcessJobRecoveryJsonAdapter());

            var relationStore =
                new EFEM.Jobs.Repository.JsonJobRelationRepository(
                    System.IO.Path.Combine(
                        EFEM.Defines.Common.RecoveryFileDefines.JobRecoveryFilePath,
                        "Relation"));

            EFEM.Jobs.Repository.IJobRelationRepository relationRepository =
                relationStore;

            _removedTargetRepository =
                relationStore;

            EFEM.Jobs.Repository.JobStorageRecovery.Repair(
                controlJobRepository,
                processJobRepository,
                relationRepository,
                _removedTargetRepository);

            EFEM.Jobs.Manager.ISecsGemResultEvaluator resultEvaluator =
                new EFEM.Jobs.Manager.SecsGemResultEvaluator();

            EFEM.Jobs.Policy.IProcessJobRemovalPolicy removingPolicy = 
                new EFEM.Jobs.Policy.RetainLinkedProcessJobUntilControlJobRemovalPolicy();

            EFEM.Jobs.Manager.JobManager.ConfigureDeferred(
                gem300Service,
                controlJobRepository,
                processJobRepository,
                relationRepository,
                resultEvaluator,
                removingPolicy);
        }
        IMaterialTrackingStorageContextFactory _storageContextFactory;
        private void ExecuteBeforeInitialization(SECSGEM.IGem300ScenarioService gem300Service)
        {
            var provider = new EFEM.Defines.ProcessTypeProvider.AppConfigProcessTypeProvider();

            DateTime clock() => DateTime.Now;
            var dbPath = System.IO.Path.Combine(EFEM.Defines.Common.RecoveryFileDefines.RecoveryDatabasePath, $"{provider.GetProcessType()}.db");

            var (substrate, carrier) = MaterialExtraAttributeFactory.Create(Work.AppConfigManager.Instance.ProcessType);

            #region <저장소 생성>
            // 1) Json 읽기/쓰기
            //_storageContextFactory = new JsonMaterialTrackingStorageContextFactory(
            //    EFEM.Defines.Common.RecoveryFileDefines.LocationHistoryPath,
            //    EFEM.Defines.Common.RecoveryFileDefines.ProcessingHistoryPath,
            //    EFEM.Defines.Common.RecoveryFileDefines.RecoveryFilePath,
            //    EFEM.Defines.Common.RecoveryFileDefines.CarrierRecoveryFilePath);

            // 2) DB 읽기/쓰기
            //_storageContextFactory = new SqliteMaterialTrackingStorageContextFactory(
            //    dbPath,
            //    clock);

            // Json 읽기/쓰기, DB 쓰기 백업모드
            _storageContextFactory = new JsonAndSqliteMaterialTrackingStorageContextFactory(
                EFEM.Defines.Common.RecoveryFileDefines.LocationHistoryPath,
                EFEM.Defines.Common.RecoveryFileDefines.ProcessingHistoryPath,
                EFEM.Defines.Common.RecoveryFileDefines.RecoveryFilePath,
                EFEM.Defines.Common.RecoveryFileDefines.CarrierRecoveryFilePath,
                dbPath,
                clock);

            var storageContext = _storageContextFactory.Create(() => carrier.GetExtraKeys(), () => substrate.GetExtraKeys(), EFEM.Migrations.MigrationSteps.GetMigrationSteps());
            #endregion </저장소 생성>

            #region <이력 기록용 트래커 생성>
            var (locationStateService, substrateProcessingService) = SubstrateHistoryServicesFactory.Create(
                storageContext.LocationHistory,
                storageContext.ProcessingHistory,
                clock,
                null);
            #endregion </이력 기록용 트래커 생성>

            #region <기판 관리자 생성>
            EFEM.MaterialTracking.SubstrateManager.Configure(
                storageContext.Substrate,
                new List<EFEM.MaterialTracking.ISubstrateEventObserver> 
                {
                    storageContext.LocationHistory,
                    storageContext.ProcessingHistory,
                    SubstrateHistoryServicesFactory.HistoryTracker,
                },
                substrate,
                provider,
                gem300Service);
            #endregion </기판 관리자 생성>

            #region <캐리어 관리자 생성>
            storageContext.Carrier.RegisterListner(new EFEM.MaterialTracking.CarrierEventObserver(storageContext.Substrate, EFEM.MaterialTracking.SubstrateManager.Instance));
            EFEM.MaterialTracking.CarrierManagementServer.Configure(storageContext.Carrier, carrier, provider);
            #endregion </캐리어 관리자 생성>

            EFEM.MaterialTracking.SubstrateManager.Instance.SubstrateProcessingStateChanged += substrateProcessingService.OnSubstrateLocationStateChanged;
            EFEM.MaterialTracking.SubstrateManager.Instance.SubstrateLocationStateChanged += locationStateService.OnSubstrateLocationStateChanged;
            EFEM.MaterialTracking.SubstrateManager.Instance.SubstrateLocationChanged += locationStateService.OnSubstrateLocationChanged;
            EFEM.MaterialTracking.SubstrateManager.Instance.SubstrateSwapped += locationStateService.OnSubstrateSwapped;
            EFEM.MaterialTracking.SubstrateManager.Instance.SubstrateRecovered += locationStateService.LoadHistoryFromStorage;
        }

        private void BuildJobBinder()
        {
            _jobBindingTargetIndex =
                new EFEM.Jobs.Binding.InMemoryJobBindingTargetIndex();

            EFEM.Jobs.Binding.ISubstrateJobBinder _jobBinder
                = new EFEM.Jobs.Binding.SubstrateJobBinder(
                    EFEM.Jobs.Manager.JobManager.Instance,
                    EFEM.MaterialTracking.SubstrateManager.Instance,
                    EFEM.MaterialTracking.CarrierManagementServer.Instance,
                    _removedTargetRepository,
                    _jobBindingTargetIndex);

            EFEM.Jobs.Binding.SubstrateJobBindingService.Configure(_jobBinder);

            EFEM.Jobs.Completion.JobCompletionService.Configure(
                new EFEM.Jobs.Completion.JobCompletionEvaluator(
                    EFEM.Jobs.Manager.JobManager.Instance,
                    EFEM.Jobs.Binding.SubstrateJobBindingService.Instance,
                    EFEM.MaterialTracking.SubstrateManager.Instance));
        }
        private void RecoverJobManager()
        {
            // 1) JSON에서 복구된 ControlJob / ProcessJob을
            //    현재 복구된 Carrier / Substrate에 다시 바인딩한다.
            EFEM.Jobs.Manager.JobManager.Instance.RebindRecoveredJobs();

            // 2) 선택 사항:
            //    GEM300 SDK가 이미 통신 가능하고 초기화 완료된 상태라면
            //    SDK/Host 쪽 Job 상태와 다시 맞추기 위해 요청한다.
            //
            // 주의:
            // 아래 요청은 GEM 통신 준비가 끝난 뒤에 호출하는 것이 안전하다.
            // 아직 GEM Online/Initialize 전이라면 여기서 호출하지 말고,
            // GEM 초기화 완료 이벤트 이후로 이동시키는 것을 권장한다.

            //EFEM.Jobs.Manager.JobManager.Instance.RequestAllProcessJobIds();
            //EFEM.Jobs.Manager.JobManager.Instance.RequestAllControlJobIds();
            //EFEM.Jobs.Manager.JobManager.Instance.RequestControlJobHeadOfQueueInfo();
        }
        private void ConfigureScenarioOperator(SECSGEM.IGem300ScenarioService gem300Service)
        {
            Vision_.Vision.GetInstance().SetUseVision(false);

            Define.DefineEnumProject.AppConfig.EN_CUSTOMER customer =
                Work.AppConfigManager.Instance.Customer;

            bool created = SECSGEM.ScenarioOperatorLazyConfigFactory.TryCreate(
                customer,
                out var config);

            if (false == created)
            {
                return;
            }

            SECSGEM.ScenarioOperator.ConfigureDeferred(
                config.ScenarioFactory,
                config.DriverFactory,
                config.CfgPath,
                config.RecipePath);

            SECSGEM.ScenarioOperator.Instance.AttachGem300Service(gem300Service);

        }        
        private void BuildLoadPorts(SECSGEM.IGem300ScenarioService gem300Service)
        {
            int i;

            #region <LoadPorts>
            var loadPortControllerType
                = Work.AppConfigManager.Instance.LoadPortControllerType;

            int countLoadPort = Work.AppConfigManager.Instance.CountLoadPort;

            for (i = 0; i < countLoadPort; ++i)
            {
                int commIndex = 0;
                int portId = i + 1;
                string name = string.Format("LP{0}", portId);

                EFEM.Modules.LoadPort.LoadPortController lp;
                switch (loadPortControllerType)
                {
                    case Define.DefineEnumProject.AppConfig.EN_LOADPORT_CONTROLLER.NONE:        // DuraPort
                        {
                            commIndex = -1;
                            lp = new EFEM.Modules.LoadPort.LoadPortControllers.LoadPortControllerSimulator(portId, name, Define.DefineEnumBase.Common.EN_CONNECTION_TYPE.SERIAL, commIndex);
                        }
                        break;
                    case Define.DefineEnumProject.AppConfig.EN_LOADPORT_CONTROLLER.DURAPORT:
                        {
                            commIndex = (int)Define.DefineEnumProject.Serial.EN_SERIAL_INDEX.LOADPORT_1 + i;
                            lp = new EFEM.Modules.LoadPort.LoadPortControllers.DuraportController(
                                portId,
                                name,
                                Work.AppConfigManager.Instance.LoadPortLoadingModes,
                                Define.DefineEnumBase.Common.EN_CONNECTION_TYPE.SERIAL,
                                commIndex);
                        }
                        break;
                    case Define.DefineEnumProject.AppConfig.EN_LOADPORT_CONTROLLER.SELOP8:
                        {
                            commIndex = (int)Define.DefineEnumProject.Serial.EN_SERIAL_INDEX.LOADPORT_1 + i;
                            lp = new EFEM.Modules.LoadPort.LoadPortControllers.SELOP8Controller(portId, name, Define.DefineEnumBase.Common.EN_CONNECTION_TYPE.SERIAL, commIndex);
                        }
                        break;

                    default:
                        lp = null;
                        break;
                }

                EFEM.Defines.LoadPort.AutomatedMaterialHandlingSystemController amhsControl = null;

                if (false == Work.AppConfigManager.Instance.Customer.Equals(Define.DefineEnumProject.AppConfig.EN_CUSTOMER.S_TP))
                {
                    #region <LoadPorts - PWA-500>
                    switch (Work.AppConfigManager.Instance.InterfaceTypePIO)
                    {
                        case Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE.E84:
                            {
                                const int Offset = 8;

                                int saftyInterLockIndex = (int)Define.DefineEnumProject.DigitalIO.PWA500W.EN_DIGITAL_IN.PROTECTION_BAR_LP;

                                int baseInputIndex = (int)Define.DefineEnumProject.DigitalIO.PWA500W.EN_DIGITAL_IN.LP1_PIO_VALID + i * Offset;
                                int baseOutputIndex = (int)Define.DefineEnumProject.DigitalIO.PWA500W.EN_DIGITAL_OUT.LP1_PIO_L_REQ + i * Offset;

                                Dictionary<int, Tuple<int, string>> inputs = new Dictionary<int, Tuple<int, string>>();
                                Dictionary<int, Tuple<int, string>> outputs = new Dictionary<int, Tuple<int, string>>();
                                for (int index = 0; index < Offset; ++index)
                                {
                                    EFEM.Defines.LoadPort.E84InputSignals inputSignalEnums = EFEM.Defines.LoadPort.E84InputSignals.Valid + index;
                                    inputs[index] = new Tuple<int, string>(index + baseInputIndex, inputSignalEnums.ToString());

                                    EFEM.Defines.LoadPort.E84OutputSignals outputSignalEnums = EFEM.Defines.LoadPort.E84OutputSignals.LoadRequest + index;
                                    outputs[index] = new Tuple<int, string>(index + baseOutputIndex, outputSignalEnums.ToString());
                                }

                                amhsControl = new EFEM.Defines.LoadPort.CustomizedE84(i, saftyInterLockIndex, inputs, outputs);
                                amhsControl.AssignActionModeChangeBeforeCarrierLoad(EFEM.Modules.LoadPortManager.Instance.ChangeLoadPortMode);
                            }
                            break;
                        case Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE.E23:
                            {
                                const int Offset = 8;

                                int saftyInterLockIndex = (int)Define.DefineEnumProject.DigitalIO.PWA500W.EN_DIGITAL_IN.PROTECTION_BAR_LP;

                                int baseInputIndex = (int)Define.DefineEnumProject.DigitalIO.PWA500W.EN_DIGITAL_IN.LP1_PIO_VALID + i * Offset;
                                int baseOutputIndex = (int)Define.DefineEnumProject.DigitalIO.PWA500W.EN_DIGITAL_OUT.LP1_PIO_L_REQ + i * Offset;

                                Dictionary<int, Tuple<int, string>> inputs = new Dictionary<int, Tuple<int, string>>();
                                Dictionary<int, Tuple<int, string>> outputs = new Dictionary<int, Tuple<int, string>>();
                                for (int index = 0; index < Offset; ++index)
                                {
                                    EFEM.Defines.LoadPort.E23InputSignals inputSignalEnums = EFEM.Defines.LoadPort.E23InputSignals.Valid + index;
                                    inputs[index] = new Tuple<int, string>(index + baseInputIndex, inputSignalEnums.ToString());

                                    EFEM.Defines.LoadPort.E23OutputSignals outputSignalEnums = EFEM.Defines.LoadPort.E23OutputSignals.LoadRequest + index;
                                    outputs[index] = new Tuple<int, string>(index + baseOutputIndex, outputSignalEnums.ToString());
                                }

                                amhsControl = new EFEM.Defines.LoadPort.CustomizedE23(i, saftyInterLockIndex, inputs, outputs);
                                amhsControl.AssignActionModeChangeBeforeCarrierLoad(EFEM.Modules.LoadPortManager.Instance.ChangeLoadPortMode);
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion </LoadPorts - PWA-500>
                }
                else
                {
                    #region <LoadPorts - PWA-500Bin>
                    switch (Work.AppConfigManager.Instance.InterfaceTypePIO)
                    {
                        case Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE.E84:
                            {
                                const int Offset = 8;

                                int interLockOffset = i / (countLoadPort / 2);
                                int saftyInterLockIndex = (int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_IN.PROTECTION_BAR_LP_1_2_3 + interLockOffset;

                                int baseInputIndex = (int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_IN.LP1_PIO_VALID + i * Offset;
                                int baseOutputIndex = (int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_OUT.LP1_PIO_L_REQ + i * Offset;

                                Dictionary<int, Tuple<int, string>> inputs = new Dictionary<int, Tuple<int, string>>();
                                Dictionary<int, Tuple<int, string>> outputs = new Dictionary<int, Tuple<int, string>>();
                                for (int index = 0; index < Offset; ++index)
                                {
                                    EFEM.Defines.LoadPort.E84InputSignals inputSignalEnums = EFEM.Defines.LoadPort.E84InputSignals.Valid + index;
                                    inputs[index] = new Tuple<int, string>(index + baseInputIndex, inputSignalEnums.ToString());

                                    EFEM.Defines.LoadPort.E84OutputSignals outputSignalEnums = EFEM.Defines.LoadPort.E84OutputSignals.LoadRequest + index;
                                    outputs[index] = new Tuple<int, string>(index + baseOutputIndex, outputSignalEnums.ToString());
                                }

                                amhsControl = new EFEM.Defines.LoadPort.CustomizedE84(i, saftyInterLockIndex, inputs, outputs);
                                amhsControl.AssignActionModeChangeBeforeCarrierLoad(EFEM.Modules.LoadPortManager.Instance.ChangeLoadPortMode);
                            }
                            break;
                        case Define.DefineEnumProject.AppConfig.EN_PIO_INTERFACE_TYPE.E23:
                            {
                                const int Offset = 8;

                                int interLockOffset = i / (countLoadPort / 2);
                                int saftyInterLockIndex = (int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_IN.PROTECTION_BAR_LP_1_2_3 + interLockOffset;

                                int baseInputIndex = (int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_IN.LP1_PIO_VALID + i * Offset;
                                int baseOutputIndex = (int)Define.DefineEnumProject.DigitalIO.PWA500BIN.EN_DIGITAL_OUT.LP1_PIO_L_REQ + i * Offset;

                                Dictionary<int, Tuple<int, string>> inputs = new Dictionary<int, Tuple<int, string>>();
                                Dictionary<int, Tuple<int, string>> outputs = new Dictionary<int, Tuple<int, string>>();
                                for (int index = 0; index < Offset; ++index)
                                {
                                    EFEM.Defines.LoadPort.E23InputSignals inputSignalEnums = EFEM.Defines.LoadPort.E23InputSignals.Valid + index;
                                    inputs[index] = new Tuple<int, string>(index + baseInputIndex, inputSignalEnums.ToString());

                                    EFEM.Defines.LoadPort.E23OutputSignals outputSignalEnums = EFEM.Defines.LoadPort.E23OutputSignals.LoadRequest + index;
                                    outputs[index] = new Tuple<int, string>(index + baseOutputIndex, outputSignalEnums.ToString());
                                }

                                amhsControl = new EFEM.Defines.LoadPort.CustomizedE23(i, saftyInterLockIndex, inputs, outputs);
                                amhsControl.AssignActionModeChangeBeforeCarrierLoad(EFEM.Modules.LoadPortManager.Instance.ChangeLoadPortMode);
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion </LoadPorts - PWA-500Bin>
                }

                Work.AppConfigManager.Instance.LoadPortLocationNames.TryGetValue(i, out Dictionary<string, string> locationNames);

                var verificationOptions = new EFEM.Defines.LoadPort.VerificationTransitionOptions
                {
                    //CarrierIdPolicy = EFEM.Defines.LoadPort.VerificationTransitionPolicy.Immediate,
                    //SlotMapPolicy = EFEM.Defines.LoadPort.VerificationTransitionPolicy.Immediate
                    CarrierIdPolicy = EFEM.Defines.LoadPort.VerificationTransitionPolicy.WaitForHostResult,
                    SlotMapPolicy = EFEM.Defines.LoadPort.VerificationTransitionPolicy.WaitForHostResult
                };

                var stateModel = EFEM.Modules.LoadPort.State.LoadPortStateModelFactory.Create(
                    EFEM.Modules.LoadPort.State.LoadPortStateModelType.E87,
                    portId,
                    verificationOptions);
                //EFEM.Defines.LoadPort.ILoadPortStateModel stateModel = new EFEM.Modules.LoadPort.State.LegacyLoadPortStateModel(portId, verificationOptions);

                var processType = Work.AppConfigManager.Instance.ProcessType;
                EFEM.Modules.LoadPort.Scheduler.LoadPortActionScheduler scheduler
                    = new EFEM.Modules.LoadPort.Scheduler.LoadPortActionScheduler(i, portId);

                EFEM.Modules.LoadPortManager.Instance.AssignLoadPorts(
                    new EFEM.Modules.LoadPort.LoadPortOperator(
                        portId,
                        name,
                        lp,
                        stateModel,
                        scheduler,
                        amhsControl,
                        locationNames,
                        gem300Service));

                #region <LoadPort Scheduler>
                //EFEM.ActionScheduler.LoadPortActionSchedulers.BaseLoadPortActionScheduler scheduler = null;
                //switch (processType)
                //{
                //    case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.NONE:
                //    case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.BIN_SORTER:
                //        {
                //            scheduler = null;// new EFEM.ActionScheduler.LoadPortActionSchedulers.ProcessTypes.PWA500BinSorterRobotActionScheduler(i);
                //            EFEM.CustomizedByProcessType.PWA500BIN.LoadPortType loadPortType = (EFEM.CustomizedByProcessType.PWA500BIN.LoadPortType)i;
                //            switch (loadPortType)
                //            {
                //                case EFEM.CustomizedByProcessType.PWA500BIN.LoadPortType.Bin_3:
                //                case EFEM.CustomizedByProcessType.PWA500BIN.LoadPortType.Bin_2:
                //                case EFEM.CustomizedByProcessType.PWA500BIN.LoadPortType.Bin_1:
                //                    scheduler = new EFEM.CustomizedByProcessType.PWA500BIN.BinLoadPortActionScheduler(i);
                //                    break;
                //                case EFEM.CustomizedByProcessType.PWA500BIN.LoadPortType.EmptyTape:
                //                    scheduler = new EFEM.CustomizedByProcessType.PWA500BIN.EmptyTapeLoadPortActionScheduler(i);
                //                    break;
                //                case EFEM.CustomizedByProcessType.PWA500BIN.LoadPortType.Core_2:
                //                case EFEM.CustomizedByProcessType.PWA500BIN.LoadPortType.Core_1:
                //                    scheduler = new EFEM.CustomizedByProcessType.PWA500BIN.CoreLoadPortActionScheduler(i);
                //                    break;
                //                default:
                //                    break;
                //            }
                //        }
                //        break;
                //    case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER:
                //        {
                //            scheduler = null;// new EFEM.ActionScheduler.LoadPortActionSchedulers.ProcessTypes.PWA500BinSorterRobotActionScheduler(i);
                //            EFEM.CustomizedByProcessType.PWA500W.LoadPortType loadPortType = (EFEM.CustomizedByProcessType.PWA500W.LoadPortType)i;
                //            switch (loadPortType)
                //            {
                //                case EFEM.CustomizedByProcessType.PWA500W.LoadPortType.Sort_12:
                //                    scheduler = new EFEM.CustomizedByProcessType.PWA500W.BinLoadPortActionScheduler(i);
                //                    break;
                //                case EFEM.CustomizedByProcessType.PWA500W.LoadPortType.Core_12:
                //                case EFEM.CustomizedByProcessType.PWA500W.LoadPortType.Core_8_2:
                //                case EFEM.CustomizedByProcessType.PWA500W.LoadPortType.Core_8_1:
                //                    scheduler = new EFEM.CustomizedByProcessType.PWA500W.CoreLoadPortActionScheduler(i);
                //                    break;
                //                default:
                //                    break;
                //            }
                //        }
                //        break;
                //    case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER_300:
                //        {
                //            scheduler = null;
                //            EFEM.CustomizedByProcessType.PWA500W.LoadPortType loadPortType = (EFEM.CustomizedByProcessType.PWA500W.LoadPortType)i;
                //            switch (loadPortType)
                //            {
                //                case EFEM.CustomizedByProcessType.PWA500W.LoadPortType.Sort_12:
                //                    scheduler = new EFEM.CustomizedByProcessType.PWA500W.BinLoadPortActionScheduler(i);
                //                    break;
                //                case EFEM.CustomizedByProcessType.PWA500W.LoadPortType.Core_12:
                //                case EFEM.CustomizedByProcessType.PWA500W.LoadPortType.Core_8_2:
                //                case EFEM.CustomizedByProcessType.PWA500W.LoadPortType.Core_8_1:
                //                    scheduler = new EFEM.ActionScheduler.LoadPortActionSchedulers.NormalLoadPortActionScheduler(i);
                //                    break;
                //                default:
                //                    break;
                //            }
                //        }
                //        break;

                //    default:
                //        break;
                //}
                //EFEM.ActionScheduler.LoadPortActionSchedulerManager.Instance.CreateScheduler(i, scheduler);
                #endregion </LoadPort Scheduler>

                #region <Rfid>

                #region <Foup>
                {
                    Define.DefineEnumProject.AppConfig.EN_RFID_CONTROLLER controllerRfidFoup = Work.AppConfigManager.Instance.ControllerRfidFoup;
                    bool failed = false;
                    int relIndex = (int)Define.DefineEnumProject.Serial.EN_SERIAL_INDEX.RFID_FOUP_1 + i;
                    RFIDOnly.RFIDReader reader = null;
                    switch (controllerRfidFoup)
                    {
                        case Define.DefineEnumProject.AppConfig.EN_RFID_CONTROLLER.NONE:
                            reader = new EFEM.Modules.RFID.Controllers.RfidSimulator(
                                    portId,
                                    Define.DefineEnumBase.Common.EN_CONNECTION_TYPE.SERIAL,
                                    relIndex);
                            break;
                        case Define.DefineEnumProject.AppConfig.EN_RFID_CONTROLLER.XEDION:
                            reader = new EFEM.Modules.RFID.Controllers.XedionRfid(
                                    portId,
                                    Define.DefineEnumBase.Common.EN_CONNECTION_TYPE.SERIAL,
                                    relIndex);
                            break;
                        case Define.DefineEnumProject.AppConfig.EN_RFID_CONTROLLER.CEYON:
                            reader = new EFEM.Modules.RFID.Controllers.CeyonRfid(
                                    portId,
                                    Define.DefineEnumBase.Common.EN_CONNECTION_TYPE.SERIAL,
                                    relIndex);
                            break;
                        default:
                            failed = true;
                            break;
                    }

                    if (failed)
                        continue;

                    EFEM.Modules.RFIDManager.Instance.AssigReader(i, EFEM.Defines.LoadPort.LoadPortLoadingMode.Foup, reader);
                    EFEM.Modules.RFIDManager.Instance.SetCarrierIdAddress(i, EFEM.Defines.LoadPort.LoadPortLoadingMode.Foup,
                        Work.AppConfigManager.Instance.FoupRfidCarrierIdAddress,
                        Work.AppConfigManager.Instance.FoupRfidCarrierIdLength);
                    EFEM.Modules.RFIDManager.Instance.SetLotIdAddress(i, EFEM.Defines.LoadPort.LoadPortLoadingMode.Foup,
                        Work.AppConfigManager.Instance.FoupRfidLotIdAddress,
                        Work.AppConfigManager.Instance.FoupRfidLotIdLength);
                }
                #endregion </Foup>

                #region <Cassette>
                {
                    Define.DefineEnumProject.AppConfig.EN_RFID_CONTROLLER controllerRfidCassette
                        = Work.AppConfigManager.Instance.ControllerRfidCassette;
                    int countRfidCassette = Work.AppConfigManager.Instance.CountRfidCassette;

                    bool failed = false;
                    int relIndex = (int)Define.DefineEnumProject.Serial.EN_SERIAL_INDEX.RFID_CASSETTE_1 + i;
                    RFIDOnly.RFIDReader reader = null;
                    switch (controllerRfidCassette)
                    {
                        case Define.DefineEnumProject.AppConfig.EN_RFID_CONTROLLER.NONE:
                            reader = new EFEM.Modules.RFID.Controllers.RfidSimulator(
                                    portId,
                                    Define.DefineEnumBase.Common.EN_CONNECTION_TYPE.SERIAL,
                                    relIndex);
                            break;
                        case Define.DefineEnumProject.AppConfig.EN_RFID_CONTROLLER.XEDION:
                            reader = new EFEM.Modules.RFID.Controllers.XedionRfid(
                                    portId,
                                    Define.DefineEnumBase.Common.EN_CONNECTION_TYPE.SERIAL,
                                    relIndex);
                            break;
                        case Define.DefineEnumProject.AppConfig.EN_RFID_CONTROLLER.CEYON:
                            reader = new EFEM.Modules.RFID.Controllers.CeyonRfid(
                                    portId,
                                    Define.DefineEnumBase.Common.EN_CONNECTION_TYPE.SERIAL,
                                    relIndex);
                            break;
                        default:
                            failed = true;
                            break;
                    }

                    if (failed)
                        continue;

                    EFEM.Modules.RFIDManager.Instance.AssigReader(i, EFEM.Defines.LoadPort.LoadPortLoadingMode.Cassette, reader);
                    EFEM.Modules.RFIDManager.Instance.SetCarrierIdAddress(i, EFEM.Defines.LoadPort.LoadPortLoadingMode.Cassette,
                        Work.AppConfigManager.Instance.CassetteRfidCarrierIdAddress,
                        Work.AppConfigManager.Instance.CassetteRfidCarrierIdLength);
                    EFEM.Modules.RFIDManager.Instance.SetLotIdAddress(i, EFEM.Defines.LoadPort.LoadPortLoadingMode.Cassette,
                        Work.AppConfigManager.Instance.CassetteRfidLotIdAddress,
                        Work.AppConfigManager.Instance.CassetteRfidLotIdLength);
                }
                #endregion </Cassette>

                #endregion </Rfid>

            }
            #endregion </LoadPorts>
        }
        private void BuildRobots()
        {
            int i;

            #region <Robot>
            var atmRobotControllerType
                = Work.AppConfigManager.Instance.AtmRobotControllerType;
            int countRobot = Work.AppConfigManager.Instance.CountRobot;
            for (i = 0; i < countRobot; ++i)
            {
                #region <Robot Scheduler>
                var processType = Work.AppConfigManager.Instance.ProcessType;
                EFEM.ActionScheduler.RobotActionSchedulers.BaseRobotActionScheduler scheduler = null;
                switch (processType)
                {
                    case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.NONE:
                    case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.BIN_SORTER:
                        scheduler = new EFEM.CustomizedByProcessType.PWA500BIN.PWA500BinSorterRobotActionScheduler(i);
                        break;
                    case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER:
                        scheduler = new EFEM.CustomizedByProcessType.PWA500W.PWA500WRobotActionScheduler(i);
                        break;
                    case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER_300:
                        {
                            scheduler = new EFEM.CustomizedByProcessType.PWA500W.PWA500WRobotActionSchedulerGEM300(i);
                        }
                        break;
                    default:
                        break;
                }

                EFEM.ActionScheduler.RobotActionSchedulerManager.Instance.CreateScheduler(i, scheduler);
                #endregion </Robot Scheduler>

                int commIndex = 0;
                string name = string.Format("Robot{0}", i + 1);

                EFEM.Modules.AtmRobot.AtmRobotController robot;

                Work.AppConfigManager.Instance.RobotStationNames.TryGetValue(i, out Dictionary<string, string> stationNames);

                switch (atmRobotControllerType)
                {
                    case Define.DefineEnumProject.AppConfig.EN_ROBOT_CONTROLLER.NONE:
                        {
                            commIndex = -1;
                            robot = new EFEM.Modules.AtmRobot.AtmRobotControllers.RobotControllerSimulator(i,
                                stationNames,
                                Define.DefineEnumBase.Common.EN_CONNECTION_TYPE.SERIAL, commIndex);
                        }
                        break;
                    case Define.DefineEnumProject.AppConfig.EN_ROBOT_CONTROLLER.QUADRA_ATM_ROBOT:
                        {
                            commIndex = (int)Define.DefineEnumProject.Serial.EN_SERIAL_INDEX.ATM_ROBOT + i;
                            robot = new EFEM.Modules.AtmRobot.AtmRobotControllers.QuadraRobotController(i,
                                stationNames,
                                Define.DefineEnumBase.Common.EN_CONNECTION_TYPE.SERIAL, commIndex);
                        }
                        break;
                    case Define.DefineEnumProject.AppConfig.EN_ROBOT_CONTROLLER.NRC:
                        {
                            commIndex = (int)Define.DefineEnumProject.Socket.EN_SOCKET_INDEX.ATM_ROBOT + i;
                            robot = new EFEM.Modules.AtmRobot.AtmRobotControllers.NRCRobotController(i,
                                stationNames,
                                Define.DefineEnumBase.Common.EN_CONNECTION_TYPE.TCP, commIndex);

                        }
                        break;
                    default:
                        robot = null;
                        break;
                }

                EFEM.Modules.AtmRobotManager.Instance.AssignRobots(new EFEM.Modules.AtmRobot.AtmRobotOperator(i, name, robot, stationNames));
            }
            #endregion </Robot>
        }
        private void BuildProcessModules()
        {
            int i;
            #region <Process Module>
            bool simulation = Work.AppConfigManager.Instance.ProcessModuleSimulation;
            int countProcessModule = Work.AppConfigManager.Instance.CountProcessModule;
            for (i = 0; i < countProcessModule; ++i)
            {
                // type
                var ProcessType = Work.AppConfigManager.Instance.ProcessType;

                // name
                string name = ProcessType.ToString();

                // location
                Work.AppConfigManager.Instance.ProcessModuleLocationNames.TryGetValue(i, out string[] locationNames);

                bool isDigitalIOSimulation = Work.AppConfigManager.Instance.ControllerDigital == Define.DefineEnumProject.AppConfig.EN_DIGITAL_IO_CONTROLLER.NONE;
                EFEM.Modules.ProcessModule.BaseProcessModule module = null;
                EFEM.Modules.ProcessModule.Communicator.BaseProcessModuleCommunicator communicator = null;
                switch (ProcessType)
                {
                    case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.NONE:
                    case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.BIN_SORTER:
                        {
                            communicator = new EFEM.CustomizedByProcessType.PWA500BIN.PWA500BINCommunicator(locationNames, simulation, isDigitalIOSimulation);
                            module = new EFEM.CustomizedByProcessType.PWA500BIN.ProcessModulePWA500BIN(i, communicator, name, simulation, isDigitalIOSimulation);
                        }
                        break;
                    case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER:
                    case Define.DefineEnumProject.AppConfig.EN_PROCESS_TYPE.DIE_TRANSFER_300:
                        {
                            communicator = new EFEM.CustomizedByProcessType.PWA500W.PWA500WCommunicator(locationNames, simulation, isDigitalIOSimulation);
                            module = new EFEM.CustomizedByProcessType.PWA500W.ProcessModulePWA500W(i, communicator, name, simulation, isDigitalIOSimulation);
                        }
                        break;
                    default:
                        break;
                }

                if (module != null)
                {
                    EFEM.Modules.ProcessModuleGroup.Instance.AssignProcessModule(i, module);
                }
            }
            #endregion </Process Module>
        }
        private void ConfigureLoadPort()
        {
            var manager = EFEM.MaterialTracking.SubstrateManager.Instance;
            var lpManager = EFEM.Modules.LoadPortManager.Instance;
            for (int i = 0; i < lpManager.Count; ++i)
            {
                var portId = lpManager.GetLoadPortPortId(i);
                var name = lpManager.GetLoadPortName(i);
                var count = lpManager.MaxCapacity;

                EFEM.MaterialTracking.LocationService.Instance.AddLoadPortLocationsAsync(name, portId, count).GetAwaiter().GetResult();
                
                //EFEM.MaterialTracking.LocationServer.AddLoadPortLocations(name, portId, count);
                manager.AddLoadPortBuffers(portId, count);

                Recipe.PARAM_EQUIPMENT param = Recipe.PARAM_EQUIPMENT.UseLoadPort1 + i;
                bool useLoadPort = FrameOfSystem3.Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
                    param.ToString(), 0, Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
                    true);
                lpManager.SetLoadPortEnabled(i, useLoadPort);
            }
        }
        private void ConfigureRobot()
        {
            var manager = EFEM.MaterialTracking.SubstrateManager.Instance;
            var rbManager = EFEM.Modules.AtmRobotManager.Instance;
            for (int i = 0; i < rbManager.Count; ++i)
            {
                var name = rbManager.GetRobotName(i);
                //EFEM.MaterialTracking.LocationServer.AddRobotLocations(name);
                EFEM.MaterialTracking.LocationService.Instance.AddRobotLocationsAsync(name).GetAwaiter().GetResult();

                manager.AddRobotBuffers(name);
            }
        }
        private void ConfigureProcessModule()
        {
            var manager = EFEM.MaterialTracking.SubstrateManager.Instance;
            var pmManager = EFEM.Modules.ProcessModuleGroup.Instance;
            for (int i = 0; i < pmManager.Count; ++i)
            {
                var name = pmManager.GetProcessModuleName(i);
                var locByEntry = pmManager.GetLocationsByEntry(i);
                var locationCapacity = pmManager.GetLocationCapacity(i);

                //EFEM.MaterialTracking.LocationServer.AddProcessModuleLocations(name, locByEntry, locationCapacity);
                EFEM.MaterialTracking.LocationService.Instance.AddProcessModuleLocationsAsync(name, locByEntry, locationCapacity).GetAwaiter().GetResult();

                manager.AddProcessModuleBuffers(name);
            }
        }
        private void BuildLocations()
        {
            //var locs = EFEM.MaterialTracking.LocationServer.GetLocations();
            //List<EFEM.MaterialTracking.LocationItem> items = new List<EFEM.MaterialTracking.LocationItem>();
            //foreach (var item in locs)
            //{
            //    if (item == null)
            //        continue;

            //    items.Add(new EFEM.MaterialTracking.LocationItem
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        LocationKind = (int)item.LocationKind,
            //        Capacity = item.Capacity
            //    });
            //}

            EFEM.MaterialTracking.LocationService.Instance.SyncAllAsync();
        }
        private void ExecuteAfterInitialization()
        {
            ConfigureLoadPort();
            ConfigureRobot();
            ConfigureProcessModule();

            EFEM.MaterialTracking.SubstrateManager.Instance.LoadRecoveryDataAll();
            EFEM.MaterialTracking.CarrierManagementServer.Instance.LoadRecoveryDataAll();

            BuildLocations();

            InitializeNewParameters();
        }
        private void InitializeParam(Recipe.EN_RECIPE_TYPE recipeType,
            string paramName,
            Recipe.EN_DATA_TYPE format,
            string defaultValue)
        {
            var type = Recipe.Recipe.GetInstance().GetValue(recipeType,
                   paramName,
                   0,
                   Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
                   string.Empty);

            if (string.IsNullOrWhiteSpace(type))
            {
                Recipe.Recipe.GetInstance().SetValue(recipeType,
                  paramName,
                  0,
                  Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
                  format.ToString());
            }
                
            var value = Recipe.Recipe.GetInstance().GetValue(recipeType,
                   paramName,
                   0,
                   Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
                   string.Empty);

            if (string.IsNullOrWhiteSpace(value))
            {
                Recipe.Recipe.GetInstance().SetValue(recipeType,
                 paramName,
                 0,
                 Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
                 defaultValue);
            }
        }
        private void UpdateInitialParam(
            string paramName,
            Recipe.EN_DATA_TYPE initialType,
            string initialValue)
        {
            var parameterName = paramName;
            var curType = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
                    parameterName,
                    0,
                    Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
                    string.Empty);
            if (string.IsNullOrWhiteSpace(curType))
            {
                Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
                parameterName,
                0,
                Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
                initialType.ToString());
            }

            var dataValue = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
                parameterName,
                0,
                Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
                string.Empty);
            if (string.IsNullOrWhiteSpace(dataValue))
            {
                Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
                parameterName,
                0,
                Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
                initialValue);
            }
        }
        private void InitializeNewParameters()
        {
            UpdateInitialParam(Recipe.PARAM_EQUIPMENT.WrittingLotIdToMACWhenLotIsTerminated.ToString(),
                Recipe.EN_DATA_TYPE.ASCII,
                "PHMAC");

            UpdateInitialParam(Recipe.PARAM_EQUIPMENT.WrittingLotIdToMACWhenCarrierIsEmpty.ToString(),
               Recipe.EN_DATA_TYPE.ASCII,
               "PRMAC");

            UpdateInitialParam(Recipe.PARAM_EQUIPMENT.WrittingLotIdToCassetteWhenLotIsTerminated.ToString(),
                Recipe.EN_DATA_TYPE.ASCII,
                "RCASSETTE");

            UpdateInitialParam(Recipe.PARAM_EQUIPMENT.WrittingLotIdToCassetteWhenCarrierIsEmpty.ToString(),
               Recipe.EN_DATA_TYPE.ASCII,
               "ECASSETTE");

            //UpdateInitialParam(Recipe.PARAM_EQUIPMENT.WrittingLotIdToClosedCassetteWhenLotIsTerminated.ToString(),
            //    Recipe.EN_DATA_TYPE.ASCII,
            //    "IRCASSETTE");

            //UpdateInitialParam(Recipe.PARAM_EQUIPMENT.WrittingLotIdToClosedCassetteWhenCarrierIsEmpty.ToString(),
            //    Recipe.EN_DATA_TYPE.ASCII,
            //    "IECASSETTE");


            //InitializeParam(Recipe.EN_RECIPE_TYPE.COMMON,
            //    Recipe.PARAM_COMMON.UseDownloadingRecipe.ToString(), Recipe.EN_DATA_TYPE.BOOL, bool.FalseString);
            //for (int i = 0; i < 6; ++i)
            //{
            //    parameter = Recipe.PARAM_EQUIPMENT.LoadPortSize1 + i;
            //    var type = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //        parameter.ToString(),
            //        0,
            //        Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
            //        Recipe.EN_DATA_TYPE.ASCII.ToString());
            //    if (string.IsNullOrWhiteSpace(type))
            //    {
            //        Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //        parameter.ToString(),
            //        0,
            //        Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
            //        Recipe.EN_DATA_TYPE.ASCII.ToString());
            //    }

            //    var value = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //        parameter.ToString(),
            //        0,
            //        Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
            //        string.Empty);
            //    if (string.IsNullOrWhiteSpace(value))
            //    {
            //        Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //        parameter.ToString(),
            //        0,
            //        Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
            //        EFEM.CustomizedByProcessType.PWA500Common.SubstrateSize.Inch_12.ToString());
            //    }


            //    parameter = Recipe.PARAM_EQUIPMENT.UseSlotValidationResult1 + i;
            //    type = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //        parameter.ToString(),
            //        0,
            //        Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
            //        string.Empty);
            //    if (string.IsNullOrWhiteSpace(type))
            //    {
            //        Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //        parameter.ToString(),
            //        0,
            //        Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
            //        Recipe.EN_DATA_TYPE.BOOL.ToString());
            //    }

            //    value = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //        parameter.ToString(),
            //        0,
            //        Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
            //        string.Empty);
            //    if (string.IsNullOrWhiteSpace(value))
            //    {
            //        Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //        parameter.ToString(),
            //        0,
            //        Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
            //        bool.FalseString);
            //    }
            //}
            //#endregion </Size>

            //string dataType, dataValue, parameterName;

            //#region <WrittingLotIdToMACWhenLotIsTerminated>
            //parameterName = Recipe.PARAM_EQUIPMENT.WrittingLotIdToMACWhenLotIsTerminated.ToString();
            //dataType = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //        parameterName,
            //        0,
            //        Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
            //        Recipe.EN_DATA_TYPE.ASCII.ToString());
            //if (string.IsNullOrWhiteSpace(dataType))
            //{
            //    Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //    parameterName,
            //    0,
            //    Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
            //    Recipe.EN_DATA_TYPE.ASCII.ToString());
            //}

            //dataValue = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //    parameterName,
            //    0,
            //    Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
            //    string.Empty);
            //if (string.IsNullOrWhiteSpace(dataValue))
            //{
            //    Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //    parameterName,
            //    0,
            //    Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
            //    "PHMAC");
            //}
            //#endregion </WrittingLotIdToMACWhenLotIsTerminated>

            //#region <WrittingLotIdToCassetteWhenLotIsTerminated>
            //parameterName = Recipe.PARAM_EQUIPMENT.WrittingLotIdToCassetteWhenLotIsTerminated.ToString();
            //dataType = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //        parameterName,
            //        0,
            //        Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
            //        Recipe.EN_DATA_TYPE.ASCII.ToString());
            //if (string.IsNullOrWhiteSpace(dataType))
            //{
            //    Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //    parameterName,
            //    0,
            //    Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
            //    Recipe.EN_DATA_TYPE.ASCII.ToString());
            //}

            //dataValue = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //    parameterName,
            //    0,
            //    Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
            //    string.Empty);
            //if (string.IsNullOrWhiteSpace(dataValue))
            //{
            //    Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //    parameterName,
            //    0,
            //    Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
            //    "RCASSETTE");
            //}
            //#endregion </WrittingLotIdToCassetteWhenLotIsTerminated>

            //parameterName = Recipe.PARAM_EQUIPMENT.WrittingLotIdToMACWhenCarrierIsEmpty.ToString();
            //dataType = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //        parameterName,
            //        0,
            //        Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
            //        Recipe.EN_DATA_TYPE.ASCII.ToString());
            //if (string.IsNullOrWhiteSpace(dataType))
            //{
            //    Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //    parameterName,
            //    0,
            //    Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
            //    Recipe.EN_DATA_TYPE.ASCII.ToString());
            //}

            //dataValue = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //    parameterName,
            //    0,
            //    Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
            //    string.Empty);
            //if (string.IsNullOrWhiteSpace(dataValue))
            //{
            //    Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //    parameterName,
            //    0,
            //    Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
            //    "PRMAC");
            //}

            //parameterName = Recipe.PARAM_EQUIPMENT.WrittingLotIdToCassetteWhenCarrierIsEmpty.ToString();
            //dataType = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //        parameterName,
            //        0,
            //        Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
            //        Recipe.EN_DATA_TYPE.ASCII.ToString());
            //if (string.IsNullOrWhiteSpace(dataType))
            //{
            //    Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //    parameterName,
            //    0,
            //    Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
            //    Recipe.EN_DATA_TYPE.ASCII.ToString());
            //}

            //dataValue = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //    parameterName,
            //    0,
            //    Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
            //    string.Empty);
            //if (string.IsNullOrWhiteSpace(dataValue))
            //{
            //    Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //    parameterName,
            //    0,
            //    Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
            //    "ECASSETTE");
            //}

            //#region <MachineName>
            //parameterName = Recipe.PARAM_EQUIPMENT.MachineName.ToString();
            //dataType = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //        parameterName,
            //        0,
            //        Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
            //        Recipe.EN_DATA_TYPE.ASCII.ToString());
            //if (string.IsNullOrWhiteSpace(dataType))
            //{
            //    Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //    parameterName,
            //    0,
            //    Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE,
            //    Recipe.EN_DATA_TYPE.ASCII.ToString());
            //}

            //dataValue = Recipe.Recipe.GetInstance().GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //    parameterName,
            //    0,
            //    Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
            //    string.Empty);
            //if (dataValue != Work.AppConfigManager.Instance.MachineName)
            //{
            //    Recipe.Recipe.GetInstance().SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT,
            //       parameterName,
            //       0,
            //       Recipe.EN_RECIPE_PARAM_TYPE.VALUE,
            //       Work.AppConfigManager.Instance.MachineName);
            //}
            //#endregion </MachineName>
        }
        private void ShutDown()
        {
            if (_storageContextFactory != null)
            {
                _storageContextFactory.ShutDown();
            }

            #region <Modules>
            EFEM.Modules.LoadPortManager.Instance.ExitLoadPorts();
            EFEM.Modules.ProcessModuleGroup.Instance.ExitProcessModuleAll();
            EFEM.Defines.Common.AsyncLoggerForEfem.Instance.ExitAsync().GetAwaiter().GetResult();
            #endregion </Modules>

            #region <FFU>
            ExternalDevice.Serial.FanFilterUnit.FanFilterUnitManager.Instance.Deactivate();
            #endregion </FFU>
        }
        #endregion </EFEM Only>
    }
}