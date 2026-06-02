using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3.SubSequence
{
	extern alias MotionInstance;

	// OBJECT ROLE : 재사용 가능한 단위 시퀀스를 위해 방법(컨트롤)을 제공한다. 
    public abstract class ASubControl
    {
        #region <FIELD>

        #region Control Index
        /// <summary>
        /// KEY int : SUB CONTROL 에서 정의된 DEVICE INDEX
        /// VALUE int : TASK 에서 정의된 DEVICE INDEX
        /// </summary>
		protected Dictionary<int, int> _dicOfMotion = new Dictionary<int, int>();
		protected Dictionary<int, int> _dicOfCylinder = new Dictionary<int, int>();
		protected Dictionary<int, int> _dicOfDigitalInput = new Dictionary<int, int>();
		protected Dictionary<int, int> _dicOfDigitalOutput = new Dictionary<int, int>();
		protected Dictionary<int, int> _dicOfAnalogInput = new Dictionary<int, int>();
		protected Dictionary<int, int> _dicOfAnalogOutput = new Dictionary<int, int>();
		protected Dictionary<int, int> _dicOfSocket = new Dictionary<int, int>();
		protected Dictionary<int, int> _dicOfSerial = new Dictionary<int, int>();
        #endregion 

        #region Instance
        protected Task.RunningTaskEx _instanceTask = null;
		protected MotionInstance::Motion_.Motion _instanceMotion = null;
        protected DigitalIO_.DigitalIO _instanceDigital = null;
        protected AnalogIO_.AnalogIO _instanceAnalog = null;
        protected Socket_.Socket _instanceSocket = null;
        protected Serial_.Serial _instanceSerial = null;
        protected Config.ConfigDevice _instanceDevice = null;
        #endregion

        #endregion </FIELD>

        #region <CONSTRUCTOR>
        public ASubControl(Task.RunningTaskEx rRunningTask)
        {
           _instanceTask = rRunningTask;
        }
        #endregion </CONSTRUCTOR>

        #region <INTERFACE>

        #region Target Index & Parameter
        public void SetTargetMotion(int nIndex, int nTargetIndex)
        {
            if (true == _dicOfMotion.ContainsKey(nIndex))
                return;

            _dicOfMotion.Add(nIndex, nTargetIndex);
        }
        public void SetTargetCylinder(int nIndex, int nTargetIndex)
        {
            if (true == _dicOfCylinder.ContainsKey(nIndex))
                return;

            _dicOfCylinder.Add(nIndex, nTargetIndex);
        }
        public void SetTargetDigitalInput(int nIndex, int nTargetIndex)
        {
            if (true == _dicOfDigitalInput.ContainsKey(nIndex))
                return;

            _dicOfDigitalInput.Add(nIndex, nTargetIndex);
        }
        public void SetTargetDigitalOutput(int nIndex, int nTargetIndex)
        {
            if (true == _dicOfDigitalOutput.ContainsKey(nIndex))
                return;

            _dicOfDigitalOutput.Add(nIndex, nTargetIndex);
        }
        public void SetTargetAnalogInput(int nIndex, int nTargetIndex)
        {
            if (true == _dicOfAnalogInput.ContainsKey(nIndex))
                return;

            _dicOfAnalogInput.Add(nIndex, nTargetIndex);
        }
        public void SetTargetAnalogOutput(int nIndex, int nTargetIndex)
        {
            if (true == _dicOfAnalogOutput.ContainsKey(nIndex))
                return;

            _dicOfAnalogOutput.Add(nIndex, nTargetIndex);
        }
        #endregion

        #endregion </INTERFACE>
    }
}
