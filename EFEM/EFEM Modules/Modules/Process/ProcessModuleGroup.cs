using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EquipmentState_;

using EFEM.Defines.Common;
using EFEM.Defines.ProcessModule;
using EFEM.Modules.ProcessModule;

namespace EFEM.Modules
{
    public class ProcessModuleGroup
    {
        #region <Constructors>
        private ProcessModuleGroup()
        {

        }
        #endregion </Constructors>

        #region <Fields>
        private static ProcessModuleGroup _instance = null;

        private readonly ConcurrentDictionary<int, BaseProcessModule> ProcessModules
            = new ConcurrentDictionary<int, BaseProcessModule>();
        #endregion </Fields>

        #region <Properties>
        public static ProcessModuleGroup Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ProcessModuleGroup();

                return _instance;
            }
        }
        public int Count
        {
            get
            {
                if (ProcessModules == null)
                    return 0;

                return ProcessModules.Count;
            }
        }
        #endregion </Properties>

        #region <Methods>

        #region <Process Module>
        public void AssignProcessModule(int moduleIndex, BaseProcessModule processModule)
        {
            ProcessModules[moduleIndex] = processModule;
        }
        public void ExitProcessModuleAll()
        {
            foreach (var item in ProcessModules)
            {
                item.Value.ExitProcessModule();
            }
        }
        public string GetProcessModuleName(int moduleIndex)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return string.Empty;

            return ProcessModules[moduleIndex].Name;
        }
        public string[] GetProcessModuleLocations(int moduleIndex)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return null;

            return ProcessModules[moduleIndex].Locations;
        }
        public string[] GetEntrywayNames(int moduleIndex)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return null;

            return ProcessModules[moduleIndex].Entrys;
        }
        public IReadOnlyDictionary<string, string> GetLocationsByEntry(int moduleIndex)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return null;

            return ProcessModules[moduleIndex].LocationsByEntry;
        }
        public IReadOnlyDictionary<string, int> GetLocationCapacity(int moduleIndex)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return null;

            Dictionary<string, int> locationAndCapacity = new Dictionary<string, int>();
            for (int i = 0; ProcessModules[moduleIndex].Locations != null && i <  ProcessModules[moduleIndex].Locations.Length; ++i)
            {
                var loc = ProcessModules[moduleIndex].Locations[i];
                var capacity = ProcessModules[moduleIndex].Capacity[i];

                locationAndCapacity[loc] = capacity;
            }

            return locationAndCapacity;
        }
        public int GetProcessModuleIndexByEntry(string entry)
        {
            int pmIndex = -1;

            foreach (var item in ProcessModules)
            {
                if (item.Value.LocationsByEntry.ContainsKey(entry))
                {
                    return item.Key;
                }
            }

            return pmIndex;
        }
        public bool SetEquipmentState(int moduleIndex, EQUIPMENT_STATE status)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            ProcessModules[moduleIndex].EquipmentState = status;
            return true;
        }
        public EQUIPMENT_STATE GetEquipmentState(int moduleIndex)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return EQUIPMENT_STATE.UNDEFINED;

            return ProcessModules[moduleIndex].EquipmentState;
        }
        public bool IsSimulationMode(int moduleIndex)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            return ProcessModules[moduleIndex].IsSimulation;
        }
        public bool SetRecipeId(int moduleIndex, string recipeId)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            ProcessModules[moduleIndex].RecipeId = recipeId;
            return true;
        }
        public string GetRecipeId(int moduleIndex)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return string.Empty;

            return ProcessModules[moduleIndex].RecipeId;
        }
        public bool SetLotId(int moduleIndex, string lotID)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            ProcessModules[moduleIndex].LotId = lotID;
            return true;
        }
        public string GetLotId(int moduleIndex)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return string.Empty;

            return ProcessModules[moduleIndex].LotId;
        }
        #endregion </Process Module>

        #region <Communication>

        #region <SMEMA>
        public void ResetSignalsAll()
        {
            foreach (var item in ProcessModules)
            {
                item.Value.ResetSignalsAll();
            }
        }
        public bool SetLoadingSignal(int moduleIndex, string location, bool enabled)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            ProcessModules[moduleIndex].SetLoadingSignal(location, enabled);
            return true;
        }

        public bool SetUnloadingSignal(int moduleIndex, string location, bool enabled)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            ProcessModules[moduleIndex].SetUnloadingSignal(location, enabled);
            return true;
        }
        public bool IsLoadingRequested(int moduleIndex, string locationName)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            List<string> locations = new List<string>();
            if (false == IsLoadingRequested(moduleIndex, ref locations))
                return false;

            return locations.Contains(locationName);
        }
        public bool IsLoadingRequested(int moduleIndex, ref List<string> locationNames)
        {
            locationNames.Clear();
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            return ProcessModules[moduleIndex].IsLoadingRequested(ref locationNames);
        }
        public bool IsUnloadingRequested(int moduleIndex, string locationName)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            List<string> locations = new List<string>();
            if (false == IsUnloadingRequested(moduleIndex, ref locations))
                return false;

            return locations.Contains(locationName);
        }
        public bool IsUnloadingRequested(int moduleIndex, ref List<string> locationNames)
        {
            locationNames.Clear();
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            return ProcessModules[moduleIndex].IsUnloadingRequested(ref locationNames);
        }
        #endregion </SMEMA>

        #region <WCF>

        #region <Connection>
        public bool GetCommunicationInfo(int pmIndex, ref NetworkInformation communicationInfo)
        {
            if (false == ProcessModules.ContainsKey(pmIndex))
                return false;

            communicationInfo = ProcessModules[pmIndex].CommunicationInfo;

            return (communicationInfo != null);
        }
        public bool InitCommunication(int pmIndex)
        {
            if (false == ProcessModules.ContainsKey(pmIndex))
                return false;

            return ProcessModules[pmIndex].InitCommunication();
        }
        #endregion </Connection>

        #region <Send>
        public bool SendMessage(int moduleIndex, string entry, string title, Dictionary<string, string> messagePairs)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            if (IsSimulationMode(moduleIndex))
                return true;

            return ProcessModules[moduleIndex].SendMessage(entry, title, messagePairs);
        }
        public bool SendMessage(int moduleIndex, string entry, string title, string substrateName)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            if (IsSimulationMode(moduleIndex))
                return true;

            return ProcessModules[moduleIndex].SendMessage(entry, title, substrateName);
        }
        public CommunicationResult IsSendingCompleted(int moduleIndex, string entry, string title)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return CommunicationResult.Error;

            if (IsSimulationMode(moduleIndex))
                return CommunicationResult.Ack;

            return CommunicationResult.Ack;
        }
        public bool GetSendingResult(int moduleIndex, string entry, string title, ref Dictionary<string, string> receivedData)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            if (IsSimulationMode(moduleIndex))
                return true;

            return ProcessModules[moduleIndex].GetSendingResult(entry, title, ref receivedData);
        }
        #endregion </Send>

        #region <Received>
        public bool SetAckReceivedMessage(int moduleIndex, string entry, string title, CommunicationResult result, string description)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            if (IsSimulationMode(moduleIndex))
                return true;

            ProcessModules[moduleIndex].SetAckToReceivedMessage(entry, title, result, description);
            return true;
        }
        public CommunicationResult IsMessageReceived(int moduleIndex, string entry, string title)
        {
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return CommunicationResult.Error;

            if (IsSimulationMode(moduleIndex))
                return CommunicationResult.Ack;

            return ProcessModules[moduleIndex].IsMessageReceived(entry, title);
        }
        public bool GetReceivedData(int moduleIndex, string entry, string title, out Dictionary<string, string> receivedData)
        {
            receivedData = null;
            if (false == ProcessModules.ContainsKey(moduleIndex))
                return false;

            if (IsSimulationMode(moduleIndex))
                return true;

            return ProcessModules[moduleIndex].GetReceivedData(entry, title, out receivedData);
        }
        #endregion </Received>

        #endregion </WCF>

        #endregion </Communication>
        public void ExecuteAll()
        {
            foreach (var item in ProcessModules)
            {
                if (item.Value == null)
                    continue;
                item.Value.Execute();
            }
        }
        #endregion </Methods>
    }
}
