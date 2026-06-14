using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EFEM.CustomizedByProcessType.PWA500Common
{
    #region <AttributeClass>
    public class MapDataLogicalCoordinates
    {
        [XmlAttribute("X")]
        public int LogicalCoordinateX { get; set; }     //필수
        [XmlAttribute("Y")]
        public int LogicalCoordinateY { get; set; }     //필수
    }
    public class MapDataXYDimensions
    {
        [XmlAttribute("X")]
        public float DimensionsX { get; set; }  //필수
        [XmlIgnore]
        public bool DimensionsXSpecified { get; set; }
        [XmlIgnore]
        public float AttributeDimensionsX
        {
            get => DimensionsXSpecified ? DimensionsX : 0;
            set
            {
                DimensionsX = value;
                DimensionsXSpecified = true;
            }
        }
        [XmlAttribute("Y")]
        public float DimensionsY { get; set; }  //필수
        [XmlIgnore]
        public bool DimensionsYSpecified { get; set; }
        [XmlIgnore]
        public float AttributeDimensionsY
        {
            get => DimensionsYSpecified ? DimensionsY : 0;
            set
            {
                DimensionsY = value;
                DimensionsYSpecified = true;
            }
        }
        [XmlAttribute("Units")]
        public string Units { get; set; }       //필수 아님
    }
    public class MapDataZDimensions
    {
        [XmlAttribute("Order")]
        public int Order { get; set; }      //필수 아님
        [XmlIgnore]
        public bool OrderSpecified { get; set; }
        [XmlIgnore]
        public int AttributeOrder
        {
            get => OrderSpecified ? Order : 0;
            set
            {
                Order = value;
                OrderSpecified = true;
            }
        }
        [XmlAttribute("Height")]
        public float Height { get; set; }   //필수 아님
        [XmlIgnore]
        public bool HeightSpecified { get; set; }
        [XmlIgnore]
        public float AttributeHeight
        {
            get => HeightSpecified ? Height : 0f;
            set
            {
                Height = value;
                HeightSpecified = true;
            }
        }
        [XmlAttribute("Units")]
        public string Units { get; set; }   //필수 아님
    }
    #endregion </AttributeClass>

    #region <MapData>
    [XmlRoot("MapData", Namespace = "urn:semi-org:xsd.E142-1.V1005.SubstrateMap")]
    public class MapData
    {
        [XmlElement("Layouts")]
        public MapDataLayouts Layouts { get; set; }             // MapDataLayout 0개 이상

        [XmlElement("Substrates")]
        public MapDataSubstrates Substrates { get; set; }       // MapDataSubstrate 0개 이상

        [XmlElement("SubstrateMaps")]
        public MapDataSubstrateMaps SubstrateMaps { get; set; } // MapDataSubstrateMap 0개 이상

        #region <현재 사양에서는 사용안함>
        //[XmlAttribute("FormatRevision")]
        //public string AttributeFormatRevision { get; set; } // 필수아님
        #endregion </현재 사양에서는 사용안함>
    }
    #endregion </MapData>

    #region <Layout>
    public class MapDataLayouts
    {
        public MapDataLayouts()
        {
            Layout = new List<MapDataLayout>();
        }
        [XmlElement("Layout")]
        public List<MapDataLayout> Layout { get; set; }
    }
    public class MapDataLayout
    {
        [XmlAttribute("LayoutId")]
        public string AttributeLayoutId { get; set; }               //필수
        [XmlAttribute("DefaultUnits")]
        public string AttributeDefaultUnits { get; set; }           //필수
        [XmlElement("Dimension")]
        public MapDataLogicalCoordinates AttributeDimension { get; set; }  //필수
        [XmlElement("DeviceSize")]
        public MapDataXYDimensions AttributeDeviceSize { get; set; }       //필수 아님
        [XmlElement("StepSize")]
        public MapDataXYDimensions AttributeStepSize { get; set; }         //필수 아님
        [XmlElement("LowerLeft")]
        public MapDataXYDimensions AttributeLowerLeft { get; set; }        //필수 아님
        [XmlElement("Z")]
        public MapDataZDimensions AttributeZ { get; set; }                 //필수 아님
        [XmlElement("TopImage")]
        public string AttributeTopImage { get; set; }               //필수 아님
        [XmlElement("BottomImage")]
        public string AttributeBottomImage { get; set; }            //필수 아님
        [XmlElement("ProductId")]
        public string AttributeProductId { get; set; }              //필수 아님
        [XmlAttribute("TopLevel")]
        public bool TopLevel { get; set; }                 //필수 아님
        [XmlIgnore]
        public bool TopLevelSpecified { get; set; }
        [XmlIgnore]
        public bool AttributeTopLevel
        {
            get => TopLevelSpecified ? TopLevel : false;
            set
            {
                TopLevel = value;
                TopLevelSpecified = true;
            }
        }
        [XmlAttribute("Package")]
        public bool Package { get; set; }                 //필수 아님
        [XmlIgnore]
        public bool PackageSpecified { get; set; }
        [XmlIgnore]
        public bool AttributePackage
        {
            get => PackageSpecified ? TopLevel : false;
            set
            {
                Package = value;
                PackageSpecified = true;
            }
        }
        [XmlElement("ChildLayouts")]
        public MapDataChildLayouts ChildLayouts { get; set; }       // MapDataChildLayout 0개 이상
    }
    public class MapDataChildLayouts
    {
        public MapDataChildLayouts()
        {
            ChildLayout = new List<MapDataLayout>();
        }
        [XmlElement("ChildLayout")]
        public List<MapDataLayout> ChildLayout { get; set; }
    }
    #endregion </Layout>

    #region <Substrate>
    public class MapDataSubstrates
    {
        public MapDataSubstrates()
        {
            Substrate = new List<MapDataSubstrate>();
        }
        [XmlElement("Substrate")]
        public List<MapDataSubstrate> Substrate { get; set; }
    }
    public class MapDataSubstrate
    {
        [XmlElement("AliasIds")]
        public MapDataAliasIds AliasIds { get; set; }
        [XmlAttribute("SubstrateType")]
        public string AttributeSubstrateType { get; set; }  //필수
        [XmlAttribute("SubstrateId")]
        public string AttributeSubstrateId { get; set; }    //필수
        [XmlElement("LotId")]
        public string AttributeLotId { get; set; }          //필수 아님
        [XmlElement("CarrierType")]
        public string AttributeCarrierType { get; set; }    //필수 아님
        [XmlElement("CarrierId")]
        public string AttributeCarrierId { get; set; }      //필수 아님
        [XmlElement("SlotNumber")]
        public int SlotNumber { get; set; }        //필수 아님
        [XmlIgnore]
        public bool SlotNumberSpecified { get; set; }
        [XmlIgnore]
        public int AttributeSlotNumber
        {
            get => SlotNumberSpecified ? SlotNumber : 0;
            set
            {
                SlotNumber = value;
                SlotNumberSpecified = true;
            }
        }
        [XmlAttribute("SubstrateNumber")]
        public int SubstrateNumber { get; set; }   //필수 아님
        [XmlIgnore]
        public bool SubstrateNumberSpecified { get; set; }
        [XmlIgnore]
        public int AttributeSubstrateNumber
        {
            get => SubstrateNumberSpecified ? SubstrateNumber : 0;
            set
            {
                SubstrateNumber = value;
                SubstrateNumberSpecified = true;
            }
        }
        [XmlElement("GoodDevices")]
        public int GoodDevices { get; set; }       //필수 아님
        [XmlIgnore]
        public bool GoodDevicesSpecified { get; set; }
        [XmlIgnore]
        public int AttributeGoodDevices
        {
            get => GoodDevicesSpecified ? GoodDevices : 0;
            set
            {
                GoodDevices = value;
                GoodDevicesSpecified = true;
            }
        }
        [XmlElement("SupplierName")]
        public string AttributeSupplierName { get; set; }   //필수 아님

        #region <현재 사양에서는 사용안함>
        //[XmlAttribute("CreateDate")]
        //public string AttributeCreateDate { get; set; }     //필수 아님
        //[XmlAttribute("LastModified")]
        //public string AttributeLastModified { get; set; }   //필수 아님
        #endregion </현재 사양에서는 사용안함>

        [XmlElement("Status")]
        public string AttributeStatus { get; set; }         //필수 아님
    }
    public class MapDataAliasIds
    {
        public MapDataAliasIds()
        {
            AliasId = new List<MapDataAliasId>();
        }
        [XmlElement("AliasId")]
        public List<MapDataAliasId> AliasId { get; set; }
    }
    public class MapDataAliasId
    {
        [XmlAttribute("Type")]
        public string AttributeType { get; set; }   //필수
        [XmlAttribute("Value")]
        public string AttributeValue { get; set; }  //필수
    }
    #endregion </Substrate>

    #region <SubstrateMap>
    public class MapDataSubstrateMaps
    {
        public MapDataSubstrateMaps()
        {
            SubstrateMap = new List<MapDataSubstrateMap>();
        }
        [XmlElement("SubstrateMap")]
        public List<MapDataSubstrateMap> SubstrateMap { get; set; }
    }
    public class MapDataSubstrateMap
    {
        public MapDataSubstrateMap()
        {
            Overlay = new List<MapDataOverlay>();
        }
        [XmlElement("Overlay")]
        public List<MapDataOverlay> Overlay { get; set; }           // MapDataOverlay 1개 이상
        //public MapDataOverlay Overlay { get; set; }
        [XmlAttribute("SubstrateType")]
        public string AttributeSubstrateType { get; set; }      //필수
        [XmlAttribute("SubstrateId")]
        public string AttributeSubstrateId { get; set; }        //필수
        [XmlAttribute("LayoutSpecifier")]
        public string AttributeLayoutSpecifier { get; set; }    //필수
        [XmlAttribute("SubstrateSide")]
        public string AttributeSubstrateSide { get; set; }      //필수 아님
        [XmlAttribute("Orientation")]
        public int Orientation { get; set; }           //필수 아님      생략되어 있으면 0이다.
        [XmlIgnore]
        public bool OrientationSpecified { get; set; }
        [XmlIgnore]
        public int AttributeOrientation
        {
            get => OrientationSpecified ? Orientation : 0;
            set
            {
                Orientation = value;
                OrientationSpecified = true;
            }
        }
        [XmlAttribute("OriginLocation")]
        public string AttributeOriginLocation { get; set; }     //필수 아님
        [XmlAttribute("AxisDirection")]
        public string AttributeAxisDirection { get; set; }      //필수 아님
    }

    public class MapDataOverlay
    {
        [XmlElement("ReferenceDevices")]
        public MapDataReferenceDevices ReferenceDevices { get; set; }   // MapDataReferenceDevice 0개 이상
        [XmlElement("BinCodeMap")]
        public MapDataBinCodeMap BinCodeMap { get; set; }
        [XmlElement("DeviceIdMap")]
        public MapDataDeviceIdMap DeviceIdMap { get; set; }
        [XmlElement("DeviceDataMap")]
        public MapDataDeviceDataMap DeviceDataMap { get; set; }
        [XmlElement("TransferMap")]
        public MapDataTransferMap TransferMap { get; set; }
        [XmlAttribute("MapName")]
        public string AttributeMapName { get; set; }    //필수
        [XmlAttribute("MapVersion")]
        public string AttributeMapVersion { get; set; } //필수 아님
    }

    #region <ReferenceDevice>
    public class MapDataReferenceDevices
    {
        public MapDataReferenceDevices()
        {
            ReferenceDevice = new List<MapDataReferenceDevice>();
        }
        [XmlElement("ReferenceDevice")]
        public List<MapDataReferenceDevice> ReferenceDevice { get; set; }
    }
    public class MapDataReferenceDevice
    {
        [XmlElement("Coordinates")]
        public MapDataLogicalCoordinates AttributeCoordinates { get; set; }    //필수
        [XmlElement("Position")]
        public MapDataXYDimensions AttributePosition { get; set; }             //필수 아님
        [XmlAttribute("Name")]
        public string AttributeName { get; set; }                       //필수 아님
    }
    #endregion </ReferenceDevice>

    #region <BincodeMap>
    public class MapDataBinCodeMap
    {
        public MapDataBinCodeMap()
        {
            BinCode = new List<MapDataBinCode>();
        }
        [XmlElement("BinDefinitions")]
        public MapDataBinDefinitions BinDefinitions { get; set; }   // MapDataBinDefinitions 0개 이상
        [XmlElement("BinCode")]
        public List<MapDataBinCode> BinCode { get; set; }           // MapDataBinCode 1개 이상
        [XmlAttribute("BinType")]
        public string AttributeBinType { get; set; }    //필수
        [XmlAttribute("NullBin")]
        public string AttributeNullBin { get; set; }    //필수
        [XmlAttribute("MapType")]
        public string AttributeMapType { get; set; }    //필수 아님
    }
    //public class MapDataBinCodes
    //{
    //    public MapDataBinCodes()
    //    {
    //        BinCode = new List<MapDataBinCode>();
    //    }
    //    [XmlElement("BinCode")]
    //    public List<MapDataBinCode> BinCode { get; set; }
    //}
    public class MapDataBinCode
    {
        //[XmlAttribute("Values")]
        //public string AttributeValues { get; set; } //필수
        //[XmlAttribute("X")]
        //public int AttributeX { get; set; }         //필수 아님
        //[XmlAttribute("Y")]
        //public int AttributeY { get; set; }         //필수 아님
        //[XmlAttribute("Number")]
        //public int AttributeNumber { get; set; }    //필수 아님
        [XmlText]
        public string Value { get; set; }           //필수

        [XmlAttribute("X")]
        public int X { get; set; }         //필수 아님 - Default : 0

        [XmlIgnore]
        public bool XSpecified { get; set; }
        [XmlIgnore]
        public int AttributeX
        {
            get => XSpecified ? X : 0;
            set
            {
                X = value;
                XSpecified = true;
            }
        }

        [XmlAttribute("Y")]
        public int Y { get; set; }         //필수 아님 - Default : 맵의 가장 첫번째 위치 (최상단행)

        [XmlIgnore]
        public bool YSpecified { get; set; }
        [XmlIgnore]
        public int AttributeY
        {
            get => YSpecified ? Y : 0;
            set
            {
                Y = value;
                YSpecified = true;
            }
        }

        [XmlAttribute("Number")]
        public int Number { get; set; }    //필수 아님

        [XmlIgnore]
        public bool NumberSpecified { get; set; }
        [XmlIgnore]
        public int AttributeNumber
        {
            get => NumberSpecified ? Number : 0;
            set
            {
                Number = value;
                NumberSpecified = true;
            }
        }
    }
    public class MapDataBinDefinitions
    {
        public MapDataBinDefinitions()
        {
            BinDefinition = new List<MapDataBinDefinition>();
        }
        [XmlElement("BinDefinition")]
        public List<MapDataBinDefinition> BinDefinition { get; set; }
    }
    public class MapDataBinDefinition
    {
        [XmlAttribute("BinCode")]
        public string AttributeBinCode { get; set; }          //필수
        [XmlAttribute("BinCount")]
        public int BinCount { get; set; }          //필수 아님 - 존재하지 않으면 정의되지 않는다.
        [XmlIgnore]
        public bool BinCountSpecified { get; set; }
        [XmlIgnore]
        public int AttributeBinCount
        {
            get => BinCountSpecified ? BinCount : 0;
            set
            {
                BinCount = value;
                BinCountSpecified = true;
            }
        }
        [XmlAttribute("BinDescription")]
        public string AttributeBinDescription { get; set; } //필수 아님
        [XmlAttribute("BinQuality")]
        public string AttributeBinQuality { get; set; }     //필수 아님
        [XmlAttribute("Pick")]
        public bool Pick { get; set; }             //필수 아님
        [XmlIgnore]
        public bool PickSpecified { get; set; }
        [XmlIgnore]
        public bool AttributePick
        {
            get => PickSpecified ? Pick : false;
            set
            {
                Pick = value;
                PickSpecified = true;
            }
        }
    }
    #endregion </BincodeMap>

    #region <DeviceIdMap>
    public class MapDataDeviceIdMap
    {
        [XmlElement("DeviceIds")]
        public MapDataDeviceIds DeviceIds { get; set; }     // MapDataDeviceId 1개 이상
    }
    public class MapDataDeviceIds
    {
        public MapDataDeviceIds()
        {
            DeviceId = new List<MapDataDeviceId>();
        }
        [XmlElement("DeviceId")]
        public List<MapDataDeviceId> DeviceId { get; set; }
    }
    public class MapDataDeviceId
    {
        [XmlText]
        public string AttributeValue { get; set; }  //필수
        [XmlAttribute("X")]
        public int AttributeX { get; set; }         //필수
        [XmlAttribute("Y")]
        public int AttributeY { get; set; }         //필수
    }
    #endregion </DeviceIdMap>

    #region <TransferMap>
    public class MapDataTransferMap
    {
        public MapDataTransferMap()
        {
            Transfer = new List<MapDataTransfer>();
        }
        [XmlElement("T")]
        public List<MapDataTransfer> Transfer { get; set; }         // MapDataTransfer 1개 이상
        [XmlAttribute("FromSubstrateType")]
        public string AttributeFromSubstrateType { get; set; }  //필수
        [XmlAttribute("FromSubstrateId")]
        public string AttributeFromSubstrateId { get; set; }    //필수
        [XmlAttribute("FromLayoutSpecifier")]
        public string AttributeFromLayoutSpecifier { get; set; }    //필수
    }
    public class MapDataTransfer
    {
        //public MapDataTransfer()
        //{
        //    AttributeT = new List<MapDataT>();
        //}
        //[XmlElement("T")]
        //public List<MapDataT> AttributeT { get; set; }    //필수
        [XmlAttribute("FX")]
        public int AttributeFX { get; set; }    //필수
        [XmlAttribute("FY")]
        public int AttributeFY { get; set; }    //필수
        [XmlAttribute("TX")]
        public int AttributeTX { get; set; }    //필수
        [XmlAttribute("TY")]
        public int AttributeTY { get; set; }    //필수
        // 2026.05.18 dwlim [ADD] Bond Head 추가
        [XmlAttribute("BondHead")]
        public string AttributeBondHead { get; set; }
    }
    //public class MapDataT
    //{
    //    [XmlAttribute("FX")]
    //    public int AttributeFX { get; set; }    //필수
    //    [XmlAttribute("FY")]
    //    public int AttributeFY { get; set; }    //필수
    //    [XmlAttribute("TX")]
    //    public int AttributeTX { get; set; }    //필수
    //    [XmlAttribute("TY")]
    //    public int AttributeTY { get; set; }    //필수
    //    // 2026.05.18 dwlim [ADD] Bond Head 추가
    //    [XmlAttribute("BondHead")]
    //    public string AttributeBondHead { get; set; }
    //}
    #endregion </TransferMap>

    #region <DeviceDataMap>
    public class MapDataDeviceDataMap
    {
        public MapDataDeviceDataMap()
        {
            DeviceData = new List<MapDataDeviceData>();
            Parameter = new List<MapDataParameter>();
        }
        [XmlElement("DeviceData")]
        public List<MapDataDeviceData> DeviceData { get; set; }     // 필수 MapDataDeviceData 1개 이상
        [XmlElement("Parameter")]
        public List<MapDataParameter> Parameter { get; set; }     // 필수 MapDataParameter 1개 이상
    }
    public class MapDataDeviceData
    {
        public MapDataDeviceData()
        {
            AttributeValues = new List<object>();
        }
        [XmlAttribute("X")]
        public int AttributeX { get; set; }         //필수 아님
        [XmlAttribute("Y")]
        public int AttributeY { get; set; }         //필수 아님
        [XmlAttribute("LID")]
        public string AttributeLID { get; set; }    //필수 아님
        /// <summary>
        /// 이건 좀 특이함. DeviceDataMap이 속성으로 갖고있는 Parameters의 순서와 일치해야함.
        /// Parameters와 연관되어 있는듯하다. (사용할 때 E142 참고해야할듯)
        /// [XmlAttribute("Values")] 이거로 하면 터진다. 이게 어떻게 쓰이는지 잘 모르겠다.
        /// 일단 Element로 했음
        /// </summary>
        [XmlElement("Values")]
        public List<object> AttributeValues { get; set; }  //필수
    }
    public class MapDataParameter
    {
        [XmlAttribute("Name")]
        public string AttributeName { get; set; }  //필수
        [XmlAttribute("Description")]
        public string AttributeDescription { get; set; }  //필수 아님
    }
    #endregion </DeviceDataMap>

    #endregion </SubstrateMap>
}
