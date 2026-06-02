using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Motion_;
using Cylinder_;
using DigitalIO_;

using FrameOfSystem3.DynamicLink_;
using FrameOfSystem3.SubSequence;
using FrameOfSystem3.Functional;

using FrameOfSystem3.SECSGEM;
using FrameOfSystem3.SECSGEM.DefineSecsGem;
using FrameOfSystem3.SECSGEM.Scenario;

using Define.DefineEnumBase.Common;
using Define.DefineEnumBase.Log;
using Define.DefineEnumProject.Task;
using Define.DefineEnumProject.Task.Global;
using Define.DefineEnumProject.SubSequence;
using Define.DefineEnumProject.Mail;

namespace FrameOfSystem3.Task
{
	public abstract class RunningTaskWrapper : RunningTaskEx
	{
		#region constructor
		protected RunningTaskWrapper(int nIndexOfTask, string strTaskName, Type enTypeProcessParameter)
			: base(nIndexOfTask, strTaskName, Enum.GetValues(enTypeProcessParameter).Length)
        {
			MakeMappingTableForAction();
			InitAddProcessParameter(enTypeProcessParameter);

			_taskOperator = TaskOperator.GetInstance();
			_recipe = Recipe.Recipe.GetInstance();
			_configDevice = FrameOfSystem3.Config.ConfigDevice.GetInstance();
			_configMotionSpeed = FrameOfSystem3.Config.ConfigMotionSpeed.GetInstance();
			_configAnalog = FrameOfSystem3.Config.ConfigAnalogIO.GetInstance();
			_interlock = Config.ConfigInterlock.GetInstance();
			_dynamicLink = DynamicLink_.DynamicLink.GetInstance();
			_log = Log.LogManager.GetInstance();

			if (false == Enum.TryParse(GetTaskName(), out _mySubscriberName))
				_mySubscriberName = EN_SUBSCRIBER.Unknown;

			_postOffice = PostOffice.GetInstance();
			_postOffice.RequestSubscribe(_mySubscriberName);

			_scenarioOperator = ScenarioOperator.Instance;
		}
		protected bool InitAddProcessParameter(Type enumType)
		{
			if (false == enumType.IsEnum)
				return false;

			_processParameterList.Clear();
			bool result = true;
			foreach (var en in Enum.GetValues(enumType))
			{
				_processParameterList.Add((Enum)en, en.ToString());
				result &= AddProcessRecipeParameter((int)en, en.ToString());
			}

			return result;
		}
		#endregion constructor

		#region field
		static TaskOperator _taskOperator = null;
		static FrameOfSystem3.Config.ConfigDevice _configDevice = null;
		static FrameOfSystem3.Config.ConfigMotionSpeed _configMotionSpeed = null;
		static FrameOfSystem3.Config.ConfigAnalogIO _configAnalog = null;
		static Config.ConfigInterlock _interlock = null;
		static DynamicLink_.DynamicLink _dynamicLink = null;
		static Log.LogManager _log = null;
		static PostOffice _postOffice = null;

		readonly EN_SUBSCRIBER _mySubscriberName;

        protected Work.AppConfigManager _appConfig = Work.AppConfigManager.Instance;    // 생성자 단계에서 사용 할 수 있으므로 바로 객체생성
		protected Dictionary<int, TickCounter_.TickCounter> _timeCheck = new Dictionary<int, TickCounter_.TickCounter>();
		int _lastSetIndexCheckTime = -1;

		protected Dictionary<int, int> _countCheck = new Dictionary<int, int>();

		Dictionary<Enum, string> _processParameterList = new Dictionary<Enum, string>();

		protected ScenarioOperator _scenarioOperator = null;

		#endregion /field

		#region constant
		protected const int SEQUENCE_END_STEP = 10000;

		const int DEFAULT_MOTION_RATIO = 100;
		const int DEFAULT_MOTION_DELAY = 5;	// 50ms > 5ms 변경.
		#endregion /constant

		#region common method

		#region task
		protected abstract void MakeMappingTableForAction();
		protected void AddRecoveryData(string taskName, FrameOfSystem3.Work.RecoveryData recoveryData)
		{
			var recovery = recoveryData as FrameOfSystem3.Work.RecoveryData;
			EN_TASK_LIST enTask;
			if (Enum.TryParse(taskName, out enTask))
			{
				_taskOperator.AddRecoveryData(enTask, ref recovery);
			}
		}
		protected EN_CHECK_MANUAL_ACTION_RESULT CheckManualAction(Func<List<string>, List<List<string>>, bool> funcCondition)
		{
			List<string> taskNameList;
			List<List<string>> actionNameList;
			if (false == _taskOperator.GetManualActionName(out taskNameList, out actionNameList))
				return EN_CHECK_MANUAL_ACTION_RESULT.NotSetupMode;

			return funcCondition(taskNameList, actionNameList)
				? EN_CHECK_MANUAL_ACTION_RESULT.Accord
				: EN_CHECK_MANUAL_ACTION_RESULT.Disaccord;
		}
		protected EN_CHECK_MANUAL_ACTION_RESULT CheckManualAction(string targetTask, Func<List<string>, bool> funcCondition)
		{
			List<string> taskNameList;
			List<List<string>> actionNameList;
			if (false == _taskOperator.GetManualActionName(out taskNameList, out actionNameList))
				return EN_CHECK_MANUAL_ACTION_RESULT.NotSetupMode;

			int index = taskNameList.IndexOf(targetTask);
			if (index < 0)
				return EN_CHECK_MANUAL_ACTION_RESULT.HaveNotTargetTask;

			if (actionNameList.Count <= index)
				return EN_CHECK_MANUAL_ACTION_RESULT.AbnormalError;	// 발생할 일 없지만 혹시 모르니 예외처리

			return funcCondition(actionNameList[index])
				? EN_CHECK_MANUAL_ACTION_RESULT.Accord
				: EN_CHECK_MANUAL_ACTION_RESULT.Disaccord;
		}
		protected EN_CHECK_MANUAL_ACTION_RESULT CheckManualAction(Func<List<string>, bool> funcCondition)
		{
			return CheckManualAction(GetTaskName(), funcCondition);
		}
		#endregion /task

