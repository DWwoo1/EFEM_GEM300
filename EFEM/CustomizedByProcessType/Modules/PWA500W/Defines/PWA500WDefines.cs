using System;
using System.Collections.Concurrent;

namespace EFEM.CustomizedByProcessType.PWA500W
{
    #region <Constants>
    public static class Constants
    {
        public static readonly string LoadingToken = "Input";
        public static readonly string UnloadingToken = "Output";

        public const string ProcessModuleCore_8_InputName = "PM1.Core_8_Input";
        public const string ProcessModuleCore_8_OutputName = "PM1.Core_8_Output";
        public const string ProcessModuleCore_12_InputName = "PM1.Core_12_Input";
        public const string ProcessModuleCore_12_OutputName = "PM1.Core_12_Output";
        public const string ProcessModuleSort_12_InputName = "PM1.Sort_12_Input";
        public const string ProcessModuleSort_12_OutputName = "PM1.Sort_12_Output";

        public static readonly string Core_8_Name = "Core_8";
        public static readonly string Core_12_Name = "Core_12";
        public static readonly string Sort_12_Name = "Sort_12";

        public const string EmptyWaferChangeReason = "FINISH_CHANGE";
        public const string EmptyWaferMaterialType = "TM_TAPE";
    }
    #endregion </Constants>

    #region <Enumerations>
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
        Core_8_In,
        Core_8_Out,
        Core_12_In,
        Core_12_Out,
        Sort_12_In,
        Sort_12_Out,
    }
    public enum ProcessModuleEntryWays
    {
        Core_8_In = 0,
        Core_8_Out,
        Core_12_In,
        Core_12_Out,
        Sort_12_In,
        Sort_12_Out,
        //Core_8_In = 0,
        //Core_8_Out,
        //Core_12_In,
        //Core_12_Out,
        //Sort_12_In,
        //Sort_12_Out,
    }
    public enum LoadPortType
    {
        Sort_12 = 0,
        Core_12,
        Core_8_1,
        Core_8_2,
    }
    #endregion </Enumerations>
}
