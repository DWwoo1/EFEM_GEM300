using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Define.DefineConstant;
using Define.DefineEnumBase;
using FileIOManager_;
using FileComposite_;

using Interlock_;

namespace FrameOfSystem3.Config
{
    public class ConfigInterlock
    {
        #region 싱글톤
        private static ConfigInterlock _Instance = new ConfigInterlock();
        public static ConfigInterlock GetInstance()
        {
            return _Instance;
        }
        #endregion

        #region 변수
        private Interlock_.Interlock m_InstanceInterlock;

        private Functional.Storage m_instanceStorage;
        #endregion

        #region 상수
        private const string m_strInterlockMotion = "MOTION";
        private const string m_strInterlockCylinder = "CYLINDER";
        private const string m_strInterlockDO = "DIGITAL_OUT";

        private const string m_strEnable = "ENABLE";

        private const string m_strInterlockCondition = "INTERLOCK_CONDITION";
        private const string m_strCompareGroup = "COMPARE_GROUP";

        #endregion

        #region 속성
        #endregion

        #region 외부 인터페이스
        #region 초기화
        public bool Init()
        {
            m_instanceStorage = Functional.Storage.GetInstance();

            if (!m_instanceStorage.Load(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK))
                return false;

            m_InstanceInterlock = Interlock_.Interlock.GetInstance();

            if (false == LoadParameter())
                return false;

            return true;

        }
        #endregion 

        #region Item

        #region Motion
        
        #region Add
        public bool AddMotion(int nMotionIndex)
        {
            if(m_InstanceInterlock.AddMotion(nMotionIndex))
            {
                string[] arMotionGroupLevel = new string[] { m_strInterlockMotion, nMotionIndex.ToString() };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arMotionGroupLevel, false);

                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, m_strEnable, true, ref arMotionGroupLevel, false);
                
