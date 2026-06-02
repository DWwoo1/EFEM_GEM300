using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using ZedGraph;

using RegisteredInstances_;

namespace FrameOfSystem3.Views.Operation
{
	public partial class Operation_Tracking : UserControlForMainView.CustomView
	{
		public Operation_Tracking()
		{
			InitializeComponent();

			#region InitializeInstance
			m_InstanceOfSelectionList	= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfAnalogIO		= FrameOfSystem3.Config.ConfigAnalogIO.GetInstance();
			m_InstanceOfCylinder		= FrameOfSystem3.Config.ConfigCylinder.GetInstance();
			m_InstanceOfDgitalIO		= FrameOfSystem3.Config.ConfigDigitalIO.GetInstance();
			m_InstanceOfMotion			= FrameOfSystem3.Config.ConfigMotion.GetInstance();
			m_InstanceDevice			= DetectControllDevice.GetInstance();
			#endregion

			#region MakeMapping
			m_dicOfMapping.Clear();
			foreach(EN_TYPE_DEVICE en in Enum.GetValues(typeof(EN_TYPE_DEVICE)))
			{
				m_dicOfMapping.Add(en.ToString(), en);
			}
			m_listOfDic.Add(m_dicOfMotionMapping);
			m_listOfDic.Add(m_dicOfAnalogMapping);
			m_listOfDic.Add(m_dicOfCylinderMapping);
			m_listOfDic.Add(m_dicOfDigitalMapping);
			#endregion

			#region TimerInit
			m_timerForGetValue.Tick		+= FunctionForTimer;
			m_timerForGetValue.Interval	= 10;
			#endregion
			
			InitializeGraph();

		}
		
		#region 상수
		private const int MAX_ITEM					= 10;
		private readonly Color[] ColorLine			= new Color[MAX_ITEM] {Color.Orange, Color.Sienna, Color.OliveDrab, Color.DodgerBlue, Color.MediumSpringGreen, Color.OrangeRed, Color.Indigo, Color.DarkMagenta, Color.DeepPink, Color.SaddleBrown};
		#endregion

		#region 변수
		private bool m_bUpdateGraph										= false;

		private Dictionary<int, DeviceInfo> m_dicOfMotionMapping		= new Dictionary<int,DeviceInfo>();
		private Dictionary<int, DeviceInfo> m_dicOfAnalogMapping		= new Dictionary<int,DeviceInfo>();
		private Dictionary<int, DeviceInfo> m_dicOfDigitalMapping		= new Dictionary<int,DeviceInfo>();
		private Dictionary<int, DeviceInfo> m_dicOfCylinderMapping		= new Dictionary<int,DeviceInfo>();
		private List<Dictionary<int, DeviceInfo>> m_listOfDic			= new List<Dictionary<int,DeviceInfo>>();

		private Dictionary<string, EN_TYPE_DEVICE> m_dicOfMapping	= new Dictionary<string,EN_TYPE_DEVICE>();

		#region Instance
		private Functional.Form_SelectionList m_InstanceOfSelectionList		    = null;
		private FrameOfSystem3.Config.ConfigMotion m_InstanceOfMotion			= null;
		private FrameOfSystem3.Config.ConfigDigitalIO m_InstanceOfDgitalIO		= null;
		private FrameOfSystem3.Config.ConfigAnalogIO m_InstanceOfAnalogIO		= null;
		private FrameOfSystem3.Config.ConfigCylinder m_InstanceOfCylinder		= null;
		private DetectControllDevice m_InstanceDevice			= null;
		#endregion

		#region Timer
		private Timer m_timerForGetValue	= new Timer();
		private Stopwatch stWatch			= new Stopwatch();
		#endregion

		#endregion

