using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

using Motion_;

namespace FrameOfSystem3.Controller.Motion
{
    class MeiMotion : Motion_.MotionController
    {
        public override bool ConnectLink()
        {
            return true;
        }

        public override void DoMotorOn(ref int nAxis, ref bool nON)
        {
            MPISDK_.Motion_.MPIMotion.GetInstance().DoMotorOn(ref nAxis, ref nON);
        }

        public override void DoReset(ref int nAxis)
        {
            MPISDK_.Motion_.MPIMotion.GetInstance().DoReset(ref nAxis);
        }

        public override void ExitController()
        {
			try
			{
				MPISDK_.Motion_.MPIMotion.GetInstance().ExitController();
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
			}
        }
        //public override void GetCommandPositionAll(ref int nCountOfAxis, ref double[] arPosition)
        //{
        //    MPISDK_.Motion_.MPIMotion.GetInstance().GetCommandPositionAll(ref nCountOfAxis, ref arPosition);
        //}
        //public override void GetActualPositionAll(ref int nCountOfAxis, ref double[] arPosition)
        //{
        //    MPISDK_.Motion_.MPIMotion.GetInstance().GetActualPositionAll(ref nCountOfAxis, ref arPosition);
        //}

        public override int GetCountOfAxis()
        {
            return MPISDK_.Motion_.MPIMotion.GetInstance().GetCountOfAxis();
        }

        public override bool GetGantryState(ref int nAxis, ref PARAM_GANTRY paramGantry)
        {
            return true;
        }

        private bool UpdateCompensationData()
        {
            return true;
//             bool bUsingCompensation = false;
//             int nCountOfItem = Work.StageMapping.GetInstance().GetMappingItemCount();         // 열거형 수량 읽어오는거
//             for (int i = 0; i < nCountOfItem; ++i)
//             {
//                 if (Work.StageMapping.GetInstance().IsCompensated(i) == false) continue;
// 
//                 bUsingCompensation = true;
// 
//                 Work.CompensationItem citem = new Work.CompensationItem(0, 0);
//                 Work.StageMapping.GetInstance().GetResult(i, ref citem);
// 
//                 int[] arrAxes = { citem.TargetAxis.x, citem.TargetAxis.y };
//                 int[] arrStart = { (int)citem.StarPosition.x, (int)citem.StarPosition.y };
//                 int[] arrDistance = { (int)citem.Distance.x, (int)citem.Distance.y };
//                 
//                 int nRowMax = citem.MaxCount.y;
//                 int nColMax = citem.MaxCount.x;
// 
//                 int[] arrEnd = 
//                 {
//                     arrStart[0] + arrDistance[0] * (nColMax - 1),
//                     arrStart[1] + arrDistance[1] * (nRowMax - 1)
//                 };
// 
//                 double[,] arrResultX = new double[nRowMax, nColMax];
//                 double[,] arrResultY = new double[nRowMax, nColMax];
//                 for (int nRow = 0; nRow < nRowMax; ++nRow)
//                 {
//                     for (int nCol = 0; nCol < nColMax; ++nCol)
//                     {
//                         DPointXY stResult = new DPointXY();
//                         Work.StageMapping.GetInstance().GetPositionByCount(i, nCol, nRow, ref stResult);
//                         arrResultX[nRow, nCol] = -stResult.x;
//                         arrResultY[nRow, nCol] = -stResult.y;
//                     }
//                 }
// 
//                 MPISDK_.Motion_.MPIMotion.GetInstance().SetCompensationInfo(ref arrAxes, ref arrStart, ref arrDistance, ref arrEnd, ref arrResultX, ref arrResultY);
//             }
// 
//             return bUsingCompensation;
        }

        public override bool InitController()
        {
			// 데이터 읽어오고
			// SDK에 전달
            bool bUsingCompensation = UpdateCompensationData();
            //bUsingCompensation = false;
            return MPISDK_.Motion_.MPIMotion.GetInstance().InitController(bUsingCompensation);
        }

