using System;
using System.Threading;
using System.Reflection;

using FrameOfSystem3.Controller.Motion.MechatroLinkAPI.References;
using FrameOfSystem3.Controller.Motion.MechatroLinkAPI.MechatroLinkDefines;

namespace FrameOfSystem3.Controller.Motion.MechatroLinkAPI.MechatroLinkAxis
{
    class MechatrolinkServoAxis : MechatrolinkAxis
    {
        #region <Constructor>
        public MechatrolinkServoAxis() { }
        public MechatrolinkServoAxis(iCommunicator pComm, int nTargetIndex, int nAxisIndex, string strName, UInt32 uAxisHandle)
            : base(pComm, nTargetIndex, nAxisIndex, strName)
        {
            AxisHandle = uAxisHandle;
        }
        #endregion </Constructor>

        #region <Fields>

        #region <Constants>
        private const int TIME_OUT_SERVO_ON = 500;
        private const int STOP_TIME = 100;
        private const int ESTOP_TIME = 50;
        #endregion </Constants>

        #region <Private Fields>
        private double m_dTargetPos;
        private UInt32 m_pSlaveHandle;
        #endregion </Private Fields>

        #endregion </Fields>

        #region <Enum>
        private enum HOME_SEQ
        {
            INIT = 0,
            PREPARE_GANTRY = 100,
            MOVE_TO_LIMIT = 200,
            MOVE_TO_ESCAPE = 300,
            HOMING = 400,
            WAIT_COMPLETION = 500,
            END = 1000,
        }

        private enum GANTRY_STATE_TYPE
        {
            MASTER = 0,
            STATE = 1
        }
        #endregion </Enum>

        #region <Methods>

        #region <Inhert Methods>

        #region <Destructor>
        public override void InitAxis()
        {
            UInt32 pRegisterDataHandle = 0;
            UInt32 uAddress = RegisterInputAddress;// +MechatrolinkDefines.INPUT_REGISTER_ADDRESS_MOTOR_USABLE;
            string strRegisterId = String.Format("IB{0:X}3", uAddress);

            UInt32 uCode = CMotionAPI.ymcGetRegisterDataHandle(strRegisterId, ref pRegisterDataHandle);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterDataHandle"))
                return;

            Int16[] arReadData = new Int16[1];
            UInt32 nReadNum = 0;
            uCode = CMotionAPI.ymcGetRegisterData(pRegisterDataHandle, 1, arReadData, ref nReadNum);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return;

            //m_bUsable = arReadData[0] == 0x00 ? false : true;

            if (false == m_bUsable)
            {
                System.Console.WriteLine("Axis {0}, Usable : {1}", AxisIndex.ToString(), m_bUsable.ToString());
            }

            base.InitAxis();
        }

        protected override void DestoryObject(ref UInt32 pAxisHandle)
        {
            ServoControl(ref pAxisHandle, false);

            CMotionAPI.ymcClearAxis(pAxisHandle);
        }
        #endregion </Destructor>

