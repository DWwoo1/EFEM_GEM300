using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Define.DefineEnumProject.SubSequence;
using Define.DefineEnumProject.SubSequence.Sample;
using TickCounter_;

namespace FrameOfSystem3.SubSequence.SubSample
{
    #region <VALUE CLASS>
    /// <summary>
    /// Sequence 에서 사용하는 Parameter 이며 Client 로 부터 전달 받음
    /// </summary>
	public class SubSeqSampleParam : SubSequenceParam
    {
    }
    #endregion </VALUE CLASS>

    class SubSeqSample : ASubSequence
    {
        #region <FIELD>
		private ISample _control = null;
		private EN_SEQUENCE_BRANCH _branch;

		private SubSeqSampleParam _parameter = null;
        private TickCounter _timeDelay = new TickCounter();
		private TickCounter _timeCheck = new TickCounter();
        #endregion </FIELD>

        #region <CONSTRUCTOR>
		public SubSeqSample(ASubControl control)
        {
			_control = control as ISample;
        }
        #endregion </CONSTRUCTOR>

		#region <PROPERTY>
		public new bool Activate
		{
			get { return base.Activate; }
			set
			{
				SetDelayTime(0);
				base.Activate = value;
			}
		}
		#endregion </PROPERTY>

		#region <OVERRIDE>
		public override void AddParameter<T>(params T[] param)
        {
			_parameter = param[0] as SubSeqSampleParam;
        }

		private enum EN_STEP
		{
			START = 0,
			END = 10000,
		}
        public override EN_SUBSEQUENCE_RESULT SubSequenceProcedure()
        {
            if (Activate.Equals(false))
                return EN_SUBSEQUENCE_RESULT.OK;

			if (false == IsDelayTimeOver(true))
				return EN_SUBSEQUENCE_RESULT.WORKING;

            switch (_stepNo)
            {
				case (int)EN_STEP.START:
					_stepNo = (int)EN_STEP.END;
                    break;
				case (int)EN_STEP.END:
                    Activate = false;
                    return EN_SUBSEQUENCE_RESULT.OK;
            }

            return EN_SUBSEQUENCE_RESULT.WORKING;
        }
        #endregion </OVERRIDE>

		#region <METHOD>

		#region Time
		private void SetDelayTime(uint delay)
		{
			_timeDelay.SetTickCount(delay);
		}
		private bool IsDelayTimeOver(bool defaultValue)
		{
			return _timeDelay.IsTickOver(defaultValue);
		}

		private void SetCheckTime(uint delay)
		{
			_timeCheck.SetTickCount(delay);
		}
		private bool IsCheckTimeOver(bool bDefaultValue)
		{
			return _timeCheck.IsTickOver(bDefaultValue);
		}
		private ulong GetCheckTimeElapsed()
		{
			return _timeCheck.GetTickCount();
		}
		#endregion

		private EN_SUBSEQUENCE_RESULT SubSequenceNG(EN_SUBSEQUENCE_RESULT result)
		{
			Activate = false;
			return result;
		}

		#endregion </METHOD>
	}
}
