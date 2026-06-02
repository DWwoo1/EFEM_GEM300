using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

using FrameOfSystem3.SECSGEM.DefineSecsGem;
using Define.DefineConstant;

namespace FrameOfSystem3.SECSGEM
{
    public abstract class SecsGem
    {
        #region <SystemTime>
        [StructLayout(LayoutKind.Sequential)]
        protected struct SYSTEMTIME
        {
            public UInt16 wYear;
            public UInt16 wMonth;
            public UInt16 wDayOfWeek;
            public UInt16 wDay;
            public UInt16 wHour;
            public UInt16 wMinute;
            public UInt16 wSecond;
            public UInt16 wMilliseconds;
        }
        [DllImport("kernel32")]
        protected static extern void GetSystemTime(out SYSTEMTIME systemTime);
        [DllImport("kernel32.dll")]
        protected static extern uint SetSystemTime(ref SYSTEMTIME lpSystemTime);

        [DllImport("kernel32.dll")]
        protected static extern uint SetLocalTime(ref SYSTEMTIME lpSystemTime);
        #endregion <SystemTime>

        #region <Fields>
        protected string _recipePath = String.Empty;

        protected PARAM_RANGE _paramRange = PARAM_RANGE.GetInstance();
        protected CollectionEvents _collectionEvents = CollectionEvents.Instance;

        protected readonly Dictionary<long, List<StatusVariable>> ReportList = new Dictionary<long, List<StatusVariable>>();
        protected readonly Dictionary<string, StatusVariable> StatusVariableList = new Dictionary<string, StatusVariable>();
        protected readonly Dictionary<string, CollectionEvent> CollectionEventList = new Dictionary<string, CollectionEvent>();
        protected readonly Dictionary<string, EquipmentConstant> EquipmentConstantList = new Dictionary<string, EquipmentConstant>();
        protected readonly Dictionary<long, string> EquipmentConstantListById = new Dictionary<long, string>();
        #endregion <Fields>

        #region <Properties>
        // Host와의 연결상태를 반환한다.
        public bool Connect { get; set; }
        #endregion </Properties>

        #region Delegates
        public event deleHandlerVoid CallbackUpdateVariables;
        protected virtual void UpdateVariables()
        {
            if (CallbackUpdateVariables == null) return;
                
            CallbackUpdateVariables();
        }

        public event deleHandlerString CallbackControlState;
        protected virtual void ControlState(string state)
        {
            if (CallbackControlState == null) return;

            CallbackControlState(state);
        }

        public event deleRemoteCommand CallbackRemoteCommand;
        protected virtual bool ReceiveRemoteCommand(string rcmdName, string[] cpNames, string[] cpValues, ref long[] cpAcks)
        {
            if (CallbackRemoteCommand == null)
                return false;

            string strLog = String.Format("RemoteCommand -> Name : {0} / CPName[CPVal] : ", rcmdName);
            for (int i = 0; i < cpNames.Length; ++i)
            {
                strLog += String.Format("{0} [{1}]", cpNames[i], cpValues[i]);
                if (i != cpNames.Length - 1)
                {
                    strLog += ", ";
                }
            }

            WriteLog(strLog);

            return CallbackRemoteCommand(rcmdName, cpNames, cpValues, ref cpAcks);
        }

        public event deleRecvClientToClientMessage CallBackClientToClientMessageReceived;
        protected bool ClientToClientMessageReceived(string device, string messageName, string sendingType, string scenarioName, string[] contentNames, string[] messages, EN_MESSAGE_RESULT result)
        {
            if (CallBackClientToClientMessageReceived == null)
            {
                WriteLog("CallbackRecvSignal null");
                return false;
            }

            return CallBackClientToClientMessageReceived(device, messageName, sendingType, scenarioName, contentNames, messages, result);
        }

        public event deleHandlerString CallbackTerminalMessage;
        protected void ShowTerminalMessage(string message)
        {
            if (CallbackTerminalMessage == null) return;

            CallbackTerminalMessage(message);
        }

        // 2022.08.18 by Thienvv [ADD] For K3: Operator-Call
        public event deleDisplayOperatorCallForm CallbackOperatorCall;
        public void ShowOperatorCall(EN_OPCALL_LEVEL level, string operatorId, bool usingBuzzer, string message)
        {
            if (CallbackOperatorCall == null) return;

            CallbackOperatorCall(level, operatorId, usingBuzzer, message);
        }

        public event deleHandlerString CallbackLogging;
        protected void WriteLog(string message)
        {
            if (CallbackLogging == null) return;

            CallbackLogging(message);
        }

        public event deleHandlerString OnWriteTerminalLog;
        protected void WriteTerminalLog(string message)
        {
            if (CallbackLogging == null) return;

            WriteLog(message);
            OnWriteTerminalLog(message);
        }

        public event deleChangeEquipmentParameters CallbackChangeSystemParameter;
        protected void ChangeSystemParameters(string[] ecNames, string[] values)
        {
            if (CallbackChangeSystemParameter == null) return;

            CallbackChangeSystemParameter(ecNames, values);
        }

        public deleSecsMessageReceived CallbackSecsMessageReceived;
       
