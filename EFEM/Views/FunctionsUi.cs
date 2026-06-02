using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Recipe;
using FrameOfSystem3.Component;

namespace FrameOfSystem3.Views
{
	class FunctionsUi
	{
		#region <SINGLE TONE>
		static private FunctionsUi _Instance = null;

		static public FunctionsUi GetInstance()
		{
			if (_Instance == null)
			{
				_Instance = new FunctionsUi();
			}
			return _Instance;
		}
		#endregion </SINGLE TONE>

		public void CheckExclusiveParameter()
		{
			var recipe = FrameOfSystem3.Recipe.Recipe.GetInstance();

			//CheckExclusiveParameter(new PARAM_COMMON[] { PARAM_COMMON.Use_ForceSend_Good, PARAM_COMMON.Use_ForceSend_Ng });
			//CheckExclusiveParameter(new PARAM_COMMON[] { PARAM_COMMON.Use_WarpagePusher, PARAM_COMMON.Use_SEQ_WaferClampReplaceWarpagePusher });
		}
		public void CheckExclusiveParameter<T>(T[] parameterList, bool limitations = true)
		{
			if (parameterList.Length < 2) return;

			EN_RECIPE_TYPE recipeType;
			if (typeof(T) == typeof(PARAM_COMMON)) recipeType = EN_RECIPE_TYPE.COMMON;
			else if (typeof(T) == typeof(PARAM_EQUIPMENT)) recipeType = EN_RECIPE_TYPE.COMMON;
			else return;

			var recipe = FrameOfSystem3.Recipe.Recipe.GetInstance();
			bool isLimitOver = true;

			foreach(T parameter in parameterList)
			{
				isLimitOver &= (limitations == recipe.GetValue(recipeType, parameter.ToString(), 0, EN_RECIPE_PARAM_TYPE.VALUE, limitations));
			}

			if (isLimitOver)
			{
				// 배타적 파라미터이므로 모두 켜져있으면 모두 끄도록 한다.
				foreach (T parameter in parameterList)
				{
					recipe.SetValue(recipeType, parameter.ToString(), 0, EN_RECIPE_PARAM_TYPE.VALUE, (false == limitations).ToString());
				}
			}
		}
	}
}
