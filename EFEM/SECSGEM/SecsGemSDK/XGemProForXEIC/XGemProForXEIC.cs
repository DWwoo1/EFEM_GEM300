using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;

using Define.DefineConstant;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

using XGEMPRO.Library;

namespace FrameOfSystem3.SECSGEM.SecsGemDll
{
    class XGemProForXEIC : SecsGem
    {
        #region <Constructors>
        #endregion </Constructors>

        #region <Types>
        #endregion </Types>

        #region <Fields>
        private XGemProW _gemDriver = null;
        private int _countOfClients;

        private readonly string PROJECT_PATH_SECTION = "XGEM";
        private readonly string PROJECT_PATH_KEY = "ProjectPath";

        private readonly string RECIPE_PATH_SECTION = "CLIENT_RECIPE_PATH"; // 레시피 경로(공유폴더) 읽어올 섹션
        private readonly string RECIPE_PATH_COUNT_KEY = "DEVICE_COUNT";     // 클라이언트별 레시피 경로(1부터 시작.. DEVICE_1, 2, 3..)
        private readonly string RECIPE_PATH_KEY = "DEVICE";     // 클라이언트별 레시피 경로(1부터 시작.. DEVICE_1, 2, 3..)

        private readonly string HANDLING_PATH = @"\XWork\Recipe\";

        private readonly string DEVICE_INFO_SECTION = "DEVICE_INFO";
        private readonly string DEVICE_INFO_CLIENT_TYPE_MASTER_KEY = "CLIENT_TYPE_MASTER";

        private readonly string ALARM_EVENT_SECTION = "ALARM_EVENT";
        private readonly string ALARM_EVENT_USE_KEY = "ALARM_EVENT_USE";
        private readonly string ALARM_EVENT_SET_KEY = "ALARM_SET_EVENT_ID";
        private readonly string ALARM_EVENT_CLEAR_KEY = "ALARM_CLEAR_EVENT_ID";

        private readonly string INIT_CONTROL_STATE = "CONTROL_STATE";
        private readonly string INIT_CONTROL_STATE_KEY = "INIT_CONTROL_STATE";

        private readonly string USER_DEFINED_FORMATTED_RECIPE_CONTROL_SECTION = "USER_DEFINED_FORMATTED_RECIPE_CONTROL";
        private readonly string USER_DEFINED_FORMATTED_RECIPE_CONTROL_USE_KEY = "USER_DEFINED_FORMATTED_RECIPE_CONTROL_USE";

        private bool _isMaster = false;
        private string _recipeHandlingPath = String.Empty;
        private string[] _recipePathInfos = null;
        private EN_CONTROL_STATE _initControlState = EN_CONTROL_STATE.HOST_OFFLINE;

        private bool _useUserDefinedCollectionEventControl = false;
        private long _alarmSetEvent;
        private long _alarmClearEvent;

        private bool _useUserDefinedFormattedRecipeControl = false;
        //Thread checkReceiving;
        //private string MDB_Password = "B1594C47";

        private ConcurrentDictionary<long, bool> _eventToSend = new ConcurrentDictionary<long, bool>();

        #region CommStatus
        private string m_strCommStatus = String.Empty;
        private EN_COMM_STATE _commState;
        #endregion

        #region ControlStatus
        //string m_strControlStatus = String.Empty;
        private EN_CONTROL_STATE _controlState;
        #endregion

        #endregion </Fields>

        #region <Methods>

        #region <Abstract Interface>

        #region <Initialize & Close>
        public override bool Init(string configPath)
        {
            //Enum.TryParse(controlState, out _initControlState);

            //_initControlState = EN_CONTROL_STATE.HOST_OFFLINE;

            // 변수값 초기화
            _controlState = EN_CONTROL_STATE.OFFLINE;

            SetCommStatus((long)EN_COMM_STATE.DISABLED);

            // m_XGem = new XGemProW();
            _gemDriver = new XGemProW();

            InitGemInfo(configPath);
            
            LinkFunctions();

            long nResult = _gemDriver.Initialize(configPath);
            if (0 != nResult)
            {
                WriteLog(String.Format("Gem Init Fail : {0}", nResult));
                return false;
            }

            System.Threading.Thread.Sleep(1000);

            nResult = _gemDriver.Start();
            if (0 != nResult)
            {
                WriteLog(String.Format("Gem Start Fail : {0}", nResult));
                return false;
            }

            return true;
        }
        public override void Close()
        {
            if (_gemDriver == null) return;

            long nResult = _gemDriver.Stop();
            if (0 != nResult)
            {
                WriteLog(String.Format("Gem Stop Fail : {0}", nResult));
            }

            System.Threading.Thread.Sleep(1000);

            nResult = _gemDriver.Close();
            if (0 != nResult)
            {
                WriteLog(String.Format("Gem Close Fail : {0}", nResult));
            }
        }
        #endregion <Initialize & Close>

        #region <Comm & Control State>
        public override void SetCommStateEnabled()
        {
            _gemDriver.GEMSetEstablish(1);
        }

        public override void SetCommStateDisabled()
        {
            _gemDriver.GEMSetEstablish(0);
        }

        public override EN_COMM_STATE GetCommState()
        {
            return _commState;
        }

        public override void SetInitControlState(EN_CONTROL_STATE enStatus)
        {
        }

        public override void SetControlState(EN_CONTROL_STATE enStatus)
        {
            switch (enStatus)
            {
                case EN_CONTROL_STATE.OFFLINE:
                    // 오프라인인 경우 변경 전에 이벤트를 업데이트해야한다.
                    // 나머지는 컨트롤 변경 후 콜백 함수에서 모두 처리가 가능하다.
                    base.ControlState(enStatus.ToString());
                    _gemDriver.GEMReqOffline();
                    break;
                case EN_CONTROL_STATE.LOCAL:
                    _gemDriver.GEMReqLocal();
                    //base.ControlState(enStatus.ToString());
                    break;
                case EN_CONTROL_STATE.REMOTE:
                    _gemDriver.GEMReqRemote();
                    //base.ControlState(enStatus.ToString());
                    break;
                case EN_CONTROL_STATE.HOST_OFFLINE:
                    _gemDriver.GEMReqHostOffline();
                    //base.ControlState(enStatus.ToString());
                    break;
            }
        }

        public override EN_CONTROL_STATE GetControlState()
        {
            return _controlState;
        }
        #endregion </Comm & Control State>

        #region <Alarm>
        public override void SetAlarm(int nAlarm)
        {
            _gemDriver.GEMSetAlarm(nAlarm, 1);
        }

        public override void ClearAlarm(int nAlarm)
        {
            _gemDriver.GEMSetAlarm(nAlarm, 0);
        }
        #endregion </Alarm>

        #region <Client To Client Message>
        public override bool SendClientToClientMessage(string device, string messageName, string sendingType, string scenarioName, string[] contentNames, string[] messages, EN_MESSAGE_RESULT result)
        {
            return SendSignal(device, messageName, sendingType, scenarioName, contentNames, messages, result) == 0 ? true : false;
        }
        #endregion </Client To Client Message>

        #region <User Defined SecsMessage>
        public override bool SendUserDefinedSecsMessage(string messageName, List<SemiObject> messageStructure)
        {
            if (false == _userDefinedSecsMessage.ContainsKey(messageName))
                return false;

            long stream = _userDefinedSecsMessage[messageName].Stream;
            long function = _userDefinedSecsMessage[messageName].Function;

            UserDefinedSecsMessage messageToSend = new UserDefinedSecsMessage(messageName, stream, function, messageStructure);
            long pObjectId = 0, pSystemByte = 0;

            WriteLog(String.Format("[EQ ==> XGEM] Send UserDefindSecsMessage : S{0}F{1}", stream, function));

            return MakeAndSendSecsMessage(stream, function, ref pObjectId, ref pSystemByte, ref messageToSend);
        }

        #endregion </User Defined SecsMessage>

        #region <Collection Event>
        public override bool SendEvent(long nEventID, long[] arrVids, string[] arrVidValues)
        {
            if (nEventID > 0)
            {
                if (false == Connect) return false;

                _eventToSend.AddOrUpdate(nEventID, true, (k, v) => v = false);
                //                 if (false == m_dicEvent.ContainsKey(nEventID))
                //                 {
                //                     m_dicEvent.TryAdd(nEventID, false);
                //                 }
                //                 else
                //                 {
                //                     m_dicEvent[nEventID] = false;
                //                 }

                _gemDriver.GEMSetEventEx(nEventID, arrVids.Length, arrVids, arrVidValues);
                string strLog = String.Format("[EQ ==> XGEM] S6F11 : {0}, Vids : ", nEventID);
                for (int i = 0; i < arrVids.Length; ++i)
                {
                    strLog = String.Format("{0}[{1} : {2}] ", strLog, arrVids[i], arrVidValues[i]);
                }

                WriteLog(strLog);

                return true;
            }

            return false;
        }

        public override bool IsEventDone(long nEventID)
        {
            if (false == _eventToSend.ContainsKey(nEventID))
                return false;
            else
            {
                return _eventToSend[nEventID];
            }
        }

        #endregion </Collection Event>

        #region <Recipe>

        #region <Uploading>
        // S7, F3 : [EQ -> Host] 레시피를 업로드한다.
        public override void ReqUploadingUnformattedRecipe(string recipeName)
        {
            System.Threading.Tasks.Task.Run(()
                => UploadingUnformattedRecipeAsync(-1, recipeName));

            //if (PrepareUploadingRecipe(recipeName))
            //{
            //    if (_gemDriver.GEMReqPPSendEx(recipeName, path) == 0)
            //    {
            //        WriteLog("[EQ ==> XGEM] GEMReqPPSendEx successfully");
            //    }
            //    else
            //    {
            //        WriteLog("[EQ ==> XGEM] Fail to GEMReqPPSendEx");
            //    }
            //}
            //else
            //{
            //    _gemDriver.GEMReqPPSendEx(null, null);
            //    WriteLog("[EQ ==> XGEM] Fail to GEMReqPPSendEx");
            //}
        }

        public override void ReqUploadingFormattedRecipe(string recipeName, Dictionary<string, string[]> recipeBodies)
        {
            //if (PrepareUploadingRecipe(recipeName))
            //{
            //    if (_gemDriver.GEMReqPPSendEx(recipeName, path) == 0)
            //    {
            //        WriteLog("[EQ ==> XGEM] GEMReqPPSendEx successfully");
            //    }
            //    else
            //    {
            //        WriteLog("[EQ ==> XGEM] Fail to GEMReqPPSendEx");
            //    }
            //}
            //else
            //{
            //    _gemDriver.GEMReqPPSendEx(null, null);
            //    WriteLog("[EQ ==> XGEM] Fail to GEMReqPPSendEx");
            //}

            string sLog = String.Format("[EQ ==> XGEM] OnGEMReqPPFmt : Ppid({0})", recipeName);
            WriteLog(sLog);

            string modelName = "";
            string softwareVersion = "";
            modelName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;

            System.Diagnostics.FileVersionInfo fv
                = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            softwareVersion = fv.ProductVersion.ToString();


            long count = recipeBodies.Count;
            string[] ccode = new string[count];
            long[] paramCount = new long[count];
            List<string> pparams = new List<string>();

            int indexCCode = 0;
            foreach (KeyValuePair<string, string[]> ccodes in recipeBodies)
            {
                int countPParam = ccodes.Value.Length;

                ccode[indexCCode] = ccodes.Key;
                paramCount[indexCCode] = countPParam;

                for (int i = 0; i < countPParam; ++i)
                {
                    pparams.Add(ccodes.Value[i]);
                }

                ++indexCCode;
            }

            long result = _gemDriver.GEMReqPPFmtSend(recipeName, modelName, softwareVersion, count, ccode, paramCount, pparams.ToArray());

            sLog = String.Format("[EQ ==> XGEM] GEMReqPPFmtSend");
            WriteLog(sLog);
        }
        #endregion </Uploading>

        #region <Downloading>
        // S7, F5 : [EQ -> Host] 레시피 다운로드를 요청한다.
        public override void ReqDownloadingUnformattedRecipe(string recipeName)
        {
            long nResult = _gemDriver.GEMReqPP(recipeName);
            if (nResult == 0)
            {
                WriteLog("[EQ ==> XGEM] GEMReqPPEx successfully");
            }
            else
            {
                WriteLog(String.Format("[EQ ==> XGEM] Fail to GEMReqPPEx ({0})", nResult));
            }
        }

        public override void ReqDownloadingFormattedRecipe(string recipeName)
        {
            long result = _gemDriver.GEMReqPPFmt(recipeName);
            if (result == 0)
            {
                WriteLog("[EQ ==> XGEM] GEMReqPPFmt successfully");
            }
            else
            {
                WriteLog(String.Format("[EQ ==> XGEM] Fail to GEMReqPPFmt ({0})", result));
            }
        }
        #endregion </Downloading>

        #endregion </Recipe>

