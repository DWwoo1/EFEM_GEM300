using System;
using System.Collections.Generic;

using FrameOfSystem3.Recipe;
using FrameOfSystem3.SECSGEM.PortRole;

using EFEM.Jobs.Binding;
using EFEM.Jobs.Completion;
using EFEM.Defines.MaterialTracking;
using EFEM.MaterialTracking;
using EFEM.Modules.LoadPort.Scheduler;
using EFEM.CustomizedByProcessType.PWA500W;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace FrameOfSystem3.SECSGEM
{
    class FunctionsForPWA500W_NRD_300 : CommonFunctionsForPWA500
    {
        #region <Constructors>
        private FunctionsForPWA500W_NRD_300() : base(false, false)
        {
            PortRoleProfile = new SeparatedBinAndEmptyPortProfile(
                Recipe,
                LoadPortManager,
                CarrierServer,
                SubstrateManager);
            //PortRoleProfile = new SharedBinAndEmptyPortProfile(
            //    Recipe,
            //    LoadPortManager,
            //    CarrierServer,
            //    SubstrateManager);
        }
        #endregion </Constructors>

        #region <Fields>
        private static FunctionsForPWA500W_NRD_300 _instance = null;
        #endregion </Fields>

        #region <Properties>
        public static FunctionsForPWA500W_NRD_300 Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FunctionsForPWA500W_NRD_300();

                return _instance;
            }
        }

        private IPortRoleProfile PortRoleProfile { get; }
        #endregion </Properties>

        #region <Methods>

        #region <개조 여부에 따른 메서드>
        public void UpdateConditionAndPolicy(
            int portId,
            SubstrateType substrateType,
            ref string lastConditionKey,
            ref ICarrierCompletionHandlingPolicy currentPolicy,
            ref ICarrierCompletionCondition currentCondition)
        {
            PortRoleProfile.UpdateConditionAndPolicy(
                portId,
                substrateType,
                ref lastConditionKey,
                ref currentPolicy,
                ref currentCondition);
        }
        public bool IsProcessingCompleted(int portId, out List<string> processJobIds)
        {
            var lpIndex = LoadPortManager.GetLoadPortIndexByPortId(portId);
            var substrateType = GetSubstrateTypeByLoadPortIndex(lpIndex);

            return PortRoleProfile.IsProcessingCompleted(portId, substrateType, out processJobIds);
        }
        public bool IsProcessingCompleted(int portId, Substrate substrate ,out List<string> processJobIds)
        {
            var lpIndex = LoadPortManager.GetLoadPortIndexByPortId(portId);
            var substrateType = GetSubstrateTypeByLoadPortIndex(lpIndex);

            int targetPortId;
            if (substrateType.Equals(SubstrateType.Core))
            {
                targetPortId = portId;
            }
            else
            {
                targetPortId = substrate.SourcePortId;
            }

            return PortRoleProfile.IsProcessingCompleted(targetPortId, substrateType, out processJobIds);
        }
        public bool IsEmptyCarrierAtSimulation(SubstrateType substrateType)
        {
            return PortRoleProfile.IsEmptyCarrierAtSimulation(substrateType);
        }
        public bool IsJobRequiredForLoading(SubstrateType type)
        {
            return PortRoleProfile.IsJobRequiredForLoading(type);
        }
        public SubstrateType GetRequestTypeFromPMForBinOrEmptyType(bool isLoading)
        {
            return PortRoleProfile.GetRequestTypeFromPMForBinOrEmptyType(isLoading);
        }
        public bool IsBinType(SubstrateType type)
        {
            return PortRoleProfile.IsBinType(type);
        }
        public bool GetNextSlotInformationToPick(int lpIndex,
            SubstrateType type,
            string processJobId,
            out LoadPortLocation location,
            out string substrateKey)
        {
            return PortRoleProfile.GetNextSlotInformationToPick(
                lpIndex,
                type,
                processJobId,
                out location,
                out substrateKey);
        }
        public CheckingCarrierCodeToUnload FindWellknownProtInfoBySubstrateType(
            Substrate substrate,
            SubstrateType subType,
            ref int portId,
            ref int slot,
            ref string description)
        {
            return PortRoleProfile.FindWellknownProtInfoBySubstrateType(
                substrate,
                subType,
                GetSubstrateTypeByLoadPortIndex,
                ref portId,
                ref slot,
                ref description);
        }
        #endregion </개조 여부에 따른 메서드>
        public string GetLoadPortNameForUIUsingSizeAndType(int lpIndex)
        {
            SubstrateType substrateType = GetSubstrateTypeByLoadPortIndex(lpIndex);

            var paramNameOfSize = PARAM_EQUIPMENT.LoadPortSize1 + lpIndex;
            string subSizeByRecipe = Recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT,
                paramNameOfSize.ToString(),
                SubstrateSize.Inch_12.ToString());

            if (false == Enum.TryParse(subSizeByRecipe, out SubstrateSize substrateSize))
                return string.Empty;

            string size = string.Empty;
            switch (substrateSize)
            {
                case SubstrateSize.Inch_8:
                    size = "(8 Inch)";
                    break;
                case SubstrateSize.Inch_12:
                    size = "(12 Inch)";
                    break;
                default:
                    break;
            }

            return string.Format("{0} {1}", substrateType.ToString(), size);            
        }
        public bool GetSubstrateSpecByLoadPortIndex(int lpIndex,
            ref SubstrateType substrateType,
            ref SubstrateSize substrateSize)
        {
            substrateType = GetSubstrateTypeByLoadPortIndex(lpIndex);

            var paramNameOfSize = PARAM_EQUIPMENT.LoadPortSize1 + lpIndex;
            string subSizeByRecipe = Recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT,
                paramNameOfSize.ToString(),
                SubstrateSize.Inch_12.ToString());

            if (false == Enum.TryParse(subSizeByRecipe, out substrateSize))
                return false;

            return true;
        }

        public override SubstrateType GetSubstrateTypeByLoadPortIndex(int lpIndex)
        {
            var paramName = PARAM_EQUIPMENT.LoadPortType1 + lpIndex;
            string subTypeByRecipe = Recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT,
                paramName.ToString(),
                SubstrateType.Core.ToString());
            
            Enum.TryParse(subTypeByRecipe, out SubstrateType substrateType);
            
            return substrateType;
        }
        public SubstrateSize GetSubstrateSizeByLocationName(string locationName)
        {
            if (locationName.Equals(Constants.ProcessModuleCore_12_InputName) ||
                locationName.Equals(Constants.ProcessModuleCore_12_OutputName))
            {
                return SubstrateSize.Inch_12;
            }
            else if (locationName.Equals(Constants.ProcessModuleSort_12_InputName) ||
                locationName.Equals(Constants.ProcessModuleSort_12_OutputName))
            {
                return SubstrateSize.Inch_12;
            }
            else
            {
                return SubstrateSize.Inch_8;
            }
        }
        public bool GetSubstrateSpecByRequestedLocation(string locationName, ref SubstrateType substrateType, ref SubstrateSize substrateSize)
        {
            if (locationName.Equals(Constants.ProcessModuleCore_12_InputName) ||
               locationName.Equals(Constants.ProcessModuleCore_12_OutputName))
            {
                substrateType = SubstrateType.Core;
                substrateSize = SubstrateSize.Inch_12;

                return true;
            }
            else if (locationName.Equals(Constants.ProcessModuleSort_12_InputName) ||
                locationName.Equals(Constants.ProcessModuleSort_12_OutputName))
            {
                substrateType = SubstrateType.Bin1;
                substrateSize = SubstrateSize.Inch_12;

                return true;
            }
            else if (locationName.Equals(Constants.ProcessModuleCore_8_InputName) ||
               locationName.Equals(Constants.ProcessModuleCore_8_OutputName))
            {
                substrateType = SubstrateType.Core;
                substrateSize = SubstrateSize.Inch_8;
                
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public SubstrateSize GetSubstrateSizeByLoadPortIndex(int lpIndex)
        {
            var paramNameOfSize = PARAM_EQUIPMENT.LoadPortSize1 + lpIndex;
            string subSizeByRecipe = Recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT,
                paramNameOfSize.ToString(),
                SubstrateSize.Inch_12.ToString());

            Enum.TryParse(subSizeByRecipe, out SubstrateSize substrateSize);
            return substrateSize;
        }
        #endregion </Methods>
    }    
}
