using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RunningTask_;

using FrameOfSystem3.Recipe;

using Define.DefineEnumProject.Task;
using Define.DefineEnumProject.SubSequence;

using FrameOfSystem3.Work;
using FrameOfSystem3.SubSequence;
using Motion_;
using Cylinder_;

namespace FrameOfSystem3.Task
{
    extern alias MotionInstance;

    /// <summary>
    /// 2021.05.17 by ssh [ADD] Class for items that need to be added to RunningTask
    /// </summary>
    public abstract class RunningTaskEx : RunningTask
    {
        #region <STRUCTURE>
        private struct MoveMotionData
        {
            #region Variables
            private int m_nIndex;
            private EN_MOTION_CONTROL_TYPE m_enControlType;
            private int m_nTargetIndex;
            private double m_dblPosition;
            private double m_dCustomSpeed; // 2022.04.27. by shkim. [ADD] 커스텀 스피드 추가
            private MOTION_SPEED_CONTENT m_enContent;
            private int m_nRatio;
            private int m_nDelay;
            private int m_nPreDelay;
            private bool m_bCheckMotion;
            private string m_sCaption;      // 2024.09.27 by junho [ADD] log caption 추가
            private int m_nCaptionRetried;
            #endregion

            #region Constructor
            public MoveMotionData(int nIndex)
            {
                m_nIndex = nIndex;

                m_enControlType = EN_MOTION_CONTROL_TYPE.ABSOLUTELY;
                m_nTargetIndex = 0;
                m_dblPosition = 0;
                m_dCustomSpeed = 0.0;
                m_enContent = MOTION_SPEED_CONTENT.RUN;
                m_nRatio = 100;
                m_nDelay = 100;
                m_nPreDelay = 0;
                m_bCheckMotion = true;
                m_sCaption = "";
                m_nCaptionRetried = 0;
            }
            #endregion

            #region Property
            public EN_MOTION_CONTROL_TYPE ControlType { set { m_enControlType = value; } get { return m_enControlType; } }
            public int TargetIndex { set { m_nTargetIndex = value; } get { return m_nTargetIndex; } }
            public double Position { set { m_dblPosition = value; } get { return m_dblPosition; } }
            public double CustomSpeed {set {m_dCustomSpeed = value;} get {return m_dCustomSpeed;}}
            public MOTION_SPEED_CONTENT Content { set { m_enContent = value; } get { return m_enContent; } }
            public int Ratio { set { m_nRatio = value; } get { return m_nRatio; } }
            public int Delay { set { m_nDelay = value; } get { return m_nDelay; } }
            public int PreDelay { set { m_nPreDelay = value; } get { return m_nPreDelay; } }
            public bool CheckMotion { set { m_bCheckMotion = value; } get { return m_bCheckMotion; } }
            public string Caption { set { m_sCaption = value; } get { return m_sCaption; } }
            public int CaptionRetried { set { m_nCaptionRetried = value; } get { return m_nCaptionRetried; } }
            #endregion
        }
        #endregion </STRUCTURE>

        #region <FIELD>

        #region Instance
        protected Recipe.Recipe _recipe = Recipe.Recipe.GetInstance();
        private Task.TaskOperator _taskOperator = Task.TaskOperator.GetInstance();
        private TaskDevice_.TaskDevice _taskDevice = TaskDevice_.TaskDevice.GetInstance();
		private MotionInstance::Motion_.Motion m_instanceMotion = MotionInstance::Motion_.Motion.GetInstance();
        private AnalogIO_.AnalogIO m_instanceAnalog = AnalogIO_.AnalogIO.GetInstance();
        #endregion

        #region Control Variable
        /// <summary>
        ///  KEY : AXIS ITEM INDEX IN TASK
        /// </summary>
        private Dictionary<int, MoveMotionData> m_dicMoveData = new Dictionary<int, MoveMotionData>();
        #endregion

        #region Recovery Data
        protected RecoveryData m_RecoveryData = null;
        #endregion

		protected string _actionName = STOP_ACTION;
		protected const string STOP_ACTION = "STOP";
		#endregion </FIELD>

		#region <CONSTRUCTOR>
		protected RunningTaskEx(int nIndexOfTask, string strTaskName, int nCountOfProcessParam)
			: base(nIndexOfTask, strTaskName, nCountOfProcessParam, Define.DefineConstant.FilePath.FILEPATH_LOG_MAIN)
        {
            InitSubSequence();
        }
        #endregion </CONSTRUCTOR>

        #region <ABSTRACT>
        abstract protected void InitSubSequence();
        abstract protected bool IsMoveAbsolutelyOK(int nTargetIndex, double dblPosition);
        abstract protected bool IsMoveReleativelyOK(int nTargetIndex, double dblDistance);

