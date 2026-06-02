using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;

using Define.DefineConstant;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

//using GEM_XGem300Pro;
using XGEM300PRO.Library;

namespace FrameOfSystem3.SECSGEM.SecsGemDll
{
    public sealed class EventInfo
    {
        public EventInfo() { }
        public EventInfo(long eventId, long[] vids, string[] values, bool useCheckSecondaryAck)
        {
            EventId = eventId;
            VariableIds = new long[vids.Length];
            VariableValues = new string[values.Length];

            Array.Copy(vids, VariableIds, vids.Length);
            Array.Copy(values, VariableValues, values.Length);

            UseCheckSecondaryAck = useCheckSecondaryAck;

            SystemByte = -1;
        }
        public long EventId { get; set; }
        public long[] VariableIds { get; set; }
        public string[] VariableValues { get; set; }
        public long SystemByte { get; set; }
        public bool UseCheckSecondaryAck { get; set; }
        public int GetVariableCount()
        {
            if (VariableIds == null || VariableValues == null)
                return 0;

            return VariableIds.Length;
        }
        public bool HasAnyLinkedVariable()
        {
            if (VariableIds == null ||
               VariableValues == null)
                return false;

            if (VariableIds.Length == 0 ||
                VariableValues.Length == 0)
                return false;

            return true;
        }
    }

    class XGemPro300 : SecsGem300
    {
        #region <Fields>
        //protected XGem300ProNet _gemDriver = null;
        protected XGem300ProW _gemDriver = null;

        private Dictionary<long, string> _eventNameById = new Dictionary<long, string>();
        private Dictionary<long, string> _vidNameById = new Dictionary<long, string>();

        private readonly ProcessForXGem DataProcessor = new ProcessForXGem();
        private ConcurrentDictionary<long, bool> _eventToSend = new ConcurrentDictionary<long, bool>();
        private const int DelayTimeForClosing = 1000;

        private const uint DelayTimeForUpdateVariables = 200;
        private const uint TimeoutCheckEvents = 1000;
        private readonly TickCounter_.TickCounter _delayForEvent = new TickCounter_.TickCounter();
        private readonly TickCounter_.TickCounter _timeCheckerEvent = new TickCounter_.TickCounter();
        private ConcurrentQueue<EventInfo> _queuedEvents = new ConcurrentQueue<EventInfo>();
        private EventInfo _currentEventInfo = null;
        private int _stepForSendingEvent;
        //private long _earlySecondarySysbyte = -1;
        //private bool _earlySecondarySeen = false;
        private readonly HashSet<long> _earlySecondarySysbytes = new HashSet<long>();
        private readonly object _eventLock = new object();

        #region CommStatus
        //protected string m_strCommStatus = string.Empty;
        //protected EN_COMM_STATE _commState;
        #endregion

        #region ControlStatus
        //string m_strControlStatus = string.Empty;
        //private EN_CONTROL_STATE _controlState;
        #endregion

        #region <Gem300>
        private ICarrierManagementDriver _cmsDriver;
        private IProcessJobDriver _pjDriver;
        private IControlJobDriver _cjDriver;
        private ISubstrateTrackingDriver _stsDriver;
        #endregion </Gem300>

        #endregion </Fields>

        #region <Properties>
        internal override ICarrierManagementDriver CmsDriver
        {
            get { return _cmsDriver; }
        }

        internal override IProcessJobDriver PjDriver
        {
            get { return _pjDriver; }
        }

        internal override IControlJobDriver CjDriver
        {
            get { return _cjDriver; }
        }

        internal override ISubstrateTrackingDriver StsDriver
        {
            get { return _stsDriver; }
        }
        #endregion </Properties>

        #region <Methods>

        #region <Abstract Interface>

        #region <Initialize & Close>
        public override bool Init(string configPath)
        {
            //Enum.TryParse(controlState, out _initControlState);

            //_initControlState = EN_CONTROL_STATE.HOST_OFFLINE;

            // 변수값 초기화

            SetCommStatus((long)EN_COMM_STATE.DISABLED);

            // m_XGem = new XGemProW();
            _gemDriver = new XGem300ProW();//new GEM_XGem300Pro.XGem300ProNet();

            DataProcessor.InitGemInfo(configPath);

            LinkFunctions();

            long nResult = _gemDriver.Initialize(configPath);
            if (0 != nResult)
            {
                WriteLog(string.Format("Gem Init Fail : {0}", nResult));
                return false;
            }

            Thread.Sleep(1000);

            nResult = _gemDriver.Start();
            if (0 != nResult)
            {
                WriteLog(string.Format("Gem Start Fail : {0}", nResult));
                return false;
            }

            BuildGem300Drivers();

            return true;
        }
        public override void Close()
        {
            if (_gemDriver == null) return;

            long nResult = _gemDriver.Stop();
            if (0 != nResult)
            {
                WriteLog(string.Format("Gem Stop Fail : {0}", nResult));
            }

            //foreach (Process pr in Process.GetProcesses())
            //{
            //    if (pr.ProcessName.Equals("XGem"))
            //    {
            //        // 2022.11.03. by shkim. [MOD] 이미 프로세스가 종료됬을 때에 대한 예외처리 추가
            //        try
            //        {
            //            pr.Kill();

            //            _gemDriver = null;

            //            return;
            //        }
            //        catch
            //        {

            //        }
            //    }
            //}

            //System.Threading.Tasks.Task.Run(() => CloseDriver());
            Thread.Sleep(DelayTimeForClosing);

            nResult = _gemDriver.Close();
            if (0 != nResult)
            {
                WriteLog(string.Format("Gem Close Fail : {0}", nResult));
            }

            _cmsDriver = null;
            _pjDriver = null;
            _cjDriver = null;
            _stsDriver = null;
            _gemDriver = null;
        }
        private async System.Threading.Tasks.Task CloseDriver()
        {
            await System.Threading.Tasks.Task.Delay(DelayTimeForClosing);

            _gemDriver.Close();
        }
        private void BuildGem300Drivers()
        {
            if (_gemDriver == null)
                return;

            _cmsDriver = new XGemCarrierManagementDriver(_gemDriver);
            _pjDriver = new XGemProcessJobDriver(_gemDriver);
            _cjDriver = new XGemControlJobDriver(_gemDriver);
            _stsDriver = new XGemSubstrateTrackingDriver(_gemDriver);
        }
        public override void MakeGemSpecification(string configDirectory, ref Dictionary<string, StatusVariable> statusVariableList, ref Dictionary<long, List<StatusVariable>> reportList, ref Dictionary<string, CollectionEvent> collectionEventList)
        {
            // XGem Config에서 각 항목을 Excel로 저장 -> Txt로 내보내기(탭으로 구분)하여 해당 폴더에 넣으면 자동 생성됨
            // SVID
            if (DataProcessor.MakeGemVariableList(configDirectory, out statusVariableList))
            {
                StatusVariableList.Clear();
                foreach (var item in statusVariableList)
                {
                    StatusVariableList[item.Key] = item.Value;
                    _vidNameById[item.Value.Id] = item.Key;
                }
            }

            // RptId
            if (DataProcessor.MakeGemReportList(configDirectory, StatusVariableList, out reportList))
            {
                ReportList.Clear();
                foreach (var item in reportList)
                {
                    ReportList[item.Key] = item.Value;
                }
            }

            // CEID
            if (DataProcessor.MakeGemEventList(configDirectory, ReportList, out collectionEventList))
            {
                CollectionEventList.Clear();
                foreach (var item in collectionEventList)
                {
                    CollectionEventList[item.Key] = item.Value;
                    
                    _eventNameById[item.Value.Id] = item.Key;
                }
            }
        }

        //24.09.27 by wdw [ADD] EQUIPMNET CONSTANT 추가
        public override void MakeGemECVSpecification(string configDirectory, ref Dictionary<string, EquipmentConstant> equipmentConstantList)
        {
            // XGem Config에서 각 항목을 Excel로 저장 -> Txt로 내보내기(탭으로 구분)하여 해당 폴더에 넣으면 자동 생성됨 
            if (DataProcessor.MakeGemConstantList(configDirectory, out equipmentConstantList))
            {
                EquipmentConstantListById.Clear();
                EquipmentConstantList.Clear();
                foreach (var item in equipmentConstantList)
                {
                    EquipmentConstantList[item.Key] = item.Value;
                    EquipmentConstantListById[item.Value.Id] = item.Key;
                }
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
            return DataProcessor.CommunicationStatus;
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
            return DataProcessor.CurrentControlStatus;
        }
        #endregion </Comm & Control State>

        #region <Alarm>
        public override void SetAlarm(int nAlarm)
        {
            WriteLog(string.Format("[EQ ==> XGEM] Report Alarm : {0}", nAlarm));

            _gemDriver.GEMSetAlarm(nAlarm, 1);
        }

        public override void ClearAlarm(int nAlarm)
        {
            WriteLog(string.Format("[EQ ==> XGEM] Clear Alarm : {0}", nAlarm));

            _gemDriver.GEMSetAlarm(nAlarm, 0);
        }
        #endregion </Alarm>

        #region <Client To Client Message>
        public override bool SendClientToClientMessage(string device, string messageName, string sendingType, string scenarioName, string[] contentNames, string[] messages, EN_MESSAGE_RESULT result)
        {
            return false;
        }
        #endregion </Client To Client Message>

        #region <User Defined SecsMessage>
        public override bool SendUserDefinedSecsMessage(long stream, long function, List<SemiObject> messageStructure)
        {
            long pObjectId = 0, pSystemByte = 0;

            WriteLog(string.Format("[EQ ==> XGEM] Send UserDefindSecsMessage : S{0}F{1}", stream, function));

            if (false == MakeAndSendSecsMessage(stream, function, ref pObjectId, ref pSystemByte, ref messageStructure))
            {
                WriteLog(string.Format("User define message send Fail"));

                return false;
            }

            return true;
        }

        #endregion </User Defined SecsMessage>

        #region <Collection Event>
        public override bool SendEvent(long nEventID, long[] arrVids, string[] arrVidValues, bool useCheckSecondaryMessage)
        {
            if (nEventID > 0)
            {
                if (false == Connect) return false;

                // SendEvent 시점은 큐 적재일 뿐이므로 아직 완료로 보지 않는다.
                _eventToSend[nEventID] = false;
                // 2025.08.12 jhlim [MOD] 체크 안 하는 경우 도착한 것으로 간주
                //_eventToSend[nEventID] = !useCheckSecondaryMessage;

                long[] vidsIn;
                if (arrVids == null)
                {
                    vidsIn = new long[0];
                }
                else
                {
                    vidsIn = arrVids;
                }

                string[] valsIn;
                if (arrVidValues == null)
                {
                    valsIn = new string[0];
                }
                else
                {
                    valsIn = arrVidValues;
                }

                var n = (vidsIn.Length < valsIn.Length) ? vidsIn.Length : valsIn.Length;

                var vidsSan = new long[n];
                var valsSan = new string[n];

                Array.Copy(vidsIn, vidsSan, n);
                for (int i = 0; i < n; i++)
                {
                    valsSan[i] = (i < valsIn.Length && valsIn[i] != null) ? valsIn[i] : string.Empty;
                }
                
                var info = new EventInfo(nEventID, vidsSan, valsSan, useCheckSecondaryMessage);
                _queuedEvents.Enqueue(info);
                return true;
            }

            return false;
        }

        //public override bool SendEvent(long nEventID, long[] arrVids, string[] arrVidValues, bool useCheckSecondaryMessage)
        //{
        //    if (nEventID > 0)
        //    {
        //        if (false == Connect) return false;

        //        // 2025.08.12 jhlim [MOD] 체크 안 하는 경우 도착한 것으로 간주
        //        _eventToSend[nEventID] = !useCheckSecondaryMessage;

        //        //                 if (false == m_dicEvent.ContainsKey(nEventID))
        //        //                 {
        //        //                     m_dicEvent.TryAdd(nEventID, false);
        //        //                 }
        //        //                 else
        //        //                 {
        //        //                     m_dicEvent[nEventID] = false;
        //        //                 }

        //        for (int i = 0; i < arrVidValues.Length; ++i)
        //        {
        //            if (arrVidValues[i] == null)
        //                arrVidValues[i] = string.Empty;
        //        }

        //        string eventId = string.Empty;
        //        foreach (var item in CollectionEventList)
        //        {
        //            if (item.Value.Id.Equals(nEventID))
        //            {
        //                eventId = item.Key;
        //                break;
        //            }
        //        }

        //        _gemDriver.GEMSetEventEx(nEventID, arrVids.Length, arrVids, arrVidValues);
        //        string strLog = string.Format("[EQ ==> XGEM] S6F11 : {0}({1})", eventId, nEventID);

        //        if (arrVids != null && arrVidValues != null)
        //        {
        //            strLog = string.Format("{0}, Vids :", strLog);

        //            for (int i = 0; i < arrVids.Length; ++i)
        //            {
        //                foreach (var item in StatusVariableList)
        //                {
        //                    if (arrVids[i].Equals(item.Value.Id))
        //                    {
        //                        strLog = string.Format("{0} [{1}({2}):{3}]", strLog, item.Key, arrVids[i], arrVidValues[i]);
        //                        break;
        //                    }
        //                }
        //            }
        //        }

        //        //for (int i = 0; i < arrVids.Length; ++i)
        //        //{
        //        //    strLog = string.Format("{0}[{1} : {2}] ", strLog, arrVids[i], arrVidValues[i]);
        //        //}

        //        if (useCheckSecondaryMessage)
        //        {
        //            WriteLog(strLog);
        //        }

        //        return true;
        //    }

        //    return false;
        //}

        public override bool IsEventDone(long nEventID)
        {
            return _eventToSend.TryGetValue(nEventID, out bool done) && done;
            //if (false == _eventToSend.ContainsKey(nEventID))
            //    return false;
            //else
            //{
            //    return _eventToSend[nEventID];
            //}
        }
        private void WriteS6F11Log(long eventId, long[] vids, string[] values, long systemByte)
        {
            if (vids == null) vids = new long[0];
            if (values == null) values = new string[0];
            var n = vids.Length < values.Length ? vids.Length : values.Length;

            string eventName = string.Empty;
            if (false == _eventNameById.TryGetValue(eventId, out eventName)) 
                eventName = string.Empty;

            var sb = new StringBuilder();

            sb.Append("[EQ ==> XGEM] S6F11 : ")
              .Append(eventName).Append('(').Append(eventId).Append(')');

            if (n > 0)
            {
                sb.Append(", Vids :");
                for (int i = 0; i < n; i++)
                {
                    string vidName;
                    _vidNameById.TryGetValue(vids[i], out vidName);

                    sb.Append(" [")
                      .Append(vidName ?? "VID")
                      .Append('(').Append(vids[i]).Append("):")
                      .Append(values[i] ?? string.Empty)
                      .Append(']');
                }
            }

            // SystemByte
            sb.Append(", SystemByte : 0x").Append(systemByte.ToString("X"));

            // 너무 길면 줄일까..?
            //const int MaxLogChars = 8000;
            //if (sb.Length > MaxLogChars)
            //{
            //    sb.Length = MaxLogChars - 3;
            //    sb.Append("...");
            //}

            WriteLog(sb.ToString());
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

            string sLog = string.Format("[EQ ==> XGEM] OnGEMReqPPFmt : Ppid({0})", recipeName);
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

            sLog = string.Format("[EQ ==> XGEM] GEMReqPPFmtSend");
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
                WriteLog(string.Format("[EQ ==> XGEM] Fail to GEMReqPPEx ({0})", nResult));
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
                WriteLog(string.Format("[EQ ==> XGEM] Fail to GEMReqPPFmt ({0})", result));
            }
        }
        #endregion </Downloading>

