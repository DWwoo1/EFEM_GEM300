using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using AnalogIO_;

using FrameOfSystem3.ExternalDevice.Serial.FanFilterUnit;
using Define.DefineEnumProject.AnalogIO.PWA500W;
using FrameOfSystem3.SECSGEM.Trace;

namespace EFEM.CustomizedByProcessType.PWA500W
{
    public interface IDetachingTraceParameterProvider
    {
        bool TryApplyExternalTraceData(IReadOnlyDictionary<string, string> values);
        void AppendDetachingTraceParameters(IDictionary<string, string> scenarioParam);
    }

    public sealed class PWA500WNRDTraceDataProvider : ITraceDataProvider, IDetachingTraceParameterProvider
    {
        private readonly AnalogIO _analogIO;
        private readonly FanFilterUnitManager _ffuManager;

        private readonly ConcurrentDictionary<long, string> _variablesToUpdate
            = new ConcurrentDictionary<long, string>();

        private Dictionary<long, int> _analogInfo = new Dictionary<long, int>();
        private ConcurrentDictionary<string, string> _traceDataAtDetaching
            = new ConcurrentDictionary<string, string>();
        private ConcurrentDictionary<string, string> _traceDataForExternal
            = new ConcurrentDictionary<string, string>();
        private Dictionary<string, long> _traceDataByName
            = new Dictionary<string, long>();

        private UnitItem _unitItem = new UnitItem();

        public PWA500WNRDTraceDataProvider(
            AnalogIO analogIO,
            FanFilterUnitManager ffuManager)
        {
            _analogIO = analogIO;
            _ffuManager = ffuManager;
        }