		// 2022.11.18 by junho [ADD] Entry에서 Subsequence들의 step을 초기화 시켜주어야 한다.
		abstract protected void SubSequenceStepInit();
        #endregion </ABSTRACT>

        #region <INTERFACE>

        #region Move Motion

        #region Clear Move Motion
        public void ClearMoveMotion()
        {
            m_dicMoveData.Clear();
        }
        #endregion

        public void SetDelayForSubSequence(int delay)
        {
            SetDelayForSequence(delay);
        }

        #region Ready Position
        public bool AddMoveReady(int nTargetIndex
            , MOTION_SPEED_CONTENT enContent = MOTION_SPEED_CONTENT.RUN
            , int nRatio = 100
            , int nDelay = 100
            , bool bCheckMotion = true)
        {
            if (true == m_dicMoveData.ContainsKey(nTargetIndex))
                return false;

            if (nTargetIndex == -1)
                return true;

            double dblPosition = 0;
            int nMotionTarget = 0;

            _taskDevice.GetDeviceTargetIndex(GetTaskName()
                , TaskDevice_.TYPE_DEVICE.MOTION
                , nTargetIndex
                , ref nMotionTarget);

            m_instanceMotion.GetParameter(nMotionTarget, PARAM_LIST_HOME.HOME_BASE_POSITION, ref dblPosition);

            MoveMotionData cMotion = new MoveMotionData(nTargetIndex);
            cMotion.ControlType = EN_MOTION_CONTROL_TYPE.ABSOLUTELY;
            cMotion.TargetIndex = nTargetIndex;
            cMotion.Position = dblPosition;
            cMotion.Content = enContent;
            cMotion.Ratio = nRatio;
            cMotion.Delay = nDelay;
            cMotion.CheckMotion = bCheckMotion;

            m_dicMoveData.Add(nTargetIndex, cMotion);

            return true;
        }
        #endregion

        #region Absolutely

        #region Normal
		/// <summary>
		/// 2022.01.06 by twkang [ADD] 
		/// </summary>
		public bool AddMoveAbsolutely(int nTargetIndex, double dblPosition
			, int nRatio
            , MOTION_SPEED_CONTENT enContent
			, bool bCheckMotion = true
			, int nDelay = 10
            , int nPreDelay = 10
            , string caption = ""
            , int captionRetried = 0)
		{
			if (true == m_dicMoveData.ContainsKey(nTargetIndex))
				return false;

			if (nTargetIndex == -1)
				return true;

			MoveMotionData cMotion	= new MoveMotionData(nTargetIndex);
			cMotion.ControlType = EN_MOTION_CONTROL_TYPE.ABSOLUTELY;
			cMotion.TargetIndex = nTargetIndex;
			cMotion.Position = dblPosition;
			cMotion.Content = enContent;
			cMotion.Ratio = nRatio;
			cMotion.Delay = nDelay;
			cMotion.CheckMotion = bCheckMotion;
            cMotion.Caption = caption;
            cMotion.CaptionRetried = captionRetried;

			m_dicMoveData.Add(nTargetIndex, cMotion);

			return true;
		}

        public bool AddMoveAbsolutely(int nTargetIndex, double dblPosition
            , int nRatio = 100
            , bool bCheckMotion = true
            , int nDelay = 10
            , int nPreDelay = 10
            , string caption = ""
            , int captionRetried = 0)
        {
            MOTION_SPEED_CONTENT enContent =  MOTION_SPEED_CONTENT.RUN;

			return AddMoveAbsolutely(nTargetIndex, dblPosition, nRatio, enContent, bCheckMotion, nDelay, nPreDelay, caption, captionRetried);
        }
        public bool AddMoveAbsolutely(int nTargetIndex, double dblPosition
            , double dCustomSpeed
            , int nRatio = 100
            , bool bCheckMotion = true
            , int nDelay = 10
            , int nPreDelay = 10
			, string caption = ""
			, int captionRetried = 0)
        {
            if (true == m_dicMoveData.ContainsKey(nTargetIndex))
                return false;

            if (nTargetIndex == -1)
                return true;

            MoveMotionData cMotion = new MoveMotionData(nTargetIndex);
            cMotion.ControlType = EN_MOTION_CONTROL_TYPE.ABSOLUTELY;
            cMotion.TargetIndex = nTargetIndex;
            cMotion.Position = dblPosition;
            cMotion.CustomSpeed = dCustomSpeed;
            cMotion.Content = MOTION_SPEED_CONTENT.RUN;
            cMotion.Ratio = nRatio;
            cMotion.Delay = nDelay;
            cMotion.PreDelay = nPreDelay;
            cMotion.CheckMotion = bCheckMotion;
            cMotion.Caption = caption;
            cMotion.CaptionRetried = captionRetried;

            m_dicMoveData.Add(nTargetIndex, cMotion);

            return true;
        }
        #endregion

