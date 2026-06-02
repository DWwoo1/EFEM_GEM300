using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using EFEM.Defines.LoadPort;

namespace EFEM.MaterialTracking
{
    public class Carrier : IMaterial
    {
        #region <Constructors>
        public Carrier(string key, int portId)
        {
            UniqueKey = key;
            CarrierId = key;
            LotId = string.Empty;
            PortId = portId;
            Capacity = 0;
            LoadTime = DateTime.Now;
            Extra = new Dictionary<string, string>();
            AccessingStatus = CarrierAccessStates.NotAccessed;
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly ConcurrentDictionary<int, CarrierSlotMapStates> _slotInformation
            = new ConcurrentDictionary<int, CarrierSlotMapStates>();

        private const string NameOfType = "Carrier";
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
        public int PortId { get; set; }
        public int Capacity { get; set; }
        public string LotId { get; set; }
        public string CarrierId { get; set; }
        public CarrierAccessStates AccessingStatus { get; set; }
        public DateTime LoadTime { get; set; }
        public DateTime UnloadTime { get; set; }
        public IReadOnlyDictionary<int, CarrierSlotMapStates> SlotMaps
        {
            get
            {
                return _slotInformation;
            }
        }
        public Dictionary<string, string> Extra { get; set; }
        #endregion </Properties>

        #region <Methods>

        #region <Carrier>
        public void SetCarrierId(string carrierId)
        {
            CarrierId = carrierId;
        }
        public void SetLotId(string lotId)
        {
            LotId = lotId;
        }
        public void SetAccessingStatus(CarrierAccessStates newState)
        {
            AccessingStatus = newState;
        }
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
        public void ClearAttributes()
        {
            if (Extra == null)
                return;

            var keys = Extra.Keys.ToArray();
            for (int i = 0; i < keys.Length; ++i)
            {
                var key = keys[i];
                Extra[key] = string.Empty;
            }
        }
        public void SetSlotMaps(IDictionary<int, CarrierSlotMapStates> map)
        {
            _slotInformation.Clear();
            foreach (var item in map)
            {
                _slotInformation[item.Key] = item.Value;
            }

            Capacity = _slotInformation.Count;
        }
        #endregion </Carrier>

        #endregion </Methods>
    }
}