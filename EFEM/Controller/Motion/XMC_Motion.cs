using Motion_;
using System;
using XMC_SDK = XMC_SDK_.Motion;

namespace FrameOfSystem3.Controller.Motion
{
    public class XMC_Motion : MotionController
    {
        public XMC_Motion()
        {

        }

        #region <ControllerConnection>

        // 2023.10.24. by shkim. 확인완료 (SYNC_VECTOR_MOTION 제외)
        public override bool InitController()
        {
            return XMC_SDK.XMCMotion.GetInstance().InitController();
        }

        // 2023.10.24. by shkim. 확인완료
        public override void ExitController()
        {
            return;
        }

        // 2023.10.24. by shkim. 확인완료
        public override bool ConnectLink()
        {
            return true;
        }

        // 2023.10.24. by shkim. 확인완료
        public override bool IsLinkConnected()
        {
            return XMC_SDK.XMCMotion.GetInstance().IsLinkConnected();
        }

        #endregion </ControllerConnection>

        #region <MotorConfiguration>

        // 2023.11.07. by shkim. 확인완료 => Motion Count를 받아오게함(SyncMotion을 위해 MS를 위해)
        public override int GetCountOfAxis()
        {
            return XMC_SDK.XMCMotion.GetInstance().GetTotalCountOfAxis();
        }

        // 2023.11.07. by shkim. 확인완료 => 기존의 축은 Axis, Motor 정보, SyncMotion을 위한 MS는 Axis 정보를 묶어서 상태 만들어서 반환
        public override void GetMotorInformationAll(ref int nCountOfAxis, ref int[] arState, ref double[] arActualPosition, ref double[] arCommandPosition)
        {
            XMC_SDK.XMCMotion.GetInstance().GetMotorInformationAll(ref nCountOfAxis, ref arState, ref arActualPosition, ref arCommandPosition);

            return;
        }

        // 2023.11.07. by shkim. 확인완료 => Amp Fault면 Clear Fault 까지, 아니면 Motion Reset만.
        public override void DoReset(ref int nAxis)
        {
            XMC_SDK.XMCMotion.GetInstance().DoReset(nAxis);
            return;
        }

        public override bool IsMotionDone(ref int nAxis)
        {
            return XMC_SDK.XMCMotion.GetInstance().IsMotionDone(nAxis);
        }

        
        public override bool IsMotorOn(ref int nAxis)
        {
            return XMC_SDK.XMCMotion.GetInstance().IsMotorOn(nAxis);
        }

        // 2023.11.07. by shkim. 확인완료 => Motion Event Clear 후, Servo On/Off
        public override void DoMotorOn(ref int nAxis, ref bool bON)
        {
#if _XMC_INIT_ERROR
            if (false == bON)
            {
                return;
            }
#endif

            XMC_SDK.XMCMotion.GetInstance().DoMotorOn(nAxis, bON);
            return;
        }

        public override bool IsHomeDone(ref int nAxis)
        {
            return XMC_SDK.XMCMotion.GetInstance().IsHomeDone(nAxis);
        }

        public override void SetActualPosition(ref int nAxis, ref double dblPosition)
        {
            XMC_SDK.XMCMotion.GetInstance().SetActualPosition(nAxis, dblPosition);
            return;
        }

        // 2023.11.07. by shkim. 확인완료
        public override void SetCommandPosition(ref int nAxis, ref double dblPosition)
        {
            XMC_SDK.XMCMotion.GetInstance().SetCommandPosition(nAxis, dblPosition);
            return;
        }

        // 2023.11.07. by shkim. 확인완료
        public override void GetAbsolutePosition(ref int nAxis, ref double dblPosition)
        {
            XMC_SDK.XMCMotion.GetInstance().GetAbsolutePosition(nAxis, ref dblPosition);
            return;
        }

        public override void GetCommandVelocity(ref int nAxis, ref double dblCommandVelocity)
        {
            XMC_SDK.XMCMotion.GetInstance().GetCommandVelocity(nAxis, ref dblCommandVelocity);
        }

        public override void GetActualVelocity(ref int nAxis, ref double dblActualVelocity)
        {
            XMC_SDK.XMCMotion.GetInstance().GetActualVelocity(nAxis, ref dblActualVelocity);
        }

