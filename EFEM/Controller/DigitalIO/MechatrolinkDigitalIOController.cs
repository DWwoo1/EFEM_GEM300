using System;
using System.Collections.Generic;
using System.Globalization;
using Define.DefineConstant;
using DigitalIO_;
using FileComposite_;
using FileIOManager_;
using FrameOfSystem3.Controller.Motion.MechatroLinkAPI;

namespace FrameOfSystem3.Controller.DigitalIO
{
    // 2021.08.16 by Thienvv [ADD]
      public enum EN_DO_LIST
      {
          MOTOR_SERVO_SW = 49,
          BREAK_RELEASE = 50,
      }

    class MechatrolinkDigitalIOController : DigitalIOController
    {
        #region <Fields>
        private uint[] arrOutput;       //2020.08.26 nogami add
        private int m_nCountOfModule;
        private MechatrolinkController common;

        private Dictionary<int, PORT_SETTING> m_dictOfDigital_ioPort = new Dictionary<int, PORT_SETTING>();
        #endregion </Fields>

        #region <Configuration Files>

        private bool SaveDigitalIOSetting()
        {
            FileComposite fComp = FileComposite.GetInstance();
            FileIOManager fIO = FileIOManager.GetInstance();

            string strRootData = "DIGITALIO_MECHATROLINK";

            m_dictOfDigital_ioPort = new Dictionary<int, PORT_SETTING>();
            for (int i = 0; i < 4; i ++ )
            {
                m_dictOfDigital_ioPort.Add(i, new PORT_SETTING());
            }

            fComp.CreateRoot(strRootData);

            for (int i = 0; i < m_dictOfDigital_ioPort.Count; ++i)
            {
                string strSubRoot = i.ToString();
                fComp.AddGroup(strRootData, strSubRoot);

                fComp.AddItem<string>(strRootData, "UNIT", i.ToString(), strSubRoot);
                fComp.AddItem<string>(strRootData, "WRITE_IO_COMMAND_ADRS", string.Format("0x{0:X4}", m_dictOfDigital_ioPort[i].write_io_command_adrs), strSubRoot);
                fComp.AddItem<string>(strRootData, "WRITE_COMMAND_CONTROL_ADRS", string.Format("0x{0:X4}", m_dictOfDigital_ioPort[i].write_command_ctrl_adrs), strSubRoot);
                fComp.AddItem<string>(strRootData, "READ_STATUS_ADRS", string.Format("0x{0:X4}", m_dictOfDigital_ioPort[i].read_status_adrs), strSubRoot);
                fComp.AddItem<string>(strRootData, "READ_COMMAND_STATUS_ADRS", string.Format("0x{0:X4}", m_dictOfDigital_ioPort[i].read_command_status_adrs), strSubRoot);
                fComp.AddItem<string>(strRootData, "IN_ADRS", string.Format("0x{0:X4}", m_dictOfDigital_ioPort[i].in_adrs), strSubRoot);
                fComp.AddItem<string>(strRootData, "OUT_ADRS", string.Format("0x{0:X4}", m_dictOfDigital_ioPort[i].out_adrs), strSubRoot);
            }

            string strData = string.Empty;
            if (!fComp.GetStructureAsString(strRootData, ref strData))
                return false;

            fComp.RemoveRoot(strRootData);
            string sFileName = strRootData + FileFormat.FILEFORMAT_CONFIG;
            return fIO.Write(FilePath.FILEPATH_CONFIG, sFileName, strData, false, true);
        }

