using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Globalization;

using FrameOfSystem3.Controller.Motion.MechatroLinkAPI.References;
using FrameOfSystem3.Controller.Motion.MechatroLinkAPI.MechatroLinkDefines;
using FrameOfSystem3.Controller.Motion.MechatroLinkAPI.MechatroLinkAxis;

namespace FrameOfSystem3.Controller.Motion.MechatroLinkAPI
{
    class MechatrolinkApiWrapper : iCommunicator
    {
        #region <Constructor>
        public MechatrolinkApiWrapper()
        {
            m_thRunning = new Thread(new ThreadStart(Execute));
            m_thRunning.IsBackground = true;
            m_thRunning.Name = "MechatrolinkGathering";
            m_thRunning.Start();
        }
        #endregion </Constructor>

        #region <Fields>
        private int m_nAxisCount;
        private int m_nServoCount;
        private int m_nSteppingCount;

        private static MechatrolinkApiWrapper _Instance = new MechatrolinkApiWrapper();

        private UInt32 m_pHandleController = 0;

        private MechatrolinkAxis[] m_arAxis = null;

        private bool m_bConnected = false;
        private Thread m_thRunning = null;

        private double[] m_arActualPosition = null;
        private double[] m_arCommandPosition = null;
        private int[] m_arState = null;

		//private UInt32 m_uPrevState = 0;		// 미사용?
        private TickCounter_.TickCounter _timeoutFromApi = new TickCounter_.TickCounter();
        private TickCounter_.TickCounter _timeChecker = new TickCounter_.TickCounter();
        private TickCounter_.TickCounter _timeGantrySeq = new TickCounter_.TickCounter();

        private ConcurrentQueue<CONTROLLER_COMMAND_TYPE> m_queueControllerCommand = new ConcurrentQueue<CONTROLLER_COMMAND_TYPE>();

        private int m_nOpeningCount = 0;
        ReaderWriterLockSlim rw = new ReaderWriterLockSlim();

        private ConcurrentDictionary<int, bool> m_dicGantryParam = new ConcurrentDictionary<int, bool>();
        private int m_nGearingSeq;
        #endregion </Fields>

        #region <Properties>
        public static MechatrolinkApiWrapper Instance
        {
            get
            {
                return _Instance;
            }
        }

        public bool Connect { get { return m_bConnected; } }

        public int AxisCount { get { return m_nAxisCount; } }
        #endregion </Properties>

        #region <Methods>

        #region <External Interfaces>

        #region <Controller>
        public bool Init()
        {
            uint uTimeout = 2000;
            if (Debugger.IsAttached)
                uTimeout = 180000;

            _timeoutFromApi.SetTickCount(uTimeout);

            while (true)
            {
                Thread.Sleep(50);

                if (m_bConnected)
                    break;

                if (_timeoutFromApi.IsTickOver(false))
                    break;
            }

            System.Console.WriteLine(String.Format("Flag:{0}, Time:{1}", m_bConnected, _timeoutFromApi.GetTickCount()));

            return m_bConnected;
            //return OpenApi();
        }

        public bool Exit()
        {
            if (false == m_bConnected)
                return true;

            m_queueControllerCommand.Enqueue(CONTROLLER_COMMAND_TYPE.EXIT_CONTROLLER);

            _timeoutFromApi.SetTickCount(2000);

            while (true)
            {
                Thread.Sleep(50);

                if (false == m_bConnected)
                    break;

                if (_timeoutFromApi.IsTickOver(false))
                    break;
            }

            System.Console.WriteLine(String.Format("Flag:{0}, Time:{1}", m_bConnected, _timeoutFromApi.GetTickCount()));

            return (false == m_bConnected);
        }
        #endregion </Controller>

        #region <Axis Status>
        public void GetActualPositionAll(ref double[] arData)
        {
            if (false == m_bConnected)
                return;

            int nLength = arData.Length;
            if (nLength > m_arActualPosition.Length)
                nLength = m_arActualPosition.Length;

            Array.Copy(m_arActualPosition, arData, nLength);
        }

        public void GetCommandPositionAll(ref double[] arData)
        {
            if (false == m_bConnected)
                return;

            int nLength = arData.Length;
            if (nLength > m_arCommandPosition.Length)
                nLength = m_arCommandPosition.Length;

            Array.Copy(m_arCommandPosition, arData, nLength);
        }

        public void GetMotorStateAll(ref int[] arData)
        {
            if (false == m_bConnected)
                return;

            int nLength = arData.Length;
            if (nLength > m_arState.Length)
                nLength = m_arState.Length;

            Array.Copy(m_arState, arData, nLength);
        }

        public bool IsMotorOn(int nAxis)
        {
            if (false == m_bConnected)
                return false;

            if (false == CheckingValidityAxis(nAxis))
                return false;

            return m_arAxis[nAxis].ServoState;
        }

