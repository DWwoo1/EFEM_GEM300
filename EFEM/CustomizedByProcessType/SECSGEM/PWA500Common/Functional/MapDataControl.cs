using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Net;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    public enum EN_BINCODE_FORMAT
    {
        Undefined = 0,
        Ascii,
        Decimal,
        HexaDecimal,
        Integer2,
    }
    class MapDataControl
    {
        #region <Constructor>
        public MapDataControl()
        {
            _pmsControl = new PMSControl();
        }
        #endregion </Constructor>

        #region <Fields>
        PMSControl _pmsControl;

        #region <Constants_Common>
        private const string AttributeSubstrateTypeFieldValue = "Wafer";
        #endregion </Constants_Common>

        #region <Constants_Layout>
        private const string AttributeLayoutIdFieldValue = "WaferLayout";
        private const string AttributeChildLayoutIdFieldValue = "Die";
        private const string AttributeLayoutDefaultUnitsFieldValue = "mm";
        private const int AttributeDefaultColCountFieldValue = 1;
        private const int AttributeDefaultRowCountFieldValue = 1;
        #endregion </Constants_Layout>

        #region <Constants_Substrate>
        #endregion </Constants_Substrate>

        #region <Constants_SubstrateMap>
        private const string AttributeSubsMapLayoutSpecFieldValue = "Wafer/Die";
        private const string AttributeSubsMapOriginLocationFieldValue = "UpperLeft";

        // Overlay
        private const string AttributeOverlayMapNameFieldValue_Core = "UploadCoreWaferMap";
        private const string AttributeOverlayMapNameFieldValue_Bin = "UploadBinWaferMap";
        private const string AttributeOverlayTransferMapNameFieldValue = "CoreToEmpty";

        // BinCodeMap
        private const string AttributeNullBinFieldValue = ".";

        #endregion </Constants_SubstrateMap>

        #endregion </Fields>

        #region <Methods>

        #region <Data Handling>

        #region <Get>
        public bool HasOnlyOneSubstrate(MapData mapData, out MapDataSubstrate subs)
        {
            if (null == mapData)
            {
                subs = null;
                return false;
            }

            if (1 != mapData.Substrates.Substrate.Count)
            {
                subs = null;
                return false;
            }

            subs = mapData.Substrates.Substrate[0];
            return true;
        }
        public bool HasOnlyOneSubstrateMap(MapData mapData, out MapDataSubstrateMap subsMap)
        {
            if (null == mapData)
            {
                subsMap = null;
                return false;
            }

            if (1 != mapData.SubstrateMaps.SubstrateMap.Count)
            {
                subsMap = null;
                return false;
            }

            subsMap = mapData.SubstrateMaps.SubstrateMap[0];
            return true;
        }
        public bool FindLayoutById(MapData mapData, string layoutId, out MapDataLayout layout)
        {
            if (null == mapData)
            {
                layout = null;
                return false;
            }
            foreach (var item in mapData.Layouts.Layout)
            {
                if (layoutId == item.AttributeLayoutId)
                {
                    layout = item;
                    return true;
                }
            }
            layout = null;
            return false;
        }
        public bool FindOverlayByMapName(MapDataSubstrateMap subsMap, string mapName, out MapDataOverlay overlay)
        {
            if (null == subsMap)
            {
                overlay = null;
                return false;
            }
            foreach (var item in subsMap.Overlay)
            {
                if (mapName == item.AttributeMapName)
                {
                    overlay = item;
                    return true;
                }
            }
            overlay = null;
            return false;
        }
        public bool FindReferenceDeviceByName(MapDataSubstrateMap subsMap, string deviceName, out MapDataReferenceDevice referenceDevice)
        {
            if (null == subsMap)
            {
                referenceDevice = null;
                return false;
            }
            foreach (var ov in subsMap.Overlay)
            {
                foreach (var refDevice in ov.ReferenceDevices.ReferenceDevice)
                {
                    if (refDevice.AttributeName.Contains(deviceName))
                    {
                        referenceDevice = refDevice;
                        return true;
                    }
                }

            }
            referenceDevice = null;
            return false;
        }
        public bool GetAngle(MapDataSubstrateMap subsMap, out double angle)
        {
            if (null == subsMap || subsMap.AttributeOrientation < 0)
            {
                angle = 0;
                return false;
            }

            angle = subsMap.AttributeOrientation;
            return true;
        }
        public bool GetOriginLocation(MapDataSubstrateMap subsMap, out string originLocation)
        {
            if (null == subsMap || string.IsNullOrEmpty(subsMap.AttributeOriginLocation))
            {
                originLocation = string.Empty;
                return false;
            }

            originLocation = subsMap.AttributeOriginLocation;
            return true;
        }
        public bool GetDieCount(MapDataSubstrate subs, out int goodDevice)
        {
            if (null == subs)
            {
                goodDevice = 0;
                return false;
            }

            goodDevice = subs.AttributeGoodDevices;
            return true;
        }
        public string GetBinCodeMapByOverlayMapName(MapData mapData, string layoutId)
        {
            MapDataOverlay overlay = null;
            EN_BINCODE_FORMAT format = EN_BINCODE_FORMAT.Undefined;
            if (null == mapData)
                return null;

            foreach (var subsMap in mapData.SubstrateMaps.SubstrateMap)
            {
                foreach (var ov in subsMap.Overlay)
                {
                    if (layoutId == ov.AttributeMapName)
                    {
                        overlay = ov;
                    }
                }
            }

            if (null == overlay)
                return null;

            if (false == Enum.TryParse(overlay.BinCodeMap.AttributeBinType, out EN_BINCODE_FORMAT result) ||
                result == EN_BINCODE_FORMAT.Undefined)
                return null;

            StringBuilder sb = new StringBuilder();
            switch (result)
            {
                case EN_BINCODE_FORMAT.Ascii:
                    {
                        foreach (var bincode in overlay.BinCodeMap.BinCode)
                        {
                            sb.Append(bincode.Value);
                        }

                        if (string.IsNullOrEmpty(sb.ToString()))
                            return null;

                        string strBinCodeMap = sb.ToString();
                        return strBinCodeMap;
                    }
                    break;
                case EN_BINCODE_FORMAT.Decimal:
                    {
                    }
                    break;
                case EN_BINCODE_FORMAT.HexaDecimal:
                    {
                    }
                    break;
                case EN_BINCODE_FORMAT.Integer2:
                    {
                    }
                    break;

                default:
                    return null;
            }

            return null;
        }
        public MapDataLayout GetRegisterdChildLayout(MapDataLayout childLayout)
        {
            MapDataLayout childLayoutInfo = new MapDataLayout();
            childLayoutInfo.AttributeLayoutId = childLayout.AttributeLayoutId;
            return childLayoutInfo;
        }
        #endregion </Get>

        #region <Set>
        public MapData MakeCoreMapObject(string lotId, string waferId, string recipeId, string binMap, int angle, int countCol, int countRow, int chipQty, int refX, int refY)
        {
            // 기존에 보냈던 것 중 보낼만한것들
            // (가능) Lot ID
            // (추후 확인 필요) Ring ID (실제 사용할지 모르겠다.)
            // (가능) Material Id ( -> Wafer Id)
            // (가능) Substrate Type ( -> Wafer)
            // (가능) Angle
            // (가능) Count Of Row, Count Of Col
            // (기존거 기억해야함 아니면 고정?) Index Of Ref X, Index Of Ref Y
            // (기존거 기억해야함 아니면 고정?) Starting Position X, Y
            // (가능) Chip Qty (일단 BinCode별 다 보내는 것으로 만들자)
            // (가능) BinCode Map (일단 한줄씩 여러번 보내는 형식으로 하자)
            // (가능) Null Bin

            // (가능) Ring ID (실제 사용할지 모르겠ㅇ)
            // (가능) Recipe Id (E142 에서 사용하는 것인지 아직 모르겠다. E142에서 Layout의 ProductId 이려나??) 
            // Die Size (아직 모르겠다. 이전에는 안썼음)
            // Port Id (E142에서는 안하는 것 같다..)
            // Carrier Id (E142에서는 안하는 것 같다..)

            MapData mapData = new MapData();
            mapData.Layouts = new MapDataLayouts();
            mapData.Substrates = new MapDataSubstrates();
            mapData.SubstrateMaps = new MapDataSubstrateMaps();

            // Layout
            MapDataLayout layout = new MapDataLayout();
            layout.AttributeLayoutId = AttributeLayoutIdFieldValue;
            layout.AttributeDefaultUnits = AttributeLayoutDefaultUnitsFieldValue;
            layout.AttributeTopLevel = true;
            layout.AttributeDimension = new MapDataLogicalCoordinates();
            layout.AttributeDimension.LogicalCoordinateX = AttributeDefaultColCountFieldValue;
            layout.AttributeDimension.LogicalCoordinateY = AttributeDefaultRowCountFieldValue;

            // Child Layout
            MapDataLayout childlayout = new MapDataLayout();
            childlayout.AttributeLayoutId = AttributeChildLayoutIdFieldValue;
            childlayout.AttributeDimension = new MapDataLogicalCoordinates();
            childlayout.AttributeDimension.LogicalCoordinateX = countCol;
            childlayout.AttributeDimension.LogicalCoordinateY = countRow;
            childlayout.AttributeDefaultUnits = AttributeLayoutDefaultUnitsFieldValue;
            childlayout.AttributeTopLevel = false;

            layout.ChildLayouts = new MapDataChildLayouts();
            layout.ChildLayouts.ChildLayout.Add(GetRegisterdChildLayout(childlayout));

            // Substrate
            MapDataSubstrate subs = new MapDataSubstrate();
            subs.AttributeSubstrateType = AttributeSubstrateTypeFieldValue;
            subs.AttributeSubstrateId = waferId;
            subs.AttributeLotId = lotId;

            // SubstrateMap
            MapDataSubstrateMap subsMap = new MapDataSubstrateMap();
            subsMap.AttributeSubstrateType = AttributeSubstrateTypeFieldValue;
            subsMap.AttributeSubstrateId = waferId;
            subsMap.AttributeLayoutSpecifier = AttributeSubsMapLayoutSpecFieldValue;
            subsMap.AttributeOrientation = angle;       // 2026.06.09 dwlim [ADD]
            subsMap.AttributeOriginLocation = AttributeSubsMapOriginLocationFieldValue;

            // Overlay
            MapDataOverlay overlay = new MapDataOverlay();
            overlay.AttributeMapName = AttributeOverlayMapNameFieldValue_Core;

            // ReferenceDevices
            //MapDataReferenceDevice referenceDevice = new MapDataReferenceDevice();
            //referenceDevice.AttributeCoordinates = new MapDataLogicalCoordinates();
            //referenceDevice.AttributeCoordinates.LogicalCoordinateX = refX;
            //referenceDevice.AttributeCoordinates.LogicalCoordinateY = refY;

            // BinCodeMap
            MapDataBinCodeMap binCodeMap = new MapDataBinCodeMap();
            binCodeMap.AttributeBinType = EN_BINCODE_FORMAT.Ascii.ToString();
            binCodeMap.AttributeNullBin = AttributeNullBinFieldValue;

            // BinDefinitions
            Dictionary<char, int> binCount = new Dictionary<char, int>();
            foreach (char item in binMap)
            {
                string bin = item.ToString();
                if (item.Equals(".") || string.IsNullOrWhiteSpace(bin))
                    continue;

                if (false == binCount.ContainsKey(item))
                    binCount[item] = 1;

                else
                    ++binCount[item];
            }

            if (0 == binCount.Count)
                return null;

            MapDataBinDefinitions binDefinitions = new MapDataBinDefinitions();
            foreach (var item in binCount)
            {
                MapDataBinDefinition binDefinition = new MapDataBinDefinition();
                //if (false == int.TryParse(item.Key.ToString(), out int bincode))
                //    return null;

                binDefinition.AttributeBinCode = item.Key.ToString();
                binDefinition.AttributeBinCount = item.Value;

                binDefinitions.BinDefinition.Add(binDefinition);
            }

            // BinCode
            List<MapDataBinCode> binCode = new List<MapDataBinCode>();
            //if (false == binMap.Length.Equals(countCol * countRow))
            //    return null;

            for (int i = 0; i < countRow; i++)
            {
                MapDataBinCode bc = new MapDataBinCode();
                bc.Value = binMap.Substring(countCol * i, countCol);
                binCode.Add(bc);
            }

            binCodeMap.BinDefinitions = binDefinitions;
            binCodeMap.BinCode = binCode;
            overlay.BinCodeMap = binCodeMap;
            //overlay.ReferenceDevices = new MapDataReferenceDevices();
            //overlay.ReferenceDevices.ReferenceDevice.Add(referenceDevice);
            subsMap.Overlay.Add(overlay);

            mapData.SubstrateMaps.SubstrateMap.Add(subsMap);
            mapData.Layouts.Layout.Add(layout);
            mapData.Layouts.Layout.Add(childlayout);
            mapData.Substrates.Substrate.Add(subs);

            return mapData;
        }
        public MapData MakeBinMapObject(string lotId, string binWaferId, string recipeId, string binMap, int angle, int countCol, int countRow, int chipQty, int refX, int refY, Dictionary<string, List<string[]>> transferedData)
        {
            
            // 기존에 보냈던 것 중 보낼만한것들
            // (가능) Lot ID
            // (추후 확인 필요) Ring ID (실제 사용할지 모르겠다.)
            // (가능) Material Id ( -> Wafer Id)
            // (가능) Substrate Type ( -> Wafer)
            // (가능) Angle
            // (가능) Count Of Row, Count Of Col
            // (기존거 기억해야함 아니면 고정?) Index Of Ref X, Index Of Ref Y
            // (기존거 기억해야함 아니면 고정?) Starting Position X, Y
            // (가능) Chip Qty (일단 BinCode별 다 보내는 것으로 만들자)
            // (가능) BinCode Map (일단 한줄씩 여러번 보내는 형식으로 하자)
            // (가능) Null Bin

            // (가능) Ring ID (실제 사용할지 모르겠ㅇ)
            // (가능) Recipe Id (E142 에서 사용하는 것인지 아직 모르겠다. E142에서 Layout의 ProductId 이려나??) 
            // Die Size (아직 모르겠다. 이전에는 안썼음)
            // Port Id (E142에서는 안하는 것 같다..)
            // Carrier Id (E142에서는 안하는 것 같다..)

            MapData mapData = new MapData();
            mapData.Layouts = new MapDataLayouts();
            mapData.Substrates = new MapDataSubstrates();
            mapData.SubstrateMaps = new MapDataSubstrateMaps();

            // Layout
            MapDataLayout layout = new MapDataLayout();
            layout.AttributeLayoutId = AttributeLayoutIdFieldValue;
            layout.AttributeDefaultUnits = AttributeLayoutDefaultUnitsFieldValue;
            layout.AttributeTopLevel = true;
            layout.AttributeDimension = new MapDataLogicalCoordinates();
            layout.AttributeDimension.LogicalCoordinateX = AttributeDefaultColCountFieldValue;
            layout.AttributeDimension.LogicalCoordinateY = AttributeDefaultRowCountFieldValue;

            // Child Layout
            MapDataLayout childlayout = new MapDataLayout();
            childlayout.AttributeLayoutId = AttributeChildLayoutIdFieldValue;
            childlayout.AttributeDimension = new MapDataLogicalCoordinates();
            childlayout.AttributeDimension.LogicalCoordinateX = countCol;
            childlayout.AttributeDimension.LogicalCoordinateY = countRow;
            childlayout.AttributeDefaultUnits = AttributeLayoutDefaultUnitsFieldValue;
            childlayout.AttributeTopLevel = false;

            layout.ChildLayouts = new MapDataChildLayouts();
            layout.ChildLayouts.ChildLayout.Add(GetRegisterdChildLayout(childlayout));

            // Substrate
            MapDataSubstrate subs = new MapDataSubstrate();
            subs.AttributeSubstrateType = AttributeSubstrateTypeFieldValue;
            subs.AttributeSubstrateId = binWaferId;
            subs.AttributeLotId = lotId;

            // SubstrateMap
            MapDataSubstrateMap subsMap = new MapDataSubstrateMap();
            subsMap.AttributeSubstrateType = AttributeSubstrateTypeFieldValue;
            subsMap.AttributeSubstrateId = binWaferId;
            subsMap.AttributeLayoutSpecifier = AttributeSubsMapLayoutSpecFieldValue;
            subsMap.AttributeOrientation = angle;
            subsMap.AttributeOriginLocation = AttributeSubsMapOriginLocationFieldValue;

            #region <MapData>
            // Overlay
            MapDataOverlay overlay_BinCodeMap = new MapDataOverlay();
            overlay_BinCodeMap.AttributeMapName = AttributeOverlayMapNameFieldValue_Bin;

            // ReferenceDevices
            //MapDataReferenceDevice referenceDevice = new MapDataReferenceDevice();
            //referenceDevice.AttributeCoordinates = new MapDataLogicalCoordinates();
            //referenceDevice.AttributeCoordinates.LogicalCoordinateX = refX;
            //referenceDevice.AttributeCoordinates.LogicalCoordinateY = refY;

            // BinCodeMap
            MapDataBinCodeMap binCodeMap = new MapDataBinCodeMap();
            binCodeMap.AttributeBinType = EN_BINCODE_FORMAT.Ascii.ToString();
            binCodeMap.AttributeNullBin = AttributeNullBinFieldValue;

            // BinDefinitions
            Dictionary<char, int> binCount = new Dictionary<char, int>();
            foreach (char item in binMap)
            {
                string bin = item.ToString();
                if (item.Equals(".") || string.IsNullOrWhiteSpace(bin))
                    continue;

                if (false == binCount.ContainsKey(item))
                    binCount[item] = 1;

                else
                    ++binCount[item];
            }

            if (0 == binCount.Count)
                return null;

            MapDataBinDefinitions binDefinitions = new MapDataBinDefinitions();
            foreach (var item in binCount)
            {
                MapDataBinDefinition binDefinition = new MapDataBinDefinition();
                //if (false == int.TryParse(item.Key.ToString(), out int bincode))
                //    return null;

                binDefinition.AttributeBinCode = item.Key.ToString();
                binDefinition.AttributeBinCount = item.Value;

                binDefinitions.BinDefinition.Add(binDefinition);
            }

            // BinCode
            List<MapDataBinCode> binCode = new List<MapDataBinCode>();
            //if (false == binMap.Length.Equals(countCol * countRow))
            //    return null;

            for (int i = 0; i < countRow; i++)
            {
                MapDataBinCode bc = new MapDataBinCode();
                bc.Value = binMap.Substring(countCol * i, countCol);
                binCode.Add(bc);
            }

            binCodeMap.BinDefinitions = binDefinitions;
            binCodeMap.BinCode = binCode;
            overlay_BinCodeMap.BinCodeMap = binCodeMap;
            //overlay_BinCodeMap.ReferenceDevices = new MapDataReferenceDevices();
            //overlay_BinCodeMap.ReferenceDevices.ReferenceDevice.Add(referenceDevice);
            subsMap.Overlay.Add(overlay_BinCodeMap);
            #endregion </MapData>

            #region <TransferMapData>
            if (transferedData == null)
                return null;

            foreach (var transferedCoreChips in transferedData)
            {
                MapDataOverlay overlay_TransferMap = new MapDataOverlay();
                overlay_TransferMap.AttributeMapName = AttributeOverlayTransferMapNameFieldValue;

                MapDataTransferMap transferMap = new MapDataTransferMap();
                //MapDataTransfer transfer = new MapDataTransfer();
                transferMap.AttributeFromSubstrateType = AttributeSubstrateTypeFieldValue;
                transferMap.AttributeFromSubstrateId = transferedCoreChips.Key;

                foreach (var item in transferedCoreChips.Value)
                {
                    if (null == item)
                        return null;

                    MapDataTransfer t = new MapDataTransfer();
                    t.AttributeFX = int.Parse(item[0]);
                    t.AttributeFY = int.Parse(item[1]);
                    t.AttributeTX = int.Parse(item[2]);
                    t.AttributeTY = int.Parse(item[3]);
                    t.AttributeBondHead = item[4];
                    transferMap.Transfer.Add(t);
                }
                //transferMap.Transfer.Add(transfer);
                overlay_TransferMap.TransferMap = transferMap;
                subsMap.Overlay.Add(overlay_TransferMap);
            }
            #endregion </TransferMapData>

            mapData.SubstrateMaps.SubstrateMap.Add(subsMap);
            mapData.Layouts.Layout.Add(layout);
            mapData.Layouts.Layout.Add(childlayout);
            mapData.Substrates.Substrate.Add(subs);

            return mapData;
        }
        #endregion </Set>

        #endregion </Data Handling>

        #region <XML>
        public MapData DeserializeMapData(string receiveXmlData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(receiveXmlData))
                    return null;

                string xmlData = receiveXmlData;

                if (xmlData.Contains("%0D") || xmlData.Contains("%0A") || xmlData.Contains("%3C"))
                {
                    xmlData = WebUtility.UrlDecode(xmlData);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(MapData));

                using (StringReader reader = new StringReader(xmlData))
                {
                    return (MapData)serializer.Deserialize(reader);
                }
            }
            catch (InvalidOperationException ex)
            {
                Exception root = ex;
                while (root.InnerException != null)
                    root = root.InnerException;

                Console.WriteLine(root.Message);

                throw new Exception("Deserialize 실패: " + root.Message, ex);
            }
            return null;
        }

        public string SerializeMapData(MapData mapData)
        {
            if (mapData == null)
                return null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MapData));

                var ns = new XmlSerializerNamespaces();
                ns.Add("", "urn:semi-org:xsd.E142-1.V1005.SubstrateMap");

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = true,
                    Encoding = new UTF8Encoding(false)
                };

                using (var stringWriter = new Utf8StringWriter())
                using (XmlWriter writer = XmlWriter.Create(stringWriter, settings))
                {
                    serializer.Serialize(writer, mapData, ns);
                    return stringWriter.ToString();
                }
            }
            catch (InvalidOperationException ex)
            {
                Exception root = ex;
                while (root.InnerException != null)
                    root = root.InnerException;

                throw new Exception("Serialize 실패: " + root.Message, ex);
            }
        }

        public class Utf8StringWriter : StringWriter
        {
            //public override Encoding Encoding => Encoding.UTF8;
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }
        #endregion </XML>

        #endregion </Methods>
    }
}