        private bool LoadDigitalIOSetting()
        {
            FileComposite fComp = FileComposite.GetInstance();
            FileIOManager fIO = FileIOManager.GetInstance();

            string strRootData = "DIGITALIO_MECHATROLINK";
            string sFilePath = FilePath.FILEPATH_CONFIG;
            string sFileName = strRootData + FileFormat.FILEFORMAT_CONFIG;
            string sFileData = string.Empty;
           
            string sFullFilePath = sFilePath + "\\" + sFileName;

            if (!System.IO.File.Exists(sFullFilePath))
            {
                return false;
            }

            if (!fIO.Read(sFilePath, sFileName, ref sFileData, FileIO.TIMEOUT_READ))
            {
                //SaveDigitalIOSetting(); // 2021.06.28 by Thienvv [ADD]
                Views.Functional.Form_MessageBox.GetInstance().ShowMessage("Check");
                return false;
            }

            string[] arData = sFileData.Split('\n');
            if (!fComp.CreateRootByString(ref arData))
                return false;

            string[] arGroup = null;
            fComp.GetListOfGroup(strRootData, ref arGroup);

            m_dictOfDigital_ioPort.Clear();
            NumberStyles numberStyle = NumberStyles.HexNumber;
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            for (int i = 0; i < arGroup.Length; i++)
            {
                PORT_SETTING work = new PORT_SETTING();
                string strTemp = string.Empty;

                string[] strGroups = { i.ToString() };
                if (!fComp.AddGroup(strRootData, strGroups))
                    return false;

                fComp.GetValue(strRootData, "UNIT", ref strTemp, strGroups);
                int nUnit = i;
                if (false == Int32.TryParse(strTemp, out nUnit))
                {
                    return false;
                }

                fComp.GetValue(strRootData, "READ_STATUS_ADRS", ref strTemp, strGroups);
                string write_io_command_adrs = strTemp.Replace("0x", "");
                if (!UInt32.TryParse(write_io_command_adrs, numberStyle, cultureInfo, out work.write_io_command_adrs))
                {
                    return false;
                }

                fComp.GetValue(strRootData, "WRITE_COMMAND_CONTROL_ADRS", ref strTemp, strGroups);
                string write_command_ctrl_adrs = strTemp.Replace("0x", "");
                if (!UInt32.TryParse(write_io_command_adrs, numberStyle, cultureInfo, out work.write_command_ctrl_adrs))
                {
                    return false;
                }

                fComp.GetValue(strRootData, "READ_STATUS_ADRS", ref strTemp, strGroups);
                string read_status_adrs = strTemp.Replace("0x", "");
                if (!UInt32.TryParse(read_status_adrs, numberStyle, cultureInfo, out work.read_status_adrs))
                {
                    return false;
                }

                fComp.GetValue(strRootData, "READ_COMMAND_STATUS_ADRS", ref strTemp, strGroups);
                string read_command_status_adrs = strTemp.Replace("0x", "");
                if (!UInt32.TryParse(read_command_status_adrs, numberStyle, cultureInfo, out work.read_command_status_adrs))
                {
                    return false;
                }

                fComp.GetValue(strRootData, "IN_ADRS", ref strTemp, strGroups);
                string in_adrs = strTemp.Replace("0x", "");
                if (!UInt32.TryParse(in_adrs, numberStyle, cultureInfo, out work.in_adrs))
                {
                    return false;
                }

                fComp.GetValue(strRootData, "OUT_ADRS", ref strTemp, strGroups);
                string out_adrs = strTemp.Replace("0x", "");
                if (!UInt32.TryParse(out_adrs, numberStyle, cultureInfo, out work.out_adrs))
                {
                    return false;
                }
                
                m_dictOfDigital_ioPort.Add(nUnit, work);
            }
            fComp.RemoveRoot(strRootData); // 2021.07.05 bby Thienvv [ADD] => test
            return true;
        }
        #endregion </Configuration Files>