        #region <Update vid>
        public override void UpdateVariable(long vid, List<SemiObject> values)
        {
            int result = 0;
            long pObjectId = 0;

            result = _gemDriver.MakeObject(ref pObjectId);
            if (result != 0) return;

            int count = values.Count;
            string messageToSend = String.Empty; // 2022.03.24 by Thienvv [ADD]
            for (int i = 0; i < count; ++i)
            {
                SemiObject obj = values[i];

                #region Format
                switch (obj.Format)
                {
                    case EN_ITEM_FORMAT.LIST:
                        {
                            SemiObjectList listItem = obj as SemiObjectList;
                            if (listItem != null)
                            {
                                result = _gemDriver.SetListItem(pObjectId, listItem.GetValues()[0]);
                                messageToSend = String.Format("{0},{1}", messageToSend, listItem.GetValues()[0]);
                            }
                        }
                        break;

                    case EN_ITEM_FORMAT.ASCII:
                        {
                            SemiObjectAscii aItem = obj as SemiObjectAscii;
                            if (aItem != null)
                            {
                                result = _gemDriver.SetStringItem(pObjectId, aItem.GetValues()[0]);
                                messageToSend = String.Format("{0},{1}", messageToSend, aItem.GetValues()[0]);
                            }
                        }
                        break;

                    case EN_ITEM_FORMAT.BINARY:
                        {
                            SemiObjectBinary bItem = obj as SemiObjectBinary;
                            if (bItem != null)
                            {
                                result = _gemDriver.SetBinaryItem(pObjectId, bItem.GetValue());
                                messageToSend = String.Format("{0},{1}", messageToSend, bItem.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.BOOL:
                        {
                            SemiObjectBool boItem = obj as SemiObjectBool;
                            if (boItem != null)
                            {
                                result = _gemDriver.SetBoolItem(pObjectId, boItem.GetValue());
                                messageToSend = String.Format("{0},{1}", messageToSend, boItem.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FLOAT4:
                        {
                            SemiObjectFloat4 f4Item = obj as SemiObjectFloat4;
                            if (f4Item != null)
                            {
                                result = _gemDriver.SetFloat4Item(pObjectId, f4Item.GetValue());
                                messageToSend = String.Format("{0},{1}", messageToSend, f4Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FLOAT8:
                        {
                            SemiObjectFloat8 f8Item = obj as SemiObjectFloat8;
                            if (f8Item != null)
                            {
                                result = _gemDriver.SetFloat8Item(pObjectId, f8Item.GetValue());
                                messageToSend = String.Format("{0},{1}", messageToSend, f8Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT:
                        {
                            SemiObjectInt i1Item = obj as SemiObjectInt;
                            if (i1Item != null)
                            {
                                result = _gemDriver.SetInt1Item(pObjectId, i1Item.GetValue());
                                messageToSend = String.Format("{0},{1}", messageToSend, i1Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT2:
                        {
                            SemiObjectInt2 i2Item = obj as SemiObjectInt2;
                            if (i2Item != null)
                            {
                                result = _gemDriver.SetInt2Item(pObjectId, i2Item.GetValue());
                                messageToSend = String.Format("{0},{1}", i2Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT4:
                        {
                            SemiObjectInt4 i4Item = obj as SemiObjectInt4;
                            if (i4Item != null)
                            {
                                result = _gemDriver.SetInt4Item(pObjectId, i4Item.GetValue());
                                messageToSend = String.Format("{0},{1}", i4Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT8:
                        {
                            SemiObjectInt8 i8Item = obj as SemiObjectInt8;
                            if (i8Item != null)
                            {
                                result = _gemDriver.SetInt8Item(pObjectId, i8Item.GetValue());
                                messageToSend = String.Format("{0},{1}", messageToSend, i8Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT:
                        {
                            SemiObjectUInt ui1Item = obj as SemiObjectUInt;
                            if (ui1Item != null)
                            {
                                result = _gemDriver.SetUint1Item(pObjectId, ui1Item.GetValue());
                                messageToSend = String.Format("{0},{1}", messageToSend, ui1Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT2:
                        {
                            SemiObjectUInt2 ui2Item = obj as SemiObjectUInt2;
                            if (ui2Item != null)
                            {
                                result = _gemDriver.SetUint2Item(pObjectId, ui2Item.GetValue());
                                messageToSend = String.Format("{0},{1}", messageToSend, ui2Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT4:
                        {
                            SemiObjectUInt4 ui4Item = obj as SemiObjectUInt4;
                            if (ui4Item != null)
                            {
                                result = _gemDriver.SetUint4Item(pObjectId, ui4Item.GetValue());
                                messageToSend = String.Format("{0},{1}", messageToSend, ui4Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT8:
                        {
                            SemiObjectUInt8 ui8Item = obj as SemiObjectUInt8;
                            if (ui8Item != null)
                            {
                                result = _gemDriver.SetUint8Item(pObjectId, ui8Item.GetValue());
                                messageToSend = String.Format("{0},{1}", messageToSend, ui8Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FILE:
                        break;
                }
                #endregion

            }
            //nResult = _gemDriver.SetListItem(pObjId, messages.Length);
            //if (nResult != 0) return nResult;

            //string strArrToSend = ""; // 2022.03.24 by Thienvv [ADD]
            //for (int i = 0; i < messages.Length; i++)
            //{
            //    nResult = _gemDriver.SetAsciiItem(pObjId, messages[i]);
            //    strArrToSend += messages[i] + "; "; // // 2022.03.24 by Thienvv [ADD]
            //    if (nResult != 0) return nResult;
            //}

            if (messageToSend.Length > 1 && messageToSend.Substring(0, 1).Equals(","))
                messageToSend = messageToSend.Remove(0, 1);

            string sLog = String.Format("Set Vid Data > Index : {0}, Data : {1} ", vid, messageToSend);
            WriteLog(sLog);

            result = _gemDriver.GEMSetVariables(pObjectId, vid);
            if (result != 0)
            {
                WriteLog(String.Format("SetVariableEx Failed : {0}", result));
            }

            result = _gemDriver.CloseObject(pObjectId);
            if (result != 0)
            {
                WriteLog(String.Format("CloseObject Failed : {0}", result));
            }
        }

        public override void UpdateVariables(long[] arrVids, string[] arrValues)
        {
            _gemDriver.GEMSetVariable(arrVids.Length, arrVids, arrValues);
        }

        public override void UpdateECV(long[] arrECV, string[] arrValues)
        {
            int nCount = arrECV.Length;
            string strLog = String.Empty;
            for (int i = 0; i < nCount; ++i)
            {
                if (arrValues[i].Equals("True") || arrValues[i].Equals("ENABLE"))
                {
                    arrValues[i] = "1";
                }
                else if (arrValues[i].Equals("False") || arrValues[i].Equals("DISABLE"))
                {
                    arrValues[i] = "0";
                }

                strLog
                    = String.Format("[EQ ==> XGEM] GEMSetECVChanged => Ecid:{0}, Val:{1}",
                        arrECV[i], arrValues[i]);

                WriteLog(strLog);
            }

            _gemDriver.GEMSetECVChanged(nCount, arrECV, arrValues);
        }
        #endregion </Update vid>

        #region <Get Value (VID)>
        public override bool GetVariables(ref long[] arrVids, ref string[] arrValues)
        {
            if (arrVids.Length == 0)
                return true;

            //if (0 == m_XGem.GEMSetVariable(arrVids.Length, arrVids, arrValues))
            //    return true;
            long result = _gemDriver.GEMGetVariable(arrVids.Length, ref arrVids, ref arrValues);
            if (0 == result)
                return true;

            return false;
        }
        #endregion </Get Value (VID)>

        #endregion </Abstract Interface>

        #region <External Interface>

        #endregion </External Interface>

        #region <Internal Interface>

        #region Initialize

        #region <Gem Information>
        private void InitGemInfo(string strIni)
        {
            Functional.IniControl ini = new Functional.IniControl(strIni);

            string strTemp = String.Empty;

            //if (false == String.IsNullOrEmpty(projectPath))
            //{
            //    ini.WriteString(PROJECT_PATH_SECTION, PROJECT_PATH_KEY, projectPath);
            //}
            // 디바이스 번호를 읽어옴
            int intTemp = ini.GetInt(RECIPE_PATH_SECTION, RECIPE_PATH_COUNT_KEY, 0);
            if (intTemp < 1)
                intTemp = 3;

            _countOfClients = intTemp;
            _recipePathInfos = new string[_countOfClients];
            for (int i = 0; i < _countOfClients; ++i)
            {
                string key = String.Format("{0}_{1}", RECIPE_PATH_KEY, i);
                strTemp = ini.GetString(RECIPE_PATH_SECTION, key, "");
                if (String.IsNullOrEmpty(strTemp))
                {
                    strTemp = @"\\127.0.0.1\RMS\";
                }
                else
                {
                    _recipePathInfos[i] = strTemp;
                }
            }

            // 레시피경로 설정
            strTemp = ini.GetString(PROJECT_PATH_SECTION, PROJECT_PATH_KEY, "");
            if (String.IsNullOrEmpty(strTemp))
            {
                strTemp = @"XGem\PPBODY\";
            }
            string path = Path.GetDirectoryName(strTemp);
            _recipeHandlingPath = String.Format(@"{0}{1}", path, HANDLING_PATH);

            if (false == Directory.Exists(_recipeHandlingPath))
            {
                Directory.CreateDirectory(_recipeHandlingPath);
            }

            _isMaster = ini.GetBool(DEVICE_INFO_SECTION, DEVICE_INFO_CLIENT_TYPE_MASTER_KEY, false);

            strTemp = ini.GetString(INIT_CONTROL_STATE, INIT_CONTROL_STATE_KEY, EN_CONTROL_STATE.LOCAL.ToString());

            EN_CONTROL_STATE state;
            if (false == Enum.TryParse(strTemp, out state))
                state = EN_CONTROL_STATE.LOCAL;

            _initControlState = state;

            _useUserDefinedCollectionEventControl = ini.GetBool(ALARM_EVENT_SECTION, ALARM_EVENT_USE_KEY, false);

            _alarmSetEvent = ini.GetLong(ALARM_EVENT_SECTION, ALARM_EVENT_SET_KEY, -1);
            _alarmClearEvent = ini.GetLong(ALARM_EVENT_SECTION, ALARM_EVENT_CLEAR_KEY, -1);
            if (_alarmSetEvent < 0 || _alarmClearEvent < 0)
                _useUserDefinedCollectionEventControl = false;

            _useUserDefinedFormattedRecipeControl = ini.GetBool(USER_DEFINED_FORMATTED_RECIPE_CONTROL_SECTION, USER_DEFINED_FORMATTED_RECIPE_CONTROL_USE_KEY, false);
        }
        #endregion </Gem Information>

        #region Event Interface Registry
        private void LinkFunctions()
        {
            #region Received Secs/Gem Messages
            _gemDriver.OnSECSMessageReceived += new OnSECSMessageReceived(OnUserDefinedSECSMessageReceived);
            _gemDriver.OnGEMReqRemoteCommand += new OnGEMReqRemoteCommand(OnGEMReqRemoteCommand);
            _gemDriver.OnGEMSecondaryMsgReceived += new OnGEMSecondaryMsgReceived(OnGEMSecondaryMsgReceived);
            #endregion

            #region Communication
            _gemDriver.OnXGEMStateEvent += new OnXGEMStateEvent(OnXGEMStateEvent);
            _gemDriver.OnGEMCommStateChanged += new OnGEMCommStateChanged(OnGEMCommStateChanged);
            _gemDriver.OnGEMControlStateChanged += new OnGEMControlStateChanged(OnGEMControlStateChanged);
            _gemDriver.OnGEMReqOffline += new OnGEMReqOffline(OnGEMReqOffline);
            _gemDriver.OnGEMReqOnline += new OnGEMReqOnline(OnGEMReqOnline);
            #endregion

            #region Time
            _gemDriver.OnGEMReqGetDateTime += new OnGEMReqGetDateTime(OnGEMReqGetDateTime);
            _gemDriver.OnGEMRspGetDateTime += new OnGEMRspGetDateTime(OnGEMRspGetDateTime);
            _gemDriver.OnGEMReqDateTime += new OnGEMReqDateTime(OnGEMReqDateTime);
            #endregion

            if (_isMaster)
            {
                #region Recipe
                _gemDriver.OnGEMReqPPList += new OnGEMReqPPList(OnGEMReqPPList);
                _gemDriver.OnGEMReqPPLoadInquire += new OnGEMReqPPLoadInquire(OnGEMReqPPLoadInquire);
                //m_XGem.OnGEMRspPPLoadInquire += new OnGEMRspPPLoadInquire(OnGEMRspPPLoadInquire);
                _gemDriver.OnGEMReqPPDelete += new OnGEMReqPPDelete(OnGEMReqPPDelete);

                #region <Formatted>
                _gemDriver.OnGEMReqPPFmtSend += new OnGEMReqPPFmtSend(OnGEMReqPPFmtSend);
                _gemDriver.OnGEMRspPPFmtSend += new OnGEMRspPPFmtSend(OnGEMRspPPFmtSend);
                _gemDriver.OnGEMReqPPFmt += new OnGEMReqPPFmt(OnGEMReqPPFmt);
                _gemDriver.OnGEMRspPPFmt += new OnGEMRspPPFmt(OnGEMRspPPFmt);
                _gemDriver.OnGEMRspPPFmtVerification += new OnGEMRspPPFmtVerification(OnGEMRspPPFmtVerification);
                #endregion </Formatted>

                #region <Unformatted>
                // Upload
                _gemDriver.OnGEMReqPP += new OnGEMReqPP(OnGEMReqPP);
                _gemDriver.OnGEMRspPPSend += new OnGEMRspPPSend(OnGEMRspPPSend);

                // Ex
                _gemDriver.OnGEMReqPPEx += new OnGEMReqPPEx(OnGEMReqPPEx);
                _gemDriver.OnGEMRspPPSendEx += new OnGEMRspPPSendEx(OnGEMRspPPSendEx);

                // Download
                _gemDriver.OnGEMReqPPSend += new OnGEMReqPPSend(OnGEMReqPPSend);
                _gemDriver.OnGEMRspPP += new OnGEMRspPP(OnGEMRspPP);

                // Ex
                _gemDriver.OnGEMReqPPSendEx += new OnGEMReqPPSendEx(OnGEMReqPPSendEx);
                _gemDriver.OnGEMRspPPEx += new OnGEMRspPPEx(OnGEMRspPPEx);
                #endregion </Unformatted>

                #endregion
            }

            #region ECV
            _gemDriver.OnGEMReqChangeECV += new OnGEMReqChangeECV(OnGEMReqChangeECV);
            _gemDriver.OnGEMECVChanged += new OnGEMECVChanged(OnGEMECVChanged);
            _gemDriver.OnGEMRspAllECInfo += new OnGEMRspAllECInfo(OnGEMRspAllECInfo);
            #endregion

            #region Terminal
            _gemDriver.OnGEMTerminalMessage += new OnGEMTerminalMessage(OnGEMTerminalMessage);
            _gemDriver.OnGEMTerminalMultiMessage += new OnGEMTerminalMultiMessage(OnGEMTerminalMultiMessage);
            #endregion

            #region Signal
            _gemDriver.OnRecvSignal += new OnRecvSignal(OnRecvSignal);
            #endregion
        }
        #endregion

        #endregion

        #region <Make User Defined Secs Message>
        private bool MakeAndSendSecsMessage(long stream, long function,
            ref long pObjectId, ref long pSystemByte,
            ref List<SemiObject> structureForAck)
        {
            long result = _gemDriver.MakeObject(ref pObjectId);
            if (result != 0)
            {
                WriteLog(String.Format("[EQ ==> XGEM] Fail to Sending Message(MakeObject) S{0}F{1} ({2})",
                    stream,
                    function,
                    result));
            }

            int count = structureForAck.Count;
            EN_ITEM_FORMAT errorFormat;
            for (int i = 0; i < count; ++i)
            {
                SemiObject obj = structureForAck[i];
                errorFormat = obj.Format;

                #region Format
                switch (obj.Format)
                {
                    case EN_ITEM_FORMAT.LIST:
                        {
                            SemiObjectList listItem = obj as SemiObjectList;
                            if (listItem != null)
                            {
                                result = _gemDriver.SetListItem(pObjectId, listItem.GetValues()[0]);
                            }
                        }
                        break;

                    case EN_ITEM_FORMAT.ASCII:
                        {
                            SemiObjectAscii aItem = obj as SemiObjectAscii;
                            if (aItem != null)
                            {
                                result = _gemDriver.SetStringItem(pObjectId, aItem.GetValues()[0]);
                            }
                        }
                        break;

                    case EN_ITEM_FORMAT.BINARY:
                        {
                            SemiObjectBinary bItem = obj as SemiObjectBinary;
                            if (bItem != null)
                            {
                                result = _gemDriver.SetBinaryItem(pObjectId, bItem.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.BOOL:
                        {
                            SemiObjectBool boItem = obj as SemiObjectBool;
                            if (boItem != null)
                            {
                                result = _gemDriver.SetBoolItem(pObjectId, boItem.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FLOAT4:
                        {
                            SemiObjectFloat4 f4Item = obj as SemiObjectFloat4;
                            if (f4Item != null)
                            {
                                result = _gemDriver.SetFloat4Item(pObjectId, f4Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FLOAT8:
                        {
                            SemiObjectFloat8 f8Item = obj as SemiObjectFloat8;
                            if (f8Item != null)
                            {
                                result = _gemDriver.SetFloat8Item(pObjectId, f8Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT:
                        {
                            SemiObjectInt i1Item = obj as SemiObjectInt;
                            if (i1Item != null)
                            {
                                result = _gemDriver.SetInt1Item(pObjectId, i1Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT2:
                        {
                            SemiObjectInt2 i2Item = obj as SemiObjectInt2;
                            if (i2Item != null)
                            {
                                result = _gemDriver.SetInt2Item(pObjectId, i2Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT4:
                        {
                            SemiObjectInt4 i4Item = obj as SemiObjectInt4;
                            if (i4Item != null)
                            {
                                result = _gemDriver.SetInt4Item(pObjectId, i4Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT8:
                        {
                            SemiObjectInt8 i8Item = obj as SemiObjectInt8;
                            if (i8Item != null)
                            {
                                result = _gemDriver.SetInt8Item(pObjectId, i8Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT:
                        {
                            SemiObjectUInt ui1Item = obj as SemiObjectUInt;
                            if (ui1Item != null)
                            {
                                result = _gemDriver.SetUint1Item(pObjectId, ui1Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT2:
                        {
                            SemiObjectUInt2 ui2Item = obj as SemiObjectUInt2;
                            if (ui2Item != null)
                            {
                                result = _gemDriver.SetUint2Item(pObjectId, ui2Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT4:
                        {
                            SemiObjectUInt4 ui4Item = obj as SemiObjectUInt4;
                            if (ui4Item != null)
                            {
                                result = _gemDriver.SetUint4Item(pObjectId, ui4Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT8:
                        {
                            SemiObjectUInt8 ui8Item = obj as SemiObjectUInt8;
                            if (ui8Item != null)
                            {
                                result = _gemDriver.SetUint8Item(pObjectId, ui8Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FILE:
                        break;
                }
                #endregion

                if (result != 0)
                {
                    WriteLog(String.Format("[EQ ==> XGEM] Fail to reply({0}) S{1}F{2} ({3})",
                        errorFormat.ToString(),
                        stream,
                        function,
                        result));

                    return false;
                }
            }

            result = _gemDriver.SendSECSMessage(pObjectId, stream, function, pSystemByte);
            if (result != 0)
            {
                WriteLog(String.Format("[EQ ==> XGEM] Fail to Sending Message S{0}F{1} ({2})",
                    stream,
                    function,
                    result));

                return false;
            }

            return true;
        }
        #endregion </Make User Define Message>

        #region <Get User Defined Secs Message>
        private void GetUserDefinedSecsMessageValue(long pObjectId, ref List<SemiObject> message)
        {
            int count = message.Count;
            for (int i = 0; i < count; ++i)
            {
                SemiObject obj = message[i];

                #region Format
                switch (obj.Format)
                {
                    case EN_ITEM_FORMAT.LIST:
                        {
                            long listCount = 0;
                            if (_gemDriver.GetListItem(pObjectId, ref listCount) == 0)
                            {
                                SemiObjectList convert = obj as SemiObjectList;
                                convert.SetValue(listCount);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.ASCII:
                        {
                            string ascii = string.Empty;
                            if (_gemDriver.GetStringItem(pObjectId, ref ascii) == 0)
                            {
                                SemiObjectAscii convert = obj as SemiObjectAscii;
                                convert.SetValue(ascii);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.BINARY:
                        {
                            byte[] byteArray = new byte[1];
                            if (_gemDriver.GetBinaryItem(pObjectId, ref byteArray) == 0)
                            {
                                SemiObjectBinary convert = obj as SemiObjectBinary;
                                convert.SetValues(byteArray);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.BOOL:
                        {
                            bool[] booleanArray = new bool[1];
                            if (_gemDriver.GetBoolItem(pObjectId, ref booleanArray) == 0)
                            {
                                SemiObjectBool convert = obj as SemiObjectBool;
                                convert.SetValues(booleanArray);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FLOAT4:
                        {
                            float[] floatArray4 = new float[1];
                            if (_gemDriver.GetFloat4Item(pObjectId, ref floatArray4) == 0)
                            {
                                SemiObjectFloat4 convert = obj as SemiObjectFloat4;
                                convert.SetValues(floatArray4);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FLOAT8:
                        {
                            double[] floatArray8 = new double[1];
                            if (_gemDriver.GetFloat8Item(pObjectId, ref floatArray8) == 0)
                            {
                                SemiObjectFloat8 convert = obj as SemiObjectFloat8;
                                convert.SetValues(floatArray8);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT:
                        {
                            sbyte[] intArray = new sbyte[1];
                            if (_gemDriver.GetInt1Item(pObjectId, ref intArray) == 0)
                            {
                                SemiObjectInt convert = obj as SemiObjectInt;
                                convert.SetValues(intArray);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT2:
                        {
                            short[] int2Array = new short[1];
                            if (_gemDriver.GetInt2Item(pObjectId, ref int2Array) == 0)
                            {
                                SemiObjectInt2 convert = obj as SemiObjectInt2;
                                convert.SetValues(int2Array);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT4:
                        {
                            int[] int4Array = new int[1];
                            if (_gemDriver.GetInt4Item(pObjectId, ref int4Array) == 0)
                            {
                                SemiObjectInt4 convert = obj as SemiObjectInt4;
                                convert.SetValues(int4Array);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT8:
                        {
                            long[] int8Array = new long[1];
                            if (_gemDriver.GetInt8Item(pObjectId, ref int8Array) == 0)
                            {
                                SemiObjectInt8 convert = obj as SemiObjectInt8;
                                convert.SetValues(int8Array);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT:
                        {
                            byte[] uintArray = new byte[1];
                            if (_gemDriver.GetUint1Item(pObjectId, ref uintArray) == 0)
                            {
                                SemiObjectUInt convert = obj as SemiObjectUInt;
                                convert.SetValues(uintArray);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT2:
                        {
                            ushort[] uint2Array = new ushort[1];
                            if (_gemDriver.GetUint2Item(pObjectId, ref uint2Array) == 0)
                            {
                                SemiObjectUInt2 convert = obj as SemiObjectUInt2;
                                convert.SetValues(uint2Array);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT4:
                        {
                            uint[] uint4Array = new uint[1];
                            if (_gemDriver.GetUint4Item(pObjectId, ref uint4Array) == 0)
                            {
                                SemiObjectUInt4 convert = obj as SemiObjectUInt4;
                                convert.SetValues(uint4Array);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT8:
                        {
                            ulong[] uint8Array = new ulong[1];
                            if (_gemDriver.GetUint8Item(pObjectId, ref uint8Array) == 0)
                            {
                                SemiObjectUInt8 convert = obj as SemiObjectUInt8;
                                convert.SetValues(uint8Array);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FILE:
                        break;
                }
                #endregion
            }

            _gemDriver.CloseObject(pObjectId);
        }
        #endregion </Get User Defined Secs Message>

        #endregion </Internal Interface>

        #region <Event Interface>

        #region Receive Message

        #region <User Defined Secs Message>
        private void OnUserDefinedSECSMessageReceived(long nObjectID, long nStream, long nFunction, long nSysbyte)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sValue = "";
            sValue = String.Format(
                        "[XGEM ==> EQ] OnSECSMessageReceived : ObjectID({0}), S{1},F{2}, Sysbyte({3})",
                            nObjectID, nStream, nFunction, nSysbyte);
            
            // S2F37에서 Event List가 0이면 모든 Event를 세팅하는데,
            // 이 때 Alarm Event도 같이 Enable 됨
            // Alarm Event를 사용하지 않는 경우 아래와 같이 UserDefinedSecsMessag로 구현해야한다.
            if (_useUserDefinedCollectionEventControl && nStream == 2 && nFunction == 37)
            {
                long enabled = 0;
                bool errorMessage = false;
                long listCount = 0;
                _gemDriver.GetListItem(nObjectID, ref listCount);
                if (listCount != 2)
                {
                    errorMessage = true;
                    return;
                }
                else
                {
                    bool[] ceed = new bool[1];
                    _gemDriver.GetBoolItem(nObjectID, ref ceed);
                    enabled = ceed[0] ? 1 : 0;
                    
                    _gemDriver.GetListItem(nObjectID, ref listCount);

                    List<long> ceids = new List<long>();
                    if (listCount == 0)
                    {
                        listCount = _collectionEvents.Events.Length;

                        for (int i = 0; i < listCount; ++i)
                        {
                            ceids.Add(_collectionEvents.Events[i]);
                        }
                    }
                    else
                    {
                        // 받아오는 이벤트만 해준다.
                        for (int i = 0; i < listCount; ++i)
                        {
                            uint[] value = new uint[1];
                            _gemDriver.GetUint4Item(nObjectID, ref value);

                            ceids.Add((long)value[0]);
                        }
                    }

                    if (enabled == 0)
                    {
                        ceids.Add(_alarmSetEvent);
                        ceids.Add(_alarmClearEvent);
                    }

                    _gemDriver.GEMSetEventEnable(listCount, ceids.ToArray(), enabled);
                }
                
                _gemDriver.CloseObject(nObjectID);

                #region <ACK : S2F38>
                long pObject = 0;
                _gemDriver.MakeObject(ref pObject);

                byte erack = 0;
                if (errorMessage)
                    erack = 1;

                _gemDriver.SetBinaryItem(pObject, erack);

                _gemDriver.SendSECSMessage(pObject, 2, 38, nSysbyte);

                _gemDriver.CloseObject(pObject);
                #endregion </ACK : S2F38>

                return;
            }
            else if (_useUserDefinedFormattedRecipeControl && nStream == 7 && (nFunction == 23 || nFunction == 24 || nFunction == 25 || nFunction == 26))
            {
                // Upload
                if (nFunction == 25)        // 서버에서 Upload 요청
                {
                    // ppid 파싱
                    string ppid = String.Empty;
                    _gemDriver.GetStringItem(nObjectID, ref ppid);
                    _gemDriver.CloseObject(nObjectID);

                    string sLog = String.Format("[XGEM ==> EQ] OnGEMReqPPFmt(UserDefined) : Ppid({0})", ppid);
                    WriteLog(sLog);
                    
                    // Ack
                    System.Threading.Tasks.Task.Run(() => UploadingFormattedRecipeUsingUserDefinedMessageAsync(ppid, nSysbyte));
                }

                // Download
            }

            UserDefinedSecsMessage structure = null, structureToSend = null;
            string functionName = String.Empty;

            long streamAck = nStream;
            long functionToSend = nFunction + 1;

            // Function 이 홀수로 오는 경우는 서버에서 먼저 보낸 것이라 응답을 해야하고,
            // 짝수로 오는 것은 설비에서 먼저 보낸 것이므로 응답할 필요가 없다.
            bool needAck = (nFunction % 2 == 1);

            string receivedMessageName = String.Empty;
            foreach (KeyValuePair<string, UserDefinedSecsMessage> kvp in _userDefinedSecsMessage)
            {
                if (kvp.Value.Stream == nStream)
                {
                    if (kvp.Value.Function == nFunction)
                    {
                        structure = new UserDefinedSecsMessage(kvp.Key, kvp.Value.Stream,
                                                kvp.Value.Function,
                                                kvp.Value.ListItemFormat);

                        receivedMessageName = kvp.Key;
                    }

                    if (kvp.Value.Function == functionToSend && needAck)
                    {
                        structureToSend = new UserDefinedSecsMessage(kvp.Key, kvp.Value.Stream,
                                                kvp.Value.Function,
                                                kvp.Value.ListItemFormat);
                    }
                }
            }

            if (structure != null && structureToSend != null || false == needAck)
            {
                WriteLog(sValue);

                ParseUserDefinedSecsMessage(ref structure, ref nObjectID);


                if (CallbackSecsMessageReceived(receivedMessageName, structure, ref structureToSend) && needAck)
                {
                    MakeAndSendSecsMessage(nStream, nFunction + 1,
                                ref nObjectID, ref nSysbyte, ref structureToSend);
                }
            }



            //long nReturn = 0;
            //int i;
            //string szTemp;
            //string szMsg;

            //sbyte[] pnI1 = new sbyte[1];
            //short[] pnI2 = new short[1];
            //byte[] pnU1 = new byte[1];
            //byte[] pnBinary = new byte[1];
            //bool[] pnBool = new bool[1];
            //int[] plI4 = new int[1];
            //int[] plI8 = new int[1];
            //ushort[] plU2 = new ushort[1];
            //float[] prF4 = new float[1];
            //uint[] prU4 = new uint[1];
            //uint[] prU8 = new uint[1];
            //double[] prF8 = new double[1];
            //string sAscii = null;
            //byte nBinary = 0;
            //long nItems = 0;
            //long lCnt = 0;



            //if ((nStream == 2) && (nFunction == 91))
            //{
            //    m_XGem.GetListItem(nObjectID, ref nItems);
            //    WriteLog("LIST {0}", nItems);

            //    // Read data
            //    m_XGem.GetStringItem(nObjectID, ref sAscii);
            //    WriteLog("     ASCII {0}", sAscii);
            //    m_XGem.GetStringItem(nObjectID, ref sAscii);
            //    WriteLog("     ASCII {0}", sAscii);
            //    m_XGem.CloseObject(nObjectID);

            //    //  Send a reply message S2F91
            //    m_XGem.MakeObject(ref nObjectID);
            //    m_XGem.SetListItem(nObjectID, 2);
            //    nBinary = 0; m_XGem.SetBinaryItem(nObjectID, nBinary);
            //    m_XGem.SetListItem(nObjectID, 2);
            //    sAscii = "XComV2";
            //    m_XGem.SetStringItem(nObjectID, sAscii);
            //    sAscii = "V2.0.0";
            //    m_XGem.SetStringItem(nObjectID, sAscii);

            //    nReturn = m_XGem.SendSECSMessage(nObjectID, nStream, nFunction + 1, nSysbyte);
            //    if (nReturn == 0)
            //    {
            //        WriteLog("[EQ ==> XGEM] Reply S2F92 successfully");
            //    }
            //    else
            //    {
            //        WriteLog(String.Format("[EQ ==> XGEM] Fail to reply S2F92 ({0})", nReturn));
            //    }
            //}
            //else if ((nStream == 2) && (nFunction == 93))
            //{
            //    ushort[] nReceived = new ushort[10000];
            //    // Read list count for L n
            //    WriteLog("S2F93 received");
            //    m_XGem.GetListItem(nObjectID, ref nItems);
            //    WriteLog(String.Format("LIST {0}", nItems));
            //    // Read data
            //    for (i = 0; i < nItems; i++)
            //    {
            //        m_XGem.GetUint2Item(nObjectID, ref plU2);
            //        nReceived[i] = plU2[i];
            //        WriteLog(String.Format("   UINT2 {0}", plU2));
            //    }
            //    m_XGem.CloseObject(nObjectID);

            //    //  Send a reply message S2F94
            //    m_XGem.MakeObject(ref nObjectID);
            //    m_XGem.SetListItem(nObjectID, 2);
            //    pnBinary[0] = 0; m_XGem.SetBinaryItem(nObjectID, pnBinary);
            //    m_XGem.SetListItem(nObjectID, nItems);
            //    for (i = 0; i < nItems; i++)
            //    {
            //        m_XGem.SetUint2Item(nObjectID, nReceived[i]);
            //    }

            //    nReturn = m_XGem.SendSECSMessage(nObjectID, nStream, nFunction + 1, nSysbyte);
            //    if (nReturn == 0)
            //    {
            //        WriteLog("[EQ ==> XGEM] Reply S2F94 successfully");
            //    }
            //    else
            //    {
            //        WriteLog(String.Format("[EQ ==> XGEM] Fail to reply S2F94 ({0})", nReturn));
            //    }
            //}
            //else if ((nStream == 101) && (nFunction == 101))
            //{
            //    //  Get data of the received message.
            //    m_XGem.GetListItem(nObjectID, ref nItems);
            //    WriteLog(String.Format("LIST {0}", nItems));
            //    m_XGem.GetListItem(nObjectID, ref nItems);
            //    WriteLog(String.Format("   LIST {0}", nItems);
            //    // Read I1 value
            //    m_XGem.GetInt1Item(nObjectID, ref pnI1);
            //    WriteLog(String.Format("       INT1 {0}", pnI1[0]);
            //    // Read I2 value
            //    m_XGem.GetInt2Item(nObjectID, ref pnI2);
            //    WriteLog(String.Format("       INT2 {0}", pnI2[0]);
            //    // Read I4 value
            //    m_XGem.GetInt4Item(nObjectID, ref plI4);
            //    WriteLog(String.Format("       INT4 {0}", plI4[0]);
            //    // Read I8 value
            //    m_XGem.GetInt4Item(nObjectID, ref plI8);
            //    WriteLog(String.Format("       INT4 {0}", plI8[0]);
            //    // Read U1 value
            //    m_XGem.GetUint1Item(nObjectID, ref pnU1);
            //    WriteLog("       UINT1 {0}", pnU1[0]);
            //    // Read U2 value
            //    m_XGem.GetUint2Item(nObjectID, ref plU2);
            //    WriteLog("       UINT2 {0}", plU2[0]);
            //    // Read U4 value
            //    m_XGem.GetUint4Item(nObjectID, ref prU4);
            //    WriteLog("       UINT4 {0}", prU4[0]);
            //    // Read U8 value
            //    m_XGem.GetUint4Item(nObjectID, ref prU8);
            //    WriteLog("       UINT4 {0}", prU8[0]);
            //    // Read F4 value
            //    m_XGem.GetFloat4Item(nObjectID, ref prF4);
            //    WriteLog("       FLOAT4 {0}", prF4[0]);
            //    // Read F8 value
            //    m_XGem.GetFloat8Item(nObjectID, ref prF8);
            //    WriteLog("       FLOAT8 {0}", prF8[0]);
            //    // Read List Count
            //    m_XGem.GetListItem(nObjectID, ref nItems);
            //    WriteLog("   LIST {0}", nItems);
            //    // Read String value
            //    m_XGem.GetStringItem(nObjectID, ref sAscii);
            //    WriteLog("       ASCII {0}", sAscii);
            //    // Read Binary value
            //    m_XGem.GetBinaryItem(nObjectID, ref pnBinary);
            //    WriteLog("       BINARY {0}", pnBinary[0]);
            //    // Read Bool value
            //    m_XGem.GetBoolItem(nObjectID, ref pnBool);
            //    WriteLog("       BOOL {0}", pnBool[0]);
            //    m_XGem.CloseObject(nObjectID);

            //    //  Send a reply message S101F102
            //    m_XGem.MakeObject(ref nObjectID);
            //    pnBinary[0] = 0; m_XGem.SetBinaryItem(nObjectID, pnBinary);

            //    nReturn = m_XGem.SendSECSMessage(nObjectID, nStream, nFunction + 1, nSysbyte);
            //    if (nReturn == 0)
            //    {
            //        WriteLog("[EQ ==> XGEM] Reply S101F102 successfully");
            //    }
            //    else
            //    {
            //        WriteLog("[EQ ==> XGEM] Fail to reply S101F102 ({0})", nReturn);
            //    }
            //}
            //else if ((nStream == 101) && (nFunction == 103))
            //{
            //    m_XGem.GetListItem(nObjectID, ref nItems);
            //    WriteLog("LIST {0}", nItems);
            //    szMsg = "   BOOL ";
            //    m_XGem.GetBoolItem(nObjectID, ref VALUE_BOOL_ARR);
            //    lCnt = VALUE_BOOL_ARR == null ? 0 : VALUE_BOOL_ARR.Length;
            //    for (i = 0; i < lCnt; i++)
            //    {
            //        szTemp = String.Format("{0} ", VALUE_BOOL_ARR[i]);
            //        szMsg += szTemp;
            //    }
            //    WriteLog(szMsg);
            //    szMsg = "   BINARY ";
            //    m_XGem.GetBinaryItem(nObjectID, ref VALUE_BINARY_ARR);
            //    lCnt = VALUE_BINARY_ARR == null ? 0 : VALUE_BINARY_ARR.Length;
            //    for (i = 0; i < lCnt; i++)
            //    {
            //        szTemp = String.Format("{0} ", VALUE_BINARY_ARR[i]);
            //        szMsg += szTemp;
            //    }
            //    WriteLog(szMsg);
            //    szMsg = "   UINT1 ";
            //    m_XGem.GetUint1Item(nObjectID, ref VALUE_U1_ARR);
            //    lCnt = VALUE_U1_ARR == null ? 0 : VALUE_U1_ARR.Length;
            //    for (i = 0; i < lCnt; i++)
            //    {
            //        szTemp = String.Format("{0} ", VALUE_U1_ARR[i]);
            //        szMsg += szTemp;
            //    }
            //    WriteLog(szMsg);
            //    szMsg = "   UINT2 ";
            //    m_XGem.GetUint2Item(nObjectID, ref VALUE_U2_ARR);
            //    lCnt = VALUE_U2_ARR == null ? 0 : VALUE_U2_ARR.Length;
            //    for (i = 0; i < lCnt; i++)
            //    {
            //        szTemp = String.Format("{0} ", VALUE_U2_ARR[i]);
            //        szMsg += szTemp;
            //    }
            //    WriteLog(szMsg);
            //    szMsg = "   UINT4 ";
            //    m_XGem.GetUint4Item(nObjectID, ref VALUE_U4_ARR);
            //    lCnt = VALUE_U4_ARR == null ? 0 : VALUE_U4_ARR.Length;
            //    for (i = 0; i < lCnt; i++)
            //    {
            //        szTemp = String.Format("{0} ", VALUE_U4_ARR[i]);
            //        szMsg += szTemp;
            //    }
            //    WriteLog(szMsg);
            //    szMsg = "   UINT4 ";
            //    m_XGem.GetUint4Item(nObjectID, ref VALUE_U8_ARR);
            //    lCnt = VALUE_U8_ARR == null ? 0 : VALUE_U8_ARR.Length;
            //    for (i = 0; i < lCnt; i++)
            //    {
            //        szTemp = String.Format("{0} ", VALUE_U8_ARR[i]);
            //        szMsg += szTemp;
            //    }
            //    WriteLog(szMsg);
            //    szMsg = "   INT1 ";
            //    m_XGem.GetInt1Item(nObjectID, ref VALUE_I1_ARR);
            //    lCnt = VALUE_I1_ARR == null ? 0 : VALUE_I1_ARR.Length;
            //    for (i = 0; i < lCnt; i++)
            //    {
            //        szTemp = String.Format("{0} ", VALUE_I1_ARR[i]);
            //        szMsg += szTemp;
            //    }
            //    WriteLog(szMsg);
            //    szMsg = "   INT2 ";
            //    m_XGem.GetInt2Item(nObjectID, ref VALUE_I2_ARR);
            //    lCnt = VALUE_I2_ARR == null ? 0 : VALUE_I2_ARR.Length;
            //    for (i = 0; i < lCnt; i++)
            //    {
            //        szTemp = String.Format("{0} ", VALUE_I2_ARR[i]);
            //        szMsg += szTemp;
            //    }
            //    WriteLog(szMsg);
            //    szMsg = "   INT4 ";
            //    m_XGem.GetInt4Item(nObjectID, ref VALUE_I4_ARR);
            //    lCnt = VALUE_I4_ARR == null ? 0 : VALUE_I4_ARR.Length;
            //    for (i = 0; i < lCnt; i++)
            //    {
            //        szTemp = String.Format("{0} ", VALUE_I4_ARR[i]);
            //        szMsg += szTemp;
            //    }
            //    WriteLog(szMsg);
            //    szMsg = "   INT4 ";
            //    m_XGem.GetInt4Item(nObjectID, ref VALUE_I8_ARR);
            //    lCnt = VALUE_I8_ARR == null ? 0 : VALUE_I8_ARR.Length;
            //    for (i = 0; i < lCnt; i++)
            //    {
            //        szTemp = String.Format("{0} ", VALUE_I8_ARR[i]);
            //        szMsg += szTemp;
            //    }
            //    WriteLog(szMsg);
            //    szMsg = "   FLOAT4 ";
            //    m_XGem.GetFloat4Item(nObjectID, ref VALUE_F4_ARR);
            //    lCnt = VALUE_F4_ARR == null ? 0 : VALUE_F4_ARR.Length;
            //    for (i = 0; i < lCnt; i++)
            //    {
            //        szTemp = String.Format("{0:f6} ", VALUE_F4_ARR[i]);
            //        szMsg += szTemp;
            //    }
            //    WriteLog(szMsg);
            //    szMsg = "   FLOAT8 ";
            //    m_XGem.GetFloat8Item(nObjectID, ref VALUE_F8_ARR);
            //    lCnt = VALUE_F8_ARR == null ? 0 : VALUE_F8_ARR.Length;
            //    for (i = 0; i < lCnt; i++)
            //    {
            //        szTemp = String.Format("{0:f6} ", VALUE_F8_ARR[i]);
            //        szMsg += szTemp;
            //    }
            //    WriteLog(szMsg);
            //    m_XGem.CloseObject(nObjectID);

            //    //  Send a reply message S5F24
            //    m_XGem.MakeObject(ref nObjectID);
            //    pnU1 = new byte[] { 1 };
            //    m_XGem.SetUint1Item(nObjectID, pnU1);

            //    nReturn = m_XGem.SendSECSMessage(nObjectID, nStream, nFunction + 1, nSysbyte);
            //    if (nReturn == 0)
            //    {
            //        WriteLog("[EQ ==> XGEM] Reply S101F104 successfully");
            //    }
            //    else
            //    {
            //        WriteLog("[EQ ==> XGEM] Fail to reply S101F104 ({0})", nReturn);
            //    }
            //}
            //else if ((nStream == 2) && (nFunction == 92))
            //{
            //    m_XGem.CloseObject(nObjectID);
            //}
            //else if ((nStream == 2) && (nFunction == 94))
            //{
            //    m_XGem.CloseObject(nObjectID);
            //}
            //else if ((nStream == 101) && (nFunction == 102))
            //{
            //    m_XGem.CloseObject(nObjectID);
            //}
            //else if ((nStream == 101) && (nFunction == 104))
            //{
            //    // Get value
            //    // Read U1 value
            //    m_XGem.GetUint1Item(nObjectID, ref pnU1);
            //    WriteLog("UINT1 {0}", pnU1[0]);
            //    m_XGem.CloseObject(nObjectID);
            //}
            //else
            //{
            //    WriteLog("Undefined message received (S{0}F{1})", nStream, nFunction);
            //    m_XGem.CloseObject(nObjectID);
            //}
        }

        private void ParseUserDefinedSecsMessage(ref UserDefinedSecsMessage message, ref long pObjectId)
        {
            int count = message.ListItemFormat.Count;
            for (int i = 0; i < count; ++i)
            {
                SemiObject obj = message.ListItemFormat[i];
                switch (obj.Format)
                {
                    case EN_ITEM_FORMAT.LIST:
                        {
                            long listCount = 0;
                            if (_gemDriver.GetListItem(pObjectId, ref listCount) == 0)
                            {
                                message.ListItemFormat[i] = new SemiObjectList(listCount);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.ASCII:
                        {
                            string ascii = string.Empty;
                            if (_gemDriver.GetStringItem(pObjectId, ref ascii) == 0)
                            {
                                message.ListItemFormat[i] = new SemiObjectAscii(obj.Name, ascii);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.BINARY:
                        {
                            byte[] byteArray = new byte[1];
                            if (_gemDriver.GetBinaryItem(pObjectId, ref byteArray) == 0)
                            {
                                message.ListItemFormat[i] = new SemiObjectBinary(obj.Name, byteArray[0]);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.BOOL:
                        {
                            bool[] booleanArray = new bool[1];
                            if (_gemDriver.GetBoolItem(pObjectId, ref booleanArray) == 0)
                            {
                                message.ListItemFormat[i] = new SemiObjectBool(obj.Name, booleanArray[0]);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FLOAT4:
                        {
                            float[] floatArray4 = new float[1];
                            if (_gemDriver.GetFloat4Item(pObjectId, ref floatArray4) == 0)
                            {
                                message.ListItemFormat[i] = new SemiObjectFloat4(obj.Name, floatArray4[0]);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FLOAT8:
                        {
                            double[] floatArray8 = new double[1];
                            if (_gemDriver.GetFloat8Item(pObjectId, ref floatArray8) == 0)
                            {
                                message.ListItemFormat[i] = new SemiObjectFloat8(obj.Name, floatArray8[0]);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT:
                        {
                            sbyte[] intArray = new sbyte[1];
                            if (_gemDriver.GetInt1Item(pObjectId, ref intArray) == 0)
                            {
                                message.ListItemFormat[i] = new SemiObjectInt(obj.Name, intArray[0]);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT2:
                        {
                            short[] int2Array = new short[1];
                            if (_gemDriver.GetInt2Item(pObjectId, ref int2Array) == 0)
                            {
                                message.ListItemFormat[i] = new SemiObjectInt2(obj.Name, int2Array[0]);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT4:
                        {
                            int[] int4Array = new int[1];
                            if (_gemDriver.GetInt4Item(pObjectId, ref int4Array) == 0)
                            {
                                message.ListItemFormat[i] = new SemiObjectInt4(obj.Name, int4Array[0]);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT8:
                        {
                            long[] int8Array = new long[1];
                            if (_gemDriver.GetInt8Item(pObjectId, ref int8Array) == 0)
                            {
                                message.ListItemFormat[i] = new SemiObjectInt8(obj.Name, int8Array[0]);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT:
                        {
                            byte[] uintArray = new byte[1];
                            if (_gemDriver.GetUint1Item(pObjectId, ref uintArray) == 0)
                            {
                                message.ListItemFormat[i] = new SemiObjectUInt(obj.Name, uintArray[0]);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT2:
                        {
                            ushort[] uint2Array = new ushort[1];
                            if (_gemDriver.GetUint2Item(pObjectId, ref uint2Array) == 0)
                            {
                                message.ListItemFormat[i] = new SemiObjectUInt2(obj.Name, uint2Array[0]);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT4:
                        {
                            uint[] uint4Array = new uint[1];
                            if (_gemDriver.GetUint4Item(pObjectId, ref uint4Array) == 0)
                            {
                                message.ListItemFormat[i] = new SemiObjectUInt4(obj.Name, uint4Array[0]);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT8:
                        {
                            ulong[] uint8Array = new ulong[1];
                            if (_gemDriver.GetUint8Item(pObjectId, ref uint8Array) == 0)
                            {
                                message.ListItemFormat[i] = new SemiObjectUInt8(obj.Name, uint8Array[0]);
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FILE:
                        break;
                }
            }

            _gemDriver.CloseObject(pObjectId);
        }

        private bool MakeAndSendSecsMessage(long stream, long function,
            ref long pObjectId, ref long pSystemByte,
            ref UserDefinedSecsMessage structureForAck)
        {
            long result = _gemDriver.MakeObject(ref pObjectId);
            if (result != 0)
            {
                WriteLog(String.Format("[EQ ==> XGEM] Fail to Sending Message(MakeObject) S{0}F{1} ({2})",
                    stream,
                    function,
                    result));
            }

            int count = structureForAck.ListItemFormat.Count;
            EN_ITEM_FORMAT errorFormat;
            for (int i = 0; i < count; ++i)
            {
                SemiObject obj = structureForAck.ListItemFormat[i];
                errorFormat = obj.Format;
                switch (obj.Format)
                {
                    case EN_ITEM_FORMAT.LIST:
                        {
                            SemiObjectList listItem = obj as SemiObjectList;
                            if (listItem != null)
                            {
                                result = _gemDriver.SetListItem(pObjectId, listItem.GetValue());
                            }
                        }
                        break;

                    case EN_ITEM_FORMAT.ASCII:
                        {
                            SemiObjectAscii aItem = obj as SemiObjectAscii;
                            if (aItem != null)
                            {
                                result = _gemDriver.SetStringItem(pObjectId, aItem.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.BINARY:
                        {
                            SemiObjectBinary bItem = obj as SemiObjectBinary;
                            if (bItem != null)
                            {
                                result = _gemDriver.SetBinaryItem(pObjectId, bItem.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.BOOL:
                        {
                            SemiObjectBool boItem = obj as SemiObjectBool;
                            if (boItem != null)
                            {
                                result = _gemDriver.SetBoolItem(pObjectId, boItem.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FLOAT4:
                        {
                            SemiObjectFloat4 f4Item = obj as SemiObjectFloat4;
                            if (f4Item != null)
                            {
                                result = _gemDriver.SetFloat4Item(pObjectId, f4Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FLOAT8:
                        {
                            SemiObjectFloat8 f8Item = obj as SemiObjectFloat8;
                            if (f8Item != null)
                            {
                                result = _gemDriver.SetFloat8Item(pObjectId, f8Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT:
                        {
                            SemiObjectInt i1Item = obj as SemiObjectInt;
                            if (i1Item != null)
                            {
                                result = _gemDriver.SetInt1Item(pObjectId, i1Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT2:
                        {
                            SemiObjectInt2 i2Item = obj as SemiObjectInt2;
                            if (i2Item != null)
                            {
                                result = _gemDriver.SetInt2Item(pObjectId, i2Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT4:
                        {
                            SemiObjectInt4 i4Item = obj as SemiObjectInt4;
                            if (i4Item != null)
                            {
                                result = _gemDriver.SetInt4Item(pObjectId, i4Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT8:
                        {
                            SemiObjectInt8 i8Item = obj as SemiObjectInt8;
                            if (i8Item != null)
                            {
                                result = _gemDriver.SetInt8Item(pObjectId, i8Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT:
                        {
                            SemiObjectUInt ui1Item = obj as SemiObjectUInt;
                            if (ui1Item != null)
                            {
                                result = _gemDriver.SetUint1Item(pObjectId, ui1Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT2:
                        {
                            SemiObjectUInt ui2Item = obj as SemiObjectUInt;
                            if (ui2Item != null)
                            {
                                result = _gemDriver.SetUint2Item(pObjectId, ui2Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT4:
                        {
                            SemiObjectUInt ui4Item = obj as SemiObjectUInt;
                            if (ui4Item != null)
                            {
                                result = _gemDriver.SetUint4Item(pObjectId, ui4Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT8:
                        {
                            SemiObjectUInt ui8Item = obj as SemiObjectUInt;
                            if (ui8Item != null)
                            {
                                result = _gemDriver.SetUint8Item(pObjectId, ui8Item.GetValue());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FILE:
                        break;
                }

                if (result != 0)
                {
                    WriteLog(String.Format("[EQ ==> XGEM] Fail to reply({0}) S{1}F{2} ({3})",
                        errorFormat.ToString(),
                        stream,
                        function,
                        result));

                    return false;
                }
            }

            result = _gemDriver.SendSECSMessage(pObjectId, stream, function, pSystemByte);
            if (result != 0)
            {
                WriteLog(String.Format("[EQ ==> XGEM] Fail to Sending Message S{0}F{1} ({2})",
                    stream,
                    function,
                    result));

                return false;
            }

            return true;
        }
        #endregion </User Defined Secs Message>

        #region <Remote Command>
        private void RemoteCommandReceivedAsync(long nMsgId, string sRcmd, long nCount, string[] psNames, string[] psVals)
        {
            long[] arrResult = new long[nCount];
            string strRcmd = sRcmd;
            string[] arrNames = new string[nCount];
            string[] arrVals = new string[nCount];
            if (psNames == null) psNames = new string[] { };
            if (psVals == null) psVals = new string[] { };
            Array.Copy(psNames, arrNames, nCount);
            Array.Copy(psVals, arrVals, nCount);

            for (int i = 0; i < nCount; ++i)
            {
                arrResult[i] = 0;
            }

            if (base.ReceiveRemoteCommand(strRcmd, arrNames, arrVals, ref arrResult))
            {
                // ProcessingScenario 에서 arrVals의 값을 변경하여 보낼 것이므로 이 값을 마스터를 제외한 모든 클라이언트에 보낸다.
                // BroadcastSignal(EN_MESSAGE_TYPE.RCMD, strRcmd, arrVals);
                _gemDriver.GEMRspRemoteCommand(nMsgId, sRcmd, 0, nCount, arrNames, arrResult);
                //ShowTerminalMessage(String.Format("RCMD Success! : {0}", strRcmd));
            }
            else
            {
                _gemDriver.GEMRspRemoteCommand(nMsgId, sRcmd, 1, arrNames.Length, arrNames, arrResult);
                //ShowTerminalMessage(String.Format("RCMD Failed! : {0}", strRcmd));
            }

            //BroadcastSignal(sRcmd, )
            if (arrResult.Length > 0)
            {
                string sLog = String.Format("[EQ ==> XGEM] GEMRspRemoteCommand, {0} ", arrResult[0]);
                WriteLog(sLog);
            }
        }

        // S2F41 Host Command Send (HCS)  H -> E
        private void OnGEMReqRemoteCommand(long nMsgId, string sRcmd, long nCount, string[] psNames, string[] psVals)   // S2F41
        {
            string sLog = string.Format("[XGEM ==> EQ] OnGEMReqRemoteCommand < S2F41 > : Remote Command({0}), ", sRcmd);

            WriteLog(sLog);

            if (psNames == null)
                psNames = new string[] { };

            if (psVals == null)
                psVals = new string[] { };

            System.Threading.Tasks.Task.Run(() => RemoteCommandReceivedAsync(nMsgId, sRcmd, nCount, psNames, psVals));
        }

        #endregion </Remote Command>

        #region SecondaryMessages
        private void OnGEMSecondaryMsgReceived(long nS, long nF, long nSysbyte, string sParam1, string sParam2, string sParam3)
        {
            foreach (var pair in _eventToSend)
            {
                if (_eventToSend[pair.Key] == false)
                {
                    _eventToSend[pair.Key] = true;
                    string sLog = String.Format("[XGEM ==> EQ] S6F12 : {0}, Ack : {1}", pair.Key, sParam1);
                    WriteLog(sLog);
                    break;
                }

            }
        }
        #endregion

        #endregion

        #region <Time>
        private void OnGEMReqGetDateTime(long nMsgId)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqGetDateTime");
            WriteLog(sLog);

            string sSystemTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            _gemDriver.GEMRspGetDateTime(nMsgId, sSystemTime);
            sLog = String.Format("[EQ ==> XGEM] GEMRspGetDateTime:{0}", sSystemTime);
            WriteLog(sLog);
        }

        private void OnGEMRspGetDateTime(string sSystemTime)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspGetDateTime : systemtime({0})", sSystemTime);
            WriteLog(sLog);

            //Debug.WriteLine(String.Format("OnGEMRspGetDateTimeExgemctrl1 th={0}\n", Thread.CurrentThread.ManagedThreadId));
        }

        private void OnGEMReqDateTime(long nMsgId, string sSystemTime)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqDateTime : systemtime({0})", sSystemTime);
            WriteLog(sLog);

            SYSTEMTIME sTime = new SYSTEMTIME();
            string sDate = sSystemTime.Remove(4);
            UInt16 nDate;
            if (!UInt16.TryParse(sDate, out nDate))
                return;
            sTime.wYear = nDate;

            sDate = sSystemTime.Substring(4, 2);
            if (!UInt16.TryParse(sDate, out nDate))
                return;
            sTime.wMonth = nDate;

            sDate = sSystemTime.Substring(6, 2);
            if (!UInt16.TryParse(sDate, out nDate))
                return;
            sTime.wDay = nDate;

            sDate = sSystemTime.Substring(8, 2);
            if (!UInt16.TryParse(sDate, out nDate))
                return;
            sTime.wHour = nDate;

            sDate = sSystemTime.Substring(10, 2);
            if (!UInt16.TryParse(sDate, out nDate))
                return;
            sTime.wMinute = nDate;

            sDate = sSystemTime.Substring(12, 2);
            if (!UInt16.TryParse(sDate, out nDate))
                return;
            sTime.wSecond = nDate;

            SetSystemTime(ref sTime);
            SetLocalTime(ref sTime);

            _gemDriver.GEMRspDateTime(nMsgId, 0);
            sLog = String.Format("[EQ ==> XGEM] GEMRspDateTime");
            WriteLog(sLog);
        }
        #endregion </Time>

        #region <Unformatted Recipe>

        #region PPLIST
        private bool GetRecipeFileList(string path, out Dictionary<int, string> result)
        {
            result = new Dictionary<int, string>();

            System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(path);
            try
            {
                int index = 0;
                foreach (var fInfo in dInfo.GetFiles())
                {
                    if (fInfo.Extension.ToLower().Equals(Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE))
                    {
                        result.Add(index, System.IO.Path.GetFileNameWithoutExtension(fInfo.Name));
                        index++;
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

        private bool GetRecipeFileListForMaster(out List<string> recipeFiles)
        {
            recipeFiles = new List<string>();
            if (_recipePathInfos == null)
                return false;

            List<Dictionary<int, string>> filesInClients = new List<Dictionary<int, string>>();
            for (int i = 0; i < _countOfClients; ++i)
            {
                Dictionary<int, string> client;
                if (false == GetRecipeFileList(_recipePathInfos[i], out client))
                    return false;

                filesInClients.Add(client);
            }

            foreach (var kvp in filesInClients[0])
            {
                string name = kvp.Value;
                if (filesInClients[1].ContainsValue(name) &&
                    filesInClients[2].ContainsValue(name))
                {
                    recipeFiles.Add(name);
                }
            }

            return true;
        }

        // S7, F19
        private void OnGEMReqPPList(long nMsgId)    // S7F19, S7F20    // RECIPE LIST REQ
        {
            long result = 0;
            //string[] saPpids = null;

            string sLog = string.Format("[XGEM ==> EQ] OnGEMReqPPList < S7F19 >");
            //Views.Setup.Setup_Gem.GetInstance.AddMessage(sLog);
            WriteLog(sLog);
            //Console.WriteLine(sLog);     
            //m_InstanceOfLog.WriteCOMMLog(sLog);

            //saPpids = GetSearchFile(_recipePath, "*.rcp");
            //nCount = saPpids.Length;
            List<string> recipeFiles;
            if (GetRecipeFileListForMaster(out recipeFiles))
            {
                int count = recipeFiles.Count;

                //RecipeSearch("D:\\ProtecDB\\");
                result = _gemDriver.GEMRspPPList(nMsgId, count, recipeFiles.ToArray());
            }

            if (result == 0)
            {
                sLog = string.Format("[Send] GEMRspPPList successfully");
                //Console.WriteLine(sLog);
                WriteLog(sLog);
                //m_InstanceOfLog.WriteCOMMLog(sLog);
            }
            else
            {
                //szMsg.Format(_T("Fail to GEMRspPPList (%d)"), nReturn );
                sLog = string.Format("[Send] Fail to GEMRspPPList, {0}", result);
                //Console.WriteLine(sLog); 
                WriteLog(sLog);
                //m_InstanceOfLog.WriteCOMMLog(sLog);
            }
        }
        #endregion

        #region <PP Load Inquire>
        // S7, F1 : [Host -> EQ] 레시피 다운로드 이전에 초기화 작업(이거 발생 후 S7, F3 발생할 것이다.)
        private void OnGEMReqPPLoadInquire(long nMsgId, string sPpid, long nLength)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqPPLoadInquire : Ppid({0}), ", sPpid);
            WriteLog(sLog);

            EN_PPGRANT grant = CallbackCheckingRecipeControlGrant(sPpid);

            _gemDriver.GEMRspPPLoadInquire(nMsgId, sPpid, (long)grant);
            sLog = String.Format("[EQ ==> XGEM] GEMRspPPLoadInquire : Ppid({0}), ", sPpid);
            WriteLog(sLog);
        }
        #endregion </PP Load Inquire>

        #region <Upload>

        #region 미사용 : XEIC는 사용할 수 없다고 한다.
        // S7, F1 : [EQ -> Host] 설비에서 레시피 업로드를 요청 전 초기화 작업(이거 발생 후 응답 뒤에 S7, F3 보낼 것이다.)
        //public void GEMReqPPLoadInquire(string file)
        //{
        //    FileInfo info = new FileInfo(file);
        //    string fileName = Path.GetFileNameWithoutExtension(file);

        //    m_XGem.GEMReqPPLoadInquire(fileName, info.Length);
        //}

        // S7, F2 : [Host -> EQ]
        // S7, F1을 XEIC에서는 사용할 수 없기 때문에 이 메시지도 받을 일이 없다.
        //private void OnGEMRspPPLoadInquire(string sPpid, long nResult)
        //{
        //    //throw new Exception("The method or operation is not implemented.");
        //    string sLog = String.Format("[XGEM ==> EQ] OnGEMRspPPLoadInquire : Ppid({0}), Result({1})", sPpid, nResult);
        //    WriteLog(sLog);
        //}
        #endregion

        // S7, F4 : [Host -> EQ] 레시피 업로드에 대한 응답
        private void OnGEMRspPPSend(string sPpid, long nResult)
        {
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspPPSend : Ppid({0}), Result({1})", sPpid, nResult);

            WriteLog(sLog);
        }

        // S7, F4 : [Host -> EQ] 레시피 업로드에 대한 응답
        private void OnGEMRspPPSendEx(string sPpid, string path, long nResult)
        {
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspPPSendEx : Ppid({0}), Result({1})", sPpid, nResult);

            WriteLog(sLog);
        }

        private bool UploadingUnformattedRecipeAsync(long nMsgId, string recipeId, string path)
        {
            long resultApi = 0;
            string recipeFullPath = String.Empty;
            if (CallbackUploadingUnformattedRecipe(recipeId, ref recipeFullPath))
            {
                bool result = true;
                if (false == File.Exists(recipeFullPath))
                {
                    result = false;
                }

                if (false == String.IsNullOrEmpty(path) && String.IsNullOrEmpty(Path.GetDirectoryName(path)))
                {
                    path += @"\";
                }

                string destPath = String.Format(@"{0}{1}{2}", _recipeHandlingPath, path, recipeId);
                try
                {
                    File.Move(recipeFullPath, destPath);
                }
                catch (Exception ex)
                {
                    WriteLog(String.Format("[XGEM ==> EQ] UploadingUnformattedRecipeAsync File copy has been failed = {0}, {1}", ex.Message, ex.StackTrace));
                }
                //byte[] recipeBodies = File.ReadAllBytes(recipeFullPath);
                //if (recipeBodies == null)
                //    result = false;

                if (result)
                {
                    if (nMsgId > 0)
                    {
                        resultApi = _gemDriver.GEMRspPPEx(nMsgId, recipeId, path);

                        WriteLog(String.Format("[XGEM ==> EQ] GEMRspPPEx successfully = {0}", resultApi));
                    }
                    else
                    {
                        resultApi = _gemDriver.GEMReqPPSendEx(recipeId, path);

                        WriteLog(String.Format("[XGEM ==> EQ] GEMReqPPSendEx successfully = {0}", resultApi));
                    }
                }
                else
                {
                    if (nMsgId > 0)
                    {
                        resultApi = _gemDriver.GEMRspPPEx(nMsgId, recipeId, path);

                        WriteLog(String.Format("[XGEM ==> EQ] Fail to GEMRspPPEx, {0}", resultApi));
                    }
                }
            }
            else
            {
                if (nMsgId > 0)
                {
                    resultApi = _gemDriver.GEMRspPPEx(nMsgId, recipeId, path);

                    WriteLog(String.Format("[XGEM ==> EQ] Fail to GEMRspPPEx, {0}", resultApi));
                }
            }


            return (resultApi == 0);
        }

        private bool UploadingUnformattedRecipeAsync(long nMsgId, string recipeId)
        {
            long resultApi = 0;
            string recipeFullPath = String.Empty;
            if (CallbackUploadingUnformattedRecipe(recipeId, ref recipeFullPath))
            {
                bool result = true;
                if (false == File.Exists(recipeFullPath))
                {
                    result = false;
                }

                byte[] recipeBodies = File.ReadAllBytes(recipeFullPath);
                if (recipeBodies == null)
                    result = false;

                if (result)
                {
                    if (nMsgId > 0)
                    {
                        resultApi = _gemDriver.GEMRspPP(nMsgId, recipeId, recipeBodies);

                        WriteLog(String.Format("[XGEM ==> EQ] OnGEMReqPP successfully = {0}", resultApi));
                    }
                    else
                    {
                        resultApi = _gemDriver.GEMReqPPSend(recipeId, recipeBodies);

                        WriteLog(String.Format("[XGEM ==> EQ] GEMReqPPSend successfully = {0}", resultApi));
                    }
                }
                else
                {
                    if (nMsgId > 0)
                    {
                        resultApi = _gemDriver.GEMRspPP(nMsgId, recipeId, null);

                        WriteLog(String.Format("[XGEM ==> EQ] Fail to OnGEMReqPP, {0}", resultApi));
                    }
                }
            }
            else
            {
                if (nMsgId > 0)
                {
                    resultApi = _gemDriver.GEMRspPP(nMsgId, recipeId, null);

                    WriteLog(String.Format("[XGEM ==> EQ] Fail to OnGEMReqPP, {0}", resultApi));
                }
            }


            return (resultApi == 0);
        }

        // S7, F5 : [Host -> EQ] 호스트에서 레시피 업로드를 요청했다.
        private void OnGEMReqPP(long nMsgId, string sPpid)
        {
            System.Threading.Tasks.Task.Run(()
                => UploadingUnformattedRecipeAsync(nMsgId, sPpid));
            //long nReturn = -1;
            //try
            //{
            //    CallbackUploadingUnFormattedRecipe(sPpid);
            //    if (PrepareUploadingRecipe(sPpid))
            //    {
            //        nReturn = _gemDriver.GEMRspPPEx(nMsgId, sPpid, sRecipePath);

            //        WriteLog(String.Format("[Send] GEMRspPPEx successfully = {0}", nReturn));
            //    }
            //    else
            //    {
            //        nReturn = _gemDriver.GEMRspPPEx(nMsgId, null, null);

            //        WriteLog(String.Format("[Send] Fail to GEMRspPPEx, {0}", nReturn));
            //    }

            //}
            //catch (Exception ex)
            //{
            //    WriteLog(String.Format("Upload Recipe Fail! {0}, {1}", ex.Message, ex.StackTrace));
            //}
        }

        // S7, F5 : [Host -> EQ] 호스트에서 레시피 업로드를 요청했다.
        private void OnGEMReqPPEx(long nMsgId, string sPpid, string path)
        {
            System.Threading.Tasks.Task.Run(()
                => UploadingUnformattedRecipeAsync(nMsgId, sPpid, path));
        }
        #endregion </Upload>

        #region <Download>
        private bool DownloadingUnformattedRecipeAsync(long nMsgId, string recipeId, string path)
        {
            long result = 0;

            if (false == String.IsNullOrEmpty(path) && String.IsNullOrEmpty(Path.GetDirectoryName(path)))
            {
                path += @"\";
            }

            string fileFullPath = String.Format(@"{0}{1}{2}", _recipeHandlingPath, path, recipeId);

            EN_ACK7 resultAck = CallbackDownloadingUnformattedRecipe(recipeId, fileFullPath);

            if (nMsgId > 0)
            {
                result = _gemDriver.GEMRspPPSendEx(nMsgId, recipeId, path, (long)resultAck);

                WriteLog(String.Format("[XGEM ==> EQ] OnGEMReqPP result = {0}", resultAck.ToString()));
            }

            return false;
        }

        private bool DownloadingUnformattedRecipeAsync(long nMsgId, string recipeId, byte[] recipeBodies)
        {
            long result = 0;
            string fileFullPath = String.Format(@"{0}\{1}", _recipeHandlingPath, recipeId);

            try
            {
                File.WriteAllBytes(fileFullPath, recipeBodies);
            }
            catch
            {
                return false;
            }

            EN_ACK7 resultAck = CallbackDownloadingUnformattedRecipe(recipeId, fileFullPath);

            if (nMsgId > 0)
            {
                result = _gemDriver.GEMRspPPSend(nMsgId, recipeId, (long)resultAck);

                WriteLog(String.Format("[XGEM ==> EQ] OnGEMReqPP result = {0}", resultAck.ToString()));
            }

            return false;
        }

        // S7, F6 : [Host -> EQ] 레시피 다운로드 요청에 대한 응답
        private void OnGEMRspPP(string sPpid, byte[] psBody)
        {
            System.Threading.Tasks.Task.Run(()
                => DownloadingUnformattedRecipeAsync(-1, sPpid, psBody));
        }

        // S7, F3 : [Host -> EQ] 레시피 다운로드를 요청받았다.
        private void OnGEMReqPPSend(long nMsgId, string sPpid, byte[] psBody)
        {
            System.Threading.Tasks.Task.Run(()
               => DownloadingUnformattedRecipeAsync(nMsgId, sPpid, psBody));

            //// 1. 레시피를 레시피 폴더로 복사하고, 내 레시피를 복사한다.
            //if (false == PrepareDownloadingRecipe(sPpid))
            //{
            //    // 실패 시 NACK 응답
            //    _gemDriver.GEMRspPPSendEx(nMsgId, sPpid, sRecipePath, (long)EN_ACK7.PERMISSION);
            //    return;
            //}

            //long nResult = _gemDriver.GEMRspPPSendEx(nMsgId, sPpid, sRecipePath, (long)EN_ACK7.OK);
        }

        // S7, F6 : [Host -> EQ] 레시피 다운로드 요청에 대한 응답
        private void OnGEMRspPPEx(string sPpid, string recipePath)
        {
            System.Threading.Tasks.Task.Run(()
                => DownloadingUnformattedRecipeAsync(-1, sPpid, recipePath));
        }

        // S7, F3 : [Host -> EQ] 레시피 다운로드를 요청받았다.
        private void OnGEMReqPPSendEx(long nMsgId, string sPpid, string recipePath)
        {
            System.Threading.Tasks.Task.Run(()
               => DownloadingUnformattedRecipeAsync(nMsgId, sPpid, recipePath));

            //// 1. 레시피를 레시피 폴더로 복사하고, 내 레시피를 복사한다.
            //if (false == PrepareDownloadingRecipe(sPpid))
            //{
            //    // 실패 시 NACK 응답
            //    _gemDriver.GEMRspPPSendEx(nMsgId, sPpid, sRecipePath, (long)EN_ACK7.PERMISSION);
            //    return;
            //}

            //long nResult = _gemDriver.GEMRspPPSendEx(nMsgId, sPpid, sRecipePath, (long)EN_ACK7.OK);
        }
        #endregion </Download>

        #endregion </Unformatted Recipe>

        #region <Formatted Recipe>
        private void DownloadFormattedRecipeAsync(long msgId, string sPpid, string sMdln, string sSoftRev, long nCount, string[] psCCode, long[] pnParamCount, string[] psParamNames)
        {
            //
            Dictionary<string, string[]> ppBodies = new Dictionary<string, string[]>();


            // ~nCount = CCode Array Length -> Key Count
            // ~psCCode = CCode Array -> Keys
            // ~pnParamCount = ParamCount Array -> Value Count
            // psParamNames = ParamName Array -> Values
            string[] pparamValues = null;
            long startingIndex = 0;
            for (long ccode = 0; ccode < nCount; ++ccode)
            {
                string key = psCCode[ccode];

                long pparamLength = pnParamCount[ccode];

                pparamValues = new string[pparamLength];
                if (ccode > 0)
                {
                    startingIndex += pnParamCount[(int)ccode - 1];
                }

                for (long pparam = 0; pparam < pparamLength; ++pparam)
                {
                    long pparamIndex = pparam + startingIndex;
                    string value = psParamNames[pparamIndex];
                    pparamValues[pparam] = psParamNames[pparamIndex];
                }

                ppBodies.Add(key, pparamValues);
            }

            CallbackReqDownloadingFormattedRecipe(sPpid, ppBodies);

            if (msgId >= 0)
            {
                _gemDriver.GEMRspPPFmtSend(msgId, sPpid, 0);
                string sLog = String.Format("[EQ ==> XGEM] GEMRspPPFmtSend");
                WriteLog(sLog);
            }

        }

        // S7, F23 : [Host -> EQ] Formatted Process Program Send -> Downloading Recipe
        private void OnGEMReqPPFmtSend(long nMsgId, string sPpid, string sMdln, string sSoftRev, long nCount, string[] psCCode, long[] pnParamCount, string[] psParamNames)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqPPFmtSend => Ppid:{0}, Mdln:{1}, SoftRev:{2}", sPpid, sMdln, sSoftRev);
            WriteLog(sLog);

            System.Threading.Tasks.Task.Run(()
                => DownloadFormattedRecipeAsync(nMsgId, sPpid, sMdln, sSoftRev, nCount, psCCode, pnParamCount, psParamNames));
        }

        // S7, F24 : [Host -> EQ] Formatted Process Program Acknowledge -> Host로부터 S7F24를 받았을 시 발생
        private void OnGEMRspPPFmtSend(string sPpid, long nResult)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspPPFmtSend : Ppid({0}), Result({1})", sPpid, nResult);
            WriteLog(sLog);
        }

        private int MakeSemiStructure(ref long pObject, SemiObject[] objects)
        {
            int result = 0;
            for (int i = 0; i < objects.Length; ++i)
            {
                EN_ITEM_FORMAT format = objects[i].Format;

                switch (format)
                {
                    case EN_ITEM_FORMAT.LIST:
                        {
                            SemiObjectList tempValue = objects[i] as SemiObjectList;
                            result = _gemDriver.SetListItem(pObject, tempValue.GetValue());
                        }
                        break;
                    case EN_ITEM_FORMAT.ASCII:
                        {
                            SemiObjectAscii tempValue = objects[i] as SemiObjectAscii;
                            result = _gemDriver.SetStringItem(pObject, tempValue.GetValue());
                        }
                        break;
                    case EN_ITEM_FORMAT.BINARY:
                        {
                            SemiObjectBinary tempValue = objects[i] as SemiObjectBinary;
                            result = _gemDriver.SetBinaryItem(pObject, tempValue.GetValue());
                        }
                        break;
                    case EN_ITEM_FORMAT.BOOL:
                        {
                            SemiObjectBool tempValue = objects[i] as SemiObjectBool;
                            result = _gemDriver.SetBoolItem(pObject, tempValue.GetValue());
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT:
                        {
                            SemiObjectUInt tempValue = objects[i] as SemiObjectUInt;
                            result = _gemDriver.SetUint1Item(pObject, tempValue.GetValue());
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT2:
                        {
                            SemiObjectUInt2 tempValue = objects[i] as SemiObjectUInt2;
                            result = _gemDriver.SetUint2Item(pObject, tempValue.GetValue());
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT4:
                        {
                            SemiObjectUInt4 tempValue = objects[i] as SemiObjectUInt4;
                            result = _gemDriver.SetUint4Item(pObject, tempValue.GetValue());
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT8:
                        {
                            SemiObjectUInt8 tempValue = objects[i] as SemiObjectUInt8;
                            result = _gemDriver.SetUint8Item(pObject, tempValue.GetValue());
                        }
                        break;
                    case EN_ITEM_FORMAT.INT:
                        {
                            SemiObjectInt tempValue = objects[i] as SemiObjectInt;
                            result = _gemDriver.SetInt1Item(pObject, tempValue.GetValue());
                        }
                        break;
                    case EN_ITEM_FORMAT.INT2:
                        {
                            SemiObjectInt2 tempValue = objects[i] as SemiObjectInt2;
                            result = _gemDriver.SetInt2Item(pObject, tempValue.GetValue());
                        }
                        break;
                    case EN_ITEM_FORMAT.INT4:
                        {
                            SemiObjectInt4 tempValue = objects[i] as SemiObjectInt4;
                            result = _gemDriver.SetInt4Item(pObject, tempValue.GetValue());
                        }
                        break;
                    case EN_ITEM_FORMAT.INT8:
                        {
                            SemiObjectInt8 tempValue = objects[i] as SemiObjectInt8;
                            result = _gemDriver.SetInt8Item(pObject, tempValue.GetValue());
                        }
                        break;
                    case EN_ITEM_FORMAT.FLOAT4:
                        {
                            SemiObjectFloat4 tempValue = objects[i] as SemiObjectFloat4;
                            result = _gemDriver.SetFloat4Item(pObject, tempValue.GetValue());
                        }
                        break;
                    case EN_ITEM_FORMAT.FLOAT8:
                        {
                            SemiObjectFloat8 tempValue = objects[i] as SemiObjectFloat8;
                            result = _gemDriver.SetFloat8Item(pObject, tempValue.GetValue());
                        }
                        break;
                    case EN_ITEM_FORMAT.FILE:
                        break;
                    default:
                        break;
                }

                if (result != 0)
                    return result;
            }

            return result;
        }

        // sysbyte가 0보다 작으면 내가 보낸거, 값이 있으면 받은거다.
        private void UploadingFormattedRecipeUsingUserDefinedMessageAsync(string ppid, long systemByte)
        {
            string sMdln = "";
            string sSoftRev = "";

            sMdln = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;

            System.Diagnostics.FileVersionInfo fv
                = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            sSoftRev = fv.ProductVersion.ToString();

            Dictionary<string, SemiObject[]> ppbodies;
            CallbackReqUploadingFormattedRecipe(ppid, out ppbodies);

            // L, 4
            //  A, PPID
            //  A, MDLN
            //  A, SOFTREV
            //  L, n CCODE LENGTH
            //      L, 2
            //          U4, CCODE
            //          L, N PPARAM LENGTH
            //              MULTI, PPARAM
            long pObject = 0;
            _gemDriver.MakeObject(ref pObject);

            _gemDriver.SetListItem(pObject, 4);
            _gemDriver.SetStringItem(pObject, ppid);
            _gemDriver.SetStringItem(pObject, sMdln);
            _gemDriver.SetStringItem(pObject, sSoftRev);

            _gemDriver.SetListItem(pObject, ppbodies.Count);
            
            foreach (var kvp in ppbodies)
            {
                _gemDriver.SetListItem(pObject, 2);

                uint ccode;
                if (uint.TryParse(kvp.Key, out ccode))
                {
                    _gemDriver.SetUint4Item(pObject, ccode);
                }
                else
                {
                    _gemDriver.SetStringItem(pObject, kvp.Key);
                }

                int paramLength = kvp.Value.Length;
                _gemDriver.SetListItem(pObject, paramLength);

                MakeSemiStructure(ref pObject, kvp.Value);

                //int currentParamCount = kvp.Value.Length;
                //paramCount[index++] = currentParamCount;

                //pparam.AddRange(kvp.Value.ToArray());
            }

            #region <For Simul>
            //string filePath = String.Format(@"{0}Temp.txt", _recipeHandlingPath);
            //if (File.Exists(filePath))
            //    File.Delete(filePath);

            //StreamWriter sw = new StreamWriter(filePath);

            //foreach (KeyValuePair<string, string[]> kvp in ppbodies)
            //{
            //    for (int i = 0; i < kvp.Value.Length; ++i)
            //    {
            //        string body = kvp.Value[i];
            //        body = body.Replace(" ", "");
            //        byte[] byteArray = Encoding.UTF8.GetBytes(body);
            //        string line = String.Format("ASCII {0}", byteArray.Length);
            //        for (int byteIndex = 0; byteIndex < byteArray.Length; ++byteIndex)
            //        {
            //            line += String.Format(" {0}", byteArray[byteIndex]);
            //        }

            //        sw.WriteLine(line);
            //    }

            //    sw.WriteLine("---------------");
            //}

            //sw.Close();
            #endregion </For Simul>

            long result = 0;
            string sLog = String.Empty;
            if (systemByte >= 0)        // 서버에서 받은거
            {
                _gemDriver.SendSECSMessage(pObject, 7, 26, systemByte);

                sLog = String.Format("[EQ ==> XGEM] GEMRspPPFmt(UserDefined) Result : {0}", result);
            }
            else
            {
                sLog = String.Format("[EQ ==> XGEM] GEMReqPPFmtSend(UserDefined) Result : {0}", result);
            }

            _gemDriver.CloseObject(pObject);

            WriteLog(sLog);
        }

        private void UploadingFormattedRecipeAsync(long msgId, string ppid)
        {
            string sMdln = "";
            string sSoftRev = "";

            sMdln = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;

            System.Diagnostics.FileVersionInfo fv
                = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            sSoftRev = fv.ProductVersion.ToString();

            // 
            Dictionary<string, SemiObject[]> ppbodies;
            CallbackReqUploadingFormattedRecipe(ppid, out ppbodies);

            long count = ppbodies.Count;
            string[] ccode = new string[count];
            List<string> pparam = new List<string>();
            long[] paramCount = new long[count];

            int index = 0;
            foreach (var kvp in ppbodies)
            {
                int currentParamCount = kvp.Value.Length;
                paramCount[index++] = currentParamCount;

                //pparam.AddRange(kvp.Value.ToArray());
                for (int i = 0; i < kvp.Value.Length; ++i)
                {
                    pparam.Add(kvp.Value[i].ValueString);
                }
            }

            #region <For Simul>
            //string filePath = String.Format(@"{0}Temp.txt", _recipeHandlingPath);
            //if (File.Exists(filePath))
            //    File.Delete(filePath);

            //StreamWriter sw = new StreamWriter(filePath);

            //foreach (KeyValuePair<string, string[]> kvp in ppbodies)
            //{
            //    for (int i = 0; i < kvp.Value.Length; ++i)
            //    {
            //        string body = kvp.Value[i];
            //        body = body.Replace(" ", "");
            //        byte[] byteArray = Encoding.UTF8.GetBytes(body);
            //        string line = String.Format("ASCII {0}", byteArray.Length);
            //        for (int byteIndex = 0; byteIndex < byteArray.Length; ++byteIndex)
            //        {
            //            line += String.Format(" {0}", byteArray[byteIndex]);
            //        }

            //        sw.WriteLine(line);
            //    }

            //    sw.WriteLine("---------------");
            //}

            //sw.Close();
            #endregion </For Simul>

            long result;
            string sLog = String.Empty;
            if (msgId >= 0)
            {
                result = _gemDriver.GEMRspPPFmt(msgId, ppid, sMdln, sSoftRev, ppbodies.Count, ppbodies.Keys.ToArray(), paramCount, pparam.ToArray());

                sLog = String.Format("[EQ ==> XGEM] GEMRspPPFmt Result : {0}", result);
            }
            else
            {
                result = _gemDriver.GEMReqPPFmtSend(ppid, sMdln, sSoftRev, ppbodies.Count, ppbodies.Keys.ToArray(), paramCount, pparam.ToArray());

                sLog = String.Format("[EQ ==> XGEM] GEMReqPPFmtSend Result : {0}", result);
            }

            WriteLog(sLog);
        }

        // S7, F25 : [Host -> EQ] Formatted Process Program Request -> Uploading Recipe
        private void OnGEMReqPPFmt(long nMsgId, string sPpid)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqPPFmt : Ppid({0})", sPpid);
            WriteLog(sLog);

            System.Threading.Tasks.Task.Run(() => UploadingFormattedRecipeAsync(nMsgId, sPpid));
        }

        // S7, F26 : [Host -> EQ] Formatted Process Program Data -> Host로부터 S7F26을 받았을 시 발생
        private void OnGEMRspPPFmt(string sPpid, string sMdln, string sSoftRev, long nCount, string[] psCCode, long[] pnParamCount, string[] psParamNames)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspPPFmt : Ppid({0})", sPpid);
            WriteLog(sLog);

            System.Threading.Tasks.Task.Run(()
                => DownloadFormattedRecipeAsync(-1, sPpid, sMdln, sSoftRev, nCount, psCCode, pnParamCount, psParamNames));
        }

        // S7, F28 : [Host -> EQ] Process Program Verification Acknowledge -> Host로부터 S7F28을 받았을 시 발생
        private void OnGEMRspPPFmtVerification(string sPpid, long nResult)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspPPFmtVerification : Result({0})", nResult);
            WriteLog(sLog);
        }
        #endregion </Formatted Recipe>

        #region <PPDELETE>
        // S7, F17 : 호스트에서 레시피를 삭제하라고 요청했다.
        private void OnGEMReqPPDelete(long nMsgId, long nCount, string[] psPpid)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqPPDelete");
            WriteLog(sLog);

            EN_ACK7 result = EN_ACK7.OK;
            List<string> recipeFiles;
            bool deleted = false;
            if (false == GetRecipeFileList(out recipeFiles))
            {
                result = EN_ACK7.NOT_FOUND;
            }
            else
            {
                // 모두 지우는게 룰이긴 한데... 위험
                if (psPpid == null)
                {
                    deleted = true;
                    for (int i = 0; i < recipeFiles.Count; ++i)
                    {
                        string path = string.Empty;
                        if (false == GetRecipeFileWithFullPath(recipeFiles[i], out path))
                            continue;

                        try
                        {
                            File.Delete(path);
                        }
                        catch (Exception ex)
                        {
                            result = EN_ACK7.PERMISSION;

                            WriteLog(string.Format("GEMRspPPDelete Failed : {0}, {1}", ex.Message, ex.StackTrace));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < recipeFiles.Count && result.Equals(EN_ACK7.OK); ++i)
                    {
                        for (int index = 0; index < psPpid.Length && result.Equals(EN_ACK7.OK); ++index)
                        {
                            if (recipeFiles[i].Equals(psPpid[index]))
                            {
                                string path = string.Empty;
                                if (false == GetRecipeFileWithFullPath(recipeFiles[i], out path))
                                {
                                    // 없으면 Nak 처리
                                    result = EN_ACK7.NOT_FOUND;
                                    break;
                                }

                                deleted = true;

                                try
                                {
                                    File.Delete(path);
                                }
                                catch (Exception ex)
                                {
                                    result = EN_ACK7.PERMISSION;

                                    WriteLog(string.Format("GEMRspPPDelete Failed : {0}, {1}", ex.Message, ex.StackTrace));
                                }
                            }
                        }

                    }
                }
            }

            if (false == deleted)
            {
                result = EN_ACK7.NOT_FOUND;
            }

            long resultDriver = _gemDriver.GEMRspPPDelete(nMsgId, nCount, psPpid, (long)result);

            if (result == EN_ACK7.OK && resultDriver == 0)
            {
                CallbackRecipeFileIsDeleted(recipeFiles.ToArray());

                sLog = String.Format("[EQ ==> XGEM] GEMRspPPDelete successfully");
            }
            else
            {
                sLog = String.Format("[EQ ==> XGEM] GEMRspPPDelete Failed Result : {0}, Ack : {1}", resultDriver, (long)EN_ACK7.NOT_FOUND);
            }


            WriteLog(sLog);
        }
        #endregion </PPDELETE>

        #region <ECV>
        private void OnGEMReqChangeECV(long nMsgId, long nCount, long[] pnEcids, string[] psVals)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqChangeECV");
            WriteLog(sLog);

            bool bExist = false;
            bool preDefinedChanged = false;
            for (int i = 0; i < nCount; i++)
            {
                if (pnEcids[i] >= _paramRange.ECID_START && pnEcids[i] <= _paramRange.ECID_END)
                {
                    bExist = true;
                    sLog = String.Format("     Ecid:{0}, Value:{1}", pnEcids[i], psVals[i]);
                    WriteLog(sLog);
                }

                if (pnEcids[i] >= _paramRange.PRE_DEFINED_ECID_START && pnEcids[i] <= _paramRange.PRE_DEFINED_ECID_END)
                {
                    preDefinedChanged = true;
                }
            }

            if (true == bExist || preDefinedChanged)
            {
                if (bExist)
                {
                    base.ChangeSystemParameters(pnEcids, psVals);
                }

                _gemDriver.GEMRspChangeECV(nMsgId, 0);
                sLog = String.Format("[EQ ==> XGEM] GEMRspChangeECV");
                WriteLog(sLog);
            }
        }

        private void OnGEMECVChanged(long nCount, long[] pnEcids, string[] psVals)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMECVChanged");
            WriteLog(sLog);

            for (int i = 0; i < nCount; i++)
            {
                if (pnEcids[i] >= _paramRange.ECID_START && pnEcids[i] <= _paramRange.ECID_END)
                {
                    sLog = String.Format("               Ecid:{0}, Value:{1}", pnEcids[i], psVals[i]);
                    WriteLog(sLog);
                }
            }
        }

        private void OnGEMRspAllECInfo(long lCount, long[] plVid, string[] psName, string[] psValue, string[] psDefault, string[] psMin, string[] psMax, string[] psUnit)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspAllECInfo");
            WriteLog(sLog);

            for (int i = 0; i < lCount; i++)
            {
                if (plVid[i] >= _paramRange.ECID_START && plVid[i] <= _paramRange.ECID_END)
                {
                    sLog = String.Format(
                                                "               Vid:{0}, Name:{1}, Value:{2}, Default:{3}, Min:{4}, Max:{5}, Unit:{6},",
                                                    plVid[i], psName[i], psValue[i], psDefault[i], psMin[i], psMax[i], psUnit[i]);
                    WriteLog(sLog);
                }
            }
        }
        #endregion </ECV>

        #region <Communication>
        private void SetCommStatus(long nState)
        {
            _commState = (EN_COMM_STATE)nState;
            m_strCommStatus = _commState.ToString();
        }

        private void SetControlStatus(long nState)
        {
            _controlState = (EN_CONTROL_STATE)nState;

            base.ControlState(_controlState.ToString());
        }

        private void OnGEMControlStateChanged(long nState)
        {
            // 2021.11.06. by shkim [MOD] 버그 수정
            if (nState < 1) return;
            else if (nState == 1) nState = (long)EN_CONTROL_STATE.OFFLINE;
            else if (nState == 3) nState = (long)EN_CONTROL_STATE.HOST_OFFLINE;
            else if (nState == 4) nState = (long)EN_CONTROL_STATE.LOCAL;
            else if (nState == 5) nState = (long)EN_CONTROL_STATE.REMOTE;
            else return;

            SetControlStatus(nState);


            //base.Connection();
        }

        private void OnGEMCommStateChanged(long nState)
        {
            SetCommStatus(nState);

            bool temp = false;
            if (nState == (int)EN_COMM_STATE.COMMUNICATING)
            {
                temp = true;
                SetControlState(_initControlState);
            }
            else if (nState == (int)EN_COMM_STATE.WAIT_CRA)
            {
                temp = true;
                SetControlState(_initControlState);
            }
            else
            {
                temp = false;
            }

            if (Connect != temp)
            {
                Connect = temp;

                if (Connect)
                {
                    UpdateVariables();
                }
            }
            //if (Connect)
            //{
            //    UpdateVariables();
            //}
            WriteLog(String.Format("[XGEM ==> EQ] CommStateChanged : {0}", (EN_COMM_STATE)nState));
        }

        private void OnXGEMStateEvent(long nState)
        {
            string szState = null;
            string sLog = null;

            if (nState == -1) { szState = "Unknown"; }
            else if (nState == 0) { szState = "Init"; }
            else if (nState == 1) { szState = "Idle"; }
            else if (nState == 2) { szState = "Setup"; }
            else if (nState == 3) { szState = "Ready"; }
            else if (nState == 4) { szState = "Execute"; }
            else { szState = "Unknown"; }

            if (nState == 4)
            {
                _gemDriver.GEMSetEstablish(1);

                sLog = String.Format("[EQ ==> XGEM] GEMSetEstablish: {0}", szState);
                WriteLog(sLog);
            }
            else
            {
                sLog = String.Format("[XGEM ==> EQ] OnXGEMStateEvent:{0}", szState);
                WriteLog(sLog);
            }
        }

        private void OnGEMReqOnline(long nMsgId, long nFromState, long nToState)
        {
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqOnline");
            WriteLog(sLog);

            sLog = String.Format("               nMsgId:{0}, nFromState:{1}, nToState:{2}", nMsgId, nFromState, nToState);
            WriteLog(sLog);
            UpdateVariables();
            _gemDriver.GEMRspOnline(nMsgId, 0);
            sLog = String.Format("[EQ ==> XGEM] GEMRspOnline => nMsgId:{0}, nAck:{1}", nMsgId, 0);
            WriteLog(sLog);
        }

        private void OnGEMReqOffline(long nMsgId, long nFromState, long nToState)
        {
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqOffline");
            WriteLog(sLog);

            sLog = String.Format("               nMsgId:{0}, nFromState:{1}, nToState:{2}", nMsgId, nFromState, nToState);
            WriteLog(sLog);

            _gemDriver.GEMRspOffline(nMsgId, 0);
            sLog = String.Format("[EQ ==> XGEM] GEMRsqOffline => nMsgId:{0}, nAck:{1}", nMsgId, 0);
            WriteLog(sLog);
        }
        #endregion </Communication>

        #region <Client To Client>
        private long SendSignal(string device, string messageName, string sendingType, string scenarioName, string[] contentNames, string[] messages, EN_MESSAGE_RESULT result)
        {
            int resultCode = 0;
            long pObjectId = 0;

            resultCode = _gemDriver.MakeObject(ref pObjectId);
            if (resultCode != 0) return resultCode;

            #region <Format>
            // <L
            //      <L
            //          <A SENDING_TYPE>
            //          <A SCENARIO_NAME>
            //          <A RESULT>
            //      >
            //      <L
            //          <A CONTENT_NAME>
            //          <A MESSAGE>
            //      >
            //      <L
            //          <A CONTENT_NAME>
            //          <A MESSAGE>
            //      >
            // >
            #endregion </Format>

            resultCode = _gemDriver.SetListItem(pObjectId, messages.Length + 1);
            if (resultCode != 0) return resultCode;

            string messageOfLogging = String.Empty;

            resultCode = _gemDriver.SetListItem(pObjectId, 3);
            if (resultCode != 0) return resultCode;

            resultCode = _gemDriver.SetAsciiItem(pObjectId, sendingType);
            if (resultCode != 0) return resultCode;

            resultCode = _gemDriver.SetAsciiItem(pObjectId, scenarioName);
            if (resultCode != 0) return resultCode;

            string resultAck = result.ToString();
            resultCode = _gemDriver.SetAsciiItem(pObjectId, resultAck);
            if (resultCode != 0) return resultCode;

            //string message = String.Empty;
            for (int i = 0; i < messages.Length; i++)
            {
                resultCode = _gemDriver.SetListItem(pObjectId, 2);
                if (resultCode != 0) return resultCode;

                resultCode = _gemDriver.SetAsciiItem(pObjectId, contentNames[i]);
                if (resultCode != 0) return resultCode;

                resultCode = _gemDriver.SetAsciiItem(pObjectId, messages[i]);
                if (resultCode != 0) return resultCode;
                //message = String.Format(" [{0} : {1}] ", contentNames[i], messages[i]);

                //messageOfLogging = String.Format("{0},{1}", messageOfLogging, message);
            }

            if (messageOfLogging.Length > 1 && messageOfLogging.Substring(0, 1).Equals(","))
                messageOfLogging = messageOfLogging.Remove(0, 1);

            //string sLog = String.Format("Send Signal Data > To : ' {0} ', Name : ' {1} ', ScenarioName : ' {2} ', Data : ' {3} ' ", device.ToString(), messageName, scenarioName, messageOfLogging);

            //WriteLog(sLog);

            resultCode = _gemDriver.SendSignal(device, messageName, pObjectId);
            //if (resultCode != 0)
            //{
            //    WriteLog(String.Format("Sending Signal Failed : Code {0}", resultCode));

            //}

            return resultCode;
        }

        private void OnRecvSignal(string device, string messageName, long nObjectID)
        {
            //long nCount = 0;
            long result = 0, messageCount = 0, countTemp = 0;

            #region <Format>
            // <L
            //      <L
            //          <A SENDING_TYPE>
            //          <A SCENARIO_NAME>
            //          <A RESULT>
            //      >
            //      <L
            //          <A CONTENT_NAME>
            //          <A MESSAGE>
            //      >
            //      <L
            //          <A CONTENT_NAME>
            //          <A MESSAGE>
            //      >
            // >
            //
            #endregion </Format>

            long tempCount = 0;
            result = _gemDriver.GetListItem(nObjectID, ref tempCount);
            if (result != 0)
            {
                WriteLog(String.Format("Invalid Receiving Message : {0}, {1}", device,
                    messageName));
                return;
            }

            messageCount = tempCount - 1;

            long titleCount = 0;
            result = _gemDriver.GetListItem(nObjectID, ref titleCount);
            if (result != 0 || titleCount < 3)
            {
                WriteLog(String.Format("Invalid Receiving Message : {0}, {1}", device,
                    messageName));
                return;
            }


            string sendingType = String.Empty;
            string scenarioName = String.Empty;

            result = _gemDriver.GetStringItem(nObjectID, ref sendingType);
            if (result != 0)
            {
                WriteLog(String.Format("Invalid Receiving Message : {0}, {1}", device,
                    messageName));
                return;
            }

            result = _gemDriver.GetStringItem(nObjectID, ref scenarioName);
            if (result != 0)
            {
                WriteLog(String.Format("Invalid Receiving Message : {0}, {1}", device,
                    messageName));
                return;
            }

            string resultAck = String.Empty;
            result = _gemDriver.GetStringItem(nObjectID, ref resultAck);
            if (result != 0)
            {
                WriteLog(String.Format("Invalid Receiving Message : {0}, {1}", device,
                    messageName));
                return;
            }

            EN_MESSAGE_RESULT messageResult;
            if (false == Enum.TryParse(resultAck, out messageResult))
            {
                WriteLog(String.Format("Invalid Receiving Message : {0}, {1}", device,
                    messageName));
                return;
            }

            string[] contentNames = new string[messageCount];
            string[] messages = new string[messageCount];

            //string messageOfLogging = String.Empty;
            //string message = String.Empty;
            for (int i = 0; i < messageCount; ++i)
            {
                result = _gemDriver.GetListItem(nObjectID, ref countTemp);
                if (result != 0 || countTemp != 2)
                {
                    WriteLog(String.Format("Invalid Receiving Message : {0}, {1}", device,
                    messageName));
                    return;
                }

                result = _gemDriver.GetStringItem(nObjectID, ref contentNames[i]);
                if (result != 0)
                {
                    WriteLog(String.Format("Invalid Receiving Message : {0}, {1}", device,
                    messageName));
                    return;
                }

                result = _gemDriver.GetStringItem(nObjectID, ref messages[i]);
                if (result != 0)
                {
                    WriteLog(String.Format("Invalid Receiving Message : {0}, {1}", device,
                    messageName));
                    return;
                }

                //message = String.Format(" [{0} : {1}] ", contentNames[i], messages[i]);
                //messageOfLogging = String.Format("{0},{1}", messageOfLogging, message);
            }

            //if (messageOfLogging.Length > 1 && messageOfLogging.Substring(0, 1).Equals(","))
            //    messageOfLogging = messageOfLogging.Remove(0, 1);

            _gemDriver.CloseObject(nObjectID);
            if (result != 0)
            {
                WriteLog(String.Format("Failed to close in receiving message : {0}, {1}", device,
                messageName));
                return;
            }

            //string sLog = String.Format("Recv Signal Data > From : ' {0} ', Name : ' {1} ', Data : ' {2} ' ", device.ToString(), messageName, messageOfLogging);

            //WriteLog(sLog);

            base.ClientToClientMessageReceived(device, messageName, sendingType, scenarioName, contentNames, messages, messageResult);

            //           if (strMessageName.Equals("SEND_RECIPE"))
            //           {
            //               #region RECIPE
            //               long countForTasks = 0;
            //               int ret = _gemDriver.GetListItem(nObjectID, ref countForTasks);
            //               if (ret != 0)
            //               {
            //                   string sLog = String.Format("[EQ ==> XGEM] SetUint1Item Failed : {0}", ret);

            //                   WriteLog(sLog);

            //                   return;
            //               }
            //               string[] paramCodes = new string[countForTasks];
            //               long[] paramCountInTask = new long[countForTasks];
            //               //List<string> paramNames = new List<string>();
            //               List<string> paramValues = new List<string>();
            //               for (byte i = 0; i < countForTasks; ++i)
            //               {
            //                   long subjectCount = 0;
            //                   // ~3
            //                   ret = _gemDriver.GetListItem(nObjectID, ref subjectCount);
            //                   if (ret != 0)
            //                   {
            //                       string sLog = String.Format("[EQ ==> XGEM] SetListItem(Sub-1) Failed : {0}, index : {1}", ret, i);

            //                       WriteLog(sLog);

            //                       return;
            //                   }

            //                   if (subjectCount != 3)
            //                   {

            //                       string sLog = String.Format("[EQ ==> XGEM] GetListItem Count Miss-match : {0}, index : {1} -> ItemCount {2}", ret, i, subjectCount);

            //                       WriteLog(sLog);

            //                       return;
            //                   }

            //                   // Task 이름 ~
            //                   string taskName = String.Empty;
            //                   ret = _gemDriver.GetStringItem(nObjectID, ref taskName);
            //                   if (ret != 0)
            //                   {
            //                       string sLog = String.Format("[EQ ==> XGEM] GetStringItem(TaskName) Failed : {0}, index : {1}", ret, i);

            //                       WriteLog(sLog);

            //                       return;
            //                   }
            //                   paramCodes[i] = taskName;

            //                   // 해당 Task의 파라메터 숫자 ~
            //                   ushort[] paramCount = new ushort[1];
            //                   ret = _gemDriver.GetUint2Item(nObjectID, ref paramCount);
            //                   if (ret != 0)
            //                   {
            //                       string sLog = String.Format("[EQ ==> XGEM] GetUint2Item(Param Count) Failed : {0}, index : {1}", ret, i);

            //                       WriteLog(sLog);

            //                       return;
            //                   }
            //                   paramCountInTask[i] = paramCount[0];

            //                   // 파라메터 숫자 ~
            //                   long parameterCount = 0;
            //                   ret = _gemDriver.GetListItem(nObjectID, ref parameterCount);
            //                   if (ret != 0)
            //                   {
            //                       string sLog = String.Format("[EQ ==> XGEM] GetListItem Failed : {0}, index : {1}", ret, i);

            //                       WriteLog(sLog);

            //                       return;
            //                   }

            //                   for (long paramIndex = 0; paramIndex < parameterCount; ++paramIndex)
            //                   {
            //                       // 리스트
            //                       long count = 0;
            //                       ret = _gemDriver.GetListItem(nObjectID, ref count);
            //                       if (ret != 0)
            //                       {
            //                           string sLog = String.Format("[EQ ==> XGEM] GetListItem Failed(Param Format, Value) : {0}, index : {1}", ret, i);

            //                           WriteLog(sLog);

            //                           return;
            //                       }

            //                       //  파람 포멧
            //                       byte[] format = new byte[1];
            //                       ret = _gemDriver.GetUint1Item(nObjectID, ref format);
            //                       if (ret != 0)
            //                       {
            //                           string sLog = String.Format("[EQ ==> XGEM] GetUint1Item(Param Format) Failed : {0}, index : {1}", ret, paramIndex);

            //                           WriteLog(sLog);

            //                           return;
            //                       }

            //                       // 파라메터 이름
            //                       //string paramName = String.Empty;
            //                       //ret = m_XGem.GetStringItem(pObjectId, ref paramName);
            //                       //if (ret != 0)
            //                       //{
            //                       //    string sLog = String.Format("[EQ ==> XGEM] GetStringItem(paramName) Failed : {0}, index : {1}", ret, paramIndex);

            //                       //    WriteLog(sLog);

            //                       //    return;
            //                       //}

            //                       //paramNames.Add(paramName);

            //                       // 파라메터 값
            //                       string value = String.Empty;
            //                       ret = _gemDriver.GetStringItem(nObjectID, ref value);
            //                       if (ret != 0)
            //                       {
            //                           string sLog = String.Format("[EQ ==> XGEM] GetStringItem(Value) Failed : {0}, index : {1}", ret, paramIndex);

            //                           WriteLog(sLog);

            //                           return;
            //                       }

            //                       paramValues.Add(value);
            //                   }
            //               }

            //               ret = _gemDriver.CloseObject(nObjectID);
            //               if (ret != 0)
            //               {
            //                   string sLog = String.Format("[EQ ==> XGEM] CloseObject Failed : {0}", ret);

            //                   WriteLog(sLog);

            //                   return;
            //               }

            //               //string[] names = new string[paramValues.Count];
            //               string[] values = new string[paramValues.Count];

            //               //names = paramNames.ToArray();
            //               values = paramValues.ToArray();

            //               //WriteLog(String.Format("Receiving Signal Elapsed Time : {0}", Math.Round(dwell.Elapsed, 2)));
            //#endregion
            //           }
            //           else
            //           {
            //               long nCount = 0;
            //               string messageOfLogging = String.Empty;

            //               _gemDriver.GetListItem(nObjectID, ref nCount);
            //               string[] arrMessages = new string[nCount];
            //               for (int i = 0; i < nCount; ++i)
            //               {
            //                   _gemDriver.GetStringItem(nObjectID, ref arrMessages[i]);

            //                   if (i == 0)
            //                       messageOfLogging = "Recv Signal";

            //                   messageOfLogging = String.Format("{0}, {1}", messageOfLogging, arrMessages[i]);
            //               }

            //               if (messageOfLogging.Length > 1 && messageOfLogging.Substring(0, 1).Equals(","))
            //                   messageOfLogging = messageOfLogging.Remove(0, 1);

            //               _gemDriver.CloseObject(nObjectID);
            //               if (nCount <= 0)
            //               {
            //                   string sLog = String.Format("Received Signal Data is Invalid! > From : {0}, Name : {1}", strSourceDevice, strMessageName);
            //                   WriteLog(sLog);

            //                   return;
            //               }

            //               WriteLog(messageOfLogging);

            //               string[] contentNames = new string[1];
            //               base.ClientToClientMessageReceived(strSourceDevice, strMessageName, contentNames, arrMessages);
            //           }
        }
        #endregion </Client To Client>

        #region <Terminal>
        private void OnGEMTerminalMessage(long nTid, string sMsg)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMTerminalMessage : Tid({0}), Msg({1})", nTid, sMsg);
            WriteLog(sLog);

            ShowTerminalMessage(sMsg);
        }

        private void OnGEMTerminalMultiMessage(long nTid, long nCount, string[] psMsg)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMTerminalMultiMessage : Tid({0}), ", nTid);
            WriteLog(sLog);
            string strMessage = String.Empty;
            for (int i = 0; i < nCount; i++)
            {
                sLog = String.Format("               B: {0}", psMsg[i]);
                strMessage += psMsg[i];
                if (i != nCount - 1)
                {
                    strMessage += "\r\n";
                }

                WriteLog(sLog);
            }
            ShowTerminalMessage(strMessage);
        }
        #endregion </Terminal>

        #endregion </Event Interface>

        #endregion </Methods>
    }
}