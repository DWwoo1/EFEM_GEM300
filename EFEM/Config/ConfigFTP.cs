using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;

using FileComposite_;
using FileIOManager_;
using Define.DefineConstant;

namespace FrameOfSystem3.Config
{
    public class ConfigFTP
    {
        public enum EN_CONFIG_ITEM
        {
            NAME,
            ADDRESS,
            ID,
            PASSWORD,
            PORT,
            PATH,
        }

        public class FTP_Server
        {
            public FTP_Server()
            {
                Name = "";
                Address = "";
                ID = "";
                Password = "";
                Port = 21;
                Path = "";
            }
            public string Name { get; set; }
            public string Address { get; set; }
            public string ID { get; set; }
            public string Password { get; set; }
            public int Port { get; set; }
            public string Path { get; set; }
        }

        #region 싱글톤
        private ConfigFTP()
        {
            m_dicOfServer = new Dictionary<int, FTP_Server>();
        }

        private static readonly Lazy<ConfigFTP> lazyInstance = new Lazy<ConfigFTP>(() => new ConfigFTP());
        static public ConfigFTP GetInstance()
        {
            return lazyInstance.Value;
        }
        #endregion

        #region Field
        Dictionary<int, FTP_Server> m_dicOfServer;

        string[] m_arServerFileLIst = new string[] { };//우선 하나만 하자. 추후에 서버별로 관리해야 하면 추가
        #endregion

        private readonly string strFTPName = "FTP";

        public string[] arServerFileLIst
        {
            get
            {
                return m_arServerFileLIst;
            }
        }

        #region Method
        private void Save()
        {
            FileComposite fComp = FileComposite.GetInstance();

            fComp.RemoveRoot(strFTPName);
            if (!fComp.CreateRoot(strFTPName))
                return;

            foreach (var Server in m_dicOfServer)
            {
                fComp.AddGroup(strFTPName, Server.Key.ToString());
                string[] arGroup = new string[] { Server.Key.ToString() };
                fComp.AddItem(strFTPName, EN_CONFIG_ITEM.NAME.ToString(), Server.Value.Name, arGroup);
                fComp.AddItem(strFTPName, EN_CONFIG_ITEM.ADDRESS.ToString(), Server.Value.Address, arGroup);
                fComp.AddItem(strFTPName, EN_CONFIG_ITEM.ID.ToString(), Server.Value.ID, arGroup);
                fComp.AddItem(strFTPName, EN_CONFIG_ITEM.PASSWORD.ToString(), Server.Value.Password, arGroup);
                fComp.AddItem(strFTPName, EN_CONFIG_ITEM.PORT.ToString(), Server.Value.Port, arGroup);
                fComp.AddItem(strFTPName, EN_CONFIG_ITEM.PATH.ToString(), Server.Value.Path, arGroup);
            }

            FileIOManager fIO = FileIOManager.GetInstance();

            string sFilePath = FilePath.FILEPATH_CONFIG;
            string sFileName = strFTPName + FileFormat.FILEFORMAT_CONFIG;
            string sFileData = string.Empty;

            if (!fComp.GetStructureAsString(strFTPName, ref sFileData))
                return;

            fComp.RemoveRoot(strFTPName);

            if (!fIO.Write(sFilePath, sFileName, sFileData, false, false))
                return;
        }
        private void Load()
        {
            m_dicOfServer.Clear();
            string sFilePath = FilePath.FILEPATH_CONFIG;
            string sFileName = strFTPName + FileFormat.FILEFORMAT_CONFIG;
            string sFileData = string.Empty;

            if (!System.IO.File.Exists(Path.Combine(sFilePath, sFileName)))
                return;

            FileIOManager fIO = FileIOManager.GetInstance();


            if (!fIO.Read(sFilePath, sFileName, ref sFileData, FileIO.TIMEOUT_READ))
                return;

            FileComposite fComp = FileComposite.GetInstance();

            string[] arData = sFileData.Split('\n');
            fComp.RemoveRoot(strFTPName);

            if (!fComp.CreateRootByString(ref arData))
                return;

            string[] arGroupList = null;
            fComp.GetListOfGroup(strFTPName, ref arGroupList);
            foreach (string sGroup in arGroupList)
            {
                int nIndex = 0;
                if (int.TryParse(sGroup, out nIndex))
                {
                    string[] arGroupLevel = new string[] { sGroup };

                    FTP_Server server = new FTP_Server();

                    string strName = "";
                    fComp.GetValue(strFTPName, EN_CONFIG_ITEM.NAME.ToString(), ref strName, arGroupLevel);
                    server.Name = strName;
                    
                    string strAddress = "";
                    fComp.GetValue(strFTPName, EN_CONFIG_ITEM.ADDRESS.ToString(), ref strAddress, arGroupLevel);
                    server.Address = strAddress;

                    string strID = "";
                    fComp.GetValue(strFTPName, EN_CONFIG_ITEM.ID.ToString(), ref strID, arGroupLevel);
                    server.ID = strID;
                   
                    string strPassword = "";
                    fComp.GetValue(strFTPName, EN_CONFIG_ITEM.PASSWORD.ToString(), ref strPassword, arGroupLevel);
                    server.Password = strPassword;

                    int nPort = 21;
                    fComp.GetValue(strFTPName, EN_CONFIG_ITEM.PORT.ToString(), ref nPort, arGroupLevel);
                    server.Port = nPort;

                    string strPath = "";
                    fComp.GetValue(strFTPName, EN_CONFIG_ITEM.PATH.ToString(), ref strPath, arGroupLevel);
                    server.Path = strPath;

                    m_dicOfServer.Add(nIndex, server);
                }
            }
        }
        #endregion