        public bool IsMotionHWLimitP(int nAxis)
        {
            if (false == m_bConnected)
                return false;

            if (false == CheckingValidityAxis(nAxis))
                return false;

            return m_arAxis[nAxis].HWLimitPositive;
        }

        public bool IsMotionHWLimitN(int nAxis)
        {
            if (false == m_bConnected)
                return false;

            if (false == CheckingValidityAxis(nAxis))
                return false;

            return m_arAxis[nAxis].HWLimitNegative;
        }

        public bool IsMotionDone(int nAxis)
        {
            if (false == m_bConnected)
                return false;

            if (false == CheckingValidityAxis(nAxis))
                return false;

            return m_arAxis[nAxis].MotionDone;
        }

        public bool IsHomeDone(int nAxis)
        {
            if (false == m_bConnected)
                return false;

            if (false == CheckingValidityAxis(nAxis))
                return false;

            return m_arAxis[nAxis].HomeCompletion;
        }

        public double GetActualPosition(int nAxis)
        {
            if (false == m_bConnected)
                return 0;

            if (false == CheckingValidityAxis(nAxis))
                return 0;

            return m_arActualPosition[nAxis];
        }

        public double GetCommandPosition(int nAxis)
        {
            if (false == m_bConnected)
                return 0;

            if (false == CheckingValidityAxis(nAxis))
                return 0;

            return m_arCommandPosition[nAxis];
        }

        public int GetMotorState(int nAxis)
        {
            if (false == m_bConnected)
                return 0;

            if (false == CheckingValidityAxis(nAxis))
                return 0;

            return m_arState[nAxis];
        }

        public bool GetGantryState(MCTL_PARAM_GANTRY mctlParam)
        {
            int nMaster = mctlParam.nAxisOfMaster;
            int nSlave = mctlParam.nAxisOfSlave;

            //return (m_arAxis[nMaster].IsGantrySettingComplete() &&
            //    m_arAxis[nSlave].IsGantrySettingComplete());
            int[] arAxes = { nMaster, nSlave };

            return (m_arAxis[nMaster].GantryState && m_arAxis[nSlave].GantryState);
        }
        #endregion </Axis Status>

        #region <Axis Commands>
        public void DoMotorOn(int nAxis, bool bEnabled)
        {
            if (false == m_bConnected)
                return;

            m_arAxis[nAxis].MotorOnOff(bEnabled);
            //if (m_arAxis[nAxis].IsGantrySettingComplete()/* && false == m_arAxis[nAxis].GantryState*/)
            //{
            //    if (bEnabled)
            //    {
            //        while(true)
            //        {
            //            Thread.Sleep(1);

            //            if (m_arAxis[nAxis].ServoState)
            //                break;
            //        }

            //        MCTL_PARAM_GANTRY stGantryParam = m_arAxis[nAxis].GetGantryParam();
            //        stGantryParam.bEnableGantry = true;

            //        SetGantry(stGantryParam, null);
            //    }
            //}
        }

        public void DoReset(int nAxis)
        {
            if (false == m_bConnected)
                return;

            m_arAxis[nAxis].DoReset();
        }

        public void SetPosition(int nAxis, double dPosition)
        {
            if (false == m_bConnected)
                return;

            m_arAxis[nAxis].SetPosition(dPosition);

            Thread.Sleep(10);
        }
        #endregion </Axis Commands>

        #region <Motion Commands>
        public void StopMotion(int nAxis, bool bEmergency)
        {
            if (false == m_bConnected)
                return;

            m_arAxis[nAxis].Stop(bEmergency);
        }

        public void StopHomeMotion(int nAxis, bool bEmergency)
        {
            StopMotion(nAxis, bEmergency);
        }

        public void MoveMotion(int nAxis, double dPosition, bool bAbsolute)
        {
            if (false == m_bConnected)
                return;

            //MCTL_PARAM_GANTRY gantry = new MCTL_PARAM_GANTRY();
            //gantry.bEnableGantry = true;
            //gantry.bReverseSlaveDirection = false;
            //gantry.nAxisOfMaster = 10;
            //gantry.nAxisOfSlave = 24;

            //m_arAxis[nAxis].SetGantry(gantry, null);
            m_arAxis[nAxis].Move(dPosition, bAbsolute);
        }

        public void MoveAtSpeed(int nAxis, bool bDirection)
        {
            if (false == m_bConnected)
                return;

            m_arAxis[nAxis].MoveAtSpeed(bDirection);
        }
        #endregion </Motion Commands>

        #region <Homing>
        public bool MoveToHome(int nAxis, ref int nSeq, ref int nDelay)
        {
            if (false == m_bConnected)
                return false;

            //if (m_arAxis[nAxis].IsGantrySettingComplete())
            //    return MoveToGantryHome(nAxis, ref nSeq, ref nDelay);
            //else
            return m_arAxis[nAxis].MoveToHome(ref nSeq, ref nDelay);
        }
        #endregion </Homing>

