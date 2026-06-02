using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TickCounter_;

using Define.DefineEnumBase.Common;
using EFEM.Defines.Communicator;

namespace FrameOfSystem3.ExternalDevice.Serial.FanFilterUnit
{
    public abstract class FanFilterUnitController
    {
        #region <Constructors>
        public FanFilterUnitController(int commIndex, EN_CONNECTION_TYPE commType, bool useDifferentialPressureMode, int unitCount)
        {
            UseDifferentialPressureMode = useDifferentialPressureMode;
            UnitCount = unitCount;
            Communicator = new Communicator(commType, commIndex);
            TicksForConnection.SetTickCount(RetryInterval);
        }
        #endregion </Constructors>

        #region <Fields>
        protected readonly Communicator Communicator = null;

        private readonly TickCounter TicksForConnection = new TickCounter();
        private readonly uint RetryInterval = 5000;
        private const int RetryCountLimit = 3;
        private int _retryCount;
        #endregion </Fields>

        #region <Properties>
        public bool UseDifferentialPressureMode { get; set; }
        public int UnitCount { get; private set; }
        public bool IsConnected
        {
            get
            {
                if (Communicator == null)
                    return false;

                return Communicator.IsConnected;
            }
        }
        #endregion </Properties>

        #region <Methods>
        public abstract bool Connect();
        public abstract bool DisConnect();
        public abstract bool SetSpeedAll(double speed);
        public abstract bool GetInformation(int index, ref UnitItem unitItem);
        public abstract bool GetUnitSpeedAll(ref double[] speed);
        // 2024.12.29. dwlim [ADD] Setting된 Pressure를 Get하는 기능이 Manuaul에 없음 - 업체확인필요
        public abstract bool SetUnitDifferentialPressure(double pressure);
        public abstract bool GetUnitDifferentialPresureAll(ref double[] pressure);     // 차압모드 전용
        public abstract bool GetUnitStatusAll(ref string[] status);
        public abstract void Execute();

        // 2024.12.21. jhlim [ADD] 연결되지 않는 경우 무한으로 익셉션이 발생해 로그가 계속 쌓이게된다..
        private bool RetryConnectIfNeed()
        {
            if (false == IsConnected)
            {
                if (_retryCount < RetryCountLimit && TicksForConnection.IsTickOver(false))
                {
                    Communicator.OpenPort();

                    TicksForConnection.SetTickCount(RetryInterval);
                    ++_retryCount;
                }

                return false;
            }
            else
            {
                // Flag Reset
                _retryCount = 0;
                TicksForConnection.SetTickCount(RetryInterval);

                return true;
            }
        }
        // 2024.12.21. jhlim [END]

        public void Monitoring()
        {
            // 2024.12.21. jhlim [ADD] 연결 체크를 상위에서 한다.
            if (false == RetryConnectIfNeed())
                return;
            // 2024.12.21. jhlim [END]

            Execute();
        }
        #endregion </Methods>
    }
}