        #endregion </Recipe>

        #region <Update vid>
        public override void UpdateVariable(long vid, System.Collections.Generic.List<SemiObject> values)
        {
            if (values == null || values.Count == 0)
            {
                WriteLog($"Set Vid Data > Index : {vid}, Data : <empty>");
                return;
            }

            long pObjectId = 0;
            long result = _gemDriver.MakeObject(ref pObjectId);
            if (result != 0)
            {
                WriteLog($"MakeObject Failed : {result}");
                return;
            }

            var sb = new StringBuilder();
            bool first = true;
            System.Action appendCommaIfNeeded = () => { if (!first) sb.Append(','); first = false; };

            try
            {
                int count = values.Count;
                for (int i = 0; i < count; ++i)
                {
                    var obj = values[i];
                    if (obj == null) continue; // 널 방어

                    switch (obj.Format)
                    {
                        case EN_ITEM_FORMAT.LIST:
                            {
                                var listItem = obj as SemiObjectList;
                                if (listItem == null) break;

                                var arr = listItem.GetValues();
                                if (arr == null || arr.Length == 0) break;

                                result = _gemDriver.SetListItem(pObjectId, arr[0]);
                                if (result != 0) { WriteLog($"SetListItem Failed : {result}"); return; }

                                appendCommaIfNeeded();
                                sb.Append(arr[0]);
                                break;
                            }

                        case EN_ITEM_FORMAT.ASCII:
                            {
                                var aItem = obj as SemiObjectAscii;
                                if (aItem == null) break;

                                var arr = aItem.GetValues();
                                var val = (arr != null && arr.Length > 0) ? (arr[0] ?? string.Empty) : string.Empty;

                                result = _gemDriver.SetStringItem(pObjectId, val);
                                if (result != 0) { WriteLog($"SetStringItem Failed : {result}"); return; }

                                appendCommaIfNeeded();
                                sb.Append(val);
                                break;
                            }

                        case EN_ITEM_FORMAT.BINARY:
                            {
                                var bItem = obj as SemiObjectBinary;
                                if (bItem == null) break;

                                var bytes = bItem.GetValues();
                                result = _gemDriver.SetBinaryItem(pObjectId, bytes);
                                if (result != 0) { WriteLog($"SetBinaryItem Failed : {result}"); return; }

                                appendCommaIfNeeded();
                                // 로깅은 사람이 읽기 좋은 Hex 혹은 간결한 Base64 중 택1
                                sb.Append(bytes != null ? BitConverter.ToString(bytes) : "<null>");
                                break;
                            }

                        case EN_ITEM_FORMAT.BOOL:
                            {
                                var bo = obj as SemiObjectBool;
                                if (bo == null) break;

                                var val = bo.GetValue();
                                result = _gemDriver.SetBoolItem(pObjectId, val);
                                if (result != 0) { WriteLog($"SetBoolItem Failed : {result}"); return; }

                                appendCommaIfNeeded();
                                sb.Append(val ? "true" : "false");
                                break;
                            }

                        case EN_ITEM_FORMAT.FLOAT4:
                            {
                                var f4 = obj as SemiObjectFloat4;
                                if (f4 == null) break;

                                var val = f4.GetValue();
                                result = _gemDriver.SetFloat4Item(pObjectId, val);
                                if (result != 0) { WriteLog($"SetFloat4Item Failed : {result}"); return; }

                                appendCommaIfNeeded();
                                sb.Append(val.ToString(CultureInfo.InvariantCulture));
                                break;
                            }

                        case EN_ITEM_FORMAT.FLOAT8:
                            {
                                var f8 = obj as SemiObjectFloat8;
                                if (f8 == null) break;

                                var val = f8.GetValue();
                                result = _gemDriver.SetFloat8Item(pObjectId, val);
                                if (result != 0) { WriteLog($"SetFloat8Item Failed : {result}"); return; }

                                appendCommaIfNeeded();
                                sb.Append(val.ToString(CultureInfo.InvariantCulture));
                                break;
                            }

                        case EN_ITEM_FORMAT.INT:
                            {
                                var i1 = obj as SemiObjectInt;
                                if (i1 == null) break;

                                var val = i1.GetValue();
                                result = _gemDriver.SetInt1Item(pObjectId, val);
                                if (result != 0) { WriteLog($"SetInt1Item Failed : {result}"); return; }

                                appendCommaIfNeeded();
                                sb.Append(val.ToString(CultureInfo.InvariantCulture));
                                break;
                            }

                        case EN_ITEM_FORMAT.INT2:
                            {
                                var i2 = obj as SemiObjectInt2;
                                if (i2 == null) break;

                                var val = i2.GetValue();
                                result = _gemDriver.SetInt2Item(pObjectId, val);
                                if (result != 0) { WriteLog($"SetInt2Item Failed : {result}"); return; }

                                appendCommaIfNeeded();
                                sb.Append(val.ToString(CultureInfo.InvariantCulture));
                                break;
                            }

                        case EN_ITEM_FORMAT.INT4:
                            {
                                var i4 = obj as SemiObjectInt4;
                                if (i4 == null) break;

                                var val = i4.GetValue();
                                result = _gemDriver.SetInt4Item(pObjectId, val);
                                if (result != 0) { WriteLog($"SetInt4Item Failed : {result}"); return; }

                                appendCommaIfNeeded();
                                sb.Append(val.ToString(CultureInfo.InvariantCulture));
                                break;
                            }

                        case EN_ITEM_FORMAT.INT8:
                            {
                                var i8 = obj as SemiObjectInt8;
                                if (i8 == null) break;

                                var val = i8.GetValue();
                                result = _gemDriver.SetInt8Item(pObjectId, val);
                                if (result != 0) { WriteLog($"SetInt8Item Failed : {result}"); return; }

                                appendCommaIfNeeded();
                                sb.Append(val.ToString(CultureInfo.InvariantCulture));
                                break;
                            }

                        case EN_ITEM_FORMAT.UINT:
                            {
                                var u1 = obj as SemiObjectUInt;
                                if (u1 == null) break;

                                var val = u1.GetValue();
                                result = _gemDriver.SetUint1Item(pObjectId, val);
                                if (result != 0) { WriteLog($"SetUint1Item Failed : {result}"); return; }

                                appendCommaIfNeeded();
                                sb.Append(val.ToString(CultureInfo.InvariantCulture));
                                break;
                            }

                        case EN_ITEM_FORMAT.UINT2:
                            {
                                var u2 = obj as SemiObjectUInt2;
                                if (u2 == null) break;

                                var val = u2.GetValue();
                                result = _gemDriver.SetUint2Item(pObjectId, val);
                                if (result != 0) { WriteLog($"SetUint2Item Failed : {result}"); return; }

                                appendCommaIfNeeded();
                                sb.Append(val.ToString(CultureInfo.InvariantCulture));
                                break;
                            }

                        case EN_ITEM_FORMAT.UINT4:
                            {
                                var u4 = obj as SemiObjectUInt4;
                                if (u4 == null) break;

                                var val = u4.GetValue();
                                result = _gemDriver.SetUint4Item(pObjectId, val);
                                if (result != 0) { WriteLog($"SetUint4Item Failed : {result}"); return; }

                                appendCommaIfNeeded();
                                sb.Append(val.ToString(CultureInfo.InvariantCulture));
                                break;
                            }

                        case EN_ITEM_FORMAT.UINT8:
                            {
                                var u8 = obj as SemiObjectUInt8;
                                if (u8 == null) break;

                                var val = u8.GetValue();
                                result = _gemDriver.SetUint8Item(pObjectId, val);
                                if (result != 0) { WriteLog($"SetUint8Item Failed : {result}"); return; }

                                appendCommaIfNeeded();
                                sb.Append(val.ToString(CultureInfo.InvariantCulture));
                                break;
                            }
                    }
                }

                string sLog = $"Set Vid Data > Index : {vid}, Data : {sb.ToString()}";
                WriteLog(sLog);

                result = _gemDriver.GEMSetVariables(pObjectId, vid);
                if (result != 0)
                {
                    WriteLog($"SetVariableEx Failed : {result}");
                    return;
                }
            }
            finally
            {
                long close = _gemDriver.CloseObject(pObjectId);
                if (close != 0)
                {
                    WriteLog($"CloseObject Failed : {close}");
                }
            }
        }