        #region <Parameter>
        public void SetMotorConfiguration(int nAxis, MCTL_PARAM_CONFIG stConfig)
        {
            m_arAxis[nAxis].SetMotorConfig(stConfig);
        }

        public void SetMotorSpeedConfiguration(int nAxis, MCTL_PARAM_SPEED stSpeed)
        {
            m_arAxis[nAxis].SetSpeedParam(stSpeed);
        }

        public void SetMotorHomeSpeedConfiguration(int nAxis, MCTL_PARAM_HOME stHomeParam)
        {
            m_arAxis[nAxis].SetHomingParam(stHomeParam);
        }

        public void SetGantryParam(MCTL_PARAM_GANTRY stGantryParam, MCTL_PARAM_SPEED stSpeed)
        {
            if (false == m_bConnected)
                return;

            if (false == CheckingValidityAxis(stGantryParam.nAxisOfMaster))
                return;

            if (false == CheckingValidityAxis(stGantryParam.nAxisOfSlave))
                return;

            m_arAxis[stGantryParam.nAxisOfMaster].SetGantryParam(stGantryParam);
            m_arAxis[stGantryParam.nAxisOfSlave].SetGantryParam(stGantryParam);
        }

        public void SetGantry(int[] arAxes)
        {
            if (false == m_bConnected)
                return;

            for (int i = 0; i < arAxes.Length; ++i)
            {
                if (false == CheckingValidityAxis(arAxes[i]))
                    return;
            }

            //if (false == m_arAxis[stGantryParam.nAxisOfMaster].ServoState)
            //    m_arAxis[stGantryParam.nAxisOfMaster].MotorOnOff(true);

            //if (false == m_arAxis[stGantryParam.nAxisOfSlave].ServoState)
            //    m_arAxis[stGantryParam.nAxisOfSlave].MotorOnOff(true);

            m_nGearingSeq = 0;
            MCTL_PARAM_GANTRY stGantryParam = m_arAxis[arAxes[0]].GetGantryParam();
            m_dicGantryParam.TryAdd(stGantryParam.nAxisOfMaster, stGantryParam.bEnableGantry);
            m_queueControllerCommand.Enqueue(CONTROLLER_COMMAND_TYPE.SET_GANTRY);
            //int[] arAxes = { stGantryParam.nAxisOfMaster, stGantryParam.nAxisOfSlave };
            //UInt32[] arHandles = { m_arAxis[stGantryParam.nAxisOfMaster].AxisHandle, 
            //                         m_arAxis[stGantryParam.nAxisOfSlave].AxisHandle };
            //bool bEnabled = stGantryParam.bEnableGantry;

            //if (bEnabled)
            //{
            //    m_arAxis[stGantryParam.nAxisOfMaster].MasterAxis = stGantryParam.nAxisOfMaster;
            //    m_arAxis[stGantryParam.nAxisOfMaster].SlaveAxis = stGantryParam.nAxisOfSlave;

            //    m_arAxis[stGantryParam.nAxisOfSlave].MasterAxis = stGantryParam.nAxisOfMaster;
            //    m_arAxis[stGantryParam.nAxisOfSlave].SlaveAxis = stGantryParam.nAxisOfSlave;
            //}

            //m_arAxis[stGantryParam.nAxisOfMaster].SetGantry(ref arHandles, arAxes, bEnabled);

            //m_arAxis[stGantryParam.nAxisOfMaster].SetGantryAxesIndex();
            //m_arAxis[stGantryParam.nAxisOfSlave].SetGantryAxesIndex();
            ////m_arAxis[stGantryParam.nAxisOfSlave].SetGantry(arAxes, bEnabled);

            //if (false == bEnabled)
            //{
            //    m_arAxis[stGantryParam.nAxisOfMaster].MasterAxis = -1;
            //    m_arAxis[stGantryParam.nAxisOfMaster].SlaveAxis = -1;

            //    m_arAxis[stGantryParam.nAxisOfSlave].MasterAxis = -1;
            //    m_arAxis[stGantryParam.nAxisOfSlave].SlaveAxis = -1;
            //}
        }
        #endregion </Parameter>

        #endregion </External Interfaces>

        #region <Internal Interfaces>

