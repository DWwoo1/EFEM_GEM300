using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

using EFEM.Modules;
using EFEM.Defines.LoadPort;
using EFEM.MaterialTracking;
using EFEM.Defines.MaterialTracking;

namespace FrameOfSystem3.Views.MapManager
{
    public delegate void DelegateCellClicked(int clickedMapIndex, Queue<int> points);

    class CarrierMapManager
    {
        #region <Constructors>
        private CarrierMapManager()
        {
            _loadPortManager = LoadPortManager.Instance;
            _carrierMapControls = new ConcurrentDictionary<int, CarrierMap>();
            for (int i = 0; i < _loadPortManager.Count; ++i)
            {
                _carrierMapControls.TryAdd(i, new CarrierMap(i));
            }
        }
        #endregion </Constructors>

        #region <Fields>
        private static readonly CarrierMapManager _instance = new CarrierMapManager();
        private static LoadPortManager _loadPortManager = null;
        private readonly ConcurrentDictionary<int, CarrierMap> _carrierMapControls = null;
        #endregion </Fields>

        #region <Properties>
        public static CarrierMapManager Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion </Properties>

        #region <Methods>
        public void AssignMapControls(int lpIndex, ref Sys3Controls.Sys3Map mapControl, DelegateCellClicked callbackCellClicked = null)
        {
            if (_carrierMapControls.TryGetValue(lpIndex, out var map))
            {
                map.AddMap(ref mapControl, callbackCellClicked);
            }
        }

        public void UpdateControls(int lpIndex)
        {
            if (_carrierMapControls.TryGetValue(lpIndex, out var map))
            {
                map.RefreshMaps();
            }
        }
        public void DisableHighlight(int lpIndex, ref Sys3Controls.Sys3Map map)
        {
            if (_carrierMapControls.TryGetValue(lpIndex, out var cm))
                cm.DisableHighlight(ref map);
        }
        #endregion </Methods>
    }

