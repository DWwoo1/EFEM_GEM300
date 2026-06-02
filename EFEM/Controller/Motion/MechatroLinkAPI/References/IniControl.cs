using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace FrameOfSystem3.Controller.Motion.MechatroLinkAPI.References
{
    class IniControl
    {
        private string iniPath;
        private string _sectionName;

        public IniControl(string path)
        {
            this.iniPath = path;
        }

        public string sectionName
        {
            get { return _sectionName; }
            set { _sectionName = value; }
        }

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(
            String section,
            String key,
            String def,
            StringBuilder retVal,
            int size,
            String filePath);

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(
            String section,
            String key,
            String val,
            String filePath);

        public String GetString(String key, String defStr)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(_sectionName, key, defStr, temp, 255, iniPath);
            return temp.ToString();
        }

        public String GetString(String section, String key, String defStr)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, defStr, temp, 255, iniPath);
            return temp.ToString();
        }

        public long GetLong(String key, long defVal)
        {
            string defStr = String.Format("{0}", defVal);

            String stemp = GetString(_sectionName, key, defStr);
            return (Convert.ToInt64(stemp));
        }

        public long GetLong(String section, String key, long defVal)
        {
            string defStr = String.Format("{0}", defVal);

            String stemp = GetString(section, key, defStr);
            return (Convert.ToInt64(stemp));
        }

        public int GetInt(String key, int defVal)
        {
            string defStr = String.Format("{0}", defVal);

            String stemp = GetString(_sectionName, key, defStr);
            return (Convert.ToInt32(stemp));
        }

        public int GetInt(String section, String key, int defVal)
        {
            string defStr = String.Format("{0}", defVal);

            String stemp = GetString(section, key, defStr);
            return (Convert.ToInt32(stemp));
        }

        public double GetDouble(String key, double defVal)
        {
            string defStr = String.Format("{0}", defVal);

            String stemp = GetString(_sectionName, key, defStr);
            return (Convert.ToDouble(stemp));
        }

        public double GetDouble(String section, String key, double defVal)
        {
            string defStr = String.Format("{0}", defVal);

            String stemp = GetString(section, key, defStr);
            return (Convert.ToDouble(stemp));
        }

        public bool WriteString(String key, String writeStr)
        {
            return (WritePrivateProfileString(_sectionName, key, writeStr, iniPath) == 0 ? false : true);
        }

        public bool WriteString(String section, String key, String writeStr)
        {
            return (WritePrivateProfileString(section, key, writeStr, iniPath) == 0 ? false : true);
        }

        public bool WriteLong(String key, long writeVal)
        {
            return (WriteString(_sectionName, key, writeVal.ToString()));
        }

        public bool WriteLong(String section, String key, long writeVal)
        {
            return (WriteString(section, key, writeVal.ToString()));
        }

        public bool WriteInt(String key, int writeVal)
        {
            return (WriteString(_sectionName, key, writeVal.ToString()));
        }

        public bool WriteInt(String section, String key, int writeVal)
        {
            return (WriteString(section, key, writeVal.ToString()));
        }

        public bool WriteDouble(String key, double writeVal)
        {
            return (WriteString(_sectionName, key, writeVal.ToString()));
        }

        public bool WriteDouble(String section, String key, double writeVal)
        {
            return (WriteString(section, key, writeVal.ToString()));
        }
    }   
}
