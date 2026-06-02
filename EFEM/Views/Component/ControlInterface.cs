using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FrameOfSystem3.Recipe;
using Define.DefineEnumProject.SelectionList;

namespace FrameOfSystem3.Component
{
    /*
     *  Label TabIndex 구분
     *  0 : Process Calculator
     *  1 : Process Keyboard
     *  10 ~ : Process Selection List
     *  
     *  100 : Equipment Calculator
     *  101 : Equipment Keyboard
     *  110 ~ : Equipment Selection List
     *  
     *  200 : Common Calculator
     *  201 : Common Keyboard
     *  210 ~ : Common Selection List
     *  
     * 
     *  Button TabIndex 구분
     *  Task 내 Motion 및 Cylinder Index
     */ 

    public class ControlInterface
	{
        public ControlInterface()
        {
			// 2025.03.18 by junho [ADD] UI init 이후 Task name이나 parameter name이 변경 되었을 때, UI update가 정상적이지 않은 버그 개선
			CustomParameterLabel.ChangedRecipeProperty += CustomParameterLabel_ChangedRecipeProperty;
			CustomParameterToggleButton.ChangedRecipeProperty += CustomParameterToggleButton_ChangedRecipeProperty;
		}

		private void CustomParameterToggleButton_ChangedRecipeProperty(CustomParameterToggleButton target, string fromTask, string fromParameter, string toTask, string toParameter)
		{
			if (target.ParameterType != EN_RECIPE_TYPE.PROCESS)
				fromTask = toTask = target.ParameterType.ToString();

			m_instanceRecipe.DeregisterEachParameterChangedEvent(fromTask, fromParameter
				, new Recipe.Recipe.DeleParameterChangedCallback((v) => { GetParameter(target); }));
			m_instanceRecipe.RegisterEachParameterChangedEvent(toTask, toParameter
				, new Recipe.Recipe.DeleParameterChangedCallback((v) => { GetParameter(target); }));
		}

		private void CustomParameterLabel_ChangedRecipeProperty(CustomParameterLabel target, string fromTask, string fromParameter, string toTask, string toParameter)
		{
			if (target.ParameterType != EN_RECIPE_TYPE.PROCESS)
				fromTask = toTask = target.ParameterType.ToString();

			m_instanceRecipe.DeregisterEachParameterChangedEvent(fromTask, fromParameter
				, new Recipe.Recipe.DeleParameterChangedCallback((v) => { GetParameter(target); }));
			m_instanceRecipe.RegisterEachParameterChangedEvent(toTask, toParameter
				, new Recipe.Recipe.DeleParameterChangedCallback((v) => { GetParameter(target); }));
		}

		#region Variable
		FrameOfSystem3.Recipe.Recipe m_instanceRecipe = FrameOfSystem3.Recipe.Recipe.GetInstance();
		FrameOfSystem3.Recipe.PreviousValueStorage _previousValueStorage = FrameOfSystem3.Recipe.PreviousValueStorage.Instance;
        FrameOfSystem3.Config.ConfigMotion m_instanceMotion = FrameOfSystem3.Config.ConfigMotion.GetInstance();
        FrameOfSystem3.Config.ConfigDevice m_instanceDevice = FrameOfSystem3.Config.ConfigDevice.GetInstance();

        FrameOfSystem3.Views.Functional.Form_Calculator m_instanceCalculator = FrameOfSystem3.Views.Functional.Form_Calculator.GetInstance();
        FrameOfSystem3.Views.Functional.Form_Keyboard m_instnaceKeyboard = FrameOfSystem3.Views.Functional.Form_Keyboard.GetInstance();
        FrameOfSystem3.Views.Functional.Form_MessageBox m_instanceMessageBox = FrameOfSystem3.Views.Functional.Form_MessageBox.GetInstance();
        FrameOfSystem3.Views.Functional.Form_SelectionList m_instanceSelection = FrameOfSystem3.Views.Functional.Form_SelectionList.GetInstance();
		FrameOfSystem3.Views.Functional.Form_DateTimeSelector m_instanceDateTimeSelector = FrameOfSystem3.Views.Functional.Form_DateTimeSelector.GetInstance();

		Define.DefineEnumProject.Task.EN_TASK_LIST m_enTaskName;

        FrameOfSystem3.Recipe.PARAM_COMMON m_enCommon = (PARAM_COMMON)0;
        FrameOfSystem3.Recipe.PARAM_EQUIPMENT m_enEquipment = (PARAM_EQUIPMENT)0;

        string m_strErrorMessage = string.Empty;

        Dictionary<CustomParameterLabel, bool> dicOfParameterLabel = new Dictionary<CustomParameterLabel, bool>();
        Dictionary<CustomParameterButton, bool> dicOfParameterButton = new Dictionary<CustomParameterButton, bool>();
        Dictionary<CustomParameterToggleButton, bool> dicOfParameterToggle = new Dictionary<CustomParameterToggleButton, bool>();
		Dictionary<CustomJogButton, bool> dicOfCustomJogButton = new Dictionary<CustomJogButton, bool>();   // 2021.08.02. by shkim. [ADD] CUSTOM JOG BUTTON
		Dictionary<CustomActionButton, bool> dicOfCustomActionButton = new Dictionary<CustomActionButton, bool>();   // 2021.08.03. by junho [ADD] CUSTOM ACTION BUTTON
		Dictionary<CustomParameterLedLabel, bool> dicOfCustomParameterLedLabel = new Dictionary<CustomParameterLedLabel, bool>();
        #endregion

        #region Property
        public string strErrorMessage { get { return m_strErrorMessage; } }
        #endregion

