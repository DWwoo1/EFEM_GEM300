using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DigitalIO_;

using EFEM.Defines.ProcessModule;

namespace EFEM.Modules.ProcessModule
{
    public class SignalBasedInterfaceHandler
    {
        #region <Constructors>
        public SignalBasedInterfaceHandler(bool digitalIOSimulation)
        {
            LoadingInputSignalsByLocation = new Dictionary<string, int>();
            LoadingOutputSignalsByLocation = new Dictionary<string, int>();

            UnloadingInputSignalsByLocation = new Dictionary<string, int>();
            UnloadingOutputSignalsByLocation = new Dictionary<string, int>();

            LoadingRequested = new ConcurrentDictionary<string, bool>();
            UnloadingRequested = new ConcurrentDictionary<string, bool>();

            IsDigitalIOSimulation = digitalIOSimulation;
            if (false == IsDigitalIOSimulation)
            {
                _digitalIO = DigitalIO.GetInstance();
            }
            else
            {
                _inputValuesForSimulation = new Dictionary<int, bool>();
                _outputValuesForSimulation = new Dictionary<int, bool>();
            }
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly Dictionary<string, int> LoadingInputSignalsByLocation = null;
        private readonly Dictionary<string, int> UnloadingInputSignalsByLocation = null;
        private readonly Dictionary<string, int> LoadingOutputSignalsByLocation = null;
        private readonly Dictionary<string, int> UnloadingOutputSignalsByLocation = null;

        private readonly Dictionary<int, bool> _inputValuesForSimulation = null;
        private readonly Dictionary<int, bool> _outputValuesForSimulation = null;

        private readonly ConcurrentDictionary<string, bool> LoadingRequested = null;
        private readonly ConcurrentDictionary<string, bool> UnloadingRequested = null;

        private static DigitalIO _digitalIO = null;
        #endregion </Fields>

        #region <Properties>
        public bool IsDigitalIOSimulation { get; }
        #endregion </Properties>

        #region <Methods>

        #region <Assign>
        public void AssignInputSignalsInLoadingLocation(string location, int signalIndex)
        {
            LoadingInputSignalsByLocation[location] = signalIndex;

            LoadingRequested[location] = false;
            
            if (IsDigitalIOSimulation)
            {
                _inputValuesForSimulation[signalIndex] = false;
            }
        }
        public void AssignOutputSignalsInLoadingLocation(string location, int signalIndex)
        {
            LoadingOutputSignalsByLocation[location] = signalIndex;

            if (IsDigitalIOSimulation)
            {
                _outputValuesForSimulation[signalIndex] = false;
            }
        }
        public void AssignInputSignalsInUnloadingLocation(string location, int signalIndex)
        {
            UnloadingInputSignalsByLocation[location] = signalIndex;

            UnloadingRequested[location] = false;

            if (IsDigitalIOSimulation)
            {
                _inputValuesForSimulation[signalIndex] = false;
            }
        }
        public void AssignOutputSignalsInUnloadingLocation(string location, int signalIndex)
        {
            UnloadingOutputSignalsByLocation[location] = signalIndex;

            if (IsDigitalIOSimulation)
            {
                _outputValuesForSimulation[signalIndex] = false;
            }
        }
        #endregion </Assign>

        #region <Send>
        public void ResetSignalsAll()
        {
            foreach (var item in LoadingOutputSignalsByLocation)
            {
                SetLoadingSignal(item.Key, false);
            }

            foreach (var item in UnloadingOutputSignalsByLocation)
            {
                SetUnloadingSignal(item.Key, false);
            }
        }
        public void SetLoadingSignal(string location, bool enabled)
        {
            int index = LoadingOutputSignalsByLocation[location];
            if (index < 0)
                return;

            WriteSignal(index, enabled);
        }
        public void SetUnloadingSignal(string location, bool enabled)
        {
            int index = UnloadingOutputSignalsByLocation[location];
            if (index < 0)
                return;

            WriteSignal(index, enabled);
        }
        #endregion </Send>

        #region <Receive>
        public bool IsLoadingRequestedBySignal(ref List<string> requestedLocation)
        {
            requestedLocation.Clear();
            foreach (var item in LoadingRequested)
            {
                if (item.Value)
                {
                    requestedLocation.Add(item.Key);
                }
            }

            return requestedLocation.Count > 0;
        }
        public bool IsUnloadingRequestedBySignal(ref List<string> requestedLocation)
        {
            requestedLocation.Clear();
            foreach (var item in UnloadingRequested)
            {
                if (item.Value)
                {
                    requestedLocation.Add(item.Key);
                }
            }

            return requestedLocation.Count > 0;
        }
        #endregion </Receive>

        public void SetLoadingSignalForSimulation(string location, bool enabled)
        {
            if (false == IsDigitalIOSimulation)
                return;

            if (false == LoadingInputSignalsByLocation.TryGetValue(location, out var index))
                return;

            _inputValuesForSimulation[index] = enabled;
        }

        public void SetUnloadingSignalForSimulation(string location, bool enabled)
        {
            if (false == IsDigitalIOSimulation)
                return;

            if (false == UnloadingInputSignalsByLocation.TryGetValue(location, out var index))
                return;

            _inputValuesForSimulation[index] = enabled;
        }
        public void UpdateInformations()
        {
            foreach (var item in LoadingInputSignalsByLocation)
            {
                LoadingRequested[item.Key] = ReadSignal(item.Value);
            }

            foreach (var item in UnloadingInputSignalsByLocation)
            {
                UnloadingRequested[item.Key] = ReadSignal(item.Value);
            }
        }

        private bool ReadSignal(int index)
        {
            if (false == IsDigitalIOSimulation)
            {
                return _digitalIO.ReadInput(index);
            }
            else
            {
                return _inputValuesForSimulation[index];
            }
        }
        private void WriteSignal(int index, bool signal)
        {
            if (false == IsDigitalIOSimulation)
            {
                _digitalIO.WriteOutput(index, signal);
            }
            else
            {
                _outputValuesForSimulation[index] = signal;
            }
        }
        #endregion </Methods>
    }
}
