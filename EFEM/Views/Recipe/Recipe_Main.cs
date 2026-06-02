using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

using System.Threading;

namespace FrameOfSystem3.Views.Recipe
{
    public partial class Recipe_Main : UserControlForMainView.CustomView
    {
        #region 상수
        private const string COPY_TOKEN                      = "_copy";

		#region GUI

		#region GridView Column Index
		private const int COLUMN_INDEX_OF_INDEX				= 0;
		private const int COLUMN_INDEX_OF_NAME				= 1;
		private const int COLUMN_INDEX_OF_TIME				= 2;
		#endregion

		#region Delay
		private const int FILE_IO_DELAY						= 100;
		#endregion

		#endregion

		#endregion

		#region 변수

		#region Recipe
		private string m_strCurruntLoadedRecipe				= string.Empty;
		#endregion

		#region Instance
		private FrameOfSystem3.Recipe.Recipe m_instanceRecipe		= null;
		private Functional.Form_MessageBox m_InstanceOfMessageBox	= null;
		private Functional.Form_Keyboard  m_instanceKeyboard        = null;
		#endregion

		string _filteringKeyword = string.Empty;
		string _currentFilePath = string.Empty;
		
        #endregion

        public Recipe_Main()
        {
            InitializeComponent();

			#region Instance
			m_instanceRecipe = FrameOfSystem3.Recipe.Recipe.GetInstance();
			m_instanceKeyboard = Views.Functional.Form_Keyboard.GetInstance();
			m_InstanceOfMessageBox = Functional.Form_MessageBox.GetInstance();
			#endregion
		}
		
