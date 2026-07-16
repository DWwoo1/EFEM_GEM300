using System;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using EFEM.Defines.LoadPort;                    // CarrierAccessStates, CarrierSlotMapStates
using EFEM.Defines.MaterialTracking;            // TransportStates, ProcessingStates, IdReadingStates
using EFEM.CustomizedByProcessType.PWA500Common; // StepsBeforeSendingCarrier

namespace EFEM.MaterialTracking
{
    /// <summary>
    /// 1회 레거시 복구데이터 변환기.
    ///
    /// 배경: 과거 버전은 열거형을 정수(ordinal)로 JSON에 저장했다. 신버전은 이름 문자열로 저장/읽기하며
    /// 정수 토큰을 조용히 해석하지 않는다(AllowIntegerValues=false). 따라서 기존 정수 복구 파일을
    /// 정상 로드하려면 최초 기동 시 1회 "정수 → 이름" 변환 후 이름 형식으로 재기록해야 한다.
    ///
    /// 핵심(scheme-aware): CarrierAccessStates 는 5.18↔6.18 사이에 ordinal 이 1 밀렸다(5.18: Unknown=0 존재).
    /// 그래서 캐리어 레코드는 Extra 키 지문으로 출처를 판정한다 — Extra 에 "KeyLotQty" 가 있으면 5.18(Unknown=0),
    /// "LotQty" 면 6.18. 그 외 열거형(Transport/Processing/IdReading/SlotMap)은 ordinal 이 안정적이라 직접 변환한다.
    ///
    /// 변환 완료 시 폴더에 스탬프(_format.json)를 남겨 재실행 시 건너뛴다(멱등).
    /// </summary>
    public static class LegacyRecoveryConverter
    {
        // 1 = legacy(정수 저장), 2 = 이름 저장(6개 DTO enum), 3 = ProcessStepBeforeSendingCarrier/BinUnloadingStep 추가.
        // 버전을 올릴 때마다 이미 2로 스탬프된 폴더도 다시 훑도록(IsStamped 비교) 해서 신규 변환 대상을 놓치지 않는다.
        public const int CurrentFormatVersion = 3;
        // 복구 폴더에 남기는 포맷 스탬프 파일명. 로드 루프는 이 파일을 자재로 오인하지 않도록 건너뛴다.
        public const string StampFileName = "_format.json";

        /// <param name="mapUnloadingStep">
        /// Substrate.Extra의 BinUnloadingStep(정수)을 이름으로 매핑하는 함수. BIN/W가 서로 다른 enum
        /// (UnloadingStepTypesFor500BIN/500W)을 쓰므로, 이 클래스는 제품을 모르는 채로 호출부(Initializer)가
        /// 현재 실행 중인 EN_PROCESS_TYPE에 맞는 enum으로 매핑해 넘긴다.
        /// </param>
        public static void EnsureConverted(string carrierDir, string substrateDir, Func<int, string> mapUnloadingStep, Action<string> log)
        {
            ConvertDir(carrierDir, ConvertCarrierFile, "carrier", log);
            ConvertDir(substrateDir, obj => ConvertSubstrateFile(obj, mapUnloadingStep), "substrate", log);
        }

