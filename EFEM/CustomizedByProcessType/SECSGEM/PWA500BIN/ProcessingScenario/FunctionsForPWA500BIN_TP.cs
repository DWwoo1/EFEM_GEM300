using System;

using FrameOfSystem3.Recipe;

using EFEM.CustomizedByProcessType.PWA500BIN;
using EFEM.CustomizedByProcessType.PWA500Common;

namespace FrameOfSystem3.SECSGEM
{
    class FunctionsForPWA500BIN_TP : CommonFunctionsForPWA500
    {
        #region <Constructors>
        private FunctionsForPWA500BIN_TP() : base(true, true) { }
        #endregion </Constructors>

        #region <Fields>
        private static FunctionsForPWA500BIN_TP _instance = null;
        #endregion </Fields>

        #region <Properties>
        public static FunctionsForPWA500BIN_TP Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FunctionsForPWA500BIN_TP();

                return _instance;
            }
        }
        #endregion </Properties>

        #region <Methods>

        public string GetSubstrateTypeForUILoadPortIndex(int lpIndex)
        {
            var paramName = PARAM_EQUIPMENT.LoadPortType1 + lpIndex;
            string subTypeByRecipe = Recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT,
                paramName.ToString(),
                SubstrateTypeForUI.Core.ToString());

            return subTypeByRecipe;
        }
        public override SubstrateType GetSubstrateTypeByLoadPortIndex(int lpIndex)
        {
            var paramName = PARAM_EQUIPMENT.LoadPortType1 + lpIndex;
            string subTypeByRecipe = Recipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT,
                paramName.ToString(),
                SubstrateTypeForUI.Core.ToString());

            if (false == Enum.TryParse(subTypeByRecipe, out SubstrateTypeForUI substrateType))
                return SubstrateType.Empty;

            switch (substrateType)
            {
                case SubstrateTypeForUI.Core:
                    return SubstrateType.Core;

                case SubstrateTypeForUI.Empty:
                    return SubstrateType.Empty;

                case SubstrateTypeForUI.StageCenter:        // Bin1
                    return SubstrateType.Bin1;

                case SubstrateTypeForUI.StageLeft:          // Bin2
                    return SubstrateType.Bin2;

                case SubstrateTypeForUI.StageRight:         // Bin3
                    return SubstrateType.Bin3;

                default:
                    return SubstrateType.Empty;
            }
        }
        #endregion </Methods>
    }    
}
