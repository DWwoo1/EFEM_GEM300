using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RunningTask_;
using DigitalIO_;
using Vision_;

using FrameOfSystem3.Functional;
using FrameOfSystem3.DynamicLink_;
using FrameOfSystem3.Log;
using FrameOfSystem3.Work;
using FrameOfSystem3.ExternalDevice.Serial.FanFilterUnit;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using Define.DefineEnumBase.Common;
using Define.DefineEnumBase.Log;
using Define.DefineEnumProject.Map;
using Define.DefineEnumProject.Vision;
using Define.DefineEnumProject.Task;
using Define.DefineEnumProject.Task.Global;
using Define.DefineEnumProject.Mail;
using Define.DefineEnumProject.Tool;
using Define.DefineEnumProject.DigitalIO;

using EFEM.Modules;
using EFEM.Defines.ProcessModule;

namespace FrameOfSystem3.Task
{
	abstract class TaskGlobal : RunningTaskWrapper
	{
		#region constructor
		public TaskGlobal(int nIndexOfTask, string strTaskName)
			: base(nIndexOfTask, strTaskName, typeof(PARAM_GLOBAL))
		{
			_taskOperator = TaskOperator.GetInstance();
			_recovery = new TaskGlobalRecovery(strTaskName, 1);
			AddRecoveryData(strTaskName, _recovery);

			_digitalIo = DigitalIO.GetInstance();
			_vision = Vision.GetInstance();
			_ffuManager = FanFilterUnitManager.Instance;
			_processGroup = ProcessModuleGroup.Instance;

			MappingDoorLockSensor();
			MappingFanAlarmSensor();
			MappingFFUAlarm();
			MappingIonizerAlarm();
			MappingAirAlarm();

			_scenarioCirculator = ScenarioCirculator.Instance;
			_scenarioCirculator.Initialize(
				UpdateScenarioParam,
				ExecuteScenario,
				GenerateAlarm,
				5000);
		}
		protected override void MakeMappingTableForAction()
		{
			foreach (TASK_ACTION enAction in Enum.GetValues(typeof(TASK_ACTION)))
			{
				m_mapppingForAction.Add(enAction.ToString(), enAction);
			}
		}
		#endregion constructor

		#region filed

		#region default

		#region instance
		private static TaskOperator _taskOperator = null;
		private static TaskGlobalRecovery _recovery = null;
		#endregion /instance

        private TASK_ACTION m_enAction                          = TASK_ACTION.STOP;
		Dictionary<string, TASK_ACTION> m_mapppingForAction     = new Dictionary<string, TASK_ACTION>();
		#endregion /default

		DigitalIO _digitalIo = null;
		TickCounter_.TickCounter _timeMonitoringIntervalFFU = new TickCounter_.TickCounter();

        Vision _vision = null;
        VISION_RESULT _visionResult = VISION_RESULT.IN_PROCESS;

        private static FanFilterUnitManager _ffuManager = null;
		private bool _isDoorOpened = true;
		private bool _isFanAlarm = true;
		private bool _isFFUAlarm = true;
		private bool _isFFUConnected = true;
		private bool _isIonizerAlarm = true;
		private bool _isAirAlarm = true;
        private readonly ConcurrentDictionary<int, bool> DoorOpenedSensors = new ConcurrentDictionary<int, bool>();
        private readonly ConcurrentDictionary<int, bool> FanAlarmSensors = new ConcurrentDictionary<int, bool>();
        private readonly ConcurrentDictionary<int, bool> FFUAlarms = new ConcurrentDictionary<int, bool>();
        private readonly ConcurrentDictionary<int, bool> IonizerAlarms = new ConcurrentDictionary<int, bool>();
        private readonly ConcurrentDictionary<int, bool> AirAlarms = new ConcurrentDictionary<int, bool>();

		private static ScenarioCirculator _scenarioCirculator = null;
		private static ProcessModuleGroup _processGroup = null;
		#endregion /filed

		#region inherit
		protected override void DoAlwaysSequence()
		{
			base.DoAlwaysSequence();

			_scenarioCirculator.Execute();

            if (_taskOperator.IsExiting)
                return;

            if (false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.UNDEFINED))
            {
				HasDoorAlarm(false);
			}

            if (false == EquipmentState_.EquipmentState.GetInstance().GetState().Equals(EquipmentState_.EQUIPMENT_STATE.UNDEFINED))
            {
				HasUtilityAlarm(false);
			}

