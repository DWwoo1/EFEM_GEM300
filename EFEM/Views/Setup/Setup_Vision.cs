using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Define.DefineEnumProject.Vision;
using Define.DefineEnumProject.SelectionList;

using Vision_;

using FrameOfSystem3.Component;
using FrameOfSystem3.Controller.Vision;

namespace FrameOfSystem3.Views.Setup
{
	public partial class Setup_Vision : UserControlForMainView.CustomView
	{
		#region Construct
		public Setup_Vision()
		{
			InitializeComponent();

			#region TabIndex
			_Set_SetUpMode_Button.TabIndex	= (int)EN_TABINDEX_BUTTON.SET_SETUP_MODE;
			_Change_Scene_Button.TabIndex	= (int)EN_TABINDEX_BUTTON.CHANGE_SCENE;
			_Set_RunMode_Button.TabIndex	= (int)EN_TABINDEX_BUTTON.SET_RUN_MODE;
			_Inspection_Button.TabIndex		= (int)EN_TABINDEX_BUTTON.INSPECTION;
			_Set_Recipe_Button.TabIndex		= (int)EN_TABINDEX_BUTTON.SET_RECIPE;
			_Get_Recipe_Button.TabIndex		= (int)EN_TABINDEX_BUTTON.GET_RECIPE;
			_Gage_Rnr_Button.TabIndex		= (int)EN_TABINDEX_BUTTON.GAGE_RNR;
			_Connect_button.TabIndex		= (int)EN_TABINDEX_BUTTON.CONNECT;
			_Show_Button.TabIndex			= (int)EN_TABINDEX_BUTTON.SHOW;
			_Hide_Button.TabIndex			= (int)EN_TABINDEX_BUTTON.HIDE;
			_Grab_Button.TabIndex			= (int)EN_TABINDEX_BUTTON.GRAB;

			_Selected_Cam_Number_Label.TabIndex	= (int)EN_TABINDEX_LABEL.CAM;
			_Selected_Algorithm_Label.TabIndex	= (int)EN_TABINDEX_LABEL.ALGORITHM;
			_Inspection_Mode_Label.TabIndex		= (int)EN_TABINDEX_LABEL.INSPECTION_MODE;
			#endregion

			#region Instance
			_MessageBox_Instance_m_p	= Functional.Form_MessageBox.GetInstance();
			_Selection_Instance_m_p		= Functional.Form_SelectionList.GetInstance();
			_Recipe_Instance_m_p		= FrameOfSystem3.Recipe.Recipe.GetInstance();
			_Vision_Instance_m_p		= Vision_.Vision.GetInstance();
			_Socket_Instance_m_p		= FrameOfSystem3.Config.ConfigSocket.GetInstance();
            _Control_Interface          = new ControlInterface();
			#endregion

			#region ComboBox Initialize
			string[] arComboBoxList = Enum.GetNames(typeof(EN_CAMERA_LIST));

			_ComboBox_4Point_Cam.Items.AddRange(arComboBoxList);
			_ComboBox_4Point_Cam.SelectedIndex = (int)EN_CAMERA_LIST.CAM1;
			#endregion

            #region Recipe
            _Control_Interface.AssignControls(this.Controls);
            #endregion


		}
		#endregion

		#region Constants
		private readonly string RECIPE_NAME_DEFAULT	= "RECIPE";

		private const int SELECT_NONE	= -1;
		#endregion

		#region Enum
		private enum EN_TABINDEX_BUTTON
		{
			CONNECT = 0,
			SET_RECIPE,
			GET_RECIPE,
			SET_RUN_MODE,
			SET_SETUP_MODE,
			GRAB,
			CHANGE_SCENE,
			INSPECTION,
			SHOW,
			HIDE,
			GAGE_RNR,
			EXCUTE_DISTORTION_CALIBRAITON,
		}
		private enum EN_TABINDEX_LABEL
		{
			CAM			= 0,
			ALGORITHM,
			INSPECTION_MODE,
		}
		#endregion
		
		#region Variable
        private Define.DefineEnumProject.Task.EN_TASK_LIST m_enTaskName = Define.DefineEnumProject.Task.EN_TASK_LIST.Sample;
		
		private int _Selected_Grab_Cam_Index_m_n					= -1;
		private int _Selected_Grab_Algorithm_m_n					= -1;

		private int _Selected_4PointCal_Cam_Index_m_n				= -1;
		private int _Selected_4PointCal_Algorithm_m_n				= -1;

