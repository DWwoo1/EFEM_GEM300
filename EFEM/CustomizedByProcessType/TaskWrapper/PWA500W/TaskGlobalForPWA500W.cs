using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM;
using Define.DefineEnumProject.Task.Global;
using Define.DefineEnumProject.DigitalIO.PWA500W;
using FrameOfSystem3.SECSGEM.Scenario;

namespace FrameOfSystem3.Task
{
    class TaskGlobalForPWA500W : TaskGlobal
    {
        #region <Constructors>
        public TaskGlobalForPWA500W(int nIndexOfTask, string strTaskName)
            : base(nIndexOfTask, strTaskName)
        {
            _functionsForPWA500 = FunctionsForPWA500W_NRD.Instance;
        }
        #endregion </Constructors>

        #region <Fields>
        private FunctionsForPWA500W_NRD _functionsForPWA500 = null;
        private bool _hasCommunicationAlarmWithPM = false;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        #region <Overrides>
        protected override void ExecuteOnAlways()
        {
            if (_functionsForPWA500.HasScenarioError)
            {
                _functionsForPWA500.HasScenarioError = false;
                string[] arAlarmSubInfo = { GetTaskName(), _functionsForPWA500.FailedScenarioTypes.ToString() };
                GenerateSequenceAlarm((int)ALARM_GLOBAL.WrongScenarioCirculation, false, ref arAlarmSubInfo);
            }

            // 작업 도중 공정설비 통신 안 될 시 에러를 발생시킨다.
            if (EquipmentState_.EquipmentState.GetInstance().GetState() == EquipmentState_.EQUIPMENT_STATE.EXECUTING)
            {
                if (false == TaskOperator.GetInstance().IsProcessModuleConnected())
                {
                    if (false == _hasCommunicationAlarmWithPM)
                    {
                        _hasCommunicationAlarmWithPM = true;
                        GenerateAlarm((int)ALARM_GLOBAL.COMMUNICATION_ERROR_WITH_PM);
                    }
                }
                else
                {
                    _hasCommunicationAlarmWithPM = false;
                }
            }

        }

        protected override void GetDoorLockSensorSignalList(out List<int> indexOfSignals)
        {
            indexOfSignals = new List<int>
            {
                (int)EN_DIGITAL_IN.EFEM_DOOR_CLOSE
            };
        }
        protected override void GetFanAlarmSensorSignalList(out List<int> indexOfSignals)
        {
            indexOfSignals = new List<int>
            {
                (int)EN_DIGITAL_IN.EFEM_POWER_BOX_FAN_STATUS,
                (int)EN_DIGITAL_IN.EFEM_IO_BOX_FAN_STATUS,
                (int)EN_DIGITAL_IN.ROBOT_CONTROLLER_FAN_ALARM
            };
        }
        protected override void GetFFUAlarmSignalList(out List<int> indexOfSignals)
        {
            indexOfSignals = new List<int>
            {
                (int)EN_DIGITAL_IN.FFU_ALARM,
            };
        }
        protected override void GetIonizerAlarmSignalList(out List<int> indexOfSignals)
        {
            indexOfSignals = new List<int>
            {
                (int)EN_DIGITAL_IN.IONIZER_1_ALARM_STATUS,
                (int)EN_DIGITAL_IN.IONIZER_2_ALARM_STATUS,
                (int)EN_DIGITAL_IN.IONIZER_3_ALARM_STATUS,
            };
        }
        protected override void GetAirAlarmSignalList(out List<int> indexOfSignals)
        {
            indexOfSignals = new List<int>
            {
                (int)EN_DIGITAL_IN.EFEM_MAIN_CDA_PRESSURE_SWITCH,
                (int)EN_DIGITAL_IN.EFEM_MAIN_VACUUM_PRESSURE_SWITCH,
                (int)EN_DIGITAL_IN.ROBOT_CDA_PRESSURE_SWITCH,
                (int)EN_DIGITAL_IN.IONIZER_CDA_PRESSURE_SWITCH,
                (int)EN_DIGITAL_IN.IONIZER_1_FLOW_METER,
                (int)EN_DIGITAL_IN.IONIZER_2_FLOW_METER,
                (int)EN_DIGITAL_IN.IONIZER_3_FLOW_METER,
            };
        }
        #endregion </Overrides>

        #endregion </Methods>
    }
}
