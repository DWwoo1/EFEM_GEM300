using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Defines.MaterialTracking;
using EFEM.CustomizedByProcessType.PWA500Common;
using EFEM.MaterialTracking;

namespace EFEM.CustomizedByProcessType.PWA500BIN
{
    public class PWA500BINSubstrateExtraAttributes : IMaterialExtraAttribute
    {
        public IEnumerable<string> GetExtraKeys()
        {
            return new[]
            {
                PWA500SubstrateAttributes.SubstrateSize,
                PWA500SubstrateAttributes.SubstrateType,
                PWA500SubstrateAttributes.RingId,
                PWA500SubstrateAttributes.PartId,
                PWA500SubstrateAttributes.LotType,
                PWA500SubstrateAttributes.StepSeq,
                PWA500SubstrateAttributes.ChipQty,
                PWA500SubstrateAttributes.BinCode,
                PWA500SubstrateAttributes.RefPositionX,
                PWA500SubstrateAttributes.RefPositionY,
                PWA500SubstrateAttributes.StartingPositionX,
                PWA500SubstrateAttributes.StartingPositionY,
                PWA500SubstrateAttributes.CountX,
                PWA500SubstrateAttributes.CountY,
                PWA500SubstrateAttributes.Angle,
                PWA500SubstrateAttributes.MapData,
                PWA500SubstrateAttributes.ParentLotId,
                PWA500SubstrateAttributes.SplittedLotId,
                PWA500SubstrateAttributes.IsLastSubstrate,
                PWA500SubstrateAttributes.IsTrackOutCompleted,
                PWA500SubstrateAttributes.BinUnloadingStep,
                PWA500SubstrateAttributes.CoreLotId,
                PWA500SubstrateAttributes.CorePartId,
                PWA500SubstrateAttributes.SplittedHistory
            };
        }
        public void CreateAttributes(Dictionary<string, string> extra)
        {
            extra[PWA500SubstrateAttributes.SubstrateType] = SubstrateType.Empty.ToString();
            extra[PWA500SubstrateAttributes.RingId] = string.Empty;

            extra[PWA500SubstrateAttributes.PartId] = string.Empty;
            extra[PWA500SubstrateAttributes.LotType] = string.Empty;
            extra[PWA500SubstrateAttributes.StepSeq] = string.Empty;
            extra[PWA500SubstrateAttributes.ChipQty] = "0";
            extra[PWA500SubstrateAttributes.BinCode] = string.Empty;

            extra[PWA500SubstrateAttributes.RefPositionX] = string.Empty;
            extra[PWA500SubstrateAttributes.RefPositionY] = string.Empty;
            extra[PWA500SubstrateAttributes.StartingPositionX] = string.Empty;
            extra[PWA500SubstrateAttributes.StartingPositionY] = string.Empty;
            extra[PWA500SubstrateAttributes.CountX] = string.Empty;
            extra[PWA500SubstrateAttributes.CountY] = string.Empty;
            extra[PWA500SubstrateAttributes.Angle] = string.Empty;
            extra[PWA500SubstrateAttributes.MapData] = string.Empty;
            extra[PWA500SubstrateAttributes.ParentLotId] = string.Empty;
            extra[PWA500SubstrateAttributes.SplittedLotId] = string.Empty;
            extra[PWA500SubstrateAttributes.IsLastSubstrate] = bool.FalseString;
            extra[PWA500SubstrateAttributes.IsTrackOutCompleted] = bool.FalseString;
            extra[PWA500SubstrateAttributes.BinUnloadingStep] = "0";
            extra[PWA500SubstrateAttributes.CoreLotId] = string.Empty;
            extra[PWA500SubstrateAttributes.CorePartId] = string.Empty;
            extra[PWA500SubstrateAttributes.SplittedHistory] = string.Empty;
        }

        public void InitializeToPublish(Dictionary<string, string> extra, IMaterial material)
        {
            if (!(material is Substrate substrate))
                return;

            string substrateType = PWA500SubstrateAttributes.SubstrateType;
            switch (substrate.SourcePortId)
            {
                case 1:
                    extra[substrateType] = SubstrateType.Bin1.ToString();
                    break;
                case 2:
                    extra[substrateType] = SubstrateType.Bin2.ToString();
                    break;
                case 3:
                    extra[substrateType] = SubstrateType.Bin3.ToString();
                    break;
                case 4:
                    extra[substrateType] = SubstrateType.Empty.ToString();
                    break;
                case 5:
                case 6:
                    extra[substrateType] = SubstrateType.Core.ToString();
                    break;
                default:
                    break;
            }
        }
    }
}