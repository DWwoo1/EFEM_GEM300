using EFEM.Database;

namespace EFEM.MaterialTracking.Inspection
{
    /// <summary>
    /// 2026.07.09. jhlim [ADD] DB 조회 페이지에서 MaterialDatabaseQuery 를 얻기 위한 정적 홀더.
    ///
    /// MaterialDbContext 는 Initializer 가 소유하는 지역 인스턴스라 UI 가 직접 참조하지 못한다.
    /// LotHistoryLog.AttachDatabaseQuery 와 같은 방식으로, 초기화 시 1회 Configure 되고
    /// UI 서브뷰는 Instance/IsAvailable 로 접근한다.
    /// Json 전용(DB 미장착) 구성에서는 Configure 가 호출되지 않아 IsAvailable == false 이며,
    /// 이때 조회 페이지는 "DB 미사용" 안내만 표시한다.
    /// 소유권: 이 홀더는 참조만 보관하고 Dispose/Shutdown 은 Initializer(소유자)에 맡긴다.
    /// </summary>
    public static class MaterialDatabaseQueryProvider
    {
        public static MaterialDatabaseQuery Instance { get; private set; }

        public static bool IsAvailable => Instance != null;

        public static void Configure(MaterialDbContext db)
        {
            if (db == null)
                return;

            Instance = new MaterialDatabaseQuery(db);
        }
    }
}