        //public override void UpdateVariable(long vid, List<SemiObject> values)
        //{
        //    long result = 0;
        //    long pObjectId = 0;

        //    result = _gemDriver.MakeObject(ref pObjectId);
        //    if (result != 0) return;

        //    int count = values.Count;
        //    var sb = new StringBuilder();
        //    for (int i = 0; i < count; ++i)
        //    {
        //        SemiObject obj = values[i];

        //        #region Format
        //        switch (obj.Format)
        //        {
        //            case EN_ITEM_FORMAT.LIST:
        //                {
        //                    SemiObjectList listItem = obj as SemiObjectList;
        //                    if (listItem != null)
        //                    {
        //                        result = _gemDriver.SetListItem(pObjectId, listItem.GetValues()[0]);
        //                        if (sb.Length > 0)
        //                            sb.Append(',');
        //                        sb.Append(listItem.GetValues()[0]);
        //                    }
        //                }
        //                break;

        //            case EN_ITEM_FORMAT.ASCII:
        //                {
        //                    SemiObjectAscii aItem = obj as SemiObjectAscii;
        //                    if (aItem != null)
        //                    {
        //                        result = _gemDriver.SetStringItem(pObjectId, aItem.GetValues()[0]);
        //                        if (sb.Length > 0)
        //                            sb.Append(',');
        //                        sb.Append(aItem.GetValues()[0]);
        //                    }
        //                }
        //                break;

        //            case EN_ITEM_FORMAT.BINARY:
        //                {
        //                    SemiObjectBinary bItem = obj as SemiObjectBinary;
        //                    if (bItem != null)
        //                    {
        //                        result = _gemDriver.SetBinaryItem(pObjectId, bItem.GetValues());
        //                        if (sb.Length > 0)
        //                            sb.Append(',');
        //                        sb.Append(bItem.GetValues());
        //                    }
        //                }
        //                break;
        //            case EN_ITEM_FORMAT.BOOL:
        //                {
        //                    SemiObjectBool boItem = obj as SemiObjectBool;
        //                    if (boItem != null)
        //                    {
        //                        result = _gemDriver.SetBoolItem(pObjectId, boItem.GetValue());

        //                        if (sb.Length > 0)
        //                            sb.Append(',');
        //                        sb.Append(boItem.GetValue());
        //                    }
        //                }
        //                break;
        //            case EN_ITEM_FORMAT.FLOAT4:
        //                {
        //                    SemiObjectFloat4 f4Item = obj as SemiObjectFloat4;
        //                    if (f4Item != null)
        //                    {
        //                        result = _gemDriver.SetFloat4Item(pObjectId, f4Item.GetValue());

        //                        if (sb.Length > 0)
        //                            sb.Append(',');
        //                        sb.Append(f4Item.GetValue());
        //                    }
        //                }
        //                break;
        //            case EN_ITEM_FORMAT.FLOAT8:
        //                {
        //                    SemiObjectFloat8 f8Item = obj as SemiObjectFloat8;
        //                    if (f8Item != null)
        //                    {
        //                        result = _gemDriver.SetFloat8Item(pObjectId, f8Item.GetValue());

        //                        if (sb.Length > 0)
        //                            sb.Append(',');
        //                        sb.Append(f8Item.GetValue());
        //                    }
        //                }
        //                break;
        //            case EN_ITEM_FORMAT.INT:
        //                {
        //                    SemiObjectInt i1Item = obj as SemiObjectInt;
        //                    if (i1Item != null)
        //                    {
        //                        result = _gemDriver.SetInt1Item(pObjectId, i1Item.GetValue());

        //                        if (sb.Length > 0)
        //                            sb.Append(',');
        //                        sb.Append(i1Item.GetValue());
        //                    }
        //                }
        //                break;
        //            case EN_ITEM_FORMAT.INT2:
        //                {
        //                    SemiObjectInt2 i2Item = obj as SemiObjectInt2;
        //                    if (i2Item != null)
        //                    {
        //                        result = _gemDriver.SetInt2Item(pObjectId, i2Item.GetValue());

        //                        if (sb.Length > 0)
        //                            sb.Append(',');
        //                        sb.Append(i2Item.GetValue());
        //                    }
        //                }
        //                break;
        //            case EN_ITEM_FORMAT.INT4:
        //                {
        //                    SemiObjectInt4 i4Item = obj as SemiObjectInt4;
        //                    if (i4Item != null)
        //                    {
        //                        result = _gemDriver.SetInt4Item(pObjectId, i4Item.GetValue());

        //                        if (sb.Length > 0)
        //                            sb.Append(',');
        //                        sb.Append(i4Item.GetValue());
        //                    }
        //                }
        //                break;
        //            case EN_ITEM_FORMAT.INT8:
        //                {
        //                    SemiObjectInt8 i8Item = obj as SemiObjectInt8;
        //                    if (i8Item != null)
        //                    {
        //                        result = _gemDriver.SetInt8Item(pObjectId, i8Item.GetValue());

        //                        if (sb.Length > 0)
        //                            sb.Append(',');
        //                        sb.Append(i8Item.GetValue());
        //                    }
        //                }
        //                break;
        //            case EN_ITEM_FORMAT.UINT:
        //                {
        //                    SemiObjectUInt ui1Item = obj as SemiObjectUInt;
        //                    if (ui1Item != null)
        //                    {
        //                        result = _gemDriver.SetUint1Item(pObjectId, ui1Item.GetValue());

        //                        if (sb.Length > 0)
        //                            sb.Append(',');
        //                        sb.Append(ui1Item.GetValue());
        //                    }
        //                }
        //                break;
        //            case EN_ITEM_FORMAT.UINT2:
        //                {
        //                    SemiObjectUInt2 ui2Item = obj as SemiObjectUInt2;
        //                    if (ui2Item != null)
        //                    {
        //                        result = _gemDriver.SetUint2Item(pObjectId, ui2Item.GetValue());

        //                        if (sb.Length > 0)
        //                            sb.Append(',');
        //                        sb.Append(ui2Item.GetValue());
        //                    }
        //                }
        //                break;
        //            case EN_ITEM_FORMAT.UINT4:
        //                {
        //                    SemiObjectUInt4 ui4Item = obj as SemiObjectUInt4;
        //                    if (ui4Item != null)
        //                    {
        //                        result = _gemDriver.SetUint4Item(pObjectId, ui4Item.GetValue());

        //                        if (sb.Length > 0)
        //                            sb.Append(',');
        //                        sb.Append(ui4Item.GetValue());
        //                    }
        //                }
        //                break;
        //            case EN_ITEM_FORMAT.UINT8:
        //                {
        //                    SemiObjectUInt8 ui8Item = obj as SemiObjectUInt8;
        //                    if (ui8Item != null)
        //                    {
        //                        result = _gemDriver.SetUint8Item(pObjectId, ui8Item.GetValue());

        //                        if (sb.Length > 0)
        //                            sb.Append(',');
        //                        sb.Append(ui8Item.GetValue());
        //                    }
        //                }
        //                break;
        //        }
        //        #endregion

        //    }
        //    //nResult = _gemDriver.SetListItem(pObjId, messages.Length);
        //    //if (nResult != 0) return nResult;

        //    //string strArrToSend = ""; // 2022.03.24 by Thienvv [ADD]
        //    //for (int i = 0; i < messages.Length; i++)
        //    //{
        //    //    nResult = _gemDriver.SetAsciiItem(pObjId, messages[i]);
        //    //    strArrToSend += messages[i] + "; "; // // 2022.03.24 by Thienvv [ADD]
        //    //    if (nResult != 0) return nResult;
        //    //}

        //    string sLog = $"Set Vid Data > Index : {vid}, Data : {sb.ToString()}";//, vid, messageToSend);
        //    WriteLog(sLog);

        //    result = _gemDriver.GEMSetVariables(pObjectId, vid);
        //    if (result != 0)
        //    {
        //        WriteLog(string.Format("SetVariableEx Failed : {0}", result));
        //    }

        //    result = _gemDriver.CloseObject(pObjectId);
        //    if (result != 0)
        //    {
        //        WriteLog(string.Format("CloseObject Failed : {0}", result));
        //    }
        //}

        public override void UpdateVariables(long[] arrVids, string[] arrValues)
        {
            _gemDriver.GEMSetVariable(arrVids.Length, arrVids, arrValues);
        }

        public override void UpdateECV(long[] arrECV, string[] arrValues)
        {
            int nCount = arrECV.Length;
            string strLog = string.Empty;
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

                //strLog = string.Format("{0}[EQ ==> XGEM] GEMSetECVChanged => Ecid:{1}, Val:{2}\n",
                //        strLog, arrECV[i], arrValues[i]);
            }

            //WriteLog(strLog);
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

            if (0 == _gemDriver.GEMGetVariable(arrVids.Length, ref arrVids, ref arrValues))
                return true;

            return false;
        }
        #endregion </Get Value (VID)>

        #region <Executing>
        public override void Execute()
        {
            base.Execute();

            // 1) 현재 in-flight가 없을 때만 새 항목을 Dequeue
            if (_currentEventInfo == null)
            {
                EventInfo next;
                bool dequeued = false;
                lock (_eventLock)
                {
                    if (_currentEventInfo == null && _queuedEvents.TryDequeue(out next))
                    {
                        _currentEventInfo = next;
                        dequeued = true;
                        InitCurrentEventInfo(false);
                    }
                }

                if (dequeued)
                {
                    InitEventSteps();
                }
                else
                {
                    return;
                }
            }

            // 2) 현재건 스텝 진행
            if (ExecuteEventStep())
            {
                lock (_eventLock)
                {
                    InitCurrentEventInfo();
                    //_currentEventInfo = null;
                    //_earlySecondarySysbyte = -1;
                    //_earlySecondarySeen = false;
                }
            }
        }
        #endregion </Executing>

        #endregion </Abstract Interface>

        #region <Internal Interface>

        #region Initialize


        #region Event Interface Registry
        private void LinkFunctions()
        {
            #region Received Secs/Gem Messages
            _gemDriver.OnSECSMessageReceived += new OnSECSMessageReceived(OnSECSMessageReceived);
            _gemDriver.OnGEMReqRemoteCommand += new OnGEMReqRemoteCommand(OnGEMReqRemoteCommand);
            _gemDriver.OnGEMSecondaryMsgReceived += new OnGEMSecondaryMsgReceived(OnGEMSecondaryMsgReceived);
            _gemDriver.OnGEMReportedEvent2 += new OnGEMReportedEvent2(OnGEMReportedEvent);
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

            #region Recipe
            _gemDriver.OnGEMReqPPList += new OnGEMReqPPList(OnGEMReqPPList);
            _gemDriver.OnGEMReqPPLoadInquire += new OnGEMReqPPLoadInquire(OnGEMReqPPLoadInquire);
            _gemDriver.OnGEMRspPPLoadInquire += new OnGEMRspPPLoadInquire(OnGEMRspPPLoadInquire);
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

            #region ECV
            _gemDriver.OnGEMReqChangeECV += new OnGEMReqChangeECV(OnGEMReqChangeECV);
            _gemDriver.OnGEMECVChanged += new OnGEMECVChanged(OnGEMECVChanged);
            _gemDriver.OnGEMRspAllECInfo += new OnGEMRspAllECInfo(OnGEMRspAllECInfo);
            #endregion

            #region Terminal
            _gemDriver.OnGEMTerminalMessage += new OnGEMTerminalMessage(OnGEMTerminalMessage);
            _gemDriver.OnGEMTerminalMultiMessage += new OnGEMTerminalMultiMessage(OnGEMTerminalMultiMessage);
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
                WriteLog(string.Format("[EQ ==> XGEM] Fail to Sending Message(MakeObject) S{0}F{1} ({2})",
                    stream,
                    function,
                    result));
            }