		#region Instance
		private Functional.Form_SelectionList _Selection_Instance_m_p	= null;
		private Functional.Form_MessageBox _MessageBox_Instance_m_p		= null;
		private Vision_.Vision _Vision_Instance_m_p						= null;
		private FrameOfSystem3.Recipe.Recipe _Recipe_Instance_m_p		= null;
		private FrameOfSystem3.Config.ConfigSocket _Socket_Instance_m_p	= null;
        private ControlInterface _Control_Interface                     = null;
		#endregion

		#region Panel
		#endregion

		#endregion

		#region Override Interface
		protected override void ProcessWhenActivation()
		{
			if(_Selected_Grab_Cam_Index_m_n == SELECT_NONE || _Selected_Grab_Algorithm_m_n == SELECT_NONE)
			{
				_Selected_Cam_Number_Label.Text	= EN_CAMERA_LIST.CAM1.ToString();
				_Selected_Grab_Cam_Index_m_n = (int)EN_CAMERA_LIST.CAM1;

				_Selected_Algorithm_Label.Text = EN_VISION_ALGORITHM.SAMPLE.ToString();
				_Selected_Grab_Algorithm_m_n = (int)EN_VISION_ALGORITHM.SAMPLE;
			}

		}
		protected override void ProcessWhenDeactivation()
		{
		}
		public override void CallFunctionByTimer()
		{
			UpdateGUIPage();

            _Control_Interface.RefreshValueParameter();

		}
		#endregion

		#region INTERNAL
		private void UpdateGUIPage()
		{
			_Controller_State_Label.Text	= _Socket_Instance_m_p.GetState((int)Define.DefineEnumProject.Socket.EN_SOCKET_INDEX.VISION).ToString();

            string strFileName = string.Empty;
          

			_Recipe_Instance_m_p.GetProcessFileNameWithoutExtension(ref strFileName);

//             strFileName = _Recipe_Instance_m_p.GetValue(Define.DefineEnumProject.Task.EN_TASK_LIST.BOND_HEAD.ToString()
//                                                           , Define.DefineEnumProject.Task.BondHead.PARAM_PROCESS.VISION_RECIPE_NAME.ToString()
//                                                           , 0, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE, strFileName);
			_Selected_Recipe_Label.Text		= strFileName;
		}

		private void SetAlgorithmData()
		{
			if(false == Enum.IsDefined(typeof(EN_CAMERA_LIST), _Selected_Grab_Cam_Index_m_n))
				return;

			EN_CAMERA_LIST enCam	= (EN_CAMERA_LIST)_Selected_Grab_Cam_Index_m_n;

			string strTitleName	= string.Format("{0} : {1}", labelAlgorithm.Text, enCam);

			switch(enCam)
			{
				case EN_CAMERA_LIST.CAM1:
// 					if(false == _Selection_Instance_m_p.CreateForm(strTitleName
// 						, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.DISPENSER_DLC_ALGORITHM
// 						, _Selected_Algorithm_Label.Text)) return;
					break;

				default:
					return;
			}

			string strResult	= string.Empty;

			_Selection_Instance_m_p.GetResult(ref _Selected_Grab_Algorithm_m_n);
			_Selection_Instance_m_p.GetResult(ref strResult);

			_Selected_Algorithm_Label.Text		= strResult;
			
		}

		private void DataLabel_ClickEvent(EN_TABINDEX_LABEL enClicked)
		{
			switch(enClicked)
			{
				case EN_TABINDEX_LABEL.ALGORITHM:
					SetAlgorithmData();
					break;
				case EN_TABINDEX_LABEL.CAM:
					if (_Selection_Instance_m_p.CreateForm(enClicked.ToString(), Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.VISION_CAMERA_LIST, _Selected_Cam_Number_Label.Text))
					{
						string strCamName	= string.Empty;

						_Selection_Instance_m_p.GetResult(ref _Selected_Grab_Cam_Index_m_n);
						_Selection_Instance_m_p.GetResult(ref strCamName);

						_Selected_Cam_Number_Label.Text	= strCamName;

						SetAlgorithmData();
					}
					break;
				case EN_TABINDEX_LABEL.INSPECTION_MODE:
					break;
				
				default:
					return;
			}
		}

		private void ChangeCalibrationView(EN_CAMERA_LIST enCam)
		{
		
		}

