using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameOfSystem3
{
    [Serializable]
	public struct DPointXY
	{
		public double x;
		public double y;

		public DPointXY(double x, double y)
		{
			this.x = x;
			this.y = y;
		}
		public DPointXY(DPointXY sp)
			: this(sp.x, sp.y)
		{
		}
		public DPointXY(string s)
		{
			DPointXY result;
			TryParse(s, out result);
			x = result.x;
			y = result.y;
		}
		public static DPointXY operator +(DPointXY pt1, DPointXY pt2)
		{
			DPointXY dPlus;

			dPlus.x = pt1.x + pt2.x;
			dPlus.y = pt1.y + pt2.y;

			return dPlus;
		}
		public static DPointXY operator -(DPointXY pt1, DPointXY pt2)
		{
			DPointXY dMinus;

			dMinus.x = pt1.x - pt2.x;
			dMinus.y = pt1.y - pt2.y;

			return dMinus;
		}
		// 2022.04.30 by junho [ADD] Operator 및 override 추가
		public static DPointXY operator *(DPointXY pt1, DPointXY pt2)
		{
			DPointXY multi;
			multi.x = pt1.x * pt2.x;
			multi.y = pt1.y * pt2.y;

			return multi;
		}
		public static DPointXY operator /(DPointXY pt1, DPointXY pt2)
		{
			DPointXY divis;

			divis.x = pt1.x / pt2.x;
			divis.y = pt1.y / pt2.y;

			return divis;
		}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (false == obj is DPointXY) return false;

			DPointXY target = (DPointXY)obj;
			return this.x == target.x && this.y == target.y;
		}
		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode();
		}

		public static bool operator ==(DPointXY pt1, DPointXY pt2)
		{
			return pt1.x == pt2.x && pt1.y == pt2.y;
		}
		public static bool operator !=(DPointXY pt1, DPointXY pt2)
		{
			return pt1.x != pt2.x || pt1.y != pt2.y;
		}

		public override string ToString()
		{
			return string.Format("{0},{1}", x.ToString(), y.ToString());
		}
		public string ToString(string format)
		{
			return string.Format("{0},{1}", x.ToString(format), y.ToString(format));
		}
		public static bool TryParse(string s, out DPointXY result)
		{
			result = new DPointXY(0.0, 0.0);
			string[] splited = s.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (splited.Length != 2)
				return false;

			if (false == double.TryParse(splited[0], out result.x)
				|| false == double.TryParse(splited[1], out result.y))
				return false;

			return true;
		}
	}
	
	[Serializable]
	public struct DPointXYT
	{
		public double x;
		public double y;
		public double t;

		public DPointXYT(double x, double y, double t)
		{
			this.x = x;
			this.y = y;
			this.t = t;
		}
		public DPointXYT(DPointXYT sp)
			: this(sp.x, sp.y, sp.t)
		{
		}
		public DPointXYT(string s)
		{
			DPointXYT result;
			TryParse(s, out result);
			x = result.x;
			y = result.y;
			t = result.t;
		}
		public static DPointXYT operator +(DPointXYT pt1, DPointXYT pt2)
		{
			DPointXYT dPlus;

			dPlus.x = pt1.x + pt2.x;
			dPlus.y = pt1.y + pt2.y;
			dPlus.t = pt1.t + pt2.t;

			return dPlus;
		}
		public static DPointXYT operator -(DPointXYT pt1, DPointXYT pt2)
		{
			DPointXYT dMinus;

			dMinus.x = pt1.x - pt2.x;
			dMinus.y = pt1.y - pt2.y;
			dMinus.t = pt1.t - pt2.t;

			return dMinus;
		}
		// 2022.04.30 by junho [ADD] Operator 및 override 추가
		public static DPointXYT operator *(DPointXYT pt1, DPointXYT pt2)
		{
			DPointXYT multi;
			multi.x = pt1.x * pt2.x;
			multi.y = pt1.y * pt2.y;
			multi.t = pt1.t * pt2.t;

			return multi;
		}
		public static DPointXYT operator /(DPointXYT pt1, DPointXYT pt2)
		{
			DPointXYT divis;

			divis.x = pt1.x / pt2.x;
			divis.y = pt1.y / pt2.y;
			divis.t = pt1.t / pt2.t;

			return divis;
		}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (false == obj is DPointXYT) return false;

			DPointXYT target = (DPointXYT)obj;
			return this.x == target.x
				&& this.y == target.y
				&& this.t == target.t;
		}

		public override int GetHashCode()
		{
			return x.GetHashCode()
				^ y.GetHashCode()
				^ t.GetHashCode();
		}

		public static bool operator ==(DPointXYT pt1, DPointXYT pt2)
		{
			return pt1.x == pt2.x
				&& pt1.y == pt2.y
				&& pt1.t == pt2.t;
		}

		public static bool operator !=(DPointXYT pt1, DPointXYT pt2)
		{
			return pt1.x != pt2.x
				|| pt1.y != pt2.y
				|| pt1.t != pt2.t;
		}

		public override string ToString()
		{
			return string.Format("{0},{1},{2}", x.ToString(), y.ToString(), t.ToString());
		}
		public string ToString(string format)
		{
			return string.Format("{0},{1},{2}", x.ToString(format), y.ToString(format), t.ToString(format));
		}
		public static bool TryParse(string s, out DPointXYT result)
		{
			result = new DPointXYT(0.0, 0.0, 0.0);
			string[] splited = s.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (splited.Length != 3)
				return false;

			if (false == double.TryParse(splited[0], out result.x)
				|| false == double.TryParse(splited[1], out result.y)
				|| false == double.TryParse(splited[2], out result.t))
				return false;

			return true;
		}
	}
	
    // 2022.04.12 by jhchoo [ADD] New DPointXYZ (DPointXYT 와 동일, T > Z 명칭만 수정)
    [Serializable]
    public struct DPointXYZ
    {
        public double x;
        public double y;
        public double z;

        public DPointXYZ(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public DPointXYZ(DPointXYZ sp)
            : this(sp.x, sp.y, sp.z)
        {
        }
		public DPointXYZ(string s)
		{
			DPointXYZ result;
			TryParse(s, out result);
			x = result.x;
			y = result.y;
			z = result.z;
		}
        public static DPointXYZ operator +(DPointXYZ pt1, DPointXYZ pt2)
        {
            DPointXYZ dPlus;

            dPlus.x = pt1.x + pt2.x;
            dPlus.y = pt1.y + pt2.y;
            dPlus.z = pt1.z + pt2.z;

            return dPlus;
        }
        public static DPointXYZ operator -(DPointXYZ pt1, DPointXYZ pt2)
        {
            DPointXYZ dMinus;

            dMinus.x = pt1.x - pt2.x;
            dMinus.y = pt1.y - pt2.y;
            dMinus.z = pt1.z - pt2.z;

            return dMinus;
		}

		public override string ToString()
		{
			return string.Format("{0},{1},{2}", x.ToString(), y.ToString(), z.ToString());
		}
		public string ToString(string format)
		{
			return string.Format("{0},{1},{2}", x.ToString(format), y.ToString(format), z.ToString(format));
		}
		public static bool TryParse(string s, out DPointXYZ result)
		{
			result = new DPointXYZ(0.0, 0.0, 0.0);
			string[] splited = s.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (splited.Length != 3)
				return false;

			if (false == double.TryParse(splited[0], out result.x)
				|| false == double.TryParse(splited[1], out result.y)
				|| false == double.TryParse(splited[2], out result.z))
				return false;

			return true;
		}
    }
	// 2020.12.01 by jhchoo [MOD] IPoint > IPointXY
    [Serializable]
	public struct IPointXY
	{
		public int x;
		public int y;

		public IPointXY(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public IPointXY(IPointXY sp)
			: this(sp.x, sp.y)
		{
		}
		public IPointXY(string s)
		{
			IPointXY result;
			TryParse(s, out result);
			x = result.x;
			y = result.y;
		}
		public static IPointXY operator +(IPointXY pt1, IPointXY pt2)
		{
			IPointXY iPlus;

			iPlus.x = pt1.x + pt2.x;
			iPlus.y = pt1.y + pt2.y;

			return iPlus;
		}
		public static IPointXY operator -(IPointXY pt1, IPointXY pt2)
		{
			IPointXY iMinus;

			iMinus.x = pt1.x - pt2.x;
			iMinus.y = pt1.y - pt2.y;

			return iMinus;
		}

		// 2022.04.30 by junho [ADD] Operator 및 override 추가
		public static IPointXY operator *(IPointXY pt1, IPointXY pt2)
		{
			IPointXY multi;
			multi.x = pt1.x * pt2.x;
			multi.y = pt1.y * pt2.y;

			return multi;
		}
		public static IPointXY operator /(IPointXY pt1, IPointXY pt2)
		{
			IPointXY divis;

			divis.x = pt1.x / pt2.x;
			divis.y = pt1.y / pt2.y;

			return divis;
		}
		public static IPointXY operator %(IPointXY pt1, IPointXY pt2)
		{
			IPointXY remainder;

			remainder.x = pt1.x % pt2.x;
			remainder.y = pt1.y % pt2.y;

			return remainder;
		}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (false == obj is IPointXY) return false;

			IPointXY target = (IPointXY)obj;
			return this.x == target.x && this.y == target.y;
		}
		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode();
		}

		public static bool operator ==(IPointXY pt1, IPointXY pt2)
		{
			return pt1.x == pt2.x && pt1.y == pt2.y;
		}
		public static bool operator !=(IPointXY pt1, IPointXY pt2)
		{
			return pt1.x != pt2.x || pt1.y != pt2.y;
		}

		public override string ToString()
		{
			return string.Format("{0},{1},{2}", x.ToString(), y.ToString());
		}
		public static bool TryParse(string s, out IPointXY result)
		{
			result = new IPointXY(0, 0);
			string[] splited = s.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (splited.Length != 2)
				return false;

			if (false == int.TryParse(splited[0], out result.x)
				|| false == int.TryParse(splited[1], out result.y))
				return false;

			return true;
		}
	}
	// 2020.12.01 by jhchoo [ADD] 3 Point 추가
    [Serializable]
	public struct IPointXYT
	{
		public int x;
		public int y;
		public int t;
		public IPointXYT(int x, int y, int t)
		{
			this.x = x;
			this.y = y;
			this.t = t;
		}

		public IPointXYT(IPointXYT sp)
			: this(sp.x, sp.y, sp.t)
		{
		}
		public IPointXYT(string s)
		{
			IPointXYT result;
			TryParse(s, out result);
			x = result.x;
			y = result.y;
			t = result.t;
		}

		public static IPointXYT operator +(IPointXYT pt1, IPointXYT pt2)
		{
			IPointXYT iPlus;

			iPlus.x = pt1.x + pt2.x;
			iPlus.y = pt1.y + pt2.y;
			iPlus.t = pt1.t + pt2.t;

			return iPlus;
		}
		public static IPointXYT operator -(IPointXYT pt1, IPointXYT pt2)
		{
			IPointXYT iMinus;

			iMinus.x = pt1.x - pt2.x;
			iMinus.y = pt1.y - pt2.y;
			iMinus.t = pt1.t - pt2.t;

			return iMinus;
		}

		// 2022.04.30 by junho [ADD] Operator 및 override 추가
		public static IPointXYT operator *(IPointXYT pt1, IPointXYT pt2)
		{
			IPointXYT multi;
			multi.x = pt1.x * pt2.x;
			multi.y = pt1.y * pt2.y;
			multi.t = pt1.t * pt2.t;

			return multi;
		}
		public static IPointXYT operator /(IPointXYT pt1, IPointXYT pt2)
		{
			IPointXYT divis;

			divis.x = pt1.x / pt2.x;
			divis.y = pt1.y / pt2.y;
			divis.t = pt1.t / pt2.t;

			return divis;
		}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (false == obj is IPointXYT) return false;

			IPointXYT target = (IPointXYT)obj;
			return this.x == target.x
				&& this.y == target.y
				&& this.t == target.t;
		}

		public override int GetHashCode()
		{
			return x.GetHashCode()
				^ y.GetHashCode()
				^ t.GetHashCode();
		}

		public static bool operator ==(IPointXYT pt1, IPointXYT pt2)
		{
			return pt1.x == pt2.x
				&& pt1.y == pt2.y
				&& pt1.t == pt2.t;
		}

		public static bool operator !=(IPointXYT pt1, IPointXYT pt2)
		{
			return pt1.x != pt2.x
				|| pt1.y != pt2.y
				|| pt1.t != pt2.t;
		}

		public override string ToString()
		{
			return string.Format("{0},{1},{2}", x.ToString(), y.ToString(), t.ToString());
		}
		public static bool TryParse(string s, out IPointXYT result)
		{
			result = new IPointXYT(0, 0, 0);
			string[] splited = s.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (splited.Length != 3)
				return false;

			if (false == int.TryParse(splited[0], out result.x)
				|| false == int.TryParse(splited[1], out result.y)
				|| false == int.TryParse(splited[2], out result.t))
				return false;

			return true;
		}
	}
	// 2022.05.23 by junho [ADD] Rectangle 추가
	public struct DRectangle
	{
		#region <FILED>
		private double _x;
		private double _y;
		private double _width;
		private double _height;

		private double _left;
		private double _right;
		private double _top;
		private double _bottom;
		#endregion </FILED>

		#region <CONSTRUCTOR>
		public DRectangle(double x, double y, double width, double height)
		{
			_x = x;
			_y = y;
			_width = width;
			_height = height;

			if (0 <= _width)
			{
				_left = _x;
				_right = _x + _width;
			}
			else
			{
				_left = _x + _width;
				_right = _x;
			}
			if (0 <= _height)
			{
				_top = _y + _height;
				_bottom = _y;
			}

			else
			{
				_top = _y;
				_bottom = _y + _height;
			}
		}
		public DRectangle(DRectangle sp)
			: this(sp.x, sp.y, sp.width, sp.height)
		{
		}
		#endregion </CONSTRUCTOR>

		#region <PROPERTY>
		public double x
		{
			get { return _x; }
			set
			{
				_x = value;
				UpdateX();
			}
		}
		public double y
		{
			get { return _y; }
			set
			{
				_y = value;
				UpdateY();
			}
		}
		public double width
		{
			get { return _width; }
			set
			{
				_width = value;
				UpdateX();
			}
		}
		public double height
		{
			get { return _height; }
			set
			{
				_height = value;
				UpdateY();
			}
		}
		public double left
		{
			get { return _left; }
			set
			{
				if (0 <= _width)
				{
					_width = _x - value + _width;
					x = value;
				}
				else
				{
					_width = value - _x;
				}
				UpdateX();
			}
		}
		public double right
		{
			get { return _right; }
			set
			{
				if (0 <= _width)
				{
					_width = value - _x;
				}
				else
				{
					_width = _x - value + _width;
					_x = value;
				}
				UpdateX();
			}
		}
		public double top
		{
			get { return _top; }
			set
			{
				if (0 <= _height)
				{
					_height = value - _y;
				}
				else
				{
					_height = _y - value + _height;
					_y = value;
				}
				UpdateY();
			}
		}
		public double bottom
		{
			get { return _bottom; }
			set
			{
				if (0 <= _height)
				{
					_height = _y - value + _height;
					_y = value;
				}
				else
				{
					_height = value - _y;
				}
				UpdateY();
			}
		}
		#endregion </PROPERTY>

		#region <OPERATOR>
		// +-*/ 는 어떤 방식으로 계산할지 특정할 수 없으니 구현 불가
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (false == obj is DRectangle) return false;

			DRectangle target = (DRectangle)obj;
			return this.x == target.x
				&& this.y == target.y
				&& this.width == target.width
				&& this.height == target.height;
		}
		public override int GetHashCode()
		{
			return x.GetHashCode()
				^ y.GetHashCode()
				^ width.GetHashCode()
				^ height.GetHashCode();
		}

		public static bool operator ==(DRectangle rect1, DRectangle rect2)
		{
			return rect1.x == rect2.x
				&& rect1.y == rect2.y
				&& rect1.width == rect2.width
				&& rect1.height == rect2.height;
		}
		public static bool operator !=(DRectangle rect1, DRectangle rect2)
		{
			return rect1.x != rect2.x
				|| rect1.y != rect2.y
				|| rect1.width != rect2.width
				|| rect1.height != rect2.height;
		}
		#endregion </OPERATOR>

		#region <METHOD>
		private void UpdateX()
		{
			if (0 <= _width)
			{
				_left = _x;
				_right = _x + _width;
			}
			else
			{
				_left = _x + _width;
				_right = _x;
			}
		}
		private void UpdateY()
		{
			if (0 <= _height)
			{
				_top = _y + _height;
				_bottom = _y;
			}
			else
			{
				_top = _y;
				_bottom = _y + _height;
			}
		}
		#endregion </METHOD>
	}
}