        #region <State>
        protected override double GetActualPosition()
        {
            UInt32 uStoredValue = 0;
            UInt32 uCode = CMotionAPI.ymcGetMotionParameterValue(AxisHandle,
                (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER, (UInt32)CMotionAPI.ApiDefs_MonPrm.SER_APOS,
                ref uStoredValue);

            if (uCode != CMotionAPI.MP_SUCCESS)
            {
                uStoredValue = 0;
            }

            return (Int32)uStoredValue;
        }

        protected override double GetCommandPosition()
        {
            UInt32 uStoredValue = 0;
            UInt32 uCode = CMotionAPI.ymcGetMotionParameterValue(AxisHandle,
                (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER, (UInt32)CMotionAPI.ApiDefs_MonPrm.SER_DPOS,
                ref uStoredValue);

            if (uCode != CMotionAPI.MP_SUCCESS)
            {
                uStoredValue = 0;
            }

            return (Int32)uStoredValue;
        }

        protected override bool GetServoState()
        {
            bool bServoState = false;

            UInt32 uStoredValue = 0;
            UInt32 uCode = CMotionAPI.ymcGetMotionParameterValue(AxisHandle,
                (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER, (UInt32)CMotionAPI.ApiDefs_MonPrm.SER_RUNSTS,
                ref uStoredValue);

            if (uCode == CMotionAPI.MP_SUCCESS)
            {
                UInt32 wStatus = uStoredValue;

                if ((wStatus & MechatrolinkDefines.SERVO_RUNNING) != 0x00)
                {
                    bServoState = true;
                }
                else
                {
                    bServoState = false;
                }
            }

            return bServoState;
        }

        protected override bool GetAlarmState()
        {
            bool bAlarm = true;

            UInt32 uStoredValue = 0;
            UInt32 uCode = CMotionAPI.ymcGetMotionParameterValue(AxisHandle,
                (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER, (UInt32)CMotionAPI.ApiDefs_MonPrm.SER_ALARM,
                ref uStoredValue);
            if (uCode == CMotionAPI.MP_SUCCESS)
            {
                //uStoredValue &= (~(UInt32)0x06);

                if (uStoredValue == 0)
                {
                    // Warning
                    // 워닝도 포함해야하나?? 일단 써보고 추후 필요하면 살리기로 한다.
                    //uCode = CMotionAPI.ymcGetMotionParameterValue(AxisHandle,
                    //    (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER, 2, ref uStoredValue);

                    //if (uCode == CMotionAPI.MP_SUCCESS)
                    //{
                    //    uStoredValue &= (~(UInt32)0xC0);
                    //}
                }

                bAlarm = (uStoredValue != 0) ? true : false;
            }

            return bAlarm;
        }

        protected override void GetHWLimitState(ref bool bLimitPos, ref bool bLimitNeg)
        {
            if (m_stConfigParam == null)
                return;

            UInt32 uStoredValue = 0;
            UInt32 uCode = CMotionAPI.ymcGetMotionParameterValue(AxisHandle,
              (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER, (UInt32)CMotionAPI.ApiDefs_MonPrm.SER_PIMON,
              ref uStoredValue);

            if (uCode == CMotionAPI.MP_SUCCESS)
            {
                if (m_stConfigParam.bUseHWLimitPositive)
                {
                    bLimitPos = ((uStoredValue & MechatrolinkDefines.LIMIT_POS) != 0) ? (true) : (false);
                }
                else
                {
                    bLimitPos = false;
                }

                if (m_stConfigParam.bUseHWLimitNegative)
                {
                    bLimitNeg = ((uStoredValue & MechatrolinkDefines.LIMIT_NEG) != 0) ? (true) : (false);
                }
                else
                {
                    bLimitNeg = false;
                }
            }
        }

        protected override bool GetMotionCompletion()
        {
            if (0 == AxisHandle)
                return false;

            UInt32 uStoredValue = 0;
            UInt32 uCode = CMotionAPI.ymcGetMotionParameterValue(AxisHandle,
                (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER, (UInt32)CMotionAPI.ApiDefs_MonPrm.SER_PIMON,
                ref uStoredValue);

            if (uCode == CMotionAPI.MP_SUCCESS)
            {
                //MotionDone = (uStoredValue != 0) ? (false) : (true);
                //m_bMotionDone = ((uStoredValue != 0) ? false : true);

                return ((uStoredValue & 0x4000) != 0) ? (true) : (false);
            }

            return false;
        }

        protected override bool GetGearedState(UInt32[] arAxes)
        {
            //if (false == base.IsGantrySettingComplete())
            //    return false;

            return base.IsGantrySettingComplete();
            //GetExecutingCommand();
            
            // 요고 슬레이브는 축 번호 받게 수정해야함
            //// Gantry Master 축 번호 비교
            //int nMasterIndex = GetGantryStateFromRegister(arAxes, (int)GANTRY_STATE_TYPE.MASTER);
            //if (MasterAxis != nMasterIndex - 1)     // 논리 축 번호
            //    return false;

            //// Gantry 상태
            //int nState = GetGantryStateFromRegister(arAxes, (int)GANTRY_STATE_TYPE.STATE);
            //switch (nState)
            //{
            //    case (int)CMotionAPI.ApiDefs.GEAR_ENGAGED:
            //        return true;
            //    default:
            //        return false;
            //}
        }
        #endregion </State>

        #region <Axis Commands>
        protected override bool ServoControl(ref UInt32 pAxisHandle, bool bEnabled)
        {
            //if (false == GetHandle(AxisIndex, ref pAxisHandle))
            //{
            //    // 보드를 열고
            //    if (false == m_pCommunicator.OpenApi(true))
            //        return;

            //    if (false == DeclareAxis(AxisIndex, ref pAxisHandle))
            //        return;
            //}

            UInt32 pDeviceHandle = 0;
            UInt32 uCode = CMotionAPI.ymcDeclareDevice(1, new UInt32[] { pAxisHandle }, ref pDeviceHandle);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDeclareDevice"))
                return false;

            UInt16 uEnabled = bEnabled ? (UInt16)CMotionAPI.ApiDefs.SERVO_ON : (UInt16)CMotionAPI.ApiDefs.SERVO_OFF;
            uCode = CMotionAPI.ymcServoControl(pDeviceHandle, uEnabled, TIME_OUT_SERVO_ON);

            // 에러 발생 시 로그만 찍기 위함
            bool bResult = m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcServoControl");

            InitMotionFlags();

            if (false == bEnabled)
                HomeCompletion = false;

            uCode = CMotionAPI.ymcClearDevice(pDeviceHandle);
            bResult &= m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcClearDevice");

            //if (bEnabled && m_pSlaveAxis != null)
            //{
            //    if (bResult && false == GantryState)
            //    {
            //        int[] arAxes = new int[m_arGearedAxes.Length];
            //        for (int i = 0; i < m_arGearedAxes.Length; ++i )
            //        {
            //            arAxes[i] = (int)m_arGearedAxes[i];
            //        }

            //        m_pCommunicator.RequestEnableGantry(arAxes);
            //    }
            //}
            return bResult;
        }

        protected override bool ClearAlarm(ref UInt32 pAxisHandle)
        {
            //if (false == GetHandle(AxisIndex, ref pAxisHandle))
            //{
            //    // 보드를 열고
            //    if (false == m_pCommunicator.OpenApi(true))
            //        return;

            //    if (false == DeclareAxis(AxisIndex, ref pAxisHandle))
            //        return;
            //}

            UInt32 uCode = CMotionAPI.ymcClearServoAlarm(pAxisHandle);

            // Warning은 출력할 필요 없다.
            if (CMotionAPI.ALM_MK_MDWARNING == uCode)
            {
                return true;
            }
            else
            {
                return m_pCommunicator.CheckingApi(uCode,
                                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcClearServoAlarm");
            }
        }

        protected override bool DefinePosition(ref UInt32 pAxisHandle, double dPosition)
        {
            //if (false == GetHandle(AxisIndex, ref pAxisHandle))
            //{
            //    // 보드를 열고
            //    if (false == m_pCommunicator.OpenApi(true))
            //        return;

            //    if (false == DeclareAxis(AxisIndex, ref pAxisHandle))
            //        return;
            //}


            UInt32 uCode = 0;
            UInt32 pDeviceHandle = 0;
            int nCount = 1;
            if (0 != m_pSlaveHandle)
            {
                nCount = 2;
                uCode = CMotionAPI.ymcDeclareDevice(2, new UInt32[] { pAxisHandle, m_pSlaveHandle }, ref pDeviceHandle);
            }
            else
            {
                uCode = CMotionAPI.ymcDeclareDevice(1, new UInt32[] { pAxisHandle }, ref pDeviceHandle);
            }
            
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDeclareDevice"))
                return false;

            //UInt32 pDeviceHandle = 0;
            //UInt32 uCode = CMotionAPI.ymcDeclareDevice(1, new UInt32[] { pAxisHandle }, ref pDeviceHandle);
            //if (false == m_pCommunicator.CheckingApi(uCode,
            //    MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDeclareDevice"))
            //    return false;
            CMotionAPI.POSITION_DATA[] stPosData = new CMotionAPI.POSITION_DATA[nCount];
            for(int i = 0; i < nCount; ++i)
            {
                stPosData[i].DataType = (int)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE;
                stPosData[i].PositionData = (Int32)dPosition;
            }


            uCode = CMotionAPI.ymcDefinePosition(pDeviceHandle, stPosData);

            // 에러 발생 시 로그만 찍기 위함
            bool bResult = m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDefinePosition");

            uCode = CMotionAPI.ymcClearDevice(pDeviceHandle);
            bResult &= m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcClearDevice");

            return bResult;
        }

        protected override void SetGearedAxis(ref UInt32[] arGantryHandles, UInt32[] arAxes, bool bEnabled)
        {
            if (bEnabled)
            {
                m_pSlaveHandle = arGantryHandles[1];
            }
            else
            {
                m_pSlaveHandle = 0;
            }
            return;

            //// 갠트리 Set/Reset
            //int nCount = arAxes.Length;
            //UInt32 pDeviceHandle = 0;

            ////if (false == GetHandle(arGantryHandles[1], ref m_pSlaveHandle))
            ////{
            ////     if (false == m_pCommunicator.OpenApi(true))
            ////         return;

            ////     if (false == DeclareAxis(nSlaveAxisIndex[1], ref m_pSlaveHandle))
            ////         return;
            ////}

            ////int[] arLogicalAxes = new int[nCount];
            ////for (int i = 0; i < nCount; ++i )
            ////{
            ////    arLogicalAxes[i] = arAxes[i] + 1;

            ////    if (false == GetHandle(arLogicalAxes[i], ref arAxisHandle[i]))
            ////    {
            ////        // 보드를 열고
            ////        if (false == m_pCommunicator.OpenApi(true))
            ////            return;

            ////        if (false == DeclareAxis(arLogicalAxes[i], ref arAxisHandle[i]))
            ////            return;
            ////    }

            ////}

            //UInt32 uCode = CMotionAPI.ymcDeclareDevice(1, new UInt32[] { arGantryHandles[1] }, ref pDeviceHandle);
            //if (false == m_pCommunicator.CheckingApi(uCode,
            //    MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDeclareDevice"))
            //    return;

            //bool bResult = true;

            //if (bEnabled)
            //{
            //    CMotionAPI.GEAR_RATIO_DATA[] pGearRatio = new CMotionAPI.GEAR_RATIO_DATA[1];
            //    pGearRatio[0].Master = 1;
            //    pGearRatio[0].Slave = 1;
            //    uCode = CMotionAPI.ymcSetGearRatio(pDeviceHandle, pGearRatio, 0);

            //    if (uCode != CMotionAPI.ALM_MK_ALREADY_COMMANDED)
            //    {
            //        // 에러 발생 시 로그만 찍기 위함
            //        bResult = m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcSetGearRatio");
            //    }
            //}

            //CMotionAPI.SYNC_DISTANCE[] pSyncDistance = new CMotionAPI.SYNC_DISTANCE[1];
            //pSyncDistance[0].SyncType = (UInt16)CMotionAPI.ApiDefs.SYNCH_TIME;
            //pSyncDistance[0].DataType = (UInt16)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE;
            //pSyncDistance[0].DistanceData = 1000;
            //UInt16[] arWaitForCompletion = new UInt16[] //{ (UInt16)CMotionAPI.ApiDefs.GEAR_COMMAND_STARTED };
            // { (UInt16)CMotionAPI.ApiDefs.GEAR_ENGAGE_COMPLETED };

            //UInt32[] pStatusHandle = new UInt32[1];
            //string strRegisterId = GetGantryRegisterName(arAxes, (int)GANTRY_STATE_TYPE.MASTER);
            //uCode = CMotionAPI.ymcGetRegisterDataHandle(strRegisterId, ref pStatusHandle[0]);

            //// 에러 발생 시 로그만 찍기 위함
            //bResult &= m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterDataHandle");

            //if (bResult && bEnabled)
            //{
            //    uCode = CMotionAPI.ymcEnableGear(arGantryHandles[0], pDeviceHandle, (UInt32)CMotionAPI.ApiDefs.MASTER_AXIS_COMMAND, pSyncDistance,
            //                  pStatusHandle, "", arWaitForCompletion, 0);

            //    if (uCode != CMotionAPI.ALM_MK_ALREADY_COMMANDED)
            //    {
            //        // 에러 발생 시 로그만 찍기 위함
            //        bResult &= m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcEnableGear");
            //    }
            //}
            //else
            //{
            //    uCode = CMotionAPI.ymcDisableGear(arGantryHandles[0], pDeviceHandle, (UInt32)CMotionAPI.ApiDefs.MASTER_AXIS_COMMAND, pSyncDistance, "", arWaitForCompletion, 0);

            //    // 에러 발생 시 로그만 찍기 위함
            //    bResult &= m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDisableGear");
            //}

            //uCode = CMotionAPI.ymcClearDevice(pDeviceHandle);
            //m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcClearDevice");
        }
        #endregion </Axis Commands>

        #region <Motion Commands>
        protected override bool StopMotion(ref UInt32 pAxisHandle, bool bEmergency)
        {
            //if (false == GetHandle(AxisIndex, ref pAxisHandle))
            //{
            //    // 보드를 열고
            //    if (false == m_pCommunicator.OpenApi(true))
            //        return;

            //    if (false == DeclareAxis(AxisIndex, ref pAxisHandle))
            //        return;
            //}
            bool bResult = true;
            if (ServoState)
            {
                UInt32 uCode = 0;
                UInt32 pDeviceHandle = 0;
                if (0 != m_pSlaveHandle)
                {
                    uCode = CMotionAPI.ymcDeclareDevice(2, new UInt32[] { pAxisHandle, m_pSlaveHandle }, ref pDeviceHandle);
                }
                else
                {
                    uCode = CMotionAPI.ymcDeclareDevice(1, new UInt32[] { pAxisHandle }, ref pDeviceHandle);
                }
                if (false == m_pCommunicator.CheckingApi(uCode,
                    MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDeclareDevice"))
                    return false;

                ///
                string strDetail = String.Empty;
                uCode = StopDriving(pDeviceHandle, bEmergency, ref strDetail);

                // 에러 발생 시 로그만 찍기 위함
                bResult = m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, strDetail);

                uCode = StopJog(pDeviceHandle, ref strDetail);

                // 에러 발생 시 로그만 찍기 위함
                bResult &= m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, strDetail);


                uCode = CMotionAPI.ymcClearDevice(pDeviceHandle);

                bResult &= m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcClearDevice");
            }

            m_bExecutingMotion = false;

            return bResult;
        }

        private UInt32 StopDriving(UInt32 pDeviceHandle, bool bEmergency, ref string strDetail)
        {
            int nCount = 1;
            if (0 != m_pSlaveHandle)
                nCount = 2;
            
            CMotionAPI.MOTION_DATA[] arMotionData = new CMotionAPI.MOTION_DATA[nCount];
            UInt16[] arWaitForCompletion = new UInt16[nCount];
            for (int i = 0; i < nCount; ++i )
            {
                arMotionData[i].AccDecType = (Int16)CMotionAPI.ApiDefs.ATYPE_TIME;
                arMotionData[i].DataType = 0;
                arMotionData[i].Deceleration = bEmergency ? ESTOP_TIME : STOP_TIME;

                arWaitForCompletion[i] = (UInt16)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED;
            }

            //UInt16[] arWaitForCompletion = new UInt16[] { (UInt16)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED };

            UInt32 uCode = CMotionAPI.ymcStopMotion(pDeviceHandle, arMotionData, null, arWaitForCompletion, 0);
            if (uCode != CMotionAPI.MP_SUCCESS)
            {
                strDetail = "After ymcStopMotion";
            }

            return uCode;
        }

        private UInt32 StopJog(UInt32 pDeviceHandle, ref string strDetail)
        {
            int nCount = 1;
            if (0 != m_pSlaveHandle)
                nCount = 2;

            UInt16[] arWaitForCompletion = new UInt16[nCount];
            for (int i = 0; i < nCount; ++i )
            {
                arWaitForCompletion[i] = (UInt16)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED;
            }
            //{ (UInt16)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED };

            UInt32 uCode = CMotionAPI.ymcStopJOG(pDeviceHandle, 0, null, arWaitForCompletion, 0);
            if (uCode != CMotionAPI.MP_SUCCESS)
            {
                strDetail = "After ymcStopJOG";
            }

            return uCode;
        }

        protected override bool MoveMotion(ref UInt32 pAxisHandle, ref int nMotionStep, double dPosition, bool bAbsolute, bool bCheckingMotion)
        {
            //if (false == GetHandle(AxisIndex, ref pAxisHandle))
            //{
            //    // 보드를 열고
            //    if (false == m_pCommunicator.OpenApi(true))
            //        return;

            //    if (false == DeclareAxis(AxisIndex, ref pAxisHandle))
            //        return;
            //}

            bool bResult = false;
            switch (nMotionStep)
            {
                case 0:
                    {
                        if (bAbsolute)
                        {
                            m_dTargetPos = dPosition;
                        }
                        else
                        {
                            m_dTargetPos = CommandPosition + dPosition;
                        }

                        UInt32 uCode = 0;
                        UInt32 pDeviceHandle = 0;
                        int nCount = 1;
                        if (0 != m_pSlaveHandle)
                        {
                            nCount = 2;
                            uCode = CMotionAPI.ymcDeclareDevice(2, new UInt32[] { pAxisHandle, m_pSlaveHandle }, ref pDeviceHandle);
                        }
                        else
                        {
                            uCode = CMotionAPI.ymcDeclareDevice(1, new UInt32[] { pAxisHandle }, ref pDeviceHandle);
                        }
                        if (false == m_pCommunicator.CheckingApi(uCode,
                            MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDeclareDevice"))
                            return false;

                        //
                        CMotionAPI.POSITION_DATA[] arPos = new CMotionAPI.POSITION_DATA[nCount];			// POSITION_DATA構造体
                        CMotionAPI.MOTION_DATA[] arMotionData = new CMotionAPI.MOTION_DATA[nCount];
                        UInt16[]  arWaitForCompletion = new UInt16[nCount];

                        for(int i = 0; i < nCount; ++i)
                        {
                            arMotionData[i].CoordinateSystem = (Int16)CMotionAPI.ApiDefs.WORK_SYSTEM;
                            arMotionData[i].MoveType = bAbsolute ? (Int16)CMotionAPI.ApiDefs.MTYPE_ABSOLUTE : (Int16)CMotionAPI.ApiDefs.MTYPE_RELATIVE;
                            arMotionData[i].VelocityType = (Int16)CMotionAPI.ApiDefs.VTYPE_UNIT_PAR;
                            arMotionData[i].AccDecType = (Int16)CMotionAPI.ApiDefs.ATYPE_TIME;
                            arMotionData[i].FilterType = GetProfileType(m_stSpeedParam.enSpeedPattern);
                            arMotionData[i].DataType = (Int32)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE;

                            arMotionData[i].Acceleration = (Int32)(m_stSpeedParam.dblAccelTime * 1000);
                            arMotionData[i].Deceleration = (Int32)(m_stSpeedParam.dblDecelTime * 1000);
                            arMotionData[i].FilterTime = (Int32)(m_stSpeedParam.dblAccelTime / 2 * 10000);
                            arMotionData[i].Velocity = (Int32)m_stSpeedParam.dblVelocity;

                            arPos[i].DataType = (Int32)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE;
                            arPos[i].PositionData = (Int32)dPosition;

                            arWaitForCompletion[i] = (UInt16)CMotionAPI.ApiDefs.COMMAND_STARTED;
                        }

                        //UInt16[] arWaitForCompletion = null;
                        ////if (bBlocking)
                        ////{
                        ////    arWaitForCompletion = new UInt16[] { (UInt16)CMotionAPI.ApiDefs.POSITIONING_COMPLETED };

                        ////}
                        ////else
                        //{
                        //    arWaitForCompletion = new UInt16[] { (UInt16)CMotionAPI.ApiDefs.COMMAND_STARTED };
                        //}

                        uCode = CMotionAPI.ymcMoveDriverPositioning(pDeviceHandle, arMotionData, arPos, 0, null, arWaitForCompletion, 0);

                        //if (bBlocking)
                        //{
                        //    m_bExecutingMotion = false;
                        //}

                        // 에러 발생 시 로그만 찍기 위함
                        bResult = m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcMoveDriverPositioning");

                        //

                        uCode = CMotionAPI.ymcClearDevice(pDeviceHandle);

                        bResult &= m_pCommunicator.CheckingApi(uCode,
                            MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcClearDevice");
                    }

                    if (bResult)
                    {
                        ++m_nMotionStep;
                    }
                    else
                    {
                        return false;
                    }

                    if (false == bCheckingMotion)
                        return true;

                    return false;

                case 1:
                    if (Math.Abs(m_dTargetPos - CommandPosition) > 100)
                        return false;


                    if (false == m_bMotionDone)
                        return false;

                    //StopMotion(ref pAxisHandle, false);

                    m_bExecutingMotion = false;

                    return true;

                default:
                    return false;

            }
        }

        //protected override void MoveMultiMotion(int[] arAxes, double dPosition, bool bAbsolute)
        //{
        //    //int nCount = arAxes.Length;
        //    //UInt32[] arAxisHandle = new UInt32[nCount];

        //    //CMotionAPI.POSITION_DATA[] arPos = new CMotionAPI.POSITION_DATA[nCount];
        //    //CMotionAPI.MOTION_DATA[] arMotionData = new CMotionAPI.MOTION_DATA[nCount];
        //    //UInt16[] arWaitForCompletion = new UInt16[nCount];

        //    //for (int i = 0; i < nCount; ++i)
        //    //{
        //    //    if (false == GetHandle(arAxes[i] + 1, ref arAxisHandle[i]))
        //    //    {
        //    //        // 보드를 열고
        //    //        if (false == m_pCommunicator.OpenApi(true))
        //    //            return;

        //    //        if (false == DeclareAxis(arAxes[i] + 1, ref arAxisHandle[i]))
        //    //            return;
        //    //    }

        //    //    arMotionData[i].CoordinateSystem = (Int16)CMotionAPI.ApiDefs.WORK_SYSTEM;
        //    //    arMotionData[i].MoveType = bAbsolute ? (Int16)CMotionAPI.ApiDefs.MTYPE_ABSOLUTE : (Int16)CMotionAPI.ApiDefs.MTYPE_RELATIVE;
        //    //    arMotionData[i].VelocityType = (Int16)CMotionAPI.ApiDefs.VTYPE_UNIT_PAR;
        //    //    arMotionData[i].AccDecType = (Int16)CMotionAPI.ApiDefs.ATYPE_TIME;
        //    //    arMotionData[i].FilterType = GetProfileType(m_stSpeedParam.enSpeedPattern);
        //    //    arMotionData[i].DataType = 0;

        //    //    arMotionData[i].Acceleration = (Int32)(m_stSpeedParam.dblAccelTime * 1000);
        //    //    arMotionData[i].Deceleration = (Int32)(m_stSpeedParam.dblDecelTime * 1000);
        //    //    arMotionData[i].FilterTime = (Int32)(m_stSpeedParam.dblAccelTime / 2 * 10000);
        //    //    arMotionData[i].Velocity = (Int32)m_stSpeedParam.dblVelocity;

        //    //    arPos[i].DataType = (Int32)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE;
        //    //    arPos[i].PositionData = (Int32)dPosition;

        //    //    arWaitForCompletion[i] = (UInt16)CMotionAPI.ApiDefs.POSITIONING_COMPLETED;
        //    //}

        //    //UInt32 pDeviceHandle = 0;
        //    //UInt32 uCode = CMotionAPI.ymcDeclareDevice((UInt16)nCount, arAxisHandle, ref pDeviceHandle);
        //    //if (false == m_pCommunicator.CheckingApi(uCode,
        //    //    MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDeclareDevice"))
        //    //    return;

        //    ////

        //    //uCode = CMotionAPI.ymcMoveDriverPositioning(pDeviceHandle, arMotionData, arPos, 0, null, arWaitForCompletion, 0);

        //    //m_bExecutingMotion = false;

        //    //if (false == m_pCommunicator.CheckingApi(uCode,
        //    //   MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcMoveDriverPositioning"))
        //    //    return;

        //    ////

        //    //uCode = CMotionAPI.ymcClearDevice(pDeviceHandle);
        //    //if (false == m_pCommunicator.CheckingApi(uCode,
        //    //    MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcClearDevice"))
        //    //    return;
        //}

        //protected override void MoveMultiJog(int[] arAxes, bool bDirection)
        //{
        //    int nCount = arAxes.Length;
        //    UInt32[] arAxisHandle = new UInt32[nCount];

        //    Int16[] arDirection = new Int16[nCount];     // 移動方向
        //    UInt16[] arTimeout = new UInt16[nCount];       // Timeout時間[ms]
        //    UInt16[] arWaitForCompletion = new UInt16[nCount];
        //    CMotionAPI.MOTION_DATA[] arMotionData = new CMotionAPI.MOTION_DATA[nCount];

        //    for (int i = 0; i < nCount; ++i)
        //    {
        //        if (false == GetHandle(arAxes[i] + 1, ref arAxisHandle[i]))
        //        {
        //            // 보드를 열고
        //            if (false == m_pCommunicator.OpenApi(true))
        //                return;

        //            if (false == DeclareAxis(arAxes[i] + 1, ref arAxisHandle[i]))
        //                return;
        //        }          

        //        arMotionData[i].CoordinateSystem = (Int16)CMotionAPI.ApiDefs.WORK_SYSTEM;
        //        arMotionData[i].MoveType = (Int16)CMotionAPI.ApiDefs.MTYPE_RELATIVE;
        //        arMotionData[i].VelocityType = (Int16)CMotionAPI.ApiDefs.VTYPE_UNIT_PAR;
        //        arMotionData[i].AccDecType = (Int16)CMotionAPI.ApiDefs.ATYPE_TIME;

        //        arMotionData[i].FilterType = GetProfileType(m_stSpeedParam.enSpeedPattern);

        //        arMotionData[i].DataType = 0x0000;
        //        arMotionData[i].Acceleration = (Int32)(m_stSpeedParam.dblAccelTime * 1000);
        //        arMotionData[i].Deceleration = (Int32)(m_stSpeedParam.dblDecelTime * 1000);
        //        arMotionData[i].FilterTime = (Int32)(m_stSpeedParam.dblAccelTime / 2 * 10000);
        //        arMotionData[i].Velocity = (Int32)m_stSpeedParam.dblVelocity;

        //        arDirection[i] = (short)((bDirection) ?
        //            (CMotionAPI.ApiDefs.DIRECTION_POSITIVE) : (CMotionAPI.ApiDefs.DIRECTION_NEGATIVE));
        //        arTimeout[i] = 0;

        //        arWaitForCompletion[i] = (UInt16)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED;
        //    }

        //    UInt32 pDeviceHandle = 0;
        //    UInt32 uCode = CMotionAPI.ymcDeclareDevice((UInt16)nCount, arAxisHandle, ref pDeviceHandle);
        //    if (false == m_pCommunicator.CheckingApi(uCode,
        //        MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDeclareDevice"))
        //        return;

        //    //
        //    uCode = CMotionAPI.ymcMoveJOG(pDeviceHandle, arMotionData, arDirection, arTimeout, 0, null, 0);
        //    if (false == m_pCommunicator.CheckingApi(uCode,
        //        MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcMoveJOG"))
        //        return;

        //    m_bExecutingMotion = true;
        //    //

        //    uCode = CMotionAPI.ymcClearDevice(pDeviceHandle);
        //    if (false == m_pCommunicator.CheckingApi(uCode,
        //        MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcClearDevice"))
        //        return;
        //}

        protected override bool MoveJog(ref UInt32 pAxisHandle, bool bDirection, bool bUsingMultiJogging = true)
        {
            //SetGearedAxis(new int[] { MasterAxis, SlaveAxis }, true);

            //if (false == GetHandle(AxisIndex, ref pAxisHandle))
            //{
            //    // 보드를 열고
            //    if (false == m_pCommunicator.OpenApi(true))
            //        return;

            //    if (false == DeclareAxis(AxisIndex, ref pAxisHandle))
            //        return;
            //}

            UInt32 uCode = 0;
            UInt32 pDeviceHandle = 0;
            int nCount = 1;
            if (0 != m_pSlaveHandle && bUsingMultiJogging)
            {
                nCount = 2;
                uCode = CMotionAPI.ymcDeclareDevice(2, new UInt32[] { pAxisHandle, m_pSlaveHandle }, ref pDeviceHandle);
            }
            else
            {
                uCode = CMotionAPI.ymcDeclareDevice(1, new UInt32[] { pAxisHandle }, ref pDeviceHandle);
            }

            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDeclareDevice"))
                return false;

            //
            Int16[] arDirection = new Int16[nCount];
            UInt16[] arTimeout = new UInt16[nCount];
            UInt16[] arWaitForCompletion = new UInt16[nCount];
            CMotionAPI.MOTION_DATA[] arMotionData = new CMotionAPI.MOTION_DATA[nCount];

            for (int i = 0; i < nCount; ++i )
            {
                arMotionData[i].CoordinateSystem = (Int16)CMotionAPI.ApiDefs.WORK_SYSTEM;
                arMotionData[i].MoveType = (Int16)CMotionAPI.ApiDefs.MTYPE_RELATIVE;
                arMotionData[i].VelocityType = (Int16)CMotionAPI.ApiDefs.VTYPE_UNIT_PAR;
                arMotionData[i].AccDecType = (Int16)CMotionAPI.ApiDefs.ATYPE_TIME;

                arMotionData[i].FilterType = GetProfileType(m_stSpeedParam.enSpeedPattern);

                arMotionData[i].DataType = 0x0000;
                arMotionData[i].Acceleration = (Int32)(m_stSpeedParam.dblAccelTime * 1000);
                arMotionData[i].Deceleration = (Int32)(m_stSpeedParam.dblDecelTime * 1000);
                arMotionData[i].FilterTime = (Int32)(m_stSpeedParam.dblAccelTime / 2 * 10000);
                arMotionData[i].Velocity = (Int32)m_stSpeedParam.dblVelocity;
            
                arDirection[i] = (short)((bDirection) ?
                    (CMotionAPI.ApiDefs.DIRECTION_POSITIVE) : (CMotionAPI.ApiDefs.DIRECTION_NEGATIVE));
                
                arTimeout[i] = 0;

                arWaitForCompletion[i] = (UInt16)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED;
            }

            //
            uCode = CMotionAPI.ymcMoveJOG(pDeviceHandle, arMotionData, arDirection, arTimeout, 0, null, 0);

            // 에러 발생 시 로그만 찍기 위함
            bool bResult = m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcMoveJOG");

            m_bExecutingMotion = true;
            //

            uCode = CMotionAPI.ymcClearDevice(pDeviceHandle);

            bResult &= m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcClearDevice");

            return bResult;
        }

        protected override bool OverrideSpeed(ref UInt32 pAxisHandle, double dVelocity)
        {
            //if (false == GetHandle(AxisIndex, ref pAxisHandle))
            //{
            //    // 보드를 열고
            //    if (false == m_pCommunicator.OpenApi(true))
            //        return;

            //    if (false == DeclareAxis(AxisIndex, ref pAxisHandle))
            //        return;
            //}

            UInt32 pDeviceHandle = 0;
            UInt32 uCode = CMotionAPI.ymcDeclareDevice(1, new UInt32[] { pAxisHandle }, ref pDeviceHandle);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDeclareDevice"))
                return false;

            ///
            CMotionAPI.MOTION_DATA[] arMotionData = new CMotionAPI.MOTION_DATA[1];
            CMotionAPI.POSITION_DATA[] arPositionData = new CMotionAPI.POSITION_DATA[1];
            arMotionData[0].Velocity = (Int32)dVelocity;
            arPositionData[0].DataType = (int)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE;

            uCode = CMotionAPI.ymcChangeDynamics(pDeviceHandle, arMotionData, arPositionData,
                (UInt32)CMotionAPI.ApiDefs.SUBJECT_VEL, null, 0);

            // 에러 발생 시 로그만 찍기 위함
            bool bResult = m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcChangeDynamics");

            //
            uCode = CMotionAPI.ymcClearDevice(pDeviceHandle);
            bResult &= m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcClearDevice");

            return bResult;
        }

        protected override EXECUTING_COMMAND_TYPE GetExecutingCommand()
        {
            UInt32 pRegisterDataHandle = 0;
            UInt32 uAddress = RegisterOutputAddress + MechatrolinkDefines.OUTPUT_REGISTER_ADDRESS_MOTION_COMMAND;
            string strRegisterId = String.Format("OW{0:X}", uAddress);

            UInt32 uCode = CMotionAPI.ymcGetRegisterDataHandle(strRegisterId, ref pRegisterDataHandle);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterDataHandle"))
                return EXECUTING_COMMAND_TYPE.UNKNOWN;

            Int16[] arReadData = new Int16[1];
            UInt32 nReadNum = 0;
            uCode = CMotionAPI.ymcGetRegisterData(pRegisterDataHandle, 1, arReadData, ref nReadNum);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return EXECUTING_COMMAND_TYPE.UNKNOWN;

            switch (arReadData[0])
            {
                case (Int16)MOTION_COMMAND_MODE_TYPE.MOTION_COMMAND_STOP:
                    return EXECUTING_COMMAND_TYPE.STOP;
                case (Int16)MOTION_COMMAND_MODE_TYPE.MOTION_COMMAND_MOVE:
                    return EXECUTING_COMMAND_TYPE.MOVE;
                case (Int16)MOTION_COMMAND_MODE_TYPE.MOTION_COMMAND_JOG:
                    return EXECUTING_COMMAND_TYPE.JOG;
                case (Int16)MOTION_COMMAND_MODE_TYPE.MOTION_COMMAND_HOME:
                    return EXECUTING_COMMAND_TYPE.HOME;
                default:
                    return EXECUTING_COMMAND_TYPE.UNKNOWN;

            }
        }
        #endregion </Motion Commands>

        #region <Homing>
        int _tempHomeSeq = -1;
        public override int CycleHoming(ref UInt32 pAxisHandle, ref int nSeq, ref int nDelay)
        {
            // NOTE : Mechatrolink flux mask Y axis home 안되는 문제 확인용 임시 로그 저장
            if (AxisIndex == 11 && _tempHomeSeq != nSeq)
            {
                _tempHomeSeq = nSeq;
                Log.LogWriter.GetInstance().MakeLog("JHLIM\\FluxAxis", nSeq.ToString());
            }
            switch (nSeq)
            {
                case (int)HOME_SEQ.INIT:
                    // 파라메터 초기화
                    HomeTimeout = false;
                    m_bReqStopHoming = false;
                    HomeCompletion = false;
                    m_tickHomingTimeoutChecker.Clear();
                    m_tickHomingTimeoutChecker.SetTickCount(m_stHomingParam.uHomeTimeout);
                    StopMotion(ref pAxisHandle, false);
                    ++nSeq; break;
                case (int)HOME_SEQ.INIT + 1:
                    ClearAlarm(ref pAxisHandle);

                    if (null == m_pSlaveAxis || false == IsGantrySettingComplete())
                    {
                        nSeq = (int)HOME_SEQ.MOVE_TO_LIMIT;
                    }
                    else
                    {
                        m_pSlaveAxis.ResetAxis();
                        m_tickHomingDelay.Clear();
                        m_tickHomingDelay.SetTickCount(200);
                        ++nSeq;
                    }
                    break;

                case (int)HOME_SEQ.INIT + 2:
                    if (false == m_tickHomingDelay.IsTickOver(false))
                        break;

                    m_pSlaveAxis.SetServoState(true);
                    nDelay = 200;
                    m_tickHomingDelay.Clear();
                    m_tickHomingDelay.SetTickCount(200);
                    nSeq = (int)HOME_SEQ.PREPARE_GANTRY;
                    break;

                case (int)HOME_SEQ.PREPARE_GANTRY:
                    {
                        if (false == m_tickHomingDelay.IsTickOver(false))
                            break;

                        if (false == m_pSlaveAxis.GetServoReady())
                            break;

                        //m_stGantryParam.bEnableGantry = false;
                        //int[] arAxes = new int[m_arGearedAxes.Length];
                        //for (int i = 0; i < m_arGearedAxes.Length; ++i)
                        //{
                        //    arAxes[i] = (int)m_arGearedAxes[i];
                        //}
                        //m_pCommunicator.RequestEnableGantry(arAxes);
                        //nDelay = 500;
                    }
                    ++nSeq; break;

                // Slave 축의 Servo를 Off하여 따라오게 만든다.
                case (int)HOME_SEQ.PREPARE_GANTRY + 1:
                    //if (GantryState)
                    //    break;

                    m_pSlaveAxis.SetServoState(false);
                    //nDelay = 200;

                    m_tickHomingDelay.Clear();
                    m_tickHomingDelay.SetTickCount(1000);   // 2024.06.05 by jhlim [ADD] Gantry 축일 경우에만 1초 더 기다리도록

                    ++nSeq; break;

                case (int)HOME_SEQ.PREPARE_GANTRY + 2:
                    if (false == m_tickHomingDelay.IsTickOver(true))
                        break;

                    if (m_pSlaveAxis.GetServoReady())
                        break;

                    //nDelay = 200;
                    //m_tickHomingDelay.Clear();
                    //m_tickHomingDelay.SetTickCount(200);

                    nSeq = (int)HOME_SEQ.MOVE_TO_LIMIT; break;

                case (int)HOME_SEQ.MOVE_TO_LIMIT:
                    {
                        //if (false == m_tickHomingDelay.IsTickOver(true))
                        //    break;

                        bool bResult = IsUsingLimitSensor(m_stHomingParam.enHomeMode);
                        if (false == bResult)
                        {
                            nSeq = (int)HOME_SEQ.HOMING;
                        }
                        else
                        {
                            SetSpeedParam(m_stHomingJoggingParam);
                            MoveJog(ref pAxisHandle, m_stHomingParam.bHomeDirection, false);
                            ++nSeq;
                        }
                    }
                    break;

                case (int)HOME_SEQ.MOVE_TO_LIMIT + 1:
                    {
                        bool bNeedStop = false;
                        if (m_stHomingParam.bHomeDirection)
                        {
                            bNeedStop = HWLimitPositive;
                        }
                        else
                        {
                            bNeedStop = HWLimitNegative;
                        }

                        if (false == bNeedStop)
                            break;

                        StopMotion(ref pAxisHandle, false);
                        ++nSeq;
                    }
                    break;

                case (int)HOME_SEQ.MOVE_TO_LIMIT + 2:
                    if (false == m_bMotionDone)
                        break;

                    //nDelay = 100;
                    m_tickHomingDelay.Clear();
                    m_tickHomingDelay.SetTickCount(200);

                    ++nSeq; break;

                case (int)HOME_SEQ.MOVE_TO_LIMIT + 3:
                    if (false == m_tickHomingDelay.IsTickOver(true))
                        break;

                    ClearAlarm(ref pAxisHandle);
                    nSeq = (int)HOME_SEQ.MOVE_TO_ESCAPE;
                    break;

                case (int)HOME_SEQ.MOVE_TO_ESCAPE:
                    //SetSpeedParam(m_stHomingEscapeParam);
                    //MoveJog(!m_stHomingParam.bHomeDirection);
                    ++nSeq; break;

                case (int)HOME_SEQ.MOVE_TO_ESCAPE + 1:
                    {
                        //bool bNeedStop = false;
                        //if (m_stHomingParam.bHomeDirection)
                        //{
                        //    bNeedStop = !HWLimitPositive;
                        //}
                        //else
                        //{
                        //    bNeedStop = !HWLimitNegative;
                        //}

                        //if (false == bNeedStop)
                        //    break;

                        //StopMotion(false);
                        ++nSeq;
                    }
                    break;

                case (int)HOME_SEQ.MOVE_TO_ESCAPE + 2:
                    //nDelay = 100;
                    ++nSeq; break;

                case (int)HOME_SEQ.MOVE_TO_ESCAPE + 3:
                    {
                        //double dPosition = m_stHomingParam.dblEscDist;

                        //// +방향이면 -방향으로, -방향이면 +방향으로 적용됨
                        //if (m_stHomingParam.bHomeDirection)
                        //{
                        //    m_stHomingParam.dblEscDist *= -1;
                        //}

                        //SetSpeedParam(m_stHomingJoggingParam);
                        //MoveMotion(m_stHomingParam.dblEscDist, false, false);
                        //nDelay = 200;
                    }
                    ++nSeq; break;

                case (int)HOME_SEQ.MOVE_TO_ESCAPE + 4:
                    //if (false == m_bMotionDone)
                    //    break;
                    //else
                    {
                        //nDelay = 100;
                        nSeq = (int)HOME_SEQ.HOMING;
                    }
                    break;

                case (int)HOME_SEQ.HOMING:
                    //ThreadPool.QueueUserWorkItem(x => CycleHoming());
                    Homing(ref pAxisHandle);
                    //nDelay = 1000;

                    m_tickHomingDelay.Clear();
                    m_tickHomingDelay.SetTickCount(500);

                    nSeq = (int)HOME_SEQ.WAIT_COMPLETION;
                    break;

                case (int)HOME_SEQ.WAIT_COMPLETION:
                    if (false == m_tickHomingDelay.IsTickOver(true))
                        break;

                    if (m_tickHomingTimeoutChecker.IsTickOver(true))
                    {
                        HomeTimeout = true;
                        return HOMING_RESULT_END;
                    }

                    if (m_bReqStopHoming)
                    {
                        //nSeq = (int)HOME_SEQ.INIT;
                        m_bReqStopHoming = false;
                        nSeq = (int)HOME_SEQ.END;
                        break;
                    }

                    if (false == MotionDone)
                        break;

                    if (false == GetHomeCompletion(ref pAxisHandle))
                        break;

                    //StopMotion(ref pAxisHandle, false);

                    //if (Math.Abs(ActualPosition - m_stHomingParam.dblHomeBasePosition) >= 0.1)
                    //    break;
                    //nDelay = 100;
                    m_tickHomingDelay.Clear();
                    m_tickHomingDelay.SetTickCount(200);

                    ++nSeq; break;

                case (int)HOME_SEQ.WAIT_COMPLETION + 1:
                    //if (false == HomeCompletion)
                    //    break;
                    if (false == m_tickHomingDelay.IsTickOver(true))
                        break;

                    if (null == m_pSlaveAxis)// || false == IsGantrySettingComplete())
                    {
                        //HomeCompletion = true;
                        m_tickHomingDelay.Clear();
                        m_tickHomingDelay.SetTickCount(100);
                        nSeq = (int)HOME_SEQ.END;
                    }
                    else
                    {
                        ++nSeq;
                    }
                    break;

                case (int)HOME_SEQ.WAIT_COMPLETION + 2:
                    m_pSlaveAxis.SetServoState(true);
                    //nDelay = 200;
                    m_tickHomingDelay.Clear();
                    m_tickHomingDelay.SetTickCount(200);

                    ++nSeq; break;

                case (int)HOME_SEQ.WAIT_COMPLETION + 3:
                    if (false == m_tickHomingDelay.IsTickOver(true))
                        break;

                    if (false == m_pSlaveAxis.GetServoReady())
                        break;
                    ++nSeq; break;

                case (int)HOME_SEQ.WAIT_COMPLETION + 4:
                    m_pSlaveAxis.MoveToHomeOffset(m_stHomingParam);
                    m_tickHomingDelay.Clear();
                    m_tickHomingDelay.SetTickCount(500);
                    ++nSeq; break;

                case (int)HOME_SEQ.WAIT_COMPLETION + 5:
                     if (false == m_tickHomingDelay.IsTickOver(true))
                        break;

                    if (m_tickHomingTimeoutChecker.IsTickOver(true))
                    {
                        HomeTimeout = true;
                        return HOMING_RESULT_END;
                    }

                    if (m_bReqStopHoming)
                    {
                        //nSeq = (int)HOME_SEQ.INIT;
                        m_bReqStopHoming = false;
                        nSeq = (int)HOME_SEQ.END;
                        break;
                    }

                    if (false == m_pSlaveAxis.GetMotionReady())
                        break;

                    m_pSlaveAxis.SetHomePosition(m_stHomingParam.dblHomeBasePosition, true);
                    m_tickHomingDelay.Clear();
                    m_tickHomingDelay.SetTickCount(500);
                    ++nSeq; break;
                    

                case (int)HOME_SEQ.WAIT_COMPLETION + 6:
                    {
                        if (m_bReqStopHoming)
                        {
                            //nSeq = (int)HOME_SEQ.INIT;
                            m_bReqStopHoming = false;
                            nSeq = (int)HOME_SEQ.END;
                            break;
                        }

                        if (false == m_tickHomingDelay.IsTickOver(true))
                            break;

                        if (false == m_pSlaveAxis.GetMotionReady())
                            break;

                        //m_stGantryParam.bEnableGantry = true;
                        //int[] arAxes = new int[m_arGearedAxes.Length];
                        //for (int i = 0; i < m_arGearedAxes.Length; ++i)
                        //{
                        //    arAxes[i] = (int)m_arGearedAxes[i];
                        //}
                        //m_pCommunicator.RequestEnableGantry(arAxes);
                    }
                    ++nSeq; break;
                
                case (int)HOME_SEQ.WAIT_COMPLETION + 7:
                    if (m_bReqStopHoming)
                    {
                        //nSeq = (int)HOME_SEQ.INIT;
                        m_bReqStopHoming = false;
                        nSeq = (int)HOME_SEQ.END;
                        break;
                    }

                    if (false == GantryState)
                        break;

                    nSeq = (int)HOME_SEQ.END; break;

                case (int)HOME_SEQ.END:
                    if (false == m_tickHomingDelay.IsTickOver(true))
                        break;

                    StopMotion(ref pAxisHandle, false);

                    HomeCompletion = true;

                    if (AxisIndex == 11)
                    {
                        Log.LogWriter.GetInstance().MakeLog("JHLIM\\FluxAxis", "------------------- Finished ------------------");
                    }

                    return HOMING_RESULT_END;
            }

            return HOMING_RESULT_WORKING;
        }
        #endregion </Homing>

        #endregion </Inhert Methods>

        #region <Internal Methods>
        protected override bool GetHandle(int nAxis, ref UInt32 pAxisHandle)
        {
            UInt32 uCode = CMotionAPI.ymcGetAxisHandle((UInt16)CMotionAPI.ApiDefs.LOGICALAXIS, 1, 0, 3,
                (UInt16)nAxis, (UInt16)nAxis, Name, ref pAxisHandle);

            return uCode == CMotionAPI.MP_SUCCESS;
        }

        protected override bool DeclareAxis(int nAxis, ref UInt32 pAxisHandle)
        {
            string strMyName = String.Format("Axis-{0}", nAxis);
            UInt32 uCode = CMotionAPI.ymcDeclareAxis(1, 0, 3,
                (UInt16)(nAxis),
                (UInt16)(nAxis),
                (UInt16)CMotionAPI.ApiDefs.REAL_AXIS,
                Name,
                ref pAxisHandle);

            return uCode == CMotionAPI.MP_SUCCESS;
        }

        private void Homing(ref UInt32 pAxisHandle)
        {
            //UInt32 pAxisHandle = 0;
            //if (false == GetHandle(AxisIndex, ref pAxisHandle))
            //{
            //    // 보드를 열고
            //    if (false == m_pCommunicator.OpenApi(true))
            //        return;

            //    if (false == DeclareAxis(AxisIndex, ref pAxisHandle))
            //        return;
            //}

            UInt32 pDeviceHandle = 0;
            UInt32 uCode = CMotionAPI.ymcDeclareDevice(1, new UInt32[] { pAxisHandle }, ref pDeviceHandle);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcDeclareDevice"))
                return;

            CMotionAPI.MOTION_DATA[] arMotionData = new CMotionAPI.MOTION_DATA[1];
            CMotionAPI.POSITION_DATA[] arOrgPos = new CMotionAPI.POSITION_DATA[1];

            UInt16[] arHomingMethod = new UInt16[1];			// 홈 타입
            UInt16[] arDirect = new UInt16[1];					// 방향
            UInt16[] arWaitForCompletion = new UInt16[1];		// API 블로킹모드

            double dVelocity = m_stHomingParam.dblHomeVelocity;
            double dSecondVelocity = m_stHomingParam.dblHomeSecondVelocity;

            arMotionData[0].MoveType = (Int16)CMotionAPI.ApiDefs.MTYPE_ABSOLUTE;
            arMotionData[0].VelocityType = (Int16)CMotionAPI.ApiDefs.VTYPE_UNIT_PAR;
            arMotionData[0].AccDecType = (Int16)CMotionAPI.ApiDefs.ATYPE_TIME;
            arMotionData[0].DataType = (Int32)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE;
            arMotionData[0].Acceleration = (Int32)(m_stHomingParam.dblHomeAccelTime * 1000);
            arMotionData[0].Deceleration = (Int32)(m_stHomingParam.dblHomeDecelTime * 1000);
            arMotionData[0].Velocity = (Int32)dVelocity;                               // Home Offset 이동 속도
            arMotionData[0].ApproachVelocity = (Int32)(m_stHomingParam.bHomeDirection ? (Int32)(dVelocity) : (Int32)(-dVelocity));
            arMotionData[0].CreepVelocity = (Int32)(m_stHomingParam.bHomeDirection ? (Int32)(-dSecondVelocity) : ((Int32)dSecondVelocity));

            arMotionData[0].FilterType = GetProfileType(m_stHomingParam.enHomeSpeedPattern);
            arMotionData[0].FilterTime = (Int32)(m_stHomingParam.dblHomeAccelTime / 2 * 10000);

            arOrgPos[0].DataType = (Int32)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE;
            arOrgPos[0].PositionData = (Int32)m_stHomingParam.dblHomeOffset;        // HomeOffset Setting

            // Signal 사용 유무 초기화
            UInt32 uUsingSignalOption = 0;
            arHomingMethod[0] = GetHomingMethod(m_stHomingParam.enHomeMode);
            if (arHomingMethod[0] == (UInt16)CMotionAPI.ApiDefs.HMETHOD_INPUT_C ||
                arHomingMethod[0] == (UInt16)CMotionAPI.ApiDefs.HMETHOD_INPUT_ONLY)
                uUsingSignalOption = 0x0800;

            uCode = CMotionAPI.ymcSetMotionParameterValue(pAxisHandle, (UInt16)CMotionAPI.ApiDefs.SETTING_PARAMETER, 5, uUsingSignalOption);

            // 에러 발생 시 로그만 찍기 위함
            bool bResult = m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcSetMotionParameterValue");

            if (bResult)
            {
                UInt16 nDirection = 0;
                // Limit를 사용하는 타입의 경우 해당 축까지 이동 후 반대방향으로 이동해야한다.
                // 홈 타입 리턴 시 리밋을 사용하면 C상 찾는 방법으로 리턴될 것이다.
                if (IsUsingLimitSensor(m_stHomingParam.enHomeMode))
                {
                    nDirection = m_stHomingParam.bHomeDirection ?
                                    (UInt16)CMotionAPI.ApiDefs.DIRECTION_NEGATIVE : (UInt16)CMotionAPI.ApiDefs.DIRECTION_POSITIVE;
                }
                else
                {
                    nDirection = m_stHomingParam.bHomeDirection ?
                    (UInt16)CMotionAPI.ApiDefs.DIRECTION_POSITIVE : (UInt16)CMotionAPI.ApiDefs.DIRECTION_NEGATIVE;
                }
                arDirect[0] = nDirection;

                //arWaitForCompletion[0] = (UInt16)CMotionAPI.ApiDefs.POSITIONING_COMPLETED;            
                arWaitForCompletion[0] = (UInt16)CMotionAPI.ApiDefs.COMMAND_STARTED;    // 비동기

                // 
                UInt32 pRegisterDataHandle = 0;
                UInt32 uAddress = RegisterOutputAddress + MechatrolinkDefines.OUTPUT_REGISTER_ADDRESS_USING_C_PHASE;
                string strRegisterId = String.Format("OW{0:X}", uAddress);
                uCode = CMotionAPI.ymcGetRegisterDataHandle(strRegisterId, ref pRegisterDataHandle);
                m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterDataHandle");

                Int16[] arOutputData = new Int16[] { 0x22 };
                uCode = CMotionAPI.ymcSetRegisterData(pRegisterDataHandle, 1, arOutputData);
                if (false == m_pCommunicator.CheckingApi(uCode,
                    MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                    return;

                uAddress = RegisterOutputAddress + MechatrolinkDefines.OUTPUT_REGISTER_ADDRESS_BASE_POSITION;
                strRegisterId = String.Format("OL{0:X}", uAddress);
                uCode = CMotionAPI.ymcGetRegisterDataHandle(strRegisterId, ref pRegisterDataHandle);

                // 에러 발생 시 로그만 찍기 위함
                bResult &= m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterDataHandle");

                if (bResult)
                {
                    // BasePosition Setting
                    Int32[] arOutputData2 = new Int32[] { (Int32)m_stHomingParam.dblHomeBasePosition };
                    uCode = CMotionAPI.ymcSetRegisterData(pRegisterDataHandle, 1, arOutputData2);

                    // 에러 발생 시 로그만 찍기 위함
                    bResult &= m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData");
                }
            }


            //
            if (bResult)
            {
                uCode = CMotionAPI.ymcMoveHomePosition(pDeviceHandle, arMotionData, arOrgPos, arHomingMethod,
                    arDirect, 0, null, arWaitForCompletion, 0);

                if (false == m_pCommunicator.CheckingApi(uCode,
                    MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcMoveHomePosition"))
                {
                    if (uCode == (UInt32)CMotionAPI.MP_TIMEOUT_ERR)
                    {
                        m_bReqStopHoming = true;
                        HomeCompletion = false;
                        HomeTimeout = true;
                    }
                    else
                    {
                        string strDetail = String.Empty;
                        StopDriving(pDeviceHandle, false, ref strDetail);
                        m_pCommunicator.CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, AxisIndex, "After StopDriving");
                    }

                    uCode = CMotionAPI.ymcClearDevice(pDeviceHandle);
                    if (false == m_pCommunicator.CheckingApi(uCode,
                        MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcClearDevice"))
                        return;

                    return;
                }
            }

            uCode = CMotionAPI.ymcClearDevice(pDeviceHandle);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcClearDevice"))
                return;

            if (m_bReqStopHoming)
                return;

			// 2023.10.13 by jhlim [DEL] improve code
            //HomeCompletion = true;
        }

        private bool GetHomeCompletion(ref UInt32 pAxisHandle)
        {
            UInt32 uStoredValue = 0;
            UInt32 uCode = CMotionAPI.ymcGetMotionParameterValue(pAxisHandle,
              (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER, (UInt32)CMotionAPI.ApiDefs_MonPrm.SER_POSSTS,
              ref uStoredValue);

            if (uCode == CMotionAPI.MP_SUCCESS)
            {
                return ((uStoredValue & 0x20) >> 5) == 1 ? true : false;
            }

            return (uCode == CMotionAPI.MP_SUCCESS);
        }

        private UInt16 GetHomingMethod(MCTL_HOME_TYPE enType)
        {
            switch (enType)
            {
                case MCTL_HOME_TYPE.HMETHOD_DEC1_C:                                     // DEC센서 + C상 체크
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_DEC1_C;
                case MCTL_HOME_TYPE.HMETHOD_ZERO:                                       // HomeSensor         
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_ZERO;
                case MCTL_HOME_TYPE.HMETHOD_DEC1_ZERO:                                  // DEC센서+ HomeSensor
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_DEC1_ZERO;
                case MCTL_HOME_TYPE.HMETHOD_C:                                          // C상 체크         
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_C;
                case MCTL_HOME_TYPE.HMETHOD_C_ONLY:                                     // C상 체크 2
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_C_ONLY;
                case MCTL_HOME_TYPE.HMETHOD_POT_C:                                      // +Limit + C상
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_POT_C;
                case MCTL_HOME_TYPE.HMETHOD_POT_C2:                                      // +Limit + C상
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_C_ONLY;   // 리밋 사용의 경우 이미 리밋까지 도달 했을 것이므로 C상 방식을 리턴
                case MCTL_HOME_TYPE.HMETHOD_POT_ONLY:
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_POT_ONLY;  // +Limit
                case MCTL_HOME_TYPE.HMETHOD_HOMELS_C:
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_HOMELS_C;  // Home + LS + C상
                case MCTL_HOME_TYPE.HMETHOD_HOMELS_ONLY:                                 // Home + LS			
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_HOMELS_ONLY;
                case MCTL_HOME_TYPE.HMETHOD_NOT_C:                                      // -Limit + C상
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_NOT_C;
                case MCTL_HOME_TYPE.HMETHOD_NOT_C2:                                      // -Limit + C상
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_C_ONLY;   // 리밋 사용의 경우 이미 리밋까지 도달 했을 것이므로 C상 방식을 리턴
                case MCTL_HOME_TYPE.HMETHOD_NOT_ONLY:                                   // -Limit
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_NOT_ONLY;
                case MCTL_HOME_TYPE.HMETHOD_INPUT_C:                                    // InputSignal + C상
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_INPUT_C;
                case MCTL_HOME_TYPE.HMETHOD_INPUT_ONLY:                                 // InputSignal
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_INPUT_ONLY;
                default:
                    return (UInt16)CMotionAPI.ApiDefs.HMETHOD_DEC1_ZERO;
            }
        }

        private bool IsUsingLimitSensor(MCTL_HOME_TYPE enType)
        {
            switch (enType)
            {
                //case MCTL_HOME_TYPE.HMETHOD_POT_C:                                      // +Limit + C상
                case MCTL_HOME_TYPE.HMETHOD_POT_C2:                                      // +Limit + C상
                //case MCTL_HOME_TYPE.HMETHOD_POT_ONLY:
                //case MCTL_HOME_TYPE.HMETHOD_NOT_C:                                      // -Limit + C상
                case MCTL_HOME_TYPE.HMETHOD_NOT_C2:                                      // -Limit + C상
                    //case MCTL_HOME_TYPE.HMETHOD_NOT_ONLY:                                   // -Limit
                    return true;
                default:
                    return false;
            }
        }

        // 마스터축번호(Target)가 10인 경우, MW01100, MW01101로 저장됨
        // MW0x000 : Maser Logical Index(Target + 1)
        // MW0xx01 : Slave 상태 (0 : GEAR_NOT_ENGAGED, 1 : GEAR_WAITING_ENGAGED, 2 : GEAR_ENGAGED, 3 : GEAR_WAITING_DISENGAGED)
        //           2를 제외하고는 모두 비정상 상태임
        private string GetGantryRegisterName(UInt32[] arAxes, int nAddress)
        {
            string strAxisIndex = (arAxes[0] + 1).ToString();
            strAxisIndex = strAxisIndex.PadLeft(1, '0');

            return String.Format("MW0{0}0{1}", strAxisIndex, nAddress);
        }

        private int GetGantryStateFromRegister(UInt32[] arAxes, int nAddress)
        {
            string strRegisterId = GetGantryRegisterName(arAxes, nAddress);
            UInt32 pState = 0;
            UInt32 uCode = CMotionAPI.ymcGetRegisterDataHandle(strRegisterId, ref pState);
            if (uCode != CMotionAPI.MP_SUCCESS)
            {
                if (false == m_pCommunicator.OpenApi(true))
                    return 0;

                uCode = CMotionAPI.ymcGetRegisterDataHandle(strRegisterId, ref pState);
                if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterDataHandle"))
                    return 0;
            }

            UInt32 pNum = 0;
            Int32[] pRegisterData = new Int32[1];
            uCode = CMotionAPI.ymcGetRegisterData(pState, 1, pRegisterData, ref pNum);
            if (false == m_pCommunicator.CheckingApi(uCode,
                MethodBase.GetCurrentMethod().Name, AxisIndex, "After ymcGetRegisterData"))
                return 0;

            return (pRegisterData[0] >> 0x08);
        }
        #endregion </Internal Methods>

        #endregion </Methods>
    }
}