		#region motion
		// 2024.09.11 by jhshin [ADD] add override method with action name, retry
		protected bool MoveAxisMotion(int axisNo, double destination, bool isCheck = true, int delay = DEFAULT_MOTION_DELAY, int ratio = DEFAULT_MOTION_RATIO, string caption = "", int captionRetried = 0)
		{
			var speedPattern = GetMotionSpeedContentByOperation();
			MOTION_RESULT motionResult = MoveAbsolutely(ComplementToCaption(caption), axisNo, destination, speedPattern, ratio, delay, 0, isCheck, captionRetried);

			switch (motionResult)
			{
				case MOTION_RESULT.OK: return true;
				case MOTION_RESULT.NOT_READY:
				case MOTION_RESULT.NOT_READY_REGISTEDINSTANCE:
				case MOTION_RESULT.FAIL_GET_ONERSHIP:
					return false;
				default:
					GenerateMotionAlarm(axisNo, motionResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}

		/// <summary>
		/// 2024.11.25. by shkim. [ADD] 상대위치 축 이동
		/// </summary>
		protected bool MoveAxisRelativeMotion(int axisNo, double relativeDistance, bool isCheck = true, int delay = DEFAULT_MOTION_DELAY, int ratio = DEFAULT_MOTION_RATIO, string caption = "", int captionRetried = 0)
		{
			var speedPattern = GetMotionSpeedContentByOperation();

			MOTION_RESULT motionResult = MoveReleatively(ComplementToCaption(caption), axisNo, relativeDistance, speedPattern, ratio, delay, 0, isCheck, captionRetried);

			switch (motionResult)
			{
				case MOTION_RESULT.OK: return true;
				case MOTION_RESULT.NOT_READY:
				case MOTION_RESULT.NOT_READY_REGISTEDINSTANCE:
				case MOTION_RESULT.FAIL_GET_ONERSHIP:
					return false;
				default:
					GenerateMotionAlarm(axisNo, motionResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}

		// 2024.09.11 by jhshin [ADD] add override method with action name, retry
		protected bool MoveAxisMotionByCustomSpeed(int axisNo, double destination, double customSpeed, bool isCheck = true, int delay = DEFAULT_MOTION_DELAY, int ratio = DEFAULT_MOTION_RATIO, string caption = "", int captionRetried = 0)
		{
			var speedPattern = _taskOperator.GetMotionSpeedContentByOperation();
			MOTION_RESULT motionResult = MoveAbsolutely(ComplementToCaption(caption), axisNo, destination, customSpeed, speedPattern, ratio, delay, 0, isCheck, captionRetried);

			switch (motionResult)
			{
				case MOTION_RESULT.OK: return true;
				case MOTION_RESULT.NOT_READY:
				case MOTION_RESULT.NOT_READY_REGISTEDINSTANCE:
				case MOTION_RESULT.FAIL_GET_ONERSHIP:
					return false;
				default:
					GenerateMotionAlarm(axisNo, motionResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}
		protected bool MoveAxisMotionByList(int axisNo, List<double> positions, List<bool> useCustomSpeeds, bool isCheck = true, int delay = DEFAULT_MOTION_DELAY, int ratio = DEFAULT_MOTION_RATIO, string caption = "", int captionRetried = 0)
		{
			var arrPosition = positions.ToArray();
			var arrSpeed = _taskOperator.GetMotionSpeedContentByOperation(useCustomSpeeds).ToArray();
			List<int> listRatio = new List<int>();
			for (int i = 0; i < positions.Count; ++i)
			{
				listRatio.Add(ratio);
			}
			int[] arrRatio = listRatio.ToArray();

			MOTION_RESULT motionResult = MoveByList(ComplementToCaption(caption), axisNo, positions.Count, ref arrPosition, ref arrSpeed, ref arrRatio, delay, 0, isCheck, captionRetried);

			switch (motionResult)
			{
				case MOTION_RESULT.OK: return true;
				case MOTION_RESULT.NOT_READY:
				case MOTION_RESULT.NOT_READY_REGISTEDINSTANCE:
				case MOTION_RESULT.FAIL_GET_ONERSHIP:
					return false;
				default:
					GenerateMotionAlarm(axisNo, motionResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}
		// 2024.09.11 by jhshin [ADD] add override method with action name, retry
		protected bool MoveAxisMotionByList(int axisNo, List<double> positions, List<double> speeds, bool isCheck = true, int delay = DEFAULT_MOTION_DELAY, int ratio = DEFAULT_MOTION_RATIO, string caption = "", int captionRetried = 0)
		{
			var arrPosition = positions.ToArray();
			var arrSpeed = speeds.ToArray();

			List<int> listRatio = new List<int>();
			List<bool> listPattern = new List<bool>();

			for (int i = 0; i < positions.Count; ++i)
			{
				listRatio.Add(ratio);
				listPattern.Add(false);
			}
			var arrPattern = _taskOperator.GetMotionSpeedContentByOperation(listPattern).ToArray();
			int[] arrRatio = listRatio.ToArray();

			MOTION_RESULT motionResult = MoveByList(ComplementToCaption(caption), axisNo, positions.Count, ref arrPosition, ref arrSpeed, ref arrPattern, ref arrRatio, delay, 0, isCheck, captionRetried);

			switch (motionResult)
			{
				case MOTION_RESULT.OK: return true;
				case MOTION_RESULT.NOT_READY:
				case MOTION_RESULT.NOT_READY_REGISTEDINSTANCE:
				case MOTION_RESULT.FAIL_GET_ONERSHIP:
					return false;
				default:
					GenerateMotionAlarm(axisNo, motionResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}

		/// <summary>
		/// 2024.11.25. by shkim. [ADD] ListMotion 사용할 때, SpeedContent도 직접 입력하는 인터페이스
		/// </summary>
		protected bool MoveAxisMotionByList(int axisNo, List<double> positions, List<double> speeds, List<Motion_.MOTION_SPEED_CONTENT> speedContents, bool isCheck = true, int delay = DEFAULT_MOTION_DELAY, int ratio = DEFAULT_MOTION_RATIO, string caption = "", int captionRetried = 0)
		{
			var arrPosition = positions.ToArray();
			var arrSpeed = speeds.ToArray();

			List<int> listRatio = new List<int>();

			for (int i = 0; i < positions.Count; ++i)
			{
				listRatio.Add(ratio);
			}
			var arrPattern = speedContents.ToArray();
			int[] arrRatio = listRatio.ToArray();

			MOTION_RESULT motionResult = MoveByList(ComplementToCaption(caption), axisNo, positions.Count, ref arrPosition, ref arrSpeed, ref arrPattern, ref arrRatio, delay, 0, isCheck, captionRetried);

			switch (motionResult)
			{
				case MOTION_RESULT.OK: return true;
				case MOTION_RESULT.NOT_READY:
				case MOTION_RESULT.NOT_READY_REGISTEDINSTANCE:
				case MOTION_RESULT.FAIL_GET_ONERSHIP:
					return false;
				default:
					GenerateMotionAlarm(axisNo, motionResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}

		// 2024.09.11 by jhshin [ADD] add override method with action name, retry
		protected bool MoveAxisMotionByTouch(int axisNo, double destination, int encoderNo, double touchThreshold, double customSpeed, bool isCheck = true, int delay = DEFAULT_MOTION_DELAY, int ratio = DEFAULT_MOTION_RATIO, string caption = "", int captionRetried = 0)
		{
			var speedPattern = _taskOperator.GetMotionSpeedContentByOperation();
			MOTION_RESULT motionResult = MoveUntilTouch(ComplementToCaption(caption), axisNo, destination, encoderNo, touchThreshold, ref customSpeed, speedPattern, ratio, delay, 0, isCheck, captionRetried);

			switch (motionResult)
			{
				case MOTION_RESULT.OK: return true;
				case MOTION_RESULT.NOT_READY:
				case MOTION_RESULT.NOT_READY_REGISTEDINSTANCE:
				case MOTION_RESULT.FAIL_GET_ONERSHIP:
					return false;
				default:
					GenerateMotionAlarm(axisNo, motionResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}
		// 2024.09.11 by jhshin [ADD] add override method with action name, retry
		protected bool MoveAxisMotionBySoftStep(int axisNo, double destination, double slowSectionDistance, double slowSectionSpeed, EN_SOFT_STEP_TYPE softStepType, bool isCheck = true, int delay = DEFAULT_MOTION_DELAY, int ratio = DEFAULT_MOTION_RATIO, string caption = "", int captionRetried = 0)
		{
			double command = GetCommandPosition(axisNo);

			slowSectionDistance = Math.Abs(slowSectionDistance);    // slow distance 부호 삭제
			if (slowSectionDistance <= 0.001)   // 2024.08.30 by junho [ADD] 1um이하라면 그냥 일반 모션으로 구동 (RSA에서 같은 위치로 여러번 보내면 축 알람 발생하는 경우 있었음)
			{
				return MoveAxisMotion(axisNo, destination, isCheck, delay, ratio, caption, captionRetried);
			}


			Func<bool> IsInSlowSection = () =>
			{
				#region
				switch (softStepType)
				{
					case EN_SOFT_STEP_TYPE.PRE:
					case EN_SOFT_STEP_TYPE.POST:
						return FunctionsETC.IsInTolerance(command, destination, slowSectionDistance);
					case EN_SOFT_STEP_TYPE.DUAL:
						return FunctionsETC.IsInTolerance(command, destination, (slowSectionDistance * 2)); // dual일 경우에는 범위가 2배
					default: return true;
				}
				#endregion
			};

			// 이동 거리가 짧을 경우, List motion을 사용할 필요가 없다.
			if (IsInSlowSection())
			{
				return MoveAxisMotionByCustomSpeed(axisNo, destination, slowSectionSpeed, isCheck, delay, ratio, caption, captionRetried);
			}

			// List motion을 만들어서 구동
			var positionList = new List<double>();
			var useCustomSpeedList = new List<bool>();
			AxSetSpeed(axisNo, slowSectionSpeed, Config.ConfigMotion.EN_SPEED_CONTENT.CUSTOM_1);    // slow speed 미리 적용
			if (command > destination)                  // 만약 -방향으로 움직이면 motion이면 search offset도 -방향으로
				slowSectionDistance *= -1;

			switch (softStepType)
			{
				case EN_SOFT_STEP_TYPE.PRE:
					positionList.Add(command + slowSectionDistance);
					positionList.Add(destination);
					useCustomSpeedList.Add(true);
					useCustomSpeedList.Add(false);
					break;
				case EN_SOFT_STEP_TYPE.POST:
					positionList.Add(destination - slowSectionDistance);
					positionList.Add(destination);
					useCustomSpeedList.Add(false);
					useCustomSpeedList.Add(true);
					break;
				case EN_SOFT_STEP_TYPE.DUAL:
					positionList.Add(command + slowSectionDistance);
					positionList.Add(destination - slowSectionDistance);
					positionList.Add(destination);
					useCustomSpeedList.Add(true);
					useCustomSpeedList.Add(false);
					useCustomSpeedList.Add(true);
					break;
				default: return false;
			}

			return MoveAxisMotionByList(axisNo, positionList, useCustomSpeedList, isCheck, delay, ratio, caption, captionRetried);
		}
		/// <summary>
		/// 각기 다른 slow pre/post speed를 가진 dual softstep을 사용하기 위한 interface
		/// </summary>
		protected bool MoveAxisMotionBySoftStep(int axisNo, double destination, double[] slowSectionDistance, double[] slowSectionSpeed, bool isCheck = true, int delay = DEFAULT_MOTION_DELAY, int ratio = DEFAULT_MOTION_RATIO, string caption = "", int captionRetried = 0)
		{
			if (slowSectionDistance.Length != 2 || slowSectionSpeed.Length != 2)
				return false;

			double command = GetCommandPosition(axisNo);

			// slow distance 부호 삭제
			slowSectionDistance = slowSectionDistance.Select(x => Math.Abs(x)).ToArray();

			Func<bool> IsInSlowSection = () =>
			#region
			{
				return FunctionsETC.IsInTolerance(command, destination
					, (slowSectionDistance[0] + slowSectionDistance[1]));
			};
			#endregion

			// 이동 거리가 짧을 경우, List motion을 사용할 필요가 없다.
			if (IsInSlowSection())
			{
				return MoveAxisMotionByCustomSpeed(axisNo, destination, slowSectionSpeed[1], isCheck, delay, ratio, caption, captionRetried);
			}

			// List motion을 만들어서 구동
			var positionList = new List<double>();
			if (command > destination)                  // 만약 -방향으로 움직이면 motion이면 search offset도 -방향으로
				slowSectionDistance = slowSectionDistance.Select(x => x *= -1).ToArray();

			// 2024.08.30 by junho [ADD] 1um이하라면 그냥 일반 모션으로 구동 (RSA에서 같은 위치로 여러번 보내면 축 알람 발생하는 경우 있었음)
			if (slowSectionDistance[0] < -0.001 || slowSectionDistance[0] > 0.001)
				positionList.Add(command + slowSectionDistance[0]);
			if (slowSectionDistance[1] < -0.001 || slowSectionDistance[1] > 0.001)
				positionList.Add(destination - slowSectionDistance[1]);
			positionList.Add(destination);

			double normalSpeed = AxGetSpeed(axisNo, _taskOperator.GetConfigSpeedContentByOperation());
			if (normalSpeed <= 0) return false;
			var speedList = new List<double>();
			speedList.Add(slowSectionSpeed[0]);
			speedList.Add(normalSpeed);
			speedList.Add(slowSectionSpeed[1]);

			return MoveAxisMotionByList(axisNo, positionList, speedList, isCheck, delay, ratio, caption, captionRetried);
		}
		protected bool MoveAxisMotionBySpeedContent(int axisNo, double destination, Motion_.MOTION_SPEED_CONTENT speedContent, bool isCheck = true, int delay = DEFAULT_MOTION_DELAY, int ratio = DEFAULT_MOTION_RATIO, string caption = "", int captionRetried = 0)
		{
			MOTION_RESULT motionResult = MoveAbsolutely(ComplementToCaption(caption), axisNo, destination, speedContent, ratio, delay, 0, isCheck, captionRetried);

			switch (motionResult)
			{
				case MOTION_RESULT.OK: return true;
				case MOTION_RESULT.NOT_READY:
				case MOTION_RESULT.NOT_READY_REGISTEDINSTANCE:
				case MOTION_RESULT.FAIL_GET_ONERSHIP:
					return false;
				default:
					GenerateMotionAlarm(axisNo, motionResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}
		protected bool MoveAxisMotionByReleatively(int axisNo, double destination, bool isCheck = true, int delay = DEFAULT_MOTION_DELAY, int ratio = DEFAULT_MOTION_RATIO, string caption = "", int captionRetried = 0)
		{
			var speedPattern = GetMotionSpeedContentByOperation();
			MOTION_RESULT motionResult = MoveReleatively(ComplementToCaption(caption), axisNo, destination, speedPattern, ratio, delay, 0, isCheck, captionRetried);

			switch (motionResult)
			{
				case MOTION_RESULT.OK: return true;
				case MOTION_RESULT.NOT_READY:
				case MOTION_RESULT.NOT_READY_REGISTEDINSTANCE:
				case MOTION_RESULT.FAIL_GET_ONERSHIP:
					return false;
				default:
					GenerateMotionAlarm(axisNo, motionResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}
		protected bool MoveAxisSpeed(int axisNo, bool direction, bool isCheck = true, int ratio = DEFAULT_MOTION_RATIO, string caption = "", int captionRetried = 0)
		{
			MOTION_RESULT motionResult = MoveAtSpeed(ComplementToCaption(caption), axisNo, direction, ratio, 0, isCheck, captionRetried);

			switch (motionResult)
			{
				case MOTION_RESULT.OK: return true;
				case MOTION_RESULT.NOT_READY:
				case MOTION_RESULT.NOT_READY_REGISTEDINSTANCE:
				case MOTION_RESULT.FAIL_GET_ONERSHIP:
					return false;
				default:
					GenerateMotionAlarm(axisNo, motionResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}
		protected bool MoveAxisSpeed(int axisNo, double speed, bool direction, bool isCheck = true, int ratio = DEFAULT_MOTION_RATIO, string caption = "", int captionRetried = 0)
		{
			MOTION_RESULT motionResult = MoveAtSpeed(ComplementToCaption(caption), axisNo, speed, direction, ratio, 0, isCheck, captionRetried);

			switch (motionResult)
			{
				case MOTION_RESULT.OK: return true;
				case MOTION_RESULT.NOT_READY:
				case MOTION_RESULT.NOT_READY_REGISTEDINSTANCE:
				case MOTION_RESULT.FAIL_GET_ONERSHIP:
					return false;
				default:
					GenerateMotionAlarm(axisNo, motionResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}
		// 2024.09.11 by jhshin [ADD] add override method with action name, retry
		protected bool StopAxisMotion(int axisNo, bool isEmergency = false, int delay = DEFAULT_MOTION_DELAY, string caption = "", int captionRetried = 0)
		{
			caption = caption == "" ? string.Format("{0}_Stop", _actionName) : string.Format("{0}_Stop_{1}_{2}", _actionName, caption, m_nSeqNum.ToString());
			MOTION_RESULT motionResult = StopMotion(caption, axisNo, isEmergency, delay, captionRetried);

			switch (motionResult)
			{
				case MOTION_RESULT.OK: return true;
				case MOTION_RESULT.NOT_READY:
				case MOTION_RESULT.NOT_READY_REGISTEDINSTANCE:
				case MOTION_RESULT.FAIL_GET_ONERSHIP:
					return false;
				default:
					GenerateMotionAlarm(axisNo, motionResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}
		// 2024.09.11 by jhshin [ADD] add override method with action name, retry
		protected bool MoveAxisHome(int axisNo, int preDelay = 0, bool isCheck = true)
		{
			MOTION_RESULT motionResult = MoveToHome(axisNo, "Home", preDelay, isCheck, 0);

			switch (motionResult)
			{
				case MOTION_RESULT.OK:
					return true;

				case MOTION_RESULT.NOT_READY:
				case MOTION_RESULT.NOT_READY_REGISTEDINSTANCE:
				case MOTION_RESULT.FAIL_GET_ONERSHIP:
					return false;
				default:
					GenerateMotionAlarm(axisNo, motionResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}
		protected double AxGetNormalSpeed(int axisNo)
		{
			return AxGetSpeed(axisNo, _taskOperator.GetConfigSpeedContentByOperation());
		}

		protected void ClearAxisPosition(int axisNo)
		{
			SetCommandPosition(axisNo, 0.0);
			SetActualPosition(axisNo, 0.0);
		}
        protected void ClearActualPosition(int axisNo)
        {
            SetActualPosition(axisNo, 0.0);
        }

		protected void GenerateMotionAlarm(int axisNo, MOTION_RESULT category)
		{
			GenerateAlarm((int)EN_TASK_COMMON_ALARM.MOTION_ERROR
				, string.Format("[{0}] : {1}", AxGetDeviceName(axisNo), category.ToString()));
		}
		protected void GenerateMotionAlarm(string axisName, MOTION_RESULT category)
		{
			GenerateAlarm((int)EN_TASK_COMMON_ALARM.MOTION_ERROR
				, string.Format("[{0}] : {1}", axisName, category.ToString()));
		}
		protected void GenerateMotionAlarm(string axisNameX, string axisNameY, MOTION_RESULT category)
		{
			GenerateAlarm((int)EN_TASK_COMMON_ALARM.MOTION_ERROR
				, string.Format("[{0}, {1}] : {2}", axisNameX, axisNameY, category.ToString()));
		}

		/// <summary>
		/// 필요 시 override해서 action별 종료 step을 정의한다.
		/// switch(m_enAction)
		/// {
		/// 	case TASK_ACTION.SAMPLE:
		/// 		return (int)STEP_SAMPLE.TERMINATE;
		/// 		break;
		/// }
		/// </summary>
		protected virtual int GetEndStepNoByActionName()
		{
			return SEQUENCE_END_STEP;
		}
		#endregion /motion
		
		#region cylinder
		// 2024.09.09 by jhshin [MOD] add overloading method with actionName, retry count
		protected new bool MoveCylinderForward(int cylinderNo, bool isCheck = true, int captionRetried = 0)
		{
			if (IsForward(cylinderNo))
				return true;

			CYLINDER_RESULT cylinderResult = MoveForward(cylinderNo, isCheck, ComplementToCaption("Forward"), captionRetried);
			switch (cylinderResult)
			{
				case CYLINDER_RESULT.OK:
					return true;

				case CYLINDER_RESULT.NOT_READY:
				case CYLINDER_RESULT.NOT_READY_REGISTEDINSTANCE:
				case CYLINDER_RESULT.FAIL_GET_ONERSHIP:
					return false;

				default:
					GenerateCylinderAlarm(cylinderNo, cylinderResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}
		// 2024.09.09 by jhshin [MOD] add overloading method with action name, retry count
		protected new bool MoveCylinderBackward(int cylinderNo, bool isCheck = true, int captionRetried = 0)
		{
			if (IsBackward(cylinderNo))
				return true;

			CYLINDER_RESULT cylinderResult = MoveBackward(cylinderNo, isCheck, ComplementToCaption("Backward"), captionRetried);
			switch (cylinderResult)
			{
				case CYLINDER_RESULT.OK:
					return true;

				case CYLINDER_RESULT.NOT_READY:
				case CYLINDER_RESULT.NOT_READY_REGISTEDINSTANCE:
				case CYLINDER_RESULT.FAIL_GET_ONERSHIP:
					return false;

				default:
					GenerateCylinderAlarm(cylinderNo, cylinderResult);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}
		protected void GenerateCylinderAlarm(int cylinderNo, CYLINDER_RESULT category)
		{
			GenerateAlarm((int)EN_TASK_COMMON_ALARM.CYLINDER_ERROR
				, string.Format("[{0}] : {1}", CyGetDeviceName(cylinderNo), category.ToString()));
		}
		#endregion /cylinder

		#region digital io
		protected bool DoutChange(int doutNo, bool targetValue, string caption = "",  int captionRetried = 0)
		{
			if (DoutReadCompare(doutNo, targetValue))
				return true;

			DIO_RESULT result = WriteOutput(doutNo, ComplementToCaption(caption), targetValue, captionRetried);
			switch (result)
			{
				case DIO_RESULT.OK:
					return true;

				case DIO_RESULT.NOT_READY:
				case DIO_RESULT.NOT_READY_REGISTEDINSTANCE:
				case DIO_RESULT.FAIL_GET_ONERSHIP:
					return false;

				default:
					GenerateDoutAlarm(doutNo, result);
					m_nSeqNum = GetEndStepNoByActionName();
					return false;
			}
		}
		#endregion /digital io

		#region analog io
		protected void AoutWrite(int targetNo, double targetValue, string caption = "", int captionRetried = 0)
		{
			if (_taskOperator.IsDryRunOrSimulationMode())
				return;

			WriteOutputValue(targetNo, ComplementToCaption(caption), targetValue, captionRetried);
		}
		#endregion /analog io

		private string ComplementToCaption(string caption)
		{
			return caption == "" ? string.Format("{0}_{1}", _actionName, m_nSeqNum.ToString()) : string.Format("{0}_{1}_{2}", _actionName, m_nSeqNum.ToString(), caption);
		}

		#region digital io
		protected bool DinCompare(int dinNo, bool targetValue)
		{
			if (_taskOperator.IsDryRunOrSimulationMode())
				return true;

			return targetValue == ReadInput(dinNo, targetValue);
		}
		protected bool DoutReadCompare(int doutNo, bool targetValue)
		{
			if (_taskOperator.IsDryRunOrSimulationMode())
				return true;

			return targetValue == ReadOutput(doutNo, targetValue);
		}
		protected void GenerateDoutAlarm(int doutNo, DIO_RESULT category)
		{
			GenerateAlarm((int)EN_TASK_COMMON_ALARM.DIGITAL_ERROR
						, string.Format("[{0}] : {1}", DoutGetDeviceName(doutNo), category.ToString()));
		}
		#endregion /digital io

		#region analog io
		protected bool AinIsInRange(int targetNo, double threshold, double tolerance)
		{
			if (_taskOperator.IsDryRunOrSimulationMode())
				return true;

			if (false == AiIsEnable(targetNo))
				return true;

			double read = ReadInputValue(targetNo);

			bool result = FunctionsETC.IsInTolerance(read, threshold, tolerance);
			return result;
		}
		protected bool AinIsUnder(int targetNo, double threshold)
		{
			if (_taskOperator.IsDryRunOrSimulationMode())
				return true;

			if (false == AiIsEnable(targetNo))
				return true;

			double read = ReadInputValue(targetNo);

			bool result = read <= threshold;
			return result;
		}
		protected bool AinIsHigh(int targetNo, double threshold)
		{
			if (_taskOperator.IsDryRunOrSimulationMode())
				return true;

			if (false == AiIsEnable(targetNo))
				return true;

			double read = ReadInputValue(targetNo);

			bool result = read >= threshold;
			return result;
		}

		protected double AoutRead(int targetNo)
		{
			return ReadOutputValue(targetNo);
		}
		#endregion /analog io

		#region alarm
		protected void GenerateAlarm(int alarmNo)
		{
			var subInformation = new string[] { GetTaskName() };
			GenerateSequenceAlarm(alarmNo, false, ref subInformation);
		}
		protected void GenerateAlarm(int alarmNo, string message)
		{
			var subInformation = new string[] { GetTaskName(), message };
			GenerateSequenceAlarm(alarmNo, false, ref subInformation);
		}
		#endregion /alarm

		#region dynamic link
		protected bool InitializeDynamicLinkState()
		{
			if (false == InitializeNodeState()) return false;
			if (false == InitializePortLink()) return false;
			if (false == InitializeNodeLink()) return false;
			TerminateWorkFlow();
			return true;
		}
		protected bool InitializeNodeState()
		{
			return _dynamicLink.InitializeNodeState(GetTaskName());
		}
		protected bool InitializePortLink()
		{
			return _dynamicLink.InitializePortLink(GetTaskName());
		}
		protected bool InitializeNodeLink()
		{
			return _dynamicLink.InitializeNodeLink(GetTaskName());
		}
		protected void TerminateWorkFlow()
		{
			_dynamicLink.TerminateWorkFlow(GetTaskName());
		}

		protected bool SetNodeState(string actionName, EN_ACTION_STATE state)
		{
			return _dynamicLink.SetNodeState(GetTaskName(), actionName, state);
		}
		protected bool GetNodeState(string actionName, out EN_ACTION_STATE state)
		{
			state = EN_ACTION_STATE.SKIP;
			return _dynamicLink.GetNodeState(GetTaskName(), actionName, ref state);
		}
		protected bool GetNodeState(EN_TASK_LIST task, string actionName, out EN_ACTION_STATE state)
		{
			state = EN_ACTION_STATE.SKIP;
			return _dynamicLink.GetNodeState(task.ToString(), actionName, ref state);
		}

		protected bool SetPortStatus(string port, EN_PORT_STATUS status)
		{
			return _dynamicLink.SetPortStatus(GetTaskName(), port, status);
		}
		protected bool SetPortStatus(EN_TASK_LIST task, string port, EN_PORT_STATUS status)
		{
			return _dynamicLink.SetPortStatus(task.ToString(), port, status);
		}

		protected DynamicLink_.EN_PORT_STATUS GetPortStatus(string port)
		{
			var readStatus = DynamicLink_.EN_PORT_STATUS.UNABLE;
			if (false == _dynamicLink.GetPortStatus(GetTaskName(), port, ref readStatus))
				readStatus = DynamicLink_.EN_PORT_STATUS.UNABLE;

			return readStatus;
		}
		protected DynamicLink_.EN_PORT_STATUS GetPortStatus(EN_TASK_LIST task, string port)
		{
			var readStatus = DynamicLink_.EN_PORT_STATUS.UNABLE;
			if (false == _dynamicLink.GetPortStatus(task.ToString(), port, ref readStatus))
				readStatus = DynamicLink_.EN_PORT_STATUS.UNABLE;

			return readStatus;
		}
		protected bool CheckPortStatus(string port, EN_PORT_STATUS targetStatus)
		{
			return GetPortStatus(port).Equals(targetStatus);
		}
		protected bool CheckPortStatus(EN_TASK_LIST task, string port, EN_PORT_STATUS status)
		{
			var readStatus = DynamicLink_.EN_PORT_STATUS.UNABLE;
			if (false == _dynamicLink.GetPortStatus(task.ToString(), port, ref readStatus))
				readStatus = DynamicLink_.EN_PORT_STATUS.UNABLE;

			return readStatus.Equals(status);
		}

		protected bool SetPortFinalCondition(string actionName, ActionNode.CheckConditionDelegate pDelegate)
		{
			return _dynamicLink.SetPortFinalCondition(GetTaskName(), actionName, pDelegate);
		}
		protected bool SetFlowPostCondition(string actionName, ActionNode.CheckActionDelegate pDelegate)
		{
			return _dynamicLink.SetFlowPostcondition(GetTaskName(), actionName, pDelegate);
		}
		protected bool SetNodeFinalCondition(string actionName, ActionNode.CheckConditionDelegate pDelegate)
		{
			return _dynamicLink.SetNodeFinalCondition(GetTaskName(), actionName, pDelegate);
		}

		protected bool IsLinkedActionToThisTask(string actionName, EN_TASK_LIST targetTask)
		{
			return _dynamicLink.IsExistThisTaskInSelectedList(GetTaskName(), actionName, targetTask.ToString());
		}
		protected bool IsContinuousFlow()
		{
			return _dynamicLink.IsContinuousFlow(GetTaskName());
		}
		protected void InitiateWorkFlow()
		{
			_dynamicLink.InitiateWorkFlow(GetTaskName());   // Flow 시작 (Flow no -> 1번으로)
		}
		protected bool SetFlowTable(string tableName)
		{
			return _dynamicLink.SetFlowTable(GetTaskName(), tableName); // 사용할 Flow table을 변경
		}
		protected bool GetFlowTableList(out string[] tableList)
		{
			string[] arrFlowTable = null;
			bool result = _dynamicLink.GetFlowTableList(GetTaskName(), ref arrFlowTable);   // flow table 가져오기
			if (false == result)
			{
				tableList = null;
				return false;
			}

			tableList = arrFlowTable;
			return true;
		}

		#endregion /dynamic link

		#region log
		protected void WriteTransferLog(EN_XFR_TYPE enLogType)
		{
			_log.WriteTransferLog(GetTaskName()
									, enLogType
									, _actionName
									, null      // Material ID
									, GetTaskName()     // From
									, GetTaskName());     // To
		}
		// 2024.08.20 by jhshin
		protected void WriteTransferLog(EN_XFR_TYPE enLogType, string meterialId, string fromDevice, string toDevice)
		{
			_log.WriteTransferLog(GetTaskName()
									, enLogType
									, _actionName
									, meterialId    // Material ID
									, fromDevice    // From
									, toDevice);      // To
		}
		protected void WriteActionProcessLog(EN_PRC_TYPE logType, string materialId)
		{
			_log.WriteProcessLog(GetTaskName(), logType, _actionName, materialId);
		}
		#endregion /log

		#region recipe control
		protected bool SetParameter(Enum parameter, string setValue)
		{
			Type parameterType = parameter.GetType();
			if (parameterType.Equals(typeof(Recipe.PARAM_COMMON)))
			{
				return _recipe.SetValue(Recipe.EN_RECIPE_TYPE.COMMON, parameter.ToString(), setValue);
			}
			else if (parameterType.Equals(typeof(Recipe.PARAM_EQUIPMENT)))
			{
				return _recipe.SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT, parameter.ToString(), setValue);
			}
			else if (parameterType.Equals(typeof(PARAM_GLOBAL)))
			{
				return _recipe.SetValue(EN_TASK_LIST.Global.ToString(), parameter.ToString(), setValue);
			}
			else
			{
				if (false == _processParameterList.ContainsKey(parameter))
					return false;

				return _recipe.SetValue(GetTaskName(), _processParameterList[parameter], setValue);
			}
		}
		protected bool SetParameter(Enum parameter, bool setValue)
		{
			return SetParameter(parameter, setValue.ToString());
		}
		protected bool SetParameter(Enum parameter, int setValue)
		{
			return SetParameter(parameter, setValue.ToString());
		}
		protected bool SetParameter(Enum parameter, double setValue)
		{
			return SetParameter(parameter, setValue.ToString());
		}
		protected bool SetParameter(Enum parameter, Enum setValue)
		{
			Type parameterType = parameter.GetType();
			if (parameterType.Equals(typeof(Recipe.PARAM_COMMON)))
			{
				return _recipe.SetValue(Recipe.EN_RECIPE_TYPE.COMMON, parameter.ToString(), setValue.ToString());
			}
			else if (parameterType.Equals(typeof(Recipe.PARAM_EQUIPMENT)))
			{
				return _recipe.SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT, parameter.ToString(), setValue.ToString());
			}
			else if (parameterType.Equals(typeof(PARAM_GLOBAL)))
			{
				return _recipe.SetValue(EN_TASK_LIST.Global.ToString(), parameter.ToString(), setValue.ToString());
			}
			else
			{
				if (false == _processParameterList.ContainsKey(parameter))
					return false;

				return _recipe.SetValue(GetTaskName(), _processParameterList[parameter], setValue.ToString());
			}
		}
		protected bool SetParameter(Enum parameter, TimeSpan setValue)
		{
			Type parameterType = parameter.GetType();
			if (parameterType.Equals(typeof(Recipe.PARAM_COMMON)))
			{
				return _recipe.SetValue(Recipe.EN_RECIPE_TYPE.COMMON, parameter.ToString(), setValue.ToString());
			}
			else if (parameterType.Equals(typeof(Recipe.PARAM_EQUIPMENT)))
			{
				return _recipe.SetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT, parameter.ToString(), setValue.ToString());
			}
			else if (parameterType.Equals(typeof(PARAM_GLOBAL)))
			{
				return _recipe.SetValue(EN_TASK_LIST.Global.ToString(), parameter.ToString(), setValue.ToString());
			}
			else
			{
				if (false == _processParameterList.ContainsKey(parameter))
					return false;

				return _recipe.SetValue(GetTaskName(), _processParameterList[parameter], setValue.ToString());
			}
		}

		protected double GetParameter(Enum parameter, double defaultValue)
		{
			Type parameterType = parameter.GetType();
			if (parameterType.Equals(typeof(Recipe.PARAM_COMMON)))
			{
				return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.COMMON, parameter.ToString(), defaultValue);
			}
			else if (parameterType.Equals(typeof(Recipe.PARAM_EQUIPMENT)))
			{
				return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT, parameter.ToString(), defaultValue);
			}
			else if (parameterType.Equals(typeof(PARAM_GLOBAL)))
			{
				return _recipe.GetValue(EN_TASK_LIST.Global.ToString(), parameter.ToString(), defaultValue);
			}
			else
			{
				if (false == _processParameterList.ContainsKey(parameter))
					return defaultValue;

				return _recipe.GetValue(GetTaskName(), _processParameterList[parameter], defaultValue);
			}
		}
		protected int GetParameter(Enum parameter, int defaultValue)
		{
			Type parameterType = parameter.GetType();
			if (parameterType.Equals(typeof(Recipe.PARAM_COMMON)))
			{
				return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.COMMON, parameter.ToString(), defaultValue);
			}
			else if (parameterType.Equals(typeof(Recipe.PARAM_EQUIPMENT)))
			{
				return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT, parameter.ToString(), defaultValue);
			}
			else if (parameterType.Equals(typeof(PARAM_GLOBAL)))
			{
				return _recipe.GetValue(EN_TASK_LIST.Global.ToString(), parameter.ToString(), defaultValue);
			}
			else
			{
				if (false == _processParameterList.ContainsKey(parameter))
					return defaultValue;

				return _recipe.GetValue(GetTaskName(), _processParameterList[parameter], defaultValue);
			}
		}
		protected bool GetParameter(Enum parameter, bool defaultValue)
		{
			Type parameterType = parameter.GetType();
			if (parameterType.Equals(typeof(Recipe.PARAM_COMMON)))
			{
				return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.COMMON, parameter.ToString(), defaultValue);
			}
			else if (parameterType.Equals(typeof(Recipe.PARAM_EQUIPMENT)))
			{
				return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT, parameter.ToString(), defaultValue);
			}
			else if (parameterType.Equals(typeof(PARAM_GLOBAL)))
			{
				return _recipe.GetValue(EN_TASK_LIST.Global.ToString(), parameter.ToString(), defaultValue);
			}
			else
			{
				if (false == _processParameterList.ContainsKey(parameter))
					return defaultValue;

				return _recipe.GetValue(GetTaskName(), _processParameterList[parameter], defaultValue);
			}
		}
		protected string GetParameter(Enum parameter, string defaultValue)
		{
			Type parameterType = parameter.GetType();
			if (parameterType.Equals(typeof(Recipe.PARAM_COMMON)))
			{
				return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.COMMON, parameter.ToString(), defaultValue);
			}
			else if (parameterType.Equals(typeof(Recipe.PARAM_EQUIPMENT)))
			{
				return _recipe.GetValue(Recipe.EN_RECIPE_TYPE.EQUIPMENT, parameter.ToString(), defaultValue);
			}
			else if (parameterType.Equals(typeof(PARAM_GLOBAL)))
			{
				return _recipe.GetValue(EN_TASK_LIST.Global.ToString(), parameter.ToString(), defaultValue);
			}
			else
			{
				if (false == _processParameterList.ContainsKey(parameter))
					return defaultValue;

				return _recipe.GetValue(GetTaskName(), _processParameterList[parameter], defaultValue);
			}
		}
		protected T GetParameter<T>(Enum parameter, T defaultValue) where T : struct
		{
			if (false == typeof(T).IsEnum)
				return defaultValue;

			T result;
			if (false == Enum.TryParse(GetParameter(parameter, defaultValue.ToString()), out result))
				return defaultValue;

			return result;
		}
		protected TimeSpan GetParameter(Enum parameter, TimeSpan defaultValue)
		{
			string strValue = GetParameter(parameter, defaultValue.ToString());
			TimeSpan result;
			if (false == TimeSpan.TryParse(strValue, out result))
				return defaultValue;

			return result;
		}
		#endregion /recipe control

		#region sub sequence
		protected override void InitSubSequence()
		{
			// if task have subsequence, need use this method.
		}

		Dictionary<Enum, ASubSequence> _subSequenceList = new Dictionary<Enum, ASubSequence>();
		protected void AddNewSubsequence(Enum subSequence, ASubSequence subSeqsuenceInstance)
		{
			_subSequenceList.Add(subSequence, subSeqsuenceInstance);
		}
		protected override void SubSequenceStepInit()
		{
			foreach (var subSeq in _subSequenceList.Values)
			{
				subSeq.InitSequenceStep();
			}
		}

		protected bool SubSequenceRun(Enum subSequence)
		{
			var instance = GetSubSequenceInstance(subSequence);
			if (instance == null) return false;

			if (false == instance.Activate)
			{
				var parameter = UpdateSubSequenceParameter(subSequence.ToString());
				if (parameter == null) return false;

				instance.AddParameter(parameter);
				instance.Activate = true;
			}

			var subSequenceResult = instance.SubSequenceProcedure();
			switch (subSequenceResult)
			{
				case EN_SUBSEQUENCE_RESULT.WORKING: return false;
				case EN_SUBSEQUENCE_RESULT.OK:
					return true;
				default:
					{
						string message = instance.GetResultData().ID;
						GenerateAlarm((int)EN_TASK_COMMON_ALARM.SUB_SEQUENCE_FAIL
							, string.Format("{0}:{1}:{2}"
							, subSequence.ToString()
							, subSequenceResult.ToString()
							, message));
						m_nSeqNum = GetEndStepNoByActionName();
					}
					return false;
			}
		}
		protected EN_SUBSEQUENCE_RESULT SubSequenceProcedure(Enum subSequence)
		{
			var instance = GetSubSequenceInstance(subSequence);
			if (instance == null) return EN_SUBSEQUENCE_RESULT.ERROR;

			if (false == instance.Activate)
			{
				var parameter = UpdateSubSequenceParameter(subSequence.ToString());
				if (parameter == null) return EN_SUBSEQUENCE_RESULT.ERROR;

				instance.AddParameter(parameter);
				instance.Activate = true;
			}

			return instance.SubSequenceProcedure();
		}
		protected bool IsSubsequenceActivate(Enum subSequence)
		{
			var instance = GetSubSequenceInstance(subSequence);
			if (instance == null) return false;
			return instance.Activate;
		}
		protected bool SetSubSequenceBranch(Enum subSequence, int branch)
		{
			var instance = GetSubSequenceInstance(subSequence);
			if (instance == null) return false;

			instance.SetSubSeqBranch(branch);
			return true;
		}
		protected virtual SubSequenceParam UpdateSubSequenceParameter(string subSequenceName)
		{
			// if task have subsequence, need use this method.
			return null;
		}
		protected SubSeqResult GetSubsequenceResultData(Enum subSequence)
		{
			var instance = GetSubSequenceInstance(subSequence);
			if (instance == null) return null;
			return instance.GetResultData();
		}
		#endregion /sub sequence

		#region post office
		protected bool CheckMail(EN_MAIL title)
		{
			return _postOffice.CheckMail(_mySubscriberName, title);
		}
		protected bool CheckMail(EN_SUBSCRIBER sender, EN_MAIL title)
		{
			return _postOffice.CheckMail(_mySubscriberName, sender, title);
		}
		protected bool SendMail(EN_SUBSCRIBER receiver, EN_MAIL title, params object[] content)
		{
			return _postOffice.SendMail(_mySubscriberName, receiver, title, content);
		}
		protected List<Mail> GetMail(EN_MAIL title)
		{
			List<Mail> result;
			if (false == _postOffice.GetMail(_mySubscriberName, title, out result))
				return null;

			return result;
		}
		protected List<Mail> GetMail(EN_SUBSCRIBER sender, EN_MAIL title)
		{
			List<Mail> result;
			if (false == _postOffice.GetMail(_mySubscriberName, sender, title, out result))
				return null;

			return result;
		}
		#endregion /post office

		#region data
		protected virtual void InitTemporaryData()
		{
			SubSequenceStepInit();
			ResetCheckTime();
			ResetCheckCountAll();
		}
		protected void SetCheckTime(int index, int time)
		{
			if (false == _timeCheck.ContainsKey(index))
				_timeCheck.Add(index, new TickCounter_.TickCounter());

			_lastSetIndexCheckTime = index;
			_timeCheck[index].SetTickCount((uint)time);
		}
		protected bool IsTimeOverCheck(bool defaultValue)
		{
			if (_lastSetIndexCheckTime == -1)
				return defaultValue;

			return IsTimeOverCheck(_lastSetIndexCheckTime, defaultValue);
		}
		protected bool IsTimeOverCheck(int index, bool defaultValue)
		{
			if (false == _timeCheck.ContainsKey(index))
				return defaultValue;

			return _timeCheck[index].IsTickOver(defaultValue);
		}
		protected void ResetCheckTime()
		{
			_lastSetIndexCheckTime = -1;
			foreach (var time in _timeCheck.Values)
			{
				time.SetTickCount(0);   // Clear?
			}
		}

		protected bool IsOverCheckCount(int index, int limit)
		{
			bool result = limit <= GetCurrentCheckCount(index);

			if (result)
				ResetCheckCount(index);
			else
				CountUpCheckCount(index);

			return result;
		}
		protected int GetCurrentCheckCount(int index)
		{
			if (false == _countCheck.ContainsKey(index))
				_countCheck.Add(index, 0);

			return _countCheck[index];
		}
		protected void SetCurrentCheckCount(int index, int count)
		{
			if (false == _countCheck.ContainsKey(index))
				_countCheck.Add(index, count);
			else
				_countCheck[index] = count;
		}
		protected void CountUpCheckCount(int index)
		{
			if (false == _countCheck.ContainsKey(index))
				_countCheck.Add(index, 0);

			++_countCheck[index];
		}
		protected void ResetCheckCount(int index)
		{
			if (false == _countCheck.ContainsKey(index))
				_countCheck.Add(index, 0);
			else
				_countCheck[index] = 0;
		}
		protected void ResetCheckCountAll()
		{
			List<int> keys = new List<int>();
			foreach (var key in _countCheck.Keys)
			{
				keys.Add(key);
			}

			foreach (int index in keys)
			{
				_countCheck[index] = 0;
			}
		}
		#endregion /data

		#region secsgem
		protected bool UpdateScenarioParam(EN_SCENARIO scenario, Dictionary<string, string> datas)
		{
			return _scenarioOperator.UpdateScenarioParam(GetTaskName(), scenario, datas);
		}
		protected bool ExecuteScenario(EN_SCENARIO scenario)
		{
			var result = _scenarioOperator.ExecuteScenario(GetTaskName(), scenario);
			switch (result)
			{
				case EN_SCENARIO_RESULT.WAITING:
				case EN_SCENARIO_RESULT.PROCEED: return false;
				case EN_SCENARIO_RESULT.COMPLETED:
					return true;
				default:
					GenerateAlarm((int)EN_TASK_COMMON_ALARM.SCENARIO_ERROR
						, string.Format("{0}:{1}", scenario.ToString(), result.ToString()));
					return true;
			}
		}
        protected Dictionary<string, string> GetScenarioResultData(EN_SCENARIO scenario)
        {
			return _scenarioOperator.GetScenarioResultData(GetTaskName(), scenario);
        }
		#endregion /secsgem

		#endregion /common method

		#region inherit method rapping

		#region dynanic link
		protected override bool SelectExecutingSequence(ref string strActionName)
		{
			string taskName = GetTaskName();
			if (_dynamicLink.TransitState(taskName, out _actionName))
			{
				if (false == UpdateActionName(_actionName))
				{
					_actionName = STOP_ACTION;
					return false;
				}

				strActionName = _actionName;
				return true;
			}

			return false;
		}
		protected override bool SelectSetupSequence(ref string strSequenceName)
		{
			_actionName = strSequenceName;
			if (false == UpdateActionName(strSequenceName))
			{
				_actionName = STOP_ACTION;
				return false;
			}

			return true;
		}
		abstract protected bool UpdateActionName(string actionName);
		#endregion /dynanic link

		#region pre/post condition
		/// <summary>
		/// Manual action 시작 전 호출됨
		/// </summary>
		protected override void DoSetupPrecondition()
		{
			Views.Functional.Form_ProgressBar.GetInstance().ShowForm(GetTaskName(), (uint)SEQUENCE_END_STEP);

			// 직접 작성 필요
			//WriteTransferLog(EN_XFR_TYPE.START);
		}
		/// <summary>
		/// Manual action 종료 후 호출 됨
		/// </summary>
		protected override void DoSetupPostcondition()
		{
			// 직접 작성 필요
			//WriteTransferLog(EN_XFR_TYPE.END);
		}

		/// <summary>
		/// Action이 종료 될 때마다 호출 됨
		/// </summary>
		public override void UpdateFinishedAction(string strTargetTask, string strActionName)
		{
		}

		/// <summary>
		/// Auto run중 action이 시작 될 때마다 호출됨
		/// </summary>
		protected override void DoExecutingPrecondition()
		{
			// 직접 작성 필요
			//WriteTransferLog(EN_XFR_TYPE.START);
		}
		/// <summary>
		/// Auto run중 action이 종료 될 때마다 호출됨
		/// </summary>
		protected override void DoExecutingPostcondition()
		{
			_dynamicLink.SetNodeState(GetTaskName(), _actionName, EN_ACTION_STATE.DONE);
			_dynamicLink.FinishAction(GetTaskName());

			// 직접 작성 필요
			//WriteTransferLog(EN_XFR_TYPE.END);
		}
		protected void DoEntryPrecondition()
		{
			_actionName = "Entry";
		}
		protected void DoInitializePrecondition()
		{
			_actionName = "Initialize";
		}
		#endregion /pre/post condition

		#region sub sequence
		protected override bool IsMoveAbsolutelyOK(int nTargetIndex, double dblPosition)
		{
			return true;
		}
		protected override bool IsMoveReleativelyOK(int nTargetIndex, double dblDistance)
		{
			return true;
		}
		#endregion /sub sequence

		#region status
		/// <summary>
		/// 2020.12.11 by yjlee [ADD] Get the status of the task.
		/// </summary>
		protected override void GetTaskStatus(ref int[] arIndexes, ref int[] arStatus)
		{
		}
		#endregion /status

		#region sequence
		/// <summary>
		/// 2020.06.02 by yjlee [ADD] It will be called always.
		/// </summary>
		protected override void DoAlwaysSequence()
		{
		}
		/// <summary>
		/// 2020.06.02 by yjlee [ADD] If there is a manual action to be operated.
		/// - Before returning 'true', it will be called continuously.
		/// </summary>
		protected override bool DoSetupSequence()
		{
			Views.Functional.Form_ProgressBar.GetInstance().UpdateStep(GetTaskName(), (uint)m_nSeqNum);
			return false;
		}
		/// <summary>
		/// 2020.06.02 by yjlee [ADD] If there is an executing action to be operated.
		/// - Before returning 'true', it will be called continuously.
		/// </summary>
		protected override bool DoExecutingSequence()
		{
			if (_taskOperator.IsSimulationMode())
				SetDelayForSequence(50);

			_dynamicLink.CheckState(GetTaskName());

			return false;
		}

		/// <summary>
		/// 2020.06.02 by yjlee [ADD] Code the sequence for exit.
		/// - Before returning 'true', it will be called continuously.
		/// </summary>
		protected override bool DoExitSequence()
		{
			switch (m_nSeqNum)
			{
				case 0:
					_actionName = STOP_ACTION;	// 2025.06.18 by junho [ADD] sequence 종료 할 때 action name 초기화
					return true;
				default:
					m_nSeqNum = 0;
					break;
			}

			return false;
		}
		/// <summary>
		/// 2021.01.04 by yjlee [ADD] If there is an sub sequence.
		/// - after all sub sequence is complete, the executing sequence will be called.
		/// </summary>
		protected override bool DoExcutingSubSequence(int nIndexOfSubSequence, ref int nSequenceNumber)
		{
			switch (nIndexOfSubSequence)
			{
				case 0:
					return true;
				default:
					nIndexOfSubSequence = 0;
					break;
			}

			return false;
		}

		#region Error Sequence
		/// <summary>
		/// 2020.06.02 by yjlee [ADD] Before a warning occur, it will be called.
		/// - Before returning 'true', it will be called continuously.
		/// </summary>
		protected override bool ProcessBeforeWarning()
		{
			switch (m_nBeforeErrorSeqNum)
			{
				case 0:
					return true;
				default:
					m_nBeforeErrorSeqNum = 0;
					break;
			}

			return false;
		}

		/// <summary>
		/// 2020.06.02 by yjlee [ADD] After a warning occur, it will be called.
		/// - Before returning 'true', it will be called continuously.
		/// </summary>
		protected override bool ProcessAfterWarning()
		{
			switch (m_nBeforeErrorSeqNum)
			{
				case 0:
					return true;
				default:
					m_nBeforeErrorSeqNum = 0;
					break;
			}

			return false;
		}

		/// <summary>
		/// 2020.06.02 by yjlee [ADD] Before an error occur, it will be called.
		/// - Before returning 'true', it will be called continuously.
		/// </summary>
		protected override bool ProcessBeforeError()
		{
			switch (m_nBeforeErrorSeqNum)
			{
				case 0:
					return true;
				default:
					m_nBeforeErrorSeqNum = 0;
					break;
			}

			return false;
		}

		/// <summary>
		/// 2020.06.02 by yjlee [ADD] After an error occur, it will be called.
		/// - Before returning 'true', it will be called continuously.
		/// </summary>
		protected override bool ProcessAfterError()
		{
			switch (m_nBeforeErrorSeqNum)
			{
				case 0:
					return true;
				default:
					m_nBeforeErrorSeqNum = 0;
					break;
			}

			return false;
		}
		#endregion

		#endregion /sequence

		#endregion /inherit method rapping

		#region method
		private Motion_.MOTION_SPEED_CONTENT GetMotionSpeedContentByOperation(bool isCustomSpeed = false)
		{
			if (isCustomSpeed)
			{
				return Motion_.MOTION_SPEED_CONTENT.CUSTOM_1;
			}

			bool bIsRunMode = (_taskOperator.GetOperationOfEquipment() == RunningMain_.OPERATION_EQUIPMENT.RUN);
			var motionSpeedContent = bIsRunMode ? Motion_.MOTION_SPEED_CONTENT.RUN : Motion_.MOTION_SPEED_CONTENT.MANUAL;
			return (Motion_.MOTION_SPEED_CONTENT)motionSpeedContent;
		}
		private bool AxSetSpeed(int axisNo, double targetSpeed, Config.ConfigMotion.EN_SPEED_CONTENT targetContent)
		{
			// Search speed를 recipe값으로 설정
			int deviceAxisNo = GetDeviceIndex(axisNo, FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.MOTION);
			if (deviceAxisNo < 0) return false;

			_configMotionSpeed.SetSpeedParameter(deviceAxisNo
				, targetContent
				, FrameOfSystem3.Config.ConfigMotionSpeed.EN_PARAM_SPEED.VELOCITY
				, targetSpeed);

			return true;
		}
		protected double AxGetSpeed(int axisNo, Config.ConfigMotion.EN_SPEED_CONTENT targetContent)
		{
			double result = -1;

			int deviceAxisNo = GetDeviceIndex(axisNo, FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.MOTION);
			if (deviceAxisNo < 0) return result;

			if (false == _configMotionSpeed.GetSpeedParameter(deviceAxisNo, targetContent
				, Config.ConfigMotionSpeed.EN_PARAM_SPEED.VELOCITY, ref result))
				return result;

			return result;
		}
		protected int GetDeviceIndex(int deviceNo, Config.ConfigDevice.EN_TYPE_DEVICE deviceType)
		{
			int result = -1;
			_configDevice.GetDeviceTargetIndex(GetTaskName()
				, deviceType
				, deviceNo
				, ref result);

			return result;
		}
		protected string AxGetDeviceName(int axisNo)
		{
			string result = "Unknown";
			_configDevice.GetDeviceTargetName(GetTaskName()
				, Config.ConfigDevice.EN_TYPE_DEVICE.MOTION
				, axisNo
				, ref result);

			return result;
		}
		protected string CyGetDeviceName(int cylinderNo)
		{
			string result = "Unknown";
			_configDevice.GetDeviceTargetName(GetTaskName()
				, Config.ConfigDevice.EN_TYPE_DEVICE.CYLINDER
				, cylinderNo
				, ref result);

			return result;
		}
		protected string DoutGetDeviceName(int doutNo)
		{
			string result = "Unknown";
			_configDevice.GetDeviceTargetName(GetTaskName()
				, Config.ConfigDevice.EN_TYPE_DEVICE.DIGITAL_OUTPUT
				, doutNo
				, ref result);

			return result;
		}
		protected string GetDeviceName(int deviceNo, Config.ConfigDevice.EN_TYPE_DEVICE type)
		{
			string result = "Unknown";
			_configDevice.GetDeviceTargetName(GetTaskName()
				, type
				, deviceNo
				, ref result);

			return result;
		}
		private bool AiIsEnable(int ainNo)
		{
			var index = GetDeviceIndex(ainNo, Config.ConfigDevice.EN_TYPE_DEVICE.ANALOG_INPUT);
			if (index < 0)
				return false;

			bool isEnable = false;
			if (false == _configAnalog.GetParameter(true, index, Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.ENABLE, ref isEnable))
				return false;

			return isEnable;
		}

		private ASubSequence GetSubSequenceInstance(Enum subsequence)
		{
			if (false == _subSequenceList.ContainsKey(subsequence)) return null;
			return _subSequenceList[subsequence];
		}
		#endregion /method

		#region enum
		protected enum EN_TASK_COMMON_ALARM
		{
			MOTION_ERROR = 91,		// token
			SUB_SEQUENCE_FAIL = 92,	// token
			WRONG_PORT_STATUS = 93,	// token
			CYLINDER_ERROR = 94,	// token
			DIGITAL_ERROR	= 95,	// token
			SCENARIO_ERROR = 96,	// token

			ABNORMAL_ALARM = 99,    // token
		}
		protected enum EN_CHECK_MANUAL_ACTION_RESULT
		{
			Accord,     // 조건 충족
			Disaccord,
			NotSetupMode,   // setup mode 아님
			HaveNotTargetTask,  // 해당 task의 operation이 아님.
			AbnormalError,
		}
		#endregion /enum
	}
}
