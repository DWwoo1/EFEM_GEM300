using System.Collections.Generic;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Communicator
{
    internal interface ISecsGemRuntime
    {
        bool IsConnect { get; }
        bool MaintenanceMode { get; set; }
        bool IsExitingRequested { get; set; }

        void AttachDisplayLog(deleHandlerString pFunc);
        void LinkTerminalMessage(deleHandlerString pFunc);
        void LinkShowOperatorCall(deleDisplayOperatorCallForm pFunc);
        void LinkConnection(deleHandlerVoid pFunc);
        void LinkControlState(deleHandlerString pFunc);
        void LinkRemoteCommand(deleRemoteCommand pFunc);
        void LinkEquipmentParameterChangeRequest(deleChangeEquipmentParameters pFunc);
        void LinkClientToClientMessage(deleRecvClientToClientMessage pFunc);
        void LinkSecsMessageReceived(deleSecsMessageReceived pFunc);
        void LinkRecipeControlGrant(deleRecipeControlGrant pFunc);
        void LinkUnFormattedRecipeControls(
            deleReqUPloadingUnformattedRecipeControl pUploadingFunc,
            deleReqDownloadingUnformattedRecipeControl pDownloadingFunc,
            deleReqUPloadingUnformattedRecipeAck pUploadingAck);
        void LinkFormattedRecipeControls(
            deleReqUploadingFormattedRecipe pUploadingFunc,
            deleReqDownloadingFormattedRecipe pDownloadingFunc);
        void LinkRecipeFileIsDeleted(deleRecipeFileIsDeleted pFunc);
        void LinkTerminalMessageWithProcessingScenario(deleHandlerString pFunc);

        bool Initialize(SecsGem driver, string cfgPath, string recipePath);
        void MakeGemSpecification(
            string configDirectory,
            out Dictionary<string, StatusVariable> statusVariableList,
            out Dictionary<long, List<StatusVariable>> reportList,
            out Dictionary<string, CollectionEvent> collectionEventList);
        void MakeGemECVSpecification(
            string configDirectory,
            out Dictionary<string, EquipmentConstant> equipmentConstantList);
        void Exit();
        bool Reset();

        void SetControlState(EN_CONTROL_STATE enControlState);
        EN_CONTROL_STATE GetControlState();
        void SetCommStateToEnable();
        void SetCommStateToDisable();
        EN_COMM_STATE GetCommState();

        void SetAlarm(int nAlarm);
        void ClearAlarm(int nAlarm);

        bool SendClientToClientMessage(
            string device,
            string messageName,
            string sendingType,
            string scenarioName,
            string[] contentNames,
            string[] messages,
            EN_MESSAGE_RESULT result,
            bool useLogging);

        void SendEvent(long eventID, long[] vids, string[] vidValues, bool useCheckSecondaryAck = true);
        bool IsSendingEventCompleted(long nEventID);
        bool SendUserDefinedSecsMessage(long stream, long function, List<SemiObject> structure);

        void ShowOperatorCallingMessage(string strMessage);

        void WriteTerminalLog(string log);
        void WriteLog(string strLog);
        void WriteScenarioLog(string strLog);

        void UpdateECVParameter(long nID, string strValue);
        void UpdateECVParameters(long[] arrIDs, string[] arrValues);
        void UpdateECVParameters(Dictionary<string, string> ecidValues);
        void UpdateVariable(long vid, List<SemiObject> value);
        void UpdateVariable(long nID, string strValue);
        void UpdateVariables(long[] arrIDs, string[] arrValues);

        void SendRecipeUploadInquire(string recipeName);
        void SendRecipeUploadUnFormatted(string recipeName);
        void SendRecipeDownloadUnFormatted(string recipeName);

        void Execute();
    }
}