        public override bool IsHomeDone(ref int nAxis)
        {
            return MPISDK_.Motion_.MPIMotion.GetInstance().isHomeDone(ref nAxis);
        }

        public override bool IsLinkConnected()
        {
            return MPISDK_.Motion_.MPIMotion.GetInstance().IsLinkConnected();
        }

        public override bool IsMotionDone(ref int nAxis)
        {
            return MPISDK_.Motion_.MPIMotion.GetInstance().isMotionDone(ref nAxis);
        }

        public override bool IsMotorOn(ref int nAxis)
        {
            return MPISDK_.Motion_.MPIMotion.GetInstance().isMotionDone(ref nAxis);
        }

        public override void MoveAbsolutely(ref int nAxis, ref double dblDestination)
        {
            #region 테스트
            //int nCount = 2;
            ////double[] arDest = { 6213783, 9320675 };     // +방향 테스트
            //double[] arDest = { 3106892, 0 };             // -방향 테스트
            //MPISDK_.SPEED_PARAM[] arrSpeed = new MPISDK_.SPEED_PARAM[nCount];

            //double dAccel = 3044754014;
            //double dJerkAcc = 101284674370;

            //for (int i = 0; i < nCount; ++i)
            //{
            //    arrSpeed[i] = new MPISDK_.SPEED_PARAM();

            //    arrSpeed[i].dAcceleration = dAccel;
            //    arrSpeed[i].dDeceleration = dAccel;
            //    arrSpeed[i].dJerkAcceleration = dJerkAcc;
            //    arrSpeed[i].dJerkDeceleration = dJerkAcc;
            //    if (i == 0)
            //    {
            //        arrSpeed[i].dVelocity = 3106891;
            //    }
            //    else
            //    {
            //        arrSpeed[i].dVelocity = 3106891 / 3;
            //    }
            //}
            //MPISDK_.Motion_.MPIMotion.GetInstance().MoveByList(ref nAxis, ref nCount, ref arDest, ref arrSpeed);
            #endregion

            //double[] temp = { 3106891.8518518517 };
            //double[] vel = { 3106891.8518518517 };
            //double[] time = { 1000 };
            //MPISDK_.Motion_.MPIMotion.GetInstance().MovePVT(ref nAxis, ref temp, ref vel, ref time);
            MPISDK_.Motion_.MPIMotion.GetInstance().MoveAbsolutely(ref nAxis, ref dblDestination);
        }

        public override void MoveAtSpeed(ref int nAxis, ref bool bDirection)
        {           
            MPISDK_.Motion_.MPIMotion.GetInstance().MoveAtSpeed(ref nAxis, ref bDirection);
        }

