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

        // 코어(Base) 캐리어 키 셋 — Extra 분리용
        private static readonly HashSet<string> _commonKeys = new HashSet<string>(StringComparer.Ordinal)
        {
            BaseCarrierAttributeKeys.UniqueKey,
            BaseCarrierAttributeKeys.LotId,
            BaseCarrierAttributeKeys.CarrierId,
            BaseCarrierAttributeKeys.CarrierAccessStatus,
            BaseCarrierAttributeKeys.LoadTime,
            BaseCarrierAttributeKeys.UnloadTime,
        };

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
                AccessStatus = s.AccessingStatus,
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
            target.AccessingStatus = dto.AccessStatus;
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
                    maps[item.Key] = item.Value;
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

        private static Dictionary<int, CarrierSlotMapStates> ExtractSlotMaps(IReadOnlyDictionary<int, CarrierSlotMapStates> maps)
        {
            if (maps == null || maps.Count == 0) return null;
            var slotMaps = new Dictionary<int, CarrierSlotMapStates>();
            foreach (var kv in maps)
            {
                slotMaps[kv.Key] = kv.Value;
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

        // ─────────────────────────────────────────────────────────────
        // 문자열 왕복(편집 UI용) — SubstrateMapper 미러
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// 캐리어의 Base 키 + Extra 를 하나의 문자열 맵으로 평탄화한다(편집 폼 입력용).
        /// </summary>
        public static Dictionary<string, string> ExtractDataAll(Carrier c)
        {
            if (c == null) return new Dictionary<string, string>();

            var data = new Dictionary<string, string>
            {
                [BaseCarrierAttributeKeys.UniqueKey]           = c.UniqueKey ?? string.Empty,
                [BaseCarrierAttributeKeys.CarrierId]           = c.CarrierId ?? string.Empty,
                [BaseCarrierAttributeKeys.LotId]               = c.LotId ?? string.Empty,
                [BaseCarrierAttributeKeys.CarrierAccessStatus] = c.AccessingStatus.ToString(),
                [BaseCarrierAttributeKeys.LoadTime]            = c.LoadTime.ToString(BaseCarrierAttributeKeys.DateTimeFormat),
                [BaseCarrierAttributeKeys.UnloadTime]          = c.UnloadTime.ToString(BaseCarrierAttributeKeys.DateTimeFormat),
            };

            var extra = ExtractExtra(c.Extra);
            if (extra != null)
            {
                foreach (var kv in extra)
                    data[kv.Key] = kv.Value;
            }

            return data;
        }

        /// <summary>
        /// 편집 폼이 돌려준 문자열 맵을 파싱해 Base 값(CarrierItem)과 Extra(out)로 나눈다.
        /// UniqueKey 가 비어 있으면 null 을 반환한다.
        /// </summary>
        public static CarrierItem GetCarrierDataFromAttributes(Dictionary<string, string> map, out Dictionary<string, string> extra)
        {
            extra = null;
            if (map == null) return null;

            var uniqueKey = Get(map, BaseCarrierAttributeKeys.UniqueKey).Trim();
            if (string.IsNullOrEmpty(uniqueKey)) return null;

            var dto = new CarrierItem
            {
                UniqueKey    = uniqueKey,
                CarrierId    = Get(map, BaseCarrierAttributeKeys.CarrierId).Trim(),
                LotId        = Get(map, BaseCarrierAttributeKeys.LotId).Trim(),
                AccessStatus = GetEnum(map, BaseCarrierAttributeKeys.CarrierAccessStatus, default(CarrierAccessStates)),
                LoadTime     = Get(map, BaseCarrierAttributeKeys.LoadTime),
                UnloadTime   = Get(map, BaseCarrierAttributeKeys.UnloadTime),
            };

            extra = GetExtraDataFromAttributesAll(map);
            return dto;
        }

        public static Dictionary<string, string> GetExtraDataFromAttributesAll(Dictionary<string, string> map)
        {
            var extra = new Dictionary<string, string>();
            if (map == null) return extra;

            foreach (var kv in map)
            {
                if (false == _commonKeys.Contains(kv.Key))
                    extra[kv.Key] = kv.Value;
            }

            return extra;
        }

        private static string Get(Dictionary<string, string> map, string key)
        {
            return map.TryGetValue(key, out var v) ? (v ?? string.Empty) : string.Empty;
        }

        private static T GetEnum<T>(Dictionary<string, string> map, string key, T @default = default(T)) where T : struct
        {
            var s = Get(map, key);
            return EnumPersistence.ParseNameOrDefault(s, @default);
        }
    }
}