        public MOTION_RESULT MoveByList(int nMotionIndex
            , int nCountOfStep
            , double[] arDestination
            , double[] arCustomSpeed
            , int[] arRatio = null
            , MOTION_SPEED_CONTENT[] arContent = null
            , int nDelay = 10
            , int nPreDelay = 0
            , bool bCheckMotion = true
            , string caption = ""
            , int captionRetried = 0)
        {
            int nRatio = 100;
            if (null == arRatio)
            {
                arRatio = new int[nCountOfStep];
                for (int nIndex = 0; nIndex < nCountOfStep; ++nIndex)
                {
                    arRatio[nIndex] = nRatio;
                }
            }
            if (null == arContent)
            {
                arContent = new Motion_.MOTION_SPEED_CONTENT[nCountOfStep];
                for (int nIndex = 0; nIndex < nCountOfStep; ++nIndex)
                {
                    arContent[nIndex] = Motion_.MOTION_SPEED_CONTENT.RUN;
                }
            }

            return base.MoveByList(ComplementToCaption(caption), nMotionIndex, nCountOfStep, ref arDestination, ref arCustomSpeed, ref arContent, ref arRatio, nDelay, nPreDelay, bCheckMotion, captionRetried);
        }

		public MOTION_RESULT MoveByLinearCoordination(int[] arIndexes
			, int nCountOfStep
			, double[,] arDestination
			, int[,] arRatio	= null
			, MOTION_SPEED_CONTENT[,] arContent	= null
			, int nDelay = 10
            , int nPreDelay = 0
            , bool bCheckMotion = true
            , string caption = ""
            , int captionRetried = 0)
		{

            return MoveByLinearCoordination(ComplementToCaption(caption), arIndexes.Length, ref arIndexes, nCountOfStep, ref arDestination, ref arContent, ref arRatio, nDelay, nPreDelay, bCheckMotion, captionRetried);
		}

        public MOTION_RESULT MoveByLinearCoordination(int[] arIndexes
            , int nCountOfStep
            , double[,] arDestination
            , double[,] arVelocity
            , int[,] arRatio = null
            , MOTION_SPEED_CONTENT[,] arContent = null
            , int nDelay = 10
            , int nPreDelay = 0
            , bool bCheckMotion = true
            , string caption = ""
            , int captionRetried = 0)
        {
            return MoveByLinearCoordination(ComplementToCaption(caption), arIndexes.Length, ref arIndexes, nCountOfStep, ref arDestination, ref arVelocity, ref arContent, ref arRatio, nDelay, nPreDelay, bCheckMotion, captionRetried);
        }

        public MOTION_RESULT MoveSyncVectorMotion(int[] arIndexes
            , double[] arDestination
            , double dCustomSpeed = -1
            , MOTION_SPEED_CONTENT enContent = MOTION_SPEED_CONTENT.RUN
            , int arRatio = 100
            , bool bAbsolute = true
            , bool bOverride = false
            , int nDelay = 10
            , int nPreDelay = 0
            , bool bCheckMotion = true
            , string caption = ""
            , int captionRetried = 0)
        {
            int nCountOfMotion = arIndexes.Length;
            return MoveSyncVectorMotion(ComplementToCaption(caption), ref nCountOfMotion, ref arIndexes, ref arDestination, ref dCustomSpeed, ref enContent, ref arRatio, ref bAbsolute, ref bOverride, ref nDelay, nPreDelay, ref bCheckMotion, captionRetried);
        }

        public MOTION_RESULT MoveSyncVectorMotionList(int[] arIndexes
            , double[,] arDestination
            , double[] dCustomSpeed = null
            , MOTION_SPEED_CONTENT[] arContent = null
            , int[] arRatio = null
            , bool bAbsolute = true
            , int nDelay = 10
            , int nPreDelay = 0
            , bool bCheckMotion = true
            , string caption = ""
            , int captionRetried = 0)
    
        {
            int nCountOfAxis = arIndexes.Length;

            return MoveSyncVectorMotionList(ComplementToCaption(caption), ref nCountOfAxis, ref arIndexes, ref arDestination, ref dCustomSpeed, ref arContent, ref arRatio, ref bAbsolute, ref nDelay, nPreDelay, ref bCheckMotion, captionRetried);
        }