		#region 상속 인터페이스
		/// <summary>
		/// 
		/// </summary>
		protected override void ProcessWhenActivation()
		{
			ResetDeviceValue();
		}
		/// <summary>
		/// 
		/// </summary>
		protected override void ProcessWhenDeactivation()
		{
			
		}
		/// <summary>
		/// 
		/// </summary>
		public override void CallFunctionByTimer()
		{
			if(false == m_bUpdateGraph)
			{
				return;
			}

			foreach(Dictionary<int, DeviceInfo> dicOfDevice in m_listOfDic)
			{
				if(dicOfDevice.Count < 1)
				{
					continue;
				}

				foreach(DeviceInfo dvinfo in dicOfDevice.Values)
				{
					if (dvinfo.m_bChangeValue)
					{
						dvinfo.m_PointList.Add(stWatch.Elapsed.TotalSeconds, dvinfo.m_bOn ? dvinfo.m_nGraphLevel : dvinfo.m_nGraphLevel + 1);
						dvinfo.m_bChangeValue = false;
					}
					dvinfo.m_PointList.Add(stWatch.Elapsed.TotalSeconds, dvinfo.m_bOn ? dvinfo.m_nGraphLevel + 1 : dvinfo.m_nGraphLevel);
					switch(dvinfo.m_enType)
					{
						case EN_TYPE_DEVICE.TYPE_ANALOG_OUT:
							dvinfo.m_bOn			= false;
							break;
						default:
							break;
					}
				}
			}

			zedGraphControl1.AxisChange();
			zedGraphControl1.GraphPane.YAxis.Scale.Max		= 8;
			zedGraphControl1.GraphPane.YAxis.Scale.Min		= -0.2;
			zedGraphControl1.Invalidate();

			if((zedGraphControl1.GraphPane.XAxis.Scale.Max - zedGraphControl1.GraphPane.XAxis.Scale.Min) > 10)
			{
				zedGraphControl1.GraphPane.XAxis.Scale.Min	= zedGraphControl1.GraphPane.XAxis.Scale.Max - 10;
			}

		}
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.10.15 by twkang [ADD] 그래프를 초기 설정한다.
		/// </summary>
		private void InitializeGraph()
		{
			zedGraphControl1.GraphPane.Title.IsVisible				= false;
			zedGraphControl1.GraphPane.Legend.FontSpec.Size			= 6;
			zedGraphControl1.GraphPane.XAxis.Title.Text				= "REAL TIME(s)";
			zedGraphControl1.GraphPane.YAxis.Scale.FontSpec.Size	= 8;
			zedGraphControl1.GraphPane.XAxis.Scale.FontSpec.Size	= 8;
			zedGraphControl1.GraphPane.YAxis.Title.Text				= "FALSE/TRUE";

			zedGraphControl1.GraphPane.YAxis.Scale.FontSpec.Size	= 1;
			zedGraphControl1.GraphPane.YAxis.MinorTic.IsOpposite	= true;
			zedGraphControl1.GraphPane.YAxis.MinorTic.IsAllTics		= false;
			
			
			zedGraphControl1.GraphPane.YAxis.MajorGrid.IsVisible	= true;

			
			zedGraphControl1.MouseWheel			+= zedGraphControl1_MouseWheel;
			zedGraphControl1.PointValueEvent	+= zedGraphControl1_PointValueEvent;
			zedGraphControl1.IsShowPointValues	= true;
		}