        #region Interface
        public bool Init()
        {
            Load();
            return true;
        }
        public bool Upload(int nIndex, string strSourceFilePath, string strFileName)
        {
            if (m_dicOfServer.ContainsKey(nIndex) == false)
            {
                return false;
            }

            int nPort = m_dicOfServer[nIndex].Port;
            string strSourceFile = System.IO.Path.Combine(strSourceFilePath, strFileName);

            string strTargetFile = String.Format("ftp://{0}:{1}{2}/{3}", m_dicOfServer[nIndex].Address, m_dicOfServer[nIndex].Port, m_dicOfServer[nIndex].Path, strFileName);

            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    FtpWebRequest fwr = (FtpWebRequest)WebRequest.Create(strTargetFile);
                    fwr.Method = WebRequestMethods.Ftp.UploadFile;
                    fwr.Credentials = new NetworkCredential(m_dicOfServer[nIndex].ID, m_dicOfServer[nIndex].Password);

                    byte[] data;

                    data = File.ReadAllBytes(strSourceFile);

                    fwr.ContentLength = data.Length;

                    using (Stream reqStream = fwr.GetRequestStream())
                    {
                        reqStream.Write(data, 0, data.Length);
                    }
                }
                catch 
                { 
                }
            });
            return true;
        }
        public bool GetFileList(int nIndex)
        {
            m_arServerFileLIst = new string[] { };// 초기화 시키자

            if (m_dicOfServer.ContainsKey(nIndex) == false)
            {
                return false;
            }

            int nPort = m_dicOfServer[nIndex].Port;
            string strTargetFile = String.Format("ftp://{0}:{1}{2}", m_dicOfServer[nIndex].Address, m_dicOfServer[nIndex].Port, m_dicOfServer[nIndex].Path);

            try
            {
                FtpWebRequest fwr = (FtpWebRequest)WebRequest.Create(strTargetFile);
                fwr.Method = WebRequestMethods.Ftp.ListDirectory;
                fwr.Credentials = new NetworkCredential(m_dicOfServer[nIndex].ID, m_dicOfServer[nIndex].Password);
                FtpWebResponse resFtp = (FtpWebResponse)fwr.GetResponse();
                StreamReader reader;
                reader = new StreamReader(resFtp.GetResponseStream());
                string strData;
                strData = reader.ReadToEnd();
                m_arServerFileLIst = strData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < m_arServerFileLIst.Length; i++)
                {
                    m_arServerFileLIst[i] = m_arServerFileLIst[i].Split('/').Last();
                }
                resFtp.Close();
            }
            catch { return false; }

            return true;
        }
        public bool CreateDirectory(int nIndex, string strDirectoryName)
        {
            if (m_dicOfServer.ContainsKey(nIndex) == false)
            {
                return false;
            }

            int nPort = m_dicOfServer[nIndex].Port;

            string strTargetDirectory = String.Format("ftp://{0}:{1}{2}/{3}", m_dicOfServer[nIndex].Address, m_dicOfServer[nIndex].Port, m_dicOfServer[nIndex].Path, strDirectoryName);

            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    FtpWebRequest fwr = (FtpWebRequest)WebRequest.Create(strTargetDirectory);
                    fwr.Method = WebRequestMethods.Ftp.MakeDirectory;
                    fwr.Credentials = new NetworkCredential(m_dicOfServer[nIndex].ID, m_dicOfServer[nIndex].Password);

                    using (var resp = fwr.GetResponse())
                    {
                        resp.Close();
                    }
                }
                catch { }
            });
            return true;
        }
        public bool Download(int nIndex, string strFilePath, string strFileName)
        {
            if (m_dicOfServer.ContainsKey(nIndex) == false)
            {
                return false;
            }

            int nPort = m_dicOfServer[nIndex].Port;
            string strOutputFile = System.IO.Path.Combine(strFilePath, strFileName);

            string strTargetFile = String.Format("ftp://{0}:{1}{2}/{3}", m_dicOfServer[nIndex].Address, m_dicOfServer[nIndex].Port, m_dicOfServer[nIndex].Path, strFileName);


            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    FtpWebRequest fwr = (FtpWebRequest)WebRequest.Create(strTargetFile);
                    fwr.Method = WebRequestMethods.Ftp.DownloadFile;
                    fwr.Credentials = new NetworkCredential(m_dicOfServer[nIndex].ID, m_dicOfServer[nIndex].Password);

                    using (WebClient cli = new WebClient())
                    {
                        cli.Credentials = new NetworkCredential(m_dicOfServer[nIndex].ID, m_dicOfServer[nIndex].Password);

                        cli.DownloadFile(strTargetFile, strOutputFile);
                    }
                           
                }
                catch { }
            });
            return true;
        }
        public bool Delete(int nIndex, string strTargetName)
        {
            if (m_dicOfServer.ContainsKey(nIndex) == false)
            {
                return false;
            }

            bool bDirectory = strTargetName.Split('.').Length == 1;
            int nPort = m_dicOfServer[nIndex].Port;

            string strTarget = String.Format("ftp://{0}:{1}{2}/{3}", m_dicOfServer[nIndex].Address, m_dicOfServer[nIndex].Port, m_dicOfServer[nIndex].Path, strTargetName);

            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    FtpWebRequest fwr = (FtpWebRequest)WebRequest.Create(strTarget);
                    if(bDirectory)
                        fwr.Method = WebRequestMethods.Ftp.RemoveDirectory;
                    else
                        fwr.Method = WebRequestMethods.Ftp.DeleteFile;
                    fwr.Credentials = new NetworkCredential(m_dicOfServer[nIndex].ID, m_dicOfServer[nIndex].Password);

                    using (var resp = fwr.GetResponse())
                    {
                        resp.Close();
                    }
                }
                catch(Exception e)
                {
					Console.WriteLine(e.Message);
                }
            });
            return true;
        }

        #region Config
        public void AddSever()
        {
            m_dicOfServer.Add(m_dicOfServer.Count, new FTP_Server());

            Save();
        }
        public int[] GetServerIndexList()
        {
            return m_dicOfServer.Keys.ToArray();
        }

        public string GetConfigValue(int nIndex, EN_CONFIG_ITEM enItem)
        {
            if (m_dicOfServer.ContainsKey(nIndex))
            {
                switch (enItem)
                {
                    case EN_CONFIG_ITEM.NAME:
                        return m_dicOfServer[nIndex].Name;
                    case EN_CONFIG_ITEM.ADDRESS:
                        return m_dicOfServer[nIndex].Address;
                    case EN_CONFIG_ITEM.ID:
                        return m_dicOfServer[nIndex].ID;
                    case EN_CONFIG_ITEM.PASSWORD:
                        return m_dicOfServer[nIndex].Password;
                    case EN_CONFIG_ITEM.PORT:
                        return m_dicOfServer[nIndex].Port.ToString();
                    case EN_CONFIG_ITEM.PATH:
                        return m_dicOfServer[nIndex].Path;
                }
            }
            return "";
        }

        public void SetConfigValue(int nIndex, EN_CONFIG_ITEM enItem, string Value)
        {
            if (m_dicOfServer.ContainsKey(nIndex))
            {
                switch (enItem)
                {
                    case EN_CONFIG_ITEM.NAME:
                        m_dicOfServer[nIndex].Name = Value;
                        break;
                    case EN_CONFIG_ITEM.ADDRESS:
                        m_dicOfServer[nIndex].Address = Value;
                        break;
                    case EN_CONFIG_ITEM.ID:
                        m_dicOfServer[nIndex].ID = Value;
                        break;
                    case EN_CONFIG_ITEM.PASSWORD:
                        m_dicOfServer[nIndex].Password = Value;
                        break;
                    case EN_CONFIG_ITEM.PORT:
                        int nPort = 21;//FTP DEFUALT 포트
                        if (int.TryParse(Value, out nPort) == false)
                            return;
                        m_dicOfServer[nIndex].Port = nPort;
                        break;
                    case EN_CONFIG_ITEM.PATH:
                        m_dicOfServer[nIndex].Path = Value;
                        break;
                }
                Save();
            }
        }
        #endregion /Config
        #endregion
    }
}
