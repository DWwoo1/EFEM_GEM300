using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AnalogIO_;
using RSASDK_.AnalogIO_;

namespace FrameOfSystem3.Controller.AnalogIO
{
    /// <summary>
    /// 2020.11.09 by yjlee [ADD] Abstract class for the analog IO using RS Automation SDK.
    /// </summary>
    class RSAAnalogIOController : AnalogIOController
    {
        #region Controller
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Initialize the controller.
        /// </summary>
        public override bool InitController()
        {
            return RSAAnalogIO.GetInstance().InitController();
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Exit the controller.
        /// </summary>
        public override void ExitController()
        {
            RSAAnalogIO.GetInstance().ExitController();
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the count of the input module.
        /// </summary>
        public override int GetCountOfInputModule()
        {
            return RSAAnalogIO.GetInstance().GetCountOfInputModule();
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the count of the output module.
        /// </summary>
        public override int GetCountOfOutputModule()
        {
            return RSAAnalogIO.GetInstance().GetCountOfOutputModule();
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the total count of the input channel.
        /// </summary>
        public override int GetCountOfInputChannel(ref int nInputModule)
        {
             return RSAAnalogIO.GetInstance().GetCountOfInputChannel(ref nInputModule);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the total count of the output channel.
        /// </summary>
        public override int GetCountOfOutputChannel(ref int nOutputMoudle)
        {
            return RSAAnalogIO.GetInstance().GetCountOfOutputChannel(ref nOutputMoudle);
        }
        #endregion

        #region Analog IO Interface
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Read the counts of the all input channel.
        /// </summary>
        public override void ReadInputAll(ref int nInputModule, ref int nCountOfChannel, ref int[] arCount)
        {
            RSAAnalogIO.GetInstance().ReadInputAll(ref nInputModule, ref nCountOfChannel, ref arCount);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Read the counts of the all output channel.
        /// </summary>
        public override void ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel, ref int[] arCount)
        {
            RSAAnalogIO.GetInstance().ReadOutputAll(ref nOutputModule, ref nCountOfChannel, ref arCount);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Write the count to the channel.
        /// </summary>
        public override void WriteOutput(ref int nOutputChannel, ref int nCount)
        {
            RSAAnalogIO.GetInstance().WriteOutput(ref nOutputChannel, ref nCount);
        }
        #endregion

        #region Analog IO table
        /// <summary>
        /// 2021.02.24 by yjlee [ADD] Set the output list table for the pattern.
        /// </summary>
        public override void SetOutputListTable(ref int nOutputChannel, ref int nCountOfLoop, ref int nSizeOfPattern, ref int[] arPattern)
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
