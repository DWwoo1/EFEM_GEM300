using System;

namespace EFEM.Jobs.Binding
{
    /// <summary>
    /// 기존 코드의 Singleton 구조에 맞춘 최소 수정용 Locator.
    /// </summary>
    public static class SubstrateJobBindingService
    {
        private static ISubstrateJobBinder _instance;

        public static ISubstrateJobBinder Instance
        {
            get
            {
                return _instance;
            }
        }

        public static void Configure(ISubstrateJobBinder instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            _instance = instance;
        }
    }
}