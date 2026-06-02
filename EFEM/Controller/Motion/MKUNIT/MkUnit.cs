using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MKUNIT
{
    #region MODE1/MODE2
    enum EN_MODE1_DECEL_STARTPOINT : byte // D7
    {
        SHIFT = 7,
        AUTO = 0,
        REMAIN_POINT = 1,
    }
    // DIR, PULSE, ACTIVE POLARITY 조합
    enum EN_MODE1_PULSE_TYPE : byte    //  D6,D5,D4
    {
        SHIFT = 4,

        // 1 펄스
        ONE_PULSE_H_DIR_L_CW_H_CCW = 0,    // 유형0
        ONE_PULSE_H_DIR_H_CW_L_CCW = 1,    // 유형1
        ONE_PULSE_L_DIR_L_CW_H_CCW = 2,    // 유형2
        ONE_PULSE_L_DIR_H_CW_L_CCW = 3,    // 유형3
        // 2 펄스
        TWO_PULSE_H_CCW_DIR_H_CW = 4,   // 유형4
        TWO_PULSE_L_CCW_DIR_L_CW = 5,   // 유형5
        TWO_PULSE_H_CW_DIR_H_CCW = 6,   // 유형6
        TWO_PULSE_L_CW_DIR_L_CCW = 7,   // 유형7

        // 2상 펄스 모드 사용하는지 확인 필요.
// 
//         // A상 DIR, B상 PULSE
//         DIR_A_H_CW_PULSE_B_H_CCW = 8,
//         DIR_A_L_CW_PULSE_B_L_CCW = 9,
//         
//         PULSE_A_H_CCW_DIR_B_H_CW = 10,
//         PULSE_A_L_CCW_DIR_B_L_CW = 11,
    }
    enum EN_MODE1_SIGNAL_SEARCH : byte   // D3, D2, D1, D0
    {
        E_LIMIT_PLUS_DIR_NEGATIVE_EDGE = 0,
        E_LIMIT_MINUS_DIR_NEGATIVE_EDGE = 1,
        SLOW_LIMIT_PLUS_DIR_NEGATIVE_EDGE = 2,
        SLOW_LIMIT_MINUS_DIR_NEGATIVE_EDGE = 3,
        ORG_POSITIVE_EDGE = 4,
        Z_PHASE_POSITIVE_EDGE = 5,
        IN_POSITIVE_EDGE = 6,
        // NOT USE 7
        E_LIMIT_PLUS_DIR_POSITIVE_EDGE = 8,
        E_LIMIT_MINUS_DIR_POSITIVE_EDGE = 9,
        SLOW_LIMIT_PLUS_DIR_POSITIVE_EDGE = 10,
        SLOW_LIMIT_MINUS_DIR_POSITIVE_EDGE = 11,
        ORG_NEGATIVE_EDGE = 12,
        Z_PHASE_NEGATIVE_EDGE = 13,
        IN_NEGATIVE_EDGE = 14,
        // NOT USE 15
    }
    enum EN_MODE2 : byte
    {
        /// <summary> DEND_N (드라이버 위치 결정 완료 신호)
        /// 펄스 출력 완료 후 DEND-n 신호가 활성화 입력될 때까지,
        /// BUSY 상태를 유지하여, 다음 명령을 금지
        /// </summary>
        DEND_N = 32,
        /// <summary> DERR_N (모터 드라이버 오류 신호)
        /// 모터 드라이버에서 출력되는 오류 신호 검출 시, 급 정지한다.
        /// </summary>
        DERR_N = 16,
        /// <summary> SLM_N
        /// Negative Slow Down Limit Sensor 활성화
        /// </summary>
        SLM_N = 8,
        /// <summary> SLM_P
        /// Positive Slow Down Limit Sensor 활성화
        /// </summary>
        SLM_P = 4,
        /// <summary> ELM_N
        /// Negative E-Stop Limit Sensor 활성화
        /// </summary>
        ELM_N = 2,
        /// <summary> ELM_P
        /// Positive E-Stop Limit Sensor 활성화
        /// </summary>
        ELM_P = 1,
        EMPTY = 0,
    }
    #endregion

    public struct HOMING_SQC
    {
        public const int INIT = 0;
        public const int MOVE = 10;
        public const int SET_ZERO = 100;
        public const int OFFSET_MOVE = 200;
        public const int ERROR = 1000;
        public const int STOP = 2000;
    }
    public class MkUnitController
    {
        private MkUnitAxis[] m_axis = null;

        /// <summary>
        /// 2019.07.01. by shkim. [ADD] MKUNIT Dll을 OPEN, USPG46 BOARD를 활성화, 축 초기값 셋팅
        /// </summary>
        /// <param name="nAxisCount">연결된 모든 축 COUNT</param>
        /// <returns></returns>
        public bool InitController(ref int nAxisCount)
         {
            nAxisCount = 0;
            if (MkUnitw.MkUnitwSetUnitType(1) == 0) return false;
             if (MkUnitw.Uspg46wDllOpen() == 1)
            {
                // 2019.07.02. by shkim [FROM HERE] Board Count 0~7 중에 랜덤인지 순차인지 확인 후 수정 필요
                for (short nBoardCount = 0; nBoardCount < 4; nBoardCount++)
                {
                    if (MkUnitw.Uspg46wCreate(ref nBoardCount) == 1)
                    {
                        nAxisCount++;
                    }
                }
                nAxisCount *= 4;
                m_axis = new MkUnitAxis[nAxisCount];

                for (int nAxisIndex = 0;  nAxisIndex < nAxisCount; ++nAxisIndex)
                {
                    m_axis[nAxisIndex] = new MkUnitAxis(nAxisIndex);
                    m_axis[nAxisIndex].InitAxis();
                }
                return true;
            }
            else return false;
        }
        /// <summary>
        /// 2019.07.01. by shkim. [ADD]
        /// </summary>
        /// <param name="m_nCountOfAxises"></param>
        public void ExitController(int m_nCountOfAxises)
        {
            for (short ctrNumber = 0; ctrNumber < m_nCountOfAxises; ctrNumber++)
            {
                MkUnitw.Uspg46wClose(ctrNumber);
            }
            return;
        }

        #region <모션 설정 관련>
        public void SetMotorScale(int nAxis, double dNumerator, double dDenominator)
        {
            m_axis[nAxis].SetMotorScale(dNumerator, dDenominator);
        }
        public void SetMotorType(int nAxis, EN_MOTOR_TYPE enType)
        {
            m_axis[nAxis].SetMotorType(enType);
        }

        public void SetMotorConfiguration(int nAxis, MOTOR_CONFIG_PARAMETER stMotorParam)
        {
            m_axis[nAxis].SetMotorConfiguration(stMotorParam);
        }
        public void SetMotorSpeedConfiguration(int nAxis, MOTOR_SPEED_PARAMETER stSpeedParam)
        {
            m_axis[nAxis].SetMotorSpeedConfiguration(stSpeedParam);
        }
        public void SetMotorHomeConfiguration(int nAxis, MOTOR_HOME_PARAMETER stHomingParam)
        {
            m_axis[nAxis].SetMotorHomingConfiguration(stHomingParam);
        }
        public void SetMotorHomeSpeedConfiguration(int nAxis, MOTOR_HOME_PARAMETER stHomeParam)
        {
            m_axis[nAxis].SetMotorHomeSpeedConfiguration(stHomeParam);
        }
        #endregion </모션 설정 관련>

        #region <모션이동관련>
        
        public void MoveSxAbsolutely(int nAxis, double dblDest, bool bWaitForDone)
        {
            m_axis[nAxis].MoveAbsolutely(dblDest, bWaitForDone);
        }
        public void MoveSxRelatively(int nAxis, double dblDest, bool bWaitForDone)
        {
            m_axis[nAxis].MoveRelatively(dblDest, bWaitForDone);
        }
        public void MoveAtSpeed(int nAxis, bool bDirection, bool bMoveInHomeMode)
        {
            m_axis[nAxis].MoveContinuousDrive(bDirection, bMoveInHomeMode);
        }
        public void StopMotion(int nAxis, bool bEmergency)
        {
            if (bEmergency)
            {
                m_axis[nAxis].SlowStop();
            }
            else
            {
                m_axis[nAxis].EStop();
            }
        }
        public void StopHomeMotion(int nAxis)
        {
            m_axis[nAxis].SlowStop();
        }

        public bool MoveToHome(int nAxis, ref int nSqc, ref uint uDelay)
        {
            return m_axis[nAxis].Homing(ref nSqc, ref uDelay);
        }

        #endregion </모션이동관련>

        #region <모션상태관련>
        // 2019.07.11. by shkim [FROM HERE] 작성필요
        public bool DoServoOn(int nAxis)
        {
            return m_axis[nAxis].DoServoOn();
        }

        // 2019.07.11. by shkim [FROM HERE] 작성필요
        public bool DoServoOff(int nAxis)
        {
            return m_axis[nAxis].DoServoOff();
        }

        public void GetState(int nAxis, ref bool[] arState)
        {
            byte byteDriveStatus = (byte)0;
            byte byteEndStatus = (byte)0;
            byte byteMechanicalStatus = (byte)0;
            byte byteUnivasalStatus = (byte)0;

            // DRIVE STATUS
            m_axis[nAxis].ReadDriveStatus(ref byteDriveStatus);

            // END STATUS READ PORT
             m_axis[nAxis].ReadEndStatus(ref byteEndStatus);

            // MECHANICAL SIGNAL READ PORT
            m_axis[nAxis].ReadMechanicalSignalStatus(ref byteMechanicalStatus);

            // UNIVASAL SIGNAL READ PORT
            m_axis[nAxis].ReadUnivesalSignalStatus(ref byteUnivasalStatus);

            arState[(int)EN_MOTOR_STATE.MOTORON] = IsSignalOn(byteUnivasalStatus, (int)EN_UNIVALSAL_STATUS.DRIVE);
            arState[(int)EN_MOTOR_STATE.ALARM] = IsSignalOn(byteEndStatus, (int)EN_END_STATUS.ALARM_SIGNAL_END);

            arState[(int)EN_MOTOR_STATE.HOMESWITCH] = IsSignalOn(byteUnivasalStatus, (int)EN_UNIVALSAL_STATUS.ORG);

            arState[(int)EN_MOTOR_STATE.LIMITPOSITIVE] = IsSignalOn(byteMechanicalStatus, (int)EN_MECHANICAL_STATUS.ELM_P);
            arState[(int)EN_MOTOR_STATE.LIMITNEGATIVE] = IsSignalOn(byteMechanicalStatus, (int)EN_MECHANICAL_STATUS.ELM_N);
        }

        public double GetCommandPosition(int nAxis)
        {
            return m_axis[nAxis].GetCommandPosition();
        }
        public double GetActualPosition(int nAxis)
        {
            return m_axis[nAxis].GetActualPosition();
        }
        public void SetCommandPosition(int nAxis, double dblPosition)
        {
            m_axis[nAxis].SetCommandPosition(dblPosition);
        }
        public void SetActualPosition(int nAxis, double dblPosition)
        {
            m_axis[nAxis].SetActualPosition(dblPosition);
        }

        public bool IsMotionDone(int nAxis)
        {
            byte byteDriveStatus = (byte)0;
            // DRIVE STATUS
            m_axis[nAxis].ReadDriveStatus(ref byteDriveStatus);
            return IsSignalOn(byteDriveStatus, (int)EN_DRIVE_STATUS.BUSY);
        }
        public bool IsMotorOn(int nAxis)
        {
            return m_axis[nAxis].GetOriginStatus();
        }
        public bool IsHomeEnd(int nAxis)
        {
            return m_axis[nAxis].GetOriginStatus();
        }

        private bool IsSignalOn(byte byteStatus, int targetValue)
         {
            int nResult;
            nResult = (int)byteStatus & targetValue;
            if (nResult > 0) return true;
            else return false;
         }

        #endregion </모션상태관련>
    }
    public class MkUnitAxis
    {
        public enum EN_ORG_STATUS
        {
            ERROR,
            RUNNING,
            END,
        }

        #region 변수
        private short m_nBoardNum;
        private short m_nAxisIndex;

        private double m_dRatioA = 1;   // mm
        private double m_dRatioB = 1;   // pulse

        #endregion

        #region 파라메터
        private EN_MODE1_PULSE_TYPE m_enPulseType;

        private EN_MOTOR_TYPE m_enMotorType;
        private MOTOR_CONFIG_PARAMETER m_stConfigParam;         // 모터 파라메터
        private MOTOR_HOME_PARAMETER m_stHomingParam;           // 홈 관련 파라메터
        private MOTOR_SPEED_PARAMETER m_stSpeedParam;           // 속도 관련 파라메터
        #endregion

        // 보드 하나에 4축씩 관리.
        public MkUnitAxis(int nAxisIndex)
        {
            m_nBoardNum = (short)(nAxisIndex / 4);
            m_nAxisIndex = (short)(nAxisIndex % 4);
        }
        
        /// <summary>
        /// 2019.07.12. by shkim [ADD] 각 축 초기 파라미터 설정
        /// (미나미 예제 Servo On 과정에서 실행)
        /// </summary>
        public void InitAxis()
        {
            byte bData = (byte)0;

            ClearInitial(); // Initial Clear
            MkUnitw.Uspg46wUniversalSignalWrite(m_nBoardNum, m_nAxisIndex, (byte)0); // Universal Write
            
            // Mode1, 2는 각 축별로 다르게 해야한다.
            if(m_enMotorType == EN_MOTOR_TYPE.SERVO)
            {
                WriteMode1(EN_MODE1_DECEL_STARTPOINT.AUTO, EN_MODE1_PULSE_TYPE.TWO_PULSE_H_CW_DIR_H_CCW, EN_MODE1_SIGNAL_SEARCH.E_LIMIT_PLUS_DIR_NEGATIVE_EDGE);
                //MkUnitw.Uspg46wMode1Write(m_nBoardNum, m_nAxisIndex, (byte)0x60); // Mode1 Write
            }
            else
            {
                WriteMode1(EN_MODE1_DECEL_STARTPOINT.AUTO, EN_MODE1_PULSE_TYPE.TWO_PULSE_H_CCW_DIR_H_CW, EN_MODE1_SIGNAL_SEARCH.E_LIMIT_PLUS_DIR_NEGATIVE_EDGE);
                //MkUnitw.Uspg46wMode1Write(m_nBoardNum, m_nAxisIndex, (byte)0x40); // Mode1 Write
            }

            ReadEndStatus(ref bData); // End Status Read
            WriteCommand(EN_USPG46_COMMAND.INPOSITION_WAIT_MODE2_SET); // Inposition Wait Mode Set
            WriteCommand(EN_USPG46_COMMAND.ALARM_STOP_ENABLE_MODE_SET); // Alarm Stop Mode Set
            WriteCommand(EN_USPG46_COMMAND.INTERRUPT_OUT_ENABLE_MODE_RESET);  // Interrupt Mode Reset
            WriteFullData(EN_USPG46_COMMAND.RANGE_WRITE, 0x1fff); // Range Write
            WriteFullData(EN_USPG46_COMMAND.START_STOP_SPEED_DATA_WRITE, 0x0001);// Start/Stop Speed Write
            WriteFullData(EN_USPG46_COMMAND.RATE1_DATA_WRITE, 0x1fff); // Rate1 Write
            WriteFullData(EN_USPG46_COMMAND.RATE2_DATA_WRITE, 0x1fff); // Rate2 Write
            WriteFullData(EN_USPG46_COMMAND.RATE3_DATA_WRITE, 0x1fff); // Rate3 Write
            WriteFullData(EN_USPG46_COMMAND.RATE_CHANGE_POINT_1_2_WRITE, 0x1fff); // Rate1-2 Change Write
            WriteFullData(EN_USPG46_COMMAND.RATE_CHANGE_POINT_2_3_WRITE, 0x1fff); // Rate2-3 Change Write
            WriteFullData(EN_USPG46_COMMAND.SLOW_DOWN_REAR_PULSE_WRITE, 0); // Slow Down Rear Pulse Write
            WriteFullData(EN_USPG46_COMMAND.INTERNAL_COUNTER_WRITE, 0); // Internal Count Write
            WriteFullData(EN_USPG46_COMMAND.INTERNAL_COMPARATE_DATA_WRITE, 0); // Internal Comparate Data Write
            WriteFullData(EN_USPG46_COMMAND.EXTERNAL_COUNTER_WRITE, 0); // External Count Write
            WriteFullData(EN_USPG46_COMMAND.EXTERNAL_COMPARATE_DATA_WRITE, 0); // External Comparate Data Write
            WriteFullData(EN_USPG46_COMMAND.EMERGENCY_LIMIT_ENABLE_MODE_SET, 0);
        }

        #region <설정관련>
        public void SetMotorScale(double dNumerator, double dDenominator)
        {
            m_dRatioA = dNumerator;
            m_dRatioB = dDenominator;
        }
        public void SetMotorType(EN_MOTOR_TYPE enType)
        {
            m_enMotorType = enType;
        }
        public void SetMotorConfiguration(MOTOR_CONFIG_PARAMETER stMotorParam)
        {
            m_stConfigParam = stMotorParam;
            
            // 2019.07.11. by shkim. [FROM HERE] Servo, Step 구분 맞는지 확인필요.
            if (m_enMotorType == EN_MOTOR_TYPE.SERVO)
                m_enPulseType = EN_MODE1_PULSE_TYPE.ONE_PULSE_H_DIR_L_CW_H_CCW; // 유형 0
            else
                m_enPulseType = EN_MODE1_PULSE_TYPE.TWO_PULSE_H_CCW_DIR_H_CW;   // 유형4

        }
        public void SetMotorSpeedConfiguration(MOTOR_SPEED_PARAMETER stSpeedParam)
        {
            m_stSpeedParam = stSpeedParam;
        }

        /// <summary>
        /// 2019.07.02. by shkim. [ADD] Homing 설정
        /// </summary>
        /// <param name="stHomeParam"></param>
        public void SetMotorHomingConfiguration(MOTOR_HOME_PARAMETER stHomeParam)
        {
            m_stHomingParam = stHomeParam;
        }
        /// <summary>
        /// 2019.07.02. by shkim. [ADD] Homing Speed 설정 [Write X, Write 는 Move 직전]
        /// </summary>
        /// <param name="stHomeParam"></param>
        public void SetMotorHomeSpeedConfiguration(MOTOR_HOME_PARAMETER stHomeParam)
        {
            m_stHomingParam = stHomeParam;
        }


        /// <summary>
        /// 2019.07.11. by shkim. [ADD] Speed Pattern에 따라 Speed Parameter를 Pulse형으로 변환하여 MKUNIT 적용
        /// </summary>
        /// <param name="nPositionPulse"></param>
        /// <param name="nStartSpeed"></param>
        private void SetSpeedParam(uint nPositionPulse, short nStartSpeed = 1)
        {
            double dFactor = m_dRatioA / m_dRatioB;
            double dTargetSpeed = m_stSpeedParam.dblVelocity;
            uint nRange, nRate1, nRate2;
            uint nStartSpeedPulse, nObjectSpeedPulse, nDiffernceSpeedPulse, nMiddleSpeedPulse;
            uint nSW1, nSW2;

            nRange = CalculateRange(dFactor, dTargetSpeed);
            nStartSpeedPulse = CalculateSpeed(dFactor, nRange, nStartSpeed);
            nObjectSpeedPulse = CalculateSpeed(dFactor, nRange, dTargetSpeed);
            nRate1 = CalculateRate(nStartSpeedPulse, nObjectSpeedPulse, m_stSpeedParam.dblAccelTime);
            nDiffernceSpeedPulse = nObjectSpeedPulse - nStartSpeedPulse;
            nMiddleSpeedPulse = (uint)(nDiffernceSpeedPulse / 2.0);

            // Linear
            if(m_stSpeedParam.enSpeedPattern == EN_SPEED_PATTERN.TRAPEZOIDAL)
            {
                WriteFullData(EN_USPG46_COMMAND.START_STOP_SPEED_DATA_WRITE, (int)nStartSpeed);
                WriteFullData(EN_USPG46_COMMAND.OBJECT_SPEED_DATA_WRITE, (int)nObjectSpeedPulse);

                WriteFullData(EN_USPG46_COMMAND.RATE1_DATA_WRITE, (int)nRate1);
                WriteFullData(EN_USPG46_COMMAND.RATE_CHANGE_POINT_1_2_WRITE, (int)8191L);
                WriteFullData(EN_USPG46_COMMAND.RATE_CHANGE_POINT_2_3_WRITE, (int)8191L);
            }
            else if(m_stSpeedParam.enSpeedPattern == EN_SPEED_PATTERN.S_CURVE)
            {
                nSW1 = (uint)(nDiffernceSpeedPulse * m_stSpeedParam.dblJerkAccelration / 100);

                if(nMiddleSpeedPulse < nSW1)
                {
                    nSW1 = nMiddleSpeedPulse;
                }
                // 0xfff = 4095
                if((int)nSW1 < 1)
                {
                    nSW1 = 1;
                }
                if (0xfff < nSW1)
                {
                    nSW1 = 0xfff;
                }
                // 0x1fff = 8191
                if((int)nRate1 < 1)
                {
                    nRate1 = 1;
                }
                if (0x1fff < nRate1)
                {
                    nRate1 = 0x1fff;
                }
                WriteFullData(EN_USPG46_COMMAND.START_STOP_SPEED_DATA_WRITE, (int)nStartSpeed);
                WriteFullData(EN_USPG46_COMMAND.OBJECT_SPEED_DATA_WRITE, (int)nObjectSpeedPulse);

                WriteFullData(EN_USPG46_COMMAND.RATE1_DATA_WRITE, (int)nRate1);
                WriteFullData(EN_USPG46_COMMAND.RATE_CHANGE_POINT_1_2_WRITE, (int)8191L);
                WriteFullData(EN_USPG46_COMMAND.RATE_CHANGE_POINT_2_3_WRITE, (int)8191L);
                if( nSW1 >= 1 && nSW1 <= 0xfff)
                {
                    WriteHalfData(EN_USPG46_COMMAND.SW1_DATA_WRITE, (short)nSW1);
                }
                WriteCommand(EN_USPG46_COMMAND.US_S_CURVE_ACCELERATE_MODE_SET);
            }
            else
            {
                // 2019.07.11. by shkim. [FROM HERE] 絶対値移動動作(절대이동동작)? DRIVE_MODE_SDAT?
                // 여기에 예제소스, set_Rate_toObjspd, set_SData 처리 과정추가할 것
            }
        }
        /// <summary>
        /// 2019.07.11. by shkim. [ADD] Homing을 위한 Speed Parameter를 Pulse형으로 변환하여 MKUNIT 적용
        /// 미나미 예제에서 rate_set2, spd_set 하는 과정
        /// </summary>
        private void SetHomeSpeedParam()
        {
            double dFactor = m_dRatioA / m_dRatioB; // dFator = pulse/mm
            double dTargetSpeed;
            uint nRange, nRate1, nRate2;
            uint nStartSpeedPulse, nObjectSpeedPulse;

            dTargetSpeed = m_stHomingParam.dblHomeVelocity;
            nRange = CalculateRange(dFactor, dTargetSpeed);
            nStartSpeedPulse = CalculateSpeed(dFactor, nRange, m_stHomingParam.dblHomeSecondVelocity);
            nObjectSpeedPulse = CalculateSpeed(dFactor, nRange, dTargetSpeed);
            nRate1 = CalculateRate(nStartSpeedPulse, nObjectSpeedPulse, m_stHomingParam.dblHomeAccelTime);
            nRate2 = CalculateRate(nStartSpeedPulse, nObjectSpeedPulse, m_stHomingParam.dblHomeDecelTime);

            WriteFullData(EN_USPG46_COMMAND.RATE1_DATA_WRITE, (int)nRate1);
            WriteFullData(EN_USPG46_COMMAND.RATE_CHANGE_POINT_1_2_WRITE, (int)8191L);
            WriteFullData(EN_USPG46_COMMAND.RATE_CHANGE_POINT_2_3_WRITE, (int)8191L);

            WriteFullData(EN_USPG46_COMMAND.RANGE_WRITE, (int)nRange);
            WriteFullData(EN_USPG46_COMMAND.START_STOP_SPEED_DATA_WRITE, (int)nStartSpeedPulse);
            WriteFullData(EN_USPG46_COMMAND.OBJECT_SPEED_DATA_WRITE, (int)nObjectSpeedPulse);
        }
        /// <summary>
        /// 2019.07.11. by shkim. [ADD] Interner(Command)와 External(Actual) Count를 특정값으로 Setting
        /// </summary>
        private void SetPosition(int nPosition = 0)
        {
            WriteFullData(EN_USPG46_COMMAND.INTERNAL_COUNTER_WRITE, nPosition);
            WriteFullData(EN_USPG46_COMMAND.EXTERNAL_COUNTER_WRITE, nPosition);
        }

        #endregion </설정관련>

        #region <상태관련>

        // 2019.07.11. by shkim [FROM HERE] 작성필요
        public bool DoServoOn()
        {
            if (false == DoServoOff()) return false;

            InitAxis();
            
            if (false == ResetDriver()) return false;

            if (false == DriveSignalOn()) return false;

            return true;
        }

        // 2019.07.11. by shkim [FROM HERE] 작성필요
        public bool DoServoOff()
        {
            SlowStop();
            if (false == DriveSignalOff()) return false;

            // Drive Off Delay 100ms
            Thread.Sleep(100);

            return true;
        }

        public bool ReadDriveStatus(ref byte bDriveStatus)
        {
            if (false == (MkUnitw.Uspg46wGetDriveStatus(m_nBoardNum, m_nAxisIndex, ref bDriveStatus) == 1)
                ? (true) : (false)) return false;
            else return true;
        }
        public bool ReadEndStatus(ref byte bEndStatus)
        {
            if (false == (MkUnitw.Uspg46wGetEndStatus(m_nBoardNum, m_nAxisIndex, ref bEndStatus) == 1)
                ? (true) : (false)) return false;
            else return true;
        }
        /// <summary>
        /// 2019.07.02. by shkim [ADD]
        /// 7 : ECUP, 6 : EDCN, 5 : DEND, 4 : DERR, 3 : -SLM, 2 : +SLM, 1 : -ELM, 0 : +ELM
        /// </summary>
        /// <param name="bMechenicalStatus"></param>
        /// <returns></returns>
        public bool ReadMechanicalSignalStatus(ref byte bMechenicalStatus)
        {
            if (false == (MkUnitw.Uspg46wGetMechanicalSignal(m_nBoardNum, m_nAxisIndex, ref bMechenicalStatus) == 1)
                ? (true) : (false)) return false;
            else return true;
        }
        /// <summary>
        /// 2019.07.02. by shkim. [ADD] 
        /// 6 : IN, 5 : Z-Phase, 4 : ORG, 1 : DRST, 0 : DRIVE (7, 3, 2 : 사용안함)
        /// </summary>
        /// <param name="bUniversalStatus"></param>
        /// <returns></returns>
        public bool ReadUnivesalSignalStatus(ref byte bUniversalStatus)
        {
            if (false == (MkUnitw.Uspg46wGetUniversalSignal(m_nBoardNum, m_nAxisIndex, ref bUniversalStatus) == 1)
                ? (true) : (false)) return false;
            else return true;
        }
        public bool WriteUnivesalSignalStatus(byte bUniversalStatus)
        {
            if (false == (MkUnitw.Uspg46wUniversalSignalWrite(m_nBoardNum, m_nAxisIndex, bUniversalStatus) == 1)
                ? (true) : (false)) return false;
            else return true;
        }

        // 작성중.
        public EN_ORG_STATUS ReadOriginStatus()
        {
            byte bStatus = (byte)0;
            if(false == (MkUnitw.Uspg46wOrgStatus(m_nBoardNum, m_nAxisIndex, ref bStatus) == 1)
                ? (true) : (false))
            {
                return EN_ORG_STATUS.ERROR;
            }
            if ((bStatus & 0x01) != 0) return EN_ORG_STATUS.RUNNING;
            else return EN_ORG_STATUS.END;
        }

        /// <summary>
        /// 2019.07.02. by shkim. [ADD] Get Command Position [mm]
        /// </summary>
        /// <returns></returns>
        public double GetCommandPosition()
        {
            // Command
            int nReadCount = 0;
            if (false == (MkUnitw.Uspg46wDataFullRead(m_nBoardNum, m_nAxisIndex, (byte)EN_USPG46_COMMAND.INTERNAL_COUNTER_READ, ref nReadCount) == 1)
                ? (true) : (false))
            {
                return 0;
            }
            else
            {
                return ConvertMMFromPulse(nReadCount);
            }
        }
        /// <summary>
        /// 2019.07.02. by shkim. [ADD] Get Actual Position [mm]
        /// </summary>
        /// <returns></returns>
        public double GetActualPosition()
        {
            int nReadCount = 0;

            if (false == (MkUnitw.Uspg46wDataFullRead(m_nBoardNum, m_nAxisIndex, (byte)EN_USPG46_COMMAND.EXTERNAL_COUNTER_READ, ref nReadCount) == 1)
                ? (true) : (false))
            {
                return 0;
            }
            else
            {
                return ConvertMMFromPulse(nReadCount);
            }
        }
        /// <summary>
        /// 2019.07.02. by shkim [ADD] Set Command Position
        /// </summary>
        /// <param name="dblPosition">Position [mm]</param>
        public void SetCommandPosition(double dblPosition)
        {
            uint nPulse = 0;
            nPulse = ConvertPulseFromMM(dblPosition);
            WriteFullData(EN_USPG46_COMMAND.INTERNAL_COUNTER_WRITE, (int)nPulse);
        }
        /// <summary>
        /// 2019.07.02. by shkim [ADD] Set Actual Position
        /// </summary>
        /// <param name="dblPosition">Position [mm]</param>
        public void SetActualPosition(double dblPosition)
        {
            uint nPulse = 0;
            nPulse = ConvertPulseFromMM(dblPosition);
            WriteFullData(EN_USPG46_COMMAND.EXTERNAL_COUNTER_WRITE, (int)nPulse);
        }

        /// <summary>
        /// 2019.07.02. by shkim [ADD] Origin 완료 상태 확인
        /// </summary>
        /// <returns></returns>
        public bool GetOriginStatus()
        {
            byte bStatus = (byte)0;

            if (false == (MkUnitw.Uspg46wOrgStatus(m_nBoardNum, m_nAxisIndex, ref bStatus) == 1)
                ? (true) : (false))
            {
                return false;
            }
            if ((bStatus & 0x01) != 1) return false;
            else return true;
        }

        #endregion </상태관련>
        
        #region <모션 이동 관련>
        public void MoveAbsolutely(double dAbsolutePos, bool bWaitForDone)
        {
            // 현재 위치를 받아와서..
            double dCurrentPos = 0;
            double dRelativePos = 0;
            uint nPositionPulse = 0;

            dCurrentPos = GetCommandPosition();
            dRelativePos = dAbsolutePos - dCurrentPos;
            nPositionPulse = (uint)Math.Abs(ConvertPulseFromMM(dRelativePos));
            SetSpeedParam(nPositionPulse);
            if(dRelativePos > 0)
            {
                WriteFullData(EN_USPG46_COMMAND.PLUS_PRESET_PULSE_DRIVE, (int)nPositionPulse);
            }
            else
            {
                WriteFullData(EN_USPG46_COMMAND.MINUS_PRESET_PULSE_DRIVE, (int)nPositionPulse);
            }
        }
        public void MoveRelatively(double dRelativePos, bool bWaitForDone)
        {
            uint nPulse = 0;

            nPulse = (uint)Math.Abs(ConvertPulseFromMM(dRelativePos));
            SetSpeedParam(nPulse);
            if (dRelativePos > 0)
            {
                WriteFullData(EN_USPG46_COMMAND.PLUS_PRESET_PULSE_DRIVE, (int)nPulse);
            }
            else
            {
                WriteFullData(EN_USPG46_COMMAND.MINUS_PRESET_PULSE_DRIVE, (int)nPulse);
            }
        }
        public void MoveContinuousDrive(bool bDirection, bool bMoveInHomeMode)
        {
            SetSpeedParam(0);
            if(bDirection)
            {
                WriteCommand(EN_USPG46_COMMAND.PLUS_CONTINUOUS_DRIVE);
            }
            else
            {
                WriteCommand(EN_USPG46_COMMAND.MINUS_CONTINUOUS_DRIVE);
            }
        }
        public bool Homing(ref int nSqc, ref uint uDelay)
        {
            bool bRet = false;
            byte bDriveStatus = (byte)0;
             byte bEndStatus = (byte)0;

            switch(nSqc)
            {
                case HOMING_SQC.INIT:
                    // Drive 상태를 확인한다.
                    if(false == ReadDriveStatus(ref bDriveStatus))
                    {
                        nSqc = HOMING_SQC.ERROR; break;
                    }
                    // 2019.07.14. by shkim. [FROM HERE] 미나미 예제 참고할 것
                    // Down Singal 켜져있으면 Error 상황으로 Mechenical Signal 확인 후 에러처리
                    if(false != IsSignalOn(bDriveStatus, (int)EN_DRIVE_STATUS.DOWN))
                    {
                        nSqc = HOMING_SQC.ERROR; break;
                    }
                    ++nSqc;
                    break;

                case HOMING_SQC.INIT + 1:
                    if(false == ReadEndStatus(ref bEndStatus))
                    {
                        nSqc = HOMING_SQC.ERROR; break;
                    }
                    if(bEndStatus == 255)
                    {
                        nSqc = HOMING_SQC.ERROR; break;
                    }

                    SetHomeSpeedParam();
                    nSqc = HOMING_SQC.MOVE;
                    break;

                case HOMING_SQC.MOVE:
                    if(m_stHomingParam.nHomeMode == (int)EN_MOTOR_HOMEMODE.EL_TWOSTEP)
                    {
                        if(m_stHomingParam.bHomeDirection) // + 방향
                        {
                            if(false == OriginReturn((int)EN_ORIGIN_MODE.PLUS_DIR_ORG_E_LIMIT))
                            {
                                nSqc = HOMING_SQC.ERROR;
                            }
                        }
                        else
                        {
                            if (false == OriginReturn((int)EN_ORIGIN_MODE.MINUS_DIR_ORG_E_LIMIT))
                            {
                                nSqc = HOMING_SQC.ERROR;
                            }
                        }
                    }
                    else if(m_stHomingParam.nHomeMode == (int)EN_MOTOR_HOMEMODE.ORG_EZ_TWOSTEP_STOP)
                    {
                        if (m_stHomingParam.bHomeDirection) // + 방향
                        {
                            if (false == OriginReturn((int)EN_ORIGIN_MODE.PLUS_DIR_ORG_P_EDGE_BEFORE_Z_N_DEG))
                            {
                                nSqc = HOMING_SQC.ERROR;
                            }
                        }
                        else
                        {
                            if (false == OriginReturn((int)EN_ORIGIN_MODE.MINUS_DIR_ORG_P_EDGE_AFTER_Z_N_EDGE))
                            {
                                nSqc = HOMING_SQC.ERROR;
                            }
                        }
                    }
                    ++nSqc; break;

                case HOMING_SQC.MOVE + 1:
                    EN_ORG_STATUS enOrgStatus;
                    enOrgStatus = ReadOriginStatus();
                    if(enOrgStatus == EN_ORG_STATUS.ERROR)
                    {
                        nSqc = HOMING_SQC.ERROR; break;
                    }
                    else if(enOrgStatus == EN_ORG_STATUS.RUNNING)
                    {
                        break;
                    }
                    if (false == ReadEndStatus(ref bEndStatus))
                    {
                        nSqc = HOMING_SQC.ERROR; break;
                    }
                    else if (IsSignalOn(bEndStatus, (int)EN_END_STATUS.NO_ERROR)) break;

                    nSqc = HOMING_SQC.SET_ZERO; break;

                case HOMING_SQC.SET_ZERO:
                    

                    nSqc = HOMING_SQC.OFFSET_MOVE; break;

                case HOMING_SQC.OFFSET_MOVE:
                    // 생략
                    nSqc = HOMING_SQC.STOP; break;

                case HOMING_SQC.ERROR:
                case HOMING_SQC.STOP:
                    break;
            }
            return bRet;
        }

        public void SlowStop()
        {
            WriteCommand(EN_USPG46_COMMAND.SLOW_DOWN_STOP);
        }
        public void EStop()
        {
            WriteCommand(EN_USPG46_COMMAND.EMERGENCY_STOP);
        }


        #endregion </모션 이동 관련>

        #region <내부함수>

        private bool ResetDriver()
        {
            if (false == DriverResetSignalOn()) return false;

            // 2019.07.12. by shkim. [ADD] Driver Reset On Delay
            Thread.Sleep(1000);

            if (false == DriverResetSignalOff()) return false;

            // 2019.07.12. by shkim. [FROM HERE] Driver Reset Off Delay
            Thread.Sleep(100);

            return true;
        }
        private bool DriverResetSignalOn()
        {
            byte bData = (byte)0;
            if (false == ReadUnivesalSignalStatus(ref bData)) return false;

            bData |= 0x02; // DRST BIT ON;
            return WriteUnivesalSignalStatus(bData);
        }
        private bool DriverResetSignalOff()
        {
            byte bData = (byte)0;
            if (false == ReadUnivesalSignalStatus(ref bData)) return false;

            bData &= 0x0d; // DRST BIT Off;
            return WriteUnivesalSignalStatus(bData);
        }
        private bool DriveSignalOn()
        {
            byte bData = (byte)0;
            if (false == ReadUnivesalSignalStatus(ref bData)) return false;

            bData |= 0x01; // DRIVE BIT ON;
            return WriteUnivesalSignalStatus(bData);
        }
        private bool DriveSignalOff()
        {
            byte bData = (byte)0;
            if (false == ReadUnivesalSignalStatus(ref bData)) return false;

            bData &= 0x0e; // DRIVE BIT OFF;
            return WriteUnivesalSignalStatus(bData);
        }


        private void ClearInitial()
        {
            WriteCommand(EN_USPG46_COMMAND.INITIAL_CLEAR);
        }
        
        
        private bool MoveFindSignal(EN_USPG46_COMMAND enMoveType, bool bFastMove = true)
        {
            switch(enMoveType)
            {
                case EN_USPG46_COMMAND.PLUS_SIGNAL_SEARCH1_DRIVE:
                case EN_USPG46_COMMAND.MINUS_SIGNAL_SEARCH1_DRIVE:
                case EN_USPG46_COMMAND.PLUS_SIGNAL_SEARCH2_DRIVE:
                case EN_USPG46_COMMAND.MINUS_SIGNAL_SEARCH2_DRIVE:
                    return WriteCommand(enMoveType);
                default:
                    return false;
            }
        }
#region 삭제예정
//         private void OriginModeSet(MOTOR_HOME_PARAMETER stHomeParam, bool bIsSignalOn = false)
//         {
//             List<EN_USER_ORG_MODE> listMode = new List<EN_USER_ORG_MODE>();
//             List<EN_USER_ORG_DIR> listDir = new List<EN_USER_ORG_DIR>();
//             List<EN_USER_ORG_SENS_EDGE> listSensor = new List<EN_USER_ORG_SENS_EDGE>();
//             List<EN_USER_ORG_SPEED> listSpeed = new List<EN_USER_ORG_SPEED>();
// 
//             #region ONE STEP TABLE 구현
//             // TABLE 0
//             listMode.Add(EN_USER_ORG_MODE.ON_SIGNAL_NEXT_SQC);
// 
//             if (stHomeParam.enHomeMode == EN_MOTOR_HOMEMODE.ORG_ONESTEP || stHomeParam.enHomeMode == EN_MOTOR_HOMEMODE.ORG_TWOSTEP)
//             {
//                 if (stHomeParam.bHomeDirection) listDir.Add(EN_USER_ORG_DIR.MINUS);
//                 else listDir.Add(EN_USER_ORG_DIR.PLUS);
//                 if (stHomeParam.bHomeLogic) listSensor.Add(EN_USER_ORG_SENS_EDGE.ORG_N_EDGE);
//                 else listSensor.Add(EN_USER_ORG_SENS_EDGE.ORG_P_EDGE);
//             }
//             else
//             {
//                 if (stHomeParam.bHomeDirection)
//                 {
//                     listDir.Add(EN_USER_ORG_DIR.MINUS);
//                     if (stHomeParam.bHomeLogic) listSensor.Add(EN_USER_ORG_SENS_EDGE.E_LIMIT_N_DIR_N_EDGE);
//                     else listSensor.Add(EN_USER_ORG_SENS_EDGE.E_LIMIT_N_DIR_P_EDGE);
//                 }
//                 else
//                 {
//                     listDir.Add(EN_USER_ORG_DIR.PLUS);
//                     if (stHomeParam.bHomeLogic) listSensor.Add(EN_USER_ORG_SENS_EDGE.E_LIMIT_P_DIR_N_EDGE);
//                     else listSensor.Add(EN_USER_ORG_SENS_EDGE.E_LIMIT_P_DIR_P_EDGE);
//                 }
//             }
//             if (bIsSignalOn) listSpeed.Add(EN_USER_ORG_SPEED.START_STOP_SPEED);
//             else listSpeed.Add(EN_USER_ORG_SPEED.MAX_SPEED);
// 
// 
//             /// TABLE 1
//             listMode.Add(EN_USER_ORG_MODE.ON_SIGNAL_BACKWARD_MOVE);
// 
//             if (stHomeParam.enHomeMode == EN_MOTOR_HOMEMODE.ORG_ONESTEP || stHomeParam.enHomeMode == EN_MOTOR_HOMEMODE.ORG_TWOSTEP)
//             {
//                 if (stHomeParam.bHomeDirection) listDir.Add(EN_USER_ORG_DIR.MINUS);
//                 else listDir.Add(EN_USER_ORG_DIR.PLUS);
//                 if (stHomeParam.bHomeLogic) listSensor.Add(EN_USER_ORG_SENS_EDGE.ORG_P_EDGE);
//                 else listSensor.Add(EN_USER_ORG_SENS_EDGE.ORG_N_EDGE);
//             }
//             else
//             {
//                 if (stHomeParam.bHomeDirection)
//                 {
//                     listDir.Add(EN_USER_ORG_DIR.MINUS);
//                     if (stHomeParam.bHomeLogic) listSensor.Add(EN_USER_ORG_SENS_EDGE.E_LIMIT_N_DIR_P_EDGE);
//                     else listSensor.Add(EN_USER_ORG_SENS_EDGE.E_LIMIT_N_DIR_N_EDGE);
//                 }
//                 else
//                 {
//                     listDir.Add(EN_USER_ORG_DIR.PLUS);
//                     if (stHomeParam.bHomeLogic) listSensor.Add(EN_USER_ORG_SENS_EDGE.E_LIMIT_P_DIR_P_EDGE);
//                     else listSensor.Add(EN_USER_ORG_SENS_EDGE.E_LIMIT_P_DIR_N_EDGE);
//                 }
//             }
//             listSpeed.Add(EN_USER_ORG_SPEED.START_STOP_SPEED);
// 
//             // TABLE 2
//             listMode.Add(EN_USER_ORG_MODE.ON_SIGNAL_NEXT_SQC);
//             listDir.Add(EN_USER_ORG_DIR.PLUS);
//             listSensor.Add(EN_USER_ORG_SENS_EDGE.E_LIMIT_P_DIR_N_EDGE);
//             listSpeed.Add(EN_USER_ORG_SPEED.MAX_SPEED);
// #endregion
// 
//             for(short i = 0; i < 3 ; i++)
//             {
//                 OriginTableSet(i, listMode[i], listDir[i], 2, listSensor[i], listSpeed[i]);
//             }
//             listMode.Clear(); listMode = null;
//             listDir.Clear(); listDir = null;
//             listSensor.Clear(); listSensor = null;
//             listSpeed.Clear(); listSpeed = null;
//         }
//         /// <summary>
//         /// 2019.07.01. by shkim. [ADD] Origin Table Setting
//         /// </summary>
//         /// <param name="nTableNumber">Table 번호</param>
//         /// <param name="nMode">모드(0~10)</param>
//         /// <param name="nDir">방향 (0:-, 1:+)</param>
//         /// <param name="nWait">엣지신호검출시간(n:0~7)(n * 100msec)</param>
//         /// <param name="nSensEdge">엣지</param>
//         /// <param name="nSpeed">스피드(0:Object, 1: Start/Stop)</param>
//         private void OriginTableSet(short nTableNumber, EN_USER_ORG_MODE enMode, EN_USER_ORG_DIR enDir, short nWait, EN_USER_ORG_SENS_EDGE enSensEdge, EN_USER_ORG_SPEED enSpeed)
//         {
//             MkUnitw.Uspg46wOriginTableSet(m_nBoardNum, m_nAxisIndex, nTableNumber, (short)enMode, (short)enDir, nWait, (short)enSensEdge, (short)enSpeed);
//         }
#endregion
        
        private bool OriginReturn(int nOriginMode)
        {
            if(false == (MkUnitw.Uspg46wOriginReturn(m_nBoardNum, m_nAxisIndex, (short)nOriginMode) == 1)
                ? (true) : (false))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int WriteMode1(EN_MODE1_DECEL_STARTPOINT enDecelStartPoint, EN_MODE1_PULSE_TYPE enPulseType, EN_MODE1_SIGNAL_SEARCH enSignalSearch)
        {
            int nData;

            nData = ((int)enDecelStartPoint << (int)EN_MODE1_DECEL_STARTPOINT.SHIFT)
                + ((int)enPulseType << (int)EN_MODE1_PULSE_TYPE.SHIFT)
                + ((int)enSignalSearch);

            return MkUnitw.Uspg46wMode1Write(m_nBoardNum, m_nAxisIndex, Convert.ToByte(nData)); ;
        }
        private int WriteMode2(List<EN_MODE2> list)
        {
            int nListCount = list.Count;
            uint nData = 0;

            for (int i = 0; i < nListCount; i++ )
            {
                nData += (uint)list[i];
            }
            list.Clear();
            list = null;
            return MkUnitw.Uspg46wMode2Write(m_nBoardNum, m_nAxisIndex, Convert.ToByte(nData)); ;
        }

        private bool WriteCommand(EN_USPG46_COMMAND enCommand)
        {
            if (false == (MkUnitw.Uspg46wCommandWrite(m_nBoardNum, m_nAxisIndex, (byte)enCommand) == 1)
                ? (true) : (false))
            {
                return false;
            }
            return true;
        }

        private int WriteHalfData(EN_USPG46_COMMAND enCommand, short nData)
        {
            return MkUnitw.Uspg46wDataHalfWrite(m_nBoardNum, m_nAxisIndex, (byte)enCommand, nData);
        }
        private int WriteFullData(EN_USPG46_COMMAND enCommand, int nData)
        {
            return MkUnitw.Uspg46wDataFullWrite(m_nBoardNum, m_nAxisIndex, (byte)enCommand, nData);
        }

        private int ReadHalfData(EN_USPG46_COMMAND enCommand, int nData)
        {
            return MkUnitw.Uspg46wDataFullRead(m_nBoardNum, m_nAxisIndex, (byte)enCommand, ref nData);
        }

        private int ReadFullData(EN_USPG46_COMMAND enCommand, ref int nData)
        {
            return MkUnitw.Uspg46wDataFullRead(m_nBoardNum, m_nAxisIndex, (byte)enCommand, ref nData);
        }


        #region <CALCULATE>
        private uint ConvertPulseFromMM(double dPosition)
        {
            return (uint)(dPosition * m_dRatioB / m_dRatioA);
        }
        private double ConvertMMFromPulse(int dPulse)
        {
            return dPulse * m_dRatioA / m_dRatioB;
        }

        /// <summary>
        /// Range = (8191 * 500) / (ObjectSpeed[mm/sec] * factor[pulse/mm] ) [MINAMI 소스 참조 - IROIRO.C]
        /// </summary>
        /// <param name="dFactor">dRatioB/dRatioA [pulse/mm]</param>
        /// <param name="dObjectSpeed">Max Speed[mm/sec]</param>
        /// <returns></returns>
        private uint CalculateRange(double dFactor, double dObjectSpeed)
        {
            uint dTempRange;

            if ((dObjectSpeed <= 0.0) || (dFactor <= 0.0)) return 8191;

            dTempRange = (uint)(4095500.0 / (dObjectSpeed * dFactor));
            if (dTempRange > 8191.0) return 8191;

            dTempRange = (uint)(dTempRange + 0.5);

            if (dTempRange < 1) return 1;

            return dTempRange;
        }
        /// <summary>
        /// Speed = ( Speed [mm/sec] * factor [pulse / mm] * Range ) / 500  [MINAMI 소스 참조 - IROIRO.C]
        /// </summary>
        /// <param name="dFactor">dRatioB/dRatioA[pulse/mm]</param>
        /// <param name="nRange">Range [500/Funit]</param>
        /// <param name="dSpeed">Max Speed[mm/sec]</param>
        /// <returns></returns>
        private uint CalculateSpeed(double dFactor, uint nRange, double dSpeed)
        {
            uint tempSpeed;
            tempSpeed = (uint)((dFactor * (double)nRange * dSpeed) / 500.0);
            if (tempSpeed > 8191.0) return 8191;

            tempSpeed = (uint)(tempSpeed + 0.5);

            if (tempSpeed < 1) return 1;
            return tempSpeed;
        }

        /// <summary>
        /// Rate = ( Accel/Decel Time [msec] x 2.048 x 10^3 ) / (Object Speed Pulse - Start Speed Pulse)
        /// </summary>
        /// <param name="nStartSpeedPulse"></param>
        /// <param name="nObjectSpeedPulse"></param>
        /// <param name="dAccelDecelTime"></param>
        /// <returns></returns>
        private uint CalculateRate(uint nStartSpeedPulse, uint nObjectSpeedPulse, double dAccelDecelTime)
        {
            uint nDifferenceSpeedPulse;
            uint tmpRate;

            nDifferenceSpeedPulse = nObjectSpeedPulse - nStartSpeedPulse;
            if (nDifferenceSpeedPulse < 1) return 1;

            tmpRate = (uint)((dAccelDecelTime * 2.048e+03) / (double)nDifferenceSpeedPulse);

            if (tmpRate > 8191.0) return 8191;

            tmpRate = (uint)(tmpRate + 0.5);

            if (tmpRate < 1) return 1;

            return tmpRate;
        }
        #endregion </CALCULATE>

        private bool IsSignalOn(byte byteStatus, int targetValue)
        {
            int nResult;
            nResult = (int)byteStatus & targetValue;
            if (nResult != 0) return true;
            else return false;
        }
        #endregion </내부함수>
    }
}