        public MOTION_RESULT MoveMultiAbsolutely(int[] arIndexes
			, double[] arDestination
			, bool bCheckMotion = true
			, MOTION_SPEED_CONTENT enContent = MOTION_SPEED_CONTENT.RUN
			, int nRatio = 100
			, int nDelay = 10
            , string caption = ""
            , int captionRetried = 0)
		{
            int nCountOfAxis = arIndexes.Length;

            int[] arRatio = new int[nCountOfAxis];
            Motion_.MOTION_SPEED_CONTENT[] arContent = new Motion_.MOTION_SPEED_CONTENT[nCountOfAxis];

            for (int nIndex = 0; nIndex < nCountOfAxis; ++nIndex)
            {
                arRatio[nIndex] = nRatio;
                arContent[nIndex] = (Motion_.MOTION_SPEED_CONTENT)enContent;
            }

			return MoveMultiAbsolutely(ComplementToCaption(caption), nCountOfAxis, ref arIndexes, ref arDestination, ref arContent, ref arRatio, nDelay, bCheckMotion, captionRetried);
		}


        public MOTION_RESULT MoveUntilTouch(int nKey
			, double dblDestination
			, int nIndexOfEncoder
            , double dblEncoderThreshold
            , double dblCustomSpeed
			, MOTION_SPEED_CONTENT enContent = MOTION_SPEED_CONTENT.RUN
			, int nRatio	= 100
			, int nDelay	= 100
			, int nPreDelay	= 0
			, bool bCheckMotion = true
            , string caption = ""
            , int captionRetried = 0)
		{
            return base.MoveUntilTouch(ComplementToCaption(caption), nKey, dblDestination, nIndexOfEncoder, dblEncoderThreshold, ref dblCustomSpeed, enContent, nRatio, nDelay, nPreDelay, bCheckMotion, captionRetried);
		}

        public MOTION_RESULT MoveUntilTouch_Analog(int nKey
            , double dblDestination
            , int[] nIndexOfAnalog
            , double dblEncoderThreshold
            , double dblCustomSpeed
            , MOTION_SPEED_CONTENT enContent = MOTION_SPEED_CONTENT.RUN
            , int nRatio = 100
            , int nDelay = 100
            , int nPreDelay = 0
            , bool bCheckMotion = true
            , string caption = ""
            , int captionRetried = 0)
        {
            //test 후 running task 안으로 넣어야 할 듯
            double ChanelValue = dblEncoderThreshold / nIndexOfAnalog.Length;
            dblEncoderThreshold = 0;
            for (int nIndex = 0; nIndex < nIndexOfAnalog.Length; nIndex++)
            {
                int nTargetIndex = 0;
                GetDeviceTargetIndex(nIndexOfAnalog[nIndex], Config.ConfigDevice.EN_TYPE_DEVICE.ANALOG_INPUT, ref nTargetIndex);
                dblEncoderThreshold += m_instanceAnalog.GetConvertedCountFromTableValue(true, nTargetIndex, ChanelValue);
            }

            return base.MoveUntilTouch_Analog(ComplementToCaption(caption), nKey, dblDestination, ref nIndexOfAnalog, dblEncoderThreshold, ref dblCustomSpeed, enContent, nRatio, nDelay, nPreDelay, bCheckMotion, captionRetried);
        }

        public MOTION_RESULT MoveByListUntilTouch(int nKey
            , int nCountOfStep
            , double[] arDestination
            , int nIndexOfEncoder
            , double dblEncoderThreshold
            , double[] arCustomSpeed
            , int[] arRatio = null
            , MOTION_SPEED_CONTENT[] arContent = null
            , int nDelay = 100
            , int nPreDelay = 0
            , bool bCheckMotion = true
            , string caption = ""
            , int captionRetried = 0)
        {
			// 2024.03.10. by shkim. [ADD] 매개변수 없을 때의 예외처리 추가, NULL 이면 안된다.
            if(arRatio == null)
            {
                arRatio = new int[arDestination.Length];
                for (int i = 0; i < arDestination.Length; i++)
                {
                    arRatio[i] = 100;
                }
            }
            if (arContent == null)
            {
                arContent = new MOTION_SPEED_CONTENT[arDestination.Length];

                for (int i = 0; i < arDestination.Length; i++)
                {
                    arContent[i] = MOTION_SPEED_CONTENT.RUN;
                }
            }
			// 2024.03.10. by shkim. [END]
            return base.MoveByListUntilTouch(ComplementToCaption(caption), nKey, nCountOfStep, ref arDestination, nIndexOfEncoder, dblEncoderThreshold, ref arCustomSpeed, ref arContent, ref arRatio, nDelay, nPreDelay, bCheckMotion, captionRetried);
        }

