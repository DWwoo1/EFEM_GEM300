using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using Define.DefineConstant;
using FileComposite_;
using FileIOManager_;

namespace FrameOfSystem3.Work
{
    /// <summary>
    /// 자재가 놓이는 곳의 상태 (예: Conveyor, Stage, Tool, Table, Buffer, etc..)
    /// </summary>
    public enum EN_PORT_STATUS
    {
        EMPTY,          // 자재가 없다
        EXIST_WAITING,  // 작업이 대기 중이다
        EXIST_WORKING,  // 작업이 진행 중이다
        EXIST_FINISHED  // 작업이 완료 되었다

        // INS-900 예시
//         EMPTY,                      // 자재가 없다
//         EXIST_WAITING_ALIGN,        // ALIGN 대기
//         EXIST_WAITING_ID_REDING,    // 바코드 읽기 대기
//         EXIST_WORKING,              // 작업이 진행 중이다
//         EXIST_FINISHED              // 작업이 완료 되었다
    }
    /// <summary>
    /// 작업 환경을 실시간 저장하여 재 실행 시 복원하기 위한 데이터
    /// </summary>
    public abstract class RecoveryData
    {
        #region 변수
        protected string m_sTaskName = "UNKNOW";
        protected bool m_bPossible = true;      // 시스템 설정 (예: Tool 사용회수 초과 시 false)
        protected string m_sMapInfoName = string.Empty;
        protected string m_sMapInfoPath = string.Empty;

        private bool m_bUnSaveFlag = false;

        // key : Port Number, Value : Port Status 
        protected ConcurrentDictionary<int, EN_PORT_STATUS> m_dicPortStatus = new ConcurrentDictionary<int, EN_PORT_STATUS>();
        #endregion

        #region 속성
        public string TaskName { get { return m_sTaskName; } }
        public bool Possible
        {
            get { return m_bPossible; }
            set
            {
                if (m_bPossible != value)
                {
                    m_bPossible = value;
                    Write();
                }
            }
        }
        public string MapInfoName
        {
            get { return m_sMapInfoName; }
            set
            {
                if (m_sMapInfoName != value)
                {
                    m_sMapInfoName = value;
                    Write();
                }
            }
        }
        public string MapInfoPath
        {
            get { return m_sMapInfoPath; }
            set
            {
                if (m_sMapInfoPath != value)
                {
                    m_sMapInfoPath = value;
                    Write();
                }
            }
        }
        public bool UnSaveFlag { get { return m_bUnSaveFlag; } set { m_bUnSaveFlag = value; } }

        // Get/Set Method
        public bool GetPortStatus(int nPortNo, out EN_PORT_STATUS ePortStatus)
        {
            return m_dicPortStatus.TryGetValue(nPortNo, out ePortStatus);
        }
        public bool SetPortStatus(int nPortNo, EN_PORT_STATUS ePortStatus)
        {
            EN_PORT_STATUS eStatus;

            bool bContainKey = m_dicPortStatus.TryGetValue(nPortNo, out eStatus);

            if (bContainKey)
            {
                if (ePortStatus != eStatus)     // 신규 값 != 기존 값
                {
                    m_dicPortStatus[nPortNo] = ePortStatus;
                    Write();
                }
            }
            else
                return false;

            return true;
        }
        #endregion

        #region 생성자
        public RecoveryData(string taskName, int nPortCount)
        {
            m_sTaskName = taskName;

            for (int i = 1; i < nPortCount + 1; ++i)
            {
                m_dicPortStatus.TryAdd(i, EN_PORT_STATUS.EMPTY);
            }

            LoadBaseData();
        }
        #endregion

        #region Constants
        private const string POSSIBLE = "Possible";
        private const string MAP_INFO_NAME = "MapInfoName";
        private const string MAP_INFO_PATH = "MapInfoPath";
        private const string PORT_STATUS = "PortStatus_";
        #endregion

