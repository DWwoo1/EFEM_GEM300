using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario.Scenarios
{
    public class UploadRecipeParamValues : ScenarioParamValues
    {
        public UploadRecipeParamValues(string recipeId, string recipePath)
            : base()
        {
            RecipeId = recipeId;
            RecipePath = recipePath;
        }

        public string RecipeId { get; private set; }
        public string RecipePath { get; private set; }
    }

    public class ScenarioUploadRecipe : ScenarioBaseClass
    {
        #region <Constructor>
        public ScenarioUploadRecipe(string name, bool useInquire,
            uint timeOut = 10000)
            : base(name, timeOut)
        {
            UseInquire = useInquire;
        }
        #endregion </Constructor>

        #region <Fields>
        UploadRecipeParamValues _paramValues = null;
        private readonly bool UseInquire;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Type>
        private enum EN_SCENARIO_SEQ
        {
            INIT = 0,
            INQUIRE_UPLOAD = 100,
            RECIPE_UPLOAD = 200,
            WIAT_ACK = 300,
            FINISH = 1000,
        }
        #endregion </Type>

        #region <Methods>
        public override void UpdateParamValues(ScenarioParamValues paramValues)
        {
            _paramValues = paramValues as UploadRecipeParamValues;
        }

        public override EN_SCENARIO_RESULT ExecuteScenario()
        {
            switch (_seqNum)
            {
                case (int)EN_SCENARIO_SEQ.INIT:
                    Activate = true;
                    InitFlags();

                    _seqNum = (int)EN_SCENARIO_SEQ.INQUIRE_UPLOAD;
                    break;

                case (int)EN_SCENARIO_SEQ.INQUIRE_UPLOAD:
                    if (UseInquire == false)
                    {
                        _seqNum = (int)EN_SCENARIO_SEQ.RECIPE_UPLOAD;
                        break;
                    }
                    _gemHandler.SendRecipeUploadInquire(_paramValues.RecipeId);
                    SetTickCount(TimeOut);
                    _seqNum = (int)EN_SCENARIO_SEQ.WIAT_ACK;
                    break;

                case (int)EN_SCENARIO_SEQ.RECIPE_UPLOAD:
                    _gemHandler.SendRecipeUploadUnFormatted(_paramValues.RecipeId);
                    SetTickCount(TimeOut);
                    _seqNum = (int)EN_SCENARIO_SEQ.WIAT_ACK;
                    break;

                case (int)EN_SCENARIO_SEQ.WIAT_ACK:
                    if (IsTickOver(true))
                    {
                        return ReturnScenarioResult(EN_SCENARIO_RESULT.TIMEOUT_ERROR);
                    }
                    switch (Permission)
                    {
                        case EN_SCENARIO_PERMISSION_RESULT.OK:
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);

                        case EN_SCENARIO_PERMISSION_RESULT.ERROR:
                            return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
                        default:
                            break;
                    }
                    break;
                default:
                    return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
            }

            return EN_SCENARIO_RESULT.PROCEED;
        }
        #endregion </Methods>
    }
}