        public void AssignControls(System.Windows.Forms.Control.ControlCollection parentControlColletion, DelAftetSetValue afterSetValue_Del = null)
        {
            foreach (var control in parentControlColletion)
            {
                System.Windows.Forms.Control ctrl = control as System.Windows.Forms.Control;

                // 2022.04.15. by WDW. [REMOVE] Disable이어도 값 표시되도록
//                 if (ctrl.Enabled == false)
//                     continue;

                // 2021.08.08. by shkim. [ADD] 그룹박스 처리 추가
                if (control is System.Windows.Forms.GroupBox)
                {
                    System.Windows.Forms.GroupBox group = (System.Windows.Forms.GroupBox)control;
                    AssignControls(group.Controls, afterSetValue_Del);
                }


				// 2023.10.02. by shkim. [ADD] Panel 처리 추가
				else if (control is System.Windows.Forms.Panel)
                {
                    System.Windows.Forms.Panel panel = (System.Windows.Forms.Panel)control;
                    AssignControls(panel.Controls, afterSetValue_Del);
                }

				else if (control is FrameOfSystem3.Views.ParameterPanel)
                {
                    FrameOfSystem3.Views.ParameterPanel panel = (FrameOfSystem3.Views.ParameterPanel)control;
                    AssignControls(panel.Controls, afterSetValue_Del);
                }
				// 2023.10.02. by shkim. [END]

				else if (control is CustomParameterLabel)
                {
                    CustomParameterLabel paraLabel = (CustomParameterLabel)control;

					if (paraLabel.ParameterName.Equals(string.Empty))
						paraLabel.ParameterName = paraLabel.Name;

					if (dicOfParameterLabel.ContainsKey(paraLabel))
						continue;

                    dicOfParameterLabel.Add(paraLabel, paraLabel.NeedRemakeMap);
                    paraLabel.Click += ChangeParameter;
					paraLabel.delAfterSetValue	= afterSetValue_Del;
					paraLabel.MouseHover += ShowDiscriptionTooltip;
                }
				else if (control is CustomParameterButton)
                {
                    CustomParameterButton paraBtn = (CustomParameterButton)control;
					if (dicOfParameterButton.ContainsKey(paraBtn))
						continue;
                    dicOfParameterButton.Add(paraBtn, paraBtn.NeedRemakeMap);
                    paraBtn.Click += ChangeParameter;
					paraBtn.delAfterSetValue	= afterSetValue_Del;
                }
				else if (control is CustomParameterToggleButton)
                {
                    CustomParameterToggleButton paraTg = (CustomParameterToggleButton)control;

					if (paraTg.ParameterName.Equals(string.Empty))
						paraTg.ParameterName = paraTg.Name;

					if (dicOfParameterToggle.ContainsKey(paraTg))
						continue;
                    dicOfParameterToggle.Add(paraTg, paraTg.NeedRemakeMap);
                    paraTg.ActiveChanged += ChangeParameter;
					paraTg.delAfterSetValue	= afterSetValue_Del;
					paraTg.MouseHover += ShowDiscriptionTooltip;
				}
				else if (control is CustomJogButton)    // 2021.08.02. by shkim. [ADD] CUSTOM JOG BUTTON
                {
                    CustomJogButton customJogButton = (CustomJogButton)control;

					if (dicOfCustomJogButton.ContainsKey(customJogButton))
						continue;
                    dicOfCustomJogButton.Add(customJogButton, false);
                    customJogButton.Click += OpenCustomJog;
					customJogButton.delAfterSetValue	= afterSetValue_Del;
                }
				else if (control is CustomActionButton)
				{
					CustomActionButton customActionButton = (CustomActionButton)control;

					if (dicOfCustomActionButton.ContainsKey(customActionButton))
						continue;
					dicOfCustomActionButton.Add(customActionButton, false);
					customActionButton.Click += DoTargetAction;
				}
				else if (control is CustomParameterLedLabel)
				{
					CustomParameterLedLabel paraLedLabel = (CustomParameterLedLabel)control;

					if (paraLedLabel.ParameterName.Equals(string.Empty))
						paraLedLabel.ParameterName = paraLedLabel.Name;

					if (dicOfCustomParameterLedLabel.ContainsKey(paraLedLabel))
						continue;

					dicOfCustomParameterLedLabel.Add(paraLedLabel, paraLedLabel.NeedRemakeMap);
					paraLedLabel.ActiveChanged += ChangeParameter;
					paraLedLabel.delAfterSetValue = afterSetValue_Del;
					paraLedLabel.MouseHover += ShowDiscriptionTooltip;
                }
				else if (control is System.Windows.Forms.TableLayoutPanel)
                {
					System.Windows.Forms.TableLayoutPanel group = (System.Windows.Forms.TableLayoutPanel)control;
					AssignControls(group.Controls, afterSetValue_Del);
				}
            }
        }
		public void AssignControlsWithAutoRefresh(System.Windows.Forms.Control.ControlCollection parentControlColletion, DelAftetSetValue afterSetValue_Del = null)
		{
			AssignControls(parentControlColletion, afterSetValue_Del);

			// 2024.09.19 by junho [MOD] 개별로 업데이트 하도록 변경
			//Recipe.Recipe.ParameterChangedNotify += ReceivedParameterChanged;
			foreach(var ctr in dicOfParameterLabel.Keys)
			{
				switch (ctr.ParameterType)
				{
					case EN_RECIPE_TYPE.PROCESS:
						m_instanceRecipe.RegisterEachParameterChangedEvent(ctr.TaskName, ctr.ParameterName
							, new Recipe.Recipe.DeleParameterChangedCallback((v) => { GetParameter(ctr); }));
						break;
					default:
						m_instanceRecipe.RegisterEachParameterChangedEvent(ctr.ParameterType.ToString(), ctr.ParameterName
							, new Recipe.Recipe.DeleParameterChangedCallback((v) => { GetParameter(ctr); }));
						break;
				}
			}
			foreach (var ctr in dicOfParameterToggle.Keys)
			{
				switch (ctr.ParameterType)
				{
					case EN_RECIPE_TYPE.PROCESS:
						m_instanceRecipe.RegisterEachParameterChangedEvent(ctr.TaskName, ctr.ParameterName
							, new Recipe.Recipe.DeleParameterChangedCallback((v) => { GetParameter(ctr); }));
						break;
					default:
						m_instanceRecipe.RegisterEachParameterChangedEvent(ctr.ParameterType.ToString(), ctr.ParameterName
							, new Recipe.Recipe.DeleParameterChangedCallback((v) => { GetParameter(ctr); }));
						break;
				}
			}

			// 2024.12.10. by shkim. [ADD] LedLabel 파라미터 기능 추가
			foreach (var ctr in dicOfCustomParameterLedLabel.Keys)
			{
				switch (ctr.ParameterType)
				{
					case EN_RECIPE_TYPE.PROCESS:
						m_instanceRecipe.RegisterEachParameterChangedEvent(ctr.TaskName, ctr.ParameterName
							, new Recipe.Recipe.DeleParameterChangedCallback((v) => { GetParameter(ctr); }));
						break;
					default:
						m_instanceRecipe.RegisterEachParameterChangedEvent(ctr.ParameterType.ToString(), ctr.ParameterName
							, new Recipe.Recipe.DeleParameterChangedCallback((v) => { GetParameter(ctr); }));
						break;
				}
			}
			// 2024.12.10. by shkim. [END]

		}
		// 이제 필요 없음.
		//private void ReceivedParameterChanged(bool result, List<Recipe.Recipe.ParameterItem> changedList)
		//{
		//	RefreshValueParameter();
		//}
		public void RefreshValueParameter()
        {
			foreach (var label in dicOfParameterLabel.Keys)
            {
				GetParameter(label);
            }

			foreach (var togle in dicOfParameterToggle.Keys)
			{
				GetParameter(togle);
			}

			foreach(var ledLabel in dicOfCustomParameterLedLabel.Keys)
			{
				GetParameter(ledLabel);
			}
        }
        public void ActivateControls()
        {
            foreach (var pTgBtn in dicOfParameterToggle)
            {
                CustomParameterToggleButton tgBtn = pTgBtn.Key;
                GetParameter(tgBtn);
            }
        }

        #region External Interface

        #region Get Parameter
        public void GetParameter(Component.CustomParameterLabel labelTarget)
        {
            string strTaskName = labelTarget.TaskName.ToString();

            if (false == IsCorrectiveName(strTaskName, labelTarget.ParameterName, labelTarget.ParameterType))
            {
				// 2023.02.03 by junho [ADD] parameter 설정 안되면 message 띄우지 않고 색으로 알리도록 변경
				labelTarget.BackGroundColor = System.Drawing.Color.IndianRed;

                labelTarget.Text = string.Empty;
                return;
            }

            string strParameter = string.Empty;
			string strUnit = string.Empty;
			string strType = string.Empty;
            switch (labelTarget.ParameterType)
            {
				case EN_RECIPE_TYPE.PROCESS:
					if (false == m_instanceRecipe.GetDeferredStorage(strTaskName, labelTarget.ParameterName, labelTarget.ParameterIndex, out strParameter))
					{
						strParameter = m_instanceRecipe.GetValue(strTaskName
							, labelTarget.ParameterName
							, labelTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE
							, string.Empty);

							labelTarget.MainFontColor = labelTarget.ParameterChangeDefaultColor;
					}
                    else
                    {
                        labelTarget.MainFontColor = labelTarget.ParameterChangeWaitColor;
                    }

					strType = m_instanceRecipe.GetValue(strTaskName
						, labelTarget.ParameterName
						, labelTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE
						, string.Empty);

					strUnit = m_instanceRecipe.GetValue(strTaskName
						, labelTarget.ParameterName
						, labelTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
						, string.Empty);

                    break;
                case EN_RECIPE_TYPE.EQUIPMENT:
                case EN_RECIPE_TYPE.COMMON:
					if (false == m_instanceRecipe.GetDeferredStorage(labelTarget.ParameterType, labelTarget.ParameterName, labelTarget.ParameterIndex, out strParameter))
					{
						strParameter = m_instanceRecipe.GetValue(labelTarget.ParameterType
							, labelTarget.ParameterName
							, labelTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE
							, string.Empty);

						labelTarget.MainFontColor = labelTarget.ParameterChangeDefaultColor;
					}
					else
					{
						labelTarget.MainFontColor = labelTarget.ParameterChangeWaitColor;
					}

					strType = m_instanceRecipe.GetValue(labelTarget.ParameterType
						, labelTarget.ParameterName
						, labelTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE
						, string.Empty);

					strUnit = m_instanceRecipe.GetValue(labelTarget.ParameterType
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
                        , string.Empty);

                    break;
			}

			// 2024.02.17 by junho [ADD] type확인해서 실수형이라면 소수점 3자리까지만 표시하도록 변경
			EN_DATA_TYPE dataType;
			if (Enum.TryParse(strType, out dataType))
			{
				switch (dataType)
				{
					case EN_DATA_TYPE.FLOAT4:
					case EN_DATA_TYPE.FLOAT8:
						{
							double value;
							if (double.TryParse(strParameter, out value))
							{
								strParameter = Math.Round(value, 3).ToString();
							}
						}
						break;
				}
			}

            labelTarget.Text = strParameter;

			if(labelTarget.UseUnitFont)
				labelTarget.UnitText = strUnit;

			//SetPropertyData(labelTarget);
        }
        public void GetParameter(Component.CustomParameterToggleButton tgTarget)
        {
            string strTaskName = tgTarget.TaskName.ToString();

            if (false == IsCorrectiveName(strTaskName, tgTarget.ParameterName, tgTarget.ParameterType))
			{
				// 2023.02.03 by junho [ADD] parameter 설정 안되면 message 띄우지 않고 색으로 알리도록 변경
				tgTarget.BackColor = System.Drawing.Color.IndianRed;

				tgTarget.Active = false;
                return;
            }

            bool bParameter = false;
			string strParameter = "False";
            switch (tgTarget.ParameterType)
            {
                case EN_RECIPE_TYPE.PROCESS:
					if (m_instanceRecipe.GetDeferredStorage(strTaskName, tgTarget.ParameterName, tgTarget.ParameterIndex, out strParameter))
					{
						if(false == bool.TryParse(strParameter, out bParameter))
						{
							tgTarget.BackColor = System.Drawing.Color.Violet;
							tgTarget.Active = false;
							return;
						}

						tgTarget.ActiveColorFirst = tgTarget.ParameterChangeWaitColorFirst;
						tgTarget.ActiveColorSecond = tgTarget.ParameterChangeWaitColorSecond;
						tgTarget.NormalColorFirst = tgTarget.ParameterChangeWaitColorFirst;
						tgTarget.NormalColorSecond = tgTarget.ParameterChangeWaitColorSecond;
					}
					else
					{
						bParameter = m_instanceRecipe.GetValue(strTaskName
						, tgTarget.ParameterName
						, tgTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE
						, false);

						tgTarget.ActiveColorFirst = tgTarget.ParameterChangeDefaultActiveColorFirst;
						tgTarget.ActiveColorSecond = tgTarget.ParameterChangeDefaultActiveColorSecond;
						tgTarget.NormalColorFirst = tgTarget.ParameterChangeDefaultNormalColorFirst;
						tgTarget.NormalColorSecond = tgTarget.ParameterChangeDefaultNormalColorSecond;
					}

                    break;
                case EN_RECIPE_TYPE.EQUIPMENT:
                case EN_RECIPE_TYPE.COMMON:
					if (m_instanceRecipe.GetDeferredStorage(tgTarget.ParameterType, tgTarget.ParameterName, tgTarget.ParameterIndex, out strParameter)
						&& bool.TryParse(strParameter, out bParameter))
					{
						if (false == bool.TryParse(strParameter, out bParameter))
						{
							tgTarget.BackColor = System.Drawing.Color.Violet;
							tgTarget.Active = false;
							return;
						}

						tgTarget.ActiveColorFirst = tgTarget.ParameterChangeWaitColorFirst;
						tgTarget.ActiveColorSecond = tgTarget.ParameterChangeWaitColorSecond;
						tgTarget.NormalColorFirst = tgTarget.ParameterChangeWaitColorFirst;
						tgTarget.NormalColorSecond = tgTarget.ParameterChangeWaitColorSecond;
					}
					else
					{

						bParameter = m_instanceRecipe.GetValue(tgTarget.ParameterType
							, tgTarget.ParameterName
							, tgTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE
							, false);

						tgTarget.ActiveColorFirst = tgTarget.ParameterChangeDefaultActiveColorFirst;
						tgTarget.ActiveColorSecond = tgTarget.ParameterChangeDefaultActiveColorSecond;
						tgTarget.NormalColorFirst = tgTarget.ParameterChangeDefaultNormalColorFirst;
						tgTarget.NormalColorSecond = tgTarget.ParameterChangeDefaultNormalColorSecond;
					}
                    break;
            }
			tgTarget.Active = bParameter;
        }