		/// <summary>
		/// 2020.10.13 by twkang [ADD] 모든 Device Value 값을 False 로 설정한다.
		/// </summary>
		private void ResetDeviceValue()
		{
			foreach (Dictionary<int, DeviceInfo> dicOfDevice in m_listOfDic)
			{
				if (dicOfDevice.Count < 1)
				{
					continue;
				}

				foreach (DeviceInfo dvinfo in dicOfDevice.Values)
				{
					dvinfo.m_bOn		= false;
				}
			}
		}
		/// <summary>
		/// 2020.10.13 by twkang [ADD] Device Tracking 을 설정한다.
		/// </summary>
		private void ActivateTracking(bool bOnTracking)
		{
			if(bOnTracking)
			{
				ResetDeviceValue();
				stWatch.Start();
				m_InstanceDevice.Start();
				m_timerForGetValue.Start();
				m_bUpdateGraph			= true;
			}
			else
			{
				m_bUpdateGraph			= false;
				m_InstanceDevice.Stop();
				m_timerForGetValue.Stop();
				stWatch.Stop();
			}
		}
		/// <summary>
		/// 2020.10.15 by twkang [ADD] 아이템의 이름을 가져온다.
		/// </summary>
		private bool GetDeviceName(EN_TYPE_DEVICE enType, int nIndex, ref string strName)
		{
			switch(enType)
			{
				case EN_TYPE_DEVICE.TYPE_ANALOG_OUT:
					return m_InstanceOfAnalogIO.GetParameter(false, nIndex, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.NAME, ref strName);
				case EN_TYPE_DEVICE.TYPE_CYLINDER:
					return m_InstanceOfCylinder.GetParameter(nIndex, FrameOfSystem3.Config.ConfigCylinder.EN_PARAM_CYLINDER.NAME, ref strName);
				case EN_TYPE_DEVICE.TYPE_DIGITAL_OUT:
					return m_InstanceOfDgitalIO.GetParameter(false, nIndex, FrameOfSystem3.Config.ConfigDigitalIO.EN_PARAM_DIGITALIO.NAME, ref strName);
				case EN_TYPE_DEVICE.TYPE_MOTION:
					return m_InstanceOfMotion.GetMotionParameter(nIndex, FrameOfSystem3.Config.ConfigMotion.EN_PARAM_MOTION.NAME, ref strName);
				default:
					return false;
			}
		}
		/// <summary>
		/// 2020.10.15 by twkang [ADD] 특정 Device Type 아이템을 그래프에서 지운다.
		/// </summary>
		private void RemoveCurveItem(ref Dictionary<int, DeviceInfo> dicOfDevice)
		{
			if (dicOfDevice.Count > 0)
			{
				foreach (DeviceInfo dvinfo in dicOfDevice.Values)
				{
					int nIndex = zedGraphControl1.GraphPane.CurveList.IndexOf(dvinfo.m_strName);
					if (nIndex != -1)
					{
						zedGraphControl1.GraphPane.CurveList.RemoveAt(nIndex);
					}
				}
			}

			dicOfDevice.Clear();
		}
		/// <summary>
		/// 2020.10.15 by twkang [ADD] 해당 아이템을 Graph 에 추가한다.
		/// </summary>
		private bool AddItemOnChart(EN_TYPE_DEVICE enType, int nGraphLevel, ref int[] arItem)
		{
			string strName					= string.Empty;

			Dictionary<int, DeviceInfo> dicOfdeviceInfo	= new Dictionary<int,DeviceInfo>();

			switch (enType)
			{
				case EN_TYPE_DEVICE.TYPE_ANALOG_OUT:
					dicOfdeviceInfo		= m_dicOfAnalogMapping;
					break;
				case EN_TYPE_DEVICE.TYPE_CYLINDER:
					dicOfdeviceInfo		= m_dicOfCylinderMapping;
					break;
				case EN_TYPE_DEVICE.TYPE_DIGITAL_OUT:
					dicOfdeviceInfo		= m_dicOfDigitalMapping;
					break;
				case EN_TYPE_DEVICE.TYPE_MOTION:
					dicOfdeviceInfo		= m_dicOfMotionMapping;
					break;
				default:
					return false;
			}

			RemoveCurveItem(ref dicOfdeviceInfo);
			
			for(int nIndex = 0, nEnd = arItem.Length; nIndex < nEnd; ++nIndex)
			{
				KeyValuePair<EN_TYPE_DEVICE, int> kvp	= new KeyValuePair<EN_TYPE_DEVICE, int>(enType, arItem[nIndex]);

				if (false == GetDeviceName(enType, arItem[nIndex], ref strName))
				{
					continue;
				}

				DeviceInfo	dvInfo	= new DeviceInfo(enType, strName, arItem[nIndex], nGraphLevel, new RollingPointPairList(500));

				var LineItem = zedGraphControl1.GraphPane.AddCurve(strName, dvInfo.m_PointList, ColorLine[nIndex], SymbolType.None);
				LineItem.Line.Width	= 3;

				if(false == dicOfdeviceInfo.ContainsKey(arItem[nIndex]))
				{
					dicOfdeviceInfo.Add(arItem[nIndex], dvInfo);
				}
			}

			return false;
		}
		/// <summary>
		/// 2020.10.15 by twkang [ADD] 특정 타입의 아이템 리스트를 가져온다.
		/// </summary>
		private void GetItemList(EN_TYPE_DEVICE enType, ref int[] arItemIndex)
		{
			string[] arItem	= null;
			int[] arIndex	= null;

			switch(enType)
			{
				case EN_TYPE_DEVICE.TYPE_ANALOG_OUT:
					if(m_InstanceOfAnalogIO.GetListOfItem(false, ref arIndex) && m_InstanceOfAnalogIO.GetListOfName(false, ref arItem))
					{
						if(m_InstanceOfSelectionList.CreateForm(enType.ToString(), arItem, arIndex, -1, true))
						{
							m_InstanceOfSelectionList.GetResult(ref arItemIndex);
						}
					}
					break;
				case EN_TYPE_DEVICE.TYPE_CYLINDER:
					if(m_InstanceOfCylinder.GetListOfItem(ref arIndex) && m_InstanceOfCylinder.GetListOfName(ref arItem))
					{
						if(m_InstanceOfSelectionList.CreateForm(enType.ToString(), arItem, arIndex, -1, true))
						{
							m_InstanceOfSelectionList.GetResult(ref arItemIndex);
						}
					}
					break;
				case EN_TYPE_DEVICE.TYPE_DIGITAL_OUT:
					if(m_InstanceOfDgitalIO.GetListOfItem(false, ref arIndex) && m_InstanceOfDgitalIO.GetListOfName(false, ref arItem))
					{
						if(m_InstanceOfSelectionList.CreateForm(enType.ToString(), arItem, arIndex, -1, true))
						{
							m_InstanceOfSelectionList.GetResult(ref arItemIndex);
						}
					}
					break;
				case EN_TYPE_DEVICE.TYPE_MOTION:
					if(m_InstanceOfMotion.GetListOfName(ref arItem) && m_InstanceOfMotion.GetListOfItem(ref arIndex))
					{
						if(m_InstanceOfSelectionList.CreateForm(enType.ToString(), arItem, arIndex, -1, true))
						{
							m_InstanceOfSelectionList.GetResult(ref arItemIndex);
						}
					}
					break;
				case EN_TYPE_DEVICE.TYPE_NONE:
					break;
			}
		}
		/// <summary>
		/// 2020.10.13 by twkang [ADD] 타이머에 의해 호출되는 함수, Device 정보를 가져온다
		/// </summary>
		private void FunctionForTimer(object sender, EventArgs e)
		{
			EN_TYPE_DEVICE enType	= EN_TYPE_DEVICE.TYPE_ANALOG_OUT;
			int nIndex		        = -1;
			bool bResult	        = false;
			while(m_InstanceDevice.Get(ref enType, ref nIndex, ref bResult))
			{
				Dictionary<int, DeviceInfo> dictionary	= new Dictionary<int,DeviceInfo>();

				switch(enType)
				{
					case EN_TYPE_DEVICE.TYPE_ANALOG_OUT:
						dictionary		= m_dicOfAnalogMapping;
						break;
					case EN_TYPE_DEVICE.TYPE_CYLINDER:
						dictionary		= m_dicOfCylinderMapping;
						break;
					case EN_TYPE_DEVICE.TYPE_DIGITAL_OUT:
						dictionary		= m_dicOfDigitalMapping;
						break;
					case EN_TYPE_DEVICE.TYPE_MOTION:
						dictionary		= m_dicOfMotionMapping;
						break;
					default:
						return;
				}

				if(dictionary.ContainsKey(nIndex))
				{
					if(dictionary[nIndex].m_bOn != bResult)
					{
						dictionary[nIndex].m_bChangeValue	= true;
					}
					dictionary[nIndex].m_bOn	= bResult;
				}
			}
		}
		#endregion