        // [CHECKS] MPI 기준으로 Servo, Step을 기준으로 Servo On이 불필요한 경우를 나눴으나 확인필요
        public override void SetMotorConfiguration(ref int nAxis, ref PARAM_CONFIG paramConfig)
        {
            XMC_SDK.PARAM_CONFIG xmcParamConfig = new XMC_SDK.PARAM_CONFIG();

            if (paramConfig.enMotorType == MOTOR_TYPE.SERVO)
                xmcParamConfig.bUseServoOn = true;
            else
                xmcParamConfig.bUseServoOn = false;

            xmcParamConfig.bUseSecondaryIndex = false;

            xmcParamConfig.bHWLimitLogicNegative = paramConfig.bHWLimitLogicNegative;
            xmcParamConfig.bHWLimitLogicPositive = paramConfig.bHWLimitLogicPositive;
            xmcParamConfig.bHWLimitStopModePositive = paramConfig.bHWLimitStopModePositive;
            xmcParamConfig.bHWLimitStopModeNegative = paramConfig.bHWLimitStopModeNegative;
            xmcParamConfig.bSWLimitStopModeNegative = paramConfig.bSWLimitStopModeNegative;
            xmcParamConfig.bSWLimitStopModeNegative = paramConfig.bSWLimitStopModeNegative;

            xmcParamConfig.bUseHWLimitNegative = paramConfig.bUseHWLimitNegative;
            xmcParamConfig.bUseHWLimitPositive = paramConfig.bUseHWLimitPositive;
            xmcParamConfig.bUseSWLimitNegative = paramConfig.bUseSWLimitNegative;
            xmcParamConfig.bUseSWLimitPositive = paramConfig.bUseSWLimitPositive;

            xmcParamConfig.dSWLimitPositionNegative = paramConfig.dblSWLimitPositionNegative;
            xmcParamConfig.dSWLimitPositionPositive = paramConfig.dblSWLimitPositionPositive;

            XMC_SDK.XMCMotion.GetInstance().SetMotorConfiguration(nAxis, xmcParamConfig);
            return;
        }

