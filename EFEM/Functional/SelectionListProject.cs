using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Define.DefineEnumProject.SelectionList;

namespace FrameOfSystem3.Functional
{
	public partial class SelectionList
	{
        private enum WaferType500Bin
        {
            Core,
            Empty,
            StageCenter,
            StageLeft,
            StageRight,
            //Bin1,
            //Bin2,
            //Bin3
        }

        // 2025.02.08. by dwlim [MOD] 
        private enum WaferType500W
        {
            Core,
            Empty,
            Bin1
        }

        private enum WaferSize
        {
            Inch_8,            // 8 inch
            Inch_12,            // 12 inch
        }
        private void MakeListByProjectEnum()
		{  
            m_DicOfList.Add(EN_SELECTIONLIST.ARM_TYPE, Enum.GetNames(typeof(EFEM.Defines.AtmRobot.RobotArmTypes)));
            m_DicOfList.Add(EN_SELECTIONLIST.WAFER_TYPE_BIN, Enum.GetNames(typeof(WaferType500Bin)));
            m_DicOfList.Add(EN_SELECTIONLIST.WAFER_TYPE_500W, Enum.GetNames(typeof(WaferType500W)));
            m_DicOfList.Add(EN_SELECTIONLIST.WAFER_SIZE, Enum.GetNames(typeof(WaferSize)));

            m_DicOfList.Add(EN_SELECTIONLIST.SUBSTRATE_TRANSFER_STATE, Enum.GetNames(typeof(EFEM.Defines.MaterialTracking.TransportStates)));
            m_DicOfList.Add(EN_SELECTIONLIST.SUBSTRATE_PROCESSING_STATE, Enum.GetNames(typeof(EFEM.Defines.MaterialTracking.ProcessingStates)));
            m_DicOfList.Add(EN_SELECTIONLIST.SUBSTRATE_ID_READING_STATE, Enum.GetNames(typeof(EFEM.Defines.MaterialTracking.IdReadingStates)));
            m_DicOfList.Add(EN_SELECTIONLIST.SUBSTRATE_TYPE, Enum.GetNames(typeof(EFEM.CustomizedByProcessType.PWA500Common.SubstrateType)));
        }
    }
}
