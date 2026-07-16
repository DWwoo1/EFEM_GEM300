using System.Collections.Generic;

using EFEM.MaterialTracking;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    public class PWA500CarrierExtraAttributes : IMaterialExtraAttribute
    {
        public IEnumerable<string> GetExtraKeys()
        {
            return new[]
            {
                PWA500CarrierAttributes.KeyPartId,
                PWA500CarrierAttributes.KeyStepSeq,
                PWA500CarrierAttributes.KeyLotType,
                PWA500CarrierAttributes.KeyLotQty,
                PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier,
                PWA500CarrierAttributes.KeyTrackInCompleted,
                PWA500CarrierAttributes.KeyDownloadingRecipeCompleted,
                PWA500CarrierAttributes.KeyLotIdToWrite
            };
        }
        public void CreateAttributes(Dictionary<string, string> extra)
        {
            extra[PWA500CarrierAttributes.KeyPartId] = string.Empty;
            extra[PWA500CarrierAttributes.KeyStepSeq] = string.Empty;
            extra[PWA500CarrierAttributes.KeyLotQty] = uint.MinValue.ToString();
            extra[PWA500CarrierAttributes.KeyProcessStepBeforeSendingCarrier] = uint.MinValue.ToString();

            extra[PWA500CarrierAttributes.KeyTrackInCompleted] = bool.FalseString;
            extra[PWA500CarrierAttributes.KeyDownloadingRecipeCompleted] = bool.FalseString;
            extra[PWA500CarrierAttributes.KeyLotIdToWrite] = string.Empty;
        }

        public void InitializeToPublish(Dictionary<string, string> extra, IMaterial material)
        {
            if (!(material is Carrier carrier))
                return;

            // 할게 있으면 여기서 한다..
        }
    }
}