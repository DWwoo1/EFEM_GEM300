using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SubSequence.SubSample.Sample;
using Define.DefineEnumProject.SubSequence.Sample;

namespace FrameOfSystem3.SubSequence.SubSample
{
	namespace Sample
	{
		public enum EN_AXIS
		{
		}
		public enum EN_CYLINDER
		{
		}
		public enum EN_DIGITAL_IN
		{
		}
		public enum EN_DIGITAL_OUT
		{
		}
		public enum EN_ANALOG_IN
		{
		}
		public enum EN_ANALOG_OUT
		{
		}
	}

	class SubCtrlSample : ASubControl, ISample
    {
        #region <CONSTRUCTOR>
		public SubCtrlSample(Task.RunningTaskEx tRunningTask)
            : base(tRunningTask)
        {
			_taskOperator = Task.TaskOperator.GetInstance();
        }
        #endregion </CONSTRUCTOR>

		#region <FILED>
		private Task.TaskOperator _taskOperator = null;
		#endregion </FIELD>

		#region <INTERFACE>

		#endregion </INTERFACE>
	}
}