    class CarrierMap
    {
        #region <Constructors>
        public CarrierMap(int lpIndex)
        {
            Index = lpIndex;
            _loadPortManager = LoadPortManager.Instance;
            _carrierServer = CarrierManagementServer.Instance;
            _substrateManager = SubstrateManager.Instance;

            PortId = _loadPortManager.GetLoadPortPortId(Index);
            //_temporaryCarrier = new Carrier(PortId);

            _mapControls = new List<Sys3Controls.Sys3Map>();
            _controlVisibilities = new List<bool>();

            _slotColors = new Dictionary<CarrierSlotMapStates, Color>();
            _processingColors = new Dictionary<ProcessingStates, Color>();
            _substrateTransferColors = new Dictionary<TransportStates, Color>();

            InitColors();

            _locking = new object();
            _eventHandlers = new Dictionary<Sys3Controls.Sys3Map, DelegateCellClicked>();
            _callbackCellClicked = new Sys3Controls.Sys3Map.DelegateGettingCellCoordinatesWithMap(CallbackCellClicked);
            _clickedCell = new Queue<int>();
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly int Index;
        private readonly int PortId;
        //private Carrier _carrier = null;
        //private Carrier _temporaryCarrier = null;
        private static LoadPortManager _loadPortManager = null;
        private static CarrierManagementServer _carrierServer = null;
        private static SubstrateManager _substrateManager = null;

        private readonly List<Sys3Controls.Sys3Map> _mapControls = null;
        private readonly List<bool> _controlVisibilities = null;
        private readonly Dictionary<Sys3Controls.Sys3Map, DelegateCellClicked> _eventHandlers = null;
        private readonly Dictionary<CarrierSlotMapStates, Color> _slotColors = null;
        private readonly Dictionary<ProcessingStates, Color> _processingColors = null;
        private readonly Dictionary<TransportStates, Color> _substrateTransferColors = null;
        private readonly Queue<int> _clickedCell = null;
        private readonly object _locking;
        private int _refreshInFlight;

        private readonly Dictionary<int, MapData> _dataToUpdate = new Dictionary<int, MapData>();

        private Color _temporaryColor;
        private Sys3Controls.Sys3Map.DelegateGettingCellCoordinatesWithMap _callbackCellClicked = null;
        #endregion </Fields>

        #region <Methods>

        private void InitColors()
        {
            Color colorNormal = Color.Silver;
            var colors = (CarrierSlotMapStates[])Enum.GetValues(typeof(CarrierSlotMapStates));
            foreach (var item in colors)
            {
                Color color = Color.Transparent;
                switch (item)
                {
                    case CarrierSlotMapStates.Undefined:
                    case CarrierSlotMapStates.Empty:
                        color = Color.White;
                        break;
                    case CarrierSlotMapStates.NotEmpty:
                        color = Color.DarkViolet;
                        break;
                    case CarrierSlotMapStates.CorrectlyOccupied:
                        color = colorNormal;
                        break;
                    case CarrierSlotMapStates.DoubleSlotted:
                        color = Color.DarkViolet;
                        break;
                    case CarrierSlotMapStates.CrossSlotted:
                        color = Color.Brown;
                        break;
                    default:
                        break;
                }

                if (color.Equals(Color.Transparent))
                    continue;

                _slotColors.Add(item, color);
            }

            var colors2 = (ProcessingStates[])Enum.GetValues(typeof(ProcessingStates));
            foreach (var item in colors2)
            {
                Color color = Color.Transparent;
                switch (item)
                {
                    case ProcessingStates.NeedsProcessing:
                        color = colorNormal;
                        break;
                    case ProcessingStates.InProcess:
                        color = Color.Blue;
                        break;
                    case ProcessingStates.Processed:
                        color = Color.Green;
                        break;
                    case ProcessingStates.Rejected:
                        color = Color.Orange;
                        break;
                    case ProcessingStates.Stopped:
                    case ProcessingStates.Aborted:
                    case ProcessingStates.Skipped:
                        color = Color.LightYellow;
                        break;
                    case ProcessingStates.Lost:
                        color = Color.Red;
                        break;
                    default:
                        break;
                }

                if (color.Equals(Color.Transparent))
                    continue;

                _processingColors.Add(item, color);
            }

            var colors3 = (TransportStates[])Enum.GetValues(typeof(TransportStates));
            foreach (var item in colors3)
            {
                Color color = Color.Transparent;
                switch (item)
                {
                    case TransportStates.AtSource:
                        color = colorNormal;
                        break;
                    case TransportStates.AtWork:
                        color = Color.Blue;
                        break;
                    case TransportStates.AtDestination:
                        color = Color.LimeGreen;
                        break;
                    default:
                        break;
                }

                if (color.Equals(Color.Transparent))
                    continue;

                _substrateTransferColors.Add(item, color);
            }
        }

        public void AddMap(ref Sys3Controls.Sys3Map map, DelegateCellClicked cellClicked = null)
        {
            if (cellClicked != null)
            {
                map.SetCallbackFunctionForGettingCell(ref _callbackCellClicked);
                _eventHandlers[map] = cellClicked;
            }

            _mapControls.Add(map);
            _controlVisibilities.Add(map.Visible);
        }

        public void DisableHighlight(ref Sys3Controls.Sys3Map map)
        {
            for (int i = 0; i < _mapControls.Count; ++i)
            {
                if (_mapControls[i].Equals(map))
                {
                    var ctrl = _mapControls[i];
                    OnUi(ctrl, () => ctrl.SetSingleCellHighlighted(-1, -1));
                    break;
                }
            }
        }

        public void RefreshMaps()
        {
            if (Interlocked.Exchange(ref _refreshInFlight, 1) == 1)
                return;
            try
            {
                if (false == _carrierServer.HasCarrier(PortId) || _carrierServer.GetCapacity(PortId) == 0)
                    InitializeMap();
                else
                    UpdateMap();
            }
            finally
            {
                Interlocked.Exchange(ref _refreshInFlight, 0);
            }
        }

        //public void RefreshMaps()
        //{
        //    if (false == _carrierServer.HasCarrier(PortId))
        //    {
        //        InitializeMap();
        //    }
        //    else
        //    {
        //        UpdateMap();
        //    }
        //}

        private void InitializeMap()
        {
            // 캐리어가 없는 경우 맵 초기화
            for (int i = 0; i < _mapControls.Count; ++i)
            {
                if (_mapControls[i] == null)
                    continue;

                var ctrl = _mapControls[i];
                OnUi(ctrl, () =>
                {
                    if (i < _controlVisibilities.Count && _controlVisibilities[i] != ctrl.Visible)
                        _controlVisibilities[i] = ctrl.Visible;
                    
                    // Visible 상태 상관없이 초기화한다. -> 보이지 않는 페이지는 자재가 제거되어도 남아있음
                    //if (false == ctrl.Visible) return;

                    if (ctrl.MapSize.Height != 1)
                    {
                        ctrl.MapSize = new Size(1, 1);
                        ctrl.SetCellColor(0, 0, ctrl.BackGroundColor, false);
                        ctrl.Invalidate();
                    }
                });
            }
        }

        private void UpdateMap()
        {
            var slotStates = _carrierServer.GetCarrierSlotMap(PortId);
            if (slotStates == null || slotStates.Count <= 0)
                return;

            int capacity = slotStates.Count;

            Color[] colors = new Color[capacity];
            string[] texts = new string[capacity];

            lock (_locking)
            {
                foreach (var item in slotStates)
                {
                    var slot = item.Key;
                    if (false == _dataToUpdate.TryGetValue(item.Key, out var md) || md == null)
                    {
                        md = new MapData();
                        _dataToUpdate[slot] = md;
                    }

                    var state = slotStates[slot];

                    if (false == _substrateManager.HasSubstrateAtLoadPort(PortId, slot))
                    {
                        md.CellColor = SubstrateMapper.GetColorBySlotStatus(CarrierSlotMapStates.Empty);
                        md.CellText = _substrateManager.GetSubstrateNameByDestinationPortId(PortId, slot);
                    }
                    else
                    {
                        if (LocationServer.GetLoadPortLocation(PortId, slot, out var lpLocation))
                        {
                            var transferStatus = TransportStates.AtSource;
                            var processingStatus = ProcessingStates.NeedsProcessing;
                            switch (state)
                            {
                                case CarrierSlotMapStates.Undefined:
                                case CarrierSlotMapStates.NotEmpty:
                                case CarrierSlotMapStates.DoubleSlotted:
                                case CarrierSlotMapStates.CrossSlotted:
                                    {
                                        md.CellColor = SubstrateMapper.GetColorBySlotStatus(state);
                                        md.CellText = state.ToString();
                                    }
                                    break;
                                case CarrierSlotMapStates.Empty:
                                case CarrierSlotMapStates.CorrectlyOccupied:
                                    {
                                        if (_substrateManager.GetTransferStatusAtLoadPort(PortId, slot, ref transferStatus) &&
                                            _substrateManager.GetProcessingStatusAtLoadPort(PortId, slot, ref processingStatus))
                                        {
                                            md.CellColor = SubstrateMapper.GetColorBySubstrateStatus(transferStatus, processingStatus);
                                            md.CellText = _substrateManager.GetSubstrateNameAtLoadPort(lpLocation.PortId, lpLocation.Slot);
                                        }
                                        else
                                        {
                                            md.CellColor = SubstrateMapper.GetColorBySlotStatus(state);
                                            md.CellText = _substrateManager.GetSubstrateNameByDestinationPortId(lpLocation.PortId, lpLocation.Slot);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            md.CellColor = SubstrateMapper.GetColorBySlotStatus(state);
                            md.CellText = string.Empty;
                        }
                    }

                    var arrayIdx = slot - 1;
                    if (arrayIdx >= 0)
                    {
                        if (colors.Length > arrayIdx)
                        {
                            colors[arrayIdx] = md.CellColor;
                        }

                        if (texts.Length > arrayIdx)
                        {
                            texts[arrayIdx] = md.CellText ?? string.Empty;
                        }
                    }
                }
                //for (int slot = 1; slot <= capacity; ++slot)
                //{
                    
                //}
            }

            for (int m = 0; m < _mapControls.Count; ++m)
            {
                var ctrl = _mapControls[m];
                if (ctrl == null)
                    continue;

                OnUi(ctrl, () =>
                {
                    if (m < _controlVisibilities.Count && _controlVisibilities[m] != ctrl.Visible)
                        _controlVisibilities[m] = ctrl.Visible;
                    if (false == ctrl.Visible)
                        return;

                    if (ctrl.MapSize.Height != capacity)
                        ctrl.MapSize = new Size(1, capacity);

                    for (int slot = 0; slot < capacity; ++slot)
                    {
                        int y = capacity - slot - 1;
                        ctrl.SetCellColor(0, y, colors[slot], false);
                        ctrl.SetCellText(0, y, texts[slot], false);
                    }

                    ctrl.Invalidate();
                });
            }
        }

        
        private void CallbackCellClicked(Sys3Controls.Sys3Map sender, Queue<Point> clickedPoint)
        {
            if (false == _carrierServer.HasCarrier(PortId))
                return;

            Queue<int> snapshot = null;
            bool doHighlight = false;
            Point lastPoint = default;
            DelegateCellClicked handler = null;
            int capacity = _carrierServer.GetCapacity(PortId);
            lock (_locking)
            {
                if (false ==  _eventHandlers.TryGetValue(sender, out handler))
                    return;

                _clickedCell.Clear();
                foreach (var p in clickedPoint)
                    _clickedCell.Enqueue(capacity - p.Y);

                snapshot = new Queue<int>(_clickedCell);
                doHighlight = sender.UseClick;
                if (doHighlight) lastPoint = clickedPoint.Last();
            }

            if (doHighlight)
                OnUi(sender, () => sender.SetSingleCellHighlighted(lastPoint.X, lastPoint.Y));

            handler(Index, snapshot);
        }

        private void OnUi(Sys3Controls.Sys3Map map, Action action)
        {
            if (map is System.Windows.Forms.Control c && c.InvokeRequired)
            {
                try
                {
                    c.BeginInvoke(action);
                }
                catch (ObjectDisposedException)
                {
                }
            }
            else
            {
                action();
            }
        }

        #endregion </Methods>
    }

    class MapData
    {
        #region <Properties>
        public Color CellColor { get; set; }
        public string CellText { get; set; }
        public bool SetHighlight { get; set; }
        public Color PaintedColor { get; set; }
        public string PaintedText { get; set; }
        #endregion </Properties>
    }
}
