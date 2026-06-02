using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AnalogIO_;
using AJINSDK_.AnalogIO_;

namespace FrameOfSystem3.Controller.AnalogIO
{
    class AJINAnalogIOController : AnalogIOController
    {
        #region Variables
        private AJINAnalogIO m_instance     = null;
        #endregion End - Variables

        #region Controller
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Initialize the controller.
        /// </summary>
        public override bool InitController()
        {
            m_instance      = AJINAnalogIO.GetInstance();

            return m_instance.InitController();
        }

        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Exit the controller.
        /// </summary>
        public override void ExitController()
        {
            m_instance.ExitController();
        }

        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Get the count of the input module.
        /// </summary>
        public override int GetCountOfInputModule()
        {
            return m_instance.GetCountOfModule();
        }

        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Get the count of the output module.
        /// </summary>
        public override int GetCountOfOutputModule()
        {
            return m_instance.GetCountOfModule();
        }

        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Get the total count of the input channel.
        /// </summary>
        public override int GetCountOfInputChannel(ref int nInputModule)
        {
             return m_instance.GetCountOfInputChannel(ref nInputModule);
        }

        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Get the total count of the output channel.
        /// </summary>
        public override int GetCountOfOutputChannel(ref int nOutputMoudle)
        {
            return m_instance.GetCountOfOutputChannel(ref nOutputMoudle);
        }
        #endregion

        #region Analog IO Interface
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Read the counts of the all input channel.
        /// </summary>
        public override void ReadInputAll(ref int nInputModule, ref int nCountOfChannel, ref int[] arCount)
        {
            m_instance.ReadInputAll(ref nInputModule, ref nCountOfChannel, ref arCount);
            if (arCount!= null)
            for(int i = 0; i < arCount.Length; i++)
            {
                if(arCount[i] > 32767)
                {
                    arCount[i] = arCount[i] - 65536;
                }
            }
        }

        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Read the counts of the all output channel.
        /// </summary>
        public override void ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel, ref int[] arCount)
        {
            m_instance.ReadOutputAll(ref nOutputModule, ref nCountOfChannel, ref arCount);
        }

        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Write the count to the channel.
        /// </summary>
        public override void WriteOutput(ref int nOutputChannel, ref int nCount)
        {
            m_instance.WriteOutput(ref nOutputChannel, ref nCount);
        }
        #endregion

        #region Analog IO table
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Set the output list table for the pattern.
        /// </summary>
        public override void SetOutputListTable(ref int nOutputChannel, ref int nCountOfLoop, ref int nSizeOfPattern, ref int[] arPattern)
        {
            m_instance.SetOutputListTable(ref nOutputChannel, ref nCountOfLoop, ref nSizeOfPattern, ref arPattern);
        }
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Set the interval of the pattern.
        /// </summary>
        public override void SetOutputListTableInterval(ref int nOutputChannel, ref double dInterval)
        {
            m_instance.SetOutputListTableInterval(ref nOutputChannel, ref dInterval);
        }
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Get the output list table about the pattern.
        /// </summary>
        public override void GetOutputListTable(ref int nOutputChannel, ref int nLoopCount, ref int nPatternSize, ref int[] arPattern)
        {
            if(arPattern == null)
                arPattern = new int[0];
            m_instance.GetOutputListTable(ref nOutputChannel, ref nLoopCount, ref nPatternSize, ref arPattern);
        }
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Get the interval of the pattern.
        /// </summary>
        public override void GetOutputListTableInterval(ref int nOutputChannel, ref double dblInterval)
        {
            m_instance.GetOutputListTableInterval(ref nOutputChannel, ref dblInterval);
        }
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Get the status of the pattern.
        /// </summary>
        public override void GetOutputListTableStatus(ref int nOutputChannel, ref int nPatternIndex, ref int nCountOfLoop, ref uint uInBusy)
        {
            m_instance.GetOutputListTableStatus(ref nOutputChannel, ref nPatternIndex, ref nCountOfLoop, ref uInBusy);
        }
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Start to write the output by the pattern.
        /// </summary>
        public override void StartOutputListTable(ref int[] arChannel, ref int nSize)
        {
            m_instance.StartOutputListTable(ref arChannel, ref nSize);
        }
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Stop to write the output by the pattern.
        /// </summary>
        public override void StopOutputListTable(ref int[] arChannel, ref int nSize)
        {
            m_instance.StopOutputListTable(ref arChannel, ref nSize);
        }
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Reset(clear) the pattern.
        /// </summary>
        public override void ResetOutputListTable(ref int nOutputChannel)
        {
            m_instance.ResetOutputListTable(ref nOutputChannel);
        }
        #endregion End - Analog IO
    }
}