			MonitoringFanFilterUnit();
			CheckRemoteCommandError();
			ExecuteOnAlways();
		}

		#region sequence
		/// <summary>
		/// 2020.06.02 by yjlee [ADD] Code the sequence to initialize this task.
		/// - Before returning 'true', it will be called continuously.
		/// </summary>
		protected override bool DoInitializeSequence()
		{
			if (_taskOperator.IsFinishingMode())
				return true;

			switch (m_nSeqNum)
			{
				case (int)STEP_INITIALIZE.START:
					Views.Functional.Form_ProgressBar.GetInstance().ShowForm(GetTaskName(), (uint)STEP_INITIALIZE.END);
					InitTemporaryData();
					if (HasDoorAlarm(true))
						return true;
                    m_nSeqNum = (int)STEP_INITIALIZE.PREPARE;
                    break;
				case (int)STEP_INITIALIZE.PREPARE:
					PostOffice.GetInstance().EmptyMailBoxAll();
                    m_nSeqNum = (int)STEP_INITIALIZE.END;
					break;

				case (int)STEP_INITIALIZE.END:
					return true;
			}

			Views.Functional.Form_ProgressBar.GetInstance().UpdateStep(GetTaskName(), (uint)m_nSeqNum);
			return false;
		}

		/// <summary>
		/// 2020.06.02 by yjlee [ADD] Code the sequence for entry.
		/// - Before returning 'true', it will be called continuously.
		/// </summary>
		protected override bool DoEntrySequence()
		{
			if (_taskOperator.IsFinishingMode())
				return true;

            switch (m_nSeqNum)
			{
				case (int)STEP_ENTRY.Start:
					InitTemporaryData();
					m_nSeqNum = (int)STEP_ENTRY.Prepare;
					break;
				case (int)STEP_ENTRY.Prepare:
					PostOffice.GetInstance().EmptyMailBoxAll();

                    if (HasDoorAlarm(true) || HasUtilityAlarm(true))
                        return true;

                    bool needCheckConnectionStatus = false;
                    if (_taskOperator.GetManualActionName(out List<string> taskNames, out List<List<string>> actionNames))
                    {
                        for (int i = 0; i < taskNames.Count; ++i)
                        {
                            if (false == Enum.TryParse(taskNames[i], out EN_TASK_LIST taskName))
                                continue;

                            if (_taskOperator.IsTypeEqualsAtmRobot(taskName))
                            {
                                needCheckConnectionStatus = true;
                                break;
                            }
                        }
                    }
					else
                    {
						needCheckConnectionStatus = true;
					}

                    if (needCheckConnectionStatus && false == _taskOperator.IsProcessModuleConnected())
                    {
                        GenerateAlarm((int)ALARM_GLOBAL.COMMUNICATION_ERROR_WITH_PM);
                        m_nSeqNum = (int)STEP_ENTRY.End;
                        break;
                    }

                    m_nSeqNum = (int)STEP_ENTRY.End;
					break;

				case (int)STEP_ENTRY.End:
					return true;
			}

			return false;
		}
		protected override bool DoSetupSequence()
		{
			base.DoSetupSequence();

			return true;
		}
		protected override bool DoExecutingSequence()
		{
			base.DoExecutingSequence();

			return true;
		}
		#endregion /sequence

		#region dynamic link
		/// <summary>
		/// 2020.06.02 by yjlee [ADD] Check whether a sequence is existent or not.
		/// </summary>
		protected override bool UpdateActionName(string actionName)
		{
			if (false == m_mapppingForAction.ContainsKey(actionName))
			{
				m_enAction = TASK_ACTION.STOP;
				return false;
			}

			m_enAction = m_mapppingForAction[actionName];
			return true;
		}
		/// <summary>
		/// Action pre condition과 flow post condition을 설정함.
		/// </summary>
		public override void InitializeActionCondition()
		{
		}
		protected override void DoExecutingPostcondition()
		{
			base.DoExecutingPostcondition();
		}
        #endregion /dynamic link

        #endregion /inherit