		public void GetParameter(Component.CustomParameterLedLabel ledTarget)
		{
			string strTaskName = ledTarget.TaskName.ToString();

			if (false == IsCorrectiveName(strTaskName, ledTarget.ParameterName, ledTarget.ParameterType))
			{
				// 2023.02.03 by junho [ADD] parameter 설정 안되면 message 띄우지 않고 색으로 알리도록 변경
				ledTarget.MainFontColor = System.Drawing.Color.IndianRed;

				ledTarget.Active = false;
				return;
			}

			bool bParameter = false;
			string strParameter = "False";
			switch (ledTarget.ParameterType)
			{
				case EN_RECIPE_TYPE.PROCESS:
					if (m_instanceRecipe.GetDeferredStorage(strTaskName, ledTarget.ParameterName, ledTarget.ParameterIndex, out strParameter))
					{
						if (false == bool.TryParse(strParameter, out bParameter))
						{
							ledTarget.BackColor = System.Drawing.Color.Violet;
							ledTarget.Active = false;
							return;
						}
					}
					else
					{
						bParameter = m_instanceRecipe.GetValue(strTaskName
						, ledTarget.ParameterName
						, ledTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE
						, false);
					}
					break;
				case EN_RECIPE_TYPE.EQUIPMENT:
				case EN_RECIPE_TYPE.COMMON:
					if (m_instanceRecipe.GetDeferredStorage(ledTarget.ParameterType, ledTarget.ParameterName, ledTarget.ParameterIndex, out strParameter)
						&& bool.TryParse(strParameter, out bParameter))
					{
						if (false == bool.TryParse(strParameter, out bParameter))
						{
							ledTarget.BackColor = System.Drawing.Color.Violet;
							ledTarget.Active = false;
							return;
						}
					}
					else
					{

						bParameter = m_instanceRecipe.GetValue(ledTarget.ParameterType
							, ledTarget.ParameterName
							, ledTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE
							, false);
					}
					break;
			}
			ledTarget.MainFontColor = System.Drawing.Color.Black;
			ledTarget.Active = bParameter;
		}
		#endregion

		#region Set Parameter

		#region Typing Parameter

		public bool SetParameter(Component.CustomParameterLedLabel labelTarget)
		{
			string strTaskName = labelTarget.TaskName.ToString();

			if (false == IsCorrectiveName(strTaskName, labelTarget.ParameterName, labelTarget.ParameterType))
			{
				labelTarget.MainFontColor = System.Drawing.Color.IndianRed;

				return false;
			}

			string strResult = string.Empty;

			switch (labelTarget.ParameterType)
			{
				case EN_RECIPE_TYPE.PROCESS:
					{
						bool beforeValue = m_instanceRecipe.GetValue(strTaskName
							, labelTarget.ParameterName
							, labelTarget.ParameterIndex
							, EN_RECIPE_PARAM_TYPE.VALUE
							, false);
						strResult = (!beforeValue).ToString();

						if (labelTarget.UseParameterChangeConfirm)
						{
							if (false == m_instanceRecipe.SetDeferredStorage(strTaskName, labelTarget.ParameterName, labelTarget.ParameterIndex, strResult))
								return false;

							labelTarget.MainFontColor = labelTarget.ParameterChangeWaitColor;
						}
						else
						{
							if (false == m_instanceRecipe.SetValue(strTaskName
								, labelTarget.ParameterName
								, labelTarget.ParameterIndex
								, EN_RECIPE_PARAM_TYPE.VALUE
								, strResult))
								return false;
						}
						labelTarget.Active = !beforeValue;
					}
					break;

				case EN_RECIPE_TYPE.EQUIPMENT:
				case EN_RECIPE_TYPE.COMMON:
					{
						bool beforeValue = m_instanceRecipe.GetValue(labelTarget.ParameterType
								, labelTarget.ParameterName
								, labelTarget.ParameterIndex
								, EN_RECIPE_PARAM_TYPE.VALUE
								, false);
						strResult = (!beforeValue).ToString();

						if (labelTarget.UseParameterChangeConfirm)
						{
							if (false == m_instanceRecipe.SetDeferredStorage(labelTarget.ParameterType, labelTarget.ParameterName, labelTarget.ParameterIndex, strResult))
								return false;

							labelTarget.MainFontColor = labelTarget.ParameterChangeWaitColor;
						}
						else
						{
							if (false == m_instanceRecipe.SetValue(labelTarget.ParameterType
								, labelTarget.ParameterName
								, labelTarget.ParameterIndex
								, EN_RECIPE_PARAM_TYPE.VALUE
								, strResult))
								return false;
						}
						labelTarget.Active = !beforeValue;
					}
					break;
			}

			return true;
		}