        // nCountOfAxis: 2개
        // arIndexes : 축 번호 배열
        // nCountOfStep : 1
        // arDestination[0, n] : 각 축의 목표 위치 배열
        // arSpeedParam : 속도 1개
        public override void MoveByLinearCoordination(ref int nCountOfAxis, ref int[] arIndexes, ref int nCountOfStep, ref double[,] arDestination, ref PARAM_SPEED[,] arSpeedParam)
        {
            int nCount = nCountOfAxis;
            MPISDK_.SPEED_PARAM[] arrSpeed = new MPISDK_.SPEED_PARAM[nCount];
            for (int i = 0; i < nCount; ++i)
            {
                arrSpeed[i] = new MPISDK_.SPEED_PARAM();
                arrSpeed[i].dVelocity = arSpeedParam[0, i].dblVelocity;
                arrSpeed[i].dAcceleration = arSpeedParam[0, i].dblAcceleration;
                arrSpeed[i].dDeceleration = arSpeedParam[0, i].dblDeceleration;

                if (arSpeedParam[0, i].enSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
                {
                    arrSpeed[i].dJerkAcceleration = 0;
                    arrSpeed[i].dJerkAcceleration = 0;
                }
                else
                {
                    arrSpeed[i].dJerkAcceleration = arSpeedParam[0, i].dblAccelJerk;
                    arrSpeed[i].dJerkDeceleration = arSpeedParam[0, i].dblDecelJerk;
                }
            }

            arIndexes[0] = 10;
            arIndexes[1] = 11;

            //MPISDK_.Motion_.MPIMotion.GetInstance().MoveByLinearCoordination(ref nCountOfAxis, arIndexes, ref nCountOfStep, arDestination, arrSpeed);
        }
        public override void MoveByList(ref int nAxis, ref int nCount, ref double[] arDestination, ref PARAM_SPEED[] arSpeedParam)
        {
            MPISDK_.SPEED_PARAM[] arrSpeed = new MPISDK_.SPEED_PARAM[nCount];
            for (int i = 0; i < nCount; ++i )
            {
                arrSpeed[i] = new MPISDK_.SPEED_PARAM();
                arrSpeed[i].dVelocity     = arSpeedParam[i].dblVelocity;
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

            MPISDK_.Motion_.MPIMotion.GetInstance().MoveByList(ref nAxis, ref nCount, ref arDestination, ref arrSpeed);
        }
        public override void MoveReleatively(ref int nAxis, ref double dblDestination)
        {
            MPISDK_.Motion_.MPIMotion.GetInstance().MoveRelatively(ref nAxis, ref dblDestination);
        }

        public override bool MoveToHome(ref int nAxis, ref int nSeqNum, ref int nDelay)
        {
            return MPISDK_.Motion_.MPIMotion.GetInstance().MoveToHome(ref nAxis, ref nSeqNum, ref nDelay);
        }

		public override void OverrideMotion(ref int nAxis, ref EN_OVERRIDE_TYPE enTypeOfOverride, ref double dblDestination, ref PARAM_SPEED paramSpeed)
		{
			MPISDK_.SPEED_PARAM stParam = new MPISDK_.SPEED_PARAM();
			stParam.dVelocity			= paramSpeed.dblVelocity;
			stParam.dAcceleration		= paramSpeed.dblAcceleration;
			stParam.dDeceleration		= paramSpeed.dblDeceleration;

			if (paramSpeed.enSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
			{
				stParam.dJerkAcceleration = 0;
				stParam.dJerkAcceleration = 0;
			}
			else
			{
				stParam.dJerkAcceleration = paramSpeed.dblAccelJerk;
				stParam.dJerkDeceleration = paramSpeed.dblDecelJerk;
			}

			MPISDK_.Motion_.MPIMotion.GetInstance().OverrideSpeed(ref nAxis, ref stParam);
		}

		//public override void OverrideSpeed(ref int nAxis, ref PARAM_SPEED paramSpeed)
		//{
		//	MPISDK_.SPEED_PARAM stParam = new MPISDK_.SPEED_PARAM();
		//	stParam.dVelocity = paramSpeed.dblVelocity;
		//	stParam.dAcceleration = paramSpeed.dblAcceleration;
		//	stParam.dDeceleration = paramSpeed.dblDeceleration;

		//	if (paramSpeed.enSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
		//	{
		//		stParam.dJerkAcceleration = 0;
		//		stParam.dJerkAcceleration = 0;
		//	}
		//	else
		//	{
		//		stParam.dJerkAcceleration = paramSpeed.dblAccelJerk;
		//		stParam.dJerkDeceleration = paramSpeed.dblDecelJerk;
		//	}

		//	MPISDK_.Motion_.MPIMotion.GetInstance().OverrideSpeed(ref nAxis, ref stParam);
		//}

        public override void SetActualPosition(ref int nAxis, ref double dblPosition)
        {
            MPISDK_.Motion_.MPIMotion.GetInstance().SetActualPosition(ref nAxis, ref dblPosition);
        }

        public override void GetMotorInformationAll(ref int nCountOfAxis, ref int[] arState, ref double[] arActualPosition, ref double[] arCommandPosition)
        {
            MPISDK_.Motion_.MPIMotion.GetInstance().GetMotorStateAll(ref nCountOfAxis, ref arState, ref arCommandPosition, ref arActualPosition);
        }

        public override void SetCommandPosition(ref int nAxis, ref double dblPosition)
        {
            MPISDK_.Motion_.MPIMotion.GetInstance().SetCommandPosition(ref nAxis, ref dblPosition);
        }

        public override void GetAbsolutePosition(ref int nAxis, ref double dblPosition)
        {
            dblPosition = MPISDK_.Motion_.MPIMotion.GetInstance().GetPrimaryFeedback(ref nAxis);
        }

        public override void SetGantry(ref int nAxis, ref PARAM_GANTRY paramGantry, ref PARAM_SPEED paramSpeed)
        {
            //MPISDK_.Motion_.MPIMotion.GetInstance().SetGantry(ref paramGantry.nIndexOfMaster, ref paramGantry.nIndexOfSlave, ref paramGantry.bEnableGantry, ref paramGantry.bReverseSlaveDirection);
            //SetMotorSpeedConfiguration(ref paramGantry.nIndexOfMaster, ref paramSpeed);
        }

        public override void SetMotorConfiguration(ref int nAxis, ref PARAM_CONFIG paramConfig)
        {
            MPISDK_.PARAM_CONFIG stParam = new MPISDK_.PARAM_CONFIG();
            if (paramConfig.enMotorType == MOTOR_TYPE.SERVO) 
                stParam.bUseServoOn = true;
            else
                stParam.bUseServoOn = false;
            stParam.bUseSecondaryIndex = false;

            stParam.bHWLimitLogicNegative = paramConfig.bHWLimitLogicNegative;
            stParam.bHWLimitLogicPositive = paramConfig.bHWLimitLogicPositive;
            stParam.bHWLimitStopModePositive = paramConfig.bHWLimitStopModePositive;
            stParam.bHWLimitStopModeNegative = paramConfig.bHWLimitStopModeNegative;
            stParam.bSWLimitStopModeNegative = paramConfig.bSWLimitStopModeNegative;
            stParam.bSWLimitStopModeNegative = paramConfig.bSWLimitStopModeNegative;

            stParam.bUseHWLimitNegative = paramConfig.bUseHWLimitNegative;
            stParam.bUseHWLimitPositive = paramConfig.bUseHWLimitPositive;
            stParam.bUseSWLimitNegative = paramConfig.bUseSWLimitNegative;
            stParam.bUseSWLimitPositive = paramConfig.bUseSWLimitPositive;

            stParam.dSWLimitPositionNegative = paramConfig.dblSWLimitPositionNegative;
            stParam.dSWLimitPositionPositive = paramConfig.dblSWLimitPositionPositive;

            MPISDK_.Motion_.MPIMotion.GetInstance().SetMotorConfig(ref nAxis, ref stParam);
        }
        public override void SetMotorHomeConfiguration(ref int nAxis, ref PARAM_HOME paramHome)
        {
            MPISDK_.HOME_PARAM stParam = new MPISDK_.HOME_PARAM();
            stParam.dVelocity = paramHome.dblHomeVelocityStart;
            stParam.dAcceleration = paramHome.dblHomeAcceleration;
            stParam.dDeceleration = paramHome.dblHomeDeceleration;
            stParam.dHomeVelocityLast = paramHome.dblHomeVelocityLast;

            if (paramHome.enHomeSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
            {
                stParam.dJerkAcceleration = 0;
                stParam.dJerkAcceleration = 0;
            }
            else
            {
                stParam.dJerkAcceleration = paramHome.dblHomeAccelJerk;
                stParam.dJerkAcceleration = paramHome.dblHomeDecelJerk;
            }

            stParam.bHomeDirection = paramHome.bHomeDirection;
            stParam.bHomeLogic = paramHome.bHomeLogic;
            stParam.nHomeMode = (int)paramHome.enHomeMode;
            stParam.dHomeBasePosition = paramHome.dblHomeBasePosition;
            stParam.dHomeEscapeDist = paramHome.dblHomeEscapeDist;
            stParam.dHomeOffset = paramHome.dblHomeOffset;
            stParam.dAbsoluteHomePosition = paramHome.dblAbsoluteHomePosition;

            MPISDK_.Motion_.MPIMotion.GetInstance().SetMotorHomeSpeed(ref nAxis, ref stParam);
        }
        public override void SetMotorHomeSpeedConfiguration(ref int nAxis, ref PARAM_HOME paramSpeed)
        {
            MPISDK_.HOME_PARAM stParam = new MPISDK_.HOME_PARAM();
            stParam.dVelocity = paramSpeed.dblHomeVelocityStart;
            stParam.dAcceleration = paramSpeed.dblHomeAcceleration;
            stParam.dDeceleration = paramSpeed.dblHomeDeceleration;
            stParam.dHomeVelocityLast = paramSpeed.dblHomeVelocityLast;

            if (paramSpeed.enHomeSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
            {
                stParam.dJerkAcceleration = 0;
                stParam.dJerkAcceleration = 0;
            }
            else
            {
                stParam.dJerkAcceleration = paramSpeed.dblHomeAccelJerk;
                stParam.dJerkAcceleration = paramSpeed.dblHomeDecelJerk;
            }

            stParam.bHomeDirection = paramSpeed.bHomeDirection;
            stParam.bHomeLogic = paramSpeed.bHomeLogic;
            stParam.nHomeMode = (int)paramSpeed.enHomeMode;
            stParam.dHomeBasePosition = paramSpeed.dblHomeBasePosition;
            stParam.dHomeEscapeDist = paramSpeed.dblHomeEscapeDist;
            stParam.dHomeOffset = paramSpeed.dblHomeOffset;
            stParam.dAbsoluteHomePosition = paramSpeed.dblAbsoluteHomePosition;

            MPISDK_.Motion_.MPIMotion.GetInstance().SetMotorHomeSpeed(ref nAxis, ref stParam);
        }
        public override void SetMotorSpeedConfiguration(ref int nAxis, ref PARAM_SPEED paramSpeed)
        {
            MPISDK_.SPEED_PARAM stParam = new MPISDK_.SPEED_PARAM();
            stParam.dVelocity = paramSpeed.dblVelocity;
            stParam.dAcceleration = paramSpeed.dblAcceleration;
            stParam.dDeceleration = paramSpeed.dblDeceleration;

            if (paramSpeed.enSpeedPattern == MOTION_SPEED_PATTERN.TRAPEZODIAL)
            {
                stParam.dJerkAcceleration = 0;
                stParam.dJerkAcceleration = 0;
            }
            else
            {
                stParam.dJerkAcceleration = paramSpeed.dblAccelJerk;
                stParam.dJerkDeceleration = paramSpeed.dblDecelJerk;
            }
            
            MPISDK_.Motion_.MPIMotion.GetInstance().SetMotorSpeed(ref nAxis, ref stParam);
        }
        public override bool GetGainTable(ref int nAxis, ref int nIndexOfTable)
        {
            //nIndexOfTable = MPISDK_.Motion_.MPIMotion.GetInstance().GetGainTable(ref nAxis);

            return nIndexOfTable != -1;
        }
        public override void SetGainTable(ref int nAxis, ref int nIndexOfTable)
        {
            MPISDK_.Motion_.MPIMotion.GetInstance().SetGainTable(ref nAxis, ref nIndexOfTable);
        }
        public override void StopHomeMotion(ref int nAxis, ref bool bEmergency)
        {
            MPISDK_.Motion_.MPIMotion.GetInstance().StopMotion(ref nAxis, ref bEmergency);
        }
        public override void StopMotion(ref int nAxis, ref bool bEmergency)
        {
            MPISDK_.Motion_.MPIMotion.GetInstance().StopMotion(ref nAxis, ref bEmergency);
        }

        public override bool MoveUntilTouch(ref int nAxis, ref int nSeqNum, ref double dblDestination, ref int nEncoderAxis, ref double dblEncoderThreshold, ref bool bTouched, ref double dblTouchedPosition, ref double dblTouchedEncoderPosition)
        {
            bool bResult = MPISDK_.Motion_.MPIMotion.GetInstance().MoveUntilTouch(ref nAxis, ref nSeqNum, ref dblDestination, ref nEncoderAxis, ref dblEncoderThreshold, ref bTouched, ref dblTouchedPosition, ref dblTouchedEncoderPosition);

            return bResult;
        }

		public override bool IsMultiHomeDone(ref int nCountOfAxis, ref int[] arAxes)
		{
			return	false;
		}

		public override bool IsMultiMotionDone(ref int nCountOfAxis, ref int[] arAxes)
		{
			return false;
		}

		public override void MoveMultiAbsolutely(ref int nCountOfAxis, ref int[] arAxes, ref double[] arDestination)
		{
			
		}

		public override void MoveMultiAtSpeed(ref int nCountOfAxis, ref int[] arAxes, ref bool[] arDirection)
		{
			
		}

		public override void MoveMultiReleatively(ref int nCountOfAxis, ref int[] arAxes, ref double[] arDestination)
		{
			
		}

		public override bool MoveMultiToHome(ref int nCountOfAxis, ref int[] arAxes, ref int nSeqNum, ref int nDelay)
		{
			return false;
		}

		public override void StopMultiHomeMotion(ref int nCountOfAxis, ref int[] arAxes, ref bool bEmergency)
		{
			
		}

		public override void StopMultiMotion(ref int nCountOfAxis, ref int[] arAxes, ref bool bEmergency)
		{
			
		}
        //public override bool MoveUntilTouch(ref int nAxis, ref int nSeqNum, ref double dblDestination, ref int nEncoderAxis, ref double dblEncoderThreshold, ref bool bTouched, ref double dblTouchedPosition)
        //{
        //    dblEncoderThreshold = 250;
        //    bool bResult = MPISDK_.Motion_.MPIMotion.GetInstance().MoveUntilTouch(ref nAxis, ref nSeqNum, ref dblDestination, ref nEncoderAxis, ref dblEncoderThreshold, ref bTouched, ref dblTouchedPosition);

        //    //Console.WriteLine("Touch Result : " + bResult);

        //    return bResult;
        //}

        public override void MoveCompare(ref int nAxis, ref double dblPosition, ref PARAM_SPEED paramSpeed, ref int nCompareAxis, ref double dblComparePosition, ref bool bLogic, ref bool bActual)
        {
            SetMotorSpeedConfiguration(ref nAxis, ref paramSpeed);

            MPISDK_.Motion_.MPIMotion.GetInstance().MoveCompare(ref nAxis, ref dblPosition, ref nCompareAxis, ref dblComparePosition, ref bLogic, ref bActual);
        }

        public override void MovePTV(ref int nAxis, ref int nCountOfStep, ref double[] arPosition, ref double[] arVelocity, ref double[] arTime)
        {
            MPISDK_.Motion_.MPIMotion.GetInstance().MovePVT(ref nAxis, ref arPosition, ref arVelocity, ref arTime);
        }

		public override bool ReleaseMotionGroup(ref int[] arAxisIndexes)
		{
			// 필요하면 만들것
			return true;
		}

#if MOTION_DUMP

		public override bool MakeDumpFile(ref int nAxis, string sFileName)
		{
			return MPISDK_.Motion_.MPIMotion.GetInstance().MakeDumpFile(ref nAxis, sFileName);
		}
#endif
    }
}
