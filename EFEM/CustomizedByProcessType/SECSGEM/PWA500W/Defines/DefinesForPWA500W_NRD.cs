using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace EFEM.CustomizedByProcessType.PWA500W
{
    #region <Param Range>
    public class ParamRange : ParamRangeBase
    {
        public override int SvidStart { get { return 0; } }
        public override int SvidEnd { get { return 0; } }
        public override int EcidStart { get { return 100000; } }
        public override int EcidCommonStart { get { return 0; } }
        public override int EcidCommonEnd { get { return 0; } }
        public override int EcidEquipStart { get { return 100000; } }
        public override int EcidEquipEnd { get { return 199999; } }
        public override int EcidEnd { get { return 199999; } }
        public override int PreDefinedEcidStart { get { return 101; } }
        public override int PreDefinedEcidEnd { get { return 130; } }
    }
    #endregion </Param Range>

    #region <Enums>
    public enum EN_EVENT_LIST
    {
        EQUIPMENT_START,                    //
        EQUIPMENT_END,                      //
        PROCESS_START,                      //
        PROCESS_END,                        //
        ERROR_START,                        //
        ERROR_STOP,                         //
        PORT_STATUS_LOAD,                   //
        PORT_STATUS_UNLOAD,                 //
        CARRIER_LOAD,                       //
        CARRIER_UNLOAD,                     //
        RFID_READ_SUPPLY_PORT,              //
        RFID_READ_SORT_PORT,                //
        REQ_LOT_INFO_SUPPLY_PORT,           //
        REQ_LOT_INFO_SORT_PORT,             //
        REQ_SLOT_INFO_CORE_PORT,            //
        REQ_SLOT_INFO_SORT_PORT,            //
        REQ_RECIPE_DOWNLOAD,                //
        REQ_RECIPE_UPLOAD,                  //
        REQ_TRACK_IN,                       //
        REQ_LOT_MATCH,                      //
        REQ_BIN_WAFER_TRACK_OUT,            //
        BIN_PART_ID_INFO_REQ,               //
        SLOT_WAFER_MAPPING_CORE_PORT,       //
        SLOT_WAFER_MAPPING_SORT_PORT,       //
        REQ_LOT_MERGE_SORT,                 //
        REQ_LOT_ID_CHANGE_SORT,             //
        WORK_START,                         //
        WORK_END,                           //
        CORE_WAFER_ID_REQ,                  //
        REQ_CORE_WAFER_SPLIT,               //
        REQ_CORE_WAFER_SPLIT_LAST,          //
        CORE_WAFER_DETACH_START,            //
        CORE_WAFER_DETACH_END,              //
        REQ_CORE_CHIP_SPLIT_FIRST,          //
        REQ_CORE_CHIP_SPLIT,                //
        REQ_CORE_CHIP_FULL_SPLIT_FIRST,
        REQ_CORE_CHIP_FULL_SPLIT,
        REQ_CORE_CHIP_MERGE,                //
        BIN_WAFER_RING_ID_READ,             //
        BIN_WORK_END,                       //
        BIN_DATA_UPLOAD,                    //
        REQ_BIN_WAFER_ID_ASSIGN,            //
        REQ_BIN_WAFER_ID_CONFIRM,           //
        BIN_SORTING_START,                  //
        BIN_SORTING_END,                    //

        //PICK_UP_END,                        //
        //PLACE_END,                          //

        LOT_START,
        LOT_END,
        WAFER_START,
        WAFER_END,
        CHAMBER_START,
        CHAMBER_END,
        CORE_MAP_UPLOAD,
        STEP_START,
        STEP_END,
        BIN_MAP_UPLOAD,
        BIN_WAFER_END,
    }

    public enum EN_SVID_LIST
    {
        PORTID,
        CARRIERID,
        CARRIER_TYPE,
        STATUS,
        SLOTID,
        OPERID,
        LOTID,
        RECIPEID,
        PARTID,
        STEPSEQ,
        LOTTYPE,
        RECIPE_STEP,
        UNITID,
        MATERIAL_LOT_ID_TO_CONSUME,
        XML_FILENAME,
        XML_FILEBODY,
        PMS_FILENAME,
        PMS_FILEBODY,
        WAFERID,
        SORTING_INFO,
        SPLIT_WAFERID,
        SCRAP_QTY,
        SCRAP_INFO,
        PICKER_COLLET_1,
        PICKER_COLLET_2,
        EJECT_HOOD_ID,
        CHANGE_REASON,
        MATERIAL_TYPE,
        RINGFRAME_ID,
        ASSIGNED_WAFERID,
        BIN_TYPE,
        CHIP_QTY,
        SLOT1_WAFERID,
        SLOT2_WAFERID,
        SLOT3_WAFERID,
        SLOT4_WAFERID,
        SLOT5_WAFERID,
        SLOT6_WAFERID,
        SLOT7_WAFERID,
        SLOT8_WAFERID,
        SLOT9_WAFERID,
        SLOT10_WAFERID,
        SLOT11_WAFERID,
        SLOT12_WAFERID,
        SLOT13_WAFERID,
        SLOT14_WAFERID,
        SLOT15_WAFERID,
        SLOT16_WAFERID,
        SLOT17_WAFERID,
        SLOT18_WAFERID,
        SLOT19_WAFERID,
        SLOT20_WAFERID,
        SLOT21_WAFERID,
        SLOT22_WAFERID,
        SLOT23_WAFERID,
        SLOT24_WAFERID,
        SLOT25_WAFERID,
        SLOT1_WAFER_CHIP_QTY,
        SLOT2_WAFER_CHIP_QTY,
        SLOT3_WAFER_CHIP_QTY,
        SLOT4_WAFER_CHIP_QTY,
        SLOT5_WAFER_CHIP_QTY,
        SLOT6_WAFER_CHIP_QTY,
        SLOT7_WAFER_CHIP_QTY,
        SLOT8_WAFER_CHIP_QTY,
        SLOT9_WAFER_CHIP_QTY,
        SLOT10_WAFER_CHIP_QTY,
        SLOT11_WAFER_CHIP_QTY,
        SLOT12_WAFER_CHIP_QTY,
        SLOT13_WAFER_CHIP_QTY,
        SLOT14_WAFER_CHIP_QTY,
        SLOT15_WAFER_CHIP_QTY,
        SLOT16_WAFER_CHIP_QTY,
        SLOT17_WAFER_CHIP_QTY,
        SLOT18_WAFER_CHIP_QTY,
        SLOT19_WAFER_CHIP_QTY,
        SLOT20_WAFER_CHIP_QTY,
        SLOT21_WAFER_CHIP_QTY,
        SLOT22_WAFER_CHIP_QTY,
        SLOT23_WAFER_CHIP_QTY,
        SLOT24_WAFER_CHIP_QTY,
        SLOT25_WAFER_CHIP_QTY,
        SLOT1_WAFER_LOT_ID,
        SLOT2_WAFER_LOT_ID,
        SLOT3_WAFER_LOT_ID,
        SLOT4_WAFER_LOT_ID,
        SLOT5_WAFER_LOT_ID,
        SLOT6_WAFER_LOT_ID,
        SLOT7_WAFER_LOT_ID,
        SLOT8_WAFER_LOT_ID,
        SLOT9_WAFER_LOT_ID,
        SLOT10_WAFER_LOT_ID,
        SLOT11_WAFER_LOT_ID,
        SLOT12_WAFER_LOT_ID,
        SLOT13_WAFER_LOT_ID,
        SLOT14_WAFER_LOT_ID,
        SLOT15_WAFER_LOT_ID,
        SLOT16_WAFER_LOT_ID,
        SLOT17_WAFER_LOT_ID,
        SLOT18_WAFER_LOT_ID,
        SLOT19_WAFER_LOT_ID,
        SLOT20_WAFER_LOT_ID,
        SLOT21_WAFER_LOT_ID,
        SLOT22_WAFER_LOT_ID,
        SLOT23_WAFER_LOT_ID,
        SLOT24_WAFER_LOT_ID,
        SLOT25_WAFER_LOT_ID,
        EFEM_MAIN_CDA_PRESSURE,
        EFEM_MAIN_VAC_PRESSURE,
        ROBOT_CDA_PRESSURE,
        IONIZER_PRESSURE,
        IONIZER_FLOW_METER_1,
        IONIZER_FLOW_METER_2,
        IONIZER_FLOW_METER_3,
        EFEM_FFU_SPEED_1,
        EFEM_FFU_SPEED_2,
        EFEM_FFU_SPEED_3,
        EFEM_FFU_SPEED_4,
        SUPPLY_BUFFER_IONIZER_FLOW,
        SORTING_BUFFER_IONIZER_FLOW,
        SUPPLY_STAGE_IONIZER_FLOW,
        SORTING_STAGE_IONIZER_FLOW,
        PM_FFU_SPEED_1,
        PM_FFU_SPEED_2,
        EJECT_MEMBRANE_AIR_REGULATOR,
        EJECT_MEMBRANE_VAC_PRESS,
        EJECT_VAC_PRESS,
        NEEDLE_HEIGHT,
        EXPENSION_HEIGHT,
        PICK_SEARCH_LEVEL,
        PICK_SEARCH_SPEED,
        PICK_DELAY,
        PICK_FORCE,
        PICK_SLOWUP_LEVEL,
        PICK_SLOWUP_SPEED,
        PLACE_SEARCH_LEVEL,
        PLACE_SEARCH_SPEED,
        PLACE_DELAY,
        PLACE_FORCE,
        PLACE_SLOWUP_LEVEL,
        PLACE_SLOWUP_SPEED,

        PortID,
    }

    
    // MoveMaterial, MaterialMoved의 Location에서 사용되는 장소
    public enum MaterialMovingLocationsAtProcessModule
    {
        SUPPLY,
        CENTER,
        LEFT,
        RIGHT,
        SUPPLY_IN_SHUTTLE,
        SUPPLY_IN_BUFFER,
        SUPPLY_OUT_BUFFER,
        SUPPLY_OUT_SHUTTLE,
        SORTING_IN_BUFFER,
        SORTING_OUT_BUFFER,
    }
    #endregion </Class>
}