        public MOTION_RESULT MoveByListUntilTouch_Analog(int nKey
            , int nCountOfStep
            , double[] arDestination
            , int[] nIndexOfAnalog
            , double dblEncoderThreshold
            , double[] arCustomSpeed
            , int[] arRatio = null
            , MOTION_SPEED_CONTENT[] arContent = null
            , int nDelay = 100
            , int nPreDelay = 0
            , bool bCheckMotion = true
            , string caption = ""
            , int captionRetried = 0)
        {
		// 2024.03.10. by shkim. [ADD] 매개변수 없을 때의 예외처리 추가, NULL 이면 안된다.
            if (arRatio == null)
            {
                arRatio = new int[arDestination.Length];
                for (int i = 0; i < arDestination.Length; i++)
                {
                    arRatio[i] = 100;
                }
            }
            if (arContent == null)
            {
                arContent = new MOTION_SPEED_CONTENT[arDestination.Length];

                for (int i = 0; i < arDestination.Length; i++)
                {
                    arContent[i] = MOTION_SPEED_CONTENT.RUN;
                }
            }
						// 2024.03.10. by shkim. [END]
            //test 후 running task 안으로 넣어야 할 듯
            double ChanelValue = dblEncoderThreshold / nIndexOfAnalog.Length;
            // dblEncoderThreshold = 0;	// 2024.03.09. by shkim. [DEL] 버그 fix
            for (int nIndex = 0; nIndex < nIndexOfAnalog.Length; nIndex++)
            {
                int nTargetIndex = 0;
                GetDeviceTargetIndex(nIndexOfAnalog[nIndex], Config.ConfigDevice.EN_TYPE_DEVICE.ANALOG_INPUT, ref nTargetIndex);
                dblEncoderThreshold += m_instanceAnalog.GetConvertedCountFromTableValue(true, nTargetIndex, ChanelValue);
            }
            return base.MoveByListUntilTouch_Analog(ComplementToCaption(caption), nKey, nCountOfStep, ref arDestination, ref nIndexOfAnalog, dblEncoderThreshold, ref arCustomSpeed, ref arContent, ref arRatio, nDelay, nPreDelay, bCheckMotion, captionRetried);
        }

        public MOTION_RESULT MovePVT(int nKey
            , int nCountOfStep
            , double[] arDestination
            , double[] arVelocity
            , double[] arTime
            , int nPreDelay = 0
            , bool bCheckMotion = true
            , string caption = ""
            , int captionRetried = 0)
        {
            return base.MovePVT(ComplementToCaption(caption), nKey, nCountOfStep, ref arDestination, ref arVelocity, ref arTime, nPreDelay, bCheckMotion, captionRetried);
        }
		public bool IsTouched(int nKey, ref double dblTouchedPosition, ref double dblTouchedEncoderPosition)
		{
			return IsMotionTouched(nKey, ref dblTouchedPosition, ref dblTouchedEncoderPosition);
		}

		#endregion

		#region Releative
		public bool AddMoveReleative(int nTargetIndex, double dblDistance
            , MOTION_SPEED_CONTENT enContent = MOTION_SPEED_CONTENT.RUN
            , int nRatio = 100
            , int nDelay = 10
            , bool bCheckMotion = true
			, string caption = ""
			, int captionRetried = 0)
        {
            if (true == m_dicMoveData.ContainsKey(nTargetIndex))
                return false;

            if (dblDistance == 0)
                return false;

            if (nTargetIndex == -1)
                return true;

            MoveMotionData cMotion = new MoveMotionData(nTargetIndex);
            cMotion.ControlType = EN_MOTION_CONTROL_TYPE.RELEATIVELY;
            cMotion.TargetIndex = nTargetIndex;
            cMotion.Position = dblDistance;
            cMotion.Content = enContent;
            cMotion.Ratio = nRatio;
            cMotion.Delay = nDelay;
            cMotion.CheckMotion = bCheckMotion;
            cMotion.Caption = caption;
            cMotion.CaptionRetried = captionRetried;

            m_dicMoveData.Add(nTargetIndex, cMotion);

            return true;
        }
      
        #endregion

