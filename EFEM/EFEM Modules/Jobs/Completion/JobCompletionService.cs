using System;

namespace EFEM.Jobs.Completion
{
    /// <summary>
    /// Job 완료 판정 객체를 전역에서 접근하기 위한 Service.
    ///
    /// 주의:
    /// - 실제 완료 판정 로직은 JobCompletionEvaluator가 담당한다.
    /// - 이 객체는 Evaluator 인스턴스를 보관하고 제공하는 역할만 한다.
    /// - Job 상태 변경, SDK 호출, Repository 갱신은 하지 않는다.
    /// </summary>
    public static class JobCompletionService
    {
        private static readonly object _sync = new object();

        private static IJobCompletionEvaluator _instance;

        /// <summary>
        /// 현재 설정된 Job 완료 판정 객체.
        /// 아직 Configure되지 않았으면 null을 반환한다.
        /// </summary>
        public static IJobCompletionEvaluator Instance
        {
            get
            {
                lock (_sync)
                {
                    return _instance;
                }
            }
        }

        /// <summary>
        /// Job 완료 판정 객체를 등록한다.
        /// 프로그램 초기화 시점에 한 번만 호출하는 것을 원칙으로 한다.
        /// </summary>
        public static void Configure(IJobCompletionEvaluator evaluator)
        {
            if (evaluator == null)
                throw new ArgumentNullException(nameof(evaluator));

            lock (_sync)
            {
                if (_instance != null)
                    throw new InvalidOperationException(
                        "JobCompletionService is already configured.");

                _instance = evaluator;
            }
        }

        /// <summary>
        /// 등록된 Evaluator를 제거한다.
        /// 일반 운전 중에는 호출하지 않고, 테스트/재초기화 용도로만 사용한다.
        /// </summary>
        public static void Clear()
        {
            lock (_sync)
            {
                _instance = null;
            }
        }

        /// <summary>
        /// 등록 여부를 확인한다.
        /// </summary>
        public static bool IsConfigured
        {
            get
            {
                lock (_sync)
                {
                    return _instance != null;
                }
            }
        }
    }
}