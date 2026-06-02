using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using SystemTasks = System.Threading.Tasks;
using System.Threading;
using System.IO;

using RecipeManager_;
using FileIOManager_;
using FileComposite_;

using FileBorn_;

using FrameOfSystem3.Log;
using FrameOfSystem3.Account;
using Define.DefineConstant;
using Define.DefineEnumBase.Log;

namespace FrameOfSystem3.Recipe
{
	#region 열거형
	public enum EN_RECIPE_TYPE
	{
		COMMON = 0,
		EQUIPMENT = 1,
		PROCESS = 2,
	}
	public enum EN_RECIPE_PARAM_TYPE
	{
		MIN = 0,
		MAX = 1,
		VALUE = 2,
		UNIT = 3,
        AUTHORITY = 4,
        DATA_TYPE = 5,
        DESCRIPTION = 6,
		DEFAULT_VALUE = 7,		// 2023.05.19 by junho [ADD] add default value
	}
    public enum EN_DATA_TYPE
    {
        ASCII,
        BINARY,
        BOOL,
        UINT1,
        UINT2,
        UINT4,
        INT1,
        INT2,
        INT4,
        FLOAT4,
        FLOAT8
    }
	#endregion

	/// <summary>
	/// 2020.06.29 by yjlee [ADD] Manages the recipe parameters.
	/// </summary>
	public class Recipe
	{
		#region Singleton
		private Recipe()
		{
			foreach(EN_RECIPE_TYPE type in Enum.GetValues(typeof(EN_RECIPE_TYPE)))
			{
				_parameteChangeBlock.TryAdd(type, new List<string>());
			}
		}
		private static Recipe m_instance            = new Recipe();
		public static Recipe GetInstance() { return m_instance; }
		#endregion

		#region Constant
		private const string TOKEN_GROUP			= "GROUP ";
		private const string ROOT_COMMON			= "COMMON";
		private const string ROOT_EQUIPMENT			= "EQUIPMENT";
		private const string ROOT_PROCESS			= "PROCESS";
		private const string ROOT_PROCESS_NEW		= "PROCESS_NEW";
		private const string ROOT_PROCESS_PROPERTY	= "PROCESS_PROPERTY";

		private const int SIZE_TABLE				= 100;

		private const int BACKUP_MAINTAIN_DAYS		= 3;
		#endregion

		#region Variables
		RecipeManager m_instanceRecipeManager   = null;
		FileIOManager m_instanceFileIO          = null;
		FileComposite m_instanceFileComposite   = null;
		LogManager m_instanceLog                = null;
		CAccount _account = null;	// 2024.05.13 by junho [ADD] 설정된 Authority와 비교를 위해 추가

		string[] m_arIntToString                = null;

		string m_strLoadedProcessFilePath		= string.Empty;
		string m_strLoadedProcessFileName		= string.Empty;
		string m_strFullFilePath                = string.Empty;

		Dictionary<EN_RECIPE_TYPE, RECIPE_TYPE> m_dicForRecipeType				= new Dictionary<EN_RECIPE_TYPE, RECIPE_TYPE>();
		Dictionary<EN_RECIPE_PARAM_TYPE, RECIPE_PARAM_TYPE> m_dicForParamType	= new Dictionary<EN_RECIPE_PARAM_TYPE, RECIPE_PARAM_TYPE>();

		string[] m_arTypeOfParameter            = null;
		Dictionary<string, RECIPE_PARAM_TYPE> m_mappingEnumForType              = new Dictionary<string, RECIPE_PARAM_TYPE>();

		// 2024.09.12 by junho [ADD] 각각의 parameter가 변경될 떄 발생하는 event 추가
		EachParameterChangedEventManager _eachParameterChangedEventManager = new EachParameterChangedEventManager();
		#endregion /Variables

		#region Internal Interface

		#region Table
		/// <summary>
		/// 2020.07.01 by yjlee [ADD] Make the mapping table for this class.
		/// </summary>
		private void MakeMappingTable()
		{
			m_arIntToString = new string[SIZE_TABLE];

			for (int nIndex = 0; nIndex < SIZE_TABLE; ++nIndex)
			{
				m_arIntToString[nIndex] = nIndex.ToString();
			}

			m_dicForRecipeType.Clear();
			m_dicForRecipeType.Add(EN_RECIPE_TYPE.COMMON, RECIPE_TYPE.COMMON);
			m_dicForRecipeType.Add(EN_RECIPE_TYPE.EQUIPMENT, RECIPE_TYPE.EQUIPMENT);

			m_dicForParamType.Clear();
			m_dicForParamType.Add(EN_RECIPE_PARAM_TYPE.AUTHORITY, RECIPE_PARAM_TYPE.AUTHORITY);
			m_dicForParamType.Add(EN_RECIPE_PARAM_TYPE.MAX, RECIPE_PARAM_TYPE.MAX);
			m_dicForParamType.Add(EN_RECIPE_PARAM_TYPE.MIN, RECIPE_PARAM_TYPE.MIN);
			m_dicForParamType.Add(EN_RECIPE_PARAM_TYPE.UNIT, RECIPE_PARAM_TYPE.UNIT);
            m_dicForParamType.Add(EN_RECIPE_PARAM_TYPE.VALUE, RECIPE_PARAM_TYPE.VALUE);
            m_dicForParamType.Add(EN_RECIPE_PARAM_TYPE.DATA_TYPE, RECIPE_PARAM_TYPE.DATA_TYPE);
			m_dicForParamType.Add(EN_RECIPE_PARAM_TYPE.DESCRIPTION, RECIPE_PARAM_TYPE.DESCRIPTION);
			m_dicForParamType.Add(EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, RECIPE_PARAM_TYPE.DEFAULT_VALUE);	// 2023.05.19 by junho [ADD] add default value

			m_arTypeOfParameter = Enum.GetNames(typeof(EN_RECIPE_PARAM_TYPE));

			foreach (string strType in m_arTypeOfParameter)
			{
				RECIPE_PARAM_TYPE enType        = RECIPE_PARAM_TYPE.VALUE;

				if (Enum.TryParse(strType, out enType))
				{
					m_mappingEnumForType.Add(strType, enType);
				}
			}
		}
		#endregion

		#region Load
		/// <summary>
		/// 2020.06.29 by yjlee [ADD] Load the common or equipment recipe file.
		/// </summary>
		private bool LoadRecipe(RECIPE_TYPE enType)
		{
			string strFileName          = string.Empty;
			string strRootName          = string.Empty;
			string strData              = string.Empty;
			string[] arParam            = null;

			switch (enType)
			{
				case RECIPE_TYPE.COMMON:
					strFileName = Define.DefineConstant.FileName.FILENAME_RECIPE_COMMON;
					strRootName = ROOT_COMMON;
					arParam = Enum.GetNames(typeof(PARAM_COMMON));
					break;

				case RECIPE_TYPE.EQUIPMENT:
					strFileName = Define.DefineConstant.FileName.FILENAME_RECIPE_EQUIPMENT;
					strRootName = ROOT_EQUIPMENT;
					arParam = Enum.GetNames(typeof(PARAM_EQUIPMENT));
					break;
			}

			strFileName += Define.DefineConstant.FileFormat.FILEFORMAT_PARAMETER;

			string strFullFilePath      = System.IO.Path.Combine(Define.DefineConstant.FilePath.FILEPATH_RECIPE
				, strFileName);

			System.IO.FileInfo fInfo    = new System.IO.FileInfo(strFullFilePath);

			if (fInfo.Exists)
			{
				if (m_instanceFileIO.Read(Define.DefineConstant.FilePath.FILEPATH_RECIPE
				, strFileName
				, ref strData
				, Define.DefineConstant.FileIO.TIMEOUT_READ))
				{
					string[] arData = strData.Split('\n');

					if (m_instanceFileComposite.CreateRootByString(ref arData))
					{
						#region Check the changed parameters
                        AddParameterItem(ref strFileName, ref strRootName, ref arParam);
						#endregion

						return SetParameter(ref enType, ref arParam);
					}
				}
			}
			else
			{
				#region Create a recipe file.
				string[] arData     = null;
				string[] arType     = new string[] { strRootName };

				FileBorn.GetInstance().GetBornFrame(BORN_LIST.RECIPE, ref arType, ref arData);

				if (m_instanceFileComposite.CreateRootByString(ref arData))
				{
                    AddParameterItem(ref strFileName, ref strRootName, ref arParam);
				}
				#endregion
			}

			return false;
		}

        /// <summary>
        /// 2020.07.01 by yjlee [ADD] Load the process recipe file.
        /// 2022.01.17. by shkim. [ADD] Vision Recipe Load 관련 파라미터 추가 (프로그램실행타임)
        /// </summary>
        private bool LoadProcessRecipe(bool bIgnoreFailedToLoadRecipeToVision = false)
        {
            string strFilePath          = string.Empty;
            string[] arGroup            = new string[]{PARAM_COMMON.PROCESS_FILE_PATH.ToString(), m_arIntToString[0]};

            if(false == m_instanceFileComposite.GetValue(ROOT_COMMON
                , ConvertParamEnumToString(RECIPE_PARAM_TYPE.VALUE)
                , ref strFilePath
                , arGroup))
            {
                return false;
            }

            string strFileName          = string.Empty;
            arGroup[0]                  = PARAM_COMMON.PROCESS_FILE_NAME.ToString();

            if (false == m_instanceFileComposite.GetValue(ROOT_COMMON
                , ConvertParamEnumToString(RECIPE_PARAM_TYPE.VALUE)
                , ref strFileName
                , arGroup))
            {
                return false;
            }
            string strErrorMsg = string.Empty;

            return LoadProcessRecipe(ref strFilePath, ref strFileName, ref strErrorMsg, bIgnoreFailedToLoadRecipeToVision);
        }
		/// <summary>
		/// 2021.07.08 by twkang [ADD] Load the process property file
		/// </summary>
		private bool LoadProcessProperty()
		{
			string strData				= string.Empty;
			//string strFilePath			= Define.DefineConstant.FilePath.FILEPATH_RECIPE;
			string strFileName			= Define.DefineConstant.FileName.FILENAME_RECIPE_PROPERTY + Define.DefineConstant.FileFormat.FILEFORMAT_PARAMETER;
			string strFullFilePath      = System.IO.Path.Combine(Define.DefineConstant.FilePath.FILEPATH_RECIPE
				, strFileName);

			System.IO.FileInfo fInfo    = new System.IO.FileInfo(strFullFilePath);

			if (fInfo.Exists)
			{
				if (m_instanceFileIO.Read(Define.DefineConstant.FilePath.FILEPATH_RECIPE
				, strFileName
				, ref strData
				, Define.DefineConstant.FileIO.TIMEOUT_READ))
				{
					string[] arData = strData.Split('\n');

					m_instanceFileComposite.RemoveRoot(ROOT_PROCESS_PROPERTY);

					return m_instanceFileComposite.CreateRootByString(ref arData);
				}
			}

			return false;
		}
		/// <summary>
		/// 2020.12.15 by yjlee [ADD] Set the parameter to the recipe.
		/// 2023.05.08. by wdw [MOD] Parameter Index 저장 형식 변경.
		/// </summary>
		private bool SetParameter(ref RECIPE_TYPE enType, ref string[] arParam)
		{
			string strRootName      = enType == RECIPE_TYPE.COMMON ? ROOT_COMMON : ROOT_EQUIPMENT;

			for (int nIndex = 0, nEnd = arParam.Length; nIndex < nEnd; ++nIndex)
			{
				string strParameter     = arParam[nIndex];
				int nCountOfItem        = m_instanceRecipeManager.GetCountOfParameter(ref strParameter);

				string[] arGroup        = new string[] { strParameter, string.Empty };

				for (int nIndexOfParameter = 0; nIndexOfParameter < nCountOfItem; ++nIndexOfParameter)
				{
                    if (nCountOfItem > 1)
                    {
                        string strParameterCount = strParameter.Split('_').Last();
                        arGroup[0] =  strParameter.Remove(strParameter.Length - strParameterCount.Length, strParameterCount.Length) + m_arIntToString[nIndexOfParameter + 1];
                    }

					arGroup[1] = m_arIntToString[0];

					foreach (string strType in m_arTypeOfParameter)
					{
						string strData      = string.Empty;

						if (m_instanceFileComposite.GetValue(strRootName
							, strType
							, ref strData
							, arGroup))
						{
							if (false == m_instanceRecipeManager.SetValue(enType
								, strParameter
                                , nIndexOfParameter
								, m_mappingEnumForType[strType]
								, strData))
							{
								return false;
							}
						}
						else
						{
							// 2023.05.21 by junho [MOD] 아이템이 없으면 만들고 넘어간다.
							//return false;
							m_instanceFileComposite.SetValue(strRootName, strType, "", arGroup);
							continue;
						}
					}
				}
			}

			return true;
		}
		/// <summary>
		/// 2020.10.16 by yjlee [ADD] Set the process parameter value by the composite.
		/// </summary>
		private bool SetProcessParameter()
		{
			string[] arTask             = null;
			int nTotalCountOfParameter  = 0;
			string[] arParameter        = null;
			int[] arCountOfParameter    = null;

			if (m_instanceRecipeManager.GetListOfProcessParameter(ref arTask
				, ref nTotalCountOfParameter
				, ref arParameter
				, ref arCountOfParameter))
			{
				for (int nIndex = 0; nIndex < nTotalCountOfParameter; ++nIndex)
				{
					string strTaskName      = arTask[nIndex];
					string strParameterName = arParameter[nIndex];

					string[] arGroup        = new string[] { strTaskName, strParameterName, string.Empty };

					for (int nIndexOfParameter = 0; nIndexOfParameter < arCountOfParameter[nIndex]; ++nIndexOfParameter)
					{
						

						foreach (var kvp in m_mappingEnumForType)
						{
							string strData      = string.Empty;
							string strRootName	= string.Empty;

							switch (kvp.Value)
							{
								case RECIPE_PARAM_TYPE.VALUE:
									strRootName = ROOT_PROCESS;
                                    if (arCountOfParameter[nIndex] > 1)
                                    {
                                        string strParameterCount = strParameterName.Split('_').Last();
                                        arGroup[1] = strParameterName.Remove(strParameterName.Length - strParameterCount.Length, strParameterCount.Length) + m_arIntToString[nIndexOfParameter + 1];
                                        arGroup[2] = m_arIntToString[0];
                                    }
									break;
								default:
                                     arGroup[1] = strParameterName;
                                     arGroup[2] = m_arIntToString[nIndexOfParameter];
									strRootName = ROOT_PROCESS_PROPERTY;
									break;
							}

							if (m_instanceFileComposite.GetValue(strRootName
								, kvp.Key
								, ref strData
								, arGroup))
							{
								if (false == m_instanceRecipeManager.SetValue(strTaskName
									, strParameterName
									, nIndexOfParameter
									, m_mappingEnumForType[kvp.Key]
									, strData))
								{
									return false;
								}
							}
							else
							{
								// 2023.05.21 by junho [MOD] 아이템이 없으면 만들고 넘어간다.
								//return false;
								m_instanceFileComposite.SetValue(strRootName, kvp.Key, "", arGroup);
								continue;
							}
						}
					}
				}
			}
			return true;
		}
		#endregion