        #region 파일 입출력
        protected bool SaveBaseData()
        {
            FileComposite fComp = FileComposite.GetInstance();

			// ROOT GROUP NAME 은 FileComposite 을 사용하는 다른 파일과 중복되면 안됨
			// 2023.05.12 by junho [MOD] Recovery data save 이상하게 저장되는 현상 개선
			fComp.RemoveRoot(m_sTaskName);
			if (fComp.CreateRoot(m_sTaskName).Equals(false))
				return false;
			//fComp.CreateRoot(m_sTaskName);

            fComp.AddItem(m_sTaskName, POSSIBLE, m_bPossible);
            fComp.AddItem(m_sTaskName, MAP_INFO_NAME, m_sMapInfoName);
            fComp.AddItem(m_sTaskName, MAP_INFO_PATH, m_sMapInfoPath);

            foreach (KeyValuePair<int, EN_PORT_STATUS> kvp in m_dicPortStatus)
            {
                fComp.AddItem(m_sTaskName, PORT_STATUS + kvp.Key, kvp.Value.ToString());
            }

            // Task 별 Recovery Data
            SaveData(ref fComp, m_sTaskName);

            FileIOManager fIO = FileIOManager.GetInstance();

            string sFilePath = FilePath.FILEPATH_RECOVERY;
            string sFileName = m_sTaskName + FileFormat.FILEFORMAT_RECOVERY;	// 2020.12.08 by jhchoo [MOD] RecoveryData > m_sTaskName
            string sFileData = string.Empty;

            if (!fComp.GetStructureAsString(m_sTaskName, ref sFileData))
                return false;

            return fIO.Write(sFilePath, sFileName, sFileData, false, false);
        }
        protected void LoadBaseData()
        {
            string sFullFilePath = FilePath.FILEPATH_RECOVERY + "\\" + m_sTaskName + FileFormat.FILEFORMAT_RECOVERY;

            if (!System.IO.File.Exists(sFullFilePath))
                return;

            FileIOManager fIO = FileIOManager.GetInstance();

            string sFilePath = FilePath.FILEPATH_RECOVERY;
            string sFileName = m_sTaskName + FileFormat.FILEFORMAT_RECOVERY;
            string sFileData = string.Empty;

            if (!fIO.Read(sFilePath, sFileName, ref sFileData, FileIO.TIMEOUT_READ))
                return;

            FileComposite fComp = FileComposite.GetInstance();

            string[] arData = sFileData.Split('\n');

            if (!fComp.CreateRootByString(ref arData))
                return;

            fComp.GetValue(m_sTaskName, POSSIBLE, ref m_bPossible);
            fComp.GetValue(m_sTaskName, MAP_INFO_NAME, ref m_sMapInfoName);
            fComp.GetValue(m_sTaskName, MAP_INFO_PATH, ref m_sMapInfoPath);

            foreach (KeyValuePair<int, EN_PORT_STATUS> kvp in m_dicPortStatus)
            {
                string sValue = string.Empty;
                fComp.GetValue(m_sTaskName, PORT_STATUS + kvp.Key, ref sValue);
                if (String.IsNullOrEmpty(sValue))
                    sValue = EN_PORT_STATUS.EMPTY.ToString();

                m_dicPortStatus[kvp.Key] = (EN_PORT_STATUS)Enum.Parse(typeof(EN_PORT_STATUS), sValue);
            }

            // Task 별 Recovery Data
            LoadData(ref fComp, m_sTaskName);
        }
        #endregion

        #region 내부 인터페이스
        protected void Write()
        {
            if (m_bUnSaveFlag)
                return;

            SaveBaseData();
        }
        #endregion

        #region 외부 인터페이스
        public void UnSave()
        {
            m_bUnSaveFlag = true;
        }
        public void Save()
        {
            m_bUnSaveFlag = false;
            Write();
        }
        // 자재가 있다면 Map Data 를 가지고 Unit 이 생성되었으므로 복구 필요
        public bool IsRecoveryStatus()
        {
            foreach (KeyValuePair<int, EN_PORT_STATUS> kvp in m_dicPortStatus)
            {
                switch (kvp.Value)
                {
                    case EN_PORT_STATUS.EXIST_WAITING:
                    case EN_PORT_STATUS.EXIST_WORKING:
                    case EN_PORT_STATUS.EXIST_FINISHED:
                        return true;
                }
            }

            return false;
        }
        #endregion

        #region 가상 함수
        // Write Recovery Data
        protected abstract void SaveData(ref FileComposite fComp, string sRootName);
        // Read Recovery Data
        protected abstract void LoadData(ref FileComposite fComp, string sRootName);
        #endregion
    }
}
