using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DigitalIO_;
using AJINSDK_.DigitalIO_;

namespace FrameOfSystem3.Controller.DigitalIO
{
    /// <summary>
    /// 2021.02.18 by yjlee [ADD] It controls the AJIN analog Input / Output Controller.
    /// </summary>
    class AJINDigitalIOController : DigitalIOController
    {
        #region Variables
        AJINDigitalIO m_instance        = null;
        #endregion End - Variables

        #region Controller
        /// <summary>
        /// 2021.02.22 by yjlee [ADD] Initialize the SDK and controller.
        /// </summary>
        public override bool InitController()
        {
            m_instance      = AJINDigitalIO.GetInstance();

            return m_instance.InitController();
        }

        /// <summary>
        /// 2021.02.22 by yjlee [ADD] Release the resources for the SDK.
        /// </summary>
        public override void ExitController()
        {
            if(null != m_instance)
            {
                m_instance.ExitController();

                m_instance      = null;
            }
        }

        /// <summary>
        /// 2021.02.22 by yjlee [ADD] Get the count of the input module.
        /// </summary>
        public override int GetCountOfInputModule()
        {
            return m_instance.GetCountOfModule();
        }

        /// <summary>
        /// 2021.02.22 by yjlee [ADD] Get the count of the output module.
        /// </summary>
        public override int GetCountOfOutputModule()
        {
            return m_instance.GetCountOfModule();
        }

        /// <summary>
        /// 2021.02.22 by yjlee [ADD] Get the count of the input channel of the module.
        /// </summary>
        public override int GetCountOfInputChannel(ref int nInputModule)
        {
            return m_instance.GetCountOfInputChannel(ref nInputModule);
        }

        /// <summary>
        /// 2021.02.22 by yjlee [ADD] Get the count of the output channel of the module.
        /// </summary>
        public override int GetCountOfOutputChannel(ref int nOutputModule)
        {
            return m_instance.GetCountOfOutputChannel(ref nOutputModule);
        }
        #endregion End - Controller

        #region Digital IO Interface
        /// <summary>
        /// 2021.02.22 by yjlee [ADD] Read the datas from the input channels of the module.
        /// </summary>
        public override uint ReadInputAll(ref int nInputModule, ref int nCountOfChannel)
        {
            return m_instance.ReadInputAll(ref nInputModule, ref nCountOfChannel);
        }

        /// <summary>
        /// 2021.02.22 by yjlee [ADD] Read the datas from the input channels of the module.
        /// </summary>
        public override uint ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel)
        {
            return m_instance.ReadOutputAll(ref nOutputModule, ref nCountOfChannel);
        }

        /// <summary>
        /// 2021.02.22 by yjlee [ADD] Write a data to the output channel.
        /// </summary>
        public override void WriteOutput(ref int nOutputChannel, ref bool bPulse)
        {
            m_instance.WriteOutput(ref nOutputChannel, ref bPulse);
        }
        #endregion End - Digital IO Interface
    }

    #region Mashaling
    #endregion End - Mashaling
}