		#region Parameter
		/// <summary>
		/// 2020.12.15 by yjlee [ADD] Add the parameters to the recipe instance.
		/// </summary>
		private void AddParameterToInstance()
		{
			foreach (PARAM_COMMON enParam in Enum.GetValues(typeof(PARAM_COMMON)))
			{
				m_instanceRecipeManager.AddRecipeItem(RECIPE_TYPE.COMMON, enParam.ToString());
			}

			foreach (PARAM_EQUIPMENT enParam in Enum.GetValues(typeof(PARAM_EQUIPMENT)))
			{
				m_instanceRecipeManager.AddRecipeItem(RECIPE_TYPE.EQUIPMENT, enParam.ToString());
			}
		}
		/// <summary>
		/// 2020.11.26 by yjlee [ADD] Add the parameter item that is no existent in the structure.
		/// 2023.05.08. by wdw [MOD] Parameter Index 저장 형식 변경.
		/// </summary>
		private void AddParameterItem(ref string strFileName, ref string strRootName, ref string[] arParam)
		{
			string[] arType         = new string[] { strRootName };
			string[] arData         = null;
			bool bRewriteFile       = false;

			#region Add the parameters
			for (int nIndex = 0, nEnd = arParam.Length; nIndex < nEnd; ++nIndex)
			{
				string strParameter = arParam[nIndex];

				if (false == m_instanceFileComposite.IsGroupExist(strRootName, strParameter))
				{
					arType[0] = strParameter;

					int nCountOfItem    = m_instanceRecipeManager.GetCountOfParameter(ref strParameter);

					for (int nIndexOfParam = 0, nEndOfIndex = nCountOfItem
						; nIndexOfParam < nEndOfIndex; ++nIndexOfParam)
					{

                        if (nCountOfItem > 1)
                        {
                            string strParameterCount = strParameter.Split('_').Last();
                            strParameter = strParameter.Remove(strParameter.Length - strParameterCount.Length, strParameterCount.Length) + m_arIntToString[nIndexOfParam + 1];
                            arType[0] = strParameter;
                        }

						if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.RECIPE
						, ref arType
						, m_arIntToString[0]
						, ref arData))
						{
							m_instanceFileComposite.CreateGroupAndItemByString(strRootName, ref arData);
						}
					}

					bRewriteFile = true;
				}
			}
			#endregion