		#region UI인터페이스
		/// <summary>
		/// 2020.10.15 by twkang [ADD] ADD 버튼 클릭 이벤트
		/// </summary>
		private void Click_AddButton(object sender, EventArgs e)
		{
			Control ctrl		= sender as Control;

			int[] arIndex		= null;
			EN_TYPE_DEVICE enType	= EN_TYPE_DEVICE.TYPE_MOTION;

			switch(ctrl.TabIndex)
			{
				case 0: // MOTION
					GetItemList(EN_TYPE_DEVICE.TYPE_MOTION, ref arIndex);
					enType		= EN_TYPE_DEVICE.TYPE_MOTION;
					break;
				case 1:	// DIGITAL
					GetItemList(EN_TYPE_DEVICE.TYPE_DIGITAL_OUT, ref arIndex);
					enType		= EN_TYPE_DEVICE.TYPE_DIGITAL_OUT;
					break;
				case 2:	// ANALOG
					GetItemList(EN_TYPE_DEVICE.TYPE_ANALOG_OUT, ref arIndex);
					enType		= EN_TYPE_DEVICE.TYPE_ANALOG_OUT;
					break;
				case 3:	// CYLINDER
					GetItemList(EN_TYPE_DEVICE.TYPE_CYLINDER, ref arIndex);
					enType		= EN_TYPE_DEVICE.TYPE_CYLINDER;
					break;
			}

			if(null == arIndex)
			{
				return;
			}

			if(arIndex.Length > MAX_ITEM)
			{
				Array.Resize(ref arIndex, MAX_ITEM);
			}

			AddItemOnChart(enType, ctrl.TabIndex << 1, ref arIndex);
		}
		/// <summary>
		/// 2020.10.15 by twkang [ADD] Clear Item 버튼 클릭 이벤트
		/// </summary>
		private void Click_ClearItem(object sender, EventArgs e)
		{
			ActivateTracking(false);
			m_ToggleInputOnDelay.Active	= false;


			foreach(var dicOfDevice in m_listOfDic)
			{
				if(dicOfDevice.Count < 1)
				{
					continue;
				}

				Dictionary<int, DeviceInfo> dictionary	= dicOfDevice;

				RemoveCurveItem(ref dictionary);
			}

			zedGraphControl1.Refresh();
			zedGraphControl1.Invalidate();
		}
		/// <summary>
		/// 2020.10.15 by twkang [ADD] DeviceTracking 온 오프 버튼 클릭 이벤트
		/// </summary>
		private void Click_ToggleButton(object sender, EventArgs e)
		{
			ActivateTracking(m_ToggleInputOnDelay.Active);
		}
		/// <summary>
		/// 2020.10.15 by twkang [ADD] 마우스 포인터 툴팁 이벤트
		/// </summary>
		private string zedGraphControl1_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
		{
			PointPair pt = curve[iPt];

			return string.Format("Device : {0} / RealTime : {1}", curve.Label.Text, pt.X);
		}
		/// <summary>
		/// 2020.10.15 by twkang [ADD] 그래프 마우스 휠 이벤트
		/// </summary>
		private void zedGraphControl1_MouseWheel(object sender, MouseEventArgs e)
		{
			zedGraphControl1.GraphPane.YAxis.Scale.Max = 8;
			zedGraphControl1.GraphPane.YAxis.Scale.Min = -0.2;
		}
		#endregion

		/// <summary>
		/// 2020.10.15 by twkang [ADD] 그래프에서 사용할 클래스
		/// </summary>
		private class DeviceInfo
		{
			public string m_strName		= string.Empty;
			public int m_Index			= -1;
			public bool m_bOn			= false;
			public bool m_bChangeValue	= false;
			public int m_nGraphLevel	= 0;

			public RollingPointPairList m_PointList		= new RollingPointPairList(500);
			public EN_TYPE_DEVICE m_enType	            = new EN_TYPE_DEVICE();
                            
			public DeviceInfo(EN_TYPE_DEVICE entype, string strName, int nIndex, int nGraphLevel, RollingPointPairList pointList, bool bOn = false)
			{
				m_enType		= entype;
				m_strName		= strName;
				m_bOn			= bOn;
				m_Index			= nIndex;
				m_nGraphLevel	= nGraphLevel;
			}
		}
	}
}
