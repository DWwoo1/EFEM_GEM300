using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3.Controller.AnalogIO
{
    class MeiAnalogIO : AnalogIO_.AnalogIOController
    {
        public override bool InitController()
        {
            return MPISDK_.AnalogIO_.MPIAnalogIO.GetInstance().InitController();
        }
        public override void ExitController()
        {
			try
			{
				MPISDK_.AnalogIO_.MPIAnalogIO.GetInstance().ExitController();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
        }
        public override int GetCountOfInputChannel(ref int nInputModule)
        {
            return MPISDK_.AnalogIO_.MPIAnalogIO.GetInstance().GetCountOfInputChannel(ref nInputModule);
        }
        public override int GetCountOfInputModule()
        {
            return MPISDK_.AnalogIO_.MPIAnalogIO.GetInstance().GetCountOfInputModule();
        }
        public override int GetCountOfOutputChannel(ref int nOutputMoudle)
        {
            return MPISDK_.AnalogIO_.MPIAnalogIO.GetInstance().GetCountOfOutputChannel(ref nOutputMoudle);
        }
        public override int GetCountOfOutputModule()
        {
            return MPISDK_.AnalogIO_.MPIAnalogIO.GetInstance().GetCountOfOutputModule();
        }
        public override void ReadInputAll(ref int nInputModule, ref int nCountOfChannel, ref int[] arCount)
        {
            MPISDK_.AnalogIO_.MPIAnalogIO.GetInstance().ReadInputAll(ref nInputModule, ref nCountOfChannel, ref arCount);
        }
        public override void ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel, ref int[] arCount)
        {
            MPISDK_.AnalogIO_.MPIAnalogIO.GetInstance().ReadOutputAll(ref nOutputModule, ref nCountOfChannel, ref arCount);          
        }
        public override void WriteOutput(ref int nOutputChannel, ref int nCount)
        {
            MPISDK_.AnalogIO_.MPIAnalogIO.GetInstance().WriteOutput(ref nOutputChannel, ref nCount);
        }

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