            string logToWrite = string.Empty;

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
                                result = _gemDriver.SetBinaryItem(pObjectId, bItem.GetValues());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.BOOL:
                        {
                            SemiObjectBool boItem = obj as SemiObjectBool;
                            if (boItem != null)
                            {
                                result = _gemDriver.SetBoolItem(pObjectId, boItem.GetValues());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FLOAT4:
                        {
                            SemiObjectFloat4 f4Item = obj as SemiObjectFloat4;
                            if (f4Item != null)
                            {
                                result = _gemDriver.SetFloat4Item(pObjectId, f4Item.GetValues());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.FLOAT8:
                        {
                            SemiObjectFloat8 f8Item = obj as SemiObjectFloat8;
                            if (f8Item != null)
                            {
                                result = _gemDriver.SetFloat8Item(pObjectId, f8Item.GetValues());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT:
                        {
                            SemiObjectInt i1Item = obj as SemiObjectInt;
                            if (i1Item != null)
                            {
                                result = _gemDriver.SetInt1Item(pObjectId, i1Item.GetValues());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT2:
                        {
                            SemiObjectInt2 i2Item = obj as SemiObjectInt2;
                            if (i2Item != null)
                            {
                                result = _gemDriver.SetInt2Item(pObjectId, i2Item.GetValues());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT4:
                        {
                            SemiObjectInt4 i4Item = obj as SemiObjectInt4;
                            if (i4Item != null)
                            {
                                result = _gemDriver.SetInt4Item(pObjectId, i4Item.GetValues());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.INT8:
                        {
                            SemiObjectInt8 i8Item = obj as SemiObjectInt8;
                            if (i8Item != null)
                            {
                                result = _gemDriver.SetInt8Item(pObjectId, i8Item.GetValues());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT:
                        {
                            SemiObjectUInt ui1Item = obj as SemiObjectUInt;
                            if (ui1Item != null)
                            {
                                result = _gemDriver.SetUint1Item(pObjectId, ui1Item.GetValues());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT2:
                        {
                            SemiObjectUInt2 ui2Item = obj as SemiObjectUInt2;
                            if (ui2Item != null)
                            {
                                result = _gemDriver.SetUint2Item(pObjectId, ui2Item.GetValues());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT4:
                        {
                            SemiObjectUInt4 ui4Item = obj as SemiObjectUInt4;
                            if (ui4Item != null)
                            {
                                result = _gemDriver.SetUint4Item(pObjectId, ui4Item.GetValues());
                            }
                        }
                        break;
                    case EN_ITEM_FORMAT.UINT8:
                        {
                            SemiObjectUInt8 ui8Item = obj as SemiObjectUInt8;
                            if (ui8Item != null)
                            {
                                result = _gemDriver.SetUint8Item(pObjectId, ui8Item.GetValues());
                            }
                        }
                        break;
                }
                #endregion

                if (result != 0)
                {
                    WriteLog(string.Format("[EQ ==> XGEM] Fail to reply({0}) S{1}F{2} ({3})",
                        errorFormat.ToString(),
                        stream,
                        function,
                        result));

                    return false;
                }

                logToWrite = string.Format("{0} [{1}:{2}]", logToWrite, obj.Format.ToString(), obj.GetValueStringAll());
            }

            result = _gemDriver.SendSECSMessage(pObjectId, stream, function, pSystemByte);
            if (result != 0)
            {
                WriteLog(string.Format("[EQ ==> XGEM] Fail to Sending Message S{0}F{1} ({2})",
                    stream,
                    function,
                    result));

                return false;
            }

            WriteLog($"[EQ ==> XGEM] Success to sending message ObjectID({pObjectId}), S{stream}F{function} : {logToWrite}, SystemByte : 0x{pSystemByte})");

            return true;
        }
        #endregion </Make User Define Message>

        #endregion </Internal Interface>

        #region <Event Interface>

        #region Receive Message

        #region <User Defined Secs Message>
        private void OnSECSMessageReceived(long nObjectID, long stream, long function, long nSysbyte)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sValue = 
                $"[XGEM ==> EQ] OnSECSMessageReceived : ObjectID({nObjectID}), S{stream},F{function}, SystemByte : 0x{nSysbyte.ToString("X")})";

            //sValue = string.Format(
            //            "[XGEM ==> EQ] OnSECSMessageReceived : ObjectID({0}), S{1},F{2}, Sysbyte({3})",
            //                nObjectID, stream, function, nSysbyte);

            // S2F37에서 Event List가 0이면 모든 Event를 세팅하는데,
            // 이 때 Alarm Event도 같이 Enable 됨
            // Alarm Event를 사용하지 않는 경우 아래와 같이 UserDefinedSecsMessag로 구현해야한다.
            if (DataProcessor.UseUserDefinedCollectionEventControl && stream == 2 && function == 37)
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
                        ceids.Add(DataProcessor.AlarmSetEvent);
                        ceids.Add(DataProcessor.AlarmClearEvent);
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
            else if (DataProcessor.UseUserDefinedFormattedRecipeControl && stream == 7 && (function == 23 || function == 24 || function == 25 || function == 26))
            {
                // Upload
                if (function == 25)        // 서버에서 Upload 요청
                {
                    // ppid 파싱
                    string ppid = string.Empty;
                    _gemDriver.GetStringItem(nObjectID, ref ppid);
                    _gemDriver.CloseObject(nObjectID);

                    string sLog = string.Format("[XGEM ==> EQ] OnGEMReqPPFmt(UserDefined) : Ppid({0})", ppid);
                    WriteLog(sLog);

                    // Ack
                    System.Threading.Tasks.Task.Run(() => UploadingFormattedRecipeUsingUserDefinedMessageAsync(ppid, nSysbyte));
                }

                // Download
            }
            WriteLog(sValue);

            UserDefinedSecsMessage structure = new UserDefinedSecsMessage(stream, function);
            ParseUserDefinedSecsMessage(ref structure, ref nObjectID);

            UserDefinedSecsMessage structureToSend = new UserDefinedSecsMessage(stream, function + 1);
            if (CallbackSecsMessageReceived(structure, ref structureToSend))
            {
                bool needAck = (function % 2 != 0);
                if (structureToSend != null && needAck)
                {
                    var messageToSend = structureToSend.ListItemFormat;
                    MakeAndSendSecsMessage(structureToSend.Stream, structureToSend.Function, ref nObjectID, ref nSysbyte,
                        ref messageToSend);
                }
            }
        }

        private void ParseUserDefinedSecsMessage(ref UserDefinedSecsMessage message, ref long pObjectId)
        {
            string allItems = string.Empty;

            _gemDriver.GetAllStringItem(pObjectId, ref allItems);
            _gemDriver.CloseObject(pObjectId);
            WriteLog(allItems);

            string[] receiveItems = allItems.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            List<string> normalizedItems = new List<string>();
            StringBuilder asciiBuffer = null;

            for (int i = 0; i < receiveItems.Length; ++i)
            {
                string targetItem = receiveItems[i];

                if (targetItem == null)
                    continue;

                targetItem = targetItem.Replace("{", "");
                targetItem = targetItem.Replace("}", "");

                if (string.IsNullOrWhiteSpace(targetItem))
                {
                    continue;
                }

                string trimmedItem = targetItem.Trim();

                if (asciiBuffer != null)
                {
                    if (IsNewItemStart(trimmedItem))
                    {
                        normalizedItems.Add(asciiBuffer.ToString());
                        asciiBuffer = null;
                    }
                    else
                    {
                        asciiBuffer.Append("\n");
                        asciiBuffer.Append(targetItem);
                        continue;
                    }
                }

                if (trimmedItem.StartsWith("A:"))
                {
                    asciiBuffer = new StringBuilder();
                    asciiBuffer.Append(targetItem);
                    continue;
                }

                normalizedItems.Add(trimmedItem);
            }

            if (asciiBuffer != null)
            {
                normalizedItems.Add(asciiBuffer.ToString());
            }

            for (int i = 0; i < normalizedItems.Count; ++i)
            {
                SemiObject value = ConvertValue(normalizedItems[i]);

                if (value == null)
                    continue;

                message.ListItemFormat.Add(value);
            }
        }

        private SemiObject ConvertValue(string item)
        {
            SemiObject value = null;

            string[] splitted = item.Split(':');
            List<string> values = new List<string>();

            for (int va = 1; va < splitted.Length; ++va)
            {
                if (splitted[va].Contains(" "))
                {
                    string[] splittedBySpace = splitted[va].Split(' ');
                    for (int j = 0; j < splittedBySpace.Length; ++j)
                    {
                        values.Add(splittedBySpace[j]);
                    }
                }
                else
                {
                    values.Add(splitted[va]);
                }
            }

            switch (splitted[0])
            {
                case "L":
                    {
                        value = new SemiObjectList(0);
                    }
                    break;
                case "A":
                    {
                        string rawAsciiValue = string.Empty;
                        int colonIndex = item.IndexOf(':');

                        if (colonIndex >= 0 && colonIndex + 1 < item.Length)
                        {
                            rawAsciiValue = item.Substring(colonIndex + 1);
                        }

                        rawAsciiValue = rawAsciiValue.Replace("%0D%0A", "\r\n");
                        rawAsciiValue = rawAsciiValue.Replace("%0A", "\n");
                        rawAsciiValue = rawAsciiValue.Replace("%0D", "\r");

                        value = new SemiObjectAscii("", rawAsciiValue);
                    }
                    break;
                case "BIN":
                    {
                        List<byte> convertedValues = new List<byte>();

                        for (int va = 0; va < values.Count; ++va)
                        {
                            int resultInt;
                            // int로 변환 시도
                            bool success = int.TryParse(values[va],
                                System.Globalization.NumberStyles.HexNumber, null, out resultInt);

                            if (success && resultInt >= byte.MinValue && resultInt <= byte.MaxValue)
                            {
                                convertedValues.Add((byte)resultInt);
                            }
                        }

                        value = new SemiObjectBinary("", convertedValues.ToArray());
                    }
                    break;
                case "B":
                    {
                        List<bool> convertedValues = new List<bool>();

                        for (int va = 0; va < values.Count; ++va)
                        {
                            bool convertedValue;
                            if (bool.TryParse(values[va], out convertedValue))
                            {
                                convertedValues.Add(convertedValue);
                            }
                        }

                        value = new SemiObjectBool("", convertedValues.ToArray());
                    }
                    break;
                case "I1":
                    {
                        List<sbyte> convertedValues = new List<sbyte>();

                        for (int va = 0; va < values.Count; ++va)
                        {
                            sbyte convertedValue;
                            if (sbyte.TryParse(values[va], out convertedValue))
                            {
                                convertedValues.Add(convertedValue);
                            }
                        }

                        value = new SemiObjectInt("", convertedValues.ToArray());
                    }
                    break;
                case "I2":
                    {
                        List<short> convertedValues = new List<short>();

                        for (int va = 0; va < values.Count; ++va)
                        {
                            short convertedValue;
                            if (short.TryParse(values[va], out convertedValue))
                            {
                                convertedValues.Add(convertedValue);
                            }
                        }

                        value = new SemiObjectInt2("", convertedValues.ToArray());
                    }
                    break;
                case "I4":
                    {
                        List<int> convertedValues = new List<int>();

                        for (int va = 0; va < values.Count; ++va)
                        {
                            int convertedValue;
                            if (int.TryParse(values[va], out convertedValue))
                            {
                                convertedValues.Add(convertedValue);
                            }
                        }
                        value = new SemiObjectInt4("", convertedValues.ToArray());
                    }
                    break;
                case "I8":
                    {
                        List<long> convertedValues = new List<long>();

                        for (int va = 0; va < values.Count; ++va)
                        {
                            long convertedValue;
                            if (long.TryParse(values[va], out convertedValue))
                            {
                                convertedValues.Add(convertedValue);
                            }
                        }

                        value = new SemiObjectInt8("", convertedValues.ToArray());
                    }
                    break;
                case "U1":
                    {
                        List<byte> convertedValues = new List<byte>();

                        for (int va = 0; va < values.Count; ++va)
                        {
                            byte convertedValue;
                            if (byte.TryParse(values[va], out convertedValue))
                            {
                                convertedValues.Add(convertedValue);
                            }
                        }
                        value = new SemiObjectUInt("", convertedValues.ToArray());
                    }
                    break;
                case "U2":
                    {
                        List<ushort> convertedValues = new List<ushort>();
                        for (int va = 0; va < values.Count; ++va)
                        {
                            ushort convertedValue;
                            if (ushort.TryParse(values[va], out convertedValue))
                            {
                                convertedValues.Add(convertedValue);
                            }
                        }
                        value = new SemiObjectUInt2("", convertedValues.ToArray());
                    }
                    break;
                case "U4":
                    {
                        List<uint> convertedValues = new List<uint>();
                        for (int va = 0; va < values.Count; ++va)
                        {
                            uint convertedValue;
                            if (uint.TryParse(values[va], out convertedValue))
                            {
                                convertedValues.Add(convertedValue);
                            }
                        }
                        value = new SemiObjectUInt4("", convertedValues.ToArray());
                    }
                    break;
                case "U8":
                    {
                        List<ulong> convertedValues = new List<ulong>();
                        for (int va = 0; va < values.Count; ++va)
                        {
                            ulong convertedValue;
                            if (ulong.TryParse(values[va], out convertedValue))
                            {
                                convertedValues.Add(convertedValue);
                            }
                        }
                        value = new SemiObjectUInt8("", convertedValues.ToArray());
                    }
                    break;
                case "F4":
                    {
                        List<float> convertedValues = new List<float>();
                        for (int va = 0; va < values.Count; ++va)
                        {
                            float convertedValue;
                            if (float.TryParse(values[va], out convertedValue))
                            {
                                convertedValues.Add(convertedValue);
                            }
                        }

                        value = new SemiObjectFloat4("", convertedValues.ToArray());
                    }
                    break;
                case "F8":
                    {
                        List<double> convertedValues = new List<double>();
                        for (int va = 0; va < values.Count; ++va)
                        {
                            double convertedValue;
                            if (double.TryParse(values[va], out convertedValue))
                            {
                                convertedValues.Add(convertedValue);
                            }
                        }
                        value = new SemiObjectFloat8("", convertedValues.ToArray());
                    }
                    break;
                default:
                    break;
            }

            return value;
        }
        private bool IsNewItemStart(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                return false;

            return item.Equals("L")
                || item.StartsWith("L:")
                || item.StartsWith("A:")
                || item.StartsWith("BIN:")
                || item.StartsWith("B:")
                || item.StartsWith("I1:")
                || item.StartsWith("I2:")
                || item.StartsWith("I4:")
                || item.StartsWith("I8:")
                || item.StartsWith("U1:")
                || item.StartsWith("U2:")
                || item.StartsWith("U4:")
                || item.StartsWith("U8:")
                || item.StartsWith("F4:")
                || item.StartsWith("F8:");
        }
        //private void ParseUserDefinedSecsMessage(ref UserDefinedSecsMessage message, ref long pObjectId)
        //{
        //    string allItems = string.Empty;
        //    _gemDriver.GetAllStringItem(pObjectId, ref allItems);
        //    _gemDriver.CloseObject(pObjectId);

        //    WriteLog(allItems);

        //    string[] receiveItems = allItems.Split('\n');
        //    int count = receiveItems.Length;
        //    string targetItem = string.Empty;

        //    for (int i = 0; i < count; ++i)
        //    {
        //        targetItem = receiveItems[i];
        //        targetItem = targetItem.Replace("{", "");
        //        targetItem = targetItem.Replace("}", "");

        //        if (targetItem.Equals(""))
        //            continue;

        //        if (targetItem.StartsWith("L")) targetItem += "0";

        //        string[] splitted = targetItem.Split(':');
        //        List<string> values = new List<string>();
        //        for (int va = 1; va < splitted.Length; ++va)
        //        {
        //            if (splitted[va].Contains(" "))
        //            {
        //                string[] splittedBySpace = splitted[va].Split(' ');
        //                for (int j = 0; j < splittedBySpace.Length; ++j)
        //                {
        //                    values.Add(splittedBySpace[j]);
        //                }
        //            }
        //            else
        //            {
        //                values.Add(splitted[va]);
        //            }
        //        }

        //        switch (splitted[0])
        //        {
        //            case "L":
        //                {
        //                    message.ListItemFormat.Add(new SemiObjectList(0));
        //                }
        //                break;
        //            case "A":
        //                {
        //                    message.ListItemFormat.Add(new SemiObjectAscii("", values[0]));
        //                }
        //                break;
        //            case "BIN":
        //                {
        //                    List<byte> convertedValues = new List<byte>();
        //                    for (int va = 0; va < values.Count; ++va)
        //                    {
        //                        // int로 변환 시도
        //                        int resultInt;
        //                        bool success = int.TryParse(values[va],
        //                            System.Globalization.NumberStyles.HexNumber, null, out resultInt);

        //                        if (success && resultInt >= byte.MinValue && resultInt <= byte.MaxValue)
        //                        {
        //                            convertedValues.Add((byte)resultInt);
        //                        }
        //                    }

        //                    if (convertedValues.Count > 0)
        //                    {
        //                        message.ListItemFormat.Add(new SemiObjectBinary("", convertedValues.ToArray()));
        //                    }
        //                }
        //                break;
        //            case "B":
        //                {
        //                    List<bool> convertedValues = new List<bool>();
        //                    for (int va = 0; va < values.Count; ++va)
        //                    {
        //                        bool convertedValue;
        //                        if (bool.TryParse(values[va], out convertedValue))
        //                        {
        //                            convertedValues.Add(convertedValue);
        //                        }
        //                    }

        //                    if (convertedValues.Count > 0)
        //                    {
        //                        message.ListItemFormat.Add(new SemiObjectBool("", convertedValues.ToArray()));
        //                    }
        //                }
        //                break;
        //            case "I1":
        //                {
        //                    List<sbyte> convertedValues = new List<sbyte>();
        //                    for (int va = 0; va < values.Count; ++va)
        //                    {
        //                        sbyte convertedValue;
        //                        if (sbyte.TryParse(values[va], out convertedValue))
        //                        {
        //                            convertedValues.Add(convertedValue);
        //                        }
        //                    }

        //                    if (convertedValues.Count > 0)
        //                    {
        //                        message.ListItemFormat.Add(new SemiObjectInt("", convertedValues.ToArray()));
        //                    }
        //                }
        //                break;
        //            case "I2":
        //                {
        //                    List<short> convertedValues = new List<short>();
        //                    for (int va = 0; va < values.Count; ++va)
        //                    {
        //                        short convertedValue;
        //                        if (short.TryParse(values[va], out convertedValue))
        //                        {
        //                            convertedValues.Add(convertedValue);
        //                        }
        //                    }

        //                    if (convertedValues.Count > 0)
        //                    {
        //                        message.ListItemFormat.Add(new SemiObjectInt2("", convertedValues.ToArray()));
        //                    }
        //                }
        //                break;
        //            case "I4":
        //                {
        //                    List<int> convertedValues = new List<int>();
        //                    for (int va = 0; va < values.Count; ++va)
        //                    {
        //                        int convertedValue;
        //                        if (int.TryParse(values[va], out convertedValue))
        //                        {
        //                            convertedValues.Add(convertedValue);
        //                        }
        //                    }
        //                    if (convertedValues.Count > 0)
        //                    {
        //                        message.ListItemFormat.Add(new SemiObjectInt4("", convertedValues.ToArray()));
        //                    }
        //                }
        //                break;
        //            case "I8":
        //                {
        //                    List<long> convertedValues = new List<long>();
        //                    for (int va = 0; va < values.Count; ++va)
        //                    {
        //                        long convertedValue;
        //                        if (long.TryParse(values[va], out convertedValue))
        //                        {
        //                            convertedValues.Add(convertedValue);
        //                        }
        //                    }
        //                    if (convertedValues.Count > 0)
        //                    {
        //                        message.ListItemFormat.Add(new SemiObjectInt8("", convertedValues.ToArray()));
        //                    }
        //                }
        //                break;
        //            case "U1":
        //                {
        //                    List<byte> convertedValues = new List<byte>();
        //                    for (int va = 0; va < values.Count; ++va)
        //                    {
        //                        byte convertedValue;
        //                        if (byte.TryParse(values[va], out convertedValue))
        //                        {
        //                            convertedValues.Add(convertedValue);
        //                        }
        //                    }
        //                    if (convertedValues.Count > 0)
        //                    {
        //                        message.ListItemFormat.Add(new SemiObjectUInt("", convertedValues.ToArray()));
        //                    }
        //                }
        //                break;
        //            case "U2":
        //                {
        //                    List<ushort> convertedValues = new List<ushort>();
        //                    for (int va = 0; va < values.Count; ++va)
        //                    {
        //                        ushort convertedValue;
        //                        if (ushort.TryParse(values[va], out convertedValue))
        //                        {
        //                            convertedValues.Add(convertedValue);
        //                        }
        //                    }
        //                    if (convertedValues.Count > 0)
        //                    {
        //                        message.ListItemFormat.Add(new SemiObjectUInt2("", convertedValues.ToArray()));
        //                    }
        //                }
        //                break;
        //            case "U4":
        //                {
        //                    List<uint> convertedValues = new List<uint>();
        //                    for (int va = 0; va < values.Count; ++va)
        //                    {
        //                        uint convertedValue;
        //                        if (uint.TryParse(values[va], out convertedValue))
        //                        {
        //                            convertedValues.Add(convertedValue);
        //                        }
        //                    }
        //                    if (convertedValues.Count > 0)
        //                    {
        //                        message.ListItemFormat.Add(new SemiObjectUInt4("", convertedValues.ToArray()));
        //                    }
        //                }
        //                break;
        //            case "U8":
        //                {
        //                    List<ulong> convertedValues = new List<ulong>();
        //                    for (int va = 0; va < values.Count; ++va)
        //                    {
        //                        ulong convertedValue;
        //                        if (ulong.TryParse(values[va], out convertedValue))
        //                        {
        //                            convertedValues.Add(convertedValue);
        //                        }
        //                    }
        //                    if (convertedValues.Count > 0)
        //                    {
        //                        message.ListItemFormat.Add(new SemiObjectUInt8("", convertedValues.ToArray()));
        //                    }
        //                }
        //                break;
        //            case "F4":
        //                {
        //                    List<float> convertedValues = new List<float>();
        //                    for (int va = 0; va < values.Count; ++va)
        //                    {
        //                        float convertedValue;
        //                        if (float.TryParse(values[va], out convertedValue))
        //                        {
        //                            convertedValues.Add(convertedValue);
        //                        }
        //                    }
        //                    if (convertedValues.Count > 0)
        //                    {
        //                        message.ListItemFormat.Add(new SemiObjectFloat4("", convertedValues.ToArray()));
        //                    }
        //                }
        //                break;
        //            case "F8":
        //                {
        //                    List<double> convertedValues = new List<double>();
        //                    for (int va = 0; va < values.Count; ++va)
        //                    {
        //                        double convertedValue;
        //                        if (double.TryParse(values[va], out convertedValue))
        //                        {
        //                            convertedValues.Add(convertedValue);
        //                        }
        //                    }
        //                    if (convertedValues.Count > 0)
        //                    {
        //                        message.ListItemFormat.Add(new SemiObjectFloat8("", convertedValues.ToArray()));
        //                    }
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //        //XValue.LoadFromString(targetItem);

        //        //switch (xValue.GetItemType())
        //        //{
        //        //    case XItemType.XITEM_NONE:
        //        //        message.ListItemFormat.Add(new SemiObjectList(0));
        //        //        break;
        //        //    case XItemType.XITEM_ASCII:
        //        //message.ListItemFormat.Add(new SemiObjectAscii("", xValue.GetString()));
        //        //        break;
        //        //    case XItemType.XITEM_BINARY:
        //        //        message.ListItemFormat.Add(new SemiObjectBinary("", xValue.GetBin()));
        //        //        break;
        //        //    case XItemType.XITEM_BOOL:
        //        //        message.ListItemFormat.Add(new SemiObjectBool("", xValue.GetBool()));
        //        //        break;
        //        //    case XItemType.XITEM_F4:
        //        //        message.ListItemFormat.Add(new SemiObjectFloat4("", xValue.GetF4()));
        //        //        break;
        //        //    case XItemType.XITEM_F8:
        //        //        message.ListItemFormat.Add(new SemiObjectFloat8("", xValue.GetF8()));
        //        //        break;
        //        //    case XItemType.XITEM_I1:
        //        //        message.ListItemFormat.Add(new SemiObjectInt("", xValue.GetI1()));
        //        //        break;
        //        //    case XItemType.XITEM_I2:
        //        //        message.ListItemFormat.Add(new SemiObjectInt2("", xValue.GetI2()));
        //        //        break;
        //        //    case XItemType.XITEM_I4:
        //        //        message.ListItemFormat.Add(new SemiObjectInt4("", xValue.GetI4()));
        //        //        break;
        //        //    case XItemType.XITEM_I8:
        //        //        message.ListItemFormat.Add(new SemiObjectInt8("", xValue.GetI8()));
        //        //        break;
        //        //    case XItemType.XITEM_U1:
        //        //        message.ListItemFormat.Add(new SemiObjectUInt("", xValue.GetU1()));
        //        //        break;
        //        //    case XItemType.XITEM_U2:
        //        //        message.ListItemFormat.Add(new SemiObjectUInt2("", xValue.GetU2()));
        //        //        break;
        //        //    case XItemType.XITEM_U4:
        //        //        message.ListItemFormat.Add(new SemiObjectUInt4("", xValue.GetU4()));
        //        //        break;
        //        //    case XItemType.XITEM_U8:
        //        //        message.ListItemFormat.Add(new SemiObjectUInt8("", xValue.GetU8()));
        //        //        break;
        //        //}
        //    }
        //}
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
                //ShowTerminalMessage(string.Format("RCMD Success! : {0}", strRcmd));
            }
            else
            {
                _gemDriver.GEMRspRemoteCommand(nMsgId, sRcmd, 1, arrNames.Length, arrNames, arrResult);
                //ShowTerminalMessage(string.Format("RCMD Failed! : {0}", strRcmd));
            }

            //BroadcastSignal(sRcmd, )
            if (arrResult.Length > 0)
            {
                string sLog = string.Format("[EQ ==> XGEM] GEMRspRemoteCommand, {0} ", arrResult[0]);
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

        #region <Collection Events>
        private void InitCurrentEventInfo(bool includeEventInfo = true)
        {
            if (includeEventInfo)
            {
                _currentEventInfo = null; // 타임아웃 시 깔끔히 해제
            }
            
            _earlySecondarySysbytes.Clear();
        }
        private void InitEventSteps()
        {
            _stepForSendingEvent = 0;
            //_timeCheckerEvent.SetTickCount(TimeoutCheckEvents);
        }
        private bool ExecuteEventStep()
        {
            switch (_stepForSendingEvent)
            {
                case 0:
                    {
                        //EventInfo cur;
                        //lock (_eventLock) 
                        //{
                        //    cur = _currentEventInfo; 
                        //}
                        //if (cur == null)
                        //    return true;

                        //_gemDriver.GEMSetVariable(
                        //    cur.GetVariableCount(),
                        //    cur.VariableIds,
                        //    cur.VariableValues);

                        _delayForEvent.SetTickCount(DelayTimeForUpdateVariables);
                        _stepForSendingEvent = 1;
                        break;
                    }

                case 1:
                    {
                        // Variable Update Delay : 200 ms 대기
                        if (false == _delayForEvent.IsTickOver(true)) 
                            break;

                        EventInfo cur;
                        lock (_eventLock)
                        {
                            cur = _currentEventInfo;
                        }
                        if (cur == null)
                            return true;

                        _gemDriver.GEMSetEventEx(cur.EventId,
                            cur.GetVariableCount(),
                            cur.VariableIds,
                            cur.VariableValues);

                        // 아래 구문으로 인해 체크하지 않는데 실제로는 S6F12가 오는데 끝나버려서 경고성로그가 계속 남게 된다.
                        // 따라서 지워버리고 Ack를 체크하지 않는 옵션은 별도의 기능을 추가해야할 듯 하다.
                        // ACK를 체크하지 않는 이벤트는
                        // 실제 전송(GEMSetEventEx) 시점을 완료로 본다.
                        //if (false == cur.UseCheckSecondaryAck)
                        //{
                        //    _eventToSend[cur.EventId] = true;
                        //    _stepForSendingEvent = 0;
                            
                        //    return true;
                        //}

                        _timeCheckerEvent.SetTickCount(TimeoutCheckEvents);
                        _stepForSendingEvent = 2; // SetEvent 후 콜백 대기
                        break;
                    }

                case 2:
                    {
                        // 최대 1초간 타입아웃 후 강제 완료 처리(실제 전송 이후 타임아웃 체크)
                        if (_timeCheckerEvent.IsTickOver(false))
                        {
                            long ceid = -1;
                            lock (_eventLock)
                            {
                                if (_currentEventInfo != null)
                                    ceid = _currentEventInfo.EventId;

                            }

                            // timeout이어도 외부에서는 done=true로 본다.
                            if (ceid > 0)
                            {
                                _eventToSend[ceid] = true;
                            }

                            WriteLog($"[Timeout] Event wait exceeded (CEID:{ceid})");
                            _stepForSendingEvent = 0;
                            return true;
                        }

                        // 완료 조건: 콜백에서 _currentEventInfo가 null로 해제됨
                        bool done;
                        lock (_eventLock)
                        {
                            done = (_currentEventInfo == null);
                        }

                        if (done)
                        {
                            _stepForSendingEvent = 0;
                            return true;
                        }

                        //if (_currentEventInfo == null)
                        //{
                        //    _stepForSendingEvent = 0;
                        //    return true; // 하나 완료
                        //}
                        break;
                    }
            }

            return false;
        }
        private void OnGEMSecondaryMsgReceived(long nS, long nF, long nSysbyte, string sParam1, string sParam2, string sParam3)
        {
            long eventId = -1;
            bool matched = false;
            bool buffered = false;
            bool noInflight = false;
            bool useCheckSecondaryAck = false;

            lock (_eventLock)
            {
                var cur = _currentEventInfo;
                if (cur == null)
                {
                    noInflight = true; // (원샷 완료 후 지각 ACK일 수 있음)
                }
                else if (cur.SystemByte < 0)
                {
                    // Reported 전에 Secondary가 먼저 왔으면 sysbyte들을 임시 보관
                    _earlySecondarySysbytes.Add(nSysbyte);
                    buffered = true;
                }
                else if (cur.SystemByte == nSysbyte)
                {
                    // 정상 매칭
                    eventId = cur.EventId;
                    useCheckSecondaryAck = cur.UseCheckSecondaryAck;
                    matched = true;
                    InitCurrentEventInfo();
                }
            }

            if (matched)
            {
                _eventToSend[eventId] = true;

                if (useCheckSecondaryAck)
                {
                    WriteLog($"[XGEM ==> EQ] S6F12 : {eventId}, SystemByte : 0x{nSysbyte.ToString("X")}");
                }
            }
            else if (buffered)
            {
                WriteLog($"[Info] Early S6F12 buffered (SystemByte : 0x{nSysbyte.ToString("X")}).");
            }
            else if (noInflight)
            {
                WriteLog("[Info] S6F12 with no inflight (likely late ACK of one-shot).");
            }
            else
            {
                WriteLog($"[Info] S6F12 received (SystemByte : 0x{nSysbyte.ToString("X")}), waiting for Reported.");
            }
        }

        //private void OnGEMSecondaryMsgReceived(long nS, long nF, long nSysbyte, string sParam1, string sParam2, string sParam3)
        //{
        //    foreach (var pair in _eventToSend)
        //    {
        //        if (_eventToSend[pair.Key] == false)
        //        {
        //            _eventToSend[pair.Key] = true;
        //            string sLog = string.Format("[XGEM ==> EQ] S6F12 : {0}, Ack : {1}", pair.Key, sParam1);
        //            WriteLog(sLog);
        //            break;
        //        }
        //    }
        //}

        // SetEvent로 보낸 뒤 콜백이 들어온다.
        private void OnGEMReportedEvent(long eventId, long systemByte)
        {
            EventInfo cur;
            lock (_eventLock) { cur = _currentEventInfo; }

            if (cur == null) { WriteLog("[Warn] ReportedEvent with no inflight."); return; }
            if (cur.EventId != eventId)
            {
                WriteLog($"[XGEM ==> EQ] [Warn] ReportedEvent mismatched. inflight:{cur.EventId}, reported:{eventId}");
                return;
            }

            long[] ids = new long[] { eventId };
            long[] enabled = new long[] { 0 };
            var result = _gemDriver.GEMGetEventEnable(1, ids, ref enabled);
            if (enabled.Length < 1 || enabled[0] == 0 || result < 0)
            {
                _eventToSend[eventId] = true;
                WriteLog($"[Warn] Event not enabled : {eventId}");
                lock (_eventLock)
                {
                    InitCurrentEventInfo(true);
                }
                return;
            }

            bool earlyMatchedNow = false;

            lock (_eventLock)
            {
                // Sysbyte 기록
                if (_currentEventInfo != null && _currentEventInfo == cur)
                    _currentEventInfo.SystemByte = systemByte;

                // Reported가 왔을 때, 이전에 받아둔 Secondary 중 현재 sysbyte가 있으면 매칭
                if (_earlySecondarySysbytes.Contains(systemByte))
                {
                    InitCurrentEventInfo();
                    earlyMatchedNow = true;
                }
            }

            if (cur.UseCheckSecondaryAck)
            {
                WriteS6F11Log(eventId, cur.VariableIds, cur.VariableValues, systemByte);
            }

            if (earlyMatchedNow)
            {
                _eventToSend[eventId] = true;
                WriteLog($"[XGEM ==> EQ] S6F12(Early) matched on Reported: {eventId}, SystemByte : 0x{systemByte.ToString("X")}");
            }
        }
        #endregion </Collection Events>

        #endregion

        #region <Time>
        private void OnGEMReqGetDateTime(long nMsgId)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = string.Format("[XGEM ==> EQ] OnGEMReqGetDateTime");
            WriteLog(sLog);

            string sSystemTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            _gemDriver.GEMRspGetDateTime(nMsgId, sSystemTime);
            sLog = string.Format("[EQ ==> XGEM] GEMRspGetDateTime:{0}", sSystemTime);
            WriteLog(sLog);
        }

        private void OnGEMRspGetDateTime(string sSystemTime)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = string.Format("[XGEM ==> EQ] OnGEMRspGetDateTime : systemtime({0})", sSystemTime);
            WriteLog(sLog);

            //Debug.WriteLine(string.Format("OnGEMRspGetDateTimeExgemctrl1 th={0}\n", Thread.CurrentThread.ManagedThreadId));
        }

        private void OnGEMReqDateTime(long nMsgId, string sSystemTime)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = string.Format("[XGEM ==> EQ] OnGEMReqDateTime : systemtime({0})", sSystemTime);
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
            sLog = string.Format("[EQ ==> XGEM] GEMRspDateTime");
            WriteLog(sLog);
        }
        #endregion </Time>

        #region <Unformatted Recipe>

        #region PPLIST
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
            if (GetRecipeFileList(out recipeFiles))
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
            string sLog = string.Format("[XGEM ==> EQ] OnGEMReqPPLoadInquire : Ppid({0}), ", sPpid);
            WriteLog(sLog);

            EN_PPGRANT grant = CallbackCheckingRecipeControlGrant(sPpid);

            _gemDriver.GEMRspPPLoadInquire(nMsgId, sPpid, (long)grant);
            sLog = string.Format("[EQ ==> XGEM] GEMRspPPLoadInquire : Ppid({0}), ", sPpid);
            WriteLog(sLog);
        }
        #endregion </PP Load Inquire>

        #region <Upload>

        // S7, F1 : [EQ -> Host] 설비에서 레시피 업로드를 요청 전 초기화 작업(이거 발생 후 응답 뒤에 S7, F3 보낼 것이다.)
        public override void ReqUploadingRecipeInquire(string sPpid)
        {
            string recipeFullPath = string.Empty;
            if (CallbackUploadingUnformattedRecipe(sPpid, ref recipeFullPath))
            {
                FileInfo info = new FileInfo(recipeFullPath);
                string fileName = Path.GetFileNameWithoutExtension(recipeFullPath);

                _gemDriver.GEMReqPPLoadInquire(fileName, info.Length);
                string sLog = string.Format("[EQ ==> XGEM] GEMRspPPLoadInquire : Ppid({0}), ", sPpid);
                WriteLog(sLog);
            }
        }

        private void OnGEMRspPPLoadInquire(string sPpid, long nResult)
        {
            //throw new Exception("The method or operation is not implemented.");

            if (nResult == 0)
            {
                //일단 언포멧티드만
                System.Threading.Tasks.Task.Run(() => UploadingUnformattedRecipeAsync(-1, sPpid));
            }
            string sLog = string.Format("[XGEM ==> EQ] OnGEMRspPPLoadInquire : Ppid({0}), Result({1})", sPpid, nResult);
            WriteLog(sLog);
        }

        // S7, F4 : [Host -> EQ] 레시피 업로드에 대한 응답
        private void OnGEMRspPPSend(string sPpid, long nResult)
        {
            //CallbackUploadingUnformattedRecipeAck(sPpid, (EN_ACK7)nResult);

            string sLog = string.Format("[XGEM ==> EQ] OnGEMRspPPSend : Ppid({0}), Result({1})", sPpid, nResult);

            WriteLog(sLog);
        }

        // S7, F4 : [Host -> EQ] 레시피 업로드에 대한 응답
        private void OnGEMRspPPSendEx(string sPpid, string path, long nResult)
        {
            string sLog = string.Format("[XGEM ==> EQ] OnGEMRspPPSendEx : Ppid({0}), Result({1})", sPpid, nResult);

            WriteLog(sLog);
        }

        private bool UploadingUnformattedRecipeAsync(long nMsgId, string recipeId, string path)
        {
            long resultApi = 0;
            string recipeFullPath = string.Empty;
            if (CallbackUploadingUnformattedRecipe(recipeId, ref recipeFullPath))
            {
                bool result = true;
                if (false == File.Exists(recipeFullPath))
                {
                    result = false;
                }

                if (false == string.IsNullOrEmpty(path) && string.IsNullOrEmpty(Path.GetDirectoryName(path)))
                {
                    path += @"\";
                }

                string destPath = string.Format(@"{0}{1}{2}", DataProcessor.RecipeHandlingPath, path, recipeId);
                try
                {
                    File.Move(recipeFullPath, destPath);
                }
                catch (Exception ex)
                {
                    WriteLog(string.Format("[XGEM ==> EQ] UploadingUnformattedRecipeAsync File copy has been failed = {0}, {1}", ex.Message, ex.StackTrace));
                }
                //byte[] recipeBodies = File.ReadAllBytes(recipeFullPath);
                //if (recipeBodies == null)
                //    result = false;

                if (result)
                {
                    if (nMsgId > 0)
                    {
                        resultApi = _gemDriver.GEMRspPPEx(nMsgId, recipeId, path);

                        WriteLog(string.Format("[XGEM ==> EQ] GEMRspPPEx successfully = {0}", resultApi));
                    }
                    else
                    {
                        resultApi = _gemDriver.GEMReqPPSendEx(recipeId, path);

                        WriteLog(string.Format("[XGEM ==> EQ] GEMReqPPSendEx successfully = {0}", resultApi));
                    }
                }
                else
                {
                    if (nMsgId > 0)
                    {
                        resultApi = _gemDriver.GEMRspPPEx(nMsgId, recipeId, path);

                        WriteLog(string.Format("[XGEM ==> EQ] Fail to GEMRspPPEx, {0}", resultApi));
                    }
                }
            }
            else
            {
                if (nMsgId > 0)
                {
                    resultApi = _gemDriver.GEMRspPPEx(nMsgId, recipeId, path);

                    WriteLog(string.Format("[XGEM ==> EQ] Fail to GEMRspPPEx, {0}", resultApi));
                }
            }


            return (resultApi == 0);
        }

        private bool UploadingUnformattedRecipeAsync(long nMsgId, string recipeId)
        {
            long resultApi = 0;
            string recipeFullPath = string.Empty;
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

                        WriteLog(string.Format("[XGEM ==> EQ] OnGEMReqPP successfully = {0}", resultApi));
                    }
                    else
                    {
                        resultApi = _gemDriver.GEMReqPPSend(recipeId, recipeBodies);

                        WriteLog(string.Format("[XGEM ==> EQ] GEMReqPPSend successfully = {0}", resultApi));
                    }
                }
                else
                {
                    if (nMsgId > 0)
                    {
                        resultApi = _gemDriver.GEMRspPP(nMsgId, recipeId, null);

                        WriteLog(string.Format("[XGEM ==> EQ] Fail to OnGEMReqPP, {0}", resultApi));
                    }
                }
            }
            else
            {
                if (nMsgId > 0)
                {
                    resultApi = _gemDriver.GEMRspPP(nMsgId, recipeId, null);

                    WriteLog(string.Format("[XGEM ==> EQ] Fail to OnGEMReqPP, {0}", resultApi));
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

            //        WriteLog(string.Format("[Send] GEMRspPPEx successfully = {0}", nReturn));
            //    }
            //    else
            //    {
            //        nReturn = _gemDriver.GEMRspPPEx(nMsgId, null, null);

            //        WriteLog(string.Format("[Send] Fail to GEMRspPPEx, {0}", nReturn));
            //    }

            //}
            //catch (Exception ex)
            //{
            //    WriteLog(string.Format("Upload Recipe Fail! {0}, {1}", ex.Message, ex.StackTrace));
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

            if (false == string.IsNullOrEmpty(path) && string.IsNullOrEmpty(Path.GetDirectoryName(path)))
            {
                path += @"\";
            }

            string fileFullPath = string.Format(@"{0}{1}{2}", DataProcessor.RecipeHandlingPath, path, recipeId);

            EN_ACK7 resultAck = CallbackDownloadingUnformattedRecipe(recipeId, fileFullPath);

            if (nMsgId > 0)
            {
                result = _gemDriver.GEMRspPPSendEx(nMsgId, recipeId, path, (long)resultAck);

                WriteLog(string.Format("[XGEM ==> EQ] OnGEMReqPP result = {0}", resultAck.ToString()));
            }

            return false;
        }

        private bool DownloadingUnformattedRecipeAsync(long nMsgId, string recipeId, byte[] recipeBodies)
        {
            long result = 0;
            //23.11.13 by wdw [remove] DataProcessor.RecipeHandlingPath에 이미 \포함 되어 있음
            //string fileFullPath = string.Format(@"{0}\{1}", DataProcessor.RecipeHandlingPath, recipeId);
            string fileFullPath = string.Format(@"{0}{1}", DataProcessor.RecipeHandlingPath, recipeId);

            try
            {
                if (false == Directory.Exists(DataProcessor.RecipeHandlingPath))
                    Directory.CreateDirectory(DataProcessor.RecipeHandlingPath);

                if (File.Exists(fileFullPath))
                    File.Delete(fileFullPath);

                File.WriteAllBytes(fileFullPath, recipeBodies);
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("Downloading has Failed : {0} - {1}", ex.Message, ex.StackTrace));
                return false;
            }

            EN_ACK7 resultAck = CallbackDownloadingUnformattedRecipe(recipeId, fileFullPath);

            if (nMsgId > 0)
            {
                result = _gemDriver.GEMRspPPSend(nMsgId, recipeId, (long)resultAck);

                WriteLog(string.Format("[XGEM ==> EQ] OnGEMReqPP result = {0}", resultAck.ToString()));
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
                string sLog = string.Format("[EQ ==> XGEM] GEMRspPPFmtSend");
                WriteLog(sLog);
            }

        }

        // S7, F23 : [Host -> EQ] Formatted Process Program Send -> Downloading Recipe
        private void OnGEMReqPPFmtSend(long nMsgId, string sPpid, string sMdln, string sSoftRev, long nCount, string[] psCCode, long[] pnParamCount, string[] psParamNames)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = string.Format("[XGEM ==> EQ] OnGEMReqPPFmtSend => Ppid:{0}, Mdln:{1}, SoftRev:{2}", sPpid, sMdln, sSoftRev);
            WriteLog(sLog);

            System.Threading.Tasks.Task.Run(()
                => DownloadFormattedRecipeAsync(nMsgId, sPpid, sMdln, sSoftRev, nCount, psCCode, pnParamCount, psParamNames));
        }

        // S7, F24 : [Host -> EQ] Formatted Process Program Acknowledge -> Host로부터 S7F24를 받았을 시 발생
        private void OnGEMRspPPFmtSend(string sPpid, long nResult)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = string.Format("[XGEM ==> EQ] OnGEMRspPPFmtSend : Ppid({0}), Result({1})", sPpid, nResult);
            WriteLog(sLog);
        }

        private long MakeSemiStructure(ref long pObject, SemiObject[] objects)
        {
            long result = 0;
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


            long result = 0;
            string sLog = string.Empty;
            if (systemByte >= 0)        // 서버에서 받은거
            {
                _gemDriver.SendSECSMessage(pObject, 7, 26, systemByte);

                sLog = string.Format("[EQ ==> XGEM] GEMRspPPFmt(UserDefined) Result : {0}", result);
            }
            else
            {
                sLog = string.Format("[EQ ==> XGEM] GEMReqPPFmtSend(UserDefined) Result : {0}", result);
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
                    pparam.Add(kvp.Value[i].GetValueString());
                }
            }

            #region <For Simul>
            //string filePath = string.Format(@"{0}Temp.txt", DataProcessor.RecipeHandlingPath);
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
            //        string line = string.Format("ASCII {0}", byteArray.Length);
            //        for (int byteIndex = 0; byteIndex < byteArray.Length; ++byteIndex)
            //        {
            //            line += string.Format(" {0}", byteArray[byteIndex]);
            //        }

            //        sw.WriteLine(line);
            //    }

            //    sw.WriteLine("---------------");
            //}

            //sw.Close();
            #endregion </For Simul>

            long result;
            string sLog = string.Empty;
            if (msgId >= 0)
            {
                result = _gemDriver.GEMRspPPFmt(msgId, ppid, sMdln, sSoftRev, ppbodies.Count, ppbodies.Keys.ToArray(), paramCount, pparam.ToArray());

                sLog = string.Format("[EQ ==> XGEM] GEMRspPPFmt Result : {0}", result);
            }
            else
            {
                result = _gemDriver.GEMReqPPFmtSend(ppid, sMdln, sSoftRev, ppbodies.Count, ppbodies.Keys.ToArray(), paramCount, pparam.ToArray());

                sLog = string.Format("[EQ ==> XGEM] GEMReqPPFmtSend Result : {0}", result);
            }

            WriteLog(sLog);
        }

        // S7, F25 : [Host -> EQ] Formatted Process Program Request -> Uploading Recipe
        private void OnGEMReqPPFmt(long nMsgId, string sPpid)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = string.Format("[XGEM ==> EQ] OnGEMReqPPFmt : Ppid({0})", sPpid);
            WriteLog(sLog);

            System.Threading.Tasks.Task.Run(() => UploadingFormattedRecipeAsync(nMsgId, sPpid));
        }

        // S7, F26 : [Host -> EQ] Formatted Process Program Data -> Host로부터 S7F26을 받았을 시 발생
        private void OnGEMRspPPFmt(string sPpid, string sMdln, string sSoftRev, long nCount, string[] psCCode, long[] pnParamCount, string[] psParamNames)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = string.Format("[XGEM ==> EQ] OnGEMRspPPFmt : Ppid({0})", sPpid);
            WriteLog(sLog);

            System.Threading.Tasks.Task.Run(()
                => DownloadFormattedRecipeAsync(-1, sPpid, sMdln, sSoftRev, nCount, psCCode, pnParamCount, psParamNames));
        }

        // S7, F28 : [Host -> EQ] Process Program Verification Acknowledge -> Host로부터 S7F28을 받았을 시 발생
        private void OnGEMRspPPFmtVerification(string sPpid, long nResult)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = string.Format("[XGEM ==> EQ] OnGEMRspPPFmtVerification : Result({0})", nResult);
            WriteLog(sLog);
        }
        #endregion </Formatted Recipe>

        #region <PPDELETE>
        // S7, F17 : 호스트에서 레시피를 삭제하라고 요청했다.
        private void OnGEMReqPPDelete(long nMsgId, long nCount, string[] psPpid)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = string.Format("[XGEM ==> EQ] OnGEMReqPPDelete");
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

                sLog = string.Format("[EQ ==> XGEM] GEMRspPPDelete successfully");
            }
            else
            {
                sLog = string.Format("[EQ ==> XGEM] GEMRspPPDelete Failed Result : {0}, Ack : {1}", resultDriver, (long)EN_ACK7.NOT_FOUND);
            }


            WriteLog(sLog);
        }
        #endregion </PPDELETE>

