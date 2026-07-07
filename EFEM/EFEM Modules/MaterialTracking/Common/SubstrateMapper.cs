using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Globalization;

using EFEM.Defines.LoadPort;
using EFEM.Defines.MaterialTracking;

namespace EFEM.MaterialTracking
{
    public static class SubstrateMapper
    {
        private readonly static Color Normal = Color.Silver;

        // 코어 키 셋(Extra 제외용)
        private readonly static HashSet<string> _commonKeys = new HashSet<string>(StringComparer.Ordinal)
        {
            BaseSubstrateAttributeKeys.UniqueKey,
            BaseSubstrateAttributeKeys.Name,
            BaseSubstrateAttributeKeys.OriginName,
            BaseSubstrateAttributeKeys.Location,
            BaseSubstrateAttributeKeys.SourcePortId,
            BaseSubstrateAttributeKeys.SourceSlot,
            BaseSubstrateAttributeKeys.SourceCarrierId,
            BaseSubstrateAttributeKeys.CurrentCarrierKey,
            BaseSubstrateAttributeKeys.DestinationPortId,
            BaseSubstrateAttributeKeys.DestinationSlot,
            BaseSubstrateAttributeKeys.LotId,
            BaseSubstrateAttributeKeys.RecipeId,
            BaseSubstrateAttributeKeys.ProcessJobId,
            BaseSubstrateAttributeKeys.ControlJobId,
            BaseSubstrateAttributeKeys.TransPortState,
            BaseSubstrateAttributeKeys.ProcessingState,
            BaseSubstrateAttributeKeys.IdReadingState,
            BaseSubstrateAttributeKeys.DoNotProcessFlag,
            BaseSubstrateAttributeKeys.Usage
        };

        private readonly static Dictionary<CarrierSlotMapStates, Color> _slotColors = new Dictionary<CarrierSlotMapStates, Color>()
        {
            [CarrierSlotMapStates.Undefined] = Color.White,
            [CarrierSlotMapStates.Empty] = Color.White,
            [CarrierSlotMapStates.NotEmpty] = Color.DarkViolet,
            [CarrierSlotMapStates.CorrectlyOccupied] = Normal,
            [CarrierSlotMapStates.DoubleSlotted] = Color.DarkViolet,
            [CarrierSlotMapStates.CrossSlotted] = Color.Brown,
        };

        private readonly static Dictionary<ProcessingStates, Color> _processingColors = new Dictionary<ProcessingStates, Color>()
        {
            [ProcessingStates.NeedsProcessing] = Normal,
            [ProcessingStates.InProcess] = Color.Blue,
            [ProcessingStates.Processed] = Color.Green,
            [ProcessingStates.Rejected] = Color.Orange,
            [ProcessingStates.Stopped] = Color.LightYellow,
            [ProcessingStates.Aborted] = Color.LightYellow,
            [ProcessingStates.Skipped] = Color.LightYellow,
            [ProcessingStates.Lost] = Color.Red,
        };

        private readonly static Dictionary<TransportStates, Color> _substrateTransferColors = new Dictionary<TransportStates, Color>()
        {
            [TransportStates.AtSource] = Normal,
            [TransportStates.AtWork] = Color.Blue,
            [TransportStates.AtDestination] = Color.LimeGreen,
        };

        public static string MakeUniqueKey(string carrierName, string locName)
        {
            var now = DateTime.Now;
           
            return $"{carrierName}_{locName}_{now.Ticks}";
        }

        public static string MakeDefualtName(string carrierName, string locName)
        {
            return $"{carrierName}_{locName}";
        }

