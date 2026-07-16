using System;

namespace EFEM.History
{
    /// <summary>
    /// 2026.07.06. jhlim [ADD] 이력 이벤트 1건의 저장소 독립 모델.
    ///
    /// 표시 이름(CarrierId/SubstrateName)과 불변 키(CarrierKey/SubstrateKey)를 함께 운반한다:
    /// - 파일 저장소는 운영자 열람성을 위해 표시 이름으로 파일을 만들고 키는 사용하지 않는다.
    /// - DB 저장소(도입 예정)는 불변 키를 키 컬럼으로 사용한다.
    ///   (CarrierKey = 캐리어 방문 단위 키, SubstrateKey = 기판 생성 단위 키 - 개명/재작업과 무관)
    /// 키는 해석 실패 시 빈 문자열일 수 있으며, 저장소는 빈 키를 허용해야 한다.
    /// </summary>
    public sealed class HistoryRecord
    {
        public DateTime Time;                               // 기록 요청 시점 (라인 타임스탬프의 근원)
        public int PortId;
        public string Category = string.Empty;              // 기판 분류 (예: Core/Bin - 파일 저장소의 폴더명)
        public string CarrierKey = string.Empty;
        public string CarrierId = string.Empty;
        public string LotId = string.Empty;         // 기록 시점에는 비어 있고 랏 확정(CompleteCarrier) 후 조회 결과에서 채워진다
        public string SubstrateKey = string.Empty;
        public string SubstrateName = string.Empty;
        public string CarrierEventCode = string.Empty;
        public string SubstrateEventCode = string.Empty;
        public string Message = string.Empty;
    }

    /// <summary>
    /// 기존 랏 히스토리 라인 포맷의 단일 정의.
    /// "MM/dd-HH:mm:ss.fff \t 캐리어이벤트 \t 기판이름 \t 기판이벤트 \t 메시지"
    /// 화면 표시와 파일 기록이 같은 문자열을 쓰도록 record.Time 기준으로 결정적으로 직렬화한다.
    /// </summary>
    public static class HistoryLineFormat
    {
        public const int FieldCount = 5;

        public static string Compose(HistoryRecord record)
        {
            return string.Format("{0}\t{1}\t{2}\t{3}\t{4}",
                ComposeTimestamp(record.Time),
                record.CarrierEventCode,
                record.SubstrateName,
                record.SubstrateEventCode,
                record.Message);
        }
        public static string ComposeTimestamp(DateTime time)
        {
            return string.Format("{0:d2}/{1:d2}-{2:d2}:{3:d2}:{4:d2}.{5:d3}",
                time.Month,
                time.Day,
                time.Hour,
                time.Minute,
                time.Second,
                time.Millisecond);
        }
        /// <summary>
        /// 2026.07.06. jhlim [ADD] Compose의 역방향 - 파일 라인을 레코드로 복원한다. (조회 계층용)
        /// 라인 타임스탬프에는 연도가 없으므로 조회/폴더 날짜에서 얻은 연도를 힌트로 받는다.
        /// 키 필드(CarrierKey/SubstrateKey)는 라인에 없으므로 빈 값으로 남는다.
        /// </summary>
        public static bool TryParse(string line, int year, out HistoryRecord record)
        {
            record = null;
            if (string.IsNullOrEmpty(line))
                return false;

            var parts = line.Split(new char[] { '\t' }, StringSplitOptions.None);
            if (parts.Length != FieldCount)
                return false;

            // "MM/dd-HH:mm:ss.fff"
            var timestamp = parts[0];
            if (timestamp.Length != 18 || timestamp[2] != '/' || timestamp[5] != '-')
                return false;

            try
            {
                int month = int.Parse(timestamp.Substring(0, 2));
                int day = int.Parse(timestamp.Substring(3, 2));
                int hour = int.Parse(timestamp.Substring(6, 2));
                int minute = int.Parse(timestamp.Substring(9, 2));
                int second = int.Parse(timestamp.Substring(12, 2));
                int millisecond = int.Parse(timestamp.Substring(15, 3));

                record = new HistoryRecord
                {
                    Time = new DateTime(year, month, day, hour, minute, second, millisecond),
                    CarrierEventCode = parts[1],
                    SubstrateName = parts[2],
                    SubstrateEventCode = parts[3],
                    Message = parts[4],
                };
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
