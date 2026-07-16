using System;

namespace EFEM.MaterialTracking
{
    /// <summary>
    /// 열거형 영속화(저장/복원) 공용 헬퍼.
    ///
    /// 정책: 영속화되는 열거형은 "멤버 이름 문자열"로만 저장한다(정수 ordinal 저장 금지).
    /// 이유: 멤버를 재배치/삭제하면 정수 ordinal 의미가 조용히 어긋나(예: CarrierAccessStates에서
    /// Unknown 제거로 InAccessed=2 가 CarrierCompleted=2 로 오독 → 미처리 캐리어 조기 배출 사고),
    /// 이름 저장은 이 부류의 사고에 영구 면역이다.
    ///
    /// 읽기 규칙(중요): 저장된 값이 "정수 문자열"이면 절대 ordinal로 조용히 해석하지 않는다.
    /// 이는 레거시 정수 데이터의 오독(=사고 재현)을 막기 위함이다. 레거시 정수는 반드시
    /// 별도의 scheme-aware 1회 변환기(LegacyEnumScheme)에서 명시적으로 해석해야 한다.
    /// 스테디 스테이트 읽기에서 이름 파싱 실패(정수/미지 토큰 포함)는 안전 기본값 + 로그로 처리한다.
    /// </summary>
    public static class EnumPersistence
    {
        /// <summary>enum → 저장용 이름 문자열.</summary>
        public static string ToName<T>(T value) where T : struct
        {
            return value.ToString();
        }

        /// <summary>
        /// 저장된 문자열을 "이름"으로만 파싱한다. 정수 문자열(레거시 ordinal)이나 미정의 이름은 실패로 간주.
        /// </summary>
        public static bool TryParseName<T>(string s, out T value) where T : struct
        {
            value = default(T);

            if (!typeof(T).IsEnum)
                return false;

            if (string.IsNullOrWhiteSpace(s))
                return false;

            var token = s.Trim();

            // 정수 토큰(레거시 ordinal)은 이름이 아니므로 조용히 해석하지 않는다.
            if (int.TryParse(token, out _))
                return false;

            if (!Enum.TryParse<T>(token, /*ignoreCase*/ false, out var parsed))
                return false;

            if (!Enum.IsDefined(typeof(T), parsed))
                return false;

            value = parsed;
            return true;
        }

        /// <summary>
        /// 이름으로 파싱하고, 실패 시 안전 기본값을 반환하며 로그를 남긴다.
        /// safeDefault는 "비가역/위험 동작을 유발하지 않는" 보수적 상태여야 한다
        /// (예: CarrierAccessStates는 CarrierCompleted가 아니라 InAccessed).
        /// </summary>
        public static T ParseNameOrDefault<T>(string s, T safeDefault, Action<string> log = null) where T : struct
        {
            if (TryParseName<T>(s, out var value))
                return value;

            var message = $"[EnumPersistence] Unrecognized {typeof(T).Name} token '{s}'. Falling back to safe default '{safeDefault}'.";
            if (log != null)
                log(message);
            else
                SafeLog(message);

            return safeDefault;
        }

        // 호출부가 로거를 넘기지 않아도 미지 토큰은 반드시 흔적을 남긴다(조용한 오독 방지).
        private static void SafeLog(string message)
        {
            try
            {
                EFEM.Defines.Common.AsyncLoggerForEfem.Instance.WriteDebugLog(message);
            }
            catch
            {
                // 로깅 실패는 무시(파싱 폴백 자체는 이미 안전).
            }
        }
    }
}