        public deleRecipeControlGrant CallbackCheckingRecipeControlGrant;
        public deleReqDownloadingFormattedRecipe CallbackReqDownloadingFormattedRecipe;
        public deleReqUploadingFormattedRecipe CallbackReqUploadingFormattedRecipe;

        public deleReqUPloadingUnformattedRecipeControl CallbackUploadingUnformattedRecipe;
        public deleReqDownloadingUnformattedRecipeControl CallbackDownloadingUnformattedRecipe;
        //24.09.20 by wdw [ADD] Scenerio S7F4 Ack 확인 
        public deleReqUPloadingUnformattedRecipeAck CallbackUploadingUnformattedRecipeAck;

        public deleRecipeFileIsDeleted CallbackRecipeFileIsDeleted;
        #endregion

        #region <Methods>

        #region <Recipe>
        public void SetRecipePath(string path)
        {
            _recipePath = path;
        }

        protected bool GetRecipeFileList(out List<string> result)
        {
            result = new List<string>();

            System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(_recipePath);
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

        protected bool GetRecipeFileWithFullPath(string ppid, out string fullPath)
        {
            string ex = System.IO.Path.GetExtension(ppid);
            string extension = Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE;

            // 중간에 '.'가 들어가면 확장자가 다를 수 있다.
            if (String.IsNullOrEmpty(ex) || false == ex.Equals(extension))
            {
                ppid += Define.DefineConstant.FileFormat.FILEFORMAT_RECIPE;
            }

            fullPath = string.Format(@"{0}\{1}", _recipePath, ppid);

            return System.IO.File.Exists(fullPath);
        }
        #endregion </Recipe>

        #region <Abstract Methods>

        #region <Init, Close>
        public abstract bool Init(string configPath);
        public abstract void Close();
        public abstract void MakeGemSpecification(string configDirectory, ref Dictionary<string, StatusVariable> statusVariableList, ref Dictionary<long, List<StatusVariable>> reportList, ref Dictionary<string, CollectionEvent> collectionEventList);
        public abstract void MakeGemECVSpecification(string configDirectory, ref Dictionary<string, EquipmentConstant> equipmentConstantList);       
        #endregion </Init, Close>

        #region <State>
        public abstract void SetCommStateEnabled();
        public abstract void SetCommStateDisabled();
        public abstract void SetControlState(EN_CONTROL_STATE state);
        public abstract void SetInitControlState(EN_CONTROL_STATE state); // 2022.09.14 by Thienvv [ADD]
        public abstract EN_COMM_STATE GetCommState();
        public abstract EN_CONTROL_STATE GetControlState();
        #endregion </State>

        #region <Alarm>
        public abstract void SetAlarm(int alid);
        public abstract void ClearAlarm(int alid);
        #endregion </Alarm>

        #region <Collection Event>
        public abstract bool SendEvent(long eventId, long[] vids, string[] values, bool useCheckSecondaryMessage);
        public abstract bool IsEventDone(long eventId);
        #endregion </Collection Event>

        #region <Recipe Control>
        // Request to upload a unformatted recipe
        public abstract void ReqUploadingRecipeInquire(string recipeName);

        // Request to upload a unformatted recipe
        public abstract void ReqUploadingUnformattedRecipe(string recipeName);

        // Request to download a unformatted recipe
        public abstract void ReqDownloadingUnformattedRecipe(string recipeName);

        // Request to upload a formatted recipe
        public abstract void ReqUploadingFormattedRecipe(string recipeName, Dictionary<string, string[]> recipeBodies);

        // Request to download a formatted recipe
        public abstract void ReqDownloadingFormattedRecipe(string recipeName);
        #endregion </Recipe Control>

        #region <Variables>
        public abstract bool GetVariables(ref long[] vids, ref string[] values);

        public abstract void UpdateVariable(long vid, List<SemiObject> values);

        public abstract void UpdateVariables(long[] vids, string[] values);

        public abstract void UpdateECV(long[] ecids, string[] values);
        #endregion </Variables>

        #region <Sending Messages>
        public abstract bool SendClientToClientMessage(string device, string messageName, string sendingType, string scenarioName, string[] contentNames, string[] messages, EN_MESSAGE_RESULT result);
        public abstract bool SendUserDefinedSecsMessage(long stream, long function, List<SemiObject> messageStructure);
        #endregion </Sending Messages>

        #region <Gathering>
        public virtual void Execute()
        {

        }
        #endregion </Gathering>

        #endregion </Abstract Methods>

        #region <Equipment Constants>
        public void UpdateEquipmentConstants(Dictionary<string, string> ecidValues)
        {
            var ecidsToUpdate = new Dictionary<long, string>();
            foreach (var item in ecidValues)
            {
                EquipmentConstant ec;
                if (EquipmentConstantList.TryGetValue(item.Key, out ec))
                {
                    ecidsToUpdate[ec.Id] = item.Value;
                }
            }

            UpdateECV(ecidsToUpdate.Keys.ToArray(), ecidsToUpdate.Values.ToArray());
        }
        #endregion </Equipment Constants>

        #endregion </Methods>
    }
}