        /// <summary>
        /// SubstrateLocationHistory(JSONL append-log, {key}.chg.jsonl)의 레거시 정수
        /// FromLocationKind/ToLocationKind 를 이름으로 변환한다. 이 저장소는 "한 파일 = 한 JSON 객체"가 아니라
        /// "한 줄 = 한 JSON 객체"인 append-log라 ConvertDir/JObject 전체 파싱 모델과 다르게 줄 단위로 처리한다.
        /// ModuleType 은 ordinal 이 안정적이므로 지문 판정 없이 직접 매핑한다.
        /// </summary>
        public static void EnsureLocationHistoryConverted(string dir, Action<string> log)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir))
                    return;

                var stampPath = Path.Combine(dir, StampFileName);
                if (IsStamped(stampPath))
                    return;

                int converted = 0, failed = 0;
                foreach (var file in Directory.GetFiles(dir, "*.chg.jsonl"))
                {
                    try
                    {
                        if (ConvertLocationHistoryJsonlFile(file))
                            converted++;
                    }
                    catch (Exception ex)
                    {
                        failed++;
                        log?.Invoke($"[LegacyConvert] location-history file failed: {file} : {ex.Message}");
                    }
                }

                WriteStamp(stampPath);
                log?.Invoke($"[LegacyConvert] location-history dir done. converted={converted}, failed={failed}, dir={dir}");
            }
            catch (Exception ex)
            {
                log?.Invoke($"[LegacyConvert] location-history dir error: {ex}");
            }
        }

        private static bool ConvertLocationHistoryJsonlFile(string path)
        {
            var lines = File.ReadAllLines(path);
            bool anyChanged = false;

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                JObject obj;
                try
                {
                    obj = JObject.Parse(lines[i]);
                }
                catch
                {
                    // 손상된 줄은 건드리지 않는다(런타임 읽기 쪽에서 스킵 처리됨).
                    continue;
                }

                bool lineChanged = false;
                lineChanged |= ConvertModuleTypeField(obj, "FromLocationKind");
                lineChanged |= ConvertModuleTypeField(obj, "ToLocationKind");

                if (lineChanged)
                {
                    lines[i] = obj.ToString(Formatting.None);
                    anyChanged = true;
                }
            }

            if (anyChanged)
            {
                var content = lines.Length > 0
                    ? string.Join("\n", lines) + "\n"
                    : string.Empty;
                AtomicWrite(path, content);
            }

            return anyChanged;
        }

        private static bool ConvertModuleTypeField(JObject obj, string field)
        {
            var tok = obj[field];
            if (tok == null || tok.Type != JTokenType.Integer)
                return false;

            obj[field] = MapOrdinal<EFEM.Defines.Common.ModuleType>(tok.Value<int>(), EFEM.Defines.Common.ModuleType.Unknown);
            return true;
        }

        private static void ConvertDir(string dir, Func<JObject, bool> convert, string label, Action<string> log)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir))
                    return;

                var stampPath = Path.Combine(dir, StampFileName);
                if (IsStamped(stampPath))
                    return;

                int converted = 0, failed = 0;
                foreach (var file in Directory.GetFiles(dir, "*.json"))
                {
                    var name = Path.GetFileName(file);
                    if (string.Equals(name, StampFileName, StringComparison.OrdinalIgnoreCase))
                        continue;
                    if (name.IndexOf(".bak", StringComparison.OrdinalIgnoreCase) >= 0)
                        continue;

                    try
                    {
                        var obj = JObject.Parse(File.ReadAllText(file));
                        if (convert(obj))
                        {
                            AtomicWrite(file, obj.ToString(Formatting.Indented));
                            converted++;
                        }
                    }
                    catch (Exception ex)
                    {
                        failed++;
                        log?.Invoke($"[LegacyConvert] {label} file failed: {file} : {ex.Message}");
                    }
                }

                WriteStamp(stampPath);
                log?.Invoke($"[LegacyConvert] {label} dir done. converted={converted}, failed={failed}, dir={dir}");
            }
            catch (Exception ex)
            {
                log?.Invoke($"[LegacyConvert] {label} dir error: {ex}");
            }
        }

        #region <Carrier>
        private static bool ConvertCarrierFile(JObject obj)
        {
            bool changed = false;

            var extra = obj["Extra"] as JObject;
            // 출처 판정: KeyLotQty(구) 존재 && LotQty(신) 부재 => 5.18(Unknown=0 체계)
            bool is518 = extra != null && extra["KeyLotQty"] != null && extra["LotQty"] == null;

            // AccessStatus: 정수면 이름으로 (scheme-aware)
            var acc = obj["AccessStatus"];
            if (acc != null && acc.Type == JTokenType.Integer)
            {
                obj["AccessStatus"] = MapCarrierAccess(acc.Value<int>(), is518);
                changed = true;
            }

            // SlotMaps: {slotNo: 정수} => {slotNo: 이름} (ordinal 안정)
            var slotMaps = obj["SlotMaps"] as JObject;
            if (slotMaps != null)
            {
                foreach (var prop in slotMaps.Properties().ToList())
                {
                    if (prop.Value != null && prop.Value.Type == JTokenType.Integer)
                    {
                        prop.Value = MapOrdinal<CarrierSlotMapStates>(prop.Value.Value<int>(), CarrierSlotMapStates.Undefined);
                        changed = true;
                    }
                }
            }

            // Extra 키 정규화: KeyLotQty -> LotQty
            if (extra != null && extra["KeyLotQty"] != null && extra["LotQty"] == null)
            {
                extra["LotQty"] = extra["KeyLotQty"];
                extra.Remove("KeyLotQty");
                changed = true;
            }

            // ProcessStepBeforeSendingCarrier: 과거 MovingAdsCompleted 삽입 이력(커밋 9a22ef2, 2026-06-25)이 있으나
            // 삽입 위치상 오독 방향이 항상 "완료->미완료"(재작업만 유발, 안전)이므로 지문 판정 없이 현재 enum으로 직접 매핑.
            // 이 값은 Extra(Dictionary<string,string>) 안에 있어 JSON에서 항상 문자열("3")로 직렬화된다(정수 토큰이 아님).
            if (extra != null)
            {
                var stepTok = extra["ProcessStepBeforeSendingCarrier"];
                if (stepTok != null && stepTok.Type == JTokenType.String && int.TryParse(stepTok.Value<string>(), out var stepInt))
                {
                    extra["ProcessStepBeforeSendingCarrier"] = MapOrdinal<StepsBeforeSendingCarrier>(stepInt, StepsBeforeSendingCarrier.Init);
                    changed = true;
                }
            }

            return changed;
        }

        private static string MapCarrierAccess(int v, bool is518)
        {
            if (is518)
            {
                // 5.18: Unknown=0, NotAccessed=1, InAccessed=2, CarrierCompleted=3, CarrierStopped=4
                switch (v)
                {
                    case 0: return CarrierAccessStates.NotAccessed.ToString();  // Unknown -> 안전(비종료)
                    case 1: return CarrierAccessStates.NotAccessed.ToString();
                    case 2: return CarrierAccessStates.InAccessed.ToString();
                    case 3: return CarrierAccessStates.CarrierCompleted.ToString();
                    case 4: return CarrierAccessStates.CarrierStopped.ToString();
                    default: return CarrierAccessStates.InAccessed.ToString();   // 미지 -> 안전(비종료)
                }
            }

            // 6.18: NotAccessed=0, InAccessed=1, CarrierCompleted=2, CarrierStopped=3
            return MapOrdinal<CarrierAccessStates>(v, CarrierAccessStates.InAccessed);
        }
        #endregion </Carrier>

        #region <Substrate>
        private static bool ConvertSubstrateFile(JObject obj, Func<int, string> mapUnloadingStep)
        {
            bool changed = false;
            // 이 열거형들은 ordinal 이 5.18↔6.18 안정적이므로 직접 변환
            changed |= ConvertIntEnumField<TransportStates>(obj, "TransportStatus", TransportStates.AtSource);
            changed |= ConvertIntEnumField<ProcessingStates>(obj, "ProcessingStatus", ProcessingStates.NeedsProcessing);
            changed |= ConvertIntEnumField<IdReadingStates>(obj, "IdReadingStatus", IdReadingStates.NotConfirmed);

            // BinUnloadingStep(Extra, 제품별 enum): 제품 정보가 없는 이 클래스 대신 호출부가 넘긴 매퍼로 변환.
            // 이 값도 Extra(Dictionary<string,string>) 안에 있어 JSON에서 항상 문자열("2")로 직렬화된다.
            var extra = obj["Extra"] as JObject;
            if (extra != null && mapUnloadingStep != null)
            {
                var stepTok = extra["BinUnloadingStep"];
                if (stepTok != null && stepTok.Type == JTokenType.String && int.TryParse(stepTok.Value<string>(), out var stepInt))
                {
                    extra["BinUnloadingStep"] = mapUnloadingStep(stepInt);
                    changed = true;
                }
            }

            return changed;
        }
        #endregion </Substrate>

        #region <Helpers>
        private static bool ConvertIntEnumField<T>(JObject obj, string field, T safe) where T : struct
        {
            var tok = obj[field];
            if (tok == null || tok.Type != JTokenType.Integer)
                return false;

            obj[field] = MapOrdinal<T>(tok.Value<int>(), safe);
            return true;
        }

        private static string MapOrdinal<T>(int v, T safe) where T : struct
        {
            if (Enum.IsDefined(typeof(T), v))
                return ((T)(object)v).ToString();

            return safe.ToString();
        }

        private static bool IsStamped(string stampPath)
        {
            try
            {
                if (!File.Exists(stampPath))
                    return false;

                var obj = JObject.Parse(File.ReadAllText(stampPath));
                var v = obj["FormatVersion"];
                return v != null && v.Type == JTokenType.Integer && v.Value<int>() >= CurrentFormatVersion;
            }
            catch
            {
                return false;
            }
        }

        private static void WriteStamp(string stampPath)
        {
            try
            {
                var obj = new JObject { ["FormatVersion"] = CurrentFormatVersion };
                AtomicWrite(stampPath, obj.ToString(Formatting.Indented));
            }
            catch
            {
                // 스탬프 실패는 치명적이지 않다(다음 기동에 재변환 시도, 변환 자체가 멱등).
            }
        }

        private static void AtomicWrite(string path, string content)
        {
            var tmp = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
            File.WriteAllText(tmp, content, new UTF8Encoding(false));

            if (File.Exists(path))
                File.Replace(tmp, path, null);
            else
                File.Move(tmp, path);
        }
        #endregion </Helpers>
    }
}