        #region <Utilities>
        private void MappingDoorLockSensor()
        {
			GetDoorLockSensorSignalList(out List<int> signals);
			// GetAirAlarmSignalList(out List<int> signals);
			for (int i = 0; i < signals.Count; ++i)
            {
                DoorOpenedSensors[signals[i]] = false;
            }
        }
        private void MappingFanAlarmSensor()
        {
            GetFanAlarmSensorSignalList(out List<int> signals);
            for (int i = 0; i < signals.Count; ++i)
            {
                FanAlarmSensors[signals[i]] = false;
            }
        }
        private void MappingFFUAlarm()
        {
            GetFFUAlarmSignalList(out List<int> signals);
            for (int i = 0; i < signals.Count; ++i)
            {
                FFUAlarms[signals[i]] = false;
            }
        }
        private void MappingIonizerAlarm()
        {
            GetIonizerAlarmSignalList(out List<int> signals);
            for (int i = 0; i < signals.Count; ++i)
            {
                IonizerAlarms[signals[i]] = false;
            }
        }
        private void MappingAirAlarm()
        {
            GetAirAlarmSignalList(out List<int> signals);
            for (int i = 0; i < signals.Count; ++i)
            {
                AirAlarms[signals[i]] = false;
            }
        }
        private bool IsDoorOpened()
        {
			if (AppConfigManager.Instance.ControllerDigital.Equals(Define.DefineEnumProject.AppConfig.EN_DIGITAL_IO_CONTROLLER.NONE))
				return false;

            foreach (var item in DoorOpenedSensors)
            {
                if (false == _digitalIo.ReadInput(item.Key))
                    return true;
            }

            return false;
        }

        private bool IsFanAlarm()
        {
            foreach (var item in FanAlarmSensors)
            {
                if (_digitalIo.ReadInput(item.Key))
                    return true;
            }

            return false;
        }
        private bool IsFFUAlarm()
        {
            foreach (var item in FFUAlarms)
            {
                if (_digitalIo.ReadInput(item.Key))
                    return true;
            }

            return false;
        }
		private bool IsFFUConnected()
        {
			return _ffuManager.IsConnected();
		}
        private bool IsIonizerAlarm()
        {
            foreach (var item in IonizerAlarms)
            {
                if (_digitalIo.ReadInput(item.Key))
                    return true;
            }

            return false;
        }
        private bool IsAirAlarm()
        {
            foreach (var item in AirAlarms)
            {
                if (false == _digitalIo.ReadInput(item.Key))
                    return true;
            }

            return false;
        }
		#endregion </Utilities>

		#region <Abstracts>
		protected abstract void GetDoorLockSensorSignalList(out List<int> indexOfSignals);
		protected abstract void GetFanAlarmSensorSignalList(out List<int> indexOfSignals);
		protected abstract void GetFFUAlarmSignalList(out List<int> indexOfSignals);
		protected abstract void GetIonizerAlarmSignalList(out List<int> indexOfSignals);
		protected abstract void GetAirAlarmSignalList(out List<int> indexOfSignals);
        #endregion </Abstracts>

        #region ExternalDevice
        private bool HasDoorAlarm(bool checkingAtSequence)
        {
            // 1. Door
            if (_isDoorOpened != IsDoorOpened() || checkingAtSequence)
            {
				_isDoorOpened = IsDoorOpened();
				if (_isDoorOpened)
                {
                    GenerateAlarm((int)ALARM_GLOBAL.DOOR_UNLOCKED);
                }
            }

			return _isDoorOpened;
        }
		private bool HasUtilityAlarm(bool checkingAtSequence)
        {
			if (GetParameter(Recipe.PARAM_COMMON.UseUtilityAlarm, true))
			{
				// 2. Fan
				if (_isFanAlarm != IsFanAlarm() || checkingAtSequence)
				{
					_isFanAlarm = IsFanAlarm();
					if (_isFanAlarm)
					{
						GenerateAlarm((int)ALARM_GLOBAL.FAN_ERROR);
					}
				}

				// 3. FFU
				if (_isFFUAlarm != IsFFUAlarm() || checkingAtSequence)
				{
					_isFFUAlarm = IsFFUAlarm();
					if (_isFFUAlarm)
					{
						GenerateAlarm((int)ALARM_GLOBAL.FFU_ERROR);
					}
				}
                
				// 3-1. FFU Connected
     //           if (_isFFUConnected != IsFFUConnected() || checkingAtSequence)
     //           {
					//_isFFUConnected = IsFFUConnected();
     //               if (false == _isFFUConnected)
     //               {
     //                   GenerateAlarm((int)ALARM_GLOBAL.FFU_CONTROLLER_NOT_CONNECTED);
     //               }
     //           }

                // 4. Ionizer Alarm
                if (_isIonizerAlarm != IsIonizerAlarm() || checkingAtSequence)
				{
					_isIonizerAlarm = IsIonizerAlarm();
					if (_isIonizerAlarm)
					{
						GenerateAlarm((int)ALARM_GLOBAL.IONIZER_ERROR);
					}
				}

				// 5. Air
				if (_isAirAlarm != IsAirAlarm() || checkingAtSequence)
				{
					_isAirAlarm = IsAirAlarm();
					if (_isAirAlarm)
					{
						GenerateAlarm((int)ALARM_GLOBAL.AIR_ERROR);
					}
				}

				return (_isFanAlarm || _isFFUAlarm || _isIonizerAlarm || _isAirAlarm);
			}
			else
			{
				return false;
			}
        }
		private void CheckRemoteCommandError()
        {
			if (_taskOperator.RemoteCommandError)
            {
				_taskOperator.RemoteCommandError = false;
				GenerateAlarm((int)ALARM_GLOBAL.FDC_ERROR);
			}
		}
        private void MonitoringFanFilterUnit()
        {
            if (false == _timeMonitoringIntervalFFU.IsTickOver(true))
                return;

            _timeMonitoringIntervalFFU.SetTickCount(5000);

            if (_ffuManager.Skipped || _ffuManager.UseDifferentialPressureMode)
                return;

            //if (IsDoorOpened())
            //{
            //    _ffuManager.RequestFullSpeedAll();
            //}
            //else
            //{
            //    _ffuManager.RequestNormalSpeedAll();
            //}
        }
        #endregion /ExternalDevice