        #region Move Motion
        /// <summary>
        /// 2021.05.03 by ssh [ADD] ADD 된 Control Motion 을 간섭 확인 후 일괄 적용
        /// </summary>
        /// <returns></returns>
        public MOTION_RESULT MoveMotion()
        {
            MOTION_RESULT enResult = MOTION_RESULT.OK;

            foreach (var MoveData in m_dicMoveData)
            {
                switch (MoveData.Value.ControlType)
                {
                    case EN_MOTION_CONTROL_TYPE.ABSOLUTELY:
                        if (false == IsMoveAbsolutelyOK(MoveData.Key, MoveData.Value.Position))
                            return MOTION_RESULT.NOT_READY;

                        break;
                    case EN_MOTION_CONTROL_TYPE.RELEATIVELY:
                        if (false == IsMoveReleativelyOK(MoveData.Key, MoveData.Value.Position))
                            return MOTION_RESULT.NOT_READY;

                        break;
                }
            }

            foreach (var MoveData in m_dicMoveData)
            {
                switch (MoveData.Value.ControlType)
                {
                        // 2022.04.27. by shkim. [ADD] 커스텀 스피드 적용
                    case EN_MOTION_CONTROL_TYPE.ABSOLUTELY:
                        enResult = MoveAbsolutely(MoveData.Value.Caption
                            , MoveData.Value.TargetIndex
                            , MoveData.Value.Position
                            , MoveData.Value.CustomSpeed
                            , MoveData.Value.Content
                            , MoveData.Value.Ratio
                            , MoveData.Value.Delay
                            , MoveData.Value.PreDelay
                            , MoveData.Value.CheckMotion
                            , MoveData.Value.CaptionRetried);
                        break;

                    case EN_MOTION_CONTROL_TYPE.RELEATIVELY:
                        enResult = MoveReleatively(MoveData.Value.Caption
                            , MoveData.Value.TargetIndex
                            , MoveData.Value.Position
                            , MoveData.Value.Content
                            , MoveData.Value.Ratio
                            , MoveData.Value.Delay
                            , MoveData.Value.PreDelay
                            , MoveData.Value.CheckMotion
                            , MoveData.Value.CaptionRetried);
                        break;
                }
            }

            ClearMoveMotion();
			
            return enResult;
        }
        #endregion

        #region Stop
        public MOTION_RESULT StopMotion(int nTargetIndex, bool isEmergency = false, int delay = 0, string caption = "", int captionRetried = 0)
        {
			caption = caption == "" ? string.Format("{0}_Stop", _actionName) : string.Format("{0}_Stop_{1}_{2}", _actionName, caption, m_nSeqNum.ToString());
			return StopMotion(caption, nTargetIndex, isEmergency, delay, captionRetried);
        }
        #endregion
        
        #region Override
        public MOTION_RESULT MoveOverrideSpeed(int nTargetIndex
            , int nRatio = 100
            , MOTION_SPEED_CONTENT enContent = MOTION_SPEED_CONTENT.RUN
            , string caption = ""
            , int captionRetried = 0)
        {
            return OverrideMotion(ComplementToCaption(caption), nTargetIndex, Motion_.MOTION_OVERRIDE_TYPE.SPEED, 0, enContent, nRatio, captionRetried);
        }

        /// <summary>
        /// 2024.01.26. by shkim. [ADD] 축이 도착하기 전 경로를 변경한다.
        /// </summary>
        public MOTION_RESULT MoveOverrideMotion(int nTargetIndex
            , double dblDestination
            , int nRatio = 100
            , MOTION_SPEED_CONTENT enContent = MOTION_SPEED_CONTENT.RUN
            , string caption = ""
            , int captionRetried = 0)
        {
            return OverrideMotion(ComplementToCaption(caption), nTargetIndex, Motion_.MOTION_OVERRIDE_TYPE.POSITION, dblDestination, enContent, nRatio, captionRetried);
        }

        #endregion

        #region Speed Move
        public MOTION_RESULT MoveSpeedDirection(int nTargetIndex, bool bDirection
            , int nRatio = 100
            , int nPreDelay = 0
            , bool bCheckMotion = false
            , string caption = ""
            , int captionRetried = 0)
        {
            return MoveAtSpeed(ComplementToCaption(caption), nTargetIndex, bDirection, nRatio, nPreDelay, bCheckMotion, captionRetried);
        }
        #endregion

        #region Home
        public MOTION_RESULT MoveHome(int nKey, int nPreDelay = 0, bool bCheckMotion = true)
        {
			return MoveToHome(nKey, "Home", nPreDelay, bCheckMotion, 0);
        }
        public bool IsHomeFinish(int nKey)
        {
            return IsHomeEnd(nKey);
        }
        #endregion

        #region GetPostion
		public double GetAxisActualPosition(int nKey)
		{
			return base.GetActualPosition(nKey);
		}
		public double GetAxisCommandPosition(int nKey)
		{
			return base.GetCommandPosition(nKey);
		}
        #endregion

        #region SetPostion
        public void SetPostion(int nKey, double dPose)
        {
            SetActualPosition(nKey, dPose);
            SetCommandPosition(nKey, dPose);
        }
        #endregion