        #region Controller
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Initialize the controller.
        /// </summary>
        public override bool InitController()
        {
            common = new MechatrolinkController();
            bool bInit = common.ApiOpen();

            LoadDigitalIOSetting();
            for (int i = 0; i < m_dictOfDigital_ioPort.Count; i++)
            {
                m_nCountOfModule++;
            }
            arrOutput = new uint[m_nCountOfModule];     //2020.08.26 nogami add

            // 2021.07.05 by Thienvv [ADD]
            int m_nCountOfInputModule = m_nCountOfModule;
            int m_nCountOfOutputModule = m_nCountOfModule;
            if (bInit)
            {
                // 연결된 모듈 개수를 얻어온다.
                m_nCountOfInputModule = GetCountOfInputModule();
                m_nCountOfOutputModule = GetCountOfOutputModule();
            }

            int[] m_arIndexOfStartingInput = new int[m_nCountOfInputModule + 1];
            int[] m_arIndexOfStartingOutput = new int[m_nCountOfOutputModule + 1];

            m_arIndexOfStartingInput[0] = 0;
            m_arIndexOfStartingOutput[0] = 0;

            // 연결된 모든 채널 개수를 얻어온다.
            int m_nCountOfInputChannel = 0;
            for (int i = 0; i < m_nCountOfInputModule; ++i)
            {
                m_nCountOfInputChannel += GetCountOfInputChannel(ref i);
                m_arIndexOfStartingInput[i + 1] = m_nCountOfInputChannel;
            }

            // 연결된 모든 채널 개수를 얻어온다.
            int m_nCountOfOutputChannel = 0;
            for (int i = 0; i < m_nCountOfOutputModule; ++i)
            {
                m_nCountOfOutputChannel += GetCountOfOutputChannel(ref i);
                m_arIndexOfStartingOutput[i + 1] = m_nCountOfOutputChannel;
            }

            UInt32[] m_arReadInputData = new UInt32[m_nCountOfInputModule];
            UInt32[] m_arReadOutputData = new UInt32[m_nCountOfOutputModule];

            //// 아이템 추가
            //for (int i = 0; i < m_nCountOfInputChannel; ++i)
            //{
            //    m_dioItemManager.AddInputItem();
            //}
            //for (int i = 0; i < m_nCountOfOutputChannel; ++i)
            //{
            //    m_dioItemManager.AddOutputItem();
            //}

            //bool bItemLoaded = m_dioItemManager.Load();

            // 2019.04.02 by yjlee [ADD] 파일 이름 한 번에 읽기
            //m_dioItemManager.LoadAsName();

            //m_arInputData = new bool[m_nCountOfInputChannel];
            //m_arOutputData = new bool[m_nCountOfOutputChannel];


            return bInit;   // true;
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Exit the controller.
        /// </summary>
        public override void ExitController()
        {
            common.ApiClose();
            arrOutput = null;       //2020.08.26 nogami add
            return;
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the count of the input module.
        /// </summary>
        public override int GetCountOfInputModule()
        {
            return m_nCountOfModule;
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the count of the output module.
        /// </summary>
        public override int GetCountOfOutputModule()
        {
            return m_nCountOfModule;
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the total count of the input channel.
        /// </summary>
        public override int GetCountOfInputChannel(ref int nInputModule)
        {
            return 32;
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Get the total count of the output channel.
        /// </summary>
        public override int GetCountOfOutputChannel(ref int nOutputMoudle)
        {
            return 32;
        }
        #endregion

        #region Digital IO Interface
        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Read the input data from the controller.
        /// </summary>
        public override uint ReadInputAll(ref int nInputModule, ref int nCountOfChannel)
        {
            PORT_SETTING port = m_dictOfDigital_ioPort[nInputModule];
            string reg = string.Format("IL{0:X4}", port.in_adrs);
            uint nReadInputData = 0;
            UInt32 rc = common.YmcRegisterRead(reg, ref nReadInputData);

            return (UInt32)nReadInputData;
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Read the output data from the controller.
        /// </summary>
        public override uint ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel)
        {
            PORT_SETTING port = m_dictOfDigital_ioPort[nOutputModule];
            string reg = string.Format("OL{0:X4}", port.out_adrs);
            uint nReadInputData = 0;
            UInt32 rc = common.YmcRegisterRead(reg, ref nReadInputData);

            //2020.08.26 nogami add start
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //Console.WriteLine(
                //    string.Format("{0}() YmcRegisterRead({1}), rc=0x{2:X}", MethodBase.GetCurrentMethod().Name,
                //        reg,
                //        rc
                //    )
                //);
                if (arrOutput != null)
                {
                    return arrOutput[nOutputModule];
                }
            }
            //2020.08.26 nogami add end

            return (UInt32)(nReadInputData);
        }

        /// <summary>
        /// 2020.11.09 by yjlee [ADD] Write the data to the controller.
        /// </summary>
        public override void WriteOutput(ref int nOutputChannel, ref bool bPulse)
        {
            short moduleNum;
            uint outputValue;

            moduleNum = (short)(nOutputChannel / 32);
            outputValue = (uint)Math.Pow(2, (nOutputChannel % 32));

            //2020.08.26 nogami add start
            if (bPulse)
            {
                arrOutput[moduleNum] = arrOutput[moduleNum] | (outputValue);
                //arrOutput[moduleNum] = arrOutput[moduleNum] | outputValue;
            }
            else
            {
                arrOutput[moduleNum] = arrOutput[moduleNum] & ~(outputValue);
            }
            //2020.08.26 nogami add end

            PORT_SETTING port = m_dictOfDigital_ioPort[moduleNum];
            uint adrs = (uint)port.out_adrs;
            if (((nOutputChannel / 16) % 2) > 0)
            {
                adrs++;
            }
            uint bit = (uint)(nOutputChannel % 16);

            string reg = string.Format("OB{0:X4}{1:X}", adrs, bit);
            UInt32 rc = common.YmcRegisterWrite(reg, (bPulse) ? (1) : (0));
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //Console.WriteLine(
                //    string.Format("{0}() YmcRegisterWrite({1},{2}), rc=0x{3:X}", MethodBase.GetCurrentMethod().Name,
                //        reg, (bPulse) ? (1) : (0),
                //        rc
                //    )
                //);

                if (CMotionAPI.ERROR_CODE_COM_NOT_OPENED == rc)
                {
                    common.ApiOpen();

                    // 2021.07.11 by Thienvv [ADD] ReWrite after open
                    System.Threading.Thread.Sleep(10);
                    rc = common.YmcRegisterWrite(reg, (bPulse) ? (1) : (0));
                    if (rc != CMotionAPI.MP_SUCCESS)
                    {
                        //Console.WriteLine(
                        //    string.Format("{0}() YmcRegisterWrite({1},{2}), rc=0x{3:X}", MethodBase.GetCurrentMethod().Name,
                        //        reg, (bPulse) ? (1) : (0),
                        //        rc
                        //    )
                        //);
                    }
                }
                return;
            }
            return;
        }
        #endregion

        #region <Inner Class, Struct>
        private struct PORT_SETTING
        {
            public UInt32 read_status_adrs;
            public UInt32 read_command_status_adrs;
            public UInt32 write_io_command_adrs;
            public UInt32 write_command_ctrl_adrs;
            public UInt32 in_adrs;
            public UInt32 out_adrs;
        }
        #endregion </Inner Class, Struct>
    }
}
