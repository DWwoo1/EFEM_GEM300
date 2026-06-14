using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFEM.Defines.MapData
{
    #region <Enumerations>
    public enum SubstrateSide
    {
        TopSide,
        BottomSide,
    }
    public enum OriginLocation
    {
        LowerLeft,
        UpperLeft,
        LowerRight,
        UpperRight,
        Center,
    }
    public enum AxisDirection
    {
        UpRight,
        DownRight,
        UpLeft,
        DownLeft,
    }
    public enum SubstrateType
    {
        Wafer,
        Frame,
        Strip,
        Tray,
    }
    public enum MapType
    {
        TwoDimensionalArray,
        RowColumn,
        Array,
        Coordinate,
    }
    #endregion </Enumerations>
    //class MapDataDefines
    //{
    //}
}