			if (bRewriteFile)
			{
				string strData      = string.Empty;

				m_instanceFileComposite.GetStructureAsString(strRootName, ref strData);

				m_instanceFileIO.Write(Define.DefineConstant.FilePath.FILEPATH_RECIPE
                    , strFileName
					, strData
					, false
					, false);
			}
		}
		/// <summary>
		/// 2021.07.08 by twkang [ADD] 
		/// </summary>
		private bool UpdateProcessPropertyItem()
		{
			string[] arTask         = null;
			string[] arParam        = null;
			int[] arCountOfItem     = null;
			int nCountOfParam       = 0;

			bool bRewrite           = false;

			if (m_instanceRecipeManager.GetListOfProcessParameter(ref arTask
				, ref nCountOfParam
				, ref arParam
				, ref arCountOfItem))
			{
				if (false == m_instanceFileComposite.IsGroupExist(ROOT_PROCESS_PROPERTY))
					m_instanceFileComposite.CreateRoot(ROOT_PROCESS_PROPERTY);

				string[] arPreGroup     = new string[2] { string.Empty, string.Empty };

				for (int nIndex = 0, nEnd = arParam.Length; nIndex < nEnd; ++nIndex)
				{
					arPreGroup[0] = arTask[nIndex];
					arPreGroup[1] = arParam[nIndex];

					if (false == m_instanceFileComposite.IsGroupExist(ROOT_PROCESS_PROPERTY, arPreGroup))
					{
						for (int nCount = 0, nCountEnd = arCountOfItem[nIndex]; nCount < nCountEnd; ++nCount)
						{
							string[] arData     = null;

							/// 2021.07.08 by twkang [ADD] Process Property
							if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.RECIPE_PROCESS
							, ref arPreGroup
							, m_arIntToString[nCount]
							, ref arData))
							{
								m_instanceFileComposite.CreateGroupAndItemByString(ROOT_PROCESS_PROPERTY, ref arData);
							}
						}

						bRewrite = true;
					}
				}
			}

			return bRewrite;
		}
		/// <summary>
		/// 2020.11.26 by yjlee [ADD] Add the process parameter item that is no existent in the structure.
		/// 2023.05.08. by wdw [MOD] Parameter Index 저장 형식 변경.
		/// </summary>
		private bool UpdateProcessParameterItem(string strRootName)
		{
			string[] arTask         = null;
			string[] arParam        = null;
			int[] arCountOfItem     = null;
			int nCountOfParam       = 0;

			bool bRewrite           = false;

			if (m_instanceRecipeManager.GetListOfProcessParameter(ref arTask
				, ref nCountOfParam
				, ref arParam
				, ref arCountOfItem))
			{
				string[] arPreGroup     = new string[2] { string.Empty, string.Empty };

				if (false == m_instanceFileComposite.IsGroupExist(strRootName))
					m_instanceFileComposite.CreateRoot(strRootName);

				for (int nIndex = 0, nEnd = arParam.Length; nIndex < nEnd; ++nIndex)
				{
					arPreGroup[0] = arTask[nIndex];
					arPreGroup[1] = arParam[nIndex];

					for (int nCount = 0, nCountEnd = arCountOfItem[nIndex]; nCount < nCountEnd; ++nCount)
					{
                        if (arCountOfItem[nIndex] > 1)
                        {
                            string strParameterCount = arParam[nIndex].Split('_').Last();
                            arPreGroup[1] = arParam[nIndex].Remove(arParam[nIndex].Length - strParameterCount.Length, strParameterCount.Length) + m_arIntToString[nCount + 1];
                        }
					    if (false == m_instanceFileComposite.IsGroupExist(strRootName, arPreGroup))
						{
							string[] arData     = null;

							if (FileBorn.GetInstance().GetItemFrame(BORN_LIST.RECIPE_PROCESS_VALUE
							, ref arPreGroup
							, m_arIntToString[0]
							, ref arData))
							{
								bool bCheck	= m_instanceFileComposite.CreateGroupAndItemByString(strRootName, ref arData);
							}
						}

						bRewrite = true;
					}
				}
			}

			return bRewrite;
		}

		#endregion

		#region Time
		/// <summary>
		/// 2021.01.15 by yjlee [ADD] Get the current time.
		/// </summary>
		private string GetCurrentTime()
		{
			return DateTime.Now.ToString("yyMMdd_HHmmss");
		}
		#endregion

		#region Save
		/// <summary>
		/// 2021.07.12 by twkang [ADD] Save the process recipe
		/// </summary>
		private bool SaveProcessRecipe(string strFilePath, string strRootName, string strFileName)
		{
			string strData			= string.Empty;
			string[] arTaskName		= null;
			int nCountOfParam		= -1;
			string[] arParamList	= null;
			int[] arCountOfItem		= null;

			if (false == BackupProcessRecipe(strFileName, strFilePath))
				return false;

			if (false == m_instanceRecipeManager.GetListOfProcessParameter(ref arTaskName
				, ref nCountOfParam
				, ref arParamList
				, ref arCountOfItem))
				return false;

			string[] arPreGroup     = new string[2] { string.Empty, string.Empty };

			m_instanceFileComposite.RemoveRoot(strRootName);
			m_instanceFileComposite.CreateRoot(strRootName);

			m_instanceFileComposite.RemoveRoot(ROOT_PROCESS_PROPERTY);
			m_instanceFileComposite.CreateRoot(ROOT_PROCESS_PROPERTY);

			for (int nIndex = 0, nEnd = arParamList.Length; nIndex < nEnd; ++nIndex)
			{
				arPreGroup[0] = arTaskName[nIndex];
				arPreGroup[1] = arParamList[nIndex];

				for (int nCount = 0, nCountEnd = arCountOfItem[nIndex]; nCount < nCountEnd; ++nCount)
				{
					string[] arPropertyData     = null;
					string[] arProcessData		= null;

					string[] strGroupLevel	= new string[] { arTaskName[nIndex], arParamList[nIndex], m_arIntToString[nCount] };

                    arPreGroup[1] = arParamList[nIndex];

                    FileBorn.GetInstance().GetItemFrame(BORN_LIST.RECIPE_PROCESS, ref arPreGroup, m_arIntToString[nCount], ref arPropertyData);

                    m_instanceFileComposite.CreateGroupAndItemByString(ROOT_PROCESS_PROPERTY, ref arPropertyData);

				
					foreach (EN_RECIPE_PARAM_TYPE en in Enum.GetValues(typeof(EN_RECIPE_PARAM_TYPE)))
					{
						string strValue = string.Empty;

                        switch (en)
                        {
                            case EN_RECIPE_PARAM_TYPE.VALUE:
                                strValue = m_instanceRecipeManager.GetValue(arTaskName[nIndex], arParamList[nIndex], nCount, m_dicForParamType[en], string.Empty);
                                if (arCountOfItem[nIndex] > 1)
                                {
                                    string strParameterCount = arParamList[nIndex].Split('_').Last();
                                    string strParaName = arParamList[nIndex].Remove(arParamList[nIndex].Length - strParameterCount.Length, strParameterCount.Length) + m_arIntToString[nCount + 1];

                                    arPreGroup[1] = strParaName;

                                    strGroupLevel[1] = strParaName;
                                    strGroupLevel[2] = m_arIntToString[0];
                                }
                                FileBorn.GetInstance().GetItemFrame(BORN_LIST.RECIPE_PROCESS_VALUE, ref arPreGroup, m_arIntToString[0], ref arProcessData);

                                m_instanceFileComposite.CreateGroupAndItemByString(strRootName, ref arProcessData);

                                m_instanceFileComposite.SetValue(strRootName, en.ToString(), strValue, strGroupLevel);
                                break;
                            default:
                                strGroupLevel[1] = arParamList[nIndex];
                                strGroupLevel[2] = m_arIntToString[nCount];

                                strValue = m_instanceRecipeManager.GetValue(arTaskName[nIndex], arParamList[nIndex], nCount, m_dicForParamType[en], string.Empty);
                               
                                m_instanceFileComposite.SetValue(ROOT_PROCESS_PROPERTY, en.ToString(), strValue, strGroupLevel);
                                break;
                        }
					}

				}
			}

			m_instanceFileComposite.GetStructureAsString(ROOT_PROCESS_PROPERTY, ref strData);

			bool bResult	= m_instanceFileIO.Write(Define.DefineConstant.FilePath.FILEPATH_RECIPE
								, Define.DefineConstant.FileName.FILENAME_RECIPE_PROPERTY + Define.DefineConstant.FileFormat.FILEFORMAT_PARAMETER
								, strData
								, false
								, false);

			m_instanceFileComposite.GetStructureAsString(strRootName, ref strData);

			bResult = m_instanceFileIO.Write(strFilePath
								, strFileName
								, strData
								, false
								, false);

			return bResult;
		}
		/// <summary>
		/// 2021.07.12 by twkang [ADD] 
		/// </summary>
		private bool BackupProcessRecipe(string strFileName, string strFilePath)
		{
			string strCurruntTime		= GetCurrentTime();
			string strBackUpPath		= Path.Combine(Define.DefineConstant.FilePath.FILEPATH_RECIPE, "BACKUP");

			//DirectoryInfo dirInfor	= new DirectoryInfo(strFilePath);

			if (false == Directory.Exists(strFilePath))
				return false;

			DirectoryInfo dirInfor	= new DirectoryInfo(strBackUpPath);
			FileInfo pCopyedFile	= null;

			#region Action before FileBackup
			// 해당 경로의 폴더가 존재하지 않을 경우
			if (false == dirInfor.Exists)
			{
				dirInfor.Create();
			}
			else
			{
				// 폴더가 존재하면 백업파일 정리
				var pTime	= DateTime.Now.AddDays(-BACKUP_MAINTAIN_DAYS);

				// 파일 날짜 체크
				foreach (var fInfo in dirInfor.GetFiles())
				{
					if (fInfo.LastWriteTime < pTime)
						fInfo.Delete();
				}
			}
			#endregion

			#region ProcessValue
			string strRecipeSource      = Path.Combine(strFilePath, strFileName);
			string strDestFileName		= strCurruntTime + strFileName;
			string strRecipeDest        = Path.Combine(strBackUpPath, strDestFileName);

			if (File.Exists(strRecipeSource))
			{
				File.Copy(strRecipeSource, strRecipeDest);

				pCopyedFile = new FileInfo(strRecipeDest);
				pCopyedFile.LastWriteTime = DateTime.Now;
			}

			#endregion

			#region ProcessProperty
			string strPropertyFileName	= Define.DefineConstant.FileName.FILENAME_RECIPE_PROPERTY + Define.DefineConstant.FileFormat.FILEFORMAT_PARAMETER;
			strRecipeSource = Path.Combine(Define.DefineConstant.FilePath.FILEPATH_RECIPE, strPropertyFileName);
			strDestFileName = strCurruntTime + strPropertyFileName;
			strRecipeDest = Path.Combine(strBackUpPath, strDestFileName);

			if (File.Exists(strRecipeSource))
			{
				File.Copy(strRecipeSource, strRecipeDest);
				pCopyedFile = new FileInfo(strRecipeDest);
				pCopyedFile.LastWriteTime = DateTime.Now;
			}
			#endregion

			return true;
		}
		#endregion

		#region Convert
		/// <summary>
		/// 2020.07.01 by yjlee [ADD] Convert the type of the parameter to the string.
		/// </summary>
		private string ConvertParamEnumToString(RECIPE_PARAM_TYPE enType)
		{
			switch (enType)
			{
				case RECIPE_PARAM_TYPE.MIN:
					return "MIN";

				case RECIPE_PARAM_TYPE.MAX:
					return "MAX";

				case RECIPE_PARAM_TYPE.AUTHORITY:
					return "AUTHORITY";

				case RECIPE_PARAM_TYPE.UNIT:
					return "UNIT";

				default:        // VALUE
					return "VALUE";
			}
		}
		private bool DataTypeCheck(EN_RECIPE_TYPE type, string parameterName, Type parameterType)
		{
			EN_DATA_TYPE dataType;
			if (false == Enum.TryParse(GetValue(type, parameterName, 0, EN_RECIPE_PARAM_TYPE.DATA_TYPE, ""), out dataType))
				return true;	// data type 설정이 안되있을 수 있으니 true 반환

			switch (dataType)
			{
				case EN_DATA_TYPE.BINARY: return true;
				case EN_DATA_TYPE.ASCII: return parameterType.Equals(typeof(string));
				case EN_DATA_TYPE.BOOL: return parameterType.Equals(typeof(bool));
				case EN_DATA_TYPE.UINT1:
				case EN_DATA_TYPE.UINT2:
				case EN_DATA_TYPE.UINT4:
				case EN_DATA_TYPE.INT1:
				case EN_DATA_TYPE.INT2:
				case EN_DATA_TYPE.INT4: return parameterType.Equals(typeof(int));
				case EN_DATA_TYPE.FLOAT4:
				case EN_DATA_TYPE.FLOAT8: return parameterType.Equals(typeof(double));
				default: return true;
			}
		}
		private bool DataTypeCheck(string taskName, string parameterName, Type parameterType)
		{
			EN_DATA_TYPE dataType;
			if (false == Enum.TryParse(GetValue(taskName, parameterName, 0, EN_RECIPE_PARAM_TYPE.DATA_TYPE, ""), out dataType))
				return true;	// data type 설정이 안되있을 수 있으니 true 반환

			switch (dataType)
			{
				case EN_DATA_TYPE.BINARY: return true;
				case EN_DATA_TYPE.ASCII: return parameterType.Equals(typeof(string));
				case EN_DATA_TYPE.BOOL: return parameterType.Equals(typeof(bool));
				case EN_DATA_TYPE.UINT1:
				case EN_DATA_TYPE.UINT2:
				case EN_DATA_TYPE.UINT4:
				case EN_DATA_TYPE.INT1:
				case EN_DATA_TYPE.INT2:
				case EN_DATA_TYPE.INT4: return parameterType.Equals(typeof(int));
				case EN_DATA_TYPE.FLOAT4:
				case EN_DATA_TYPE.FLOAT8: return parameterType.Equals(typeof(double));
				default: return true;
			}
		}
		#endregion

		#region Prepare for set
		/// <summary>
		/// 2023.05.20 by junho [ADD] 설정된 Data type과 비교를 위해 추가
		/// </summary>
		private void CheckDataType(EN_RECIPE_TYPE enType, string strName, int nIndexOfItem, ref string strValue)
		{
			EN_DATA_TYPE dataType;
			if (false == Enum.TryParse(GetValue(enType, strName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.DATA_TYPE, EN_DATA_TYPE.ASCII.ToString()), out dataType))
				return;

			switch (dataType)
			{
				case EN_DATA_TYPE.ASCII:
				case EN_DATA_TYPE.BINARY:
					break;
				case EN_DATA_TYPE.BOOL:
					#region
					{
						bool parseTest;
						if (false == bool.TryParse(strValue, out parseTest))
						{
							strValue = GetValue(enType, strName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, default(bool).ToString());
						}
					}
					#endregion
					break;
				case EN_DATA_TYPE.FLOAT4:
				case EN_DATA_TYPE.FLOAT8:
					#region
					{
						double parseTest;
						if (false == double.TryParse(strValue, out parseTest))
						{
							strValue = GetValue(enType, strName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, default(double).ToString());
						}
					}
					#endregion
					break;
				case EN_DATA_TYPE.INT1:
				case EN_DATA_TYPE.INT2:
				case EN_DATA_TYPE.INT4:
					#region
					{
						int parseTest;
						if (false == int.TryParse(strValue, out parseTest))
						{
							try
							{
								string splitValue = strValue.Split('.')[0];
								if (false == int.TryParse(splitValue, out parseTest))
								{
									strValue = GetValue(enType, strName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, default(int).ToString());
								}
								else
								{
									strValue = splitValue.ToString();
								}
							}
							catch
							{
								strValue = GetValue(enType, strName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, default(int).ToString());
							}
						}
					}
					#endregion
					break;
				case EN_DATA_TYPE.UINT1:
				case EN_DATA_TYPE.UINT2:
				case EN_DATA_TYPE.UINT4:
					#region
					if (strValue.Contains('-'))
					{
						strValue = "0";
					}
					else
					{
						int parseTest;
						if (false == int.TryParse(strValue, out parseTest))
						{
							try
							{
								string splitValue = strValue.Split('.')[0];
								if (false == int.TryParse(splitValue, out parseTest))
								{
									strValue = GetValue(enType, strName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, default(int).ToString());
								}
							}
							catch
							{
								strValue = GetValue(enType, strName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, default(int).ToString());
							}
						}
					}
					#endregion
					break;
			}
		}
		/// <summary>
		/// 2023.05.20 by junho [ADD] 설정된 Data type과 비교를 위해 추가
		/// </summary>
		private void CheckDataType(string strTaskName, string strParameterName, int nIndexOfItem, ref string strValue)
		{
			EN_DATA_TYPE dataType;
			if (false == Enum.TryParse(GetValue(strTaskName, strParameterName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.DATA_TYPE, EN_DATA_TYPE.ASCII.ToString()), out dataType))
				return;

			switch (dataType)
			{
				case EN_DATA_TYPE.ASCII:
				case EN_DATA_TYPE.BINARY:
					break;
				case EN_DATA_TYPE.BOOL:
					#region
					{
						bool parseTest;
						if (false == bool.TryParse(strValue, out parseTest))
						{
							strValue = GetValue(strTaskName, strParameterName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, default(bool).ToString());
						}
					}
					#endregion
					break;
				case EN_DATA_TYPE.FLOAT4:
				case EN_DATA_TYPE.FLOAT8:
					#region
					{
						double parseTest;
						if (false == double.TryParse(strValue, out parseTest))
						{
							strValue = GetValue(strTaskName, strParameterName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, default(double).ToString());
						}
					}
					#endregion
					break;
				case EN_DATA_TYPE.INT1:
				case EN_DATA_TYPE.INT2:
				case EN_DATA_TYPE.INT4:
					#region
					{
						int parseTest;
						if (false == int.TryParse(strValue, out parseTest))
						{
							try
							{
								string splitValue = strValue.Split('.')[0];
								if (false == int.TryParse(splitValue, out parseTest))
								{
									strValue = GetValue(strTaskName, strParameterName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, default(int).ToString());
								}
								else
								{
									strValue = splitValue.ToString();
								}
							}
							catch
							{
								strValue = GetValue(strTaskName, strParameterName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, default(int).ToString());
							}
						}
					}
					#endregion
					break;
				case EN_DATA_TYPE.UINT1:
				case EN_DATA_TYPE.UINT2:
				case EN_DATA_TYPE.UINT4:
					#region
					if (strValue.Contains('-'))
					{
						strValue = "0";
					}
					else
					{
						int parseTest;
						if (false == int.TryParse(strValue, out parseTest))
						{
							try
							{
								string splitValue = strValue.Split('.')[0];
								if (false == int.TryParse(splitValue, out parseTest))
								{
									strValue = GetValue(strTaskName, strParameterName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, default(int).ToString());
								}
							}
							catch
							{
								strValue = GetValue(strTaskName, strParameterName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, default(int).ToString());
							}
						}
					}
					#endregion
					break;
			}
		}
		/// <summary>
		/// 2024.05.13 by junho [ADD] 설정된 Authority와 비교를 위해 추가
		/// </summary>
		private bool CheckAuthority(EN_RECIPE_TYPE enType, string strName, int nIndexOfItem, EN_RECIPE_PARAM_TYPE paramType)
		{
			CAccount.EN_PARAM_USER_AUTHORITY targetAuthority;
			switch (paramType)
			{
				case EN_RECIPE_PARAM_TYPE.MIN:
				case EN_RECIPE_PARAM_TYPE.MAX:
				case EN_RECIPE_PARAM_TYPE.UNIT:
				case EN_RECIPE_PARAM_TYPE.DATA_TYPE:
				case EN_RECIPE_PARAM_TYPE.DESCRIPTION:
				case EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE:
				case EN_RECIPE_PARAM_TYPE.AUTHORITY:
					targetAuthority = CAccount.EN_PARAM_USER_AUTHORITY.MASTER;
					break;

				case EN_RECIPE_PARAM_TYPE.VALUE:
					if (false == Enum.TryParse(GetValue(enType, strName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.AUTHORITY, EN_DATA_TYPE.ASCII.ToString()), out targetAuthority))
						return true; // 설정하지 않았을 경우는 통과하도록 한다.
					break;

				default:
					return true;
			}

			var currentAuthority = _account.GetLoginedAuthority();
			return targetAuthority <= currentAuthority;
		}
		/// <summary>
		/// 2024.05.13 by junho [ADD] 설정된 Authority와 비교를 위해 추가
		/// </summary>
		private bool CheckAuthority(string strTaskName, string strParameterName, int nIndexOfItem, EN_RECIPE_PARAM_TYPE paramType)
		{
			CAccount.EN_PARAM_USER_AUTHORITY targetAuthority;
			switch (paramType)
			{
				case EN_RECIPE_PARAM_TYPE.MIN:
				case EN_RECIPE_PARAM_TYPE.MAX:
				case EN_RECIPE_PARAM_TYPE.UNIT:
				case EN_RECIPE_PARAM_TYPE.DATA_TYPE:
				case EN_RECIPE_PARAM_TYPE.DESCRIPTION:
				case EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE:
				case EN_RECIPE_PARAM_TYPE.AUTHORITY:
					targetAuthority = CAccount.EN_PARAM_USER_AUTHORITY.MASTER;
					break;

				case EN_RECIPE_PARAM_TYPE.VALUE:
					if (false == Enum.TryParse(GetValue(strTaskName, strParameterName, nIndexOfItem, EN_RECIPE_PARAM_TYPE.AUTHORITY, EN_DATA_TYPE.ASCII.ToString()), out targetAuthority))
						return true; // 설정하지 않았을 경우는 통과하도록 한다.
					break;

				default:
					return true;
			}

			var currentAuthority = _account.GetLoginedAuthority();
			return targetAuthority <= currentAuthority;
		}
		#endregion /Prepare for set

		#endregion

		#region External Interface

		#region Initialize & Exit
		/// <summary>
		/// 2020.06.29 by yjlee [ADD] Initialize the class for loading and using the recipe parameters.
		/// </summary>
		public bool Init()
		{
			m_instanceRecipeManager = RecipeManager.GetInstance();
			m_instanceFileIO = FileIOManager.GetInstance();
			m_instanceFileComposite = FileComposite.GetInstance();
			m_instanceLog = LogManager.GetInstance();
			_account = CAccount.GetInstance();	

			MakeMappingTable();

			AddParameterToInstance();

			bool bLoadCommonRecipe      = LoadRecipe(RECIPE_TYPE.COMMON);
			bool bLoadEquipmentRecipe   = LoadRecipe(RECIPE_TYPE.EQUIPMENT);
			bool bLoadPropertyRecipe	= LoadProcessProperty();
            // 2022.01.17. [MOD] 프로그램 초기 실행 중에는 비전 프로그램이 실행안되어있을 수도 있으니, 비전 레시피 로드 결과를 무시한다.
            bool bLoadProcessRecipe = bLoadCommonRecipe && LoadProcessRecipe(true);
            // bool bLoadProcessRecipe     = bLoadCommonRecipe && LoadProcessRecipe();

			// 2024.09.12 by junho [ADD] 각각의 parameter가 변경될 떄 발생하는 event 추가
			_eachParameterChangedEventManager.Init();

			ParameterChangedNotify += OnEachParameterChangedNotify;
			return bLoadCommonRecipe && bLoadEquipmentRecipe && bLoadProcessRecipe;
		}
		#endregion /Initialize & Exit

		#region Load
		/// <summary>
		/// 2020.06.29 by yjlee [ADD] Load the process recipe file.
		/// 2022.01.17. by shkim. [ADD] Vision Recipe Load 관련 파라미터 추가 (프로그램실행타임)
		/// </summary>
		public bool LoadProcessRecipe(ref string strFilePath, ref string strFileName, ref string strErrorMsg, bool bIgnoreFailedToLoadRecipeToVision = false)
		{
			strErrorMsg = "";

			if (string.IsNullOrEmpty(strFilePath)
				|| string.IsNullOrEmpty(strFileName))
			{
				return false;
			}

			string strData = string.Empty;

			// 2025.04.23 by junho [MOD] file name에 일본어 등 지원 안되는 문자 들어갔을 경우 Exception 발생하므로 recipe load만 실패하고 넘어가도록 변경
			try
			{
				if (false == m_instanceFileIO.Read(strFilePath, strFileName, ref strData, Define.DefineConstant.FileIO.TIMEOUT_READ))
					return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}

			m_instanceFileComposite.RemoveRoot(ROOT_PROCESS);

			string[] arData = strData.Split('\n');

			if (m_instanceFileComposite.CreateRootByString(ref arData))
			{
				#region Set the file path of the process parameter
				string[] arGroup = new string[] { PARAM_COMMON.PROCESS_FILE_PATH.ToString(), m_arIntToString[0] };

				if (false == m_instanceFileComposite.SetValue(ROOT_COMMON, ConvertParamEnumToString(RECIPE_PARAM_TYPE.VALUE)
					, strFilePath
					, arGroup)
					|| false == m_instanceRecipeManager.SetValue(RECIPE_TYPE.COMMON, arGroup[0], 0, RECIPE_PARAM_TYPE.VALUE, strFilePath))
				{
					return false;
				}

				arGroup[0] = PARAM_COMMON.PROCESS_FILE_NAME.ToString();

				if (false == m_instanceFileComposite.SetValue(ROOT_COMMON, ConvertParamEnumToString(RECIPE_PARAM_TYPE.VALUE)
					, strFileName
					, arGroup)
					|| false == m_instanceRecipeManager.SetValue(RECIPE_TYPE.COMMON, arGroup[0], 0, RECIPE_PARAM_TYPE.VALUE, strFileName))
				{
					return false;
				}
				#endregion

				if (m_instanceFileComposite.GetStructureAsString(ROOT_COMMON, ref strData)
					&& m_instanceFileIO.Write(Define.DefineConstant.FilePath.FILEPATH_RECIPE
						, Define.DefineConstant.FileName.FILENAME_RECIPE_COMMON + Define.DefineConstant.FileFormat.FILEFORMAT_PARAMETER
						, strData
						, false
						, false))
				{
					m_strLoadedProcessFileName = strFileName;
					m_strLoadedProcessFilePath = strFilePath;

					m_strFullFilePath = System.IO.Path.Combine(m_strLoadedProcessFilePath, m_strLoadedProcessFileName);

					if (UpdateProcessParameterItem(ROOT_PROCESS))
					{
						m_instanceFileComposite.GetStructureAsString(ROOT_PROCESS, ref strData);
						m_instanceFileIO.Write(strFilePath, strFileName, strData, false, false);
					}

					if (UpdateProcessPropertyItem())
					{
						/// 2021.07.08 by twkang [ADD] Property Parameter Save
						m_instanceFileComposite.GetStructureAsString(ROOT_PROCESS_PROPERTY, ref strData);
						m_instanceFileIO.Write(Define.DefineConstant.FilePath.FILEPATH_RECIPE
							, Define.DefineConstant.FileName.FILENAME_RECIPE_PROPERTY + Define.DefineConstant.FileFormat.FILEFORMAT_PARAMETER
							, strData
							, false
							, false);
					}

					// svid update
					FrameOfSystem3.Functional.PostOffice.GetInstance().SendMail(Define.DefineEnumProject.Mail.EN_SUBSCRIBER.ScenarioCirculator
						, Define.DefineEnumProject.Mail.EN_MAIL.SendScenario
						, FrameOfSystem3.SECSGEM.DefineSecsGem.EN_SCENARIO.RecipeChanged
						, new Dictionary<string, string>());

					if (SetProcessParameter())
					{
						//return bIgnoreFailedToLoadRecipeToVision;
						// 2022.11.03. by shkim. [CHECKS] 레시피를 로드할 때 비전레시피도 함께 로드하도록 하는 것인데,
						// 이를 위해선 현재 Vision.dll에 연결 상태 관련 인터페이스와 설비 파라미터가 추가로 필요하다.
						// -> vision time out으로 처리하도록 한다 by junho

						bool bUseVision = false;// GetValue(EN_RECIPE_TYPE.EQUIPMENT, PARAM_EQUIPMENT.Use_Vision.ToString(), 0, EN_RECIPE_PARAM_TYPE.VALUE, false);
						if (false == bUseVision) return true;

						string recipeName = GetValue(EN_RECIPE_TYPE.COMMON, PARAM_COMMON.PROCESS_FILE_NAME.ToString(), 0, EN_RECIPE_PARAM_TYPE.VALUE, "RECIPE");
						if (string.IsNullOrEmpty(recipeName)) return false;
						string reicpeNameWithoutExtenstion = System.IO.Path.GetFileNameWithoutExtension(recipeName);
						if (string.IsNullOrEmpty(reicpeNameWithoutExtenstion)) return false;


						Vision_.Vision vision = Vision_.Vision.GetInstance();
						Vision_.VISION_RESULT result;
						vision.ResetVision();

						while (true)
						{
							result = vision.LoadRecipeAll(reicpeNameWithoutExtenstion);
							if (result != Vision_.VISION_RESULT.IN_PROCESS)
							{
								break;
							}
							System.Threading.Thread.Sleep(1);
						}

						if (result == Vision_.VISION_RESULT.TIMEOUT_VISION)
						{
							strErrorMsg = "Vision Program 통신 시간 초과";
							return bIgnoreFailedToLoadRecipeToVision;
						}
						else if (result != Vision_.VISION_RESULT.COMPLETE)
						{
							strErrorMsg = "Vision Program Recipe 불러오기 실패";
							return bIgnoreFailedToLoadRecipeToVision;
						}

						// 2024.05.20 by junho [ADD] improve code
						return true;
					}
					else
					{
						return bIgnoreFailedToLoadRecipeToVision;
					}
				}
			}

			return false;
		}
		#endregion /Load

		#region Save
		/// <summary>
		/// 2021.07.12 by twkang [ADD] Save the process recipe
		/// </summary>
		public void SaveProcessRecipe()
		{
			SaveProcessRecipe(m_strLoadedProcessFilePath, ROOT_PROCESS, m_strLoadedProcessFileName);
		}
		#endregion /Save

		#region File Management
		/// <summary>
		/// 2020.06.29 by yjlee [ADD] Make the process recipe file.
		/// </summary>
		public bool MakeProcessRecipeFile(ref string strFilePath, ref string strFileName)
		{
			string[] arData     = null;
			string[] arType     = new string[] { ROOT_PROCESS_NEW };
			bool bReturnValue   = false;

			FileBorn.GetInstance().GetBornFrame(BORN_LIST.RECIPE_PROCESS_VALUE
				, ref arType
				, ref arData);

			if (m_instanceFileComposite.CreateRootByString(ref arData))
			{
				if (UpdateProcessParameterItem(ROOT_PROCESS_NEW))
				{
					string strData      = string.Empty;

					if (m_instanceFileComposite.GetStructureAsString(ROOT_PROCESS_NEW, ref strData)
						&& m_instanceFileIO.Write(strFilePath
							, strFileName
							, strData.Replace(TOKEN_GROUP + ROOT_PROCESS_NEW, TOKEN_GROUP + ROOT_PROCESS)
							, false
							, false))
					{
						bReturnValue = true;
					}
				}
			}

			m_instanceFileComposite.RemoveRoot(ROOT_PROCESS_NEW);

			return bReturnValue;
		}

		/// <summary>
		/// 레시피 파일을 읽어서 데이터로 반환한다.
		/// </summary>
		/// <param name="recipeFilePath">파일 경로</param>
		/// <param name="fileName">파일 이름 (확장자 포함)</param>
		/// <param name="includeProperty">프로퍼티 여부(같은경로에 있어야 함)</param>
		/// <param name="result">반환 데이터 [taskName][parameterName][type][value]</param>
		/// <returns></returns>
        public bool LoadFileToPpFmt(string recipeFilePath, string fileName, string propertyPath, bool includeProperty
			, out Dictionary<string, Dictionary<string, Dictionary<string, string>>> result)
		{
			// [taskName][parameterName][type][value]
			result = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

			#region file load
			if (string.IsNullOrEmpty(recipeFilePath) || string.IsNullOrEmpty(fileName))
				return false;

			if (false == System.IO.File.Exists(string.Format("{0}\\{1}", recipeFilePath, fileName)))
				return false;

			string fileData = string.Empty;

			FileIOManager fileIO = FileIOManager.GetInstance();
			if (false == fileIO.Read(recipeFilePath, fileName, ref fileData, FileIO.TIMEOUT_READ))
				return false;

			FileComposite file = FileComposite.GetInstance();

			file.RemoveRoot(ROOT_PROCESS);
			string[] arReadData = fileData.Split('\n');

			if (false == file.CreateRootByString(ref arReadData))
				return false;
			#endregion /file load

			string[] taskList = null;
			if (false == file.GetListOfGroup(ROOT_PROCESS, ref taskList))
				return false;

			foreach (string taskName in taskList)
			{
				result.Add(taskName, new Dictionary<string, Dictionary<string, string>>());

				string[] parameterList = null;
				if (false == file.GetListOfGroup(ROOT_PROCESS, ref parameterList, taskName))
					continue;

				foreach (string parameterName in parameterList)
				{
					result[taskName].Add(parameterName, new Dictionary<string, string>());

					string value = string.Empty;
					if (file.GetValue(ROOT_PROCESS, RECIPE_PARAM_TYPE.VALUE.ToString(), ref value, taskName, parameterName, "0"))
					{
						result[taskName][parameterName].Add(RECIPE_PARAM_TYPE.VALUE.ToString(), value);
					}
				}
			}

			if (false == includeProperty)
				return true;

			#region property

			#region file load
			fileName = "property.prm";
            if (false == System.IO.File.Exists(string.Format("{0}\\{1}", propertyPath, fileName)))
				return false;

			fileData = string.Empty;

			if (false == fileIO.Read(propertyPath, fileName, ref fileData, FileIO.TIMEOUT_READ))
				return false;

			file.RemoveRoot(ROOT_PROCESS_PROPERTY);
			arReadData = fileData.Split('\n');

			if (false == file.CreateRootByString(ref arReadData))
				return false;
			#endregion /file load
			
			List<string> arrType = Enum.GetNames(typeof(RECIPE_PARAM_TYPE)).ToList();
			arrType.Remove(RECIPE_PARAM_TYPE.VALUE.ToString());

			taskList = null;
			if (file.GetListOfGroup(ROOT_PROCESS_PROPERTY, ref taskList))
			{
				foreach (string taskName in taskList)
				{
					if (false == result.ContainsKey(taskName))
						continue;

					string[] parameterList = null;
					if (false == file.GetListOfGroup(ROOT_PROCESS_PROPERTY, ref parameterList, taskName))
						continue;

					foreach (string parameterName in parameterList)
					{
						if (false == result[taskName].ContainsKey(parameterName))
							continue;

						foreach (string type in arrType)
						{
							string value = string.Empty;
							if (false == file.GetValue(ROOT_PROCESS_PROPERTY, type, ref value, taskName, parameterName, "0"))
								value = string.Empty;

							result[taskName][parameterName].Add(type, value);
						}
					}
				}
			}

			// propety file이 완전하지 않을 수 있음.
			var emptyPropertys = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
			foreach (var kvpTask in result)
			{
				foreach (var kvpParameter in kvpTask.Value)
				{
					foreach(string type in arrType)
					{
						if (false == result[kvpTask.Key][kvpParameter.Key].ContainsKey(type))
						{
							if (false == emptyPropertys.ContainsKey(kvpTask.Key))
								emptyPropertys.Add(kvpTask.Key, new Dictionary<string,Dictionary<string,string>>());

							if (false == emptyPropertys[kvpTask.Key].ContainsKey(kvpParameter.Key))
								emptyPropertys[kvpTask.Key].Add(kvpParameter.Key, new Dictionary<string, string>());

							emptyPropertys[kvpTask.Key][kvpParameter.Key].Add(type, string.Empty);
						}
					}
				}
			}
			foreach (var kvpTask in emptyPropertys)
			{
				foreach (var kvpParameter in kvpTask.Value)
				{
					foreach (string type in arrType)
					{
						// 로직상 data list에 key가 없을 수 없으니 contains key는 따로 수행하지 않도록 한다.
						result[kvpTask.Key][kvpParameter.Key].Add(type, string.Empty);
					}
				}
			}
			#endregion /property

			return true;
		}

		/// <summary>
		/// 레시피 데이터를 저장한다.
		/// </summary>
		/// <param name="recipeFilePath">저장 경로</param>
		/// <param name="fileName">저장할 이름 (확장자 포함)</param>
		/// <param name="includeProperty">프로퍼티 저장 여부</param>
		/// <param name="recipeData">레시피 데이터 [taskName][parameterName][type][value]</param>
		/// <returns></returns>
        public bool SaveFileFromPpFmt(string recipeFilePath, string propertyPath, string fileName, bool includeProperty
			, Dictionary<string, Dictionary<string, Dictionary<string, string>>> recipeData)
		{
			FileComposite file = FileComposite.GetInstance();
			file.RemoveRoot(ROOT_PROCESS);

			if (false == file.CreateRoot(ROOT_PROCESS))
				return false;

			//// 여기부터 file 객체를 활용하여 데이터 저장한다.
			foreach(var kvpTask in recipeData)
			{
				if (false == file.AddGroup(ROOT_PROCESS, kvpTask.Key))
					return false;

				foreach(var kvpParameter in kvpTask.Value)
				{
					if (false == file.AddGroup(ROOT_PROCESS, kvpTask.Key, kvpParameter.Key))
						return false;

					if (false == kvpParameter.Value.ContainsKey(RECIPE_PARAM_TYPE.VALUE.ToString()))
						return false;

					if (false == file.AddGroup(ROOT_PROCESS, kvpTask.Key, kvpParameter.Key, "0"))
						return false;

					file.AddItem(ROOT_PROCESS, RECIPE_PARAM_TYPE.VALUE.ToString(), kvpParameter.Value[RECIPE_PARAM_TYPE.VALUE.ToString()]
						, kvpTask.Key, kvpParameter.Key, "0");
				}
			}

			#region file save
			string fileData = string.Empty;
			if (false == file.GetStructureAsString(ROOT_PROCESS, ref fileData))
				return false;

			file.RemoveRoot(ROOT_PROCESS);

			FileIOManager fileIO = FileIOManager.GetInstance();
			if (false == fileIO.Write(recipeFilePath, fileName, fileData, false, true))
				return false;

			#endregion /file save

			if (false == includeProperty)
				return true;

			#region property
			
			List<string> arrType = Enum.GetNames(typeof(RECIPE_PARAM_TYPE)).ToList();
			arrType.Remove(RECIPE_PARAM_TYPE.VALUE.ToString());

			file.RemoveRoot(ROOT_PROCESS_PROPERTY);

			if (false == file.CreateRoot(ROOT_PROCESS_PROPERTY))
				return false;

			//// 여기부터 file 객체를 활용하여 데이터 저장한다.
			foreach (var kvpTask in recipeData)
			{
				if (false == file.AddGroup(ROOT_PROCESS_PROPERTY, kvpTask.Key))
					return false;

				foreach (var kvpParameter in kvpTask.Value)
				{
					if (false == file.AddGroup(ROOT_PROCESS_PROPERTY, kvpTask.Key, kvpParameter.Key))
						return false;

					if (false == file.AddGroup(ROOT_PROCESS_PROPERTY, kvpTask.Key, kvpParameter.Key, "0"))
						return false;

					foreach (string type in arrType)
					{
						string value = string.Empty;

						if (kvpParameter.Value.ContainsKey(type))
							value = kvpParameter.Value[type];

						file.AddItem(ROOT_PROCESS_PROPERTY, type, value, kvpTask.Key, kvpParameter.Key, "0");
					}
				}
			}

			#region file save
			fileData = string.Empty;
			if (false == file.GetStructureAsString(ROOT_PROCESS_PROPERTY, ref fileData))
				return false;

			file.RemoveRoot(ROOT_PROCESS_PROPERTY);

            if (false == fileIO.Write(propertyPath, "property.prm", fileData, false, true))	
				return false;

			#endregion /file save

			#endregion /property

			return true;
		}

		/// <summary>
		/// Description을 별도의 csv file로 export
		/// Format:name,description
		/// </summary>
		public bool ExportDescription()
		{
			return false;
		}
		/// <summary>
		/// Export된 description csv file을 import
		/// isFillUpBlank:작성되있지 않은 항목만 설정
		/// </summary>
		public bool ImportDescription(bool isFillUpBlank)
		{
			return false;
		}
		#endregion /File Management

		#region Get Information

		#region Common & Equipment
		/// <summary>
		/// 2020.07.03 by yjlee [ADD] Get the information of the loaded process recipe.
		/// </summary>
		public bool GetProcessFileInformation(ref string strFilePath, ref string strFileName)
		{
			if (string.IsNullOrEmpty(m_strLoadedProcessFilePath)
				|| string.IsNullOrEmpty(m_strLoadedProcessFileName)) { return false; }

			strFilePath = m_strLoadedProcessFilePath;
			strFileName = m_strLoadedProcessFileName;

			return true;
		}

		/// <summary>
		/// 2021.08.25 by twkang [ADD] Get the filename of loaded process recipe without extention
		/// </summary>
		public bool GetProcessFileNameWithoutExtension(ref string strProcessFileName)
		{
			if (string.IsNullOrEmpty(m_strLoadedProcessFileName)) { return false; }

			strProcessFileName = Path.GetFileNameWithoutExtension(m_strLoadedProcessFileName);

			return true;
		}
		public string GetProcessFileNameWithoutExtension()
		{
			string result = "";
			if (false == GetProcessFileNameWithoutExtension(ref result))
				result = "";

			return result;
		}
		public bool GetProcessFileDirectory(ref string directory)
		{
			if (string.IsNullOrEmpty(m_strLoadedProcessFilePath))
				return false;

			directory = m_strLoadedProcessFilePath;
			return true;
		}
		public string GetProcessFileDirectory()
		{
			string result = string.Empty;
			if (false == GetProcessFileDirectory(ref result))
				return string.Empty;

			return result;
		}

		/// <summary>
		/// 2020.08.28 by twkang [ADD] 레피시 파라미터의 값을 반환한다.
		/// </summary>
		public bool GetValue(EN_RECIPE_TYPE enType, string strName, int nIndexOfItem, EN_RECIPE_PARAM_TYPE enParam, bool bDefaultValue)
		{
			return m_instanceRecipeManager.GetValue(m_dicForRecipeType[enType], strName, nIndexOfItem, m_dicForParamType[enParam], bDefaultValue);
		}
		/// <summary>
		/// 2020.08.28 by twkang [ADD] 레피시 파라미터의 값을 반환한다.
		/// </summary>
		public int GetValue(EN_RECIPE_TYPE enType, string strName, int nIndexOfItem, EN_RECIPE_PARAM_TYPE enParam, int nDefaultValue)
		{
			return m_instanceRecipeManager.GetValue(m_dicForRecipeType[enType], strName, nIndexOfItem, m_dicForParamType[enParam], nDefaultValue);
		}
		/// <summary>
		/// 2020.08.28 by twkang [ADD] 레피시 파라미터의 값을 반환한다.
		/// </summary>
		public double GetValue(EN_RECIPE_TYPE enType, string strName, int nIndexOfItem, EN_RECIPE_PARAM_TYPE enParam, double dblDefaultValue)
		{
			return m_instanceRecipeManager.GetValue(m_dicForRecipeType[enType], strName, nIndexOfItem, m_dicForParamType[enParam], dblDefaultValue);
		}
		/// <summary>
		/// 2020.08.28 by twkang [ADD] 레피시 파라미터의 값을 반환한다.
		/// </summary>
		public string GetValue(EN_RECIPE_TYPE enType, string strName, int nIndexOfItem, EN_RECIPE_PARAM_TYPE enParam, string strDefaultValue)
		{
			return m_instanceRecipeManager.GetValue(m_dicForRecipeType[enType], strName, nIndexOfItem, m_dicForParamType[enParam], strDefaultValue);
		}

		/// <summary>
		/// 2023.05.20 by junho [ADD] GetValue interface 추가
		/// </summary>
		public bool GetValue(EN_RECIPE_TYPE type, string parameterName, bool defaultValue)
		{
			if (false == DataTypeCheck(type, parameterName, defaultValue.GetType()))
				return defaultValue;

			return GetValue(type, parameterName, 0, EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
		}
		/// <summary>
		/// 2023.05.20 by junho [ADD] GetValue interface 추가
		/// </summary>
		public int GetValue(EN_RECIPE_TYPE type, string parameterName, int defaultValue)
		{
			if (false == DataTypeCheck(type, parameterName, defaultValue.GetType()))
				return defaultValue;

			return GetValue(type, parameterName, 0, EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
		}
		/// <summary>
		/// 2023.05.20 by junho [ADD] GetValue interface 추가
		/// </summary>
		public double GetValue(EN_RECIPE_TYPE type, string parameterName, double defaultValue)
		{
			if (false == DataTypeCheck(type, parameterName, defaultValue.GetType()))
				return defaultValue;

			return GetValue(type, parameterName, 0, EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
		}
		/// <summary>
		/// 2023.05.20 by junho [ADD] GetValue interface 추가
		/// </summary>
		public string GetValue(EN_RECIPE_TYPE type, string parameterName, string defaultValue)
		{
			// 2023.12.19 by junho [DEL] string type일 경우에는 type을 확인하지 않는다.
			//SetValue(type, parameterName, 0, EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, defaultValue);
			//if (false == DataTypeCheck(type, parameterName, defaultValue.GetType()))
			//	return defaultValue;

			return GetValue(type, parameterName, 0, EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
		}

		/// <summary>
		/// 2024.01.04 by junho [ADD] 현재 recipe file 경로의 recipe list를 반환한다.
		/// </summary>
		public List<string> GetRecipeFileList()
		{
			List<string> result;
			if (false == GetRecipeFileList(out result))
				result = new List<string>();

			return result;
		}
		/// <summary>
		/// 2024.01.04 by junho [ADD] 현재 recipe file 경로의 recipe list를 반환한다.
		/// </summary>
		public bool GetRecipeFileList(out List<string> result)
		{
			result = new List<string>();
			if (string.IsNullOrEmpty(m_strLoadedProcessFilePath))
				return false;

			System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(m_strLoadedProcessFilePath);
			try
			{
				foreach (var fInfo in dInfo.GetFiles())
				{
					if (fInfo.Extension.ToLower().Equals(Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE))
					{
						result.Add(System.IO.Path.GetFileNameWithoutExtension(fInfo.Name));
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}

			return true;
		}
		#endregion

		#region Process
		/// <summary>
		/// 2020.10.16 by yjlee [ADD] Get the value of the process recipe.
		/// </summary>
		public bool GetValue(string strTaskName, string strParameterName, int nIndexOfItem, EN_RECIPE_PARAM_TYPE enParam, bool bDefaultValue)
		{
			return m_instanceRecipeManager.GetValue(strTaskName, strParameterName, nIndexOfItem, m_dicForParamType[enParam], bDefaultValue);
		}
		/// <summary>
		/// 2020.10.16 by yjlee [ADD] Get the value of the process recipe.
		/// </summary>
		public int GetValue(string strTaskName, string strParameterName, int nIndexOfItem, EN_RECIPE_PARAM_TYPE enParam, int nDefaultValue)
		{
			return m_instanceRecipeManager.GetValue(strTaskName, strParameterName, nIndexOfItem, m_dicForParamType[enParam], nDefaultValue);
		}
		/// <summary>
		/// 2020.10.16 by yjlee [ADD] Get the value of the process recipe.
		/// </summary>
		public string GetValue(string strTaskName, string strParameterName, int nIndexOfItem, EN_RECIPE_PARAM_TYPE enParam, string strDefaultValue)
		{
			return m_instanceRecipeManager.GetValue(strTaskName, strParameterName, nIndexOfItem, m_dicForParamType[enParam], strDefaultValue);
		}
		/// <summary>
		/// 2020.10.16 by yjlee [ADD] Get the value of the process recipe.
		/// </summary>
		public double GetValue(string strTaskName, string strParameterName, int nIndexOfItem, EN_RECIPE_PARAM_TYPE enParam, double dblDefaultValue)
		{
			return m_instanceRecipeManager.GetValue(strTaskName, strParameterName, nIndexOfItem, m_dicForParamType[enParam], dblDefaultValue);
		}
		/// <summary>
		/// 2021.03.16 by twkang [ADD] Get the list of process parameter
		/// </summary>
		public bool GetListOfProcessParameter(ref string[] arTaskName, ref int nCountOfParam, ref string[] arParamList, ref int[] arCountOfItem)
		{
			return m_instanceRecipeManager.GetListOfProcessParameter(ref arTaskName, ref nCountOfParam, ref arParamList, ref arCountOfItem);
		}
		/// <summary>
		/// 2021.03.16 by twkang [ADD] Get the value of all process parameter
		/// </summary>
		public void GetProcessValueAll(string strTaskName, int nCountOfParam, ref string[] arParamList, ref int[] arCountOfItem, ref string[][] arValue)
		{
			m_instanceRecipeManager.GetProcessValueAll(strTaskName, nCountOfParam, ref arParamList, ref arCountOfItem, ref arValue);
		}

		/// <summary>
		/// 2023.05.20 by junho [ADD] GetValue interface 추가
		/// </summary>
		public bool GetValue(string taskName, string parameterName, bool defaultValue)
		{
			if (false == DataTypeCheck(taskName, parameterName, defaultValue.GetType()))
				return defaultValue;

			return GetValue(taskName, parameterName, 0, EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
		}
		/// <summary>
		/// 2023.05.20 by junho [ADD] GetValue interface 추가
		/// </summary>
		public int GetValue(string taskName, string parameterName, int defaultValue)
		{
			if (false == DataTypeCheck(taskName, parameterName, defaultValue.GetType()))
				return defaultValue;

			return GetValue(taskName, parameterName, 0, EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
		}
		/// <summary>
		/// 2023.05.20 by junho [ADD] GetValue interface 추가
		/// </summary>
		public double GetValue(string taskName, string parameterName, double defaultValue)
		{
			if (false == DataTypeCheck(taskName, parameterName, defaultValue.GetType()))
				return defaultValue;

			return GetValue(taskName, parameterName, 0, EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
		}
		/// <summary>
		/// 2023.05.20 by junho [ADD] GetValue interface 추가
		/// </summary>
		public string GetValue(string taskName, string parameterName, string defaultValue)
		{
			// 2023.12.19 by junho [DEL] string type일 경우에는 type을 확인하지 않는다.
			//SetValue(taskName, parameterName, 0, EN_RECIPE_PARAM_TYPE.DEFAULT_VALUE, defaultValue.ToString());
			//if (false == DataTypeCheck(taskName, parameterName, defaultValue.GetType()))
			//	return defaultValue;

			return GetValue(taskName, parameterName, 0, EN_RECIPE_PARAM_TYPE.VALUE, defaultValue);
		}
		#endregion

		#endregion /Get Information

		#region Set Information
		/// <summary>
		/// 2020.08.27 by twkang [ADD] 레시피 파라미터의 값을 설정한다.
		/// 2020.12.16 by yjlee [ADD] Bug fixed about the file path to save it.
		/// 2021.01.18 by yjlee [ADD] Add the configuration log.
		/// 2022.05.01 by junho [ADD] Add the Write Log option
		/// 2023.05.08. by wdw [MOD] Parameter Index 저장 형식 변경.
		/// </summary>
		public bool SetValue(EN_RECIPE_TYPE enType, string strName, int nIndexOfItem, EN_RECIPE_PARAM_TYPE enParam, string strValue, bool isWriteLog = true, bool doNotify = true)
		{
			string[] arGroup			= new string[] { strName, m_arIntToString[0] };
            int nParameterCount = m_instanceRecipeManager.GetCountOfParameter(ref strName);
            string strIndexParameterName = strName;

            if (nParameterCount > 1)
            {
                string strParameterCount = strName.Split('_').Last();
                strIndexParameterName = strName.Remove(strName.Length - strParameterCount.Length, strParameterCount.Length) + m_arIntToString[nIndexOfItem + 1];
                arGroup = new string[] { strIndexParameterName, m_arIntToString[0] };
            }

			string strData				= string.Empty;
			string strFileName          = string.Empty;
			string strType              = enType.ToString();
			string strParam             = enParam.ToString();
			string strPreviousData		= string.Empty;

			// 2021.05.31 by twkang [ADD] Log 작성을 위해 추가
			if(m_instanceFileComposite.GetValue(strType, strParam, ref strPreviousData, arGroup))
			{
				// 2023.05.20 by junho [ADD] 같은 값이면 set하지 않도록 변경
				if (strPreviousData.Equals(strValue))
				{
					// 2025.03.12 by junho [ADD] 같은 값일 때 ui 색상 변경 안되는 버그 개선
					if (enParam == EN_RECIPE_PARAM_TYPE.VALUE)
						RemoveDeferredStorage(enType, strName, nIndexOfItem);
					return true;
				}

				/// 2023.05.20 by junho [ADD] 설정된 Data type과 비교를 위해 추가
				if (enParam == EN_RECIPE_PARAM_TYPE.VALUE)
				{
					CheckDataType(enType, strName, nIndexOfItem, ref strValue);
				}

				/// 2024.05.13 by junho [ADD] 설정된 Authority와 비교를 위해 추가
				if (false == CheckAuthority(enType, strName, nIndexOfItem, enParam))
				{
					//Functional.PostOffice.GetInstance().SendMail(Define.DefineEnumProject.Mail.EN_SUBSCRIBER.OperationMain
					//   , Define.DefineEnumProject.Mail.EN_MAIL.UI_ShowMessageBox, "Can not change parameter at this user level");
					return false;
				}

				if (m_instanceFileComposite.SetValue(strType, strParam, strValue, arGroup))
				{
					if (m_dicForRecipeType.ContainsKey(enType)
						&& m_dicForParamType.ContainsKey(enParam))
					{
						switch (enType)
						{
							case EN_RECIPE_TYPE.COMMON:
								strFileName = Define.DefineConstant.FileName.FILENAME_RECIPE_COMMON;
								break;

							case EN_RECIPE_TYPE.EQUIPMENT:
								strFileName = Define.DefineConstant.FileName.FILENAME_RECIPE_EQUIPMENT;
								break;
						}

						strFileName += Define.DefineConstant.FileFormat.FILEFORMAT_PARAMETER;

						if (m_instanceFileComposite.GetStructureAsString(enType.ToString(), ref strData)
							&& m_instanceFileIO.Write(Define.DefineConstant.FilePath.FILEPATH_RECIPE
							, strFileName
							, strData
							, false
							, false)
							&& m_instanceRecipeManager.SetValue(m_dicForRecipeType[enType], strName, nIndexOfItem, m_dicForParamType[enParam], strValue))
						{
							string strFullFilePath = System.IO.Path.Combine(Define.DefineConstant.FilePath.FILEPATH_RECIPE, strFileName);

							if (isWriteLog)
							{
								// 2021.01.18 by yjlee [ADD] Write the configuration log.
								// 2021.05.31 by twkang [MOD] Setting Log 에서 Change 로 변경
								m_instanceLog.WriteConfigurationLog(ref strType, EN_CFG_TYPE.CHANGE, strIndexParameterName, strPreviousData, strValue);
								m_instanceLog.WriteConfigurationLog(ref strType, EN_CFG_TYPE.SAVE, string.Empty, string.Empty, strFullFilePath);
							}

							if (enParam == EN_RECIPE_PARAM_TYPE.VALUE)
							{
								RemoveDeferredStorage(enType, strName, nIndexOfItem);
								if (doNotify)
								{
									// 2024.09.12 by junho [ADD] 각각의 parameter가 변경될 떄 발생하는 event 추가
									OnNotifyParameterChanged(true, new List<ParameterItem>() { new ParameterItem(enType, strName, nIndexOfItem, strValue) });
								}

								SECSGEM.ScenarioOperator.Instance.UpdateECVParameters(strName, strValue);
								// 2024.09.19 by junho [DEL] scenario circulator로 이동
								//switch (enType)
								//{
								//    case EN_RECIPE_TYPE.COMMON:
								//        {
								//            PARAM_COMMON data;
								//            if (Enum.TryParse(strName, out data))
								//            {
								//                SECSGEM.ScenarioOperator.Instance.UpdateCommonParameters(data, strValue);
								//            }
								//        }
								//        break;

								//    case EN_RECIPE_TYPE.EQUIPMENT:
								//        {
								//            PARAM_EQUIPMENT data;
								//            if (Enum.TryParse(strName, out data))
								//            {
								//                SECSGEM.ScenarioOperator.Instance.UpdateMachineParameters(data, strValue);
								//            }
								//        }
								//        break;
								//}

								// 2024.09.19 by junho [DEL] efem manager에서 직접 event 받도록 변경
								//if (enParam == EN_RECIPE_PARAM_TYPE.VALUE)
								//{
								//Work.UIComunicationEvent.Inform_ToTask(Work.UIComunicationEvent.EN_REQUEST_FROM_UI.SECSGEM_SEND_ECID, Define.DefineEnumProject.Task.EN_TASK_LIST.Global.ToString()
								//	, new object[] { enType, strName, strValue });
								//}
								return true;
							}
						}
					}
				}
			}

			return false;
		}
		public bool SetValue(EN_RECIPE_TYPE type, string parameterName, string setValue, bool isWriteLog = true)
		{
			return SetValue(type, parameterName, 0, EN_RECIPE_PARAM_TYPE.VALUE, setValue, isWriteLog);
		}
		/// <summary>
		/// 2020.10.16 by yjlee [ADD] Set the value of the process recipe.
		/// 2021.01.18 by yjlee [ADD] Add to write the log.
		/// 2023.05.08. by wdw [MOD] Parameter Index 저장 형식 변경.
		/// </summary>
		public bool SetValue(string strTaskName, string strParameterName, int nIndexOfItem, EN_RECIPE_PARAM_TYPE enParam, string strValue, bool doNotify = true)
		{
            if (nIndexOfItem < 0)
                nIndexOfItem = 0;

            string[] arGroup = new string[] { strTaskName, strParameterName, m_arIntToString[nIndexOfItem] };
            string strIndexParameterName = strParameterName;
            int nParameterCount = m_instanceRecipeManager.GetCountOfParameter(ref strParameterName);
            if (nParameterCount > 1
                && enParam == EN_RECIPE_PARAM_TYPE.VALUE)
            {
                string strParameterCount = strParameterName.Split('_').Last();
                strIndexParameterName = strParameterName.Remove(strParameterName.Length - strParameterCount.Length, strParameterCount.Length) + m_arIntToString[nIndexOfItem + 1];
                arGroup = new string[] { strTaskName, strIndexParameterName, m_arIntToString[0] };
            }
         
			string strPreviousData  = string.Empty;
			string strItemName      = enParam.ToString();
			string strRootName		= string.Empty;
			string strFilePath		= string.Empty;
			string strFileName		= string.Empty;
			string strFullFilePath	= string.Empty;

			switch (enParam)
			{
				case EN_RECIPE_PARAM_TYPE.VALUE:
					strRootName = ROOT_PROCESS;
					strFilePath = m_strLoadedProcessFilePath;
					strFileName = m_strLoadedProcessFileName;
					break;
				default:
					strRootName = ROOT_PROCESS_PROPERTY;
					strFilePath = Define.DefineConstant.FilePath.FILEPATH_RECIPE;
					strFileName = Define.DefineConstant.FileName.FILENAME_RECIPE_PROPERTY + Define.DefineConstant.FileFormat.FILEFORMAT_PARAMETER;
					break;
			}

			strFullFilePath = System.IO.Path.Combine(strFilePath, strFileName);

			if (m_instanceFileComposite.GetValue(strRootName, strItemName, ref strPreviousData, arGroup))
			{
				// 2023.05.20 by junho [ADD] 같은 값이면 set하지 않도록 변경
				if (strPreviousData.Equals(strValue))
				{
					// 2025.03.12 by junho [ADD] 같은 값일 때 ui 색상 변경 안되는 버그 개선
					if (enParam == EN_RECIPE_PARAM_TYPE.VALUE)
						RemoveDeferredStorage(strTaskName, strParameterName, nIndexOfItem);
					return true;
				}

				/// 2023.05.20 by junho [ADD] 설정된 Data type과 비교를 위해 추가
				if (enParam == EN_RECIPE_PARAM_TYPE.VALUE)
				{
					CheckDataType(strTaskName, strParameterName, nIndexOfItem, ref strValue);
				}

				/// 2024.05.13 by junho [ADD] 설정된 Authority와 비교를 위해 추가
				if (false == CheckAuthority(strTaskName, strParameterName, nIndexOfItem, enParam))
				{
					//Functional.PostOffice.GetInstance().SendMail(Define.DefineEnumProject.Mail.EN_SUBSCRIBER.OperationMain
					//   , Define.DefineEnumProject.Mail.EN_MAIL.UI_ShowMessageBox, "Can not change parameter at this user level");
					return false;
				}

				if (m_instanceFileComposite.SetValue(strRootName, strItemName, strValue, arGroup))
				{
					string strData          = string.Empty;

					if (m_instanceFileComposite.GetStructureAsString(strRootName, ref strData)
							&& m_instanceFileIO.Write(strFilePath
							, strFileName
							, strData
							, false
							, false)
							&& m_instanceRecipeManager.SetValue(strTaskName, strParameterName, nIndexOfItem, m_dicForParamType[enParam], strValue))
					{
						// 2021.12.03 by junho [MOD] improve code
						// 2021.01.18 by yjlee [ADD] Write the configuration log.
						//m_instanceLog.WriteConfigurationLog(ref strTaskName, EN_CFG_TYPE.CHANGE, strItemName, strPreviousData, strValue);
                        m_instanceLog.WriteConfigurationLog(ref strTaskName, EN_CFG_TYPE.CHANGE, strIndexParameterName, strPreviousData, strValue);
						m_instanceLog.WriteConfigurationLog(ref strTaskName, EN_CFG_TYPE.SAVE, string.Empty, string.Empty, strFullFilePath);

						if(enParam == EN_RECIPE_PARAM_TYPE.VALUE)
						{
							RemoveDeferredStorage(strTaskName, strParameterName, nIndexOfItem);
							if (doNotify)
							{
								// 2024.09.12 by junho [ADD] 각각의 parameter가 변경될 떄 발생하는 event 추가
								OnNotifyParameterChanged(true, new List<ParameterItem>() { new ParameterItem(strTaskName, strParameterName, nIndexOfItem, strValue) });
							}
						}

						return true;
					}
				}
			}

			return false;
		}
		public bool SetValue(string taskName, string parameterName, string setValue)
		{
			return SetValue(taskName, parameterName, 0, EN_RECIPE_PARAM_TYPE.VALUE, setValue);
		}
		#endregion /Set Information

		#region DeferredStorage

		#region common/equipment
		/// <summary>
		/// key: {recipe type}.{parameterName}.{index}
		/// value: stored value
		/// </summary>
		ConcurrentDictionary<string, string> _deferredStorageForEcParmater = new ConcurrentDictionary<string, string>();

		// TODO : 요기필요 -> 이렇게 처리하면 안 될 것 같다
		// 2025.07.08. jhlim [MOD] 컨펌 사용하지 않는 경우 추가 -> 기존에는 SetValue로 직접 하던 것을 컨펌 미확인 이후 Apply 하도록 변경
		public bool SetDeferredStorage(EN_RECIPE_TYPE type, string parameterName, int indexOfItem, string newValue, bool needCheckConfirmation = true)
		{
			if (needCheckConfirmation && IsBlockedParameterChange(type))
			{
				//Functional.PostOffice.GetInstance().SendMail(Define.DefineEnumProject.Mail.EN_SUBSCRIBER.OperationMain
				//   , Define.DefineEnumProject.Mail.EN_MAIL.UI_ShowMessageBox, "Parameter change blocked");
				return false;
            }

			string key = string.Format("{0}.{1}.{2}", type.ToString(), parameterName, indexOfItem.ToString());
			string r = _deferredStorageForEcParmater.AddOrUpdate(key, newValue, (k, v) => { return newValue; });
			return r == newValue;
		}
		// 2025.07.08. jhlim [END]
		public bool SetDeferredStorage(EN_RECIPE_TYPE type, string parameterName, string newValue)
		{
			return SetDeferredStorage(type, parameterName, 0, newValue);
		}
		public bool GetDeferredStorage(EN_RECIPE_TYPE type, string parameterName, int indexOfItem, out string result)
		{
			string key = string.Format("{0}.{1}.{2}", type.ToString(), parameterName, indexOfItem.ToString());
			return _deferredStorageForEcParmater.TryGetValue(key, out result);
		}
		public bool GetDeferredStorage(EN_RECIPE_TYPE type, string parameterName, out string result)
		{
			return GetDeferredStorage(type, parameterName, 0, out result);
		}
		private void RemoveDeferredStorage(EN_RECIPE_TYPE type, string parameterName, int indexOfItem = 0)
		{
			string key = string.Format("{0}.{1}.{2}", type.ToString(), parameterName, indexOfItem.ToString());
			if (_deferredStorageForEcParmater.ContainsKey(key))
			{
				string s;
				_deferredStorageForEcParmater.TryRemove(key, out s);
			}
		}
		#endregion /common/equipment

		#region recipe
		/// <summary>
		/// key: {task name}.{parameterName}.{index}
		/// value: stored value
		/// </summary>
		ConcurrentDictionary<string, string> _deferredStorageForRecipeParmater = new ConcurrentDictionary<string, string>();
		public bool SetDeferredStorage(string taskName, string parameterName, int indexOfItem, string newValue)
		{
			if (IsBlockedParameterChange(EN_RECIPE_TYPE.PROCESS))
			{
				//Functional.PostOffice.GetInstance().SendMail(Define.DefineEnumProject.Mail.EN_SUBSCRIBER.OperationMain
				//   , Define.DefineEnumProject.Mail.EN_MAIL.UI_ShowMessageBox, "Parameter change blocked");
				return false;
            }

			string key = string.Format("{0}.{1}.{2}", taskName.ToString(), parameterName, indexOfItem.ToString());
			string r = _deferredStorageForRecipeParmater.AddOrUpdate(key, newValue, (k, v) => { return newValue; });
			return r == newValue;
		}
		public bool SetDeferredStorage(string taskName, string parameterName, string newValue)
		{
			return SetDeferredStorage(taskName, parameterName, 0, newValue);
		}
		public bool GetDeferredStorage(string taskName, string parameterName, int indexOfItem, out string result)
		{
			string key = string.Format("{0}.{1}.{2}", taskName.ToString(), parameterName, indexOfItem.ToString());
			return _deferredStorageForRecipeParmater.TryGetValue(key, out result);
		}
		public bool GetDeferredStorage(string taskName, string parameterName, out string result)
		{
			return GetDeferredStorage(taskName, parameterName, 0, out result);
		}
		private void RemoveDeferredStorage(string taskName, string parameterName, int indexOfItem = 0)
		{
			string key = string.Format("{0}.{1}.{2}", taskName.ToString(), parameterName, indexOfItem.ToString());
			if (_deferredStorageForRecipeParmater.ContainsKey(key))
			{
				string s;
				_deferredStorageForRecipeParmater.TryRemove(key, out s);
			}
		}
		#endregion /recipe

		/// <summary>
		/// DeferredStorage에 저장되어 있는 내용 적용
		/// </summary>
		public async void ApplyDeferredStorage()
		{
			Dictionary<EN_RECIPE_TYPE, bool> includedType;
			var changedList = MakeParameterChangedList(out includedType);

			#region Parameter change block 여부 확인
			foreach (var kvp in includedType)
			{
				if (false == kvp.Value)
					continue;

				if (IsBlockedParameterChange(kvp.Key))
				{
					//Functional.PostOffice.GetInstance().SendMail(Define.DefineEnumProject.Mail.EN_SUBSCRIBER.OperationMain
					//   , Define.DefineEnumProject.Mail.EN_MAIL.UI_ShowMessageBox, "Parameter change blocked");

					// confirm 실패해도 결과 통지
					OnNotifyParameterChanged(false, null);
					return;
				}
			}
			#endregion /Parameter change block 여부 확인

			// confirm을 위한 block
			RequestParameterChangeBlock("RecipeConfirm");

			#region confirm
			/// 2024.05.29 by junho [NOTE] 제약사항
			/// 만약 secsgem으로 recipe change event 보냈는데 상위에서 confirm을 해야하는 상황에서,
			/// 상위는 OK 보내줬는데 다른쪽에서 confirm fail 때리면 상위에 바꾼다하고 안바뀌는 상황이 발생함.
			/// 그럴땐 event를 2개로 나눠서 처리 해야 할 것 같은데 당장 하기엔 너무 일찍 일어난 새일거같아서 배제시킴
			var taskConfirm = OnIsParameterChangeConfirmedAsync(changedList);
			bool waitResult = await taskConfirm;
			if (false == waitResult)
			{
				// confirm을 위한 block 해제
				foreach (EN_RECIPE_TYPE en in Enum.GetValues(typeof(EN_RECIPE_TYPE)))
					RetractParameterChangeBlock(en, "RecipeConfirm");

				// confirm 실패해도 결과 통지
				OnNotifyParameterChanged(false, null);
				return;
			}
			#endregion /confirm

			#region apply
			bool result = true;
			foreach (var item in changedList)
			{
				switch(item.Type)
				{
					case EN_RECIPE_TYPE.COMMON:
					case EN_RECIPE_TYPE.EQUIPMENT:
						result &= SetValue(item.Type, item.ParameterName, item.Index, EN_RECIPE_PARAM_TYPE.VALUE, item.Value, doNotify:false);
						break;
					case EN_RECIPE_TYPE.PROCESS:
						result &= SetValue(item.TaskName, item.ParameterName, item.Index, EN_RECIPE_PARAM_TYPE.VALUE, item.Value, doNotify:false);
						break;
					default: continue;
				}
			}
			#endregion /apply

			// confirm을 위한 block 해제
			RetractParameterChangeBlock("RecipeConfirm");

			// notify
			OnNotifyParameterChanged(true, changedList);
		}
		private List<ParameterItem> MakeParameterChangedList(out Dictionary<EN_RECIPE_TYPE, bool> includedType)
		{
			includedType = new Dictionary<EN_RECIPE_TYPE, bool>();
			foreach (EN_RECIPE_TYPE en in Enum.GetValues(typeof(EN_RECIPE_TYPE)))
				includedType.Add(en, false);

			string parameterName;
			int indexOfItem;
			string newValue;

			List<ParameterItem> changedList = new List<ParameterItem>();

			EN_RECIPE_TYPE type;
			foreach (var kvp in _deferredStorageForEcParmater)
			{
				string[] splited = kvp.Key.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
				try
				{
					if (false == Enum.TryParse(splited[0], out type))
						continue;

					parameterName = splited[1];

					if (false == int.TryParse(splited[2], out indexOfItem))
						continue;

					newValue = kvp.Value;
				}
				catch { continue; }

				changedList.Add(new ParameterItem(type, parameterName, indexOfItem, newValue));
				includedType[type] = true;
			}

			string taskName;
			foreach (var kvp in _deferredStorageForRecipeParmater)
			{
				string[] splited = kvp.Key.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
				try
				{
					taskName = splited[0];
					parameterName = splited[1];

					if (false == int.TryParse(splited[2], out indexOfItem))
						continue;
					newValue = kvp.Value;
				}
				catch { continue; }

				changedList.Add(new ParameterItem(taskName, parameterName, indexOfItem, newValue));
				includedType[EN_RECIPE_TYPE.PROCESS] = true;
			}
			return changedList;
		}

		/// <summary>
		/// DeferredStorage에 저장되어 있는 내용 취소
		/// 
		/// 2024.09.19 by junho [MOD] 항상 notify event 발생시키도록 변경
		///  public void ClearDeferredStorage(bool isNotify)
		/// </summary>
		public void ClearDeferredStorage()
		{
			Dictionary<EN_RECIPE_TYPE, bool> includedType;
			var changedList = MakeParameterChangedList(out includedType);

			_deferredStorageForEcParmater.Clear();
			_deferredStorageForRecipeParmater.Clear();
			
			OnNotifyParameterChanged(false, changedList);
		}

		#region parameter change confirm/notify
		public class ParameterItem
		{
			private ParameterItem(string parameterName, int index, string value)
			{
				ParameterName = parameterName;
				Value = value;
				Index = index;
			}

			public ParameterItem(EN_RECIPE_TYPE type, string parameterName, int index, string value)
				: this(parameterName, index, value)
			{
				Type = type;
				TaskName = "";
			}
			public ParameterItem(string taskName, string parameterName, int index, string value)
				: this(parameterName, index, value)
			{
				Type = EN_RECIPE_TYPE.PROCESS;
				TaskName = taskName;
			}

			public ParameterItem(EN_RECIPE_TYPE type, string parameterName, string value) : this(type, parameterName, 0, value) { }
			public ParameterItem(string taskName, string parameterName, string value) : this(taskName, parameterName, 0, value) { }

			public ParameterItem(ParameterItem source)
			{
				TaskName = source.TaskName;
				Type = source.Type;
				ParameterName = source.ParameterName;
				Value = source.Value;
			}

			public string TaskName { get; set; }
			public EN_RECIPE_TYPE Type { get; set; }
			public string ParameterName { get; set; }
			public string Value { get; set; }
			public int Index { get; set; }
		}

		#region notify
		public delegate void DeleParameterChangedNotify(bool result, List<ParameterItem> changedList);
		/// <summary>
		/// Parameter 변경사항이 적용이 된 후에 발생하는 이벤트
		/// </summary>
		public static event DeleParameterChangedNotify ParameterChangedNotify = null;
		/// <summary>
		/// Parameter 변경사항을 알리는 이벤트 발생 인터페이스
		/// </summary>
		private void OnNotifyParameterChanged(bool result, List<ParameterItem> changedList)
		{
			if (ParameterChangedNotify != null)
			{
				if (changedList == null)
					changedList = new List<ParameterItem>();

				ParameterChangedNotify(result, changedList);
			}
		}
		#endregion /notify

		#region confirm
		public delegate SystemTasks.Task<bool> DeleParameterChangeConfirm(List<ParameterItem> changeList, SystemTasks.TaskCompletionSource<bool> ayncWaitResult);
		/// <summary>
		/// Parameter 변경사항 적용 전에 발생하는 이벤트
		/// </summary>
		public static event DeleParameterChangeConfirm ParameterChangeConfirm = null;
		/// <summary>
		/// changeList를 통지하여 구독자들의 반환값을 AND연산하여 반환한다. (대기 가능)
		/// </summary>
		private async SystemTasks.Task<bool> OnIsParameterChangeConfirmedAsync(List<ParameterItem> changeList)
		{
			if (ParameterChangeConfirm == null)
				return true;

			var deleList = ParameterChangeConfirm.GetInvocationList().Cast<DeleParameterChangeConfirm>().ToArray();
			SystemTasks.Task<bool>[] tasks = deleList.Select(f => f(changeList, new SystemTasks.TaskCompletionSource<bool>())).ToArray();
			bool[] results = await SystemTasks.Task.WhenAll(tasks);

			return results.Aggregate((acc, x) => acc && x);
		}
		#endregion /confirm

		#region notify about each
		// 2024.09.12 by junho [ADD] 각각의 parameter가 변경될 떄 발생하는 event 추가
		class EachParameterChangedEventManager
		{
			Dictionary<string, EachParameterChangedEvent> _eventList = new Dictionary<string, EachParameterChangedEvent>();

			public void Init()
			{
				_eventList.Clear();
				foreach (PARAM_COMMON en in Enum.GetValues(typeof(PARAM_COMMON)))
				{
					var key = string.Format("{0}.{1}", EN_RECIPE_TYPE.COMMON.ToString(), en.ToString());
					_eventList.Add(key, new EachParameterChangedEvent());
				}
				foreach (PARAM_EQUIPMENT en in Enum.GetValues(typeof(PARAM_EQUIPMENT)))
				{
					var key = string.Format("{0}.{1}", EN_RECIPE_TYPE.EQUIPMENT.ToString(), en.ToString());
					_eventList.Add(key, new EachParameterChangedEvent());
				}
				foreach (Define.DefineEnumProject.Task.EN_TASK_LIST enTask in Enum.GetValues(typeof(Define.DefineEnumProject.Task.EN_TASK_LIST)))
				{
					Type t = FrameOfSystem3.Component.ConvertProjectTaskData.GetDefinedParameter(enTask);
					if (t == null || false == t.IsEnum) continue;
					foreach (Enum en in Enum.GetValues(t))
					{
						var key = string.Format("{0}.{1}", enTask.ToString(), en.ToString());
						_eventList.Add(key, new EachParameterChangedEvent());
					}
				}
			}

			public void OnNotify(string key, string value)
			{
				if (false == _eventList.ContainsKey(key))
					return;

				_eventList[key].OnNotify(value);
			}
			public void OnNotify(EN_RECIPE_TYPE type, string name, string value)
			{
				OnNotify(string.Format("{0}.{1}", type.ToString(), name), value);
			}
			public void OnNotify(string taskName, string name, string value)
			{
				OnNotify(string.Format("{0}.{1}", taskName, name), value);
			}

			public void Register(string key, DeleParameterChangedCallback cb)
			{
				if (false == _eventList.ContainsKey(key))
					return;

				_eventList[key].Register(cb);
			}
			public void Deregister(string key, DeleParameterChangedCallback cb)
			{
				if (false == _eventList.ContainsKey(key))
					return;

				_eventList[key].Deregister(cb);
			}

			class EachParameterChangedEvent
			{
				event DeleParameterChangedCallback Notify;
				public void Register(DeleParameterChangedCallback cb)
				{
					Notify += cb;
				}
				public void Deregister(DeleParameterChangedCallback cb)
				{
					Notify -= cb;
				}
				public void OnNotify(string value)
				{
					if (Notify != null)
						Notify(value);
				}
			}
		}
		public delegate void DeleParameterChangedCallback(string value);
		public void RegisterEachParameterChangedEvent(string type, string name, DeleParameterChangedCallback cb)
		{
			_eachParameterChangedEventManager.Register(string.Format("{0}.{1}", type, name), cb);
		}
		public void DeregisterEachParameterChangedEvent(string type, string name, DeleParameterChangedCallback cb)
		{
			_eachParameterChangedEventManager.Deregister(string.Format("{0}.{1}", type, name), cb);
		}

		private void OnEachParameterChangedNotify(bool result, List<ParameterItem> changedList)
		{
			foreach (var item in changedList)
			{
				switch (item.Type)
				{
					case EN_RECIPE_TYPE.COMMON:
					case EN_RECIPE_TYPE.EQUIPMENT:
						_eachParameterChangedEventManager.OnNotify(item.Type, item.ParameterName, item.Value);
						break;
					case EN_RECIPE_TYPE.PROCESS:
						_eachParameterChangedEventManager.OnNotify(item.TaskName, item.ParameterName, item.Value);
						break;
					default:
						break;
					}
			}
		}
		#endregion /notify about each

		#endregion /parameter change confirm/notify

		#region parameter change block
		ConcurrentDictionary<EN_RECIPE_TYPE, List<string>> _parameteChangeBlock = new ConcurrentDictionary<EN_RECIPE_TYPE, List<string>>();
		/// <summary>
		/// Parameter 변경과 변경사항 적용 모두 block
		/// </summary>
		public void RequestParameterChangeBlock(EN_RECIPE_TYPE type, string requester)
		{
			if (false == _parameteChangeBlock[type].Contains(requester))
				_parameteChangeBlock[type].Add(requester);
		}
		/// <summary>
		/// Parameter 변경과 변경사항 적용 모두 block
		/// </summary>
		public void RequestParameterChangeBlock(string requester)
		{
			RequestParameterChangeBlock(EN_RECIPE_TYPE.COMMON, requester);
			RequestParameterChangeBlock(EN_RECIPE_TYPE.EQUIPMENT, requester);
			RequestParameterChangeBlock(EN_RECIPE_TYPE.PROCESS, requester);
		}

		/// <summary>
		/// Parameter 변경과 변경사항 적용 block 해제
		/// </summary>
		public void RetractParameterChangeBlock(EN_RECIPE_TYPE type, string requester)
		{
			if (_parameteChangeBlock[type].Contains(requester))
				_parameteChangeBlock[type].Remove(requester);
		}
		/// <summary>
		/// Parameter 변경과 변경사항 적용 block 해제
		/// </summary>
		public void RetractParameterChangeBlock(string requester)
		{
			RetractParameterChangeBlock(EN_RECIPE_TYPE.COMMON, requester);
			RetractParameterChangeBlock(EN_RECIPE_TYPE.EQUIPMENT, requester);
			RetractParameterChangeBlock(EN_RECIPE_TYPE.PROCESS, requester);
		}

		/// <summary>
		/// Parameter 변경 금지를 요청한 명단 반환
		/// </summary>
		public List<string> GetParameterChangeBlockList(EN_RECIPE_TYPE type)
		{
			var result = new List<string>();

			if (false == _parameteChangeBlock.ContainsKey(type))
				return result;

			foreach (string name in _parameteChangeBlock[type])
				result.Add(name);
			
			return result;
		}
		/// <summary>
		/// Parameter 변경이 금지되어 있는지 반환
		/// </summary>
		private bool IsBlockedParameterChange(EN_RECIPE_TYPE type)
		{
			return _parameteChangeBlock[type].Count > 0;
		}
		/// <summary>
		/// 강제로 Parameter 변경 금지 해제
		/// </summary>
		public void ClearParameterChangeBlockList()
		{
			foreach(var kvp in _parameteChangeBlock)
			{
				kvp.Value.Clear();
			}
		}
		#endregion /parameter change block

		#endregion /DeferredStorage

		#endregion /External Interface
	}
}