        #region <Init/Exit/Gathering>
        private void Execute()
        {
            while (true)
            {
                Thread.Sleep(1);

                if (false == m_bConnected)
                {
                    // 모션 연결 시도
                    if (false == OpenApi())
                    {
                        Thread.Sleep(500);

                        continue;
                    }
                }

                // 상태 읽어오기
                if (false == IsLinkConnected())
                {
                    m_bConnected = false;

                    Thread.Sleep(500);

                    continue;
                }

                if (m_arAxis == null)
                {
                    m_bConnected = false;

                    continue;
                }

                // 축 정보를 갱신한다.
                for (int i = 0; i < m_arAxis.Length; ++i)
                {
                    UpdateAxisStatus(i);
                }

                // Controller 관련 명령
                if (m_queueControllerCommand.Count > 0)
                {
                    CONTROLLER_COMMAND_TYPE enCommand;
                    m_queueControllerCommand.TryDequeue(out enCommand);
                    if (enCommand.Equals(CONTROLLER_COMMAND_TYPE.EXIT_CONTROLLER))
                    {
                        CloseApi();
                        return;
                    }
                    else if (enCommand.Equals(CONTROLLER_COMMAND_TYPE.SET_GANTRY))
                    {
                        bool bResult = true;
                        foreach (KeyValuePair<int, bool> kvp in m_dicGantryParam)
                        {
                            bResult &= SetGearedMotion(kvp.Key);
                        }

                        //System.Console.WriteLine("Set Gearing Seq : {0}", m_nGearingSeq);
                        if (bResult)
                        {
                            m_dicGantryParam.Clear();
                        }
                        else
                        {
                            m_queueControllerCommand.Enqueue(CONTROLLER_COMMAND_TYPE.SET_GANTRY);
                        }
                    }
                }
            }
        }

        private void UpdateCountInfo()
        {
            string strDir = @".\MechatrolinkConfig\";
            if (false == Directory.Exists(strDir))
            {
                Directory.CreateDirectory(strDir);
            }

            string strFullPath = String.Format(@"{0}MechatrolinkConfig.ini", strDir);

            IniControl _ini = new IniControl(strFullPath);

            m_nServoCount = _ini.GetInt(MechatrolinkDefines.SECTION_COUNTS,
                MechatrolinkDefines.KEY_COUNT_SERVO_AXIS, 26);

            m_nSteppingCount = _ini.GetInt(MechatrolinkDefines.SECTION_COUNTS,
                MechatrolinkDefines.KEY_COUNT_STEPPING_AXIS, 4);

            //_ini.WriteInt(MechatrolinkDefines.SECTION_COUNTS,
            //    MechatrolinkDefines.KEY_COUNT_SERVO_AXIS, m_nServoCount);

            //_ini.WriteInt(MechatrolinkDefines.SECTION_COUNTS,
            //    MechatrolinkDefines.KEY_COUNT_STEPPING_AXIS, m_nSteppingCount);

            m_nAxisCount = m_nServoCount + m_nSteppingCount;
        }