		public void SetMotionSpeed(int nKey, Config.ConfigMotion.EN_SPEED_CONTENT contant, double speed)
		{
			// Search speed를 recipe값으로 설정
			int nAxisNumber = -1;
			Config.ConfigDevice.GetInstance().GetDeviceTargetIndex(GetTaskName()
				, Config.ConfigDevice.EN_TYPE_DEVICE.MOTION
				, nKey
				, ref nAxisNumber);

			Config.ConfigMotionSpeed.GetInstance().SetSpeedParameter(nAxisNumber
				, contant
				, Config.ConfigMotionSpeed.EN_PARAM_SPEED.VELOCITY
				, speed);
		}
		public bool IsAxisMotionDone(int nKey)
		{
			return IsMotionDone(nKey);
		}
		public bool IsMotionTouched(int nKey, out double touchedPosition)
		{
			touchedPosition = 0.0;
			double encoderPosition = 0.0;
			if (false == IsMotionTouched(nKey, ref touchedPosition, ref encoderPosition))
				return false;

			touchedPosition += encoderPosition;
			return true;
		}

        #region Mei Gain
		
		// 2024.11.16. by shkim. [MOD] Bug Fix. 값 비교 후 다를 때만 Setting 되게 되어있어 적용 안되었었다.
        public bool SetGainOffset(int nIndex, double dValue)
        {
            int nTargetKey = 0;

            Config.ConfigDevice.GetInstance().GetDeviceTargetIndex(GetTaskName()
                                                    , Config.ConfigDevice.EN_TYPE_DEVICE.MOTION
                                                    , nIndex
                                                    , ref nTargetKey);

            //if (dValue != Config.ConfigMotion.GetInstance().GetOutputOffset(nTargetKey))
            //    return false;

            Config.ConfigMotion.GetInstance().SetOutputOffset(nTargetKey, dValue);

            return true;
        }
        #endregion /Mei Gain
        #endregion

        #region Move Cylinder
        // 2024.09.19 by jhshin [MOD] add action name, retry count
        public CYLINDER_RESULT MoveCylinderForward(int nTargetIndex, bool bCheckCylinder, int captionRetried = 0)
        {
            if (nTargetIndex == -1)
                return CYLINDER_RESULT.INVALID_INDEX;

            return MoveForward(nTargetIndex, bCheckCylinder, ComplementToCaption("Forward"), captionRetried);
        }
        // 2024.09.19 by jhshin [MOD] add action name, retry count
        public CYLINDER_RESULT MoveCylinderBackward(int nTargetIndex, bool bCheckCylinder, int captionRetried = 0)
        {
            if (nTargetIndex == -1)
                return CYLINDER_RESULT.INVALID_INDEX;

            return MoveBackward(nTargetIndex, bCheckCylinder, ComplementToCaption("Backward"), captionRetried);
        }

		// 2022.05.04 by junho [ADD] Is Add interface for cylinder steate
		public bool IsCylinderForward(int nTargetIndex, bool defaultValue)
		{
			if (nTargetIndex == -1)
				return defaultValue;

			return IsForward(nTargetIndex);
		}
		public bool IsCylinderBackward(int nTargetIndex, bool defaultValue)
		{
			if (nTargetIndex == -1)
				return defaultValue;

			return IsBackward(nTargetIndex);
		}
		#endregion

        #region Digital I/O

        #region Input
        public bool ReadDigitalInput(int nTargetKey, bool bReadData, int nDelayTime)
        {
            if (nTargetKey == -1)
                return bReadData;

            if (_taskOperator.GetRunMode() == RunningMain_.RUN_MODE.DRY_RUN
                || _taskOperator.GetRunMode() == RunningMain_.RUN_MODE.SIMULATION)
            {
                SetDelayForSequence(nDelayTime);

                return bReadData;
            }

            return ReadInput(nTargetKey, bReadData);
        }
        #endregion

        #region Output
        public bool ReadDigitalOutput(int nTargetKey, bool bReadData, int nDelayTime)
        {
            if (nTargetKey == -1)
                return true;

            if (_taskOperator.GetRunMode() == RunningMain_.RUN_MODE.DRY_RUN
                || _taskOperator.GetRunMode() == RunningMain_.RUN_MODE.SIMULATION)
            {
                SetDelayForSequence(nDelayTime);

                return bReadData;
            }

            return ReadOutput(nTargetKey, bReadData);
        }

        public void WriteDigitalOutput(int nTargetKey, bool bWriteValue, string caption = "", int captionRetried = 0)
        {
            if (nTargetKey == -1)
                return;

            WriteOutput(nTargetKey, ComplementToCaption(caption), bWriteValue, captionRetried);
        }
        #endregion

