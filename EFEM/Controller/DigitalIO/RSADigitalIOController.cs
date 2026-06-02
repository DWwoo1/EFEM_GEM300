using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DigitalIO_;
using RSASDK_.DigitalIO_;

namespace FrameOfSystem3.Controller.DigitalIO
{
    /// <summary>
    /// 2020.11.09 by yjlee [ADD] Abstract class for the digital IO using RS Automation SDK.
    /// </summary>
    class RSADigitalIOController : DigitalIOController
    {
        #region Controller
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Initialize the controller.
        /// </summary>
        public override bool InitController()
        {
            return RSADigitalIO.GetInstance().InitController();
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Exit the controller.
        /// </summary>
        public override void ExitController()
        {
            RSADigitalIO.GetInstance().ExitController();
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the count of the input module.
        /// </summary>
        public override int GetCountOfInputModule()
        {
            return RSADigitalIO.GetInstance().GetCountOfInputModule();
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the count of the output module.
        /// </summary>
        public override int GetCountOfOutputModule()
        {
            return RSADigitalIO.GetInstance().GetCountOfOutputModule();
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the total count of the input channel.
        /// </summary>
        public override int GetCountOfInputChannel(ref int nInputModule)
        {
             return RSADigitalIO.GetInstance().GetCountOfInputChannel(ref nInputModule);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the total count of the output channel.
        /// </summary>
        public override int GetCountOfOutputChannel(ref int nOutputMoudle)
        {
            return RSADigitalIO.GetInstance().GetCountOfOutputChannel(ref nOutputMoudle);
        }
        #endregion

        #region Digital IO Interface
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Read the input data from the controller.
        /// </summary>
        public override uint ReadInputAll(ref int nInputModule, ref int nCountOfChannel)
        {
            return RSADigitalIO.GetInstance().ReadInputAll(ref nInputModule, ref nCountOfChannel);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Read the output data from the controller.
        /// </summary>
        public override uint ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel)
        {
            return RSADigitalIO.GetInstance().ReadOutputAll(ref nOutputModule, ref nCountOfChannel);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Write the data to the controller.
        /// </summary>
        public override void WriteOutput(ref int nOutputChannel, ref bool bPulse)
        {
            RSADigitalIO.GetInstance().WriteOutput(ref nOutputChannel, ref bPulse);
        }
        #endregion
    }
}
