using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario.Common
{
	public class ScenarioUpdateFDC : ScenarioBaseClass
	{
		#region <Constructor>
		public ScenarioUpdateFDC(
			string name,
			Func<Dictionary<long, string>> funcGetFdcData)
			: base(name, 0)
		{
			_funcGetFdcData = funcGetFdcData;
		}
		#endregion </Constructor>

		Func<Dictionary<long, string>> _funcGetFdcData;

		public override void UpdateParamValues(ScenarioParamValues paramValues)
		{
		}
		enum EN_STEP
		{
			Init,
			VidUpdate,
		}
		public override EN_SCENARIO_RESULT ExecuteScenario()
		{
			switch (_seqNum)
			{
				case (int)EN_STEP.Init:
					Activate = true;
					InitFlags();
					_seqNum = (int)EN_STEP.VidUpdate;
					break;

				case (int)EN_STEP.VidUpdate:
					{
						var fdcData = _funcGetFdcData();
						_gemHandler.UpdateVariables(fdcData.Keys.ToArray(), fdcData.Values.ToArray());
					}
                    return ReturnScenarioResult(EN_SCENARIO_RESULT.COMPLETED);

				default:
                    return ReturnScenarioResult(EN_SCENARIO_RESULT.ERROR);
			}

			return EN_SCENARIO_RESULT.PROCEED;
		}
	}
}