        private void InitAxisInfo()
        {
            string strDir = @".\MechatrolinkConfig\";
            if (false == Directory.Exists(strDir))
            {
                Directory.CreateDirectory(strDir);
            }

            string strFullPath = String.Format(@"{0}MechatrolinkConfig.ini", strDir);

            IniControl _ini = new IniControl(strFullPath);

            m_arActualPosition = new double[m_nAxisCount];
            m_arCommandPosition = new double[m_nAxisCount];
            m_arState = new int[m_nAxisCount];

            int nAxisIndex;
            m_arAxis = new MechatrolinkAxis[m_nAxisCount];

            UInt32 uCode = 0;
            string strKey = String.Empty, strInput = String.Empty, strOutput = String.Empty;
            string strMyName = String.Empty;
            UInt32 nInputAddress = 0, nOutputAddress = 0;
            for (int i = 0; i < m_nAxisCount; ++i)
            {
                nAxisIndex = i + 1;
                strMyName = String.Format("Axis-{0}", nAxisIndex);

                // Servo/Step 나눠서 설정한다.
                if (i < m_nServoCount)
                {
                    UInt32 pHandle = 0;

                    // 축 정의
                    uCode = DeclareAxis(nAxisIndex, strMyName, ref pHandle);
                    if (false == CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, nAxisIndex, "After ymcDeclareAxis"))
                        continue;

                    m_arAxis[i] = new MechatrolinkServoAxis(this, i, nAxisIndex, strMyName, pHandle);

                    /*int nOffset = i * 128;*/

                    // Input Address
                    strKey = String.Format("{0}_{1}", MechatrolinkDefines.KEY_SERVO_INPUT_ADDRESS, i);
                    strInput = _ini.GetString(MechatrolinkDefines.SECTION_SERVO_AXIS_INFO,
                    strKey, "0x8000");

                    strInput = strInput.Replace("0x", "");
                    UInt32.TryParse(strInput,
                        NumberStyles.HexNumber, CultureInfo.CreateSpecificCulture("en-US"),
                        out nInputAddress);

                    //_ini.WriteString(MechatrolinkDefines.SECTION_SERVO_AXIS_INFO, strKey, String.Format("{0:x}", nInputAddress));

                    // Output Address
                    strKey = String.Format("{0}_{1}", MechatrolinkDefines.KEY_SERVO_OUTPUT_ADDRESS, i);
                    strOutput = _ini.GetString(MechatrolinkDefines.SECTION_SERVO_AXIS_INFO,
                    strKey, "0x8000");

                    strOutput = strOutput.Replace("0x", "");
                    UInt32.TryParse(strInput,
                        NumberStyles.HexNumber, CultureInfo.CreateSpecificCulture("en-US"),
                        out nOutputAddress);

                    //_ini.WriteString(MechatrolinkDefines.SECTION_SERVO_AXIS_INFO, strKey, String.Format("{0:x}", nOutputAddress));

                    m_arAxis[i].ServoMotor = true;
                }
                else
                {
                    m_arAxis[i] = new MechatrolinkSteppingAxis(this, i, nAxisIndex, strMyName);
                    m_arAxis[i].ServoMotor = false;

                    int nOffsetedIndex = i - m_nServoCount;

                    // Input Address
                    strKey = String.Format("{0}_{1}", MechatrolinkDefines.KEY_STEPPING_INPUT_ADDRESS, nOffsetedIndex);
                    strInput = _ini.GetString(MechatrolinkDefines.SECTION_STEPPING_AXIS_INFO,
                    strKey, "0x10");

                    strInput = strInput.Replace("0x", "");
                    UInt32.TryParse(strInput,
                        NumberStyles.HexNumber, CultureInfo.CreateSpecificCulture("en-US"),
                        out nInputAddress);

                    //_ini.WriteString(MechatrolinkDefines.SECTION_STEPPING_AXIS_INFO, strKey, strInput);

                    // Output
                    strKey = String.Format("{0}_{1}", MechatrolinkDefines.KEY_STEPPING_OUTPUT_ADDRESS, nOffsetedIndex);
                    strOutput = _ini.GetString(MechatrolinkDefines.SECTION_STEPPING_AXIS_INFO,
                    strKey, "0x30");

                    strOutput = strOutput.Replace("0x", "");
                    UInt32.TryParse(strOutput,
                        NumberStyles.HexNumber, CultureInfo.CreateSpecificCulture("en-US"),
                        out nOutputAddress);

                    //nOutputAddress = Convert.ToInt32(strOutput, 16);


                    //UInt32 nGeneralInputAddress = 0, nGeneralOutputAddress = 0;

                    //// Input Address
                    //strKey = String.Format("{0}_{1}", MechatrolinkDefines.KEY_AXIS_GENERAL_INPUT_ADDRESS, nOffsetedIndex);
                    //strInput = _ini.GetString(MechatrolinkDefines.SECTION_STEPPING_AXIS_INFO,
                    //strKey, "0x00");

                    //strInput = strInput.Replace("0x", "");
                    //UInt32.TryParse(strInput,
                    //    NumberStyles.HexNumber, CultureInfo.CreateSpecificCulture("en-US"),
                    //    out nGeneralInputAddress);

                    //_ini.WriteString(MechatrolinkDefines.KEY_AXIS_GENERAL_INPUT_ADDRESS, strKey, strInput);

                    //// Output
                    //strKey = String.Format("{0}_{1}", MechatrolinkDefines.KEY_AXIS_GENERAL_OUTPUT_ADDRESS, nOffsetedIndex);
                    //strOutput = _ini.GetString(MechatrolinkDefines.SECTION_STEPPING_AXIS_INFO,
                    //strKey, "0x00");

                    //strOutput = strOutput.Replace("0x", "");
                    //UInt32.TryParse(strOutput,
                    //    NumberStyles.HexNumber, CultureInfo.CreateSpecificCulture("en-US"),
                    //    out nGeneralOutputAddress);

                    //_ini.WriteString(MechatrolinkDefines.KEY_AXIS_GENERAL_OUTPUT_ADDRESS, strKey, strInput);

                    //m_arAxis[i].GeneralInputAddress = nGeneralInputAddress;
                    //m_arAxis[i].GeneralOutputAddress = nGeneralOutputAddress;
                }

                m_arAxis[i].RegisterInputAddress = nInputAddress;
                m_arAxis[i].RegisterOutputAddress = nOutputAddress;

                m_arAxis[i].InitAxis();
            }
        }
        #endregion </Init/Exit/Gathering>

        #region <Checking Validity>
        private bool CheckingValidityAxis(int nAxis)
        {
            if (m_arAxis == null)
                return false;

            if (nAxis >= m_arAxis.Length || nAxis < 0)
                return false;

            if (m_arAxis[nAxis] == null)
                return false;

            return true;
        }