        public bool SetParameter(Component.CustomParameterToggleButton tgTarget)
        {
            string strTaskName = tgTarget.TaskName.ToString();
            if (false == IsCorrectiveName(strTaskName, tgTarget.ParameterName, tgTarget.ParameterType))
            {
				// 2023.02.03 by junho [ADD] parameter 설정 안되면 message 띄우지 않고 색으로 알리도록 변경
				tgTarget.BackColor = System.Drawing.Color.IndianRed;

				tgTarget.Active = false;
                return false;
            }
			string strResult = tgTarget.Active.ToString();

			SetPropertyData(tgTarget);

			switch (tgTarget.ParameterType)
			{
				case EN_RECIPE_TYPE.PROCESS:
					if (tgTarget.UseParameterChangeConfirm)
					{
						if (false == m_instanceRecipe.SetDeferredStorage(strTaskName, tgTarget.ParameterName, tgTarget.ParameterIndex, strResult))
							return false;

						tgTarget.ActiveColorFirst = tgTarget.ParameterChangeWaitColorFirst;
						tgTarget.ActiveColorSecond = tgTarget.ParameterChangeWaitColorSecond;
						tgTarget.NormalColorFirst = tgTarget.ParameterChangeWaitColorFirst;
						tgTarget.NormalColorSecond = tgTarget.ParameterChangeWaitColorSecond;
					}
					else
					{
						if (false == m_instanceRecipe.SetValue(strTaskName
							, tgTarget.ParameterName
							, tgTarget.ParameterIndex
							, EN_RECIPE_PARAM_TYPE.VALUE
							, strResult))
							return false;
					}
					break;
				case EN_RECIPE_TYPE.EQUIPMENT:
				case EN_RECIPE_TYPE.COMMON:
					if (tgTarget.UseParameterChangeConfirm)
					{
						if (false == m_instanceRecipe.SetDeferredStorage(tgTarget.ParameterType, tgTarget.ParameterName, tgTarget.ParameterIndex, strResult))
							return false;

						tgTarget.ActiveColorFirst = tgTarget.ParameterChangeWaitColorFirst;
						tgTarget.ActiveColorSecond = tgTarget.ParameterChangeWaitColorSecond;
						tgTarget.NormalColorFirst = tgTarget.ParameterChangeWaitColorFirst;
						tgTarget.NormalColorSecond = tgTarget.ParameterChangeWaitColorSecond;
					}
					else
					{
						if (false == m_instanceRecipe.SetValue(tgTarget.ParameterType
						, tgTarget.ParameterName
						, tgTarget.ParameterIndex
						, EN_RECIPE_PARAM_TYPE.VALUE
						, strResult))
							return false;
					}
					break;
			}

            return true;
        }
        public bool SetParameter(Component.CustomParameterLabel labelTarget)
        {
            string strTaskName = labelTarget.TaskName.ToString();

			if (false == IsCorrectiveName(strTaskName, labelTarget.ParameterName, labelTarget.ParameterType))
            {
				// 2023.02.03 by junho [ADD] parameter 설정 안되면 message 띄우지 않고 색으로 알리도록 변경
				labelTarget.BackGroundColor = System.Drawing.Color.IndianRed;

				labelTarget.Text = string.Empty;
                return false;
            }

            string strResult = string.Empty;

			SetPropertyData(labelTarget);

            switch (labelTarget.ParameterSettingType)
            {
				case Component.EN_LABEL_PARAMETER_TYPE.CALCULATE_DOUBLE:
				case Component.EN_LABEL_PARAMETER_TYPE.CALCULATE_INT:
				case Component.EN_LABEL_PARAMETER_TYPE.CALCULATE_UINT:
                    if (false == GetResultByCalculator(labelTarget, ref strResult))
                        return false;

                    break;
                case Component.EN_LABEL_PARAMETER_TYPE.KEYBOARD:
					{
						string previousValue = "", previousTime = "", strUnit = "";

						// 2025.06.13 by junho [ADD] Property 정보 추가
						#region get property
						switch (labelTarget.ParameterType)
						{
							case EN_RECIPE_TYPE.PROCESS:
								strUnit = m_instanceRecipe.GetValue(labelTarget.TaskName.ToString()
									, labelTarget.ParameterName
									, labelTarget.ParameterIndex
									, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
									, string.Empty);
								break;
							case EN_RECIPE_TYPE.COMMON:
							case EN_RECIPE_TYPE.EQUIPMENT:
								strUnit = m_instanceRecipe.GetValue(labelTarget.ParameterType
									, labelTarget.ParameterName
									, labelTarget.ParameterIndex
									, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
									, string.Empty);
								break;
						}

						_previousValueStorage.Get(labelTarget.ParameterType, labelTarget.TaskName, labelTarget.ParameterName, out previousValue, out previousTime, string.Empty);
						#endregion get property

						if (m_instnaceKeyboard.CreateForm(labelTarget.Text, strTitle: labelTarget.ParameterName, strUnit: strUnit, strPrevious: previousValue, strPreviousTime: previousTime) == false)
							return false;

						m_instnaceKeyboard.GetResult(ref strResult);
					}
                    break;
                case Component.EN_LABEL_PARAMETER_TYPE.SELECT:
					#region 
					{
						// 2025.06.13 by junho [ADD] Previous value 추가
						string previousValue = "", previousTime = "";
						_previousValueStorage.Get(labelTarget.ParameterType, labelTarget.TaskName, labelTarget.ParameterName, out previousValue, out previousTime, string.Empty);

						if (false == m_instanceSelection.CreateForm(labelTarget.ParameterName, labelTarget.SelectionList, labelTarget.Text, previousValue, previousTime))
							return false;

						m_instanceSelection.GetResult(ref strResult);
					}
                    #endregion
                    break;
                case Component.EN_LABEL_PARAMETER_TYPE.FOLDER_DIALOG:
					#region 
					// 2025.04.21 by junho [MOD] Dialog type일 때, Keyboard 입력도 가능하도록 변경
					if (false == m_instanceSelection.CreateForm("INSERT TYPE", new string[] { "DIALOG", "KEYBOARD" }, "DIALOG"))
						return false;

					m_instanceSelection.GetResult(ref strResult);
					switch (strResult)
					{
						case "DIALOG":
							{
								System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
								switch (dlg.ShowDialog())
								{
									case System.Windows.Forms.DialogResult.OK:
										strResult = dlg.SelectedPath;
										dlg.Dispose();
										break;
									default:
										dlg.Dispose();
										return false;
								}
							}
							break;
						case "KEYBOARD":
							{
								if (m_instnaceKeyboard.CreateForm(labelTarget.Text) == false)
									return false;

								m_instnaceKeyboard.GetResult(ref strResult);
							}
							break;
						default:
							return false;

					}
					#endregion
                    break;
				case Component.EN_LABEL_PARAMETER_TYPE.DATE_TIME:
					#region 
					{
						TimeSpan oldValue;
						if (false == TimeSpan.TryParse(labelTarget.Text, out oldValue)) oldValue = TimeSpan.Zero;
						if (false == m_instanceDateTimeSelector.CreateForm(oldValue, Views.Functional.Form_DateTimeSelector.EShowType.Full, labelTarget.ParameterName))
							return false;

						m_instanceDateTimeSelector.GetResult(ref strResult);
					}
					#endregion
					break;
				default:
                    m_strErrorMessage = String.Format("You have not [Parameter Setting Type] set it up.");
                    return false;
            }

			switch (labelTarget.ParameterType)
			{
				case EN_RECIPE_TYPE.PROCESS:
					if (labelTarget.UseParameterChangeConfirm)
					{
						if (false == m_instanceRecipe.SetDeferredStorage(strTaskName, labelTarget.ParameterName, labelTarget.ParameterIndex, strResult))
							return false;

						labelTarget.MainFontColor = labelTarget.ParameterChangeWaitColor;
					}
					else
					{
						if (false == m_instanceRecipe.SetValue(strTaskName
							, labelTarget.ParameterName
							, labelTarget.ParameterIndex
							, EN_RECIPE_PARAM_TYPE.VALUE
							, strResult))
							return false;
					}
					break;
				case EN_RECIPE_TYPE.EQUIPMENT:
				case EN_RECIPE_TYPE.COMMON:
					if (labelTarget.UseParameterChangeConfirm)
					{
						if (false == m_instanceRecipe.SetDeferredStorage(labelTarget.ParameterType, labelTarget.ParameterName, labelTarget.ParameterIndex, strResult))
							return false;

						labelTarget.MainFontColor = labelTarget.ParameterChangeWaitColor;
					}
					else
					{
						// TODO : 요기필요 -> 이렇게 처리하면 안 될 것 같다
						// 2025.07.08. jhlim [MOD] 컨펌 사용하지 않는 경우 추가 -> 기존에는 SetValue로 직접 하던 것을 컨펌 미확인 이후 Apply 하도록 변경
						m_instanceRecipe.SetDeferredStorage(labelTarget.ParameterType, labelTarget.ParameterName, labelTarget.ParameterIndex, strResult, false);
						m_instanceRecipe.ApplyDeferredStorage();
						//if (false == m_instanceRecipe.SetValue(labelTarget.ParameterType
						//	, labelTarget.ParameterName
						//	, labelTarget.ParameterIndex
						//	, EN_RECIPE_PARAM_TYPE.VALUE
						//	, strResult))
						//	return false;
						// 2025.07.08. jhlim [END]
					}
					break;
			}

			labelTarget.Text = strResult;

			return true;
        }