        #region <ECV>
        private void OnGEMReqChangeECV(long nMsgId, long nCount, long[] pnEcids, string[] psVals)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = string.Format("[XGEM ==> EQ] OnGEMReqChangeECV");
            WriteLog(sLog);

            if (DataProcessor.CurrentControlStatus != EN_CONTROL_STATE.REMOTE
                || EquipmentState_.EquipmentState.GetInstance().GetState() != EquipmentState_.EQUIPMENT_STATE.IDLE)
            {
                _gemDriver.GEMRspChangeECV(nMsgId, 2); // busy
                sLog = string.Format("[EQ ==> XGEM] GEMRspChangeECV Fail [Ack : 2]");
                WriteLog(sLog);
                return;
            }

            // 2024.10.11. jhlim [MOD] 형식 변경에 따른 수정
            int countPreDefined = 0;
            var ecValuesToChange = new Dictionary<string, string>();
            for (int i = 0; i < nCount; i++)
            {
                long id = pnEcids[i];

                if (false == EquipmentConstantListById.ContainsKey(id))
                    continue;

                string value = psVals[i];
                string name = EquipmentConstantListById[id];
                ecValuesToChange[name] = value;
                sLog = string.Format("ECID Change Requested > Ecid:{0}, Name:{1}, Value:{2}", id, name, value);
                WriteLog(sLog);
                // EquipmentConstants를 Config에서 읽으니, 아래 범위 검사는 필요없을 듯하다.
                //if (pnEcids[i] >= _paramRange.ECID_START && pnEcids[i] <= _paramRange.ECID_END)
                //{
                //    sLog = string.Format("     Ecid:{0}, Value:{1}", pnEcids[i], psVals[i]);
                //    WriteLog(sLog);
                //}
                if (pnEcids[i] >= _paramRange.PRE_DEFINED_ECID_START && pnEcids[i] <= _paramRange.PRE_DEFINED_ECID_END)
                {
                    ++countPreDefined;
                }
            }
            //bool bExisted = false;
            //for (int i = 0; i < nCount; i++)
            //{
            //    if (pnEcids[i] >= _paramRange.ECID_START && pnEcids[i] <= _paramRange.ECID_END)
            //    {
            //        bExisted = true;
            //        sLog = string.Format("     Ecid:{0}, Value:{1}", pnEcids[i], psVals[i]);
            //        WriteLog(sLog);
            //    }
            //    if (pnEcids[i] >= _paramRange.PRE_DEFINED_ECID_START && pnEcids[i] <= _paramRange.PRE_DEFINED_ECID_END)
            //    {
            //        preDefinedChanged = true;
            //    }
            //}
            //if (bExisted || preDefinedChanged)
            //{
            //    if (bExisted)
            //    {
            //        base.ChangeSystemParameters(ecValuesToChange.Keys.ToArray(), ecValuesToChange.Values.ToArray());
            //    }

