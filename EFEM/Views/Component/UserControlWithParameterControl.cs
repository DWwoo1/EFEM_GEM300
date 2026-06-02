using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FrameOfSystem3.Views;

namespace FrameOfSystem3.Component
{
    public partial class UserControlWithParameterControl : UserControlForMainView.CustomView
    {
        public UserControlWithParameterControl()
        {
            InitializeComponent();

            foreach (var control in this.Controls)
            {
                if (control is CustomParameterLabel)
                {
                    CustomParameterLabel paraLabel = (CustomParameterLabel)control;
                    paraLabel.ParameterName = paraLabel.Name;
                    dicOfParameterLabel.Add(paraLabel, paraLabel.NeedRemakeMap);
                    paraLabel.Click += ChangeParameter;
                }
                if (control is CustomParameterButton)
                {
                    CustomParameterButton paraBtn = (CustomParameterButton)control;
                    dicOfParameterButton.Add(paraBtn, paraBtn.NeedRemakeMap);
                    paraBtn.Click += ChangeParameter;
                }
//                 if (control is CustomParameterToggleButton)
//                 {
//                     CustomParameterToggleButton paraTg = (CustomParameterToggleButton)control;
//                     paraTg.ParameterName = paraTg.Name;
//                     dicOfParameterToggle.Add(paraTg, paraTg.NeedRemakeMap);
//                     paraTg.ActiveChanged += Clicked_ParameterToggle;
//                 }
            }
        }
        
        private ControlInterface parameterCtl = new ControlInterface();
        // private Task.TaskOperator instanceTaskOperator = null;

        // protected FrameOfSystem3.Recipe.Recipe instanceRecipe = FrameOfSystem3.Recipe.Recipe.GetInstance();
        protected Views.Functional.Form_MessageBox instanceMessageBox = null;

        private Dictionary<CustomParameterLabel, bool> dicOfParameterLabel = new Dictionary<CustomParameterLabel, bool>();
        private Dictionary<CustomParameterButton, bool> dicOfParameterButton = new Dictionary<CustomParameterButton, bool>();
        private Dictionary<CustomParameterToggleButton, bool> dicOfParameterToggle = new Dictionary<CustomParameterToggleButton, bool>();

        protected void RefreshValueParameter()
        {
            foreach (var pLabel in dicOfParameterLabel)
            {
                CustomParameterLabel label = pLabel.Key;
                parameterCtl.GetParameter(label);
            }
        }
        protected void ActivateControls()
        {
            instanceMessageBox = Views.Functional.Form_MessageBox.GetInstance();
            
            foreach (var pTgBtn in dicOfParameterToggle)
            {
                CustomParameterToggleButton tgBtn = pTgBtn.Key;
                parameterCtl.GetParameter(tgBtn);
            }
        }

        private void ChangeParameter(object sender, EventArgs e)
        {
            // 설비 가동 중 파라미터 변경 금지
            if (false == ChangeableStateForParameter()) return;
            
            if(sender is CustomParameterLabel)
            {
                CustomParameterLabel label = (CustomParameterLabel)sender;
                if (parameterCtl.SetParameter(label))
                {
                    if (label.NeedRemakeMap)
                    {
                        Define.DefineEnumeration.Map.EN_MAP_LIST targetMap;
                        if (Enum.TryParse<Define.DefineEnumeration.Map.EN_MAP_LIST>(label.AssociatedMap, out targetMap))
                        {
                            Work.WorkInformation.GetInstance().MakeMapInformation(targetMap);
                        }
                    }
                }
            }
            else if (sender is CustomParameterButton)
            {
                CustomParameterButton btn = (CustomParameterButton)sender;
                if (parameterCtl.SetParameter(btn))
                {
                    if (btn.NeedRemakeMap)
                    {
                        Define.DefineEnumeration.Map.EN_MAP_LIST targetMap;
                        if (Enum.TryParse<Define.DefineEnumeration.Map.EN_MAP_LIST>(btn.AssociatedMap, out targetMap))
                        {
                            Work.WorkInformation.GetInstance().MakeMapInformation(targetMap);
                        }
                    }
                }
            }
            else if (sender is CustomParameterToggleButton)
            {
                CustomParameterButton btn = (CustomParameterButton)sender;
                if (parameterCtl.SetParameter(btn))
                {
                    if (btn.NeedRemakeMap)
                    {
                        Define.DefineEnumeration.Map.EN_MAP_LIST targetMap;
                        if (Enum.TryParse<Define.DefineEnumeration.Map.EN_MAP_LIST>(btn.AssociatedMap, out targetMap))
                        {
                            Work.WorkInformation.GetInstance().MakeMapInformation(targetMap);
                        }
                    }
                }
            }
        }

        
    }
}
