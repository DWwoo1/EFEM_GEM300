using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AnalogIO_;

using XMC_SDK = XMC_SDK_.AnalogIO;

namespace FrameOfSystem3.Controller.AnalogIO
{
    public class XMC_AnalogIO : AnalogIOController
    {
        public XMC_AnalogIO()
        {

        }

        public override bool InitController()
        {
            return XMC_SDK.XMCAnalogIO.GetInstance().InitController();
        }
        public override void ExitController()
        {
            XMC_SDK.XMCAnalogIO.GetInstance().ExitController();
            return;
        }

        public override int GetCountOfInputModule()
        {
            return XMC_SDK.XMCAnalogIO.GetInstance().GetCountOfInputModule();
        }

        public override int GetCountOfInputChannel(ref int nInputModule)
        {
            return XMC_SDK.XMCAnalogIO.GetInstance().GetCountOfInputChannel(nInputModule);
        }

        public override int GetCountOfOutputModule()
        {
            return XMC_SDK.XMCAnalogIO.GetInstance().GetCountOfOutputModule();
        }

        public override int GetCountOfOutputChannel(ref int nOutputMoudle)
        {
            return XMC_SDK.XMCAnalogIO.GetInstance().GetCountOfOutputChannel(nOutputMoudle);
        }

        public override void ReadInputAll(ref int nInputModule, ref int nCountOfChannel, ref int[] arCount)
        {
            // [CHECKS] 2023.10.23. by shkim. 읽히는 것은 확인했으나, 많이 느리다...
            // Framwork Monitoring 기준 164 ms...
            XMC_SDK.XMCAnalogIO.GetInstance().ReadInputAll(nInputModule, nCountOfChannel, ref arCount);

            return;
        }

        public override void ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel, ref int[] arCount)
        {
            XMC_SDK.XMCAnalogIO.GetInstance().ReadOutputAll(nOutputModule, nCountOfChannel, ref arCount);

            return;
        }

        public override void WriteOutput(ref int nOutputChannel, ref int nCount)
        {
            XMC_SDK.XMCAnalogIO.GetInstance().WriteOutput(nOutputChannel, nCount);
            return;
        }

        public override void GetOutputListTable(ref int nOutputChannel, ref int nLoopCount, ref int nPatternSize, ref int[] arPattern)
        {
            return;
        }

        public override void GetOutputListTableInterval(ref int nOutputChannel, ref double dblInterval)
        {
            return;
        }

        public override void GetOutputListTableStatus(ref int nOutputChannel, ref int nPatternIndex, ref int nCountOfLoop, ref uint uInBusy)
        {
            return;
        }

        public override void ResetOutputListTable(ref int nOutputChannel)
        {
            return;
        }

        public override void SetOutputListTable(ref int nOutputChannel, ref int nCountOfLoop, ref int nSizeOfPattern, ref int[] arPattern)
        {
            return;
        }

        public override void SetOutputListTableInterval(ref int nOutputChannel, ref double dInterval)
        {
            return;
        }

        public override void StartOutputListTable(ref int[] arChannel, ref int nSize)
        {
            return;
        }

        public override void StopOutputListTable(ref int[] arChannel, ref int nSize)
        {
            return;
        }
    }
}