        #region common method

       
        #endregion /common method

        #region task unique

        #endregion /task unique

        #region <SecsGem>
		protected abstract void ExecuteOnAlways();

        //      private bool UpdateScenarioParam(string scenario, Dictionary<string, string> datas)
        //      {
        //          return _scenarioOperator.UpdateScenarioParam(scenario, datas);
        //      }
        //private bool ExecuteScenario(string scenario)
        //      {
        //          var result = _scenarioOperator.ExecuteScenario(scenario);
        //          switch (result)
        //          {
        //              case EN_SCENARIO_RESULT.PROCEED: return false;
        //              case EN_SCENARIO_RESULT.COMPLETED:
        //                  return true;
        //              default:
        //                  GenerateAlarm((int)EN_TASK_COMMON_ALARM.SCENARIO_ERROR
        //                      , string.Format("{0}:{1}", scenario.ToString(), result.ToString()));
        //                  return true;
        //          }
        //      }

        #endregion </SecsGem>

        #region enum

        #region action
        /// <summary>
        /// 2020.06.02 by yjlee [ADD] Enumerate the actions of the task.
        /// </summary>
        public enum TASK_ACTION
		{
			STOP = 0,
		}
		private enum EN_SUBSEQUENCE
		{
			SAMPLE,
		}
		#endregion /action

		#region step
		private enum STEP_INITIALIZE
		{
			START = 0,
            PREPARE = 50,

			END = 10000,
		}
		private enum STEP_ENTRY
		{
			Start = 0,
			Prepare = 50,

			End = 10000,
		}
		private enum STEP_EXIT
		{
			START = 0,
			END = 10000,
		}

		private enum STEP_SAMPLE
		{
			START = 0,

			MOVE = 1000,

			TERMINATE = 9000,
			END = 10000,
		}
		private enum STEP_SIMPLE_MOVE
		{
			START = 0,

			MOVE = 1000,

			END = 10000,
		}
		#endregion /step

		#region position
		private enum EN_POSITION_SAMPLE
		{
			Sample,
		}
		#endregion /position

		#region time
		private enum EN_DELAY_TIME
		{
			Sample,
			Manual,
		}
		private enum EN_CHECK_TIME
		{
			Sample,
			Manual,
		}
		#endregion /time

		#region repeat count
		private enum EN_CHECK_COUNT
		{
			Sample,
			Manual,
		}
		#endregion /repeat count

		#endregion /enum
	}
	class TaskGlobalRecovery : RecoveryData
	{
		public TaskGlobalRecovery(string taskName, int nPortCount)
			: base(taskName, nPortCount)
		{
		}

		protected override void LoadData(ref FileComposite_.FileComposite fComp, string sRootName)
		{
		}
		protected override void SaveData(ref FileComposite_.FileComposite fComp, string sRootName)
		{
		}
	}
}