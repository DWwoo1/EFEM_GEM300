using System;

using FrameOfSystem3.Recipe;

using EFEM.CustomizedByProcessType.PWA500W;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace FrameOfSystem3.SECSGEM
{
    class FunctionsForPWA500W_NRD : CommonFunctionsForPWA500
    {
        #region <Constructors>
        private FunctionsForPWA500W_NRD() : base(false, false) { }
        #endregion </Constructors>

        #region <Fields>
        private static FunctionsForPWA500W_NRD _instance = null;
        #endregion </Fields>

        #region <Properties>
        public static FunctionsForPWA500W_NRD Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FunctionsForPWA500W_NRD();

                return _instance;
            }
        }
        #endregion </Properties>

        #region <Methods>
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
