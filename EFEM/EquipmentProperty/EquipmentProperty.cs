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
    #region Enum
    public enum EN_EQUIPMENT_PROPERTY_LIST
    {
        MATERIAL_EXIST,
        ACTION_RUNNING,
    }

    #region Property Values
    public enum EN_MATERIAL_EXIST_VALUES
    {
        UNDEFINED   =   -1,
        EMPTY       =    0,
        EXIST       =    1,
    }

    public enum EN_ACTION_RUNNING_VALUES
    {
        UNDEFINED   =   -1,
        STOP        =    0,
        RUNNING     =    1,
    }
    #endregion Property Values
    #endregion Enum

    public delegate void DelegateUpdateProperty();

    public class EquipmentProperty : DesignPattern_.Observer_.Subject
    {
        #region 싱글톤
        private EquipmentProperty() 
        {
            m_dicOfProperty = new Dictionary<EN_EQUIPMENT_PROPERTY_LIST, int>();
            foreach (EN_EQUIPMENT_PROPERTY_LIST en in Enum.GetValues(typeof(EN_EQUIPMENT_PROPERTY_LIST)))
            {
                m_dicOfProperty.Add(en, -1);
            }
            GetValue(EN_EQUIPMENT_PROPERTY_LIST.MATERIAL_EXIST);
        }

        private static readonly Lazy<EquipmentProperty> lazyInstance = new Lazy<EquipmentProperty>(() => new EquipmentProperty());
        static public EquipmentProperty GetInstance() 
        {
            return lazyInstance.Value; 
        }
        #endregion

        #region Field
        Dictionary<EN_EQUIPMENT_PROPERTY_LIST, int> m_dicOfProperty;
        #endregion

        #region Method
        #region Get
        #region Integral
        public int GetValue(EN_EQUIPMENT_PROPERTY_LIST enProperty)
        {
            return m_dicOfProperty[enProperty];
        }

        public int[] GetValuesArray()
        {
            return m_dicOfProperty.Values.ToArray();
        }
        #endregion /Integral

        #region Enum
        public EN_MATERIAL_EXIST_VALUES GetValueMaterialExist()
        {
            return (EN_MATERIAL_EXIST_VALUES)m_dicOfProperty[EN_EQUIPMENT_PROPERTY_LIST.MATERIAL_EXIST];
        }
        public EN_ACTION_RUNNING_VALUES GetValueActionRunning()
        {
            return (EN_ACTION_RUNNING_VALUES)m_dicOfProperty[EN_EQUIPMENT_PROPERTY_LIST.ACTION_RUNNING];
        }
        #endregion /Enum
        #endregion /Get

        public bool SetValue(EN_EQUIPMENT_PROPERTY_LIST enProperty, Enum tValue)
        {
            if (m_dicOfProperty[enProperty].Equals(tValue))
                return false;

            switch (enProperty)
            {
                case EN_EQUIPMENT_PROPERTY_LIST.MATERIAL_EXIST:
                    if (tValue.GetType() != typeof(EN_MATERIAL_EXIST_VALUES))
                        return false;
                    else
                        m_dicOfProperty[enProperty] = (int)(EN_MATERIAL_EXIST_VALUES)tValue;
                    break;

                case EN_EQUIPMENT_PROPERTY_LIST.ACTION_RUNNING:
                    if (tValue.GetType() != typeof(EN_ACTION_RUNNING_VALUES))
                        return false;
                    else
                        m_dicOfProperty[enProperty] = (int)(EN_ACTION_RUNNING_VALUES)tValue;
                    break;
            }

            Notify();
            return true;
        }
        #endregion

        public DelegateUpdateProperty delegateUpdateProperty = null;

        public void Execute()
        {
            if (delegateUpdateProperty != null)
                delegateUpdateProperty();
        }
    }
}

