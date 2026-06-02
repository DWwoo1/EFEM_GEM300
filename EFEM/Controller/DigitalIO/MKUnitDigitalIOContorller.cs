using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Xml;
using System.Xml.Serialization;

using DigitalIO_;
using MKUNIT;

namespace FrameOfSystem3.Controller.DigitalIO
{
    class MKUnitDigitalIOContorller : DigitalIOController
    {
        private const int COUNT_MODULE = 6;
        private const int COUNT_CHANNEL = 32;
        private int m_nCountOfModule;
        
        private uint[] arrOutput;

        private MKUnitOutputDataWritter writter = new MKUnitOutputDataWritter();

        #region DIO 입출력 관련 추상 함수들

        // 초기화 관련 함수들
        #region 생성자, 초기화 관련 함수
        
        /// <summary>
        /// 2019.06.26. by shkim. [ADD] 모듈 초기화
        /// </summary>
        /// <returns></returns>
        public override bool InitController()
        {
            if (MkUnitw.Usio32wDllOpen() == 1)
            {
                for (short i = 0; i < COUNT_MODULE; i++)
                {
                    // MK-UNIT은 0~5의 범위에서 설정한다.
                    if(MkUnitw.Usio32wCreate(ref i) == 1)
                    {
                        m_nCountOfModule++;
                    }
                }
                
                arrOutput = new uint[m_nCountOfModule];

                // ?NOTE : 여기서 Exception 발생하는데... 
                if (writter.ReadData(ref arrOutput))
                {
                    for (short i = 0; i < m_nCountOfModule; ++i)
                    {
                        MkUnitw.Usio32wOutPortDW(i, arrOutput[i]);
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 2019.06.26. by shkim. [ADD] 모듈 종료
        /// </summary>
        /// <returns></returns>
        public override void ExitController()
        {
            writter.WriteData(ref arrOutput);

            for (short i = 0; i < m_nCountOfModule; i++ )
            {
                MkUnitw.Usio32wClose(i);
            }

            MkUnitw.Usio32wDllClose();
            arrOutput = null;
            return;
        }

        #region 상태반환 함수
        /// <summary>
        /// 2019.06.26. by shkim. [ADD] 현재 연결된 모듈의 개수를 반환
        /// </summary>
        /// <returns></returns>
        public override int GetCountOfInputModule()
        {
            return m_nCountOfModule;
        }
        public override int GetCountOfOutputModule()
        {
            return m_nCountOfModule;
        }
        /// <summary>
        /// 2019.06.26. by shkim. [ADD] 현재 연결된 Input 채널 수를 반환
        /// </summary>
        /// <param name="nModule"></param>
        /// <returns></returns>
        public override int GetCountOfInputChannel(ref int nInputModule)
        {
            return COUNT_CHANNEL;
        }
        /// <summary>
        /// 2019.06.26. by shkim. [ADD] 현재 연결된 Output 채널 수를 반환
        /// </summary>
        /// <param name="nModule"></param>
        /// <returns></returns>
        public override int GetCountOfOutputChannel(ref int nOutputMoudle)
        {
            return COUNT_CHANNEL;
        }
        #endregion
        #endregion

        // 포트 신호 쓰기
        #region 포트 신호 쓰기
        /// <summary>
        /// 2019.06.26. by shkim. [ADD] Output 채널 Index에서 Pulse 쓰기
        /// </summary>
        /// <param name="nIndex">Target Index</param>
        /// <param name="bPulse">Pulse</param>
        public override void WriteOutput(ref int nOutputChannel, ref bool bPulse)
        {
            short moduleNum;
            uint outputValue;

            moduleNum = (short)(nOutputChannel / COUNT_CHANNEL);
            outputValue = (uint)Math.Pow(2, (nOutputChannel % COUNT_CHANNEL));

            if(bPulse)
            {
                arrOutput[moduleNum] = arrOutput[moduleNum] | (outputValue);
                //arrOutput[moduleNum] = arrOutput[moduleNum] | outputValue;
            }
            else
            {
                arrOutput[moduleNum] = arrOutput[moduleNum] & ~(outputValue);
            }

            MkUnitw.Usio32wOutPortDW(moduleNum, arrOutput[moduleNum]);
        }
        #endregion

        // 포트 신호 읽기
        #region 포트 신호 읽기
        /// <summary>
        /// 2019.06.26. by shkim. [ADD] 해당 모듈 Input의 모든 채널 값 읽기
        /// </summary>
        /// <param name="nModule">모듈 인덱스 번호</param>
        /// <returns></returns>
        public override uint ReadInputAll(ref int nInputModule, ref int nCountOfChannel)
        {
            int nResult;
            uint nReadInputData = 0;
            nResult = MkUnitw.Usio32wInPortDW((short)nInputModule, ref nReadInputData);

            return nReadInputData;
        }
        /// <summary>
        /// 2019.06.26. by shkim. [ADD] 해당 모듈 Output의 모든 채널 값 읽기
        /// </summary>
        /// <param name="nModule">모듈 인덱스 번호</param>
        /// <returns></returns>
        public override uint ReadOutputAll(ref int nOutputModule, ref int nCountOfChannel)
        {
            return arrOutput[nOutputModule];
        }
        #endregion


        #endregion
    }

    class MKUnitOutputDataWritter
    {
        #region <Fields>
        private DigitalOutputData m_DigitalOutputData = new DigitalOutputData();
        #endregion </Fields>

        #region <Methods>
        public bool ReadData(ref uint[] arOutput)
        {
            string strDirPath = String.Format(@"{0}\MKUnit", System.Environment.CurrentDirectory);
            if (false == Directory.Exists(strDirPath))
            {
                Directory.CreateDirectory(strDirPath);
            }

            string strFilePath = String.Format(@"{0}\Output.xml", strDirPath);

            if (false == File.Exists(strFilePath))
            {
                return false;
            }

            FileStream fs = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);

            try
            {
                System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(typeof(DigitalOutputData));
                m_DigitalOutputData = xml.Deserialize(fs) as DigitalOutputData;

                fs.Close();

                Array.Copy(m_DigitalOutputData.m_arOutput, arOutput, arOutput.Length);

                return true;
            }
            catch (Exception ex)
            {
                if (fs != null) fs.Close();

                File.Delete(strFilePath);

                System.Console.WriteLine("Digital IO File Exception : {0} - {1}", ex.Message, ex.StackTrace);

                return false;
            }
        }

        public bool WriteData(ref uint[] arOutput)
        {
            Array.Copy(arOutput, m_DigitalOutputData.m_arOutput, arOutput.Length);

            string strDirPath = String.Format(@"{0}\MKUnit", System.Environment.CurrentDirectory);
            if (false == Directory.Exists(strDirPath))
                Directory.CreateDirectory(strDirPath);

            string strFilePath = String.Format(@"{0}\Output.xml", strDirPath);

            FileStream fs = new FileStream(strFilePath, FileMode.Create, FileAccess.Write);

            try
            {
                System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(typeof(DigitalOutputData));
                xml.Serialize(fs, m_DigitalOutputData);

                fs.Close();

                return true;
            }
            catch (Exception ex)
            {
                if (fs != null) fs.Close();

                System.Console.WriteLine("Digital IO File Exception : {0} - {1}", ex.Message, ex.StackTrace);

                return false;
            }
        }
        #endregion </Methods>
    }

    [Serializable]
    public class DigitalOutputData
    {
        public uint[] m_arOutput = new uint[10];
    }
}
