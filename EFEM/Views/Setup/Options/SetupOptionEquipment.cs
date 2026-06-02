using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FrameOfSystem3.Component;

using FrameOfSystem3.Recipe;
using Define.DefineEnumProject.Task;
using FrameOfSystem3.Config;

namespace FrameOfSystem3.Views.Setup.Options
{
	public partial class SetupOptionEquipment : ParameterPanel
	{
		#region <CONSTRUCTOR>
		public SetupOptionEquipment()
		{
			InitializeComponent();

			_selectionList = Functional.Form_SelectionList.GetInstance();

			_recipe = FrameOfSystem3.Recipe.Recipe.GetInstance();
			lblMachineLanguage.Text = ConfigLanguage.GetInstance().GetLangauge().ToString();
		}
		#endregion </CONSTRUCTOR>

		#region <Fields>
		private static Functional.Form_SelectionList _selectionList = null;
		private static FrameOfSystem3.Recipe.Recipe _recipe = null;
		#endregion </Fields>

		private void BtnButtonClicked(object sender, EventArgs e)
        {
			if (sender.Equals(lblMachineLanguage))
            {
                if (!(sender is Sys3Controls.Sys3Label label))
                    return;

                if (_selectionList.CreateForm("Select Language", Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.LANGUAGE, label.Text))
                {					
					string selectedLanguage = string.Empty;
					_selectionList.GetResult(ref selectedLanguage);
					if (false == Enum.TryParse(selectedLanguage, out ConfigLanguage.EN_PARAM_LANGUAGE targetLanguage))
						return;

					if (_recipe.SetValue(EN_RECIPE_TYPE.EQUIPMENT, PARAM_EQUIPMENT.MachineLanguage.ToString(), selectedLanguage))
					{
						ConfigLanguage.GetInstance().SetLanguage(targetLanguage);
					}
                }				
			}
        }
    }
}
