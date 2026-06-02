using System;
using System.Collections.Concurrent;

namespace EFEM.CustomizedByProcessType.PWA500BIN
{
    #region <Constants>
    public static class Constants
    {
        public static readonly string LoadingToken = "Input";
        public static readonly string UnloadingToken = "Output";

        public const string ProcessModuleCoreInputName = "PM1.CoreInput";
        public const string ProcessModuleSortInputName = "PM1.SortInput";
        public const string ProcessModuleCoreOutputName = "PM1.CoreOutput";
        public const string ProcessModuleSortOutputName = "PM1.SortOutput";

        public const string CoreName = "Core";
        public const string SortName = "Sort";

        public const string EmptyWaferChangeReason = "FINISH_CHANGE";
        public const string EmptyWaferMaterialType = "TM_TAPE";
    }
    #endregion </Constants>

    #region <Enumerations>
    public enum SubstrateTypeForUI
    {
        Core,
        Empty,
        StageCenter,
        StageLeft,
        StageRight,
    }
    public enum SubstrateTypeForControl
    {
        Core,
        EmptyTape,
        Bin,
        //All
    }
    public enum WCFServiceIndex
    {
        //SecsGem,
        EFEM,
        //CoreIn,
        //SortIn,
        //CoreOut,
        //SortOut,
    }
    public enum WCFClientIndex
    {
        //Main,
        SortIn,
        CoreIn,
        CoreOut,
        SortOut,
    }
    public enum ProcessModuleEntryWays
    {
        CoreIn = 0,
        SortIn,
        CoreOut,
        SortOut,
    }
    public enum LoadPortType
    {
        Bin_3 = 0,
        Bin_2,
        Bin_1,
        EmptyTape,
        Core_2,
        Core_1,
    }
    #endregion </Enumerations>
}