		private async void CommandButton_ClickEvent(EN_TABINDEX_BUTTON enClicked)
		{
			RequestDataOfProtecVision requestAdditionalData = null;

			Task<VISION_RESULT> taskResult	= null;
			this.Enabled					= false;

			switch(enClicked)
			{
				case EN_TABINDEX_BUTTON.EXCUTE_DISTORTION_CALIBRAITON:
                    break;
				case EN_TABINDEX_BUTTON.SET_SETUP_MODE:
					taskResult	= _Vision_Instance_m_p.SetRunModeAsync(false);
					break;
				case EN_TABINDEX_BUTTON.CHANGE_SCENE:
					taskResult	= _Vision_Instance_m_p.ChangeSceneAsync(_Selected_Grab_Cam_Index_m_n, _Selected_Grab_Algorithm_m_n);
					break;
				case EN_TABINDEX_BUTTON.SET_RUN_MODE:
					taskResult	= _Vision_Instance_m_p.SetRunModeAsync(true);
					break;
				case EN_TABINDEX_BUTTON.INSPECTION:
					requestAdditionalData	= new RequestDataOfProtecVision(0, null);
					taskResult	= _Vision_Instance_m_p.ExecuteInspectionAsync(_Selected_Grab_Cam_Index_m_n, requestAdditionalData);
					break;
				case EN_TABINDEX_BUTTON.GET_RECIPE:
					taskResult	= _Vision_Instance_m_p.GetCurrentRecipeAsync(0);
					break;
				case EN_TABINDEX_BUTTON.SET_RECIPE:
					{
                        OpenFileDialog fDialog = new OpenFileDialog();

//                         string RecipePath = _Recipe_Instance_m_p.GetValue(FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT
//                                                          , FrameOfSystem3.Recipe.PARAM_EQUIPMENT.VISION_RECIPE_PATH.ToString()
//                                                          , 0, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE, "");
                        string RecipePath = "";

                        fDialog.InitialDirectory = RecipePath;
                        fDialog.Filter = "Vision Recipe files (*.cm1)|*.cm1";
                        
                        switch (fDialog.ShowDialog())
                        {
                            case DialogResult.OK:
                                string recipeName = fDialog.FileName;

                                recipeName = System.IO.Path.GetFileNameWithoutExtension(recipeName);
//                                 _Recipe_Instance_m_p.SetValue(Define.DefineEnumProject.Task.EN_TASK_LIST.BOND_HEAD.ToString()
//                                                                 , Define.DefineEnumProject.Task.BondHead.PARAM_PROCESS.VISION_RECIPE_NAME.ToString()
//                                                                 , 0, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE, recipeName);
                                taskResult = _Vision_Instance_m_p.LoadRecipeAsync(recipeName);
                                break;
                            default:
                                break;
                        }
					
					}
					break;
				case EN_TABINDEX_BUTTON.GRAB:
					requestAdditionalData	= new RequestDataOfProtecVision(0, null);
					taskResult	= _Vision_Instance_m_p.GrabImageAsync(_Selected_Grab_Cam_Index_m_n, requestAdditionalData);
					break;
				case EN_TABINDEX_BUTTON.HIDE:
					taskResult	= _Vision_Instance_m_p.SetShowHideAsync(false);
					break;
				case EN_TABINDEX_BUTTON.SHOW:
					taskResult	= _Vision_Instance_m_p.SetShowHideAsync(true);
					break;

				default:
					break;
			}

			if(taskResult != null)
			{
				await taskResult;
				_MessageBox_Instance_m_p.ShowMessage(string.Format("{0} : {1}", enClicked, taskResult.Result));
			}

			this.Enabled	= true;
		}
		#endregion

		#region GUI EVENT
		private void Click_CommandButton(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			CommandButton_ClickEvent((EN_TABINDEX_BUTTON)ctrl.TabIndex);
		}
		/// <summary>
		/// 2021.12.22 by twkang [ADD] 데이터 라벨 클릭 이벤트
		/// </summary>
		private void Click_DataLabel(object sender, EventArgs e)
		{
			Control ctrl	 = sender as Control;

			DataLabel_ClickEvent((EN_TABINDEX_LABEL)ctrl.TabIndex);
		}
		/// <summary>
		/// 2021.01.03 by twkang [ADD] 
		/// </summary>
		private void Chage_ComboBoxValue(object sender, EventArgs e)
		{
            ChangeCalibrationView((EN_CAMERA_LIST)_ComboBox_4Point_Cam.SelectedIndex);
		}
		#endregion

        private void _Selected_Recipe_Label_Click(object sender, EventArgs e)
        {

        }
	}
}