        #region Write And Read
        public bool WriteDigitalAndRead(int nOutputIndex, bool bOutputActive, int[] nInputIndex, bool[] bInputActive, string caption = "", int captionRetried = 0)
        {
            for (int i = 0; i < nInputIndex.Length; ++i)
            {
                if (ReadInput(nInputIndex[i], bInputActive[i])
                    != bInputActive[i])
                {
                    WriteOutput(nOutputIndex, ComplementToCaption(caption), bOutputActive, captionRetried);
                    return false;
                }
            }

            return true;
        }
        #endregion

        #endregion

        #region Analog I/O

        #region Input
        public double ReadAnalogInputVolt(int nTargetKey)
        {
            if (nTargetKey == -1)
                return 0;

            return ReadInputVolt(nTargetKey);
        }
        public double ReadAnalogInputValue(int nTargetKey)
        {
            if (nTargetKey == -1)
                return 0;

            return ReadInputValue(nTargetKey);
        }
        #endregion

        #region Output
        public double ReadAnalogOutputVolt(int nTargetKey)
        {
            if (nTargetKey == -1)
                return 0;

            return ReadOutputVolt(nTargetKey);
        }
        public double ReadAnalogOutputValue(int nTargetKey)
        {
            if (nTargetKey == -1)
                return 0;

            return ReadOutputValue(nTargetKey);
        }
        public void WriteAnalogOutputVolt(int nTargetKey, double dblWriteValue, string caption = "", int captionRetried = 0)
        {
            if (nTargetKey == -1)
                return;

            WriteOutputVolt(nTargetKey, ComplementToCaption(caption), dblWriteValue, captionRetried);
        }
        public void WriteAnalogOutputValue(int nTargetKey, double dblWriteValue, string caption = "", int captionRetried = 0)
        {
            if (nTargetKey == -1)
                return;

            WriteOutputValue(nTargetKey, ComplementToCaption(caption), dblWriteValue, captionRetried);
        }
        #endregion

        #region Write And Read
        public bool WriteAnalogAndRead(int nOutputIndex
            , int nOutputType
            , double dblOutputActive
            , int[] nInputIndex
            , int[] nInputType
            , double[] dblInputActive
            , double[] dblInputMinusError
            , double[] dblInputPlusError
            , string caption = "", int captionRetried = 0)
        {
            switch (nOutputType)
            {
                // Volt
                case 0:
                    WriteOutputVolt(nOutputIndex, ComplementToCaption(caption), dblOutputActive, captionRetried);
                    break;

                // Value
                case 1:
                    WriteOutputValue(nOutputIndex, ComplementToCaption(caption), dblOutputActive, captionRetried);
                    break;
            }

            double dblMinusActive = 0;
            double dblTargetActive = 0;
            double dblPlusActive = 0;

            for (int i = 0; i < nInputIndex.Length; ++i)
            {
                switch (nOutputType)
                {
                    // Volt
                    case 0:
                        dblTargetActive = ReadInputVolt(nInputIndex[i]);
                        break;

                    // Value
                    case 1:
                        dblTargetActive = ReadInputValue(nInputIndex[i]);
                        break;
                }

                dblMinusActive = dblInputActive[i] - dblInputMinusError[i];
                dblPlusActive = dblInputActive[i] + dblInputPlusError[i];

                if (dblTargetActive <= dblMinusActive || dblPlusActive <= dblTargetActive)
                    return false;
            }

            return true;
        }
        #endregion

        #endregion

        public bool IsDryRunOrSimulation()
        {
            return _taskOperator.GetRunMode() == RunningMain_.RUN_MODE.DRY_RUN
                || _taskOperator.GetRunMode() == RunningMain_.RUN_MODE.SIMULATION;
        }
		public bool IsSimulation()
		{
			return _taskOperator.GetRunMode() == RunningMain_.RUN_MODE.SIMULATION;
		}
		
        #region Get TargetIndex
        public bool GetDeviceTargetIndex(int nIndexOfItem, Config.ConfigDevice.EN_TYPE_DEVICE enDeviceType, ref int nIndex)
        {
            if (!Config.ConfigDevice.GetInstance().GetDeviceTargetIndex(GetTaskName()
                 , enDeviceType
                 , nIndexOfItem
                 , ref nIndex))
                return false;

            return true;
        }
		#endregion
		#endregion </INTERFACE>

		private string ComplementToCaption(string caption)
		{
			return caption == ""
				? string.Format("{0}_{1}", _actionName, m_nSeqNum.ToString())
                : string.Format("{0}_{1}_{2}", _actionName, m_nSeqNum.ToString(), caption);
		}
	}
}