		#endregion /Typing Parameter

		#region Position Parameter
		public bool SetParameter(Component.CustomParameterButton buttonTarget)
        {
            m_strErrorMessage = string.Empty;

            string strTaskName = buttonTarget.TaskName.ToString();
            string strAxisName = string.Empty;

            if (false == IsCorrectiveName(buttonTarget.TaskName.ToString(), buttonTarget.ParameterName, buttonTarget.ParameterType))
			{
				// 2023.02.03 by junho [ADD] parameter 설정 안되면 message 띄우지 않고 색으로 알리도록 변경
				buttonTarget.GradientFirstColor = System.Drawing.Color.IndianRed;
				buttonTarget.GradientSecondColor = System.Drawing.Color.IndianRed;

                return false;
			}

            // double dblActualPosition = GetActualPosition(buttonTarget, ref strAxisName);
            double dblActualPosition = 0.0;
            if (false == GetActualPosition(buttonTarget, ref strAxisName, ref dblActualPosition))
                return false;
            
            // string strMessage = String.Format("Do you want Set parameter? \\n AXIS : {0} \\n PARAMETER : {1} \\n{2}"
            string strMessage = String.Format("Do you want Set parameter? \\n PARAMETER : {0} \\n AXIS : {1} | Value : {2}"
                , buttonTarget.ParameterName
                , strAxisName
                , Math.Round(dblActualPosition,3));

            if (m_instanceMessageBox.ShowMessage(strMessage, "SET PARAMETER") == false)
                return false;

            string strActualPosition = String.Format("{0:0.000}", dblActualPosition);

            switch (buttonTarget.ParameterType)
            {
                case EN_RECIPE_TYPE.PROCESS:
                    if (false == m_instanceRecipe.SetValue(strTaskName
                        , buttonTarget.ParameterName
                        , buttonTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE
                        , strActualPosition))
                        return false;

                    break;
                case EN_RECIPE_TYPE.EQUIPMENT:
                    if (false == m_instanceRecipe.SetValue(EN_RECIPE_TYPE.EQUIPMENT
                        , buttonTarget.ParameterName
                        , buttonTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE
                        , strActualPosition))
                        return false;

                    break;
                case EN_RECIPE_TYPE.COMMON:
                    if (false == m_instanceRecipe.SetValue(EN_RECIPE_TYPE.COMMON
                        , buttonTarget.ParameterName
                        , buttonTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE
                        , strActualPosition))
                        return false;

                    break;
            }


            return true;
        }
        #endregion

		#endregion /Set Parameter

		#region Change recipe property
		/// <summary>
		/// 가지고있는 control의 target task가 taskFrom이면 taskTo로 교체한다.
		/// </summary>
		public void ChangeTargetTask(string taskFrom, string taskTo)
		{
			foreach(var control in dicOfParameterLabel.Keys)
			{
				if (control.TaskName == taskFrom)
				{
					control.TaskName = taskTo;
				}
			}
			foreach (var control in dicOfParameterButton.Keys)
			{
				if (control.TaskName == taskFrom)
					control.TaskName = taskTo;
			}
			foreach (var control in dicOfParameterToggle.Keys)
			{
				if (control.TaskName == taskFrom)
				{
					control.TaskName = taskTo;
				}
			}
			foreach (var control in dicOfCustomJogButton.Keys)
			{
				if (control.TaskName1 == taskFrom)
					control.TaskName1 = taskTo;
				if (control.TaskName2 == taskFrom)
					control.TaskName2 = taskTo;
			}
			foreach (var control in dicOfCustomActionButton.Keys)
			{
				if (control.TaskName == taskFrom)
					control.TaskName = taskTo;
			}
		}
		public void ChangeTargetAxis(string axixFrom, string axisTo)
		{
			foreach (var control in dicOfCustomJogButton.Keys)
			{
				if (control.AxisName1 == axixFrom)
					control.AxisName1 = axisTo;
				if (control.AxisName2 == axixFrom)
					control.AxisName2 = axisTo;
			}
		}
		#endregion /Change recipe property

		#endregion

		#region Internal Interface

		#region Name Check
		private bool IsCorrectiveName(string strTaskName, string strParameterName, EN_RECIPE_TYPE enParameterType)
        {
            switch (enParameterType)
            {
                case EN_RECIPE_TYPE.COMMON:
                    if (Enum.TryParse(strParameterName, out m_enCommon) == false)
                    {
						// 2023.02.03 by junho [DEL] parameter 설정 안되면 message 띄우지 않고 색으로 알리도록 변경
						//m_strErrorMessage = String.Format("You entered Parameter Name incorrectly! \\n PARAMETER TYPE : {0} \\n PARAMETER NAME : {1}", EN_RECIPE_TYPE.COMMON, strParameterName);
						//m_instanceMessageBox.ShowMessage(m_strErrorMessage);
                        return false;
                    }
                    break;
                case EN_RECIPE_TYPE.EQUIPMENT:
                    if (Enum.TryParse(strParameterName, out m_enEquipment) == false)
                    {
						// 2023.02.03 by junho [DEL] parameter 설정 안되면 message 띄우지 않고 색으로 알리도록 변경
						//m_strErrorMessage = String.Format("You entered Parameter Name incorrectly! \\n PARAMETER TYPE : {0} \\n PARAMETER NAME : {1}", EN_RECIPE_TYPE.EQUIPMENT, strParameterName);
						//m_instanceMessageBox.ShowMessage(m_strErrorMessage);
                        return false;
                    }

                    break;
                case EN_RECIPE_TYPE.PROCESS:
                    #region Process
                    if (Enum.TryParse(strTaskName, out m_enTaskName) == false)
                    {
						// 2023.02.03 by junho [DEL] parameter 설정 안되면 message 띄우지 않고 색으로 알리도록 변경
						//m_strErrorMessage = String.Format("You entered Task Name incorrectly! \\n TASK NAME : {0}", m_enTaskName);
						//m_instanceMessageBox.ShowMessage(m_strErrorMessage);
						return false;
                    }

					if(false == ConvertProjectTaskData.IsDefinedParameter(m_enTaskName, strParameterName))
					{
						// 2023.02.03 by junho [DEL] parameter 설정 안되면 message 띄우지 않고 색으로 알리도록 변경
						//m_strErrorMessage = String.Format("You entered Parameter Name incorrectly! \\n TASK NAME : {0} \\n PARAMETER NAME : {1}", m_enTaskName, strParameterName);
						//m_instanceMessageBox.ShowMessage(m_strErrorMessage);
						return false;
					}
                    #endregion
                    break;
            }

            return true;
        }
        #endregion

        #region Axis Check
        private int GetAxisNumber(Component.CustomParameterButton buttonTarget, ref string strAxisName)
        {
            int nAxisNumber = 0;
            string strTaskName = buttonTarget.TaskName.ToString();

            return nAxisNumber;
        }
        #endregion

