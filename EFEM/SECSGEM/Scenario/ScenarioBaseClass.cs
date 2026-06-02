using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario
{
    public class ScenarioParamValues
    {
        public ScenarioParamValues(List<string> values = null)
        {
            if (values != null)
            {
                _variableValues = new string[values.Count];

                Array.Copy(values.ToArray(), _variableValues, values.Count);
            }
        }

        public string[] VariableValues
        {
            get
            {
                return _variableValues;
            }
        }
       
        protected string[] _variableValues = null;

        protected Dictionary<string, string> _resultData = null;
        public Dictionary<string, string> GetResultData()
        {
            return _resultData;
        }
        public void SetResultData(Dictionary<string, string> dataToSet)
        {
            _resultData = new Dictionary<string, string>(dataToSet);
        }
    }

    public abstract class ScenarioBaseClass
    {
        #region <Constructor>
        public ScenarioBaseClass(string name, uint timeOut)
        {
            TimeOut = timeOut;

            Name = name;
            Activate = false;
        }
        #endregion </Constructor>

        #region <Fields>
        protected bool _activated = false;
        protected int _seqNum = 0;      // 1000이 종료

        private TickCounter_.TickCounter _tickCounter = new TickCounter_.TickCounter();

        protected Communicator.SecsGemHandler _gemHandler = Communicator.SecsGemHandler.Instance;

        protected readonly uint TimeOut;
        #endregion </Fields>

        #region <Properties>
        public string Name { get; private set; }

        public bool Activate
        {
            get
            {
                return _activated;
            }
            set
            {
                if (false == value)
                {
                    InitFlags();
                }

                _activated = value;
            }
        }
        public bool Usable { get; set; }

        public EN_SCENARIO_PERMISSION_RESULT Permission { get; protected set; }

        public int Step { get { return _seqNum; } }

        public long ReceiveStream { get; protected set; }
        public long ReceiveFunction { get; protected set; }
        public bool Receiving { get; protected set; }
        #endregion </Properties>

        #region <Methods>

        #region Abstract Method
        public abstract EN_SCENARIO_RESULT ExecuteScenario();
        public abstract void UpdateParamValues(ScenarioParamValues paramValues);
        public virtual Dictionary<string, string> GetResultData()
        {
            return null;
        }
        #endregion

        #region Public Method
        public virtual void InitFlags()
        {
            _seqNum = 0;

            Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
        }
        public virtual void InitResultData() { }

        public virtual void InitPermission()
        {
            Permission = EN_SCENARIO_PERMISSION_RESULT.PROCEED;
        }

        public void UpdatePermission(bool permission)
        {
            if (permission)
                Permission = EN_SCENARIO_PERMISSION_RESULT.OK;
            else
                Permission = EN_SCENARIO_PERMISSION_RESULT.ERROR;
        }
        #endregion

        #region Protected Method
        protected EN_SCENARIO_RESULT ReturnScenarioResult(EN_SCENARIO_RESULT result)
        {
            switch (result)
            {
                case EN_SCENARIO_RESULT.WAITING:
                case EN_SCENARIO_RESULT.PROCEED:
                    break;

                case EN_SCENARIO_RESULT.COMPLETED:
                case EN_SCENARIO_RESULT.ERROR:
                case EN_SCENARIO_RESULT.TIMEOUT_ERROR:
                    Activate = false;
                    break;
            }

            return result;
        }
        protected bool ControlStateIsRemote()
        {
            return (_gemHandler.GetControlState().Equals(EN_CONTROL_STATE.REMOTE));
        }
        protected void SetTickCount(uint time)
        {
            _tickCounter.SetTickCount(time);
        }
        protected bool IsTickOver(bool defaultValue)
        {
            return _tickCounter.IsTickOver(defaultValue);
        }
        protected void WriteLog(string messageToWrite)
        {
            _gemHandler.WriteLog(messageToWrite);
        }
        #endregion

        #region Virtual Method
        public virtual bool UpdateReceiveMessage(List<SemiObject> listOfReceive)
        {
            Receiving = false;
            return true;
        }
        public virtual bool UpdateReceiveMessage(string rcmdName, string[] cpNames, string[] cpValues)
        {
            Receiving = false;
            return true;
        }
        #endregion

        #endregion </Methods>
    }

    public abstract class AutoScenarioBase : ScenarioBaseClass
    {
        protected AutoScenarioBase(string name, uint timeOut)
            : base(name, timeOut)
        {
            AutoState = EN_AUTO_SCENARIO_STATE.IDLE;
        }

        public EN_AUTO_SCENARIO_STATE AutoState { get; private set; }

        public virtual void SetAutoState(EN_AUTO_SCENARIO_STATE state)
        {
            AutoState = state;
        }

        public override void InitFlags()
        {
            base.InitFlags();
            AutoState = EN_AUTO_SCENARIO_STATE.IDLE;
        }
    }
}