        // 2023.11.07. by shkim. 확인완료
        public override void SetMotorSpeedConfiguration(ref int nAxis, ref PARAM_SPEED paramSpeed)
        {
            XMC_SDK.PARAM_SPEED xmcParamSpeed = new XMC_SDK.PARAM_SPEED();
            xmcParamSpeed.dVelocity = paramSpeed.dblVelocity;
            xmcParamSpeed.dAcceleration = paramSpeed.dblAcceleration;
            xmcParamSpeed.dDeceleration = paramSpeed.dblDeceleration;

            if (paramSpeed.enSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
            {
                xmcParamSpeed.dJerkAcceleration = 0;
                xmcParamSpeed.dJerkAcceleration = 0;
            }
            else
            {
                xmcParamSpeed.dJerkAcceleration = paramSpeed.dblAccelJerk;
                xmcParamSpeed.dJerkDeceleration = paramSpeed.dblDecelJerk;
            }

            XMC_SDK.XMCMotion.GetInstance().SetMotorSpeedConfiguration(nAxis, xmcParamSpeed);

            return;
        }

        // 2023.11.07. by shkim. 확인완료 => AA 설비 센서 유무 문제로 Home + Index를 활용한 Touch Probe만 확인
        public override void SetMotorHomeConfiguration(ref int nAxis, ref PARAM_HOME paramHome)
        {
            XMC_SDK.PARAM_HOME xmcParamHome = new XMC_SDK.PARAM_HOME();
            xmcParamHome.dVelocity = paramHome.dblHomeVelocityStart;
            xmcParamHome.dAcceleration = paramHome.dblHomeAcceleration;
            xmcParamHome.dDeceleration = paramHome.dblHomeDeceleration;
            xmcParamHome.dHomeVelocityLast = paramHome.dblHomeVelocityLast;

            if (paramHome.enHomeSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
            {
                xmcParamHome.dJerkAcceleration = 0;
                xmcParamHome.dJerkAcceleration = 0;
            }
            else
            {
                xmcParamHome.dJerkAcceleration = paramHome.dblHomeAccelJerk;
                xmcParamHome.dJerkAcceleration = paramHome.dblHomeDecelJerk;
            }

            xmcParamHome.bHomeDirection = paramHome.bHomeDirection;
            xmcParamHome.bHomeLogic = paramHome.bHomeLogic;
            xmcParamHome.nHomeMode = (int)paramHome.enHomeMode;
            xmcParamHome.dHomeBasePosition = paramHome.dblHomeBasePosition;
            xmcParamHome.dHomeEscapeDist = paramHome.dblHomeEscapeDist;
            xmcParamHome.dHomeOffset = paramHome.dblHomeOffset;
            xmcParamHome.dAbsoluteHomePosition = paramHome.dblAbsoluteHomePosition;

            XMC_SDK.XMCMotion.GetInstance().SetMotorHomeConfiguration(nAxis, xmcParamHome);
            return;
        }

        // 2023.11.07. by shkim. 확인완료
        public override void SetMotorHomeSpeedConfiguration(ref int nAxis, ref PARAM_HOME paramHome)
        {
            XMC_SDK.PARAM_HOME xmcParamHome = new XMC_SDK.PARAM_HOME();
            xmcParamHome.dVelocity = paramHome.dblHomeVelocityStart;
            xmcParamHome.dAcceleration = paramHome.dblHomeAcceleration;
            xmcParamHome.dDeceleration = paramHome.dblHomeDeceleration;
            xmcParamHome.dHomeVelocityLast = paramHome.dblHomeVelocityLast;

            if (paramHome.enHomeSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
            {
                xmcParamHome.dJerkAcceleration = 0;
                xmcParamHome.dJerkAcceleration = 0;
            }
            else
            {
                xmcParamHome.dJerkAcceleration = paramHome.dblHomeAccelJerk;
                xmcParamHome.dJerkAcceleration = paramHome.dblHomeDecelJerk;
            }

            xmcParamHome.bHomeDirection = paramHome.bHomeDirection;
            xmcParamHome.bHomeLogic = paramHome.bHomeLogic;
            xmcParamHome.nHomeMode = (int)paramHome.enHomeMode;
            xmcParamHome.dHomeBasePosition = paramHome.dblHomeBasePosition;
            xmcParamHome.dHomeEscapeDist = paramHome.dblHomeEscapeDist;
            xmcParamHome.dHomeOffset = paramHome.dblHomeOffset;
            xmcParamHome.dAbsoluteHomePosition = paramHome.dblAbsoluteHomePosition;

            XMC_SDK.XMCMotion.GetInstance().SetMotorHomeConfiguration(nAxis, xmcParamHome);
            return;
        }

        /// <summary>
        /// (필요없음) XMC 현재 PID만 지원
        /// </summary>
        public override void SetGainType(ref int nAxis, ref int nIndexOfGainType)
        {
            return;
        }

        // 2023.11.07. by shkim. 확인완료
        public override void SetGainTable(ref int nAxis, ref int nIndexOfTable)
        {
            XMC_SDK.XMCMotion.GetInstance().SetGainTable(nAxis, nIndexOfTable);
            return;
        }

        // 2023.11.07. by shkim. 확인완료
        public override bool GetGainTable(ref int nAxis, ref int nIndexOfTable)
        {
            return XMC_SDK.XMCMotion.GetInstance().GetGainTable(nAxis, ref nIndexOfTable);
        }

        public override void SetOutputOffset(ref int nIndexOfItem, ref double dblOffsetValue)
        {
            XMC_SDK.XMCMotion.GetInstance().SetOutputOffset(nIndexOfItem, dblOffsetValue);
        }

        public override double GetOutputOffset(ref int nIndexOfItem)
        {
            return XMC_SDK.XMCMotion.GetInstance().GetOutputOffset(nIndexOfItem);
        }

        public override void SetOutputLimit(ref int nIndexOfItem, ref double dblHighLimit, ref double dblLowLimit)
        {
            XMC_SDK.XMCMotion.GetInstance().SetOutputLimit(nIndexOfItem, dblHighLimit, dblLowLimit);
        }

        public override void SetGantry(ref int nAxis, ref PARAM_GANTRY paramGantry, ref PARAM_SPEED paramSpeed)
        {
            XMC_SDK.XMCMotion.GetInstance().SetGantry(paramGantry.nIndexOfMaster, paramGantry.nIndexOfSlave, paramGantry.bEnableGantry);
            return;
        }

#endregion </MotorConfiguration>

        #region <MoveInterface>

        // 2023.11.07. by shkim. 확인완료 => AA 설비 센서 유무 문제로 Home + Index를 활용한 Touch Probe만 확인
        public override bool MoveToHome(ref int nAxis, ref int nSeqNum, ref int nDelay)
        {
            return XMC_SDK.XMCMotion.GetInstance().MoveToHome(nAxis, ref nSeqNum, ref nDelay);
        }

        // 2023.11.07. by shkim. 확인완료
        public override void MoveAbsolutely(ref int nAxis, ref double dblDestination)
        {
            XMC_SDK.XMCMotion.GetInstance().MoveAbsolutely(nAxis, dblDestination);
            return;
        }

        // 2023.11.07. by shkim. 확인완료
        public override void MoveReleatively(ref int nAxis, ref double dblDistance)
        {
            XMC_SDK.XMCMotion.GetInstance().MoveRelatively(nAxis, dblDistance);
            return;
        }

        // 2023.11.07. by shkim. 확인완료
        public override void MoveAtSpeed(ref int nAxis, ref bool bDirection)
        {
            XMC_SDK.XMCMotion.GetInstance().MoveAtSpeed(nAxis, bDirection);
            return;
        }

        // 2023.11.07. by shkim. 확인완료
        public override void MoveByList(ref int nAxis, ref int nCount, ref double[] arDestination, ref PARAM_SPEED[] arSpeedParam)
        {
            XMC_SDK.PARAM_SPEED[] arrSpeed = new XMC_SDK.PARAM_SPEED[nCount];
            for (int i = 0; i < nCount; ++i)
            {
                arrSpeed[i] = new XMC_SDK.PARAM_SPEED();
                arrSpeed[i].dVelocity = arSpeedParam[i].dblVelocity;
                arrSpeed[i].dAcceleration = arSpeedParam[i].dblAcceleration;
                arrSpeed[i].dDeceleration = arSpeedParam[i].dblDeceleration;

                if (arSpeedParam[i].enSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
                {
                    arrSpeed[i].dJerkAcceleration = 0;
                    arrSpeed[i].dJerkAcceleration = 0;
                }
                else
                {
                    arrSpeed[i].dJerkAcceleration = arSpeedParam[i].dblAccelJerk;
                    arrSpeed[i].dJerkDeceleration = arSpeedParam[i].dblDecelJerk;
                }
            }

            XMC_SDK.XMCMotion.GetInstance().MoveByList(nAxis, nCount, arDestination, arrSpeed);

            return;
        }

        // [CHECKS] 2023.11.02. by shkim. RunningTask에서 OverrideMotion으로 줬는데, EN_OVERRIDE_TYPE == Speed... 버그 확인필요
        public override void OverrideMotion(ref int nAxis, ref EN_OVERRIDE_TYPE enTypeOfOverride, ref double dblDestination, ref PARAM_SPEED paramSpeed)
        {
            XMC_SDK.PARAM_SPEED xmcParamSpeed = new XMC_SDK.PARAM_SPEED();

            xmcParamSpeed = new XMC_SDK.PARAM_SPEED();
            xmcParamSpeed.dVelocity = paramSpeed.dblVelocity;
            xmcParamSpeed.dAcceleration = paramSpeed.dblAcceleration;
            xmcParamSpeed.dDeceleration = paramSpeed.dblDeceleration;

            if (paramSpeed.enSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
            {
                xmcParamSpeed.dJerkAcceleration = 0;
                xmcParamSpeed.dJerkAcceleration = 0;
            }
            else
            {
                xmcParamSpeed.dJerkAcceleration = paramSpeed.dblAccelJerk;
                xmcParamSpeed.dJerkDeceleration = paramSpeed.dblDecelJerk;
            }

            if (enTypeOfOverride == EN_OVERRIDE_TYPE.SPEED)
            {
                XMC_SDK.XMCMotion.GetInstance().OverrideSpeed(nAxis, xmcParamSpeed);
            }
            else // speed, position 모두? EN_OVERRIDE_TYPE.SPEED, EN_OVERRIDE_TYPE.ALL, Modify Motion을 의미한다.
            {
                XMC_SDK.XMCMotion.GetInstance().OverrideMotion(nAxis, dblDestination, xmcParamSpeed);
            }

            return;
        }

        // [미사용] MoveSyncVectorMotion으로 대체
        public override void MoveByLinearCoordination(ref int nAxisCount, ref int[] arIndexes, ref int nCountOfStep, ref double[,] arDestination, ref PARAM_SPEED[,] arSpeedParam)
        {
            return;
        }

        // 2D 또는 3D 직교좌표 공간에서 단일 직선이동을 실행하기 위한 명령 (모든 축의 스케일 동일) (by jhchoo)
        public override void MoveSyncVectorMotion(ref int nAxisCount, ref int[] arAxes, ref double[] arDestination, ref PARAM_SPEED paramSpeed, ref bool bAbsolute, ref bool bOverride)
        {
            XMC_SDK.PARAM_SPEED xmcParamSpeed = new XMC_SDK.PARAM_SPEED();

            xmcParamSpeed = new XMC_SDK.PARAM_SPEED();
            xmcParamSpeed.dVelocity = paramSpeed.dblVelocity;
            xmcParamSpeed.dAcceleration = paramSpeed.dblAcceleration;
            xmcParamSpeed.dDeceleration = paramSpeed.dblDeceleration;

            if (paramSpeed.enSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
            {
                xmcParamSpeed.dJerkAcceleration = 0;
                xmcParamSpeed.dJerkAcceleration = 0;
            }
            else
            {
                xmcParamSpeed.dJerkAcceleration = paramSpeed.dblAccelJerk;
                xmcParamSpeed.dJerkDeceleration = paramSpeed.dblDecelJerk;
            }

            XMC_SDK.XMCMotion.GetInstance().MoveSyncVectorMotion(nAxisCount, arAxes, arDestination, xmcParamSpeed, bAbsolute, bOverride);
            return;
        }

        // 2D 또는 3D 직교좌표 공간에서 단일 직선이동을 실행하기 위한 특정 트리거 조건 명령 (모든 축의 스케일 동일)     (by jhchoo)
        public override void MoveSyncVectorMotionCompare(ref int nAxisCount, ref int[] arAxes, ref double[] arDestination, ref PARAM_SPEED paramSpeed, ref bool bAbsolute, ref int nCompareAxis, ref double dComparePosition, ref bool bLOGIC, ref bool bACTUAL)
        {
            XMC_SDK.PARAM_SPEED xmcParamSpeed = new XMC_SDK.PARAM_SPEED();

            xmcParamSpeed = new XMC_SDK.PARAM_SPEED();
            xmcParamSpeed.dVelocity = paramSpeed.dblVelocity;
            xmcParamSpeed.dAcceleration = paramSpeed.dblAcceleration;
            xmcParamSpeed.dDeceleration = paramSpeed.dblDeceleration;

            if (paramSpeed.enSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
            {
                xmcParamSpeed.dJerkAcceleration = 0;
                xmcParamSpeed.dJerkAcceleration = 0;
            }
            else
            {
                xmcParamSpeed.dJerkAcceleration = paramSpeed.dblAccelJerk;
                xmcParamSpeed.dJerkDeceleration = paramSpeed.dblDecelJerk;
            }

            XMC_SDK.XMCMotion.GetInstance().MoveSyncVectorMotionCompare(nAxisCount, arAxes, arDestination, xmcParamSpeed, bAbsolute, nCompareAxis, dComparePosition, bLOGIC, bACTUAL);
            return;
        }

        // 2D 또는 3D 직교좌표 공간에서 여러 직선이동을 순차적으로 실행하기 위한 명령 (모든 축의 스케일 동일)    (by jhchoo)
        public override void MoveSyncVectorMotionList(ref int nAxisCount, ref int[] arAxes, ref double[,] arDestination, ref PARAM_SPEED[] arSpeedParam, ref bool bAbsolute)
        {
            XMC_SDK.PARAM_SPEED[] arXmcParamSpeed = new XMC_SDK.PARAM_SPEED[arSpeedParam.Length];
            for (int index = 0; index < arSpeedParam.Length; index++ )
            {
                arXmcParamSpeed[index] = new XMC_SDK.PARAM_SPEED();
                arXmcParamSpeed[index].dVelocity = arSpeedParam[index].dblVelocity;
                arXmcParamSpeed[index].dAcceleration = arSpeedParam[index].dblAcceleration;
                arXmcParamSpeed[index].dDeceleration = arSpeedParam[index].dblDeceleration;

                if (arSpeedParam[index].enSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
                {
                    arXmcParamSpeed[index].dJerkAcceleration = 0;
                    arXmcParamSpeed[index].dJerkAcceleration = 0;
                }
                else
                {
                    arXmcParamSpeed[index].dJerkAcceleration = arSpeedParam[index].dblAccelJerk;
                    arXmcParamSpeed[index].dJerkDeceleration = arSpeedParam[index].dblDecelJerk;
                }
            }
            XMC_SDK.XMCMotion.GetInstance().MoveSyncVectorMotionList(nAxisCount, arAxes, arDestination, arXmcParamSpeed, bAbsolute);
            return;
        }

        /// <summary>
        /// Compare 축의 Compare Position이 위치(Command or Actual)가 지정한 로직의 조건에 충족되었을 때 구동을 시작한다.
        /// </summary>
        /// <param name="bLogic">true : Greater than to, false : Less than to</param>
        public override void MoveCompare(ref int nAxis, ref double dblPosition, ref PARAM_SPEED paramSpeed, ref int nCompareAxis, ref double dblComparePosition, ref bool bLogic, ref bool bActual)
        {
            XMC_SDK.PARAM_SPEED xmcParamSpeed = new XMC_SDK.PARAM_SPEED();

            xmcParamSpeed = new XMC_SDK.PARAM_SPEED();
            xmcParamSpeed.dVelocity = paramSpeed.dblVelocity;
            xmcParamSpeed.dAcceleration = paramSpeed.dblAcceleration;
            xmcParamSpeed.dDeceleration = paramSpeed.dblDeceleration;

            if (paramSpeed.enSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
            {
                xmcParamSpeed.dJerkAcceleration = 0;
                xmcParamSpeed.dJerkAcceleration = 0;
            }
            else
            {
                xmcParamSpeed.dJerkAcceleration = paramSpeed.dblAccelJerk;
                xmcParamSpeed.dJerkDeceleration = paramSpeed.dblDecelJerk;
            }

            XMC_SDK.XMCMotion.GetInstance().MoveCompare(nAxis, dblPosition, xmcParamSpeed, nCompareAxis, dblComparePosition, bLogic, bActual);
            return;
        }

        // 2023.11.07. by shkim. 확인완료
        public override void StopMotion(ref int nAxis, ref bool bEmergency)
        {
#if _XMC_INIT_ERROR
            return;
#endif
            XMC_SDK.XMCMotion.GetInstance().StopMotion(nAxis, bEmergency);
            return;
        }

        // MPI 확인했을 때, 동일하게 StopMotion으로 적용되어있는데, HomeMotion을 나눠둔 이유 확인필요
        public override void StopHomeMotion(ref int nAxis, ref bool bEmergency)
        {
#if _XMC_INIT_ERROR
            return;
#endif

            XMC_SDK.XMCMotion.GetInstance().StopMotion(nAxis, bEmergency);
            return;
        }

        /// <summary>
        /// 2024.03.08. 확인완료
        /// </summary>
        public override bool MoveUntilTouch(ref int nAxis, ref int nSeqNum, ref double dblDestination, ref int nEncoderAxis, ref double dblEncoderThreshold, ref bool bTouched, ref double dblTouchedPosition, ref double dblTochedEncoderPosition)
        {
            return XMC_SDK.XMCMotion.GetInstance().MoveUntilTouch(nAxis, ref  nSeqNum, dblDestination, nEncoderAxis, dblEncoderThreshold, ref  bTouched, ref  dblTouchedPosition, ref  dblTochedEncoderPosition);
        }

        /// <summary>
        /// 2024.03.08. 확인완료
        /// </summary>
        public override bool MoveUntilTouch_Analog(ref int nAxis, ref int nSeqNum, ref double dblDestination, ref int[] arAnalog, ref double dblAnalogThreshold, ref bool bTouched, ref double dblTouchedPosition, ref double dblTochedAnalogValue)
        {
            return XMC_SDK.XMCMotion.GetInstance().MoveUntilTouch_Analog(nAxis, ref nSeqNum, dblDestination, arAnalog, dblAnalogThreshold, ref bTouched, ref  dblTouchedPosition, ref  dblTochedAnalogValue);
        }

        /// <summary>
        /// 2024.03.10. 확인완료
        /// </summary>
        public override bool MoveByListUntilTouch(ref int nAxis, ref int nSeqNum, ref int nCount, ref double[] arDestination, ref PARAM_SPEED[] arSpeedParam, ref int nEncoderAxis, ref double dblEncoderThreshold, ref bool bTouched, ref double dblTouchedPosition, ref double dblTochedEncoderPosition)
        {
            XMC_SDK.PARAM_SPEED[] arrSpeed = new XMC_SDK.PARAM_SPEED[nCount];
            for (int i = 0; i < nCount; ++i)
            {
                arrSpeed[i] = new XMC_SDK.PARAM_SPEED();
                arrSpeed[i].dVelocity = arSpeedParam[i].dblVelocity;
                arrSpeed[i].dAcceleration = arSpeedParam[i].dblAcceleration;
                arrSpeed[i].dDeceleration = arSpeedParam[i].dblDeceleration;

                if (arSpeedParam[i].enSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
                {
                    arrSpeed[i].dJerkAcceleration = 0;
                    arrSpeed[i].dJerkAcceleration = 0;
                }
                else
                {
                    arrSpeed[i].dJerkAcceleration = arSpeedParam[i].dblAccelJerk;
                    arrSpeed[i].dJerkDeceleration = arSpeedParam[i].dblDecelJerk;
                }
            }

            return XMC_SDK.XMCMotion.GetInstance().MoveByListUntilTouch(nAxis
                , ref nSeqNum
                , nCount, arDestination, arrSpeed, nEncoderAxis, dblEncoderThreshold
                , ref bTouched, ref dblTouchedPosition, ref dblTochedEncoderPosition);
        }

        /// <summary>
        /// 2024.03.10. 확인완료
        /// </summary>
        public override bool MoveByListUntilTouch_Analog(ref int nAxis, ref int nSeqNum, ref int nCount, ref double[] arDestination, ref PARAM_SPEED[] arSpeedParam, ref int[] arAnalog, ref double dblAnalogThreshold, ref bool bTouched, ref double dblTouchedPosition, ref double dblTochedAnalogValue)
        {
            XMC_SDK.PARAM_SPEED[] arrSpeed = new XMC_SDK.PARAM_SPEED[nCount];
            for (int i = 0; i < nCount; ++i)
            {
                arrSpeed[i] = new XMC_SDK.PARAM_SPEED();
                arrSpeed[i].dVelocity = arSpeedParam[i].dblVelocity;
                arrSpeed[i].dAcceleration = arSpeedParam[i].dblAcceleration;
                arrSpeed[i].dDeceleration = arSpeedParam[i].dblDeceleration;

                if (arSpeedParam[i].enSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
                {
                    arrSpeed[i].dJerkAcceleration = 0;
                    arrSpeed[i].dJerkAcceleration = 0;
                }
                else
                {
                    arrSpeed[i].dJerkAcceleration = arSpeedParam[i].dblAccelJerk;
                    arrSpeed[i].dJerkDeceleration = arSpeedParam[i].dblDecelJerk;
                }
            }

            return XMC_SDK.XMCMotion.GetInstance().MoveByListUntilTouch_Analog(nAxis
                , ref nSeqNum
                , nCount, arDestination, arrSpeed,  arAnalog,  dblAnalogThreshold, ref bTouched, ref dblTouchedPosition, ref dblTochedAnalogValue);
        }

        
        
#endregion </MoveInterface>

        // [CHECKS]구현 필요
        public override bool SetChangeTorqueLimitPositionEvent(int nAxis, double dEventPosition, ushort nDefaultTorque, ushort nLimitTorque, double dEventPositionWidth)
        {
            return false;
        }

        // [CHECKS]구현 필요
        public override bool GetCurrentMotorTorqueValue(int nAxis, ref short dCurrentTorque)
        {
            return false;
        }

        public override bool MakeDumpFile(ref int nAxis, string sFileName)
        {
            string parsedFilename = System.IO.Path.GetFileName(sFileName);
            string currentDirectory = System.Environment.CurrentDirectory;
            string xmcDumpPath = currentDirectory + "\\..\\Log\\XMCDump";

            if(false == System.IO.Directory.Exists(xmcDumpPath))
            {
                System.IO.Directory.CreateDirectory(xmcDumpPath);
            }

            string fullFilename = string.Format("{0}\\{1}", xmcDumpPath, parsedFilename);

            // XMC_SDK.XMCMotion.GetInstance().MakeDumpFile(nAxis, sFileName);
            System.Console.WriteLine(fullFilename);
            XMC_SDK.XMCMotion.GetInstance().MakeDumpFile(nAxis, fullFilename);
            return false;
        }

        // [필요없음]
        public override bool ReleaseMotionGroup(ref int[] arAxisIndexes)
        {
            return false;
        }

        // [CHECSK] 필요여부 확인 필요 (GMS, XMC)
        public override void SetCallbackChangeMotorBrakeState(int nAxis, DelegateChangeMotorBrakeState pFuncSetMotorBrakeState)
        {
            return;
        }

        #region <Compensator>
        public override void Apply2DCompensationInformation(int nAxisIndexToCompensation, CompensationDirection enDirectionType,
            int nAxisIndexForX, double dStartingPositionsX, double dEndingPositionX, double dDistanceX,
            int nAxisIndexForY, double dStartingPositionsY, double dEndingPositionY, double dDistanceY,
            ref double[,] arTableByRowCol)
        {
            // TODO : 기어비, Target 번호는 Motion 객체를 통해야만 받아올 수 있기 때문에, 일단 Config에 접근하여 아이템 정보를 읽어오도록 한다.
            
            #region <Output Axis>
            //int targetOutputAxis = 0;
            //Config.ConfigMotion.GetInstance().GetMotionParameter(nAxisIndexToCompensation, Config.ConfigMotion.EN_PARAM_MOTION.TARGET, ref targetOutputAxis);
            #endregion </Output Axis>

            #region <X>
            //int targetAxisX = 0;
            //Config.ConfigMotion.GetInstance().GetMotionParameter(nAxisIndexForX, Config.ConfigMotion.EN_PARAM_MOTION.TARGET, ref targetAxisX);
            //double inputScaleCprX = 0, inputScaleMprX = 0;
            //Config.ConfigMotion.GetInstance().GetConfigParameter(nAxisIndexForX, Config.ConfigMotion.EN_PARAM_CONFIG.INPUT_SCALE_CPR, ref inputScaleCprX);
            //Config.ConfigMotion.GetInstance().GetConfigParameter(nAxisIndexForX, Config.ConfigMotion.EN_PARAM_CONFIG.INPUT_SCALE_MPR, ref inputScaleMprX);
            //double ratioX = inputScaleCprX / inputScaleMprX;

            //int startingPosX = (int)(dStartingPositionsX * ratioX);
            //int endingPosX = (int)(dEndingPositionX * ratioX);
            //int distanceX = (int)(dDistanceX * ratioX);
            #endregion <X>

            #region <Y>
            //int targetAxisY = 0;
            //Config.ConfigMotion.GetInstance().GetMotionParameter(nAxisIndexForY, Config.ConfigMotion.EN_PARAM_MOTION.TARGET, ref targetAxisY);
            //double inputScaleCprY = 0, inputScaleMprY = 0;
            //Config.ConfigMotion.GetInstance().GetConfigParameter(nAxisIndexForY, Config.ConfigMotion.EN_PARAM_CONFIG.INPUT_SCALE_CPR, ref inputScaleCprY);
            //Config.ConfigMotion.GetInstance().GetConfigParameter(nAxisIndexForY, Config.ConfigMotion.EN_PARAM_CONFIG.INPUT_SCALE_MPR, ref inputScaleMprY);
            //double ratioY = inputScaleCprY / inputScaleMprY;

            //int startingPosY = (int)(dStartingPositionsY * ratioY);
            //int endingPosY = (int)(dEndingPositionY * ratioY);
            //int distanceY = (int)(dDistanceX * ratioX);
            #endregion <Y>

            XMC_SDK_.Compensator.COMPENSATION_2D_ITEM_INFO itemInfo = new XMC_SDK_.Compensator.COMPENSATION_2D_ITEM_INFO();
            itemInfo.nOutputAxis = nAxisIndexToCompensation;
            itemInfo.nInputAxisX = nAxisIndexForX;
            itemInfo.nDistanceX = (int)dDistanceX;
            itemInfo.nStartingPositionX = (int)Math.Round(dStartingPositionsX - dDistanceX, 0);
            itemInfo.nEndingPositionX = (int)Math.Round(dEndingPositionX + dDistanceX, 0);

            itemInfo.nInputAxisY = nAxisIndexForY;
            itemInfo.nDistanceY = (int)dDistanceY;
            itemInfo.nStartingPositionY = (int)Math.Round(dStartingPositionsY - dDistanceY, 0);
            itemInfo.nEndingPositionY   = (int)Math.Round(dEndingPositionY + dDistanceY, 0);

            #region <Direction Type>
            //double ratioForTable;
            //switch (enDirectionType)
            //{
            //    case CompensationDirection.X:
            //        ratioForTable = ratioX;
            //        itemInfo.enDirectionType = XMC_SDK_.Compensator.CompensationDirection.X;
            //        break;
            //    case CompensationDirection.Y:
            //        ratioForTable = ratioY;
            //        itemInfo.enDirectionType = XMC_SDK_.Compensator.CompensationDirection.Y;
            //        break;
            //    default:
            //        return;
            //}
            #endregion </Direction Type>

            #region <Table>
            int rowMax = arTableByRowCol.GetLength(0) + 2;
            int colMax = arTableByRowCol.GetLength(1) + 2;
            
            // TODO : 값이 어떻게 다른지 비교 필요
            int tempRow = (int)((itemInfo.nEndingPositionY - itemInfo.nStartingPositionY) / itemInfo.nDistanceY + 1);
            int tempCol = (int)((itemInfo.nEndingPositionX - itemInfo.nStartingPositionX) / itemInfo.nDistanceX + 1);
            itemInfo.arTable = new int[rowMax * colMax];

            int index = 0;
            for (int row = 0; row < rowMax; ++row)
            {
                for (int col = 0; col < colMax; ++col)
                {
                    if (row == 0 || row == rowMax - 1 || col == 0 || col == colMax - 1)
                    {
                        itemInfo.arTable[index++] = 0;
                    }
                    else
                    {
                        itemInfo.arTable[index++] = (int)(arTableByRowCol[row - 1, col - 1]);
                    }
                }
            }
            #endregion </Table>

            XMC_SDK.XMCMotion.GetInstance().SaveCompensationInformation(nAxisIndexToCompensation, itemInfo);
        }

        public override void Enable2DCompensation(int nAxisIndexToCompensation, bool bEnabled)
        {
            // TODO : 기어비, Target 번호는 Moton 객체를 통해야만 받아올 수 있기 때문에, 일단 Config에 접근하여 아이템 정보를 읽어오도록 한다.
            //int targetOutputAxis = 0;
            //Config.ConfigMotion.GetInstance().GetMotionParameter(nAxisIndexToCompensation, Config.ConfigMotion.EN_PARAM_MOTION.TARGET, ref targetOutputAxis);
            XMC_SDK.XMCMotion.GetInstance().EnableCompensation(nAxisIndexToCompensation, bEnabled);
        }
        #endregion </Compensator>

        // MEI, RSA 미확인(미사용) Interface (2023/10/08)	=> BP5000WIR 소스 기준
        #region MEI, RSA 미확인(미사용) Interface (2023/10/08)	=> BP5000WIR 소스 기준
        public override bool GetGantryState(ref int nAxis, ref PARAM_GANTRY paramGantry)
        {
            return true;
        }
        public override bool IsMultiHomeDone(ref int nCountOfAxis, ref int[] arAxes)
        {
            return false;
        }
        public override bool IsMultiMotionDone(ref int nCountOfAxis, ref int[] arAxes)
        {
            return false;
        }

        public override void MoveMultiAbsolutely(ref int nCountOfAxis, ref int[] arAxes, ref double[] arDestination)
        {
            return;
        }
        public override void MoveMultiAtSpeed(ref int nCountOfAxis, ref int[] arAxes, ref bool[] arDirection)
        {
            return;
        }
        public override void MoveMultiReleatively(ref int nCountOfAxis, ref int[] arAxes, ref double[] arDestination)
        {
            return;
        }
        public override bool MoveMultiToHome(ref int nCountOfAxis, ref int[] arAxes, ref int nSeqNum, ref int nDelay)
        {
            return false;
        }
        public override void StopMultiHomeMotion(ref int nCountOfAxis, ref int[] arAxes, ref bool bEmergency)
        {
            return;
        }
        public override void StopMultiMotion(ref int nCountOfAxis, ref int[] arAxes, ref bool bEmergency)
        {
            return;
        }

        //public virtual void MovePTV(ref int nAxis, ref int nCountOfStep, ref double[] arPosition, ref double[] arVelocity, ref double[] arTime)
        //{
        //    return;
        //}

        #endregion
    }
}
