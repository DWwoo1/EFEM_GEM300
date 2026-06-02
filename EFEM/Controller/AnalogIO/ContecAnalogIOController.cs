using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AnalogIO_;
using FrameOfSystem3.Controller.AnalogIO.Contec;

namespace FrameOfSystem3.Controller.AnalogIO
{
    /// <summary>
    /// 2021.06.30 by Thienvv [ADD] Abstract class for the analog IO using Contec Analog Controller
	/// 
	/// Device Name : AIO000
	/// Device : AIO-160802AY-USB
    /// </summary>
    class ContecAnalogIOController : AnalogIOController
    {
        #region <Fields>
        private ContecAnalog mAnalog = ContecAnalog.GetInstance();
		object _lock = new object();
        #endregion </Fields>

        #region Controller
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Initialize the controller.
        /// </summary>
        public override bool InitController()
        {
            string strResult;
            return mAnalog.InitController(out strResult);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Exit the controller.
        /// </summary>
        public override void ExitController()
        {
            mAnalog.CloseController();
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the count of the input module.
        /// </summary>
        public override int GetCountOfInputModule()
        {
            return mAnalog.InModuleCount;
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the count of the output module.
        /// </summary>
        public override int GetCountOfOutputModule()
        {
            return mAnalog.OutModuleCount;
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the total count of the input channel.
        /// </summary>
        public override int GetCountOfInputChannel(ref int nInputModule)
        {
            return mAnalog.InChannelCount;
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the total count of the output channel.
        /// </summary>
        public override int GetCountOfOutputChannel(ref int nOutputMoudle)
        {
            return mAnalog.OutChannelCount;
        }
        #endregion

        #region Analog IO Interface
		// 2022.07.28 by junho [MOD] (double)Volt -> (int)Count
		public override void ReadInputAll(ref int nInputModule, ref int nCountOfChannel, ref int[] arCount)
        {
			lock (_lock)
			{
				mAnalog.ReadAllInput(nInputModule, nCountOfChannel, out arCount);
			}
        }
		// 2022.07.28 by junho [MOD] (double)Volt -> (int)Count
		public override void ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel, ref int[] arCount)
        {
			mAnalog.ReadAllOutput(nOutputModule, nCountOfChannel, out arCount);
		}
		// 2022.07.28 by junho [MOD] (double)Volt -> (int)Count
		public override void WriteOutput(ref int nOutputChannel, ref int nCount)
        {
			lock (_lock)
			{
				mAnalog.WriteOutput(nOutputChannel, nCount);
			}
        }

		#region 사용안함? by junho
		// 2022.07.28 by junho [MOD] (double)Volt -> (int)Count
		public void WriteWaveFormOutputStartVoltage(ref int nOutputChannel, ref bool bSquareWaveMode, ref int dblLowCount, ref int dblHighCount, ref double dblFrequency)
		{
			mAnalog.WriteWaveFormOutputStartVoltage(nOutputChannel, bSquareWaveMode ? EN_ANALOG_WAVE_FORM.SQUARE_WAVE : EN_ANALOG_WAVE_FORM.SINE_WAVE, dblLowCount, dblHighCount, dblFrequency);
		}

		public void WriteWaveFormOutputStop(ref int nOutputChannel, ref bool bImmStop, ref double dblVolt)
		{
			mAnalog.WriteWaveFormOutputStop(nOutputChannel, bImmStop, ref dblVolt);
		}

		public bool IsWaveFormOutput(ref int nOutputChannel)
		{
			return mAnalog.IsWaveFormOutput(nOutputChannel);
		}

		public bool IsWaveFormOutputCheck(ref int nOutputChannel)
		{
			return mAnalog.IsWaveFormOutputCheck(nOutputChannel);
		}

		public bool IsSameWaveFormOutput(ref int nOutputChannel, ref bool bSquareWaveMode, ref double dblLowVolt, ref double dblHighVolt, ref double dblFrequency)
		{
			return mAnalog.IsSameWaveFormOutput(nOutputChannel, bSquareWaveMode ? EN_ANALOG_WAVE_FORM.SQUARE_WAVE : EN_ANALOG_WAVE_FORM.SINE_WAVE, dblLowVolt, dblHighVolt, dblFrequency);
		}
		#endregion

		public override void SetOutputListTable(ref int nOutputChannel, ref int nCountOfLoop, ref int nSizeOfPattern, ref int[] arPattern)
		{
			
		}

		#endregion

		#region Analog IO table - Not Use
		/// <summary>
        /// 2021.02.24 by yjlee [ADD] Set the output list table for the pattern.
        /// </summary>
        public void SetOutputListTable(ref int nOutputChannel, ref int nCountOfLoop, ref int nSizeOfPattern, ref double[] arPattern)
        {
        }
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Set the interval of the pattern.
        /// </summary>
        public override void SetOutputListTableInterval(ref int nOutputChannel, ref double dInterval)
        {
        }
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Get the output list table about the pattern.
        /// </summary>
        public override void GetOutputListTable(ref int nOutputChannel, ref int nLoopCount, ref int nPatternSize, ref int[] arPattern)
        {
        }
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Get the interval of the pattern.
        /// </summary>
        public override void GetOutputListTableInterval(ref int nOutputChannel, ref double dblInterval)
        {
        }
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Get the status of the pattern.
        /// </summary>
        public override void GetOutputListTableStatus(ref int nOutputChannel, ref int nPatternIndex, ref int nCountOfLoop, ref uint uInBusy)
        {
        }
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Start to write the output by the pattern.
        /// </summary>
        public override void StartOutputListTable(ref int[] arChannel, ref int nSize)
        {
        }
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Stop to write the output by the pattern.
        /// </summary>
        public override void StopOutputListTable(ref int[] arChannel, ref int nSize)
        {
        }
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Reset(clear) the pattern.
        /// </summary>
        public override void ResetOutputListTable(ref int nOutputChannel)
        {
        }
        #endregion End - Analog IO
    }
}
