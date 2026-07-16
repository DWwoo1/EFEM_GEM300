using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFEM.Defines.MaterialTracking
{
    #region <Enumerations>
    public enum TransportStates
    {
        AtSource = 0,
        AtWork = 1,
        AtDestination = 2
    }
    public enum ProcessingStates
    {
        NeedsProcessing = 0,
        InProcess = 1,
        Processed = 2,
        Aborted = 3,
        Stopped = 4,
        Rejected = 5,
        Lost = 6,
        Skipped = 7
    }
    // [영속화 enum] 저장은 이름으로. 멤버 재배치/삭제 금지 — 끝에만 추가.
    public enum IdReadingStates
    {
        NotConfirmed = 0,
        WaitingForHost = 1,
        Confirmed = 2,
        ConfirmationFailed = 3,
    }
    public enum OccupancyState
    {
        Unoccupied = 0,
        Occupied
    }
    public enum ReadResult
    {
        Succeed = 0,
        Failed
    }
    #endregion </Enumerations>

    #region <Class>
    //public class LocationInfo
    //{
    //    #region <Constructors>
    //    public LocationInfo(string location, int slot)
    //    {
    //        Location = location;
    //        Slot = slot;
    //    }
    //    #endregion </Constructors>

    //    #region <Properties>
    //    public string Location { get; set; }
    //    public int Slot { get; set; }
    //    #endregion </Properties>
    //}
    public static class ETC
    {
        public const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";
    }
    public static class BaseCarrierAttributeKeys
    {
        public const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";

        public const string UniqueKey = "UniqueKey";
        public const string LotId = "LotId";
        public const string CarrierId = "CarrierId";
        public const string CarrierAccessStatus = "CarrierAccessStatus";
        public const string LoadTime = "LoadTime";
        public const string UnloadTime = "UnloadTime";
    }
    public static class BaseSubstrateAttributeKeys
    {
        public const string UniqueKey = "UniqueKey";
        public const string Name = "Name";
        public const string OriginName = "OriginName";
        public const string Location = "Location";
        public const string SourcePortId = "SourcePortId";
        public const string SourceSlot = "SourceSlot";
        public const string SourceCarrierId = "SourceCarrierId";
        public const string CurrentCarrierKey = "CurrentCarrierKey";
        public const string DestinationPortId = "DestinationPortId";
        public const string DestinationSlot = "DestinationSlot";
        public const string LotId = "LotId";
        public const string RecipeId = "RecipeId";
        public const string ProcessJobId = "ProcessJobId";
        public const string ControlJobId = "ControlJobId";
        public const string TransPortState = "TransPortState";
        public const string ProcessingState = "ProcessingState";
        public const string IdReadingState = "IdReadingState";
        public const string DoNotProcessFlag = "DoNotProcessFlag";
        public const string Usage = "Usage";
    }

    public static class CarrierAttributeRecoveryDefines
    {
        public const string FileName = "CarrierInformation";
        public const string FileExtension = "xml";
        public const string FileRootName = "CarrierAttributeInformation";
        public static readonly string FileNameWithExtension = string.Format("{0}.{1}", FileName, FileExtension);
        public static readonly string FilePath = string.Format(@"{0}\..\Recovery\LP", Environment.CurrentDirectory);
    }
    #endregion </Class>
}