            //    _gemDriver.GEMRspChangeECV(nMsgId, 0);
            //    sLog = string.Format("[EQ ==> XGEM] GEMRspChangeECV");
            //    WriteLog(sLog);
            //}
            //else
            //{
            //    //
            //    _gemDriver.GEMRspChangeECV(nMsgId, 1); // one or more constants does not exist
            //    sLog = string.Format("[EQ ==> XGEM] GEMRspChangeECV Fail [Ack : 1]");
            //    WriteLog(sLog);
            //}

            // 서버로부터 요청받은 수량 비교
            int countToRequested = ecValuesToChange.Count + countPreDefined;
            if (countToRequested != pnEcids.Length)
            {
                _gemDriver.GEMRspChangeECV(nMsgId, (long)EAC.CONSTANTS_DOES_NOT_EXIST);
            }
            else
            {
                if (ecValuesToChange.Count > 0 || countPreDefined > 0)
                {
                    if (ecValuesToChange.Count > 0)
                    {
                        base.ChangeSystemParameters(ecValuesToChange.Keys.ToArray(), ecValuesToChange.Values.ToArray());
                    }

                    _gemDriver.GEMRspChangeECV(nMsgId, 0);
                    sLog = string.Format("[EQ ==> XGEM] GEMRspChangeECV");
                    WriteLog(sLog);
                }
                else
                {
                    _gemDriver.GEMRspChangeECV(nMsgId, (long)EAC.CONSTANTS_DOES_NOT_EXIST);
                    sLog = string.Format("[EQ ==> XGEM] GEMRspChangeECV Fail [Ack : 1]");
                    WriteLog(sLog);
                }
            }
            // 2024.10.11. jhlim [END]
        }

        private void OnGEMECVChanged(long nCount, long[] pnEcids, string[] psVals)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = string.Format("[XGEM ==> EQ] OnGEMECVChanged");
            //WriteLog(sLog);

            for (int i = 0; i < nCount; i++)
            {
                if (pnEcids[i] >= _paramRange.ECID_START && pnEcids[i] <= _paramRange.ECID_END)
                {
                    //sLog = string.Format("               Ecid:{0}, Value:{1}", pnEcids[i], psVals[i]);
                    //WriteLog(sLog);
                }
            }
        }

        private void OnGEMRspAllECInfo(long lCount, long[] plVid, string[] psName, string[] psValue, string[] psDefault, string[] psMin, string[] psMax, string[] psUnit)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = string.Format("[XGEM ==> EQ] OnGEMRspAllECInfo");
            WriteLog(sLog);

            for (int i = 0; i < lCount; i++)
            {
                if (plVid[i] >= _paramRange.ECID_START && plVid[i] <= _paramRange.ECID_END)
                {
                    sLog = string.Format(
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
            DataProcessor.SetCommunicationStatus(nState);
        }

        private void SetControlStatus(long nState)
        {
            DataProcessor.SetControlStatus(nState);

            base.ControlState(DataProcessor.CurrentControlStatus.ToString());
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
                SetControlState(DataProcessor.InitControlStatus);
            }
            else if (nState == (int)EN_COMM_STATE.WAIT_CRA)
            {
                temp = true;
                SetControlState(DataProcessor.InitControlStatus);
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
            WriteLog(string.Format("[XGEM ==> EQ] CommStateChanged : {0}", (EN_COMM_STATE)nState));
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

                sLog = string.Format("[EQ ==> XGEM] GEMSetEstablish: {0}", szState);
                WriteLog(sLog);
            }
            else
            {
                sLog = string.Format("[XGEM ==> EQ] OnXGEMStateEvent:{0}", szState);
                WriteLog(sLog);
            }
        }

        private void OnGEMReqOnline(long nMsgId, long nFromState, long nToState)
        {
            string sLog = string.Format("[XGEM ==> EQ] OnGEMReqOnline");
            WriteLog(sLog);

            sLog = string.Format("               nMsgId:{0}, nFromState:{1}, nToState:{2}", nMsgId, nFromState, nToState);
            WriteLog(sLog);
            UpdateVariables();
            _gemDriver.GEMRspOnline(nMsgId, 0);
            sLog = string.Format("[EQ ==> XGEM] GEMRspOnline => nMsgId:{0}, nAck:{1}", nMsgId, 0);
            WriteLog(sLog);
        }

        private void OnGEMReqOffline(long nMsgId, long nFromState, long nToState)
        {
            string sLog = string.Format("[XGEM ==> EQ] OnGEMReqOffline");
            WriteLog(sLog);

            sLog = string.Format("               nMsgId:{0}, nFromState:{1}, nToState:{2}", nMsgId, nFromState, nToState);
            WriteLog(sLog);

            _gemDriver.GEMRspOffline(nMsgId, 0);
            sLog = string.Format("[EQ ==> XGEM] GEMRsqOffline => nMsgId:{0}, nAck:{1}", nMsgId, 0);
            WriteLog(sLog);
        }
        #endregion </Communication>

        #region <Terminal>
        private void OnGEMTerminalMessage(long nTid, string sMsg)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = string.Format("[XGEM ==> EQ] TerminalMessage : Tid({0}), Msg({1})", nTid, sMsg);
            WriteTerminalLog(sLog);

            ShowTerminalMessage(sMsg);
        }

        private void OnGEMTerminalMultiMessage(long nTid, long nCount, string[] psMsg)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sLog = string.Format("[XGEM ==> EQ] OnGEMTerminalMultiMessage : Tid({0}), ", nTid);
            WriteTerminalLog(sLog);
            string strMessage = string.Empty;
            for (int i = 0; i < nCount; i++)
            {
                sLog = string.Format("               B: {0}", psMsg[i]);
                strMessage += psMsg[i];
                if (i != nCount - 1)
                {
                    strMessage += "\r\n";
                }

                WriteTerminalLog(sLog);
            }
            ShowTerminalMessage(strMessage);
        }
        #endregion </Terminal>

        #endregion </Event Interface>

        #endregion </Methods>
    }
}