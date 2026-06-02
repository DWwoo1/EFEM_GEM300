using EFEM.Defines.LoadPort;

namespace EFEM.Modules.LoadPort.State
{
    public enum LoadPortStateModelType
    {
        Legacy = 0,
        E87 = 1
    }

    public static class LoadPortStateModelFactory
    {
        public static ILoadPortStateModel Create(
            LoadPortStateModelType modelType,
            int portId,
            VerificationTransitionOptions options = null)
        {
            switch (modelType)
            {
                case LoadPortStateModelType.E87:
                    return new E87LoadPortStateModel(portId, options);

                case LoadPortStateModelType.Legacy:
                default:
                    return new LegacyLoadPortStateModel(portId, options);
            }
        }
    }
}