        public TraceDefinition BuildDefinition()
        {
            BuildTraceMaps();

            return new TraceDefinition
            {
                IsEnabled = _variablesToUpdate.Count > 0,
                IntervalMs = 2000,
            };
        }
        public IReadOnlyCollection<long> GetConfiguredVariableIds()
        {
            return _variablesToUpdate.Keys
                .OrderBy(k => k)
                .ToList();
        }
        public bool Initialize(ITraceRecoveryStore recoveryStore)
        {
            bool loadedFromTraceInfo = false;

            if (recoveryStore != null &&
                recoveryStore.TryReadTraceInfo(out var info, out var detaching, out var trace))
            {
                _traceDataByName = info.OrderBy(kvp => kvp.Value)
                                       .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                _traceDataAtDetaching = new ConcurrentDictionary<string, string>(detaching);
                _traceDataForExternal = new ConcurrentDictionary<string, string>(trace);

                foreach (KeyValuePair<string, long> item in _traceDataByName)
                {
                    _variablesToUpdate[item.Value] = string.Empty;
                }

                loadedFromTraceInfo = true;
            }

            if (false == loadedFromTraceInfo)
            {
                BuildDefaultExternalTraceMaps();

                if (recoveryStore != null)
                {
                    recoveryStore.WriteTraceInfo(
                        _traceDataByName,
                        _traceDataAtDetaching.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                }
            }

            Dictionary<long, string> valuesFromRecovery = new Dictionary<long, string>(_variablesToUpdate);
            recoveryStore?.TryReadLastValues(ref valuesFromRecovery);

            foreach (KeyValuePair<long, string> item in valuesFromRecovery)
            {
                if (_variablesToUpdate.ContainsKey(item.Key))
                {
                    _variablesToUpdate[item.Key] = item.Value ?? string.Empty;
                }
            }

            return true;
        }

        public void SaveRecovery(ITraceRecoveryStore recoveryStore)
        {
            if (recoveryStore == null)
                return;

            recoveryStore.WriteLastValues(new Dictionary<long, string>(_variablesToUpdate));
        }

        public void Refresh()
        {
            RefreshLocalTraceValues();
            RefreshMappedTraceValues();
        }

        public bool TryGetSnapshot(out Dictionary<long, string> snapshot)
        {
            snapshot = new Dictionary<long, string>();

            if (EquipmentState_.EquipmentState.GetInstance().GetState()
                == EquipmentState_.EQUIPMENT_STATE.UNDEFINED)
                return false;

            foreach (KeyValuePair<long, string> item in _variablesToUpdate)
            {
                snapshot[item.Key] = item.Value ?? string.Empty;
            }

            return true;
        }

        public bool TryGetValueFromId(long variableId, out string value)
        {
            if (_variablesToUpdate.TryGetValue(variableId, out string current))
            {
                value = current ?? string.Empty;
                return true;
            }

            value = string.Empty;
            return false;
        }

        public bool TryApplyExternalTraceData(IReadOnlyDictionary<string, string> values)
        {
            if (values == null)
                return false;

            foreach (KeyValuePair<string, string> item in values)
            {
                string value = item.Value ?? string.Empty;
                _traceDataForExternal[item.Key] = value;

                if (_traceDataAtDetaching.ContainsKey(item.Key))
                {
                    _traceDataAtDetaching[item.Key] = value;
                }
            }

            return true;
        }
        public void AppendDetachingTraceParameters(IDictionary<string, string> scenarioParam)
        {
            if (scenarioParam == null)
                return;

            foreach (KeyValuePair<string, string> item in _traceDataAtDetaching)
            {
                scenarioParam[item.Key] = item.Value ?? string.Empty;
            }
        }
        private void RefreshLocalTraceValues()
        {
            foreach (KeyValuePair<long, int> item in _analogInfo)
            {
                if (_variablesToUpdate.ContainsKey(item.Key))
                {
                    _variablesToUpdate[item.Key] = _analogIO.ReadInputValue(item.Value).ToString();
                }
            }

            for (int i = 0; i < _ffuManager.Count && i < 4; ++i)
            {
                if (false == _ffuManager.GetInformation(i, ref _unitItem))
                    continue;

                _variablesToUpdate[1707 + i] = _unitItem.CurrentSpeed.ToString();
            }
        }

        private void RefreshMappedTraceValues()
        {
            foreach (KeyValuePair<string, string> item in _traceDataForExternal)
            {
                if (_traceDataByName.TryGetValue(item.Key, out long variableId))
                {
                    _variablesToUpdate[variableId] = item.Value ?? string.Empty;
                }
            }
        }

        private void BuildTraceMaps()
        {
            _variablesToUpdate.Clear();

            _analogInfo = new Dictionary<long, int>();
            _traceDataAtDetaching = new ConcurrentDictionary<string, string>();
            _traceDataForExternal = new ConcurrentDictionary<string, string>();
            _traceDataByName = new Dictionary<string, long>();

            // EFEM / local 기본 VID만 생성
            _variablesToUpdate[1700] = string.Empty;
            _variablesToUpdate[1701] = string.Empty;
            _variablesToUpdate[1702] = string.Empty;
            _variablesToUpdate[1703] = string.Empty;
            _variablesToUpdate[1704] = string.Empty;
            _variablesToUpdate[1705] = string.Empty;
            _variablesToUpdate[1706] = string.Empty;
            _variablesToUpdate[1707] = string.Empty;
            _variablesToUpdate[1708] = string.Empty;
            _variablesToUpdate[1709] = string.Empty;
            _variablesToUpdate[1710] = string.Empty;

            _analogInfo = new Dictionary<long, int>
            {
                [1700] = (int)EN_ANALOG_IN.EFEM_MAIN_CDA_PRESSURE_SWITCH,
                [1701] = (int)EN_ANALOG_IN.EFEM_MAIN_CDA_VACUUM_SWITCH,
                [1702] = (int)EN_ANALOG_IN.ROBOT_CDA_PRESSURE_SWITCH,
                [1703] = (int)EN_ANALOG_IN.IONIZER_PRESSURE_SWITCH,
                [1704] = (int)EN_ANALOG_IN.IONIZER_1,
                [1705] = (int)EN_ANALOG_IN.IONIZER_2,
                [1706] = (int)EN_ANALOG_IN.IONIZER_3
            };
        }

        private void BuildDefaultExternalTraceMaps()
        {
            _traceDataForExternal = new ConcurrentDictionary<string, string>
            {
                [EN_SVID_LIST.SUPPLY_BUFFER_IONIZER_FLOW.ToString()] = string.Empty,
                [EN_SVID_LIST.SORTING_BUFFER_IONIZER_FLOW.ToString()] = string.Empty,
                [EN_SVID_LIST.SUPPLY_STAGE_IONIZER_FLOW.ToString()] = string.Empty,
                [EN_SVID_LIST.SORTING_STAGE_IONIZER_FLOW.ToString()] = string.Empty,

                [EN_SVID_LIST.PM_FFU_SPEED_1.ToString()] = string.Empty,
                [EN_SVID_LIST.PM_FFU_SPEED_2.ToString()] = string.Empty,

                [EN_SVID_LIST.EJECT_MEMBRANE_AIR_REGULATOR.ToString()] = string.Empty,
                [EN_SVID_LIST.EJECT_MEMBRANE_VAC_PRESS.ToString()] = string.Empty,
                [EN_SVID_LIST.EJECT_VAC_PRESS.ToString()] = string.Empty,

                [EN_SVID_LIST.NEEDLE_HEIGHT.ToString()] = string.Empty,
                [EN_SVID_LIST.EXPENSION_HEIGHT.ToString()] = string.Empty,
                [EN_SVID_LIST.PICK_SEARCH_LEVEL.ToString()] = string.Empty,
                [EN_SVID_LIST.PICK_SEARCH_SPEED.ToString()] = string.Empty,
                [EN_SVID_LIST.PICK_DELAY.ToString()] = string.Empty,
                [EN_SVID_LIST.PICK_FORCE.ToString()] = string.Empty,
                [EN_SVID_LIST.PICK_SLOWUP_LEVEL.ToString()] = string.Empty,
                [EN_SVID_LIST.PICK_SLOWUP_SPEED.ToString()] = string.Empty,
                [EN_SVID_LIST.PLACE_SEARCH_LEVEL.ToString()] = string.Empty,
                [EN_SVID_LIST.PLACE_SEARCH_SPEED.ToString()] = string.Empty,
                [EN_SVID_LIST.PLACE_DELAY.ToString()] = string.Empty,
                [EN_SVID_LIST.PLACE_FORCE.ToString()] = string.Empty,
                [EN_SVID_LIST.PLACE_SLOWUP_LEVEL.ToString()] = string.Empty,
                [EN_SVID_LIST.PLACE_SLOWUP_SPEED.ToString()] = string.Empty
            };

            _traceDataAtDetaching = new ConcurrentDictionary<string, string>
            {
                [EN_SVID_LIST.NEEDLE_HEIGHT.ToString()] = string.Empty,
                [EN_SVID_LIST.EXPENSION_HEIGHT.ToString()] = string.Empty,
                [EN_SVID_LIST.PICK_SEARCH_LEVEL.ToString()] = string.Empty,
                [EN_SVID_LIST.PICK_SEARCH_SPEED.ToString()] = string.Empty,
                [EN_SVID_LIST.PICK_DELAY.ToString()] = string.Empty,
                [EN_SVID_LIST.PICK_FORCE.ToString()] = string.Empty,
                [EN_SVID_LIST.PICK_SLOWUP_LEVEL.ToString()] = string.Empty,
                [EN_SVID_LIST.PICK_SLOWUP_SPEED.ToString()] = string.Empty,
                [EN_SVID_LIST.PLACE_SEARCH_LEVEL.ToString()] = string.Empty,
                [EN_SVID_LIST.PLACE_SEARCH_SPEED.ToString()] = string.Empty,
                [EN_SVID_LIST.PLACE_DELAY.ToString()] = string.Empty,
                [EN_SVID_LIST.PLACE_FORCE.ToString()] = string.Empty,
                [EN_SVID_LIST.PLACE_SLOWUP_LEVEL.ToString()] = string.Empty,
                [EN_SVID_LIST.PLACE_SLOWUP_SPEED.ToString()] = string.Empty
            };

            _traceDataByName = new Dictionary<string, long>
            {
                [EN_SVID_LIST.SUPPLY_BUFFER_IONIZER_FLOW.ToString()] = 1750,
                [EN_SVID_LIST.SORTING_BUFFER_IONIZER_FLOW.ToString()] = 1751,
                [EN_SVID_LIST.SUPPLY_STAGE_IONIZER_FLOW.ToString()] = 1752,
                [EN_SVID_LIST.SORTING_STAGE_IONIZER_FLOW.ToString()] = 1753,

                [EN_SVID_LIST.PM_FFU_SPEED_1.ToString()] = 1754,
                [EN_SVID_LIST.PM_FFU_SPEED_2.ToString()] = 1755,

                [EN_SVID_LIST.EJECT_MEMBRANE_AIR_REGULATOR.ToString()] = 1760,
                [EN_SVID_LIST.EJECT_MEMBRANE_VAC_PRESS.ToString()] = 1761,
                [EN_SVID_LIST.EJECT_VAC_PRESS.ToString()] = 1762,

                [EN_SVID_LIST.NEEDLE_HEIGHT.ToString()] = 2000,
                [EN_SVID_LIST.EXPENSION_HEIGHT.ToString()] = 2001,
                [EN_SVID_LIST.PICK_SEARCH_LEVEL.ToString()] = 2010,
                [EN_SVID_LIST.PICK_SEARCH_SPEED.ToString()] = 2011,
                [EN_SVID_LIST.PICK_DELAY.ToString()] = 2012,
                [EN_SVID_LIST.PICK_FORCE.ToString()] = 2013,
                [EN_SVID_LIST.PICK_SLOWUP_LEVEL.ToString()] = 2014,
                [EN_SVID_LIST.PICK_SLOWUP_SPEED.ToString()] = 2015,
                [EN_SVID_LIST.PLACE_SEARCH_LEVEL.ToString()] = 2020,
                [EN_SVID_LIST.PLACE_SEARCH_SPEED.ToString()] = 2021,
                [EN_SVID_LIST.PLACE_DELAY.ToString()] = 2022,
                [EN_SVID_LIST.PLACE_FORCE.ToString()] = 2023,
                [EN_SVID_LIST.PLACE_SLOWUP_LEVEL.ToString()] = 2024,
                [EN_SVID_LIST.PLACE_SLOWUP_SPEED.ToString()] = 2025
            };

            foreach (KeyValuePair<string, long> item in _traceDataByName.OrderBy(kvp => kvp.Value))
            {
                _variablesToUpdate[item.Value] = string.Empty;
            }
        }
    }
}