        public bool OpenApi(bool bReOpen = false)
        {
            rw.EnterWriteLock();

            if (false == bReOpen)
            {
                m_bConnected = false;

                _timeChecker.SetTickCount(2000);
            }

            UInt32 uCode;
            CMotionAPI.COM_DEVICE ComDevice = new CMotionAPI.COM_DEVICE();

            System.Console.WriteLine("Open Count : {0}", ++m_nOpeningCount);

            try
            {
                SetDevice(ref ComDevice);
                uCode = CMotionAPI.ymcOpenController(ref ComDevice, ref m_pHandleController);
                if (false == CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, -1, "After ymcOpenController"))
                    return false;

                uCode = CMotionAPI.ymcSetController(m_pHandleController);
                if (false == CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, -1, "After ymcSetController"))
                    return false;

                uCode = CMotionAPI.ymcSetAPITimeoutValue(30000);
                if (false == CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, -1, "After ymcSetAPITimeoutValue"))
                    return false;

                uCode = CMotionAPI.ymcClearAllAxes();
                if (false == CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, -1, "After ymcClearAllAxes"))
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : {0}, {1}", ex.Message, ex.StackTrace);

                rw.ExitWriteLock();

                return false;
            }

            if (false == bReOpen)
            {
                UpdateCountInfo();

                InitAxisInfo();

                m_bConnected = true;

                System.Console.WriteLine("OpenApi Elapsed Time : {0}", _timeChecker.GetTickCount());
            }

            rw.ExitWriteLock();

            return true;
        }

        public bool CheckingApi(UInt32 uCode, string strFuncName, int nAxis, string strMessage = "")
        {
            if (uCode != CMotionAPI.MP_SUCCESS)
            {
				// NOTE : Mechatrolink flux mask Y axis home 안되는 문제 확인용 임시 로그 저장
				if(nAxis != 11)
					return false;

                var trace = new System.Diagnostics.StackTrace();

                string strLog = String.Format("{0} - 0x{1:X}\n{2}\n{3}", strFuncName, uCode, strMessage, trace.ToString());

                if (nAxis > 0)
                {
                    strLog = String.Format("Target Axis {0} : {1}", (nAxis - 1).ToString(), strLog);
                }

                System.Console.WriteLine(strLog);
				Log.LogWriter.GetInstance().MakeLog("JHLIM\\FluxAxis\\CheckingApi", strLog);

                return false;
            }


            return true;
        }

        public void RequestEnableGantry(int[] arAxes)
        {
            SetGantry(arAxes);
        }

        public bool GetConnectionState()
        {
            return m_bConnected;
        }
        #endregion </Checking Validity>

        #region <Open & Close>
        private void SetDevice(ref CMotionAPI.COM_DEVICE dev)
        {
            dev.ComDeviceType = (UInt16)CMotionAPI.ApiDefs.COMDEVICETYPE_PCIe_MODE;//(UInt16)CMotionAPI.ApiDefs.COMDEVICETYPE_PCI_MODE;
            dev.PortNumber = 1;
            dev.CpuNumber = 1;    //(UInt16)spn_CpuNo_1.Value;	//cpuno;
            dev.NetworkNumber = 0;
            dev.StationNumber = 0;
            dev.UnitNumber = 0;
            dev.IPAddress = null;
            dev.Timeout = 10000;
        }

        private bool CloseApi()
        {
            for (int i = 0; i < m_nAxisCount; ++i)
            {
                if (null == m_arAxis[i])
                    continue;

                m_arAxis[i].Close();
            }

            UInt32 uCode = CMotionAPI.ymcClearAllAxes();
            CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, -1, "After ymcClearAllAxes");

            uCode = CMotionAPI.ymcCloseController(m_pHandleController);

            if (uCode == CMotionAPI.MP_SUCCESS)
            {
                m_bConnected = false;
            }

            return CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, -1, "After ymcCloseController");
        }

        private UInt32 DeclareAxis(int nAxisIndex, string strMyName, ref UInt32 pAxisHandle)
        {
            return CMotionAPI.ymcDeclareAxis(1, 0, 3,
                (UInt16)(nAxisIndex),
                (UInt16)(nAxisIndex),
                (UInt16)CMotionAPI.ApiDefs.REAL_AXIS,
                strMyName,
                ref pAxisHandle);
        }

        #endregion </Open & Close>

        #region <Checking Link Status>
        private bool IsLinkConnected()
        {
            UInt32 uData = 0;
            bool bResult = CpuStatusCheck(ref uData);

            //if (uData != m_uPrevState)
            //{
            //    System.Console.WriteLine("Cpu State Changed : {0}", uData);
            //    m_uPrevState = uData;
            //}

            if (false == bResult)
            {
                //return (ERR_CODE.ERR_MLINK_ABNORMAL);    //MP3100ステータス取得
                return false;
            }

            if ((uData & 0x01) == 0)
            {
                //return (ERR_CODE.ERR_MLINK_READY);   //MP3100 READY OFF
                return false;
            }
            if ((uData & 0x02) == 0)
            {
                //return (ERR_CODE.ERR_MLINK_CPU); //MP3100 CPU停止
                return false;
            }

            //if (mode == 1)
            {
                if ((uData & 0x04) != 0)
                {
                    //UInt32 code = 0;
                    //GetCpuAlarmCode(ref code);
                    //return (ERR_CODE.ERR_MLINK_ALARM);   //MP3100 アラーム発生
                    //return false;
                }
                if ((uData & 0x08) != 0)
                {
                    //UInt32 code = 0;
                    //GetCpuAlarmCode(ref code);
                    //return (ERR_CODE.ERR_MLINK_ERROR);   //MP3100 エラー発生
                    return false;
                }
            }
            return true;
        }

        private bool CpuStatusCheck(ref UInt32 dat)
        {
            UInt32 uCode;
            uCode = YmcRegisterRead("SL00040", ref dat);

            return CheckingApi(uCode, MethodBase.GetCurrentMethod().Name, -1, "After YmcRegisterRead");
        }
        #endregion </Checking Link Status>

        #region <Status>
        public void UpdateAxisStatus(int nAxis)
        {
            if (false == m_bConnected)
                return;

            if (false == CheckingValidityAxis(nAxis))
                return;

            m_arAxis[nAxis].UpdateAxisStatus(ref m_arActualPosition[nAxis], ref m_arCommandPosition[nAxis], ref m_arState[nAxis]);
        }
        #endregion </Status>

        private UInt32 YmcRegisterRead(string reg, ref UInt32 data)
        {
            if (m_bConnected == false)
            {
                return CMotionAPI.MP_FAIL;
            }

            UInt32 hRegisterData = 0;  // 레지스터 데이터 핸들
            UInt32 ReadDataNumber = 0; // 얻은 레지스터 수
            short[] shortData = new short[2];   // 레지스터 데이터 저장 변수
            UInt32 uCode;

            uCode = CMotionAPI.ymcGetRegisterDataHandle(reg, ref hRegisterData);
            if (uCode != CMotionAPI.MP_SUCCESS)
            {
                if (CMotionAPI.ERROR_CODE_COM_NOT_OPENED == uCode)
                {
                    m_bConnected = false;
                }

                return uCode;
            }

            uint num;
            switch (reg[1])
            {
                case 'L':
                    num = 2;
                    break;
                default:        //W,B
                    num = 1;
                    break;
            }

            //Read Register data
            uCode = CMotionAPI.ymcGetRegisterData(hRegisterData, num, shortData, ref ReadDataNumber);
            if (uCode != CMotionAPI.MP_SUCCESS)
            {
                return uCode;
            }

            data = 0;
            for (int i = 0; i < ReadDataNumber; i++)
            {
                data += (UInt32)((shortData[i] & 0xffff) << (i * 16));
            }
            return uCode;
        }


        //private bool MoveToGantryHome(int nAxis, ref int nSeq, ref int nDelay)
        //{
        //    if (false == m_bConnected)
        //        return false;

        //    bool bResult = false;
        //    int nTempSeq = nSeq;
        //    if (0 == nTempSeq)
        //    {
        //        bResult = m_arAxis[nAxis].MoveToHome(ref nSeq, ref nDelay);
        //        int nSlave = m_arAxis[nAxis].SlaveAxis;

        //        if (nSlave >= 0)
        //        {   
        //            int[] arAxes = { nAxis, nSlave };
        //            //m_arAxis[nAxis].SetGantry(arAxes, false);
        //            MCTL_PARAM_GANTRY stGantryParam = new MCTL_PARAM_GANTRY();
        //            stGantryParam.nAxisOfMaster = nAxis;
        //            stGantryParam.nAxisOfSlave = nSlave;
        //            stGantryParam.bEnableGantry = true;

        //            //SetGear(stGantryParam, null);
        //            SetGantry(stGantryParam, null);

        //            m_arAxis[nSlave].MotorOnOff(false);

        //            m_arAxis[nSlave].SetPosition(m_arAxis[nAxis].ActualPosition);

        //            nDelay += 1000;
        //        }

        //        return bResult;
        //    }

        //    bResult = m_arAxis[nAxis].MoveToHome(ref nSeq, ref nDelay);
        //    if (bResult)
        //    {
        //        int nSlave = m_arAxis[nAxis].SlaveAxis;

        //        if (nSlave >= 0)
        //        {
        //            m_arAxis[nSlave].MotorOnOff(true);

        //            while(true)
        //            {
        //                Thread.Sleep(1);
        //                if (m_arAxis[nSlave].ServoState)
        //                    break;
        //            }

        //            int[] arAxes = { nAxis, nSlave };

        //            //m_arAxis[nAxis].SetGantry(arAxes, true);
        //            //MCTL_PARAM_GANTRY stGantryParam = new MCTL_PARAM_GANTRY();
        //            //stGantryParam.nAxisOfMaster = nAxis;
        //            //stGantryParam.nAxisOfSlave = nSlave;
        //            //stGantryParam.bEnableGantry = true;

        //            //SetGantry(stGantryParam, null);

        //            m_arAxis[nSlave].SetPosition(m_arAxis[nAxis].ActualPosition);
        //        }
        //    }

        //    return bResult;
        //}

        private bool SetGearedMotion(int nMasterAxis)
        {
            //if (false == m_bConnected)
            //    return;

            //if (false == CheckingValidityAxis(stGantryParam.nAxisOfMaster))
            //    return;

            //if (false == CheckingValidityAxis(stGantryParam.nAxisOfSlave))
            //    return;

            MCTL_PARAM_GANTRY stGantryParam = m_arAxis[nMasterAxis].GetGantryParam();
            int nMaster = stGantryParam.nAxisOfMaster;
            int nSlave = stGantryParam.nAxisOfSlave;

            if (0 > nMaster || 0 > nSlave)
                return true;

            UInt32[] arAxes = { (UInt32)nMaster, (UInt32)nSlave };

            bool bState = stGantryParam.bEnableGantry;

            switch (m_nGearingSeq)
            {
                case 0:
                    // 모두 서보 온이면 건너뛰기
					//if (m_arAxis[nMaster].ServoState && m_arAxis[nSlave].ServoState)
					//{
					//	m_nGearingSeq += 3; 
					//}
					//else
                    {
                        ++m_nGearingSeq;
                    }
                    break;    
                case 1:
                    // 서보 온 전에 알람 클리어
                    m_arAxis[nMaster].DoReset();
                    m_arAxis[nSlave].DoReset();

					//_timeGantrySeq.Clear();
					//_timeGantrySeq.SetTickCount(200);

                    ++m_nGearingSeq; break;

                case 2:
                    // 서보 온
					//if (false == _timeGantrySeq.IsTickOver(true))
					//	break;

                    //if (false == m_arAxis[nMaster].ServoState)
                    //    m_arAxis[nMaster].MotorOnOff(true);

                    //if (false == m_arAxis[nSlave].ServoState)
                    //    m_arAxis[nSlave].MotorOnOff(true);

                    ++m_nGearingSeq; break;

                case 3:
                    // 서보 온 확인
					//if (false == m_arAxis[nMaster].ServoState ||
					//	false == m_arAxis[nSlave].ServoState)
					//	break;

					//_timeGantrySeq.Clear();
					//_timeGantrySeq.SetTickCount(200);

                    ++m_nGearingSeq; break;

                case 4:
					//if (false == _timeGantrySeq.IsTickOver(true))
					//	break;

                    // 갠트리 셋
                    UInt32[] arHandles = { m_arAxis[nMaster].AxisHandle, m_arAxis[nSlave].AxisHandle };
                    bool bEnabled = stGantryParam.bEnableGantry;

                    if (bEnabled)
                    {
                        m_arAxis[nMaster].MasterAxis = nMaster;
                        m_arAxis[nMaster].SlaveAxis = nSlave;

                        m_arAxis[nSlave].MasterAxis = nMaster;
                        m_arAxis[nSlave].SlaveAxis = nSlave;
                    }

                    m_arAxis[nMaster].SetGantryAxesIndex();
                    m_arAxis[nSlave].SetGantryAxesIndex();

                    m_arAxis[nMaster].SetGantry(ref arHandles, arAxes, bEnabled);

                    if (false == bEnabled)
                    {
                        m_arAxis[nMaster].MasterAxis = -1;
                        m_arAxis[nMaster].SlaveAxis = -1;

                        m_arAxis[nSlave].MasterAxis = -1;
                        m_arAxis[nSlave].SlaveAxis = -1;

                        //m_arAxis[stGantryParam.nAxisOfMaster].SetSlaveAxis(null);
                    }
                    else
                    {
                        m_arAxis[nMaster].SetSlaveAxis(m_arAxis[nSlave]);
                    }

                    ++m_nGearingSeq; break;
                case 5:
                    {
                        if (bState != m_arAxis[nMaster].GantryState ||
                            bState != m_arAxis[nSlave].GantryState)
                            break;
                        else
                            return true;
                    }
            }

            //Thread.Sleep(100);

            //m_arAxis[stGantryParam.nAxisOfMaster].SetGantry(ref arHandles, arAxes, false);
            //if (bEnabled)
            //{
            //    Thread.Sleep(100);

            //    m_arAxis[stGantryParam.nAxisOfMaster].SetGantry(ref arHandles, arAxes, bEnabled);
            //}

            return false;
        }
        #endregion </Internal Interfaces>

        #endregion </Methods>
    }
}