        #region 상속 인터페이스
        /// <summary>
        /// 2020.07.03 by yjlee [ADD] It will be called when the form is activated.
        /// </summary>
        protected override void ProcessWhenActivation()
        {
			ResetControl();

			string strProcessFilePath	= string.Empty;
			string strProcessFileName	= string.Empty;

            if(m_instanceRecipe.GetProcessFileInformation(ref strProcessFilePath, ref strProcessFileName))
			{
				m_strCurruntLoadedRecipe	= Path.GetFileNameWithoutExtension(strProcessFileName);

				UpdateGroupView(strProcessFilePath);
				UpdateFileView(strProcessFilePath);
			}
			else
			{
				m_btnPrevious.Enabled	= false;
				m_btnGCreate.Enabled	= false;
				m_btnRCreate.Enabled	= false;
			}

			// 2022.06.08 by junho [ADD] Update vision use option
			//bool isUseVision = FrameOfSystem3.Recipe.Recipe.GetInstance().GetValue(
			//	FrameOfSystem3.Recipe.EN_RECIPE_TYPE.EQUIPMENT
			//	, FrameOfSystem3.Recipe.PARAM_EQUIPMENT.Use_Vision.ToString()
			//	, 0
			//	, FrameOfSystem3.Recipe.EN_RECIPE_PARAM_TYPE.VALUE
			//	, false);

			// 2025.03.27 by junho [ADD] Dry run/Simulation mode 일 때 vision 사용 옵션 항상 무시하도록 변경
			//if (Task.TaskOperator.GetInstance().IsDryRunOrSimulationMode())
			//	isUseVision = false;

			// 2022.06.08 by junho [ADD] Update vision use option
			//Vision_.Vision.GetInstance().SetUseVision(isUseVision);
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessWhenDeactivation()
        {

        }
        /// <summary>
        /// 2020.06.02 by twkang [ADD] 타이머에 의해 호출되는 함수이다. 폴더와 파일의 변경사항을 업데이트한다.
        /// </summary>
        public override void CallFunctionByTimer()
        {

        }
        #endregion

		#region Internal Interface

		#region GUI
		/// <summary>
		/// 2021.06.18 by twkang [ADD] Page 초기화
		/// </summary>
		private void ResetControl()
		{
			m_dgviewFile.Rows.Clear();
			m_dgviewGroup.Rows.Clear();

			ResetFolderLabel();
			ResetFileLabel();

			ActiveFileControls(false);
			ActiveFolderControls(false);

			_filteringKeyword = string.Empty;
			lbl_Filtering.Text = _filteringKeyword;
		}
		/// <summary>
		/// 2021.06.18 by twkang [ADD] 폴더 관련 라벨 초기환
		/// </summary>
		private void ResetFolderLabel()
		{
			m_labelFullPath.Text = "--";
			m_labelGFolder.Text = "--";
		}
		/// <summary>
		/// 2021.06.18 by twkang [ADD] 파일 관련 라벨 초기화
		/// </summary>
		private void ResetFileLabel()
		{
			m_labelRFile.Text = "--";
			m_labelSelectedRecipe.Text = "--";
		}

		/// <summary>
		/// 2021.06.18 by twkang [ADD] 폴더관련 컨트롤 상태 설정
		/// </summary>
		private void ActiveFolderControls(bool bActive)
		{
			m_btnGCreate.Enabled	= true;
			m_btnPrevious.Enabled	= true;
			m_btnGCopy.Enabled		= bActive;
			m_btnGModify.Enabled	= bActive;
			m_btnGRemove.Enabled	= bActive;
			m_btnEnter.Enabled		= bActive;
		}
		/// <summary>
		/// 2021.06.18 by twkang [ADD] 파일관련 컨트롤 상태 설정
		/// </summary>
		private void ActiveFileControls(bool bActive)
		{
			m_btnRCreate.Enabled	= true;
			m_btnRCopy.Enabled		= bActive;
			m_btnRModify.Enabled	= bActive;
			m_btnRRemove.Enabled	= bActive;
			m_btnLoad.Enabled		= bActive;
		}
		/// <summary>
		/// 2021.06.18 by twkang [ADD] 폴더리스트를 업데이트 한다.
		/// </summary>
		private void UpdateGroupView(string strFilePath)
		{
			List<string> listForDirectory	= new List<string>();

			if (false == GetDirectoryList(strFilePath, ref listForDirectory))
				return;

			m_dgviewGroup.Rows.Clear();

			for(int nIndex = 0, nEnd = listForDirectory.Count; nIndex < nEnd; ++nIndex)
			{
				m_dgviewGroup.Rows.Add();

				m_dgviewGroup[COLUMN_INDEX_OF_INDEX, nIndex].Value	= nIndex;
				m_dgviewGroup[COLUMN_INDEX_OF_NAME, nIndex].Value	= listForDirectory[nIndex];
			}
				
			m_dgviewGroup.ClearSelection();

			m_labelFullPath.Text	= strFilePath;
		}
		/// <summary>
		/// 2021.06.18 by twkang [ADD] 파일리스트를 업데이트 한다.
		/// </summary>
		private void UpdateFileView(string strFilePath)
		{
			List<KeyValuePair<string, DateTime>> listOfRecipeFile	= null;

			if(false == GetFileList(strFilePath, ref listOfRecipeFile))
				return;

			m_dgviewFile.Rows.Clear();

			int rowCount = 0;
			for (int nIndex = 0, nEnd = listOfRecipeFile.Count; nIndex < nEnd; ++nIndex)
			{
				if (_filteringKeyword != string.Empty && false == listOfRecipeFile[nIndex].Key.Contains(_filteringKeyword))
					continue;

				m_dgviewFile.Rows.Add();

				m_dgviewFile[COLUMN_INDEX_OF_INDEX, rowCount].Value = nIndex;
				m_dgviewFile[COLUMN_INDEX_OF_NAME, rowCount].Value = listOfRecipeFile[nIndex].Key;
				m_dgviewFile[COLUMN_INDEX_OF_TIME, rowCount].Value = listOfRecipeFile[nIndex].Value.ToString("yyyy/MM/dd HH:mm");
				++rowCount;
			}

			m_dgviewFile.ClearSelection();
			_currentFilePath = strFilePath;
		}
		/// <summary>
		/// 2021.06.18 by twkang [ADD] 선택된 레시피 이름 Label 업데이트
		/// </summary>
		private void WriteRecipeNameToLabel(string strRecipeName)
		{
			m_labelSelectedRecipe.Text	= strRecipeName;
			m_labelRFile.Text			= strRecipeName;
		}
		/// <summary>
		/// 2021.07.28 by twkang [ADD] 버튼 클릭 확인
		/// </summary>
		private bool ConfirmButtonClick(string strActionName)
		{
			return m_InstanceOfMessageBox.ShowMessage(string.Format("Action : [{0}], This is a confirmation message", strActionName));
		}
		#endregion

		#region FileIO
		/// <summary>
		/// 2020.05.20 by twkang [ADD] 문자열에 특수문자가 포함되어있는지 확인한다.
		/// </summary>
		private bool IsInvalidChars(ref string strText)
		{
			if (string.IsNullOrEmpty(strText))
				return false;

			// 2025.04.23 by junho [ADD] Encoding 확인 추가 (파일 생성은 되는데 나중에 load할 때 exception 발생함 > FileIoManager.dll에서 encoding 바뀌면 여기도 바꿔줘야함...)
			Encoding encoding = Encoding.Default;

			// 문자열을 해당 인코딩으로 바이트로 변환
			byte[] bytes = encoding.GetBytes(strText);

			// 바이트를 다시 문자열로 복원
			string decoded = encoding.GetString(bytes);

			// 원래 문자열과 복원된 문자열 비교
			if (false == strText.Equals(decoded))
				return true;

			Regex regex = new Regex(string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars()))));
			return regex.IsMatch(strText);

		}
		/// <summary>
		/// 2021.06.18 by twkang [ADD] 해당 경로의 레시피 파일을 가져온다.
		/// </summary>
		private bool GetFileList(string strFilePath, ref List<KeyValuePair<string, DateTime>> listOfRecipeFile)
		{
			if(string.IsNullOrEmpty(strFilePath))
				return false;

			listOfRecipeFile		= new List<KeyValuePair<string,DateTime>>();
			DirectoryInfo dInfo		= new DirectoryInfo(strFilePath);

			try
			{
				foreach (var fInfo in dInfo.GetFiles())
				{
					if (fInfo.Extension.ToLower().Equals(Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE))
						listOfRecipeFile.Add(new KeyValuePair<string, DateTime>(Path.GetFileNameWithoutExtension(fInfo.Name), fInfo.LastWriteTime));
				}
			}
			catch
			{

			}

			return true;

		}
		/// <summary>
		/// 2021.06.18 by twkang [ADD] 해당 경로의 폴더 리스트를 가져온다.
		/// </summary>
		private bool GetDirectoryList(string strFilePath, ref List<string> listOfFolder)
		{
			if (string.IsNullOrEmpty(strFilePath))
				return false;

			DirectoryInfo dInfo		= new DirectoryInfo(strFilePath);
			listOfFolder			= new List<string>();

			try
			{
				foreach (var fInfo in dInfo.GetDirectories())
				{
					listOfFolder.Add(fInfo.Name);
				}
			}
			catch
			{

			}

			return true;
		}
		/// <summary>
		/// 2021.06.18 by twkang [ADD] 해당경로에 폴더생성
		/// </summary>
		private bool MakeDirectory(string strFilePath, string strFolderName)
		{
			if(string.IsNullOrEmpty(strFilePath)
				|| IsInvalidChars(ref strFolderName))
				return false;

			DirectoryInfo dInfo	= new DirectoryInfo(strFilePath);

			try
			{
				dInfo.CreateSubdirectory(strFolderName);
			}
			catch
			{
				return false;
			}
			return true;
		}
		#endregion

		#endregion

		#region GUI Event
		/// <summary>
		/// 2021.06.18 by twkang [ADD] 파일설정 창 버튼클릭 이벤트
		/// </summary>
		private async void Click_FileConfiguration(object sender, EventArgs e)
		{
			try
			{
				this.Enabled = false;
				Control ctrl = sender as Control;

				// 2021.08.29 by junho [ADD] Vision Interwork
				Task<Vision_.VISION_RESULT> taskResult = null;
				var vision = Vision_.Vision.GetInstance();

				string strFullFilePath = m_labelFullPath.Text;
				string strSelectedFile = string.Empty;

				// 2021.08.29 by junho [ADD] Vision Interwork
				string strCurrentRecipeName = string.Empty;
				m_instanceRecipe.GetProcessFileNameWithoutExtension(ref strCurrentRecipeName);

				switch (ctrl.TabIndex)
				{
					case 0: // Create
						#region
						if (m_instanceKeyboard.CreateForm())
						{
							string strResult = string.Empty;
							m_instanceKeyboard.GetResult(ref strResult);
							// 2025.03.18 by junho [ADD] 앞 뒤에 공백생기지 않도록 개선
							strResult = strResult.TrimStart();
							strResult = strResult.TrimEnd();

							strResult += Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE;

							// 2022.06.08 by junho [ADD] File 유무 확인 예외처리 추가
							if (File.Exists(string.Format("{0}\\{1}", strFullFilePath, strResult)))
							{
								m_InstanceOfMessageBox.ShowMessage(string.Format("The file already exists. ({0})", strFullFilePath));
								return;
							}

							if (IsInvalidChars(ref strResult)
								|| false == m_instanceRecipe.MakeProcessRecipeFile(ref strFullFilePath, ref strResult))
								m_InstanceOfMessageBox.ShowMessage("Check");

							// 2021.08.29 by junho [ADD] Vision Interwork
							taskResult = vision.CopyRecipeAsync(strCurrentRecipeName
								, System.IO.Path.GetFileNameWithoutExtension(strResult));

							// 파일 IO Write Delay
							Thread.Sleep(FILE_IO_DELAY);
						}
						#endregion
						break;
					case 1: // Copy
						#region
						strSelectedFile = m_dgviewFile.SelectedCells[COLUMN_INDEX_OF_NAME].Value.ToString();

						List<KeyValuePair<string, DateTime>> listOfRecipeFile = null;

						if (false == GetFileList(m_labelFullPath.Text, ref listOfRecipeFile))
							return;

						string strTemp = strSelectedFile;

						for (int nIndex = 0, nEnd = listOfRecipeFile.Count; nIndex < nEnd; ++nIndex)
						{
							if (listOfRecipeFile[nIndex].Key == strTemp)
							{
								strTemp += COPY_TOKEN;
								nIndex = 0;
							}
						}
						{
							string strRecipeSource = Path.Combine(strFullFilePath, strSelectedFile + Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE);
							string strRecipeDest = Path.Combine(strFullFilePath, strTemp + Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE);

							// 2022.06.08 by junho [ADD] File 유무 확인 예외처리 추가
							if (false == File.Exists(strRecipeSource))
							{
								m_InstanceOfMessageBox.ShowMessage(string.Format("The file could not be found. ({0})", strRecipeSource));
								return;
							}

							File.Copy(strRecipeSource, strRecipeDest);

							// 2021.08.29 by junho [ADD] Vision Interwork
							taskResult = vision.CopyRecipeAsync(strCurrentRecipeName
								, System.IO.Path.GetFileNameWithoutExtension(strRecipeDest));
						}
						#endregion
						break;
					case 2: // Modify (Rename)
						#region
						strSelectedFile = m_dgviewFile.SelectedCells[COLUMN_INDEX_OF_NAME].Value.ToString();

						if (m_instanceKeyboard.CreateForm(strSelectedFile))
						{
							string strResult = string.Empty;
							m_instanceKeyboard.GetResult(ref strResult);
							// 2025.03.18 by junho [ADD] 앞 뒤에 공백생기지 않도록 개선
							strResult = strResult.TrimStart();
							strResult = strResult.TrimEnd();

							if (IsInvalidChars(ref strResult))
								break;

							string strRecipeSource = Path.Combine(strFullFilePath, strSelectedFile + Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE);
							string strRecipeDest = Path.Combine(strFullFilePath, strResult + Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE);

							// 2022.06.08 by junho [ADD] File 유무 확인 예외처리 추가
							if (false == File.Exists(strRecipeSource))
							{
								m_InstanceOfMessageBox.ShowMessage(string.Format("The file could not be found. ({0})", strRecipeSource));
								return;
							}

							if (File.Exists(strRecipeDest))
							{
								m_InstanceOfMessageBox.ShowMessage(string.Format("The file already exists. ({0})", strRecipeDest));
								return;
							}

							File.Move(strRecipeSource, strRecipeDest);

							// 2025.06.12 by junho [ADD] Previous value storage 기능 추가
							FrameOfSystem3.Recipe.PreviousValueStorage.Instance.FileRename(strSelectedFile, strResult);

							// 2021.08.29 by junho [ADD] Vision Interwork
							taskResult = vision.CopyRecipeAsync(
								System.IO.Path.GetFileNameWithoutExtension(strRecipeSource),
								System.IO.Path.GetFileNameWithoutExtension(strRecipeDest));
							await taskResult;
							if (taskResult.Result == Vision_.VISION_RESULT.COMPLETE)
							{
								taskResult = vision.DeleteRecipeAsync(
									System.IO.Path.GetFileNameWithoutExtension(strRecipeSource));
							}
						}
						#endregion
						break;
					case 3: // Remove
						#region
						// 2022.06.08 by junho [MOD] Remove 확인 메시지 문구 변경
						//if(false == ConfirmButtonClick("REMOVE"))
						//	return;
						strSelectedFile = m_dgviewFile.SelectedCells[COLUMN_INDEX_OF_NAME].Value.ToString();

						if (strSelectedFile != m_strCurruntLoadedRecipe)
						{
							string strDeleteFile = Path.Combine(strFullFilePath, strSelectedFile + Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE);

							// 2022.06.08 by junho [ADD] File 유무 확인 예외처리 추가
							if (false == File.Exists(strDeleteFile))
							{
								m_InstanceOfMessageBox.ShowMessage(string.Format("The file could not be found. ({0})", strDeleteFile));
								return;
							}

							if (false == m_InstanceOfMessageBox.ShowMessage(string.Format("Do you want remove the recipe file? ({0})", strDeleteFile)))
								return;

							File.Delete(strDeleteFile);

							// 2025.06.12 by junho [ADD] Previous value storage 기능 추가
							FrameOfSystem3.Recipe.PreviousValueStorage.Instance.FileRemove(strSelectedFile);

							// 2021.08.29 by junho [ADD] Vision Interwork
							taskResult = vision.DeleteRecipeAsync(
								System.IO.Path.GetFileNameWithoutExtension(strDeleteFile));
						}
						else
						{
							// 안내문구 변경 : 현재 로드된 레시피는 삭제할 수 없음.
							m_InstanceOfMessageBox.ShowMessage("Try deleting the recipe again after changing it to a different one.");
							// m_InstanceOfMessageBox.ShowMessage("Check");
						}
						#endregion
						break;
					default:
						return;
				}

				if (taskResult != null)
				{
					await taskResult;
					m_InstanceOfMessageBox.ShowMessage(string.Format("Vision result : {0}", taskResult.Result));
				}

				ResetFileLabel();

				UpdateFileView(strFullFilePath);

				ActiveFileControls(false);
			}
			finally
			{
				this.Enabled = true;
			}
		}
		/// <summary>
		/// 2021.06.18 by twkang [ADD] 폴더설정 창 버튼 클릭 이벤트
		/// </summary>
		private void Click_FolderConfiguation(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			string strDirectory	= strDirectory	= m_labelFullPath.Text;

			switch(ctrl.TabIndex)
			{
				case 0: // Create
					if(m_instanceKeyboard.CreateForm(m_labelGFolder.Text))
					{
						string strResult	= string.Empty;
						m_instanceKeyboard.GetResult(ref strResult);

						if(false == MakeDirectory(m_labelFullPath.Text, strResult))
							m_InstanceOfMessageBox.ShowMessage("Check");

						// 파일 IO Write Delay
						Thread.Sleep(FILE_IO_DELAY);
					}
					break;
				case 1: // Modify
					if(m_instanceKeyboard.CreateForm(m_labelGFolder.Text))
					{
						string strDirectoryName	= string.Empty;
						
						m_instanceKeyboard.GetResult(ref strDirectoryName);

						string strSoucePath		= Path.Combine(m_labelFullPath.Text, m_labelGFolder.Text);
						string strDirectoryPath	= Path.Combine(m_labelFullPath.Text, strDirectoryName);

						Directory.Move(strSoucePath, strDirectoryPath);
					}
					break;
				case 2: // Copy
					{
						string strSouceItem	= m_dgviewGroup.SelectedCells[COLUMN_INDEX_OF_NAME].Value.ToString();

						List<string> listForFolder	= null;

						GetDirectoryList(m_labelFullPath.Text, ref listForFolder);

						while(listForFolder.Contains(strSouceItem))
						{
							strSouceItem	+= COPY_TOKEN;
						}

						if (false == MakeDirectory(m_labelFullPath.Text, strSouceItem))
							m_InstanceOfMessageBox.ShowMessage("Check");
					}
					break;
				case 3: // Remove
					{
						if(false == ConfirmButtonClick("REMOVE"))
							return;

						string strSouceItem	= m_dgviewGroup.SelectedCells[COLUMN_INDEX_OF_NAME].Value.ToString();

						string strSelectedFolderPath = Path.Combine(m_labelFullPath.Text, strSouceItem);

						DirectoryInfo dInfo	= new DirectoryInfo(strSelectedFolderPath);

						dInfo.Delete(true);
					}
					break;
			}

			ResetFolderLabel();

			UpdateGroupView(strDirectory);

			ActiveFolderControls(false);
		}

		/// <summary>
		/// 2021.06.18 by twkang [ADD] 폴더이동 버튼 이벤트
		/// </summary>
		private void Click_FolderControl(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			string strSelectedFolderName	= string.Empty;
			string strFullFilePath			= string.Empty;

			switch(ctrl.TabIndex)
			{
				case 0: // Enter
					strSelectedFolderName	= m_dgviewGroup.SelectedCells[COLUMN_INDEX_OF_NAME].Value.ToString();
					strFullFilePath			= Path.Combine(m_labelFullPath.Text, strSelectedFolderName);
					break;
				case 1: // Previous
					DirectoryInfo dInfo	= new DirectoryInfo(m_labelFullPath.Text);

					strFullFilePath	= dInfo.Parent == null ? m_labelFullPath.Text : dInfo.Parent.FullName;
					break;
			}

			ResetControl();

			UpdateGroupView(strFullFilePath);

			UpdateFileView(strFullFilePath);
		}
		/// <summary>
		/// 2021.06.18 by twkang [ADD] 레시피 로드 버튼 이벤트
		/// </summary>
		private async void Click_RecipeLoad(object sender, EventArgs e)
		{
			if (false == Task.TaskOperator.GetInstance().IsIdleMode())
			{
				m_InstanceOfMessageBox.ShowMessage("Can not change the recipe in not IDLE");
				return;
			}

			this.Enabled = false;

			System.Threading.CancellationTokenSource cancelToken = new System.Threading.CancellationTokenSource();

			var recipeLoadResult = System.Threading.Tasks.Task.Factory.StartNew<string>(LoadRecipe);
			await recipeLoadResult;
			m_InstanceOfMessageBox.ShowMessage(recipeLoadResult.Result);
			this.Enabled = true;
		}
		private string LoadRecipe()
		{
			string strFilePath = m_labelFullPath.Text;
			string strSelectedRecipeFile = m_labelRFile.Text + Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE;

			string strErrMsg = string.Empty;

			if (m_instanceRecipe.LoadProcessRecipe(ref strFilePath, ref strSelectedRecipeFile, ref strErrMsg))
			{
				m_strCurruntLoadedRecipe = Path.GetFileNameWithoutExtension(strSelectedRecipeFile);
				return "OK";
				// m_InstanceOfMessageBox.ShowMessage("OK");
			}
			else
			{
				return string.IsNullOrEmpty(strErrMsg) ? "FAIL" : string.Format("FAIL : {0}", strErrMsg);
				// m_InstanceOfMessageBox.ShowMessage(string.IsNullOrEmpty(strErrMsg) ? "FAIL" : string.Format("FAIL : {0}", strErrMsg));
			}
		}

		/// <summary>
		/// 2021.06.18 by twkang [ADD] 전체경로 라벨 클릭 이벤트
		/// </summary>
		private void Click_FullFilePathLabel(object sender, EventArgs e)
		{
			// 2025.04.21 by junho [MOD] 경로 설정을 Keyboard 입력으로도 가능하도록 변경
			var selectionList = Functional.Form_SelectionList.GetInstance();
			if(false == selectionList.CreateForm("INSERT TYPE", new string[] { "DIALOG", "KEYBOARD" }, "DIALOG"))
				return;

			string strResult = string.Empty;
			selectionList.GetResult(ref strResult);
			switch (strResult)
			{
				case "DIALOG":
					using (FolderBrowserDialog fDialog = new FolderBrowserDialog())
					{
						switch (fDialog.ShowDialog())
						{
							case DialogResult.OK:
								strResult = fDialog.SelectedPath;
								fDialog.Dispose();
								break;
							default:
								fDialog.Dispose();
								return;
						}
					}
					break;
				case "KEYBOARD":
					if (false == m_instanceKeyboard.CreateForm("SELECT PATH"))
						return;

					m_instanceKeyboard.GetResult(ref strResult);
					break;
				default:
					return;

			}

			ResetControl();
			UpdateGroupView(strResult);
			UpdateFileView(strResult);
		}
		/// <summary>
		/// 2021.06.18 by twkang [ADD] 폴더아이템 클릭 이벤트
		/// </summary>
		private void Click_GroupView(object sender, DataGridViewCellEventArgs e)
		{
			int nIndex  = e.RowIndex;
			if (nIndex < 0)
			{
				m_dgviewGroup.ClearSelection();
				ActiveFolderControls(false);
				return;
			}

			string strSelectedFolder	= m_dgviewGroup[COLUMN_INDEX_OF_NAME, nIndex].Value.ToString();
			m_labelGFolder.Text			= strSelectedFolder;

			ActiveFolderControls(true);
		}
		/// <summary>
		/// 2021.06.21 by twkang [ADD] 폴더아이템 더블클릭 이벤트
		/// </summary>
		private void DoubleClick_GroupView(object sender, DataGridViewCellEventArgs e)
		{
			int nIndex  = e.RowIndex;
			if (nIndex < 0)
			{
				m_dgviewGroup.ClearSelection();
				ActiveFolderControls(false);
				return;
			}

			string strSelectedFolder	= m_dgviewGroup[COLUMN_INDEX_OF_NAME, nIndex].Value.ToString();
			string strFullFilePath		= Path.Combine(m_labelFullPath.Text, strSelectedFolder);

			ResetControl();

			UpdateGroupView(strFullFilePath);

			UpdateFileView(strFullFilePath);
		}
		/// <summary>
		/// 2021.06.18 by twkang [ADD] 파일아이템 클릭 이벤트
		/// </summary>
		private void Click_FileView(object sender, DataGridViewCellEventArgs e)
		{
			int nIndex = e.RowIndex;

			if (nIndex < 0)
			{
				ActiveFileControls(false);
				return; 
			}

			string strSelectedFile	= m_dgviewFile[COLUMN_INDEX_OF_NAME, nIndex].Value.ToString();
			m_labelRFile.Text		= strSelectedFile;

			ActiveFileControls(true);

			m_labelSelectedRecipe.Text	= Path.Combine(m_labelFullPath.Text, strSelectedFile);
		}
		private void lbl_Filtering_Click(object sender, EventArgs e)
		{
			if (_currentFilePath == string.Empty)
				return;

			if (false == m_instanceKeyboard.CreateForm(_filteringKeyword))
				return;

			m_instanceKeyboard.GetResult(ref _filteringKeyword);
			UpdateFileView(_currentFilePath);
			lbl_Filtering.Text = _filteringKeyword;
		}
		#endregion
	}
}
