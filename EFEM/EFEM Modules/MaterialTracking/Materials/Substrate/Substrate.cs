using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Threading;

using FrameOfSystem3.Work;
using EFEM.Defines.Common;
using EFEM.Defines.MaterialTracking;

namespace EFEM.MaterialTracking
{
    public class Substrate : IMaterial
    {
        #region <Constructors>
        public Substrate(string uniqueKey, string name)
        {
            UniqueKey = uniqueKey;
            Name = name;
            OriginName = name;

            Extra = new Dictionary<string, string>();

            // CarrierId
            // Port
            // Slot
            // DestPort
            // Dest Slot
            // CJ, PJ
            // Recipe Id
            // Transport State
            // Processing State
            // Lot Id
            // Usage
            // DoNotProcessFlag
        }
        #endregion </Constructors>

        #region <Fields>
        private const string NameOfType = "Substrate";
        #endregion </Fields>

        #region <Properties>
        public string MaterialType
        {
            get
            {
                return NameOfType;
            }
        }
        public string UniqueKey { get; set; }
        public string Name { get; set; }
        public string OriginName { get; set; }  // For traceability, the name of the material when it is created. It will not be changed even if the material is renamed.
        public string LocationId { get; set; }
        public int SourcePortId { get; set; }
        public int SourceSlot { get; set; }
        public string SourceCarrierId { get; set; }
        public string CurrentCarrierKey { get; set; }
        public int DestinationPortId { get; set; }
        public int DestinationSlot { get; set; } 
        public string LotId { get; set; } = string.Empty;
        public string RecipeId { get; set; } = string.Empty;
        public string ProcessJobId { get; set; } = string.Empty;
        public string ControlJobId { get; set; } = string.Empty;
        public TransportStates TransportStatus { get; set; }
        public ProcessingStates ProcessingStatus { get; set; }
        public IdReadingStates IdReadingStatus { get; set; }
        public bool DoNotProcessFlag { get; set; }
        public bool Usage { get; set; }
        public Dictionary<string, string> Extra { get; set; }
        #endregion </Properties>

        #region <Methods>
        public string GetAttribute(string key)
        {
            if (Extra == null)
                return string.Empty;

            if (false == Extra.TryGetValue(key, out string value))
                return string.Empty;

            return value;
        }
        public void SetAttribute(string key, string value)
        {
            if (Extra == null)
                return;

            if (value == null)
                value = string.Empty;

            Extra[key] = value;
        }
        #endregion </Methods>
    }
}