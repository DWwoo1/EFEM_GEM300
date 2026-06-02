using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DigitalIO_;

using XMC_SDK = XMC_SDK_.DigitalIO;

namespace FrameOfSystem3.Controller.DigitalIO
{
    public class XMC_DigitalIO : DigitalIOController
    {
        public XMC_DigitalIO()
        {

        }

        public override bool InitController()
        {
            return XMC_SDK.XMCDigitalIO.GetInstance().InitController();
        }
        public override void ExitController()
        {
            XMC_SDK.XMCDigitalIO.GetInstance().ExitController();
        }

        public override int GetCountOfInputModule()
        {
            return XMC_SDK.XMCDigitalIO.GetInstance().GetCountOfInputModule();
        }

        public override int GetCountOfInputChannel(ref int nInputModule)
        {
            return XMC_SDK.XMCDigitalIO.GetInstance().GetCountOfInputChannel(nInputModule);
        }

        public override int GetCountOfOutputModule()
        {
            return XMC_SDK.XMCDigitalIO.GetInstance().GetCountOfOutputModule();
        }

        public override int GetCountOfOutputChannel(ref int nOutputMoudle)
        {
            return XMC_SDK.XMCDigitalIO.GetInstance().GetCountOfOutputChannel(nOutputMoudle);
        }

        public override uint ReadInputAll(ref int nInputModule, ref int nCountOfChannel)
        {
            return XMC_SDK.XMCDigitalIO.GetInstance().ReadInputAll(nInputModule, nCountOfChannel);
        }
        public override uint ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel)
        {
            return XMC_SDK.XMCDigitalIO.GetInstance().ReadOutputAll(nOutputModule, nCountOfChannel);
        }
        public override void WriteOutput(ref int nOutputChannel, ref bool bPulse)
        {
            XMC_SDK.XMCDigitalIO.GetInstance().WriteOutput(nOutputChannel, bPulse);
        }
    }
}
