using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

using FileIOManager_;
using FileComposite_;
using Define.DefineConstant;

namespace FrameOfSystem3.EquipmentProperty
{
    public class RawMaterialPortManager
    {
        #region 싱글톤
        private RawMaterialPortManager() 
        {
            m_dicOfRawMaterialExist = new Dictionary<string, bool>();
        }

        private static readonly Lazy<RawMaterialPortManager> lazyInstance = new Lazy<RawMaterialPortManager>(() => new RawMaterialPortManager());
        static public RawMaterialPortManager GetInstance()
        {
            return lazyInstance.Value; 
        }
        #endregion

        #region Field
        Dictionary<string, bool> m_dicOfRawMaterialExist;
        #endregion

        #region Method
        public void AddRawMaterialPort(string strTaskName, string strPortName)
        {
            string strPortKey = strTaskName + "." + strPortName;

            if (m_dicOfRawMaterialExist.ContainsKey(strPortKey) == false)
            {
                DynamicLink_.EN_PORT_STATUS enPort = DynamicLink_.EN_PORT_STATUS.EMPTY;
                if (DynamicLink_.DynamicLink.GetInstance().GetPortStatus(strPortKey, ref enPort))
                {
                    bool bExist = enPort == DynamicLink_.EN_PORT_STATUS.EXIST
                               || enPort == DynamicLink_.EN_PORT_STATUS.WORKING
                               || enPort == DynamicLink_.EN_PORT_STATUS.FINISHED;
                    m_dicOfRawMaterialExist.Add(strPortKey, bExist);
                }
            }
        }

        public void SetRawMaterialExist(string strPortKey, DynamicLink_.EN_PORT_STATUS enPort)
        {
            if (m_dicOfRawMaterialExist.ContainsKey(strPortKey))
            {
                bool bExist = enPort == DynamicLink_.EN_PORT_STATUS.EXIST
                            || enPort == DynamicLink_.EN_PORT_STATUS.WORKING
                            || enPort == DynamicLink_.EN_PORT_STATUS.FINISHED;
                m_dicOfRawMaterialExist[strPortKey] = bExist;
            }
        }

        public bool GetRawMaterialExist()
        {
            bool bExist = false;
            foreach(var kpv in m_dicOfRawMaterialExist)
            {
                bExist |= kpv.Value;
            }
            return bExist;
        }
        #endregion
    }
}

