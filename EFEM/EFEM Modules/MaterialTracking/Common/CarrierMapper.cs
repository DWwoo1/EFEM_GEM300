using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using EFEM.MaterialTracking;
using EFEM.Defines.LoadPort;
using EFEM.Defines.MaterialTracking;

namespace EFEM.MaterialTracking
{
    public static class CarrierMapper
    {
        private const string LP = "LP";
        private const string Carrier = "CARRIER";
        private const char Separator = '_';

        public static string MakeCarrierKey(int portId)
        {
            var now = DateTime.Now;
            return $"{LP}{portId}_{Carrier}_{now.Ticks}";
        }

        public static bool TryGetPortIdByKey(string key, out int portId)
        {
            portId = 0;

            if (string.IsNullOrEmpty(key))
                return false;

            // 1) "LP"로 시작하는지 확인
            if (false == key.StartsWith(LP, StringComparison.Ordinal))
                return false;

            // 2) 첫 번째 '_' 위치 찾기
            int underscoreIndex = key.IndexOf(Separator);
            if (underscoreIndex < 0)
                return false;

            // 3) "LP" 바로 뒤부터 '_' 바로 전까지가 portId 부분이므로 인덱스를 찾아옴
            int numberStart = LP.Length;
            int numberLength = underscoreIndex - numberStart;
            if (numberLength <= 0)
                return false;

            // 4) 문자열을 자르고
            string numberPart = key.Substring(numberStart, numberLength);

            // 5) int로 파싱
            return int.TryParse(numberPart, out portId);
        }

        public static CarrierItem ToData(Carrier s)
        {
            if (s == null) return null;

            var dto = new CarrierItem
            {
                UniqueKey = s.UniqueKey ?? string.Empty,
                LotId = s.LotId ?? string.Empty,
                CarrierId = s.CarrierId ?? s.UniqueKey ?? string.Empty,
                AccessStatus = (int)s.AccessingStatus,
                PortId = s.PortId,
                Capacity = s.Capacity,
                LoadTime = s.LoadTime.ToString(ETC.DateTimeFormat),
                UnloadTime = s.LoadTime.ToString(ETC.DateTimeFormat),
                SlotMaps = ExtractSlotMaps(s.SlotMaps),
                Extra = ExtractExtra(s.Extra),
            };

            return dto;
        }

        public static void Apply(Carrier target, CarrierItem dto)
        {
            if (target == null || dto == null) return;

            if (false == string.Equals(target.UniqueKey, dto.UniqueKey, StringComparison.Ordinal))
                target.UniqueKey = dto.UniqueKey ?? string.Empty;

            target.LotId = dto.LotId;
            target.CarrierId = dto.CarrierId;
            target.AccessingStatus = (CarrierAccessStates)dto.AccessStatus;
            target.Capacity = dto.Capacity;

            DateTime.TryParse(dto.LoadTime, out var loadTime);
            target.LoadTime = loadTime;

            DateTime.TryParse(dto.LoadTime, out var unloadTime);
            target.UnloadTime = unloadTime;

            Dictionary<int, CarrierSlotMapStates> maps = new Dictionary<int, CarrierSlotMapStates>();
            if (dto.SlotMaps != null)
            {
                foreach (var item in dto.SlotMaps)
                {
                    maps[item.Key] = (CarrierSlotMapStates)item.Value;
                }
            }
            target.SetSlotMaps(maps);

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

        public static Carrier ToDomain(CarrierItem dto)
        {
            if (dto == null) return null;

            var key = dto.UniqueKey;
            int portId = dto.PortId;
            if (string.IsNullOrWhiteSpace(key))
            {
                key = MakeCarrierKey(portId);
            }

            var c = new Carrier(key, portId);
            Apply(c, dto);

            return c;
        }

        private static Dictionary<int, int> ExtractSlotMaps(IReadOnlyDictionary<int, CarrierSlotMapStates> maps)
        {
            if (maps == null || maps.Count == 0) return null;
            var slotMaps = new Dictionary<int, int>();
            foreach (var kv in maps)
            {
                slotMaps[kv.Key] = (int)kv.Value;
            }

            return slotMaps;
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

        //private static string GetOrDefault(Dictionary<string, string> map, string key, string @default)
        //{
        //    return map.TryGetValue(key, out var v) ? (v ?? string.Empty) : (@default ?? string.Empty);
        //}

        //private static int GetInt(Dictionary<string, string> map, string key, int @default = 0)
        //{
        //    var s = Get(map, key);
        //    return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v) ? v : @default;
        //}

        //private static bool GetBool(Dictionary<string, string> map, string key, bool @default = false)
        //{
        //    var s = Get(map, key);
        //    return bool.TryParse(s, out var v) ? v : @default;
        //}

        //private static T GetEnum<T>(Dictionary<string, string> map, string key, T @default = default(T)) where T : struct
        //{
        //    var s = Get(map, key);
        //    return Enum.TryParse<T>(s, true, out var v) ? v : @default;
        //}
    }
}
