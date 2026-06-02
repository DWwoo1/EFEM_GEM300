using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Define.DefineEnumProject.SelectionList;
using Cylinder_;

namespace FrameOfSystem3.Functional
{
	extern alias AlarmInstance;

	/// <summary>
	/// 2020.06.05 by twkang [ADD] Form_SelectionList 에서 사용하는 배열과 열거형을 위한 클래스이다.
	/// </summary>
    public partial class SelectionList
	{		
		#region 싱글톤
		private static SelectionList _Instance	= new SelectionList();
		public static SelectionList GetInstance()
		{
			return _Instance;			
		}

		private SelectionList()
		{			
			MakeListByEnum();
			MakeListByProjectEnum();
		}
		#endregion

		#region 변수
		Dictionary<Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST, string[]> m_DicOfList = new Dictionary<Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST, string[]>();
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.05 by twkang [ADD] 열거형으로 리스트를 만든다.
		/// </summary>
		private void MakeListByEnum()
		{
			List<string> listForSelectionList	= new List<string>();

			#region OPERATION_EQUIPMENT
			foreach (RunningMain_.RUN_MODE en in Enum.GetValues(typeof(RunningMain_.RUN_MODE)))
			{
				switch(en)
				{
					case RunningMain_.RUN_MODE.AUTO:
					case RunningMain_.RUN_MODE.DRY_RUN:
					case RunningMain_.RUN_MODE.SIMULATION:
						listForSelectionList.Add(en.ToString());
						break;
					default:
						continue;
				}
			}
			m_DicOfList.Add(EN_SELECTIONLIST.OPERATION_EQUIPMENT, listForSelectionList.ToArray());
			#endregion
			
			#region ENABLE & DISABLE
			listForSelectionList	= new List<string>();

			listForSelectionList.Add(Define.DefineConstant.SelectionList.ENABLE);
			listForSelectionList.Add(Define.DefineConstant.SelectionList.DISABLE);

			m_DicOfList.Add(EN_SELECTIONLIST.ENABLE_DISABLE, listForSelectionList.ToArray());
			#endregion

			#region TRUE & FALSE
			listForSelectionList	= new List<string>();

			listForSelectionList.Add(Define.DefineConstant.SelectionList.TRUE);
			listForSelectionList.Add(Define.DefineConstant.SelectionList.FALSE);

			m_DicOfList.Add(EN_SELECTIONLIST.TRUE_FALSE, listForSelectionList.ToArray());
			#endregion

			m_DicOfList.Add(EN_SELECTIONLIST.CYLINDER_MONITORING_MODE, Enum.GetNames(typeof(Cylinder_.MONITORING_MODE)));
			m_DicOfList.Add(EN_SELECTIONLIST.SOCKET_PROTOCOL_TYPE, Enum.GetNames(typeof(Socket_.PROTOCOL_TYPE)));
			m_DicOfList.Add(EN_SELECTIONLIST.SOCKET_LOG_TYPE, Enum.GetNames(typeof(Socket_.LOG_TYPE)));
			m_DicOfList.Add(EN_SELECTIONLIST.SERIAL_DATA_BIT, Enum.GetNames(typeof(Serial_.SERIAL_BYTESIZE)));
			m_DicOfList.Add(EN_SELECTIONLIST.SERIAL_BAUDRATE, Enum.GetNames(typeof(Serial_.SERIAL_BAUDRATE)));
			m_DicOfList.Add(EN_SELECTIONLIST.SERIAL_STOPBIT, Enum.GetNames(typeof(Serial_.SERIAL_STOPBIT)));
			m_DicOfList.Add(EN_SELECTIONLIST.SERIAL_PARITY, Enum.GetNames(typeof(Serial_.SERIAL_PARITY)));
			m_DicOfList.Add(EN_SELECTIONLIST.SERIAL_LOG_TYPE, Enum.GetNames(typeof(Serial_.LOG_TYPE)));
			m_DicOfList.Add(EN_SELECTIONLIST.INTERRUPT_ACTION, Enum.GetNames(typeof(Interrupt_.INTERRUPT_ACTION)));
			
			#region ALARM_STATE
			listForSelectionList	= new List<string>();
			
			foreach(AlarmInstance::Alarm_.ALARM_STATE en in Enum.GetValues(typeof(AlarmInstance::Alarm_.ALARM_STATE)))
			{
				switch (en)
				{
					case AlarmInstance::Alarm_.ALARM_STATE.ERROR:
					case AlarmInstance::Alarm_.ALARM_STATE.NORMAL:
					case AlarmInstance::Alarm_.ALARM_STATE.NOTICE:
					case AlarmInstance::Alarm_.ALARM_STATE.WARNING:
						listForSelectionList.Add(en.ToString());
						break;
				}
			}
			m_DicOfList.Add(Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ALARM_STATE, listForSelectionList.ToArray());
			#endregion

			m_DicOfList.Add(EN_SELECTIONLIST.EQUIPMENT_STATE, Enum.GetNames(typeof(EquipmentState_.EQUIPMENT_STATE)));
			m_DicOfList.Add(EN_SELECTIONLIST.ACTION_LOGIC, Enum.GetNames(typeof(TaskAction_.ACTIONLINK_LOGIC)));
			m_DicOfList.Add(EN_SELECTIONLIST.ACTION_COMPARE_STATE, Enum.GetNames(typeof(TaskAction_.ACTIONLINK_COMPARE_STATE)));
			m_DicOfList.Add(EN_SELECTIONLIST.ANALOG_DATA_TYPE, Enum.GetNames(typeof(AnalogIO_.RAW_DATA_TYPE)));
			m_DicOfList.Add(EN_SELECTIONLIST.MOTION_MOTOR_TYPE, Enum.GetNames(typeof(Motion_.MOTOR_TYPE)));
			m_DicOfList.Add(EN_SELECTIONLIST.MOTION_MOTION_TYPE, Enum.GetNames(typeof(Motion_.MOTION_TYPE)));
			m_DicOfList.Add(EN_SELECTIONLIST.MOTION_INMODE, Enum.GetNames(typeof(Motion_.MOTOR_INPUTMODE)));
			m_DicOfList.Add(EN_SELECTIONLIST.MOTION_OUTMODE, Enum.GetNames(typeof(Motion_.MOTOR_OUTPUTMODE)));

			#region MOTION_MOTOR_DIRECTION
			listForSelectionList	= new List<string>();
			listForSelectionList.Add(Define.DefineConstant.Motion.INVERT_ON);
			listForSelectionList.Add(Define.DefineConstant.Motion.INVERT_OFF);
			m_DicOfList.Add(EN_SELECTIONLIST.MOTION_MOTOR_DIRECTION, listForSelectionList.ToArray());
			#endregion

			#region MOTION_LOGIC
			listForSelectionList	= new List<string>();
			listForSelectionList.Add(Define.DefineConstant.Motion.LOGIC_ACTIVE_HIGH);
			listForSelectionList.Add(Define.DefineConstant.Motion.LOGIC_ACTIVE_LOW);
			m_DicOfList.Add(EN_SELECTIONLIST.MOTION_LOGIC, listForSelectionList.ToArray());
			#endregion

			m_DicOfList.Add(EN_SELECTIONLIST.MOTION_HOME_MODE, Enum.GetNames(typeof(Motion_.HOME_MODE)));

			#region MOTION_LIMIT_STOP_MODE
			listForSelectionList	= new List<string>();
			listForSelectionList.Add(Define.DefineConstant.Motion.STOPMODE_EMG);
			listForSelectionList.Add(Define.DefineConstant.Motion.STOPMODE_DECEL);
			m_DicOfList.Add(EN_SELECTIONLIST.MOTION_LIMIT_STOP_MODE, listForSelectionList.ToArray());
			#endregion

			#region DIRECTION
			listForSelectionList	= new List<string>();
			listForSelectionList.Add(Define.DefineConstant.Motion.DIRECTION_POSITIVE);
			listForSelectionList.Add(Define.DefineConstant.Motion.DIRECITON_NEGATIVE);
			m_DicOfList.Add(EN_SELECTIONLIST.MOTION_HOME_DIRECTION, listForSelectionList.ToArray());
			#endregion

			m_DicOfList.Add(EN_SELECTIONLIST.MOTION_SPEED_PATTERN, Enum.GetNames(typeof(Motion_.MOTION_SPEED_PATTERN)));
			m_DicOfList.Add(EN_SELECTIONLIST.USER_AUTHORITY, Enum.GetNames(typeof(Account.CAccount.EN_PARAM_USER_AUTHORITY)));
			m_DicOfList.Add(EN_SELECTIONLIST.DEVICE_TYPE, Enum.GetNames(typeof(RegisteredInstances_.EN_TYPE_DEVICE)));
			m_DicOfList.Add(EN_SELECTIONLIST.VISION_CAMERA_LIST, Enum.GetNames(typeof(Define.DefineEnumProject.Vision.EN_CAMERA_LIST)));
			m_DicOfList.Add(EN_SELECTIONLIST.ALIGN_CAMERA_SCENE, Enum.GetNames(typeof(Define.DefineEnumProject.Vision.EN_VISION_ALGORITHM)));
			m_DicOfList.Add(EN_SELECTIONLIST.TASK_LIST, Enum.GetNames(typeof(Define.DefineEnumProject.Task.EN_TASK_LIST)));
			m_DicOfList.Add(EN_SELECTIONLIST.LANGUAGE, Enum.GetNames(typeof(Define.DefineEnumBase.Common.EN_LANGUAGE)));
			m_DicOfList.Add(EN_SELECTIONLIST.PORT_STATUS, Enum.GetNames(typeof(DynamicLink_.EN_PORT_STATUS)));
			m_DicOfList.Add(EN_SELECTIONLIST.VISION_CALIBRATION_MODE, Enum.GetNames(typeof(Define.DefineEnumProject.Vision.EN_VISION_CALIBRATION_MODE)));
			m_DicOfList.Add(EN_SELECTIONLIST.RECIPE_TYPE, Enum.GetNames(typeof(Recipe.EN_RECIPE_TYPE)));
			m_DicOfList.Add(EN_SELECTIONLIST.INTERLOCK_COMPARE_DEVICE, Enum.GetNames(typeof(Define.DefineEnumBase.Interlock.EN_INTERLOCK_DEVICE)));
			m_DicOfList.Add(EN_SELECTIONLIST.MOTION_COMPARE_DIRECTION, Enum.GetNames(typeof(Define.DefineEnumBase.Interlock.EN_MOTION_COMPARE_DIRECTION)));
			m_DicOfList.Add(EN_SELECTIONLIST.CYLINDER_COMPARE_DIRECTION, Enum.GetNames(typeof(Define.DefineEnumBase.Interlock.EN_CYLINDER_COMPARE_DIRECTION)));
			m_DicOfList.Add(EN_SELECTIONLIST.MOTION_MOVING_DIRECTION, Enum.GetNames(typeof(Define.DefineEnumBase.Interlock.EN_MOTION_MOVING_DIRECTION)));
			m_DicOfList.Add(EN_SELECTIONLIST.MATRIX, Enum.GetNames(typeof(Define.DefineEnumBase.Common.EN_MATRIX)));
			m_DicOfList.Add(EN_SELECTIONLIST.FORWARD_BACKWARD, Enum.GetNames(typeof(Define.DefineEnumBase.Common.EN_FORWARD_BACKWARD)));
		}
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.06.05 by twkang [ADD] 해당 열거형에 맞는 배열을 반환한다.
		/// </summary>
		public bool GetList(Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST enList, ref string[] strArrResult)
		{
			if (false == m_DicOfList.ContainsKey(enList))
			{
				return false;
			}

			strArrResult = m_DicOfList[enList];

			return true;
		}
		#endregion
    }
}