                string[] arIndterlockConditionLevel = new string[] { m_strInterlockMotion, nMotionIndex.ToString(), m_strInterlockCondition };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arIndterlockConditionLevel, false);
                string[] arCompareGroupLevel = new string[] { m_strInterlockMotion, nMotionIndex.ToString(), m_strCompareGroup };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareGroupLevel, false);

                m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
                return true;
            }
            return false;
        }
        public bool AddMotionInterlockCondition(int nMotionIndex, int nInterlockCondition)
        {
            if (m_InstanceInterlock.AddMotionInterlockCondition(nMotionIndex, nInterlockCondition))
            {
                string[] arConditionLevel = new string[] { m_strInterlockMotion, nMotionIndex.ToString(), m_strInterlockCondition, nInterlockCondition.ToString() };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arConditionLevel, false);

                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY.ToString(), 0, ref arConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.DIRECTION.ToString(), true, ref arConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.MOVING_DIRECTION.ToString(), EN_MOTION_MOVING_DIRECTION.BOTH, ref arConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.THRESHOLD.ToString(), 0.0, ref arConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.RELATIVE.ToString(), false, ref arConditionLevel, false);

                m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
                return true;
            }
            return false;
        }
        public bool AddMotionCompareGroup(int nMotionIndex, int nCompareGroupIndex)
        {
            if (m_InstanceInterlock.AddMotionCompareGroup(nMotionIndex, nCompareGroupIndex))
            {
                string[] arCompareGroupLevel = new string[] { m_strInterlockMotion, nMotionIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString() };

                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareGroupLevel, false);

                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE.ToString(), true, ref arCompareGroupLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME.ToString(), "", ref arCompareGroupLevel, false);

                m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
                return true;
            }
            return false;
        }
        public bool AddMotionCompareCondition(int nMotionIndex, int nCompareGroupIndex, int nCompareConditionIndex)
        {
            if (m_InstanceInterlock.AddMotionCompareCondition(nMotionIndex, nCompareGroupIndex, nCompareConditionIndex))
            {
                string[] arCompareConditionLevel = new string[] { m_strInterlockMotion, nMotionIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString(), nCompareConditionIndex.ToString() };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareConditionLevel, false);

                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP.ToString(), 0, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE.ToString(), EN_INTERLOCK_DEVICE.MOTION, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX.ToString(), 0, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX.ToString(), -1, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION.ToString(), true, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD.ToString(), 0.0, ref arCompareConditionLevel, false);

                m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
                return true;
            }
            return false;
        }
        #endregion /Add

        #region Remove
        public bool RemoveMotion(int nMotionIndex)
        {
            if (m_InstanceInterlock.RemoveMotion(nMotionIndex))
            {
                string[] arMotionGroupLevel = new string[] { m_strInterlockMotion, nMotionIndex.ToString() };
                return m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arMotionGroupLevel);
            }
            return false;
        }

        public bool RemoveMotionInterlockCondition(int nMotionIndex, int nInterlockCondition)
        {
            if (m_InstanceInterlock.RemoveMotionInterlockCondition(nMotionIndex, nInterlockCondition))
            {
                // 2024.09.30 by junho [ADD] remove 할 때 index가 연속되도록 개선
                //string[] arConditionLevel = new string[] { m_strInterlockMotion, nMotionIndex.ToString(), m_strInterlockCondition, nInterlockCondition.ToString() };
                //return m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arConditionLevel);
                return FileUpdateInterlockCondition(m_strInterlockMotion, nMotionIndex);
			}
			return false;
        }
        public bool RemoveMotionCompareGroup(int nMotionIndex, int nCompareGroupIndex)
        {
            if (m_InstanceInterlock.RemoveMotionCompareGroup(nMotionIndex, nCompareGroupIndex))
            {
                // 2024.09.30 by junho [ADD] remove 할 때 index가 연속되도록 개선
                //string[] arCompareGroupLevel = new string[] { m_strInterlockMotion, nMotionIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString() };
                //return m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareGroupLevel);
                return FileUpdateCompareGroup(m_strInterlockMotion, nMotionIndex, nCompareGroupIndex);
			}
			return false;
        }
        public bool RemoveMotionCompareCondition(int nMotionIndex, int nCompareGroupIndex, int nCompareConditionIndex)
        {
            if (m_InstanceInterlock.RemoveMotionCompareCondition(nMotionIndex, nCompareGroupIndex, nCompareConditionIndex))
            {
                // 2024.09.30 by junho [ADD] remove 할 때 index가 연속되도록 개선
                //string[] arCompareConditionkLevel = new string[] { m_strInterlockMotion, nMotionIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString(), nCompareConditionIndex.ToString() };
                //return m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareConditionkLevel);
                return FileUpdateCompareCondition(m_strInterlockMotion, nMotionIndex, nCompareGroupIndex);
            }
			return false;
        }
        #endregion /Remove

        #region Set
        public bool SetMotionEnable(int nMotionIndex, bool bEnable)
        {
            if (m_InstanceInterlock.SetInterLockMotionEnable(nMotionIndex, bEnable))
            {
                string[] arMotionGroupLevel = new string[] { m_strInterlockMotion, nMotionIndex.ToString() };
                return m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, m_strEnable, bEnable, ref arMotionGroupLevel);
            }
            return false;
        }

        public bool SetMotionInterlockCondition(int nMotionIndex, int nInterlockCondition, EN_INTERLOCK_PARAMTER_TYPE enType, object value)
        {
            if (m_InstanceInterlock.SetMotionInterlockCondition(nMotionIndex, nInterlockCondition, enType, value))
            {
                string[] arConditionLevel = new string[] { m_strInterlockMotion, nMotionIndex.ToString(), m_strInterlockCondition, nInterlockCondition.ToString() };
                return m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, enType.ToString(), value, ref arConditionLevel);
            }
            return false;
        }

        public bool SetMotionCompareGroup(int nMotionIndex, int nCompareGroupIndex, EN_COMPARE_GROUP_PARAMTER_TYPE enType, object value)
        {
            if (m_InstanceInterlock.SetMotionCompareGroup(nMotionIndex, nCompareGroupIndex, enType, value))
            {
                string[] arCompareGroupLevel = new string[] { m_strInterlockMotion, nMotionIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString() };
                return m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, enType.ToString(), value, ref arCompareGroupLevel);
            }
            return false;
        }

        public bool SetMotionCompareCondition(int nMotionIndex, int nCompareGroupIndex, int nCompareConditionIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE enType, object value)
        {
            if (m_InstanceInterlock.SetMotionCompareCondition(nMotionIndex, nCompareGroupIndex, nCompareConditionIndex, enType, value))
            {
                string[] arCompareConditionkLevel = new string[] { m_strInterlockMotion, nMotionIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString(), nCompareConditionIndex.ToString() };
                return m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, enType.ToString(), value, ref arCompareConditionkLevel);
            }
            return false;
        }
        #endregion /Set

        #endregion /Motion

		#region Cylinder

		#region Add
		public bool AddCylinder(int nCylinderIndex)
        {
            if (m_InstanceInterlock.AddCylinder(nCylinderIndex))
            {
                string[] arCylinderGroupLevel = new string[] { m_strInterlockCylinder, nCylinderIndex.ToString() };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCylinderGroupLevel, false);

                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, m_strEnable, true, ref arCylinderGroupLevel, false);

                string[] arIndterlockConditionLevel = new string[] { m_strInterlockCylinder, nCylinderIndex.ToString(), m_strInterlockCondition };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arIndterlockConditionLevel, false);
                string[] arCompareGroupLevel = new string[] { m_strInterlockCylinder, nCylinderIndex.ToString(), m_strCompareGroup };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareGroupLevel, false);

                m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
                return true;
            }
            return false;
        }

        public bool AddCylinderInterlockCondition(int nCylinderIndex, int nInterlockCondition)
        {
            if (m_InstanceInterlock.AddCylinderInterlockCondition(nCylinderIndex, nInterlockCondition))
            {
                string[] arConditionLevel = new string[] { m_strInterlockCylinder, nCylinderIndex.ToString(), m_strInterlockCondition, nInterlockCondition.ToString() };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arConditionLevel, false);

                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY.ToString(), 0, ref arConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.THRESHOLD.ToString(), 0.0, ref arConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.DIRECTION.ToString(), true, ref arConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.MOVING_DIRECTION.ToString(), EN_MOTION_MOVING_DIRECTION.BOTH, ref arConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.RELATIVE.ToString(), false, ref arConditionLevel, false);

                m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
                return true;
            }
            return false;
        }
        public bool AddCylinderCompareGroup(int nCylinderIndex, int nCompareGroupIndex)
        {
            if (m_InstanceInterlock.AddCylinderCompareGroup(nCylinderIndex, nCompareGroupIndex))
            {
                string[] arCompareGroupLevel = new string[] { m_strInterlockCylinder, nCylinderIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString() };

                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE.ToString(), true, ref arCompareGroupLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME.ToString(), "", ref arCompareGroupLevel, false);

                m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
                return true;
            }
            return false;
        }
        public bool AddCylinderCompareCondition(int nCylinderIndex, int nCompareGroupIndex, int nCompareConditionIndex)
        {
            if (m_InstanceInterlock.AddCylinderCompareCondition(nCylinderIndex, nCompareGroupIndex, nCompareConditionIndex))
            {
                string[] arCompareConditionLevel = new string[] { m_strInterlockCylinder, nCylinderIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString(), nCompareConditionIndex.ToString() };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareConditionLevel, false);

                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP.ToString(), 0, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE.ToString(), EN_INTERLOCK_DEVICE.MOTION, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX.ToString(), 0, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX.ToString(), -1, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION.ToString(), true, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD.ToString(), 0.0, ref arCompareConditionLevel, false);

                m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
                return true;
            }
            return false;
        }
        #endregion /Add

        #region Remove
        public bool RemoveCylinder(int nCylinderIndex)
        {
            if (m_InstanceInterlock.RemoveCylinder(nCylinderIndex))
            {
                string[] arCylinderGroupLevel = new string[] { m_strInterlockCylinder, nCylinderIndex.ToString() };
                return m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCylinderGroupLevel);
            }
            return false;
        }

        public bool RemoveCylinderInterlockCondition(int nCylinderIndex, int nInterlockCondition)
        {
            if (m_InstanceInterlock.RemoveCylinderInterlockCondition(nCylinderIndex, nInterlockCondition))
            {
                // 2024.09.30 by junho [ADD] remove 할 때 index가 연속되도록 개선
                //string[] arConditionLevel = new string[] { m_strInterlockCylinder, nCylinderIndex.ToString(), m_strInterlockCondition, nInterlockCondition.ToString() };
                //return m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arConditionLevel);
                return FileUpdateInterlockCondition(m_strInterlockCylinder, nCylinderIndex);
			}
            return false;
        }
        public bool RemoveCylinderCompareGroup(int nCylinderIndex, int nCompareGroupIndex)
        {
            if (m_InstanceInterlock.RemoveCylinderCompareGroup(nCylinderIndex, nCompareGroupIndex))
            {
                // 2024.09.30 by junho [ADD] remove 할 때 index가 연속되도록 개선
                //string[] arCompareGroupLevel = new string[] { m_strInterlockCylinder, nCylinderIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString() };
                //return m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareGroupLevel);
                return FileUpdateCompareGroup(m_strInterlockCylinder, nCylinderIndex, nCompareGroupIndex);
            }
            return false;
        }
        public bool RemoveCylinderCompareCondition(int nCylinderIndex, int nCompareGroupIndex, int nCompareConditionIndex)
        {
            if (m_InstanceInterlock.RemoveCylinderCompareCondition(nCylinderIndex, nCompareGroupIndex, nCompareConditionIndex))
            {
				// 2024.09.30 by junho [ADD] remove 할 때 index가 연속되도록 개선
				//string[] arCompareConditionkLevel = new string[] { m_strInterlockCylinder, nCylinderIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString(), nCompareConditionIndex.ToString() };
				//return m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareConditionkLevel);
				return FileUpdateCompareCondition(m_strInterlockCylinder, nCylinderIndex, nCompareGroupIndex);
			}
			return false;
        }
        #endregion /Add

        #region Set
        public bool SetCylinderEnable(int nCylinderIndex, bool bEnable)
        {
            if (m_InstanceInterlock.SetInterLockCylinderEnable(nCylinderIndex, bEnable))
            {
                string[] arCylinderGroupLevel = new string[] { m_strInterlockCylinder, nCylinderIndex.ToString() };
                return m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, m_strEnable, bEnable, ref arCylinderGroupLevel);
            }
            return false;
        }

        public bool SetCylinderInterlockCondition(int nCylinderIndex, int nInterlockCondition, EN_INTERLOCK_PARAMTER_TYPE enType, object value)
        {
            if (m_InstanceInterlock.SetCylinderInterlockCondition(nCylinderIndex, nInterlockCondition, enType, value))
            {
                string[] arConditionLevel = new string[] { m_strInterlockCylinder, nCylinderIndex.ToString(), m_strInterlockCondition, nInterlockCondition.ToString() };
                return m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, enType.ToString(), value, ref arConditionLevel);
            }
            return false;
        }

        public bool SetCylinderCompareGroup(int nCylinderIndex, int nCompareGroupIndex, EN_COMPARE_GROUP_PARAMTER_TYPE enType, object value)
        {
            if (m_InstanceInterlock.SetCylinderCompareGroup(nCylinderIndex, nCompareGroupIndex, enType, value))
            {
                string[] arCompareGroupLevel = new string[] { m_strInterlockCylinder, nCylinderIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString() };
                return m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, enType.ToString(), value, ref arCompareGroupLevel);
            }
            return false;
        }

        public bool SetCylinderCompareCondition(int nCylinderIndex, int nCompareGroupIndex, int nCompareConditionIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE enType, object value)
        {
            if (m_InstanceInterlock.SetCylinderCompareCondition(nCylinderIndex, nCompareGroupIndex, nCompareConditionIndex, enType, value))
            {
                string[] arCompareConditionkLevel = new string[] { m_strInterlockCylinder, nCylinderIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString(), nCompareConditionIndex.ToString() };
                return m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, enType.ToString(), value, ref arCompareConditionkLevel);
            }
            return false;
        }
        #endregion /Set

        #endregion /Cylinder

        #region DO

        #region Add
        public bool AddDO(int nDOIndex)
        {
            if (m_InstanceInterlock.AddDO(nDOIndex))
            {
                string[] arDOGroupLevel = new string[] { m_strInterlockDO, nDOIndex.ToString() };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arDOGroupLevel, false);

                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, m_strEnable, true, ref arDOGroupLevel, false);

                string[] arIndterlockConditionLevel = new string[] { m_strInterlockDO, nDOIndex.ToString(), m_strInterlockCondition };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arIndterlockConditionLevel, false);
                string[] arCompareGroupLevel = new string[] { m_strInterlockDO, nDOIndex.ToString(), m_strCompareGroup };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareGroupLevel, false);

                m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
                return true;
            }
            return false;
        }

        public bool AddDOInterlockCondition(int nDOIndex, int nInterlockCondition)
        {
            if (m_InstanceInterlock.AddDOInterlockCondition(nDOIndex, nInterlockCondition))
            {
                string[] arConditionLevel = new string[] { m_strInterlockDO, nDOIndex.ToString(), m_strInterlockCondition, nInterlockCondition.ToString() };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arConditionLevel, false);

                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY.ToString(), 0, ref arConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.DIRECTION.ToString(), true, ref arConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.MOVING_DIRECTION.ToString(), EN_MOTION_MOVING_DIRECTION.BOTH, ref arConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.THRESHOLD.ToString(), 0.0, ref arConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.RELATIVE.ToString(), false, ref arConditionLevel, false);

                m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
                return true;
            }
            return false;
        }
        public bool AddDOCompareGroup(int nDOIndex, int nCompareGroupIndex)
        {
            if (m_InstanceInterlock.AddDOCompareGroup(nDOIndex, nCompareGroupIndex))
            {
                string[] arCompareGroupLevel = new string[] { m_strInterlockDO, nDOIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString() };

                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE.ToString(), true, ref arCompareGroupLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME.ToString(), "", ref arCompareGroupLevel, false);

                m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
                return true;
            }
            return false;
        }
        public bool AddDOCompareCondition(int nDOIndex, int nCompareGroupIndex, int nCompareConditionIndex)
        {
            if (m_InstanceInterlock.AddDOCompareCondition(nDOIndex, nCompareGroupIndex, nCompareConditionIndex))
            {
                string[] arCompareConditionLevel = new string[] { m_strInterlockDO, nDOIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString(), nCompareConditionIndex.ToString() };
                m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareConditionLevel, false);

                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP.ToString(), 0, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE.ToString(), EN_INTERLOCK_DEVICE.MOTION, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX.ToString(), 0, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX.ToString(), -1, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION.ToString(), true, ref arCompareConditionLevel, false);
                m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD.ToString(), 0.0, ref arCompareConditionLevel, false);

                m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
                return true;
            }
            return false;
        }
        #endregion /Add

        #region Remove
        public bool RemoveDO(int nDOIndex)
        {
            if (m_InstanceInterlock.RemoveDO(nDOIndex))
            {
                string[] arDOGroupLevel = new string[] { m_strInterlockDO, nDOIndex.ToString() };
                return m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arDOGroupLevel);
            }
            return false;
        }

        public bool RemoveDOInterlockCondition(int nDOIndex, int nInterlockCondition)
		{
			if (m_InstanceInterlock.RemoveDOInterlockCondition(nDOIndex, nInterlockCondition))
			{
				// 2024.09.30 by junho [ADD] remove 할 때 index가 연속되도록 개선
				//string[] arConditionLevel = new string[] { m_strInterlockDO, nDOIndex.ToString(), m_strInterlockCondition, nInterlockCondition.ToString() };
				//return m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arConditionLevel);
				return FileUpdateInterlockCondition(m_strInterlockDO, nDOIndex);
			}
			return false;
		}
		public bool RemoveDOCompareGroup(int nDOIndex, int nCompareGroupIndex)
		{
			if (m_InstanceInterlock.RemoveDOCompareGroup(nDOIndex, nCompareGroupIndex))
			{
				// 2024.09.30 by junho [ADD] remove 할 때 index가 연속되도록 개선
				//string[] arCompareGroupLevel = new string[] { m_strInterlockDO, nDOIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString() };
				//return m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareGroupLevel);
				return FileUpdateCompareGroup(m_strInterlockDO, nDOIndex, nCompareGroupIndex);
			}
			return false;
		}
		public bool RemoveDOCompareCondition(int nDOIndex, int nCompareGroupIndex, int nCompareConditionIndex)
		{
			if (m_InstanceInterlock.RemoveDOCompareCondition(nDOIndex, nCompareGroupIndex, nCompareConditionIndex))
			{
				// 2024.09.30 by junho [ADD] remove 할 때 index가 연속되도록 개선
				//string[] arCompareConditionkLevel = new string[] { m_strInterlockDO, nDOIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString(), nCompareConditionIndex.ToString() };
				//return m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareConditionkLevel);
				return FileUpdateCompareCondition(m_strInterlockDO, nDOIndex, nCompareGroupIndex);
			}
			return false;
        }
        #endregion /Add

        #region Set
        public bool SetDOEnable(int nDOIndex, bool bEnable)
        {
            if (m_InstanceInterlock.SetInterLockDOEnable(nDOIndex, bEnable))
            {
                string[] arDOGroupLevel = new string[] { m_strInterlockDO, nDOIndex.ToString() };
                return m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, m_strEnable, bEnable, ref arDOGroupLevel);
            }
            return false;
        }

        public bool SetDOInterlockCondition(int nDOIndex, int nInterlockCondition, EN_INTERLOCK_PARAMTER_TYPE enType, object value)
        {
            if (m_InstanceInterlock.SetDOInterlockCondition(nDOIndex, nInterlockCondition, enType, value))
            {
                string[] arConditionLevel = new string[] { m_strInterlockDO, nDOIndex.ToString(), m_strInterlockCondition, nInterlockCondition.ToString() };
                return m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, enType.ToString(), value, ref arConditionLevel);
            }
            return false;
        }

        public bool SetDOCompareGroup(int nDOIndex, int nCompareGroupIndex, EN_COMPARE_GROUP_PARAMTER_TYPE enType, object value)
        {
            if (m_InstanceInterlock.SetDOCompareGroup(nDOIndex, nCompareGroupIndex, enType, value))
            {
                string[] arCompareGroupLevel = new string[] { m_strInterlockDO, nDOIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString() };
                return m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, enType.ToString(), value, ref arCompareGroupLevel);
            }
            return false;
        }

        public bool SetDOCompareCondition(int nDOIndex, int nCompareGroupIndex, int nCompareConditionIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE enType, object value)
        {
            if (m_InstanceInterlock.SetDOCompareCondition(nDOIndex, nCompareGroupIndex, nCompareConditionIndex, enType, value))
            {
                string[] arCompareConditionkLevel = new string[] { m_strInterlockDO, nDOIndex.ToString(), m_strCompareGroup, nCompareGroupIndex.ToString(), nCompareConditionIndex.ToString() };
                return m_instanceStorage.SetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, enType.ToString(), value, ref arCompareConditionkLevel);
            }
            return false;
        }
        #endregion /Set

        #endregion /DO

        #endregion /Item

        #region 발생 정보
        public string GetMotionLastInterference(int nMotionIndex)
        {
            string strReturn = "";
            List<Interlock_.EN_INTERLOCK_DEVICE> lstIterlockDevice = new List<Interlock_.EN_INTERLOCK_DEVICE>();
            List<int> lstIterlockDeviceIndex = new List<int>();
            Interlock_.Interlock.GetInstance().GetMotionLastInterference(nMotionIndex, ref lstIterlockDevice, ref lstIterlockDeviceIndex);
            for (int nIndex = 0; nIndex < lstIterlockDevice.Count; nIndex++ )
            {
                switch (lstIterlockDevice[nIndex])
                {
                    case EN_INTERLOCK_DEVICE.MOTION:
                    case EN_INTERLOCK_DEVICE.MOTION_RELATIVE:
                        strReturn += string.Format("{0} (MOTION)\n" ,ConfigMotion.GetInstance().GetName(lstIterlockDeviceIndex[nIndex]));
                        break;
                    case EN_INTERLOCK_DEVICE.CYLINDER:
                        strReturn += string.Format("{0} (CYLINDER)\n", ConfigCylinder.GetInstance().GetName(lstIterlockDeviceIndex[nIndex]));
                        break;
                    case EN_INTERLOCK_DEVICE.DI:
                        strReturn += string.Format("{0} (DIGITAL INPUT)\n", ConfigDigitalIO.GetInstance().GetName(true, lstIterlockDeviceIndex[nIndex]));
                        break;
                    case EN_INTERLOCK_DEVICE.DO:
                        strReturn += string.Format("{0} (DIGTAL OUTPUT)\n", ConfigDigitalIO.GetInstance().GetName(false, lstIterlockDeviceIndex[nIndex]));
                        break;
                }

            }
            return strReturn;
        }

        public string GetCylinderLastInterference(int nCylinderIndex)
        {
            string strReturn = "";
            List<Interlock_.EN_INTERLOCK_DEVICE> lstIterlockDevice = new List<Interlock_.EN_INTERLOCK_DEVICE>();
            List<int> lstIterlockDeviceIndex = new List<int>();
            Interlock_.Interlock.GetInstance().GetCylinderLastInterference(nCylinderIndex, ref lstIterlockDevice, ref lstIterlockDeviceIndex);
            for (int nIndex = 0; nIndex < lstIterlockDevice.Count; nIndex++)
            {
                switch (lstIterlockDevice[nIndex])
                {
                    case EN_INTERLOCK_DEVICE.MOTION:
                    case EN_INTERLOCK_DEVICE.MOTION_RELATIVE:
                        strReturn += string.Format("{0} (MOTION)\n", ConfigMotion.GetInstance().GetName(lstIterlockDeviceIndex[nIndex]));
                        break;
                    case EN_INTERLOCK_DEVICE.CYLINDER:
                        strReturn += string.Format("{0} (CYLINDER)\n", ConfigCylinder.GetInstance().GetName(lstIterlockDeviceIndex[nIndex]));
                        break;
                    case EN_INTERLOCK_DEVICE.DI:
                        strReturn += string.Format("{0} (DIGITAL INPUT)\n", ConfigDigitalIO.GetInstance().GetName(true, lstIterlockDeviceIndex[nIndex]));
                        break;
                    case EN_INTERLOCK_DEVICE.DO:
                        strReturn += string.Format("{0} (DIGTAL OUTPUT)\n", ConfigDigitalIO.GetInstance().GetName(false, lstIterlockDeviceIndex[nIndex]));
                        break;
                }

            }
            return strReturn;
        }

        public string GetDOLastInterference(int nDOIndex)
        {
            string strReturn = "";
            List<Interlock_.EN_INTERLOCK_DEVICE> lstIterlockDevice = new List<Interlock_.EN_INTERLOCK_DEVICE>();
            List<int> lstIterlockDeviceIndex = new List<int>();
            Interlock_.Interlock.GetInstance().GetDOLastInterference(nDOIndex, ref lstIterlockDevice, ref lstIterlockDeviceIndex);
            for (int nIndex = 0; nIndex < lstIterlockDevice.Count; nIndex++)
            {
                switch (lstIterlockDevice[nIndex])
                {
                    case EN_INTERLOCK_DEVICE.MOTION:
                    case EN_INTERLOCK_DEVICE.MOTION_RELATIVE:
                        strReturn += string.Format("{0} (MOTION)\n", ConfigMotion.GetInstance().GetName(lstIterlockDeviceIndex[nIndex]));
                        break;
                    case EN_INTERLOCK_DEVICE.CYLINDER:
                        strReturn += string.Format("{0} (CYLINDER)\n", ConfigCylinder.GetInstance().GetName(lstIterlockDeviceIndex[nIndex]));
                        break;
                    case EN_INTERLOCK_DEVICE.DI:
                        strReturn += string.Format("{0} (DIGITAL INPUT)\n", ConfigDigitalIO.GetInstance().GetName(true, lstIterlockDeviceIndex[nIndex]));
                        break;
                    case EN_INTERLOCK_DEVICE.DO:
                        strReturn += string.Format("{0} (DIGTAL OUTPUT)\n", ConfigDigitalIO.GetInstance().GetName(false, lstIterlockDeviceIndex[nIndex]));
                        break;
                }

            }
            return strReturn;
        }
        #endregion

        #endregion

        #region 내부 인터페이스
        #region 파일 입출력
        private bool LoadParameter()
        {
            bool bReturn = true;
            string strValue = string.Empty;
            string[] strArrGroup = null;

            if (m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref strArrGroup))
            {
                foreach (string strDevice in strArrGroup)
                {
                    switch (strDevice)
                    {
                        case m_strInterlockMotion:
                            bReturn &= LoadMotion();
                            break;
                        case m_strInterlockCylinder:
                            bReturn &= LoadCylinder();
                            break;
                        case m_strInterlockDO:
                            bReturn &= LoadDO();
                            break;
                    }
                }
                return bReturn;
            }
            return false;
        }
        private bool LoadMotion()
        {
            bool bReturn = true;

            string[] arMotionGroupLevel = new string[] { m_strInterlockMotion };
            string[] arMotionList = new string[] { };
            m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arMotionList, arMotionGroupLevel);
            int nMotionIndex = 0;
            foreach (string sMotion in arMotionList)
            {
                if (!int.TryParse(sMotion, out nMotionIndex))
                    continue;
                if (m_InstanceInterlock.AddMotion(nMotionIndex))
                {
                    string[] arMotionItemGroupLevel = m_instanceStorage.AddGroupLevel(arMotionGroupLevel, sMotion);
                    string[] arMotionCondition = new string[] { };
                    bool bEnable = true;
                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, "ENABLE", ref bEnable, ref arMotionItemGroupLevel);
                    bReturn &= m_InstanceInterlock.SetInterLockMotionEnable(nMotionIndex, bEnable);

                    #region Interlock Condition
                    string[] arInterlockConditionGroupLevel = m_instanceStorage.AddGroupLevel(arMotionItemGroupLevel, m_strInterlockCondition);
                    string[] arInterlockCondition = new string[] { };
                    m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arInterlockCondition, arInterlockConditionGroupLevel);
                    foreach (string sInterlockCondition in arInterlockCondition)
                    {
                        int nInterlockCondition = 0;
                        if (!int.TryParse(sInterlockCondition, out nInterlockCondition))
                            continue;
                        if (m_InstanceInterlock.AddMotionInterlockCondition(nMotionIndex, nInterlockCondition))
                        {
                            string[] arInterlockItemGroupLevel = m_instanceStorage.AddGroupLevel(arInterlockConditionGroupLevel, sInterlockCondition);

                            int nCompareKey = 0;
                            bool bDirection = true;
                            double dThreshold = 0.0;
                            string strDirection = "";
                            EN_MOTION_MOVING_DIRECTION enDirection = EN_MOTION_MOVING_DIRECTION.BOTH;
                            bool bRelative = false;

                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY.ToString(), ref nCompareKey, ref arInterlockItemGroupLevel);
                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.DIRECTION.ToString(), ref bDirection, ref arInterlockItemGroupLevel);
                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.THRESHOLD.ToString(), ref dThreshold, ref arInterlockItemGroupLevel);
                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.MOVING_DIRECTION.ToString(), ref strDirection, ref arInterlockItemGroupLevel);
                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.RELATIVE.ToString(), ref bRelative, ref arInterlockItemGroupLevel);
                            
                            bReturn &= m_InstanceInterlock.SetMotionInterlockCondition(nMotionIndex, nInterlockCondition, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY, nCompareKey);
                            bReturn &= m_InstanceInterlock.SetMotionInterlockCondition(nMotionIndex, nInterlockCondition, EN_INTERLOCK_PARAMTER_TYPE.DIRECTION, bDirection);
                            bReturn &= m_InstanceInterlock.SetMotionInterlockCondition(nMotionIndex, nInterlockCondition, EN_INTERLOCK_PARAMTER_TYPE.THRESHOLD, dThreshold);
                            bReturn &= Enum.TryParse(strDirection, out enDirection);
                            bReturn &= m_InstanceInterlock.SetMotionInterlockCondition(nMotionIndex, nInterlockCondition, EN_INTERLOCK_PARAMTER_TYPE.MOVING_DIRECTION, enDirection);
                            bReturn &= m_InstanceInterlock.SetMotionInterlockCondition(nMotionIndex, nInterlockCondition, EN_INTERLOCK_PARAMTER_TYPE.RELATIVE, bRelative);

                        }
                    }
                    #endregion /Interlock Condition

                    #region CompareGroup
                    string[] arCompareGroupGroupLevel = m_instanceStorage.AddGroupLevel(arMotionItemGroupLevel, m_strCompareGroup);
                    string[] arCompareGroup = new string[] { };
                    m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareGroup, arCompareGroupGroupLevel);
                    foreach (string sCompareGroup in arCompareGroup)
                    {
                        int nCompareGroup = 0;
                        if (!int.TryParse(sCompareGroup, out nCompareGroup))
                            continue;
                        string[] arCompareGroupItemGroupLevel = m_instanceStorage.AddGroupLevel(arCompareGroupGroupLevel, sCompareGroup);

                        if (m_InstanceInterlock.AddMotionCompareGroup(nMotionIndex, nCompareGroup))
                        {
                            bEnable = true;
                            string strName = "";

                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE.ToString(), ref bEnable, ref arCompareGroupItemGroupLevel);
                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME.ToString(), ref strName, ref arCompareGroupItemGroupLevel);

                            bReturn &= m_InstanceInterlock.SetMotionCompareGroup(nMotionIndex, nCompareGroup, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE, bEnable);
                            bReturn &= m_InstanceInterlock.SetMotionCompareGroup(nMotionIndex, nCompareGroup, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME, strName);

                            #region CompareCondtion
                            string[] arCompareCondition = new string[] { };
                            m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareCondition, arCompareGroupItemGroupLevel);
                            foreach (string sCompareCondition in arCompareCondition)
                            {
                                int nCompareCondition = 0;
                                if (!int.TryParse(sCompareCondition, out nCompareCondition))
                                    continue;

                                if (m_InstanceInterlock.AddMotionCompareCondition(nMotionIndex, nCompareGroup, nCompareCondition))
                                {
                                    string[] arCompareConditionGroupLevel = m_instanceStorage.AddGroupLevel(arCompareGroupItemGroupLevel, sCompareCondition);

                                    int nGroupKey = 0;
                                    string strDevice = "";
                                    EN_INTERLOCK_DEVICE enDevice = EN_INTERLOCK_DEVICE.MOTION;
                                    int nDeviceIndex = 0;
                                    int nRelativeDeviceIndex = -1;
                                    bool bDirection = true;
                                    double dThreshold = 0.0;

                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP.ToString(), ref nGroupKey, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE.ToString(), ref strDevice, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX.ToString(), ref nDeviceIndex, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX.ToString(), ref nRelativeDeviceIndex, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION.ToString(), ref bDirection, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD.ToString(), ref dThreshold, ref arCompareConditionGroupLevel);

                                    bReturn &= m_InstanceInterlock.SetMotionCompareCondition(nMotionIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP, nGroupKey);
                                    bReturn &= Enum.TryParse(strDevice, out enDevice);
                                    bReturn &= m_InstanceInterlock.SetMotionCompareCondition(nMotionIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE, enDevice);
                                    bReturn &= m_InstanceInterlock.SetMotionCompareCondition(nMotionIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX, nDeviceIndex);
                                    bReturn &= m_InstanceInterlock.SetMotionCompareCondition(nMotionIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX, nRelativeDeviceIndex);
                                    bReturn &= m_InstanceInterlock.SetMotionCompareCondition(nMotionIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION, bDirection);
                                    bReturn &= m_InstanceInterlock.SetMotionCompareCondition(nMotionIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD, dThreshold);
                                }
                            }
                            #endregion /CompareCondtion

                        }

                    }
                    #endregion /CompareGroup
                }
            }
            return bReturn;
        }
        private bool LoadCylinder()
        {
            bool bReturn = true;

            string[] arCylinderGroupLevel = new string[] { m_strInterlockCylinder };
            string[] arCylinderList = new string[] { };
            m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCylinderList, arCylinderGroupLevel);
            int nCylinderIndex = 0;
            foreach (string sCylinder in arCylinderList)
            {
                if (!int.TryParse(sCylinder, out nCylinderIndex))
                    continue;
                if (m_InstanceInterlock.AddCylinder(nCylinderIndex))
                {
                    string[] arCylinderItemGroupLevel = m_instanceStorage.AddGroupLevel(arCylinderGroupLevel, sCylinder);
                    string[] arCylinderCondition = new string[] { };
                    bool bEnable = true;
                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, "ENABLE", ref bEnable, ref arCylinderItemGroupLevel);
                    bReturn &= m_InstanceInterlock.SetInterLockCylinderEnable(nCylinderIndex, bEnable);

                    #region Interlock Condition
                    string[] arInterlockConditionGroupLevel = m_instanceStorage.AddGroupLevel(arCylinderItemGroupLevel, m_strInterlockCondition);
                    string[] arInterlockCondition = new string[] { };
                    m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arInterlockCondition, arInterlockConditionGroupLevel);
                    foreach (string sInterlockCondition in arInterlockCondition)
                    {
                        int nInterlockCondition = 0;
                        if (!int.TryParse(sInterlockCondition, out nInterlockCondition))
                            continue;
                        if (m_InstanceInterlock.AddCylinderInterlockCondition(nCylinderIndex, nInterlockCondition))
                        {
                            string[] arInterlockItemGroupLevel = m_instanceStorage.AddGroupLevel(arInterlockConditionGroupLevel, sInterlockCondition);

                            int nCompareKey = 0;
                            bool bDirection = true;

                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY.ToString(), ref nCompareKey, ref arInterlockItemGroupLevel);
                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.DIRECTION.ToString(), ref bDirection, ref arInterlockItemGroupLevel);

                            bReturn &= m_InstanceInterlock.SetCylinderInterlockCondition(nCylinderIndex, nInterlockCondition, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY, nCompareKey);
                            bReturn &= m_InstanceInterlock.SetCylinderInterlockCondition(nCylinderIndex, nInterlockCondition, EN_INTERLOCK_PARAMTER_TYPE.DIRECTION, bDirection);

                        }
                    }
                    #endregion /Interlock Condition

                    #region CompareGroup
                    string[] arCompareGroupGroupLevel = m_instanceStorage.AddGroupLevel(arCylinderItemGroupLevel, m_strCompareGroup);
                    string[] arCompareGroup = new string[] { };
                    m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareGroup, arCompareGroupGroupLevel);
                    foreach (string sCompareGroup in arCompareGroup)
                    {
                        int nCompareGroup = 0;
                        if (!int.TryParse(sCompareGroup, out nCompareGroup))
                            continue;
                        string[] arCompareGroupItemGroupLevel = m_instanceStorage.AddGroupLevel(arCompareGroupGroupLevel, sCompareGroup);

                        if (m_InstanceInterlock.AddCylinderCompareGroup(nCylinderIndex, nCompareGroup))
                        {
                            bEnable = true;
                            string strName = "";

                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE.ToString(), ref bEnable, ref arCompareGroupItemGroupLevel);
                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME.ToString(), ref strName, ref arCompareGroupItemGroupLevel);

                            bReturn &= m_InstanceInterlock.SetCylinderCompareGroup(nCylinderIndex, nCompareGroup, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE, bEnable);
                            bReturn &= m_InstanceInterlock.SetCylinderCompareGroup(nCylinderIndex, nCompareGroup, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME, strName);

                            #region CompareCondtion
                            string[] arCompareCondition = new string[] { };
                            m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareCondition, arCompareGroupItemGroupLevel);
                            foreach (string sCompareCondition in arCompareCondition)
                            {
                                int nCompareCondition = 0;
                                if (!int.TryParse(sCompareCondition, out nCompareCondition))
                                    continue;

                                if (m_InstanceInterlock.AddCylinderCompareCondition(nCylinderIndex, nCompareGroup, nCompareCondition))
                                {
                                    string[] arCompareConditionGroupLevel = m_instanceStorage.AddGroupLevel(arCompareGroupItemGroupLevel, sCompareCondition);

                                    int nGroupKey = 0;
                                    string strDevice = "";
                                    EN_INTERLOCK_DEVICE enDevice = EN_INTERLOCK_DEVICE.MOTION;
                                    int nDeviceIndex = 0;
                                    int nRelativeDeviceIndex = -1;
                                    bool bDirection = true;
                                    double dThreshold = 0.0;

                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP.ToString(), ref nGroupKey, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE.ToString(), ref strDevice, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX.ToString(), ref nDeviceIndex, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX.ToString(), ref nRelativeDeviceIndex, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION.ToString(), ref bDirection, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD.ToString(), ref dThreshold, ref arCompareConditionGroupLevel);

                                    bReturn &= m_InstanceInterlock.SetCylinderCompareCondition(nCylinderIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP, nGroupKey);
                                    bReturn &= Enum.TryParse(strDevice, out enDevice);
                                    bReturn &= m_InstanceInterlock.SetCylinderCompareCondition(nCylinderIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE, enDevice);
                                    bReturn &= m_InstanceInterlock.SetCylinderCompareCondition(nCylinderIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX, nDeviceIndex);
                                    bReturn &= m_InstanceInterlock.SetCylinderCompareCondition(nCylinderIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX, nRelativeDeviceIndex);
                                    bReturn &= m_InstanceInterlock.SetCylinderCompareCondition(nCylinderIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION, bDirection);
                                    bReturn &= m_InstanceInterlock.SetCylinderCompareCondition(nCylinderIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD, dThreshold);
                                }
                            }
                            #endregion /CompareCondtion

                        }

                    }
                    #endregion /CompareGroup
                }
            }
            return bReturn;
        }
        private bool LoadDO()
        {
            bool bReturn = true;

            string[] arDOGroupLevel = new string[] { m_strInterlockDO };
            string[] arDOList = new string[] { };
            m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arDOList, arDOGroupLevel);
            int nDOIndex = 0;
            foreach (string sDO in arDOList)
            {
                if (!int.TryParse(sDO, out nDOIndex))
                    continue;
                if (m_InstanceInterlock.AddDO(nDOIndex))
                {
                    string[] arDOItemGroupLevel = m_instanceStorage.AddGroupLevel(arDOGroupLevel, sDO);
                    string[] arDOCondition = new string[] { };
                    bool bEnable = true;
                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, "ENABLE", ref bEnable, ref arDOItemGroupLevel);
                    bReturn &= m_InstanceInterlock.SetInterLockDOEnable(nDOIndex, bEnable);

                    #region Interlock Condition
                    string[] arInterlockConditionGroupLevel = m_instanceStorage.AddGroupLevel(arDOItemGroupLevel, m_strInterlockCondition);
                    string[] arInterlockCondition = new string[] { };
                    m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arInterlockCondition, arInterlockConditionGroupLevel);
                    foreach (string sInterlockCondition in arInterlockCondition)
                    {
                        int nInterlockCondition = 0;
                        if (!int.TryParse(sInterlockCondition, out nInterlockCondition))
                            continue;
                        if (m_InstanceInterlock.AddDOInterlockCondition(nDOIndex, nInterlockCondition))
                        {
                            string[] arInterlockItemGroupLevel = m_instanceStorage.AddGroupLevel(arInterlockConditionGroupLevel, sInterlockCondition);

                            int nCompareKey = 0;
                            bool bDirection = true;

                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY.ToString(), ref nCompareKey, ref arInterlockItemGroupLevel);
                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.PULSE.ToString(), ref bDirection, ref arInterlockItemGroupLevel);

                            bReturn &= m_InstanceInterlock.SetDOInterlockCondition(nDOIndex, nInterlockCondition, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY, nCompareKey);
                            bReturn &= m_InstanceInterlock.SetDOInterlockCondition(nDOIndex, nInterlockCondition, EN_INTERLOCK_PARAMTER_TYPE.PULSE, bDirection);

                        }
                    }
                    #endregion /Interlock Condition

                    #region CompareGroup
                    string[] arCompareGroupGroupLevel = m_instanceStorage.AddGroupLevel(arDOItemGroupLevel, m_strCompareGroup);
                    string[] arCompareGroup = new string[] { };
                    m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareGroup, arCompareGroupGroupLevel);
                    foreach (string sCompareGroup in arCompareGroup)
                    {
                        int nCompareGroup = 0;
                        if (!int.TryParse(sCompareGroup, out nCompareGroup))
                            continue;
                        string[] arCompareGroupItemGroupLevel = m_instanceStorage.AddGroupLevel(arCompareGroupGroupLevel, sCompareGroup);

                        if (m_InstanceInterlock.AddDOCompareGroup(nDOIndex, nCompareGroup))
                        {
                            bEnable = true;
                            string strName = "";

                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE.ToString(), ref bEnable, ref arCompareGroupItemGroupLevel);
                            bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME.ToString(), ref strName, ref arCompareGroupItemGroupLevel);

                            bReturn &= m_InstanceInterlock.SetDOCompareGroup(nDOIndex, nCompareGroup, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE, bEnable);
                            bReturn &= m_InstanceInterlock.SetDOCompareGroup(nDOIndex, nCompareGroup, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME, strName);

                            #region CompareCondtion
                            string[] arCompareCondition = new string[] { };
                            m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareCondition, arCompareGroupItemGroupLevel);
                            foreach (string sCompareCondition in arCompareCondition)
                            {
                                int nCompareCondition = 0;
                                if (!int.TryParse(sCompareCondition, out nCompareCondition))
                                    continue;

                                if (m_InstanceInterlock.AddDOCompareCondition(nDOIndex, nCompareGroup, nCompareCondition))
                                {
                                    string[] arCompareConditionGroupLevel = m_instanceStorage.AddGroupLevel(arCompareGroupItemGroupLevel, sCompareCondition);

                                    int nGroupKey = 0;
                                    string strDevice = "";
                                    EN_INTERLOCK_DEVICE enDevice = EN_INTERLOCK_DEVICE.MOTION;
                                    int nDeviceIndex = 0;
                                    int nRelativeDeviceIndex = -1;
                                    bool bDirection = true;
                                    double dThreshold = 0.0;

                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP.ToString(), ref nGroupKey, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE.ToString(), ref strDevice, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX.ToString(), ref nDeviceIndex, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX.ToString(), ref nRelativeDeviceIndex, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION.ToString(), ref bDirection, ref arCompareConditionGroupLevel);
                                    bReturn &= m_instanceStorage.GetParameter(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD.ToString(), ref dThreshold, ref arCompareConditionGroupLevel);

                                    bReturn &= m_InstanceInterlock.SetDOCompareCondition(nDOIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP, nGroupKey);
                                    bReturn &= Enum.TryParse(strDevice, out enDevice);
                                    bReturn &= m_InstanceInterlock.SetDOCompareCondition(nDOIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE, enDevice);
                                    bReturn &= m_InstanceInterlock.SetDOCompareCondition(nDOIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX, nDeviceIndex);
                                    bReturn &= m_InstanceInterlock.SetDOCompareCondition(nDOIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX, nRelativeDeviceIndex);
                                    bReturn &= m_InstanceInterlock.SetDOCompareCondition(nDOIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION, bDirection);
                                    bReturn &= m_InstanceInterlock.SetDOCompareCondition(nDOIndex, nCompareGroup, nCompareCondition, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD, dThreshold);
                                }
                            }
                            #endregion /CompareCondtion

                        }

                    }
                    #endregion /CompareGroup
                }
            }
            return bReturn;
        }
		#endregion

		private bool FileUpdateInterlockCondition(string subject, int index)
		{
			object[] arCompareKey = new object[] { };
			object[] arThreshold = new object[] { };
			object[] arDirection = new object[] { };
			object[] arMovingDirection = new object[] { };
			object[] arRelative = new object[] { };
			switch (subject)
			{
				case m_strInterlockMotion:
					if (false == m_InstanceInterlock.GetMotionInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY, ref arCompareKey)) return false;
					if (false == m_InstanceInterlock.GetMotionInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.THRESHOLD, ref arThreshold)) return false;
					if (false == m_InstanceInterlock.GetMotionInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.DIRECTION, ref arDirection)) return false;
					if (false == m_InstanceInterlock.GetMotionInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.MOVING_DIRECTION, ref arMovingDirection)) return false;
					if (false == m_InstanceInterlock.GetMotionInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.RELATIVE, ref arRelative)) return false;
					break;
				case m_strInterlockCylinder:
					if (false == m_InstanceInterlock.GetCylinderInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY, ref arCompareKey)) return false;
					if (false == m_InstanceInterlock.GetCylinderInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.THRESHOLD, ref arThreshold)) return false;
					if (false == m_InstanceInterlock.GetCylinderInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.DIRECTION, ref arDirection)) return false;
					if (false == m_InstanceInterlock.GetCylinderInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.MOVING_DIRECTION, ref arMovingDirection)) return false;
					if (false == m_InstanceInterlock.GetCylinderInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.RELATIVE, ref arRelative)) return false;
					break;
				case m_strInterlockDO:
					if (false == m_InstanceInterlock.GetDOInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY, ref arCompareKey)) return false;
					if (false == m_InstanceInterlock.GetDOInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.THRESHOLD, ref arThreshold)) return false;
					if (false == m_InstanceInterlock.GetDOInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.DIRECTION, ref arDirection)) return false;
					if (false == m_InstanceInterlock.GetDOInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.MOVING_DIRECTION, ref arMovingDirection)) return false;
					if (false == m_InstanceInterlock.GetDOInterLockConditionList(index, EN_INTERLOCK_PARAMTER_TYPE.RELATIVE, ref arRelative)) return false;
					break;
				default: return false;
			}

			string[] arConditionLevel = new string[] { subject, index.ToString(), m_strInterlockCondition };
			if (false == m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arConditionLevel)) return false;
			if (false == m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arConditionLevel, false)) return false;

			for (int i = 0; i < arCompareKey.Length; ++i)
			{
				string[] arSaveGroup = new string[] { subject, index.ToString(), m_strInterlockCondition, i.ToString() };
				m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arSaveGroup, false);

				m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.COMPARE_KEY.ToString(), arCompareKey[i], ref arSaveGroup, false);
				m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.THRESHOLD.ToString(), arThreshold[i], ref arSaveGroup, false);
				m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.DIRECTION.ToString(), arDirection[i], ref arSaveGroup, false);
				m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.MOVING_DIRECTION.ToString(), arMovingDirection[i], ref arSaveGroup, false);
				m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_INTERLOCK_PARAMTER_TYPE.RELATIVE.ToString(), arRelative[i], ref arSaveGroup, false);
			}
			return m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
		}
		private bool FileUpdateCompareGroup(string subject, int index, int compareGroupIndex)
		{
			object[] arIndex = new object[] { };
			object[] arEnable = new object[] { };
			object[] arName = new object[] { };
			switch (subject)
			{
				case m_strInterlockMotion:
					if (false == m_InstanceInterlock.GetMotionCompareGroupList(index, EN_COMPARE_GROUP_PARAMTER_TYPE.INDEX, ref arIndex)) return false;
					if (false == m_InstanceInterlock.GetMotionCompareGroupList(index, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE, ref arEnable)) return false;
					if (false == m_InstanceInterlock.GetMotionCompareGroupList(index, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME, ref arName)) return false;
					break;
				case m_strInterlockCylinder:
					if (false == m_InstanceInterlock.GetCylinderCompareGroupList(index, EN_COMPARE_GROUP_PARAMTER_TYPE.INDEX, ref arIndex)) return false;
					if (false == m_InstanceInterlock.GetCylinderCompareGroupList(index, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE, ref arEnable)) return false;
					if (false == m_InstanceInterlock.GetCylinderCompareGroupList(index, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME, ref arName)) return false;
					break;
				case m_strInterlockDO:
					if (false == m_InstanceInterlock.GetDOCompareGroupList(index, EN_COMPARE_GROUP_PARAMTER_TYPE.INDEX, ref arIndex)) return false;
					if (false == m_InstanceInterlock.GetDOCompareGroupList(index, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE, ref arEnable)) return false;
					if (false == m_InstanceInterlock.GetDOCompareGroupList(index, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME, ref arName)) return false;
					break;
				default: return false;
			}

			string[] arCompareGroupLevel = new string[] { subject, index.ToString(), m_strCompareGroup };
			if (false == m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareGroupLevel)) return false;
			if (false == m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arCompareGroupLevel, false)) return false;

			for (int i = 0; i < arIndex.Length; ++i)
			{
				string[] arSaveGroup = new string[] { subject, index.ToString(), m_strCompareGroup, i.ToString() };
			if (false == m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arSaveGroup, false)) return false;
				m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_GROUP_PARAMTER_TYPE.ENABLE.ToString(), arEnable[i], ref arSaveGroup, false);
				m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_GROUP_PARAMTER_TYPE.NAME.ToString(), arName[i], ref arSaveGroup, false);

				FileUpdateCompareCondition(subject, index, i);
			}
			return m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
		}
		private bool FileUpdateCompareCondition(string subject, int index, int compareGroupIndex)
		{
			object[] arGroupKey = new object[] { };
			object[] arDevice = new object[] { };
			object[] arDeviceIndex = new object[] { };
			object[] arRelativeIndex = new object[] { };
			object[] arDirection = new object[] { };
			object[] arThreshold = new object[] { };
			switch (subject)
			{
				case m_strInterlockMotion:
					if (false == m_InstanceInterlock.GetMotionCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP, ref arGroupKey)) return false;
					if (false == m_InstanceInterlock.GetMotionCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE, ref arDevice)) return false;
					if (false == m_InstanceInterlock.GetMotionCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX, ref arDeviceIndex)) return false;
					if (false == m_InstanceInterlock.GetMotionCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX, ref arRelativeIndex)) return false;
					if (false == m_InstanceInterlock.GetMotionCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION, ref arDirection)) return false;
					if (false == m_InstanceInterlock.GetMotionCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD, ref arThreshold)) return false;
					break;
				case m_strInterlockCylinder:
					if (false == m_InstanceInterlock.GetCylinderCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP, ref arGroupKey)) return false;
					if (false == m_InstanceInterlock.GetCylinderCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE, ref arDevice)) return false;
					if (false == m_InstanceInterlock.GetCylinderCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX, ref arDeviceIndex)) return false;
					if (false == m_InstanceInterlock.GetCylinderCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX, ref arRelativeIndex)) return false;
					if (false == m_InstanceInterlock.GetCylinderCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION, ref arDirection)) return false;
					if (false == m_InstanceInterlock.GetCylinderCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD, ref arThreshold)) return false;
					break;
				case m_strInterlockDO:
					if (false == m_InstanceInterlock.GetDOCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP, ref arGroupKey)) return false;
					if (false == m_InstanceInterlock.GetDOCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE, ref arDevice)) return false;
					if (false == m_InstanceInterlock.GetDOCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX, ref arDeviceIndex)) return false;
					if (false == m_InstanceInterlock.GetDOCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX, ref arRelativeIndex)) return false;
					if (false == m_InstanceInterlock.GetDOCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION, ref arDirection)) return false;
					if (false == m_InstanceInterlock.GetDOCompareConditionList(index, compareGroupIndex, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD, ref arThreshold)) return false;
					break;
				default: return false;
			}

            string[] currentGroups = new string[] { };
			string[] upperGroup = new string[] { subject, index.ToString(), m_strCompareGroup, compareGroupIndex.ToString() };
            m_instanceStorage.GetListOfGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref currentGroups, upperGroup);
            foreach(string groupName in currentGroups)
			{
			    string[] deleteGroups = new string[] { subject, index.ToString(), m_strCompareGroup, compareGroupIndex.ToString(), groupName };
				if (false == m_instanceStorage.RemoveGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref deleteGroups)) return false;
			}

			for (int i = 0; i < arGroupKey.Length; ++i)
			{
				string[] arSaveGroup = new string[] { subject, index.ToString(), m_strCompareGroup, compareGroupIndex.ToString(), i.ToString() };
				if (false == m_instanceStorage.AddGroup(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, ref arSaveGroup, false)) return false;
				m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.KEY_GROUP.ToString(), arGroupKey[i], ref arSaveGroup, false);
				m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_TYPE.ToString(), arDevice[i], ref arSaveGroup, false);
				m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DEVICE_INDEX.ToString(), arDeviceIndex[i], ref arSaveGroup, false);
				m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.RELATIVE_DEVICE_INDEX.ToString(), arRelativeIndex[i], ref arSaveGroup, false);
				m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.DIRECTION.ToString(), arDirection[i], ref arSaveGroup, false);
				m_instanceStorage.AddItem(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK, EN_COMPARE_CONDITION_PARAMTER_TYPE.THRESHOLD.ToString(), arThreshold[i], ref arSaveGroup, false);
			}
			return m_instanceStorage.Save(Define.DefineEnumBase.Storage.EN_STORAGE_LIST.INTERLOCK);
		}

		#endregion
	}
}
