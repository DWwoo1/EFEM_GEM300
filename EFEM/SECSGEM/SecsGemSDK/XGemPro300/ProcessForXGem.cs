using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.SecsGemDll
{
    class ProcessForXGem
    {
        #region <Constructors>
        public ProcessForXGem()
        {
            UseUserDefinedFormattedRecipeControl = false;
            UseUserDefinedCollectionEventControl = false;
            RecipeHandlingPath = string.Empty;

            CurrentControlStatus = EN_CONTROL_STATE.HOST_OFFLINE;
            InitControlStatus = EN_CONTROL_STATE.HOST_OFFLINE;
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly string PROJECT_PATH_SECTION = "XGEM";
        private readonly string PROJECT_PATH_KEY = "ProjectPath";

        private readonly string HANDLING_PATH = @"\XWork\Recipe\";

        private readonly string ALARM_EVENT_SECTION = "ALARM_EVENT";
        private readonly string ALARM_EVENT_USE_KEY = "ALARM_EVENT_USE";
        private readonly string ALARM_EVENT_SET_KEY = "ALARM_SET_EVENT_ID";
        private readonly string ALARM_EVENT_CLEAR_KEY = "ALARM_CLEAR_EVENT_ID";

        private readonly string INIT_CONTROL_STATE = "CONTROL_STATE";
        private readonly string INIT_CONTROL_STATE_KEY = "INIT_CONTROL_STATE";

        private readonly string USER_DEFINED_FORMATTED_RECIPE_CONTROL_SECTION = "USER_DEFINED_FORMATTED_RECIPE_CONTROL";
        private readonly string USER_DEFINED_FORMATTED_RECIPE_CONTROL_USE_KEY = "USER_DEFINED_FORMATTED_RECIPE_CONTROL_USE";

        private const string NameOfStatusVariableConfig = "StatusVariableListConfig.txt";
        private const string NameOfReportListConfig = "ReportListConfig.txt";
        private const string NameOfCollectionEventConfig = "CollectionEventConfig.txt";
        private const string NameOfEquipConstantConfig = "EquipmentConstantListConfig.txt";
        #endregion </Fields>

        #region <Properties>
        public string CommunicationStatusAsString { get; private set; }
        public EN_COMM_STATE CommunicationStatus { get; private set; }
        public EN_CONTROL_STATE CurrentControlStatus { get; private set; }
        public EN_CONTROL_STATE InitControlStatus { get; private set; }
        public string RecipeHandlingPath { get; private set; }
        public bool UseUserDefinedFormattedRecipeControl { get; private set; }
        public bool UseUserDefinedCollectionEventControl { get; private set; }
        public long AlarmSetEvent { get; private set; }
        public long AlarmClearEvent { get; private set; }

        #endregion </Properties>

        #region <Methods>

        #region <Gem Information>
        public void InitGemInfo(string strIni)
        {
            Functional.IniControl ini = new Functional.IniControl(strIni);

            //if (false == string.IsNullOrEmpty(projectPath))
            //{
            //    ini.WriteString(PROJECT_PATH_SECTION, PROJECT_PATH_KEY, projectPath);
            //}

            // 레시피경로 설정
            string strTemp = ini.GetString(PROJECT_PATH_SECTION, PROJECT_PATH_KEY, "");
            if (string.IsNullOrEmpty(strTemp))
            {
                strTemp = @"XGem\PPBODY\";
            }
            string path = Path.GetDirectoryName(strTemp);
            RecipeHandlingPath = string.Format(@"{0}{1}", path, HANDLING_PATH);

            if (false == Directory.Exists(RecipeHandlingPath))
            {
                Directory.CreateDirectory(RecipeHandlingPath);
            }


            strTemp = ini.GetString(INIT_CONTROL_STATE, INIT_CONTROL_STATE_KEY, EN_CONTROL_STATE.LOCAL.ToString());

            EN_CONTROL_STATE state;
            if (false == Enum.TryParse(strTemp, out state))
                state = EN_CONTROL_STATE.LOCAL;

            InitControlStatus = state;

            UseUserDefinedCollectionEventControl = ini.GetBool(ALARM_EVENT_SECTION, ALARM_EVENT_USE_KEY, false);

            AlarmSetEvent = ini.GetLong(ALARM_EVENT_SECTION, ALARM_EVENT_SET_KEY, -1);
            AlarmClearEvent = ini.GetLong(ALARM_EVENT_SECTION, ALARM_EVENT_CLEAR_KEY, -1);
            if (AlarmSetEvent < 0 || AlarmClearEvent < 0)
                UseUserDefinedCollectionEventControl = false;

            UseUserDefinedFormattedRecipeControl = ini.GetBool(USER_DEFINED_FORMATTED_RECIPE_CONTROL_SECTION, USER_DEFINED_FORMATTED_RECIPE_CONTROL_USE_KEY, false);
        }
        public void SetCommunicationStatus(long state)
        {
            CommunicationStatus = (EN_COMM_STATE)state;
        }
        public void SetControlStatus(long state)
        {
            CurrentControlStatus = (EN_CONTROL_STATE)state;
        }
        #endregion </Gem Information>

        #region <Config>
        public bool MakeGemReportList(string configDirectory, Dictionary<string, StatusVariable> statusVariableList, out Dictionary<long, List<StatusVariable>> reportList)
        {
            reportList = new Dictionary<long, List<StatusVariable>>();

            // RptId
            string reportFilePath = string.Format(@"{0}\Config\{1}", configDirectory, NameOfReportListConfig);
            if (false == File.Exists(reportFilePath))
                return false;

            reportList.Clear();

            StreamReader srReportId = null;
            try
            {
                srReportId = new StreamReader(reportFilePath);

                string readAll = srReportId.ReadToEnd();
                srReportId.Close();

                string[] line = readAll.Split('\n');
                for (int i = 0; i < line.Length; ++i)
                {
                    string temporaryLine = line[i].Replace("\r", "");

                    string[] keyValue = line[i].Split('\t');
                    if (keyValue.Length < 3)
                        continue;

                    long id;
                    if (false == long.TryParse(keyValue[0], out id))
                        continue;

                    string[] values = keyValue[2].Replace('"', ' ').Split(',');
                    List<StatusVariable> svidList = new List<StatusVariable>();
                    for (int sv = 0; sv < values.Length; ++sv)
                    {
                        long svid;
                        if (false == long.TryParse(values[sv], out svid))
                            continue;

                        StatusVariable statusVariable = null;
                        foreach (var item in statusVariableList)
                        {
                            if (item.Value.Id.Equals(svid))
                            {
                                statusVariable = item.Value;
                                break;
                            }
                        }

                        svidList.Add(statusVariable);
                    }

                    reportList[id] = svidList;
                }
            }
            catch
            {
                if (srReportId != null)
                    srReportId.Close();

                reportList.Clear();
            }

            return (reportList.Count > 0);
        }
        public bool MakeGemVariableList(string configDirectory, out Dictionary<string, StatusVariable> statusVariables)
        {
            // SVID
            string svidFilePath = string.Format(@"{0}\Config\{1}", configDirectory, NameOfStatusVariableConfig);
            statusVariables = new Dictionary<string, StatusVariable>();
            StreamReader srSvid = null;
            try
            {
                srSvid = new StreamReader(svidFilePath);

                string readAll = srSvid.ReadToEnd();
                srSvid.Close();

                string[] line = readAll.Split('\n');
                for (int i = 0; i < line.Length; ++i)
                {
                    string temporaryLine = line[i].Replace("\r", "");
                    string[] keyValue = line[i].Split('\t');
                    if (keyValue.Length < 2)
                        continue;

                    long id;
                    if (false == long.TryParse(keyValue[0], out id))
                        continue;

                    string name = keyValue[1];
                    statusVariables[name] = new StatusVariable(id, name);
                }
            }
            catch
            {
                if (srSvid != null)
                    srSvid.Close();

                statusVariables.Clear();
            }

            return (statusVariables.Count > 0);
        }
        public bool MakeGemEventList(string configDirectory, Dictionary<long, List<StatusVariable>> reportList, out Dictionary<string, CollectionEvent> collectionEventList)
        {
            collectionEventList = new Dictionary<string, CollectionEvent>();

            // CEID
            string ceidFilePath = string.Format(@"{0}\Config\{1}", configDirectory, NameOfCollectionEventConfig);

            StreamReader srCeidId = null;
            try
            {
                srCeidId = new StreamReader(ceidFilePath);

                string readAll = srCeidId.ReadToEnd();
                srCeidId.Close();

                string[] line = readAll.Split('\n');
                for (int i = 0; i < line.Length; ++i)
                {
                    string temporaryLine = line[i].Replace("\r", "");

                    string[] keyValue = temporaryLine.Split('\t');

                    long id;
                    if (false == long.TryParse(keyValue[0], out id))
                        continue;

                    string name = keyValue[1];

                    // RPT
                    string[] values = keyValue[4].Replace('"', ' ').Split(',');
                    Dictionary<long, StatusVariable> svidInCeid = new Dictionary<long, StatusVariable>();
                    for (int rpt = 0; rpt < values.Length; ++rpt)
                    {
                        long rptId;
                        if (false == long.TryParse(values[rpt], out rptId))
                            continue;

                        if (false == reportList.ContainsKey(rptId))
                            continue;

                        for (int sv = 0; sv < reportList[rptId].Count; ++sv)
                        {
                            long vid = reportList[rptId][sv].Id;
                            svidInCeid[vid] = reportList[rptId][sv];
                        }
                    }

                    //bool useConfirm = false;
                    //if (false == customScenario)
                    //{
                    //    if (false == bool.TryParse(keyValue[3], out useConfirm))
                    //        continue;
                    //}

                    collectionEventList[name] = new CollectionEvent(id, svidInCeid);
                }
            }
            catch
            {
                if (srCeidId != null)
                    srCeidId.Close();

                collectionEventList.Clear();
                return false;
            }

            return collectionEventList.Count > 0;
        }

        //24.09.27 by wdw [ADD] EQUIPMNET CONSTANT 추가
        public bool MakeGemConstantList(string configDirectory, out Dictionary<string, EquipmentConstant> equipmentConstantList)
        {
            // ECV
            string ecidFilePath = string.Format(@"{0}\Config\{1}", configDirectory, NameOfEquipConstantConfig);

            equipmentConstantList = new Dictionary<string, EquipmentConstant>();

            StreamReader srEcid = null;
            try
            {
                srEcid = new StreamReader(ecidFilePath);

                string readAll = srEcid.ReadToEnd();
                srEcid.Close();

                string[] line = readAll.Split('\n');
                for (int i = 0; i < line.Length; ++i)
                {
                    string temporaryLine = line[i].Replace("\r", "");
                    string[] keyValue = line[i].Split('\t');
                    if (keyValue.Length < 2)
                        continue;

                    long id;
                    if (false == long.TryParse(keyValue[0], out id))
                        continue;

                    string name = keyValue[1];
                    equipmentConstantList[name] = new EquipmentConstant(id, name);
                }
            }
            catch
            {
                if (srEcid != null)
                    srEcid.Close();

                equipmentConstantList.Clear();
                return false;
            }

            return (equipmentConstantList.Count > 0);
        }
        #endregion </Config>

        #endregion </Methods>
    }
}