        public static Color GetColorBySubstrateStatus(TransportStates transferStatus, ProcessingStates processingStatus)
        {
            Color color;
            switch (processingStatus)
            {
                case ProcessingStates.NeedsProcessing:
                case ProcessingStates.InProcess:
                case ProcessingStates.Processed:
                    {
                        switch (transferStatus)
                        {
                            case TransportStates.AtWork:
                            case TransportStates.AtSource:
                                {
                                    if (_processingColors.TryGetValue(processingStatus, out color))
                                    {
                                        return color;
                                    }
                                }
                                break;
                            case TransportStates.AtDestination:
                                {
                                    if (_substrateTransferColors.TryGetValue(transferStatus, out color))
                                    {
                                        return color;
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;
                case ProcessingStates.Rejected:
                case ProcessingStates.Stopped:
                case ProcessingStates.Aborted:
                case ProcessingStates.Skipped:
                case ProcessingStates.Lost:
                    {
                        if (_processingColors.TryGetValue(processingStatus, out color))
                        {
                            return color;
                        }
                    }
                    break;
                default:
                    break;
            }

            return Color.White;
        }
        public static Color GetColorBySlotStatus(CarrierSlotMapStates state)
        {
            return _slotColors.TryGetValue(state, out var color) ? color : Color.White;
        }

        public static SubstrateItem ToData(Substrate s)
        {
            if (s == null) return null;

            var dto = new SubstrateItem
            {
                UniqueKey = s.UniqueKey ?? string.Empty,
                Name = s.Name ?? s.UniqueKey ?? string.Empty,
                OriginName = s.OriginName ?? s.Name ?? string.Empty,
                LocationId = s.LocationId != null ? s.LocationId : string.Empty,
                SourcePortId = s.SourcePortId,
                SourceSlot = s.SourceSlot,
                SourceCarrierId = s.SourceCarrierId ?? string.Empty,
                CurrentCarrierKey = s.CurrentCarrierKey ?? string.Empty,
                DestinationPortId = s.DestinationPortId,
                DestinationSlot = s.DestinationSlot,
                LotId = s.LotId ?? string.Empty,
                RecipeId = s.RecipeId ?? string.Empty,
                ProcessJobId = s.ProcessJobId ?? string.Empty,
                ControlJobId = s.ControlJobId ?? string.Empty,
                TransportStatus = (int)s.TransportStatus,
                ProcessingStatus = (int)s.ProcessingStatus,
                IdReadingStatus = (int)s.IdReadingStatus,
                DoNotProcessFlag = s.DoNotProcessFlag,
                Usage = s.Usage,
                Extra = ExtractExtra(s.Extra),
            };

            return dto;
        }

        public static void Apply(Substrate target, SubstrateItem dto)
        {
            if (target == null || dto == null) return;

            if (false == string.Equals(target.UniqueKey, dto.UniqueKey, StringComparison.Ordinal))
                target.UniqueKey = dto.UniqueKey ?? string.Empty;

            target.Name = dto.Name ?? dto.UniqueKey ?? string.Empty;
            target.OriginName = target.Name;

            //if (!string.IsNullOrEmpty(dto.LocationId))
            //    target.SetAttribute(BaseSubstrateAttributeKeys.Location, dto.LocationId);


            // TODO : 아래는 추후 제거될 코드
            if (dto.LocationId.Contains("PM"))
            {
                var name = EFEM.Modules.ProcessModuleGroup.Instance.GetProcessModuleName(0);
                dto.LocationId = name;
            }
            //LocationServer.Location loc = new LocationServer.Location(string.Empty);
            //LocationServer.LocationServer.Instance.GetLocationByName(dto.LocationId, ref loc);

            target.LocationId = dto.LocationId;

            target.SourcePortId = dto.SourcePortId;
            target.SourceSlot = dto.SourceSlot;
            target.SourceCarrierId = dto.SourceCarrierId ?? string.Empty;
            target.CurrentCarrierKey = dto.CurrentCarrierKey ?? string.Empty;
            target.DestinationPortId = dto.DestinationPortId;
            target.DestinationSlot = dto.DestinationSlot;

            target.LotId = dto.LotId ?? string.Empty;
            target.RecipeId = dto.RecipeId ?? string.Empty;

            target.ProcessJobId = dto.ProcessJobId ?? string.Empty;
            target.ControlJobId = dto.ControlJobId ?? string.Empty;

            target.TransportStatus = (TransportStates)dto.TransportStatus;
            target.ProcessingStatus = (ProcessingStates)dto.ProcessingStatus;
            target.IdReadingStatus = (IdReadingStates)dto.IdReadingStatus;

            target.DoNotProcessFlag = dto.DoNotProcessFlag;
            target.Usage = dto.Usage;

            if (dto.Extra != null)
            {
                foreach (var kv in dto.Extra)
                {
                    if (kv.Value == null)
                        continue;

                    target.Extra[kv.Key] = kv.Value;
                }
            }
        }

        public static Substrate ToDomain(SubstrateItem dto)
        {
            if (dto == null) return null;
            var s = new Substrate(dto.Name ?? dto.UniqueKey ?? string.Empty, dto.UniqueKey ?? string.Empty);
            Apply(s, dto);
            return s;
        }

        public static Dictionary<string, string> GetExtraDataFromAttributesAll(Dictionary<string, string> map)
        {
            var extra = new Dictionary<string, string>();
            foreach (var kv in map)
            {
                if (false == _commonKeys.Contains(kv.Key))
                    extra[kv.Key] = kv.Value;
            }

            return extra;
        }
        public static SubstrateItem GetSubstrateDataFromAttributes(Dictionary<string, string> map, out Dictionary<string, string> extra)
        {
            extra = null;

            // Key
            var uniqueKey = Get(map, BaseSubstrateAttributeKeys.UniqueKey).Trim();
            if (string.IsNullOrEmpty(uniqueKey)) return null;

            // 값 추출
            var dto = new SubstrateItem
            {
                UniqueKey = uniqueKey,
                Name = GetOrDefault(map, BaseSubstrateAttributeKeys.Name, uniqueKey).Trim(),
                OriginName = GetOrDefault(map, BaseSubstrateAttributeKeys.OriginName, uniqueKey).Trim(),
                LocationId = Get(map, BaseSubstrateAttributeKeys.Location).Trim(),
                SourcePortId = GetInt(map, BaseSubstrateAttributeKeys.SourcePortId, 0),
                SourceSlot = GetInt(map, BaseSubstrateAttributeKeys.SourceSlot, 0),
                SourceCarrierId = Get(map, BaseSubstrateAttributeKeys.SourceCarrierId).Trim(),
                CurrentCarrierKey = Get(map, BaseSubstrateAttributeKeys.CurrentCarrierKey).Trim(),
                DestinationPortId = GetInt(map, BaseSubstrateAttributeKeys.DestinationPortId, 0),
                DestinationSlot = GetInt(map, BaseSubstrateAttributeKeys.DestinationSlot, 0),
                LotId = Get(map, BaseSubstrateAttributeKeys.LotId).Trim(),
                RecipeId = Get(map, BaseSubstrateAttributeKeys.RecipeId).Trim(),
                ProcessJobId = Get(map, BaseSubstrateAttributeKeys.ProcessJobId).Trim(),
                ControlJobId = Get(map, BaseSubstrateAttributeKeys.ControlJobId).Trim(),

                TransportStatus = (int)GetEnum(map, BaseSubstrateAttributeKeys.TransPortState, default(TransportStates)),
                ProcessingStatus = (int)GetEnum(map, BaseSubstrateAttributeKeys.ProcessingState, default(ProcessingStates)),
                IdReadingStatus = (int)GetEnum(map, BaseSubstrateAttributeKeys.IdReadingState, default(IdReadingStates)),

                DoNotProcessFlag = GetBool(map, BaseSubstrateAttributeKeys.DoNotProcessFlag, false),
                Usage = GetBool(map, BaseSubstrateAttributeKeys.Usage, false),
            };

            if (dto.Extra == null)
                dto.Extra = new Dictionary<string, string>();

            // Extra 수집(코어 키 제외하고 모두)
            extra = GetExtraDataFromAttributesAll(map);
            //dto.Extra = extra;

            return dto;
        }
        public static Dictionary<string, string> ExtractDataAll(Substrate s)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                [BaseSubstrateAttributeKeys.UniqueKey] = s.UniqueKey,
                [BaseSubstrateAttributeKeys.Name] = s.Name,
                [BaseSubstrateAttributeKeys.OriginName] = s.OriginName,
                [BaseSubstrateAttributeKeys.Location] = s.LocationId,
                [BaseSubstrateAttributeKeys.SourcePortId] = s.SourcePortId.ToString(),
                [BaseSubstrateAttributeKeys.SourceSlot] = s.SourceSlot.ToString(),
                [BaseSubstrateAttributeKeys.SourceCarrierId] = s.SourceCarrierId,
                [BaseSubstrateAttributeKeys.CurrentCarrierKey] = s.CurrentCarrierKey,
                [BaseSubstrateAttributeKeys.DestinationPortId] = s.DestinationPortId.ToString(),
                [BaseSubstrateAttributeKeys.DestinationSlot] = s.DestinationSlot.ToString(),
                [BaseSubstrateAttributeKeys.LotId] = s.LotId,
                [BaseSubstrateAttributeKeys.RecipeId] = s.RecipeId,
                [BaseSubstrateAttributeKeys.ProcessJobId] = s.ProcessJobId,
                [BaseSubstrateAttributeKeys.ControlJobId] = s.ControlJobId,
                [BaseSubstrateAttributeKeys.TransPortState] = s.TransportStatus.ToString(),
                [BaseSubstrateAttributeKeys.ProcessingState] = s.ProcessingStatus.ToString(),
                [BaseSubstrateAttributeKeys.IdReadingState] = s.IdReadingStatus.ToString(),
                [BaseSubstrateAttributeKeys.DoNotProcessFlag] = s.DoNotProcessFlag.ToString(),
                [BaseSubstrateAttributeKeys.Usage] = s.Usage.ToString()
            };

            var extra = ExtractExtra(s.Extra);
            if (extra != null )
            {
                foreach (var item in extra)
                {
                    data[item.Key] = item.Value;
                }
            }
            
            return data;
        }
        private static Dictionary<string, string> ExtractExtra(Dictionary<string, string> all)
        {
            if (all == null || all.Count == 0) return null;
            var extra = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var kv in all)
            {
                extra[kv.Key] = kv.Value ?? string.Empty;
            }    
                
            return extra.Count == 0 ? null : extra;
        }

        private static string Get(Dictionary<string, string> map, string key)
        {
            return map.TryGetValue(key, out var v) ? (v ?? string.Empty) : string.Empty;
        }

        private static string GetOrDefault(Dictionary<string, string> map, string key, string @default)
        {
            return map.TryGetValue(key, out var v) ? (v ?? string.Empty) : (@default ?? string.Empty);
        }

        private static int GetInt(Dictionary<string, string> map, string key, int @default = 0)
        {
            var s = Get(map, key);
            return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v) ? v : @default;
        }

        private static bool GetBool(Dictionary<string, string> map, string key, bool @default = false)
        {
            var s = Get(map, key);
            return bool.TryParse(s, out var v) ? v : @default;
        }

        private static T GetEnum<T>(Dictionary<string, string> map, string key, T @default = default(T)) where T : struct
        {
            var s = Get(map, key);
            return Enum.TryParse<T>(s, true, out var v) ? v : @default;
        }

    }
}
