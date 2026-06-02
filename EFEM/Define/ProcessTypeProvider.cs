using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.Work;
using Define.DefineEnumProject.AppConfig;

namespace EFEM.Defines.ProcessTypeProvider
{
    public interface IProcessTypeProvider
    {
        EN_PROCESS_TYPE GetProcessType();
    }

    public sealed class AppConfigProcessTypeProvider : IProcessTypeProvider
    {
        public EN_PROCESS_TYPE GetProcessType()
            => AppConfigManager.Instance.ProcessType;
    }
}