        #region Actual Position
        // 2021.06.15. by shkim. [MOD] Enum 값이 아닌 컨트롤에 사전에 설정된 taskName, axisName으로 Target Index를 확인하도록 수정
        private bool GetActualPosition(Component.CustomParameterButton buttonTarget, ref string strAxisName, ref double actualPos)
        {
            int nAxisNumber = 0;
            int nIndexOfTaskAxis = 0;
            string strTaskName = buttonTarget.TaskName.ToString();
            
            int nDeviceCount = 0;
            int[] arrDeviceIndexNumbers = null;
            Config.ConfigDevice.GetInstance().GetIndexesOfDevice(strTaskName, Config.ConfigDevice.EN_TYPE_DEVICE.MOTION, ref nDeviceCount, ref arrDeviceIndexNumbers);

            for(int i = 0; i < nDeviceCount; i++)
            {
                string tempTargetDeviceName = string.Empty;
                Config.ConfigDevice.GetInstance().GetDeviceTargetName(strTaskName, Config.ConfigDevice.EN_TYPE_DEVICE.MOTION, arrDeviceIndexNumbers[i], ref tempTargetDeviceName);
                if (buttonTarget.AxisName.Equals(tempTargetDeviceName))
                {
                    nIndexOfTaskAxis = arrDeviceIndexNumbers[i];

                    if (false == m_instanceDevice.GetDeviceTargetIndex(strTaskName
                    , FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.MOTION
                    , nIndexOfTaskAxis
                    , ref nAxisNumber))
                    {
                        return false;
                    }
                        
                    if (false == m_instanceDevice.GetDeviceTargetName(strTaskName
                                , FrameOfSystem3.Config.ConfigDevice.EN_TYPE_DEVICE.MOTION
                                , nIndexOfTaskAxis
                                , ref strAxisName))
                    {
                        return false;
                    }

                    actualPos = m_instanceMotion.GetActualPosition(nAxisNumber);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Calculator
        private bool GetResultByCalculator(Component.CustomParameterLabel labelTarget, ref string strResult)
        {
            string strOld = string.Empty; // 2021.06.08. by shkim. [ADD] 이전 값 출력 추가
            string strMin = string.Empty;
            string strMax = string.Empty;
            string strUnit = string.Empty;
			string strPrevious = string.Empty;
			string strPreviousTime = string.Empty;

			#region Get old, min, max, unit
			switch (labelTarget.ParameterType)
            {
                case EN_RECIPE_TYPE.PROCESS:
                    strMin = m_instanceRecipe.GetValue(labelTarget.TaskName.ToString()
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MIN
                        , string.Empty);

                    strMax = m_instanceRecipe.GetValue(labelTarget.TaskName.ToString()
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MAX
                        , string.Empty);

                    strUnit = m_instanceRecipe.GetValue(labelTarget.TaskName.ToString()
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
                        , string.Empty);

                    strOld = m_instanceRecipe.GetValue(labelTarget.TaskName.ToString()
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE
                        , string.Empty);

                    break;
                case EN_RECIPE_TYPE.COMMON:
                case EN_RECIPE_TYPE.EQUIPMENT:
                    strMin = m_instanceRecipe.GetValue(labelTarget.ParameterType
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MIN
                        , string.Empty);

                    strMax = m_instanceRecipe.GetValue(labelTarget.ParameterType
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MAX
                        , string.Empty);

                    strUnit = m_instanceRecipe.GetValue(labelTarget.ParameterType
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
                        , string.Empty);

                    strOld = m_instanceRecipe.GetValue(labelTarget.ParameterType
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE
                        , string.Empty);

                    break;
			}

			// 2025.06.13 by junho [ADD] previous value 추가
			_previousValueStorage.Get(labelTarget.ParameterType, labelTarget.TaskName, labelTarget.ParameterName, out strPrevious, out strPreviousTime, string.Empty);
			#endregion

			// 2023.02.07 by junho [ADD] title 표시 할 수 있도록 기능 추가
			// if (m_instanceCalculator.CreateForm(labelTarget.Text, strMin, strMax, labelTarget.ParameterName, strUnit) == false)
			//if (m_instanceCalculator.CreateForm(strOld, strMin, strMax, strUnit) == false)
			if (m_instanceCalculator.CreateForm(strOld, strMin, strMax, strUnit, labelTarget.ParameterName, strPrevious, strPreviousTime) == false)
                return false;

            double dblResult = 0, dblMin = 0, dblMax = 0;
            m_instanceCalculator.GetResult(ref dblResult);

			bool isSetMinValue = m_instanceCalculator.GetMin(ref dblMin);
			bool isSetMaxValue = m_instanceCalculator.GetMax(ref dblMax);
			bool isSetUnitValue = m_instanceCalculator.GetUnit(ref strUnit);

			#region Set min, max, unit
			switch (labelTarget.ParameterType)
			{
				case EN_RECIPE_TYPE.PROCESS:
					if (isSetMinValue)
					{
						m_instanceRecipe.SetValue(labelTarget.TaskName.ToString()
							, labelTarget.ParameterName
							, labelTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MIN
							, dblMin.ToString());
					}
					if (isSetMaxValue)
					{
						m_instanceRecipe.SetValue(labelTarget.TaskName.ToString()
							, labelTarget.ParameterName
							, labelTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MAX
							, dblMax.ToString());
					}
					if (isSetUnitValue)
					{
						m_instanceRecipe.SetValue(labelTarget.TaskName.ToString()
							, labelTarget.ParameterName
							, labelTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
							, strUnit);
					}
					break;
				case EN_RECIPE_TYPE.COMMON:
				case EN_RECIPE_TYPE.EQUIPMENT:
					if (isSetMinValue)
					{
						m_instanceRecipe.SetValue(labelTarget.ParameterType
							, labelTarget.ParameterName
							, labelTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MIN
							, dblMin.ToString());
					}
					if (isSetMaxValue)
					{
						m_instanceRecipe.SetValue(labelTarget.ParameterType
							, labelTarget.ParameterName
							, labelTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MAX
							, dblMax.ToString());
					}
					if (isSetUnitValue)
					{
						m_instanceRecipe.SetValue(labelTarget.ParameterType
							, labelTarget.ParameterName
							, labelTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
							, strUnit);
					}
					break;
			}
			#endregion

			if(dblMin > dblResult || dblResult > dblMax)
			{
				m_instanceMessageBox.ShowMessage("Out of parameter min/max range");
				return false;
			}

			strResult = dblResult.ToString();


            return true;
        }
        #endregion

        #region Min Max Unit
        private void SetPropertyData(Component.CustomParameterLabel labelTarget)
        {
            string strMin = string.Empty;
            string strMax = string.Empty;
            string strUnit = string.Empty;
			string strDataType = string.Empty;
			string strDefaultValue = string.Empty;

            switch (labelTarget.ParameterType)
            #region 
            {
                case EN_RECIPE_TYPE.PROCESS:
                    strMin = m_instanceRecipe.GetValue(labelTarget.TaskName.ToString()
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MIN
                        , string.Empty);

                    strMax = m_instanceRecipe.GetValue(labelTarget.TaskName.ToString()
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MAX
                        , string.Empty);

                    strUnit = m_instanceRecipe.GetValue(labelTarget.TaskName.ToString()
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
                        , string.Empty);

					strDataType = m_instanceRecipe.GetValue(labelTarget.TaskName.ToString()
						, labelTarget.ParameterName
						, labelTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE
						, string.Empty);

					strDefaultValue = m_instanceRecipe.GetValue(labelTarget.TaskName.ToString()
						, labelTarget.ParameterName
						, labelTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE
						, string.Empty);

                    break;
                case EN_RECIPE_TYPE.EQUIPMENT:
                case EN_RECIPE_TYPE.COMMON:
                    strMin = m_instanceRecipe.GetValue(labelTarget.ParameterType
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MIN
                        , string.Empty);

                    strMax = m_instanceRecipe.GetValue(labelTarget.ParameterType
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MAX
                        , string.Empty);

                    strUnit = m_instanceRecipe.GetValue(labelTarget.ParameterType
                        , labelTarget.ParameterName
                        , labelTarget.ParameterIndex
                        , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
                        , string.Empty);

					strDataType = m_instanceRecipe.GetValue(labelTarget.ParameterType
						, labelTarget.ParameterName
						, labelTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE
						, string.Empty);

					strDefaultValue = m_instanceRecipe.GetValue(labelTarget.ParameterType
						, labelTarget.ParameterName
						, labelTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE
						, string.Empty);

                    break;
            }
            #endregion

            if (strMin.Equals(string.Empty))
			#region
			{
				if (false == labelTarget.ParameterMIN.Equals(string.Empty))
				{
					switch (labelTarget.ParameterType)
					{
						case EN_RECIPE_TYPE.PROCESS:
							m_instanceRecipe.SetValue(labelTarget.TaskName.ToString()
								, labelTarget.ParameterName
								, labelTarget.ParameterIndex
								, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MIN
								, labelTarget.ParameterMIN);

							break;
						case EN_RECIPE_TYPE.EQUIPMENT:
						case EN_RECIPE_TYPE.COMMON:
							m_instanceRecipe.SetValue(labelTarget.ParameterType
								, labelTarget.ParameterName
								, labelTarget.ParameterIndex
								, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MIN
								, labelTarget.ParameterMIN);

							break;
					}
				}
			}
			#endregion

			if (strMax.Equals(string.Empty))
			#region
			{
				if (false == labelTarget.ParameterMAX.Equals(string.Empty))
				{
					switch (labelTarget.ParameterType)
					{
						case EN_RECIPE_TYPE.PROCESS:
							m_instanceRecipe.SetValue(labelTarget.TaskName.ToString()
								, labelTarget.ParameterName
								, labelTarget.ParameterIndex
								, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MAX
								, labelTarget.ParameterMAX);

							break;
						case EN_RECIPE_TYPE.EQUIPMENT:
						case EN_RECIPE_TYPE.COMMON:
							m_instanceRecipe.SetValue(labelTarget.ParameterType
								, labelTarget.ParameterName
								, labelTarget.ParameterIndex
								, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MAX
								, labelTarget.ParameterMAX);

							break;
					}

				}
			}
			#endregion

			Recipe.EN_DATA_TYPE dataType;
			switch (labelTarget.ParameterSettingType)
			{
				case EN_LABEL_PARAMETER_TYPE.CALCULATE_DOUBLE:
					dataType = EN_DATA_TYPE.FLOAT8;
					break;
				case EN_LABEL_PARAMETER_TYPE.CALCULATE_INT:
					dataType = EN_DATA_TYPE.INT4;
					break;
				case EN_LABEL_PARAMETER_TYPE.CALCULATE_UINT:
					dataType = EN_DATA_TYPE.UINT4;
					break;
				default:
					dataType = EN_DATA_TYPE.ASCII;
					break;
			}

			if (strUnit.Equals(string.Empty))
            #region 
            {
                if (false == labelTarget.ParameterUNIT.Equals(string.Empty))
                {
                    switch (labelTarget.ParameterType)
                    {
                        case EN_RECIPE_TYPE.PROCESS:
                            m_instanceRecipe.SetValue(labelTarget.TaskName.ToString()
                                , labelTarget.ParameterName
                                , labelTarget.ParameterIndex
                                , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
								, dataType == EN_DATA_TYPE.ASCII ? "" : labelTarget.ParameterUNIT);

                            break;
                        case EN_RECIPE_TYPE.EQUIPMENT:
                        case EN_RECIPE_TYPE.COMMON:
                            m_instanceRecipe.SetValue(labelTarget.ParameterType
                                , labelTarget.ParameterName
                                , labelTarget.ParameterIndex
                                , FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
								, dataType == EN_DATA_TYPE.ASCII ? "" : labelTarget.ParameterUNIT);

                            break;
                    }

                    labelTarget.UseUnitFont = true;
                    labelTarget.UnitText = labelTarget.ParameterUNIT;
                }
                else
                {
                    labelTarget.UseUnitFont = false;
                }
            }
            else
            {
                labelTarget.UseUnitFont = true;
                labelTarget.UnitText = strUnit;
            }
            #endregion

			if (strDataType.Equals(string.Empty))
			#region
			{
				switch (labelTarget.ParameterType)
				{
					case EN_RECIPE_TYPE.PROCESS:
						m_instanceRecipe.SetValue(labelTarget.TaskName.ToString()
							, labelTarget.ParameterName
							, labelTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE
							, dataType.ToString());

						break;
					case EN_RECIPE_TYPE.EQUIPMENT:
					case EN_RECIPE_TYPE.COMMON:
						m_instanceRecipe.SetValue(labelTarget.ParameterType
							, labelTarget.ParameterName
							, labelTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE
							, dataType.ToString());

						break;
				}
			}
			#endregion

			if (strDefaultValue.Equals(string.Empty))
			#region
			{
				if (false == labelTarget.ParameterDefaultValue.Equals(string.Empty))
				{
					switch (labelTarget.ParameterType)
					{
						case EN_RECIPE_TYPE.PROCESS:
							m_instanceRecipe.SetValue(labelTarget.TaskName.ToString()
								, labelTarget.ParameterName
								, labelTarget.ParameterIndex
								, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE
								, labelTarget.ParameterDefaultValue);

							break;
						case EN_RECIPE_TYPE.EQUIPMENT:
						case EN_RECIPE_TYPE.COMMON:
							m_instanceRecipe.SetValue(labelTarget.ParameterType
								, labelTarget.ParameterName
								, labelTarget.ParameterIndex
								, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE
								, labelTarget.ParameterDefaultValue);

							break;
					}

				}
			}
			#endregion
        }

		const string def_ToggleDefaultMin = "0";
		const string def_ToggleDefaultMax = "1";
		const string def_ToggleDefaultUnit = "Boolean";
		private void SetPropertyData(Component.CustomParameterToggleButton toggleTarget)
		{
			string strMin = string.Empty;
			string strMax = string.Empty;
			string strUnit = string.Empty;
			string strDataType = string.Empty;
			string strDefaultValue = string.Empty;

			switch (toggleTarget.ParameterType)
			#region
			{
				case EN_RECIPE_TYPE.PROCESS:
					strMin = m_instanceRecipe.GetValue(toggleTarget.TaskName.ToString()
						, toggleTarget.ParameterName
						, toggleTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MIN
						, string.Empty);

					strMax = m_instanceRecipe.GetValue(toggleTarget.TaskName.ToString()
						, toggleTarget.ParameterName
						, toggleTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MAX
						, string.Empty);

					strUnit = m_instanceRecipe.GetValue(toggleTarget.TaskName.ToString()
						, toggleTarget.ParameterName
						, toggleTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
						, string.Empty);

					strDataType = m_instanceRecipe.GetValue(toggleTarget.TaskName.ToString()
						, toggleTarget.ParameterName
						, toggleTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE
						, string.Empty);

					strDefaultValue = m_instanceRecipe.GetValue(toggleTarget.TaskName.ToString()
						, toggleTarget.ParameterName
						, toggleTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE
						, string.Empty);

					break;
				case EN_RECIPE_TYPE.EQUIPMENT:
				case EN_RECIPE_TYPE.COMMON:
					strMin = m_instanceRecipe.GetValue(toggleTarget.ParameterType
						, toggleTarget.ParameterName
						, toggleTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MIN
						, string.Empty);

					strMax = m_instanceRecipe.GetValue(toggleTarget.ParameterType
						, toggleTarget.ParameterName
						, toggleTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MAX
						, string.Empty);

					strUnit = m_instanceRecipe.GetValue(toggleTarget.ParameterType
						, toggleTarget.ParameterName
						, toggleTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
						, string.Empty);

					strDataType = m_instanceRecipe.GetValue(toggleTarget.ParameterType
						, toggleTarget.ParameterName
						, toggleTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE
						, string.Empty);

					strDefaultValue = m_instanceRecipe.GetValue(toggleTarget.ParameterType
						, toggleTarget.ParameterName
						, toggleTarget.ParameterIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE
						, string.Empty);

					break;
			}
			#endregion

			if (strMin.Equals(string.Empty))
			#region
			{
				switch (toggleTarget.ParameterType)
				{
					case EN_RECIPE_TYPE.PROCESS:
						m_instanceRecipe.SetValue(toggleTarget.TaskName.ToString()
							, toggleTarget.ParameterName
							, toggleTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MIN
							, def_ToggleDefaultMin);

						break;
					case EN_RECIPE_TYPE.EQUIPMENT:
					case EN_RECIPE_TYPE.COMMON:
						m_instanceRecipe.SetValue(toggleTarget.ParameterType
							, toggleTarget.ParameterName
							, toggleTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MIN
							, def_ToggleDefaultMin);

						break;
				}
			}
			#endregion

			if (strMax.Equals(string.Empty))
			#region
			{
				switch (toggleTarget.ParameterType)
				{
					case EN_RECIPE_TYPE.PROCESS:
						m_instanceRecipe.SetValue(toggleTarget.TaskName.ToString()
							, toggleTarget.ParameterName
							, toggleTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MAX
							, def_ToggleDefaultMax);

						break;
					case EN_RECIPE_TYPE.EQUIPMENT:
					case EN_RECIPE_TYPE.COMMON:
						m_instanceRecipe.SetValue(toggleTarget.ParameterType
							, toggleTarget.ParameterName
							, toggleTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.MAX
							, def_ToggleDefaultMax);

						break;
				}
			}
			#endregion

			if (strUnit.Equals(string.Empty))
			#region
			{
				switch (toggleTarget.ParameterType)
				{
					case EN_RECIPE_TYPE.PROCESS:
						m_instanceRecipe.SetValue(toggleTarget.TaskName.ToString()
							, toggleTarget.ParameterName
							, toggleTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
							, def_ToggleDefaultUnit);

						break;
					case EN_RECIPE_TYPE.EQUIPMENT:
					case EN_RECIPE_TYPE.COMMON:
						m_instanceRecipe.SetValue(toggleTarget.ParameterType
							, toggleTarget.ParameterName
							, toggleTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.UNIT
							, def_ToggleDefaultUnit);

						break;
				}
			}
			#endregion

			if (strDataType.Equals(string.Empty))
			#region
			{
				switch (toggleTarget.ParameterType)
				{
					case EN_RECIPE_TYPE.PROCESS:
						m_instanceRecipe.SetValue(toggleTarget.TaskName.ToString()
							, toggleTarget.ParameterName
							, toggleTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE
							, Recipe.EN_DATA_TYPE.BOOL.ToString());

						break;
					case EN_RECIPE_TYPE.EQUIPMENT:
					case EN_RECIPE_TYPE.COMMON:
						m_instanceRecipe.SetValue(toggleTarget.ParameterType
							, toggleTarget.ParameterName
							, toggleTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DATA_TYPE
							, Recipe.EN_DATA_TYPE.BOOL.ToString());

						break;
				}
			}
			#endregion

			if (strDefaultValue.Equals(string.Empty))
			#region
			{
				switch (toggleTarget.ParameterType)
				{
					case EN_RECIPE_TYPE.PROCESS:
						m_instanceRecipe.SetValue(toggleTarget.TaskName.ToString()
							, toggleTarget.ParameterName
							, toggleTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE
							, default(bool).ToString());

						break;
					case EN_RECIPE_TYPE.EQUIPMENT:
					case EN_RECIPE_TYPE.COMMON:
						m_instanceRecipe.SetValue(toggleTarget.ParameterType
							, toggleTarget.ParameterName
							, toggleTarget.ParameterIndex
							, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE
							, default(bool).ToString());

						break;
				}
			}
			#endregion

		}
        #endregion

        #endregion

        /// <summary>
        /// 2021.06.14. by shkim. [ADD] 파라미터 변경 가능한 상태 확인
        /// </summary>
        /// <returns></returns>
        private bool ChangeableStateForParameter()
        {
            EquipmentState_.EQUIPMENT_STATE equipmentState = EquipmentState_.EquipmentState.GetInstance().GetState();
            return (equipmentState == EquipmentState_.EQUIPMENT_STATE.PAUSE || equipmentState == EquipmentState_.EQUIPMENT_STATE.IDLE);
        }
        private void ChangeParameter(object sender, EventArgs e)
        {
			// 2021.11.12 by junho [ADD] 설비 가동중 파라미터 변경 가능 옵션 추가
			if (sender is CustomParameterLabel)
			{
				bool isUnlock = m_instanceRecipe.GetValue(EN_RECIPE_TYPE.EQUIPMENT, Recipe.PARAM_EQUIPMENT.UnlockParameterChange.ToString(), 0, EN_RECIPE_PARAM_TYPE.VALUE, false);

				if (false == ChangeableStateForParameter() && false == isUnlock)
					return;
			}
			else
			{
				// 설비 가동 중 파라미터 변경 금지
				if (false == ChangeableStateForParameter())
				{
					if (sender is CustomParameterToggleButton)
					{
						GetParameter(sender as CustomParameterToggleButton);
					}
					return;
				}
			}

            if (sender is CustomParameterLabel)
            {
                CustomParameterLabel label = (CustomParameterLabel)sender;
                if (SetParameter(label))
                {
                    if (label.NeedRemakeMap)
                    {
                        Define.DefineEnumProject.Map.EN_MAP_TYPE targetMap;
                        if (Enum.TryParse<Define.DefineEnumProject.Map.EN_MAP_TYPE>(label.AssociatedMap, out targetMap))
                        {
							// Map을 다시 그려야 할 경우 여기서 처리
                        }

						if (label.delAfterSetValue != null)
							label.delAfterSetValue(label.ParameterType, label.TaskName, label.ParameterName);
                    }
                }
            }
            else if (sender is CustomParameterButton)
            {
                CustomParameterButton btn = (CustomParameterButton)sender;
                if (SetParameter(btn))
                {
                    if (btn.NeedRemakeMap)
                    {
                        Define.DefineEnumProject.Map.EN_MAP_TYPE targetMap;
                        if (Enum.TryParse<Define.DefineEnumProject.Map.EN_MAP_TYPE>(btn.AssociatedMap, out targetMap))
                        {
							// Map을 다시 그려야 할 경우 여기서 처리
                        }

						if (btn.delAfterSetValue != null)
							btn.delAfterSetValue(btn.ParameterType, btn.TaskName, btn.ParameterName);
                    }
                }
            }
            else if (sender is CustomParameterToggleButton)
            {
                CustomParameterToggleButton btn = (CustomParameterToggleButton)sender;
                if (SetParameter(btn))
                {
                    btn.InformParameterValueChanged(btn, btn.Active);

                    if (btn.NeedRemakeMap)
                    {
                        Define.DefineEnumProject.Map.EN_MAP_TYPE targetMap;
                        if (Enum.TryParse<Define.DefineEnumProject.Map.EN_MAP_TYPE>(btn.AssociatedMap, out targetMap))
                        {
							// Map을 다시 그려야 할 경우 여기서 처리
                        }

						if (btn.delAfterSetValue != null)
							btn.delAfterSetValue(btn.ParameterType, btn.TaskName, btn.ParameterName);
                    }
                }
				else
				{
					// 2024.06.25 by junho [ADD] set parameter 실패 했을 때에는 다시 돌려줘야 한다.
					btn.Active = !btn.Active;
				}
            }
			else if(sender is CustomParameterLedLabel)
			{
				CustomParameterLedLabel ledLabel  = (CustomParameterLedLabel)sender;
				if(SetParameter(ledLabel))
				{
					ledLabel.InformParameterValueChanged(ledLabel, ledLabel.Active);
				}
				else
				{
					//ledLabel.Active = !ledLabel.Active;
				}
			}
        }

        private void OpenCustomJog(object sender, EventArgs e)
        {
            CustomJogButton jogButton = (CustomJogButton)sender;
			string recipeKey1, recipeKey2;

			if (jogButton.ParameterType1 == EN_RECIPE_TYPE.PROCESS)
				recipeKey1 = jogButton.TaskName1;
			else
				recipeKey1 = jogButton.ParameterType1.ToString();


			if (jogButton.ParameterType2 == EN_RECIPE_TYPE.PROCESS)
				recipeKey2 = jogButton.TaskName2;
			else
				recipeKey2 = jogButton.ParameterType2.ToString();

            FrameOfSystem3.Views.Functional.Jog.Form_Jog.GetInstance().CreateForm(
                jogButton.JogTitle,
				recipeKey1,
                jogButton.ParameterName1,
                jogButton.AxisIndex1,
				recipeKey2,
                jogButton.ParameterName2,
                jogButton.AxisIndex2);
        }

		private void DoTargetAction(object sender, EventArgs e)
		{
			CustomActionButton actionButton = (CustomActionButton)sender;

			string[] task = new string[] { actionButton.TaskName };
			string[] action = new string[] { actionButton.ActionName };

			m_strErrorMessage = String.Format("Do you want This Action START? \\n TASK NAME : {0} \\n ACTION NAME : {1}", actionButton.TaskName, actionButton.ActionName);
			if (false == m_instanceMessageBox.ShowMessage(m_strErrorMessage))
				return;

			bool rtn = Task.TaskOperator.GetInstance().SetOperation(ref task, ref action);

			if(rtn)
			{

			}
		}

		/// <summary>
		/// 2024.03.01 by junho [ADD] discription이 설정되어 있을 경우 tooltip을 띄운다.
		/// </summary>
		private void ShowDiscriptionTooltip(object sender, EventArgs e)
		{
			EN_RECIPE_TYPE paraType;
			string taskName, paraName;
			int paraIndex;
			Action<string> showTooltip;

			if (sender is CustomParameterLabel)
			{
				CustomParameterLabel label = (CustomParameterLabel)sender;
				paraType = label.ParameterType;
				taskName = label.TaskName.ToString();
				paraName = label.ParameterName;
				paraIndex = label.ParameterIndex;
				showTooltip = label.ShowDescriptionOnTooltip;

			}
			else if (sender is CustomParameterToggleButton)
			{
				CustomParameterToggleButton toggle = (CustomParameterToggleButton)sender;
				paraType = toggle.ParameterType;
				taskName = toggle.TaskName.ToString();
				paraName = toggle.ParameterName;
				paraIndex = toggle.ParameterIndex;
				showTooltip = toggle.ShowDescriptionOnTooltip;
			}
			else return;

			if (false == IsCorrectiveName(taskName, paraName, paraType))
				return;

			string description;
			switch (paraType)
			{
				case EN_RECIPE_TYPE.PROCESS:
					description = m_instanceRecipe.GetValue(taskName
						, paraName
						, paraIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DESCRIPTION
						, string.Empty);
					break;
				case EN_RECIPE_TYPE.EQUIPMENT:
				case EN_RECIPE_TYPE.COMMON:
					description = m_instanceRecipe.GetValue(paraType
						, paraName
						, paraIndex
						, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.DESCRIPTION
						, string.Empty);
					break;
				default: return;
			}

			showTooltip(description);
		}
	}
}
