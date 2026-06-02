using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3.Recipe
{
    /// <summary>
    /// 2020.06.29 by yjlee [ADD] Enumerate the common parameters.
    /// </summary>
    public enum PARAM_COMMON
    {
        PROCESS_FILE_PATH,
        PROCESS_FILE_NAME,

        UseSecsGem,
        UseDownloadingRecipe,

        UseCycleMode,
        UseUtilityAlarm,

        PIO_Software_Version,
        UseAccessViolation,

        // 2025.09.24 dwlim [ADD] E84에 명시된 Timeout 추가
        TP1,    // L/U REQ ON ~ TR_REQ ON 까지(2sec)
        TP2,    // READY ON ~ BUSY ON 까지(2sec)
        TP3,    // BUSY ON ~ CARRIER ON/OFF 까지(60sec)
        TP4,    // L/U REQ OFF ~ BUSY OFF 까지(60sec)
        TP5,    // READY OFF ~ VALID OFF 까지(2sec)

        TD3,

        TC1,
        TC2,
        // 2025.09.24 dwlim [END]
    }

    /// <summary>
    /// 2020.06.29 by yjlee [ADD] Enumerate the equipment parameters.
    /// </summary>
    public enum PARAM_EQUIPMENT
    {
		MachineLanguage,
		MachineName,
        UnlockParameterChange,
        RAM_METRICS_EXPORT_PATH,

        UseRobotUpperArm,
        UseRobotLowerArm,

        UseLoadPort1,
        UseLoadPort2,
        UseLoadPort3,
        UseLoadPort4,
        UseLoadPort5,
        UseLoadPort6,

        LoadPortType1,
        LoadPortType2,
        LoadPortType3,
        LoadPortType4,
        LoadPortType5,
        LoadPortType6,

        LoadPortSize1,
        LoadPortSize2,
        LoadPortSize3,
        LoadPortSize4,
        LoadPortSize5,
        LoadPortSize6,

        UseSlotValidationResult1,
        UseSlotValidationResult2,
        UseSlotValidationResult3,
        UseSlotValidationResult4,
        UseSlotValidationResult5,
        UseSlotValidationResult6,

        UseCapacityLimitBin1,
        UseCapacityLimitBin2,
        UseCapacityLimitBin3,

        AvailableCarrierCapacityBin1,
        AvailableCarrierCapacityBin2,
        AvailableCarrierCapacityBin3,

        HandlingWaitTime,
        HandlingRequestDelayEachLoadPorts,
        BinWaferStepId,
        RFIDTaggingRetryLimit,

        WrittingLotIdToMACWhenLotIsTerminated,
        WrittingLotIdToCassetteWhenLotIsTerminated,

        WrittingLotIdToMACWhenCarrierIsEmpty,
        WrittingLotIdToCassetteWhenCarrierIsEmpty,

        //WrittingLotIdToMACWhenLotIsTerminated,
        //WrittingLotIdToMACWhenCarrierIsEmpty,
        //WrittingLotIdToCassetteWhenLotIsTerminated,
        //WrittingLotIdToCassetteWhenCarrierIsEmpty,
        //WrittingLotIdToClosedCassetteWhenLotIsTerminated,
        //WrittingLotIdToClosedCassetteWhenCarrierIsEmpty,
    }
}