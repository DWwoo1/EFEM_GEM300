using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3.SECSGEM.DefineSecsGem
{
	public enum EN_ACTION_DATA
	{
		AlarmCode,
		AlarmType,
	}

    #region <ParamRanges>
	public class PARAM_RANGE
	{
		#region singleton
		private static PARAM_RANGE _Instance = null;
		public static PARAM_RANGE GetInstance()
		{
			if (_Instance == null)
			{
				_Instance = new PARAM_RANGE();
			}
			return _Instance;
		}
		private PARAM_RANGE()
		{
			ParamRangeBase paramRange = null;
            switch (Work.AppConfigManager.Instance.Customer)
            {
                case Define.DefineEnumProject.AppConfig.EN_CUSTOMER.S_TP:
					paramRange = new EFEM.CustomizedByProcessType.PWA500BIN.ParamRange();
					break;
                default:
                    break;
            }
            //switch(Work.AppConfigManager.Instance.Customer)
            //{
            //	case Define.DefineEnumProject.Common.EN_CUSTOMER.Simmtech:
            //		paramRange = new Simmtech.ParamRange();
            //		break;
            //	case Define.DefineEnumProject.Common.EN_CUSTOMER.SiliconBox:
            //		paramRange = new Siliconbox.ParamRange();
            //		break;
            //}

            if (paramRange == null)
				return;

			SVID_START = paramRange.SvidStart;
			SVID_END = paramRange.SvidEnd;

			ECID_START = paramRange.EcidStart;
			ECID_COMMON_START = paramRange.EcidCommonStart;
			ECID_COMMON_END = paramRange.EcidCommonEnd;
			ECID_EQUIP_START = paramRange.EcidEquipStart;
			ECID_EQUIP_END = paramRange.EcidEquipEnd;
			ECID_END = paramRange.EcidEnd;

            PRE_DEFINED_ECID_START = paramRange.PreDefinedEcidStart;
            PRE_DEFINED_ECID_END = paramRange.PreDefinedEcidEnd;
		}
		#endregion /singleton

		public readonly int SVID_START = 0;
		public readonly int SVID_END = 0;
			   
		public readonly int ECID_START = 0;
		public readonly int ECID_COMMON_START = 0;
		public readonly int ECID_COMMON_END = 0;
		public readonly int ECID_EQUIP_START = 0;
		public readonly int ECID_EQUIP_END = 0;
		public readonly int ECID_END = 0;
        
        public readonly int PRE_DEFINED_ECID_START = 0;
        public readonly int PRE_DEFINED_ECID_END = 0;
	}
	public abstract class ParamRangeBase
	{
		public abstract int SvidStart { get; }
		public abstract int SvidEnd { get; }
		public abstract int EcidStart { get; }
		public abstract int EcidCommonStart { get; }
		public abstract int EcidCommonEnd { get; }
		public abstract int EcidEquipStart { get; }
		public abstract int EcidEquipEnd { get; }
		public abstract int EcidEnd { get; }

        public abstract int PreDefinedEcidStart { get; }
        public abstract int PreDefinedEcidEnd { get; }
	}
    #endregion </ParamRanges>
    public class CollectionEvents
    {
        #region <Constructors>
        public CollectionEvents()
        {
			//switch (Work.AppConfigManager.Instance.Customer)
			//{
			//	case Define.DefineEnumProject.Common.EN_CUSTOMER.Simmtech:
			//		{
			//			var list = (Simmtech.EN_EVENT_LIST[])Enum.GetValues(typeof(Simmtech.EN_EVENT_LIST));

			//			_events = new long[list.Length];
			//			int index = 0;
			//			foreach (var item in list)
			//			{
			//				_events[index++] = (long)item;
			//			}
			//		}
			//		break;
			//	case Define.DefineEnumProject.Common.EN_CUSTOMER.SiliconBox:
			//		{
			//			var list = (Siliconbox.EN_EVENT_LIST[])Enum.GetValues(typeof(Siliconbox.EN_EVENT_LIST));

			//			_events = new long[list.Length];
			//			int index = 0;
			//			foreach (var item in list)
			//			{
			//				_events[index++] = (long)item;
			//			}
			//		}
			//		break;
			//}
        }
        #endregion </Constructors>

        #region <Types>
        #endregion </Types>

        #region <Fields>
        private static CollectionEvents _instance = null;
        private long[] _events = null;
        #endregion </Fields>

        #region <Properties>
        public static CollectionEvents Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CollectionEvents();
                }

                return _instance;
            }
        }

        public long[] Events
        {
            get
            {
                return _events;
            }
        }
        #endregion </Properties>

        #region <Methods>
        #endregion </Methods>
    }
}