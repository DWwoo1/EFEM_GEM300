using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3.Controller.DigitalIO
{
    class MeiDigitalIO : DigitalIO_.DigitalIOController
    {
        public override bool InitController()
        {
            return MPISDK_.DigitalIO_.MPIDigitalIO.GetInstance().InitController();
        }

        public override void ExitController()
        {
			try
			{
				MPISDK_.DigitalIO_.MPIDigitalIO.GetInstance().ExitController();
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
			}
        }

        public override int GetCountOfInputChannel(ref int nInputModule)
        {
            return MPISDK_.DigitalIO_.MPIDigitalIO.GetInstance().GetCountOfInputChannel(ref nInputModule);
        }

        public override int GetCountOfInputModule()
        {
            return MPISDK_.DigitalIO_.MPIDigitalIO.GetInstance().GetCountOfInputModule();
        }

        public override int GetCountOfOutputChannel(ref int nOutputModule)
        {
            return MPISDK_.DigitalIO_.MPIDigitalIO.GetInstance().GetCountOfOutputChannel(ref nOutputModule);
        }

        public override int GetCountOfOutputModule()
        {
            return MPISDK_.DigitalIO_.MPIDigitalIO.GetInstance().GetCountOfOutputModule();
        }

        public override uint ReadInputAll(ref int nInputModule, ref int nCountOfChannel)
        {
            return MPISDK_.DigitalIO_.MPIDigitalIO.GetInstance().ReadInputAll(ref nInputModule);
        }

        public override uint ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel)
        {
            return MPISDK_.DigitalIO_.MPIDigitalIO.GetInstance().ReadOutputAll(ref nOutputModule);
        }

        public override void WriteOutput(ref int nOutputChannel, ref bool bPulse)
        {
            MPISDK_.DigitalIO_.MPIDigitalIO.GetInstance().WriteOutput(ref nOutputChannel, ref bPulse);
        }
    }
}
