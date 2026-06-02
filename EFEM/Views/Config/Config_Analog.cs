using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using ZedGraph;

using Define.DefineConstant;
using RegisteredInstances_;
using FrameOfSystem3.Views.Config.Analog;

namespace FrameOfSystem3.Views.Config
{
    public partial class Config_Analog : UserControlForMainView.CustomView
	{
        enum EN_SELECTED_ANALOG_TYPE
        { 
            INPUT = 0,
            OUTPUT = 1,
        }

		#region 상수
		private const int HEIGHT_OF_ROWS			= 30;

		Sys3Controls.Sys3button m_btnSelectedTab = null;
        
		Sys3Controls.Sys3button[] m_btnInputSelected = new Sys3Controls.Sys3button[] { null, null, null};
		Sys3Controls.Sys3button[] m_btnOutputSelected = new Sys3Controls.Sys3button[] { null, null, null};

		//파라미터들의 최대, 최소값
		private readonly string MIN_OF_TARGET		= "-1";
		private readonly string MIN_OF_BIT			= "0";
		private readonly string MAX_OF_BIT			= "16";
		private readonly string MIN_OF_VALUE		= "0";
		private readonly string MAX_OF_VALUE		= "999999";

		private const int SELECT_NONE				= -1;

        private const int DEVICEINFO_ANALOG_INPUT   = 0;
        private const int DEVICEINFO_ANALOG_OUTPUT  = 1;

		enum EN_TYPE { AnalogInput, AnalogOutput };

        private Stopwatch m_StopWatch = null;

		#region GUI
		private readonly string LEFT_ARROW			= "◀";
		private readonly string RIGHT_ARROW			= "▶";

		private const int EXTEND_WIDTH_X			= 278;

		private readonly Color c_clrTrue			= Color.DodgerBlue;
		private readonly Color c_clrFalse			= Color.White;
		#endregion

		#region GridView Column Index
		private const int COLUMN_INDEX_OF_INDEX		= 0;
		private const int COLUMN_INDEX_OF_ENABLE	= 1;
		private const int COLUMN_INDEX_OF_NAME		= 2;
		private const int COLUMN_INDEX_OF_TAGNUMER	= 3;
		private const int COLUMN_INDEX_OF_TARGET	= 4;
		private const int COLUMN_INDEX_OF_BIT		= 5;
        private const int COLUMN_INDEX_OF_DATA_TYPE = 6;
		private const int COLUMN_INDEX_OF_MIN		= 7;
		private const int COLUMN_INDEX_OF_MAX		= 8;
		private const int COLUMN_INDEX_OF_VOLT		= 9;
		private const int COLUMN_INDEX_OF_VALUE		= 10;

		private const int COLUMN_INDEX_OF_TABLE_VOLT	= 1;
		private const int COLUMN_INDEX_OF_TABLE_VALUE	= 2;
		#endregion
		
		#endregion

		#region 변수
		private int m_nCountOfInputGroup		= 0;
		private int m_nCountOfOutputGroup		= 0;

		private int m_nIndexOfSelectedOfInput	= -1;
		private int m_nIndexOfSelectedOfOutput	= -1;

		private int m_nIndexOfSelectedRowsInput		= -1;
		private int m_nIndexOfSelectedRowsOutput	= -1;

		private int[] m_nArrIndexOfInputItem			= null;
		private int[] m_nArrIndexOfOutputItem			= null;

        private int[] m_arSelectedInputItems        = null;
        private int[] m_arSelectedOutputItems       = null;

        private readonly string[] m_arSelectedType = { "VOLT TYPE", "VALUE TYPE" };


		Functional.Form_SelectionList m_InstanceOfSelectionList		= null;
		Functional.Form_Keyboard m_InstanceOfKeyboard				= null;
		Functional.Form_Calculator m_InstanceOfCalculator			= null;
		FrameOfSystem3.Config.ConfigAnalogIO m_InstanceOfAnalogIO			= null;
		Functional.Form_MessageBox m_InstanceOfMessageBox			= null;


		// modify New variable //
		Dictionary<int,DeviceInfo> m_InputDeviceInfo = new Dictionary<int, DeviceInfo>();
        Dictionary<int, DeviceInfo> m_OutputDeviceInfo = new Dictionary<int, DeviceInfo>();
        List<Dictionary<int, DeviceInfo>> m_ListDeviceInfo = new List<Dictionary<int, DeviceInfo>>();

		System.Array colorsArray;
		KnownColor[] allColors;
		private readonly Color[] ColorLine = new Color[10] { Color.Orange, Color.Sienna, Color.OliveDrab, Color.DodgerBlue, Color.MediumSpringGreen, Color.OrangeRed, Color.Indigo, Color.DarkMagenta, Color.DeepPink, Color.SaddleBrown };

        private int ColorIndex = 0;
		bool m_bUpdateGraph = false;

        
        ConcurrentDictionary<int, Config_AnalogItemDisplay> m_dicAnalogItemDisplay = new ConcurrentDictionary<int, Config_AnalogItemDisplay>();

        // 2023.09.08. by shkim. [ADD] UI 크기 유동적으로 변경
        ConcurrentDictionary<int, Panel> m_dicAnalogItemDisplayPanel = new ConcurrentDictionary<int, Panel>();
        // 2023.09.08. by shkim. [END]


        int m_nSelectedAnalogViewType = -1;
        Config_AnalogItemDisplay m_ctrlAnalogItemDisplay_Input = new Config_AnalogItemDisplay();
        Config_AnalogItemDisplay m_ctrlAnalogItemDisplay_Output = new Config_AnalogItemDisplay();

		#region Timer
		private Stopwatch stWatch = new Stopwatch();
		#endregion

		#endregion

		public Config_Analog()
        {
            InitializeComponent();

			this.DoubleBuffered					= true;
			m_InstanceOfAnalogIO				= FrameOfSystem3.Config.ConfigAnalogIO.GetInstance();
			m_InstanceOfCalculator				= Functional.Form_Calculator.GetInstance();
			m_InstanceOfKeyboard				= Functional.Form_Keyboard.GetInstance();
			m_InstanceOfSelectionList			= Functional.Form_SelectionList.GetInstance();
			m_InstanceOfMessageBox				= Functional.Form_MessageBox.GetInstance();

            m_StopWatch = new Stopwatch();

            m_ListDeviceInfo.Add(m_InputDeviceInfo);
            m_ListDeviceInfo.Add(m_OutputDeviceInfo);

            InitAnalogItemDisplay();
            
			m_btnSelectedTab = m_btnInput;

			m_btnInputSelected = new Sys3Controls.Sys3button[] { m_btnInputAdd, m_btnInputRemove, m_btnInputExtend };
			m_btnOutputSelected = new Sys3Controls.Sys3button[] { m_btnOutputAdd, m_btnOutputRemove, m_btnOutputExtend };

			InitializeGraph();
		}

        #region <AnalogItemView>
        void InitAnalogItemDisplay()
        {
            m_dicAnalogItemDisplay.TryAdd((int)EN_SELECTED_ANALOG_TYPE.INPUT, m_ctrlAnalogItemDisplay_Input);
            m_dicAnalogItemDisplay.TryAdd((int)EN_SELECTED_ANALOG_TYPE.OUTPUT, m_ctrlAnalogItemDisplay_Output);

            DataGridView itemDataGridView = GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.INPUT);
            if (itemDataGridView != null)
            {
                m_groupInputList.Controls.Add(itemDataGridView);
                itemDataGridView.Dock = DockStyle.Fill;

                itemDataGridView.CellClick += Click_InputItem;
                itemDataGridView.CellDoubleClick += DoubleClick_InputItem;
            }
            itemDataGridView = GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.OUTPUT);
            if (itemDataGridView != null)
            {
                m_groupOutputList.Controls.Add(itemDataGridView);
                itemDataGridView.Dock = DockStyle.Fill;

                itemDataGridView.CellClick += Click_OutputItem;
                itemDataGridView.CellDoubleClick += DoubleClick_OutputItem;
            }

            m_nSelectedAnalogViewType = (int)EN_SELECTED_ANALOG_TYPE.OUTPUT;
            ShowAnalogItemDisplay((int)EN_SELECTED_ANALOG_TYPE.INPUT);
        }
        void ShowAnalogItemDisplay(int nShowTarget_AnalogItemView)
        {
            if(m_nSelectedAnalogViewType != nShowTarget_AnalogItemView)
            {
                _tableLayoutPanel_Main.SuspendLayout();

                _tableLayoutPanel_List_InputOutput.RowStyles[m_nSelectedAnalogViewType].Height = 0;
                _tableLayoutPanel_List_InputOutput.RowStyles[nShowTarget_AnalogItemView].Height = _tableLayoutPanel_List_InputOutput.Height;

                _tableLayoutPanel_LookUpTable_InputOutput.RowStyles[m_nSelectedAnalogViewType].Height = 0;
                _tableLayoutPanel_LookUpTable_InputOutput.RowStyles[nShowTarget_AnalogItemView].Height = _tableLayoutPanel_LookUpTable_InputOutput.Height;
                
                m_nSelectedAnalogViewType = nShowTarget_AnalogItemView;

                _tableLayoutPanel_Main.ResumeLayout(true);
            }
        }

        DataGridView GetDataGridViewByInOut(int nSelectTarget_AnalogItemView)
        {
            Config_AnalogItemDisplay ctrlAnalogItemDisplay = null;
            if (m_dicAnalogItemDisplay.TryGetValue(nSelectTarget_AnalogItemView, out ctrlAnalogItemDisplay))
            {
                return ctrlAnalogItemDisplay.ItemDataGridView;
            }
            return null;
        }
        #endregion </AnalogItemView>

        #region 상속 인터페이스
        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessWhenActivation()
        {
			InitializeInputList();

            DataGridView dgViewList = null;

			if(SELECT_NONE != m_nIndexOfSelectedRowsInput)
			{
                dgViewList = GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.INPUT);
                dgViewList.Rows[m_nIndexOfSelectedRowsInput].Selected = true;
			}

			InitializeOutputList();

			if(SELECT_NONE != m_nIndexOfSelectedRowsOutput)
			{
                dgViewList = GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.OUTPUT);
                dgViewList.Rows[m_nIndexOfSelectedRowsOutput].Selected = true;
			}
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
			UpdateInputValue();
			UpdateOutputValue();
			if(m_bUpdateGraph)
            {
				m_ZedGraph.AxisChange();
				m_ZedGraph.Refresh();
			}
		}
        #endregion

        #region 외부인터페이스

        #endregion

        #region 내부인터페이스

        private void InitializeGraph()
        {
            m_ZedGraph.GraphPane.Title.IsVisible = false;
            m_ZedGraph.GraphPane.Legend.FontSpec.Size = 6;
            m_ZedGraph.GraphPane.XAxis.Title.Text = "REAL TIME(s)";
            m_ZedGraph.GraphPane.YAxis.Scale.FontSpec.Size = 8;
            m_ZedGraph.GraphPane.XAxis.Scale.FontSpec.Size = 8;

            // modify //
            m_ZedGraph.GraphPane.YAxis.Title.Text = "INPUT/OUTPUT VOLTAGE";
            // modify end //

            m_ZedGraph.GraphPane.YAxis.Scale.FontSpec.Size = 8;
            m_ZedGraph.GraphPane.YAxis.MinorTic.IsOpposite = true;
            m_ZedGraph.GraphPane.YAxis.MinorTic.IsAllTics = false;
					

            m_ZedGraph.GraphPane.YAxis.MajorGrid.IsVisible = true;


            //m_ZedGraph.MouseWheel += m_zedGraph_MouseWheel;
            //m_ZedGraph.PointValueEvent += m_zedGraph_PointValueEvent;
            m_ZedGraph.IsShowPointValues = true;

            //	// modify //
            colorsArray = Enum.GetValues(typeof(KnownColor));
            allColors = new KnownColor[colorsArray.Length];
            Array.Copy(colorsArray, allColors, colorsArray.Length);
            //	 // modify end //

        }

		/// <summary>
		/// 2020.06.15 by twkang [ADD] InputList의 Volt, Value 값을 업데이트한다.
		/// </summary>
		private void UpdateInputValue()
		{
			int[] arIndex			= null;
			if(false == m_InstanceOfAnalogIO.GetListOfItem(true,  ref arIndex))
			{
				return;
			}

            DataGridView dgViewList = null;
            dgViewList = GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.INPUT);

			for (int nIndex = 0, nEnd = arIndex.Length; nIndex < nEnd; ++nIndex)
			{
                dgViewList[COLUMN_INDEX_OF_VOLT, nIndex].Value = String.Format("{0:0.000}", m_InstanceOfAnalogIO.ReadInputVolt(arIndex[nIndex]));
                dgViewList[COLUMN_INDEX_OF_VALUE, nIndex].Value = String.Format("{0:0.000}", m_InstanceOfAnalogIO.ReadInputValue(arIndex[nIndex]));

                if (true == m_bUpdateGraph)
                    // 그래프에 들어가는 값 갱신
                    if (m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT].ContainsKey(nIndex))
                    {
                        if(m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT][nIndex].On)
                            if(m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT][nIndex].IsVoltType)
                                m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT][nIndex].AddValue(m_StopWatch.Elapsed.TotalSeconds, Convert.ToDouble(dgViewList[COLUMN_INDEX_OF_VOLT, nIndex].Value));
                            else
                                m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT][nIndex].AddValue(m_StopWatch.Elapsed.TotalSeconds, Convert.ToDouble(dgViewList[COLUMN_INDEX_OF_VALUE, nIndex].Value));
                    }
			}
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] OuputList의 Volt, Value 값을 업데이트한다.
		/// </summary>
		private void UpdateOutputValue() 
		{
			int[] arIndex			= null;	
			if(false == m_InstanceOfAnalogIO.GetListOfItem(false, ref arIndex))
			{
				return;
			}
            DataGridView dgViewList = null;
            dgViewList = GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.OUTPUT);

			for (int nIndex = 0, nEnd = arIndex.Length; nIndex < nEnd; ++nIndex)
			{
                dgViewList[COLUMN_INDEX_OF_VOLT, nIndex].Value = String.Format("{0:0.000}", m_InstanceOfAnalogIO.ReadOutputVolt(arIndex[nIndex]));
                dgViewList[COLUMN_INDEX_OF_VALUE, nIndex].Value = String.Format("{0:0.000}", m_InstanceOfAnalogIO.ReadOutputValue(arIndex[nIndex]));

                if (true == m_bUpdateGraph)
                    if (m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT].ContainsKey(nIndex))
                    {
                        if (true == m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT][nIndex].On)
                            if(m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT][nIndex].IsVoltType)
                                m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT][nIndex].AddValue(m_StopWatch.Elapsed.TotalSeconds, Convert.ToDouble(dgViewList[COLUMN_INDEX_OF_VOLT, nIndex].Value));
                            else
                                m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT][nIndex].AddValue(m_StopWatch.Elapsed.TotalSeconds, Convert.ToDouble(dgViewList[COLUMN_INDEX_OF_VALUE, nIndex].Value));
                    }
			}
		}
		/// <summary>
		/// 2020.06.08 by twkang [ADD] AnalogIO INPUT 아이템들의 값을 갱신한다.
		/// </summary>
		private void InitializeInputList()
		{
			if(false == m_InstanceOfAnalogIO.GetListOfItem(true, ref m_nArrIndexOfInputItem))
			{
				return;
			}

			m_nCountOfInputGroup	= m_nArrIndexOfInputItem.Length;

            DataGridView dgViewList = null;
            dgViewList = GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.INPUT);

            dgViewList.Rows.Clear();

			for(int i = 0; i < m_nCountOfInputGroup; i++)
			{
				AddItemOnGrid(true, m_nArrIndexOfInputItem[i]);
			}

		}
		/// <summary>
		/// 2020.07.01 by twkang [ADD] Input, 또는 Output 아이템을 GridView 에 추가한다.
		/// </summary>
		private void AddItemOnGrid(bool bInput, int nIndexOfItem)
		{
            DataGridView dgViewList = bInput ? GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.INPUT) : GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.OUTPUT);

			int nIndexOfRow		= dgViewList.Rows.Count;

			dgViewList.Rows.Add();

			bool bResult		= false;
			string strValue		= string.Empty;

			dgViewList.Rows[nIndexOfRow].Height = HEIGHT_OF_ROWS;

			dgViewList[COLUMN_INDEX_OF_INDEX, nIndexOfRow].Value = nIndexOfItem.ToString();
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.BackColor = Color.Silver;
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionBackColor = Color.Silver;
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionForeColor = Color.Black;

			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_VALUE].Style.BackColor = Color.Silver;
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_VALUE].Style.SelectionBackColor = Color.Silver;
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_VALUE].Style.SelectionForeColor = Color.Black;

			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_VOLT].Style.BackColor = Color.Silver;
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_VOLT].Style.SelectionBackColor = Color.Silver;
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_VOLT].Style.SelectionForeColor = Color.Black;

			bResult = false;
			m_InstanceOfAnalogIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.ENABLE, ref bResult);
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
			dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;

			m_InstanceOfAnalogIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.NAME, ref strValue);
			dgViewList[COLUMN_INDEX_OF_NAME, nIndexOfRow].Value		= strValue;

			m_InstanceOfAnalogIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.TAG_NO, ref strValue);
			dgViewList[COLUMN_INDEX_OF_TAGNUMER, nIndexOfRow].Value	= strValue;

			m_InstanceOfAnalogIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.TARGET, ref strValue);
			dgViewList[COLUMN_INDEX_OF_TARGET, nIndexOfRow].Value	= strValue;

			m_InstanceOfAnalogIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.BIT, ref strValue);
			dgViewList[COLUMN_INDEX_OF_BIT, nIndexOfRow].Value		= strValue;

            m_InstanceOfAnalogIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.RAW_DATA_TYPE, ref strValue);
            dgViewList[COLUMN_INDEX_OF_DATA_TYPE, nIndexOfRow].Value = strValue;

			m_InstanceOfAnalogIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.MIN, ref strValue);
			dgViewList[COLUMN_INDEX_OF_MIN, nIndexOfRow].Value		= strValue;

			m_InstanceOfAnalogIO.GetParameter(bInput, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.MAX, ref strValue);
			dgViewList[COLUMN_INDEX_OF_MAX, nIndexOfRow].Value		= strValue;

			dgViewList.Rows[nIndexOfRow].Selected	= false;
		}
		/// <summary>
		/// 2020.11.05 by twkang [ADD] Input 또는 Output Table 값을 GridView 에 추가한다.
		/// </summary>
		private void AddTableItemOnGrid(bool bInput, int nIndexOfItem)
		{
			DataGridView dgViewList		= bInput ? m_dgViewLookupInput : m_dgViewLookupOutput;
			
			double[] dbVolt		= null;
			double[] dbValue	= null;

			if (false == m_InstanceOfAnalogIO.GetDataAll(bInput, nIndexOfItem, ref dbVolt, ref dbValue))
			{
				return;
			}

			for(int nIndex = 0 , nEnd = m_InstanceOfAnalogIO.GetColumnOfTable(); nIndex < nEnd; ++nIndex)
			{
				int nIndexOfRow		= dgViewList.Rows.Count;

				dgViewList.Rows.Add();

				dgViewList.Rows[nIndexOfRow].Height						= HEIGHT_OF_ROWS;
				dgViewList[COLUMN_INDEX_OF_INDEX, nIndexOfRow].Value	= nIndex.ToString();

				dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.BackColor			= Color.Silver;
				dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionBackColor	= Color.Silver;
				dgViewList.Rows[nIndexOfRow].Cells[COLUMN_INDEX_OF_INDEX].Style.SelectionForeColor	= Color.Black;

				dgViewList[COLUMN_INDEX_OF_TABLE_VOLT, nIndex].Value		= dbVolt[nIndex].ToString();
				dgViewList[COLUMN_INDEX_OF_TABLE_VALUE, nIndex].Value		= dbValue[nIndex].ToString();
			}
		}

		/// <summary>
		/// 2020.06.08 by twkang [ADD] AnalogIO OUTPUT 아이템들의 값을 갱신한다.
		/// </summary>
		private void InitializeOutputList()
		{
			if (false == m_InstanceOfAnalogIO.GetListOfItem(false, ref m_nArrIndexOfOutputItem))
			{
				return;
			}

			m_nCountOfOutputGroup = m_nArrIndexOfOutputItem.Length;

            DataGridView dgViewList = null;
            dgViewList = GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.OUTPUT);

            dgViewList.Rows.Clear();

			for (int i = 0; i < m_nCountOfOutputGroup; i++)
			{
				AddItemOnGrid(false, m_nArrIndexOfOutputItem[i]);
			}
		}
		/// <summary>
		/// 2020.07.01 by twkang [ADD] 선택한 아이템을 지운다.
		/// </summary>
		private bool RemoveItemOnGrid(bool bInput)
		{
            DataGridView dgViewList = bInput ? GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.INPUT) : GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.OUTPUT);
			DataGridView dgViewTable	= bInput ? m_dgViewLookupInput : m_dgViewLookupOutput;

			int nSelectIndex			= bInput ? m_nIndexOfSelectedRowsInput : m_nIndexOfSelectedRowsOutput;

			if(SELECT_NONE == nSelectIndex)
			{
				return false;
			}
			
			int nItemIndex	= int.Parse(dgViewList[COLUMN_INDEX_OF_INDEX, nSelectIndex].Value.ToString());

			if (m_InstanceOfAnalogIO.RemoveItem(bInput, nItemIndex))
			{
				dgViewList.Rows.RemoveAt(nSelectIndex);

				if(null != dgViewList.CurrentCell)
				{
					dgViewList.Rows[dgViewList.CurrentRow.Index].Selected		= false;
				}
				return true;
			}
			return false;
		}
		/// <summary>
		/// 2020.10.20 by twkang [ADD] 아이템 선택을 초기화한다.
		/// </summary>
		private void ResetSelectedInputItem()
		{
			m_nIndexOfSelectedOfInput		= SELECT_NONE;
			m_nIndexOfSelectedRowsInput		= SELECT_NONE;

			m_dgViewLookupInput.Rows.Clear();

			ActiveControls(true, false);
		}
		private void ResetSelectedOutputItem()
		{
			m_nIndexOfSelectedOfOutput		= SELECT_NONE;
			m_nIndexOfSelectedRowsOutput	= SELECT_NONE;

			m_dgViewLookupOutput.Rows.Clear();

			ActiveControls(false, false);
		}
		/// <summary>
		/// 2020.10.20 by twkang [ADD] 컨트롤들의 활성화 여부를 설정한다.
		/// </summary>
		private void ActiveControls(bool bInput, bool bEnable)
		{
			if(bInput)
			{
				m_btnInputRemove.Enabled	= bEnable;
			}
			else
			{
				m_btnOutputRemove.Enabled	= bEnable;
			}
		}

		/// <summary>
		/// 2021.03.11 by jhlee [ADD] 버튼에 따라 Graph의 활성화를 변경해준다.
		/// </summary>
		private void ActivateTracking(bool bOnTracking)
		{
			if (bOnTracking)
			{
				m_StopWatch.Start();
				m_bUpdateGraph = true;
			}
			else
			{
				m_bUpdateGraph = false;
                m_StopWatch.Stop();
			}
        }


        /// <summary>
        /// 2021.05.15 by jhlee [ADD] List를 가져와 Form 형성 및 ZedGraph 제작
        /// </summary>
        private void GetListOfItems(EN_TYPE enType, ref int[] arIndex)
        {
            int nCount = -1;
            int[] arItemIndex = null; 
            string[] arItemString = null;
            string Type = null;

            switch(enType)
            {
                case EN_TYPE.AnalogInput:
                    arItemIndex = m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT].Keys.ToArray();
                    nCount = m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT].Keys.Count();

                    if (false == m_InstanceOfAnalogIO.GetListOfName(true, ref arItemString))
                    {
                        return;
                    }

                    string[] arSelectedInputString = new string[nCount];

                    for (int nIndex = 0, nEnd = nCount; nIndex < nEnd; ++nIndex )
                    {
                        if (true == m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT][arItemIndex[nIndex]].On)
                            arSelectedInputString[nIndex] = arItemString[arItemIndex[nIndex]];
                    }

                    if (false == m_InstanceOfSelectionList.CreateForm("ANALOG INPUT", arItemString, m_nArrIndexOfInputItem, arSelectedInputString, true))
                    {
                        return;
                    }

                    break;

                case EN_TYPE.AnalogOutput:
                    arItemIndex = m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT].Keys.ToArray();
                    nCount = m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT].Keys.Count();


                    if ( false == m_InstanceOfAnalogIO.GetListOfName(false, ref arItemString))
                    {
                        return;
                    }

                    string[] arSelectedOutputString = new string[nCount];

                    for (int nIndex = 0, nEnd = nCount; nIndex < nEnd; ++nIndex)
                    {
                        if (true == m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT][arItemIndex[nIndex]].On)
                         arSelectedOutputString[nIndex] = arItemString[arItemIndex[nIndex]];
                    }


                    if (false == m_InstanceOfSelectionList.CreateForm("ANALOG OUTPUT", arItemString, m_nArrIndexOfOutputItem, arSelectedOutputString, true))
                    {
                        return;
                    }
                    break;
            }
            m_InstanceOfSelectionList.GetResult(ref arIndex);

            if (0 < arIndex.Length && m_InstanceOfSelectionList.CreateForm("SELECT TYPE", m_arSelectedType, ""))
                m_InstanceOfSelectionList.GetResult(ref Type);

            // 결과 값을 기반으로 ZedGraph 제작
            CreateZedGraph(arIndex, arItemString, enType, ref Type);
        }

        /// <summary>
        /// 2021.05.15 by jhlee [ADD] ZedGraph를 생성한다.
        /// 선택되지 않은 것들은 Active False
        /// </summary>
        /// 
        private void CreateZedGraph(int[] SelectedIndex, string[] arItemString, EN_TYPE enType, ref string strType)
        {
            switch(enType)
            {
                case EN_TYPE.AnalogInput:

                    // Active = false로 Setting
                    foreach(int nIndex in m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT].Keys)
                    {
                        m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT][nIndex].On = false;
                    }

                    // Active = true로 변경 and 새로운 Index가 추가 될 경우 Graph 추가 , 기존에 있던 그래프는..?
                    foreach(int nIndex in SelectedIndex)
                    {
                        // 아무것도 없을 때
                        if (false == m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT].ContainsKey(nIndex))
                        {
                            DeviceInfo Device = strType == 
                                "VOLT TYPE" ?   new DeviceInfo(EN_TYPE.AnalogInput, arItemString[nIndex], nIndex, new RollingPointPairList(500), true) :
                                                new DeviceInfo(EN_TYPE.AnalogInput, arItemString[nIndex], nIndex, new RollingPointPairList(500), false);
                            var LineItem = m_ZedGraph.GraphPane.AddCurve(arItemString[nIndex], Device.PorintList, ColorLine[nIndex % 10], SymbolType.None);
                            LineItem.Line.Width = 3;
                            m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT].Add(nIndex, Device);
                        }

                        else 
                        {
                            if(-1 == m_ZedGraph.GraphPane.CurveList.IndexOf(arItemString[nIndex]))
                            {
                                // 생성은 되었으나 Remove를 하지 않았을때 기존 data를 저장해 놓음
                                DeviceInfo Device = m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT][nIndex];
                                Device.IsVoltType = strType == "VOLT TYPE" ? true : false;
                                var LineItem = m_ZedGraph.GraphPane.AddCurve(arItemString[nIndex], Device.PorintList, ColorLine[nIndex % 10], SymbolType.None);
                                LineItem.Line.Width = 3;
                            }
                        }
                        m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT][nIndex].On = true;
                    }
                    // 선택되지 않은 것은 그래프  ZedGraph 화면에서 제거
                    int[] arInputKeys = m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT].Keys.ToArray();

                    for (int nIndex = 0, nEnd = arInputKeys.Length; nIndex < nEnd; ++nIndex)
                    {
                        if (false == m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT][arInputKeys[nIndex]].On)
                        {
                            if (m_ZedGraph.GraphPane.CurveList.Count < 1)
                            {
                                return;
                            }
                            m_ZedGraph.GraphPane.CurveList.RemoveAt(m_ZedGraph.GraphPane.CurveList.IndexOf(m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT][arInputKeys[nIndex]].Name));
                            m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT].Remove(arInputKeys[nIndex]);
                        }
                    }

                    break;

                case EN_TYPE.AnalogOutput:

                    foreach (int nIndex in m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT].Keys)
                    {
                        m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT][nIndex].On = false;
                    }

                    foreach(int nIndex in SelectedIndex)
                    {
                        if (false == m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT].ContainsKey(nIndex))
                        {
                            DeviceInfo Device = strType ==
                                "VOLT TYPE" ? new DeviceInfo(EN_TYPE.AnalogOutput, arItemString[nIndex], nIndex, new RollingPointPairList(500), true) :
                                                new DeviceInfo(EN_TYPE.AnalogOutput, arItemString[nIndex], nIndex, new RollingPointPairList(500), false);
                            var LineItem = m_ZedGraph.GraphPane.AddCurve(arItemString[nIndex], Device.PorintList, ColorLine[nIndex % 10], SymbolType.None);
                            LineItem.Line.Width = 3;
                            m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT].Add(nIndex, Device);
                        }

                        else
                        { 
                            if(-1 == m_ZedGraph.GraphPane.CurveList.IndexOf(arItemString[nIndex]))
                            {
                                DeviceInfo Device = m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT][nIndex];
                                Device.IsVoltType = strType == "VOLT TYPE" ? true : false;
                                ColorIndex = ColorIndex % 10;
                                var LineItem = m_ZedGraph.GraphPane.AddCurve(arItemString[nIndex], Device.PorintList, ColorLine[ColorIndex++], SymbolType.None);
                                LineItem.Line.Width = 3;
                            }
                        m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT][nIndex].On = true;
                        }
                    }

                    int[] arOutputKeys = m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT].Keys.ToArray();

                    for (int nIndex = 0, nEnd = arOutputKeys.Length; nIndex < nEnd; ++nIndex)
                    {
                        if (false == m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT][arOutputKeys[nIndex]].On)
                        {
                            if (m_ZedGraph.GraphPane.CurveList.Count < 1)
                            {
                                return;
                            }
                            m_ZedGraph.GraphPane.CurveList.RemoveAt(m_ZedGraph.GraphPane.CurveList.IndexOf(m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT][arOutputKeys[nIndex]].Name));
                            m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT].Remove(arOutputKeys[nIndex]);
                        }
                    }

                    break;
            }
        }

		/// <summary>
		/// 2021.07.28 by twkang [ADD] 버튼 클릭 확인
		/// </summary>
		private bool ConfirmButtonClick(string strActionName)
		{
			return m_InstanceOfMessageBox.ShowMessage(string.Format("Action : [{0}], This is a confirmation message", strActionName));
		}
        #endregion

        #region 외부 인터페이스

        #endregion

        #region 이벤트

        /// <summary>
        /// 2020.07.01 by twkang [ADD] Input View 버튼 클릭 이벤트
        /// </summary>
        private void Click_InputButtons(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;

            switch (ctrl.TabIndex)
            {
                case 0: // ADD
                    int nItem = m_InstanceOfAnalogIO.AddAnalogItem(true);
                    if (SELECT_NONE != nItem)
                    {
                        InitializeInputList();
                    }
                    break;
                case 1:	// REMOVE
					if(ConfirmButtonClick("REMOVE"))
					{
						RemoveItemOnGrid(true);
					}
                    break;
            }
            ResetSelectedInputItem();
        }
        /// <summary>
        /// 2020.07.01 by twkang [ADD] Output View 버튼 클릭 이벤트
        /// </summary>
        private void Click_OutputButtons(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;

            switch (ctrl.TabIndex)
            {
                case 0: // ADD
                    int nItem = m_InstanceOfAnalogIO.AddAnalogItem(false);
                    if (SELECT_NONE != nItem)
                    {
                        InitializeOutputList();
                    }
                    break;
                case 1:	// REMOVE
                    RemoveItemOnGrid(false);
                    break;
            }

            ResetSelectedOutputItem();
        }

        private void Click_Clear(object sender, EventArgs e)
        {
            foreach(DeviceInfo Device in m_ListDeviceInfo[DEVICEINFO_ANALOG_INPUT].Values)
            {
                Device.On = false;
            }

            foreach (DeviceInfo Device in m_ListDeviceInfo[DEVICEINFO_ANALOG_OUTPUT].Values)
            {
                Device.On = false;
            }

            m_ZedGraph.GraphPane.CurveList.Clear();
        }

        /// <summary>
		/// 2020.11.02 by twkang [ADD] 탭 전환 버튼을 클릭했을 경우 발생한다.
		/// 2021.03.11 by jhlee [MODIFY] 
		/// </summary>
		private void Click_ChangeList(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;

			if (m_btnSelectedTab.TabIndex == ctrl.TabIndex) { return; }

			m_btnSelectedTab.GradientFirstColor = Color.White;
			m_btnSelectedTab.GradientSecondColor = Color.White;
			m_btnSelectedTab.MainFontColor = Color.DarkBlue;
			m_btnSelectedTab.ButtonClicked = false;

			switch (ctrl.TabIndex)
			{
				case 0: // INPUT
                    ShowAnalogItemDisplay((int)EN_SELECTED_ANALOG_TYPE.INPUT);
					m_btnSelectedTab = m_btnInput;
					break;
				case 1: // OUTPUT
                    ShowAnalogItemDisplay((int)EN_SELECTED_ANALOG_TYPE.OUTPUT);
					m_btnSelectedTab = m_btnOutput;
					break;
			}
			m_btnSelectedTab.ButtonClicked = true;
			m_btnSelectedTab.GradientFirstColor = Color.DarkBlue;
			m_btnSelectedTab.GradientSecondColor = Color.DarkBlue;
			m_btnSelectedTab.MainFontColor = Color.White;
        }
        /// <summary>
		/// 2020.07.20 by twkang [ADD] 방향 버튼을 눌렀을 때
		/// </summary>
		private void Click_ArrowButton(object sender, EventArgs e)
		{
			Control ctrl	= sender as Control;

			switch(ctrl.TabIndex)
			{
				case 0: // INPUT LIST
					if(m_btnInputExtend.Text == LEFT_ARROW)
					{
                        m_btnInputAdd.Visible = true;
                        m_btnInputRemove.Visible = true;
						m_btnInputExtend.Text		= RIGHT_ARROW;
					}
					else
					{
                        m_btnInputAdd.Visible = false;
                        m_btnInputRemove.Visible = false;
						m_btnInputExtend.Text		= LEFT_ARROW;
						
					}
					m_groupInputList.Invalidate();
					break;
				case 1:	// OUTPUT LIST
					if(m_btnOutputExtend.Text == LEFT_ARROW)
					{
                        m_btnOutputAdd.Visible = true;
                        m_btnOutputRemove.Visible = true;
						m_btnOutputExtend.Text		= RIGHT_ARROW;
					}
					else
					{
                        m_btnOutputAdd.Visible = false;
                        m_btnOutputRemove.Visible = false;
						m_btnOutputExtend.Text		= LEFT_ARROW;
					}
					m_groupOutputList.Invalidate();
					break;
			}
		}
		/// <summary>
		/// 2020.10.15 by twkang [ADD] DeviceTracking 온 오프 버튼 클릭 이벤트
		/// </summary>
		private void Click_ToggleButton(object sender, EventArgs e)
		{
			ActivateTracking(m_ToggleInputOnDelay.Active);
		}

        private void Click_btnADD(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;

            switch(ctrl.TabIndex)
            {
                case 10:
                    GetListOfItems(EN_TYPE.AnalogInput, ref m_arSelectedInputItems);
                    break;

                case 11:
                    GetListOfItems(EN_TYPE.AnalogOutput, ref m_arSelectedOutputItems);
                    break;

                default:
                    break;
            }
        }


        /// <summary>
        /// 2020.06.08 by twkang [ADD] Output Grid View 클릭시 해당 아이템 인덱스를 저장한다.
        /// </summary>
        private void Click_OutputItem(object sender, DataGridViewCellEventArgs e)
        {
            int nRowIndex = e.RowIndex;

            DataGridView dgViewList = null;
            dgViewList = GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.OUTPUT);

            if (nRowIndex < 0 || nRowIndex >= dgViewList.RowCount) return;

            m_nIndexOfSelectedRowsOutput = nRowIndex;

            if (int.TryParse(dgViewList[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString(), out m_nIndexOfSelectedOfOutput))
            {
                m_dgViewLookupOutput.Rows.Clear();

                AddTableItemOnGrid(false, m_nIndexOfSelectedOfOutput);
                ActiveControls(false, true);
            }
        }
        /// <summary>
        /// 2020.06.08 by twkang [ADD] Input Grid View 클릭시 해당 아이템 인덱스를 저장한다.
        /// </summary>
        private void Click_InputItem(object sender, DataGridViewCellEventArgs e)
        {
            int nRowIndex = e.RowIndex;
            DataGridView dgViewList = null;
            dgViewList = GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.INPUT);

            if (nRowIndex < 0 || nRowIndex >= dgViewList.RowCount) return;

            m_nIndexOfSelectedRowsInput = nRowIndex;

            if (int.TryParse(dgViewList[COLUMN_INDEX_OF_INDEX, nRowIndex].Value.ToString(), out m_nIndexOfSelectedOfInput))
            {
                m_dgViewLookupInput.Rows.Clear();

                AddTableItemOnGrid(true, m_nIndexOfSelectedOfInput);
                ActiveControls(true, true);
            }
        }
        /// <summary>
        /// 2020.06.08 by twkang [ADD] Input 아이템들을 클릭했을 때 발생하는 이벤트 이다.
        /// </summary>
        private void DoubleClick_InputItem(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            int nColumnIndex = e.ColumnIndex;

            DataGridView dgViewList = null;
            dgViewList = GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.INPUT);

            if (nRowindex < 0 || nRowindex >= dgViewList.RowCount) { return; }

            int nIndexOfItem = int.Parse(dgViewList[0, nRowindex].Value.ToString());

            string strResult = string.Empty;
            bool bResult = false;
            int nResult = -1;

            switch (nColumnIndex)
            {
                case COLUMN_INDEX_OF_ENABLE:
                    if (m_InstanceOfSelectionList.CreateForm(SelectionList.ENABLE
                        , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ENABLE_DISABLE
                        , dgViewList.Rows[nRowindex].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor == c_clrTrue ? SelectionList.ENABLE : SelectionList.DISABLE
                        , false
                        , SelectionList.DISABLE))
                    {
                        m_InstanceOfSelectionList.GetResult(ref strResult);
                        bResult = strResult.Equals(SelectionList.ENABLE);
                        if (m_InstanceOfAnalogIO.SetParameter(true, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.ENABLE, bResult))
                        {
                            dgViewList.Rows[nRowindex].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
                            dgViewList.Rows[nRowindex].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;
                        }
                    }
                    break;
                case COLUMN_INDEX_OF_NAME:
                    if (m_InstanceOfKeyboard.CreateForm(dgViewList[COLUMN_INDEX_OF_NAME, nRowindex].Value.ToString()))
                    {
                        m_InstanceOfKeyboard.GetResult(ref strResult);
                        if (m_InstanceOfAnalogIO.SetParameter(true, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.NAME, strResult))
                        {
                            dgViewList[COLUMN_INDEX_OF_NAME, nRowindex].Value = strResult;
                        }
                    }
                    break;
                case COLUMN_INDEX_OF_TAGNUMER:
                    if (m_InstanceOfKeyboard.CreateForm(dgViewList[COLUMN_INDEX_OF_TAGNUMER, nRowindex].Value.ToString()))
                    {
                        m_InstanceOfKeyboard.GetResult(ref strResult);
                        if (m_InstanceOfAnalogIO.SetParameter(true, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.TAG_NO, strResult))
                        {
                            dgViewList[COLUMN_INDEX_OF_TAGNUMER, nRowindex].Value = strResult;
                        }
                    }
                    break;
                case COLUMN_INDEX_OF_TARGET:
                    if (m_InstanceOfCalculator.CreateForm(dgViewList[COLUMN_INDEX_OF_TARGET, nRowindex].Value.ToString()
                        , MIN_OF_TARGET
                        , m_nCountOfInputGroup.ToString()))
                    {
                        m_InstanceOfCalculator.GetResult(ref nResult);
                        if (m_InstanceOfAnalogIO.SetParameter(true, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.TARGET, nResult))
                        {
                            dgViewList[COLUMN_INDEX_OF_TARGET, nRowindex].Value = nResult.ToString();
                        }
                    }
                    break;
                case COLUMN_INDEX_OF_BIT:
                    if (m_InstanceOfCalculator.CreateForm(dgViewList[COLUMN_INDEX_OF_BIT, nRowindex].Value.ToString()
                        , MIN_OF_BIT
                        , MAX_OF_BIT))
                    {
                        m_InstanceOfCalculator.GetResult(ref nResult);
                        if (m_InstanceOfAnalogIO.SetParameter(true, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.BIT, nResult))
                        {
                            dgViewList[COLUMN_INDEX_OF_BIT, nRowindex].Value = nResult.ToString();
                        }
                    }
                    break;

                // 2022.11.04. by shkim. [ADD] Raw data type에 따른 계산과정 추가
                case COLUMN_INDEX_OF_DATA_TYPE:
                    AnalogIO_.RAW_DATA_TYPE enRawDataType = AnalogIO_.RAW_DATA_TYPE.SIGNED;
                    int nRawDataType = -1;
                    if (false == int.TryParse(dgViewList[COLUMN_INDEX_OF_DATA_TYPE, nRowindex].Value.ToString(), out nRawDataType))
                    {
                        enRawDataType = AnalogIO_.RAW_DATA_TYPE.SIGNED;
                    }
                    else
                    {
                        if (false == Enum.TryParse<AnalogIO_.RAW_DATA_TYPE>(nRawDataType.ToString(), out enRawDataType))
                        {
                            enRawDataType = AnalogIO_.RAW_DATA_TYPE.SIGNED;
                        }
                    }
                    if (m_InstanceOfSelectionList.CreateForm("ANALOG RAW DATA TYPE"
                                                            , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ANALOG_DATA_TYPE
                                                            , "PRE_VALUE"))
                    {
                        m_InstanceOfSelectionList.GetResult(ref nRawDataType);
                        if (m_InstanceOfAnalogIO.SetParameter(true, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.RAW_DATA_TYPE, nRawDataType))
                        {
                            dgViewList[COLUMN_INDEX_OF_DATA_TYPE, nRowindex].Value = nRawDataType.ToString();
                        }
                    }
                    break;
                // 2022.11.04. by shkim. [END]
                
                case COLUMN_INDEX_OF_MIN:
                    if (m_InstanceOfCalculator.CreateForm(dgViewList[COLUMN_INDEX_OF_MIN, nRowindex].Value.ToString()
                        , MIN_OF_VALUE
                        , MAX_OF_VALUE))
                    {
                        m_InstanceOfCalculator.GetResult(ref nResult);
                        if (m_InstanceOfAnalogIO.SetParameter(true, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.MIN, nResult))
                        {
                            dgViewList[COLUMN_INDEX_OF_MIN, nRowindex].Value = nResult.ToString();
                        }
                    }
                    break;
                case COLUMN_INDEX_OF_MAX:
                    if (m_InstanceOfCalculator.CreateForm(dgViewList[COLUMN_INDEX_OF_MAX, nRowindex].Value.ToString()
                        , MIN_OF_VALUE
                        , MAX_OF_VALUE))
                    {
                        m_InstanceOfCalculator.GetResult(ref nResult);
                        if (m_InstanceOfAnalogIO.SetParameter(true, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.MAX, nResult))
                        {
                            dgViewList[COLUMN_INDEX_OF_MAX, nRowindex].Value = nResult.ToString();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 2020.06.08 by twkang [ADD] Input Item Table 을 클릭했을 때 발생하는 이벤트
        /// </summary>
        private void DoubleClick_Input_Table(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            int nColumnIndex = e.ColumnIndex;

            if (nRowindex < 0 || nRowindex >= m_dgViewLookupInput.RowCount) { return; }

            int nIndex = int.Parse(m_dgViewLookupInput[COLUMN_INDEX_OF_INDEX, nRowindex].Value.ToString());

            double dbVolt = 0;
            double dbValue = 0;

            DataGridView dgViewList = null;
            dgViewList = GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.INPUT);

            switch (nColumnIndex)
            {
                case COLUMN_INDEX_OF_TABLE_VOLT:
                    if (m_InstanceOfCalculator.CreateForm(m_dgViewLookupInput[COLUMN_INDEX_OF_TABLE_VOLT, nRowindex].Value.ToString()
                        , dgViewList[COLUMN_INDEX_OF_MIN, m_nIndexOfSelectedOfInput].Value.ToString()
                        , dgViewList[COLUMN_INDEX_OF_MAX, m_nIndexOfSelectedOfInput].Value.ToString()))
                    {
                        m_InstanceOfCalculator.GetResult(ref dbVolt);
                        if (m_InstanceOfAnalogIO.SetDataTable(true, m_nIndexOfSelectedOfInput, nIndex, FrameOfSystem3.Config.ConfigAnalogIO.EN_TABLE_TYPE.VOLT, dbVolt))
                        {
                            m_dgViewLookupInput[COLUMN_INDEX_OF_TABLE_VOLT, nRowindex].Value = dbVolt.ToString();
                        }
                    }
                    break;
                case COLUMN_INDEX_OF_TABLE_VALUE:
                    if (m_InstanceOfCalculator.CreateForm(m_dgViewLookupInput[COLUMN_INDEX_OF_TABLE_VALUE, nRowindex].Value.ToString()
                        , MIN_OF_VALUE
                        , MAX_OF_VALUE))
                    {
                        m_InstanceOfCalculator.GetResult(ref dbValue);
                        if (m_InstanceOfAnalogIO.SetDataTable(true, m_nIndexOfSelectedOfInput, nIndex, FrameOfSystem3.Config.ConfigAnalogIO.EN_TABLE_TYPE.VALUE, dbValue))
                        {
                            m_dgViewLookupInput[COLUMN_INDEX_OF_TABLE_VALUE, nRowindex].Value = dbValue.ToString();
                        }
                    }
                    break;
            }
            m_dgViewLookupInput.Rows[nRowindex].Selected = false;
        }
        /// <summary>
        /// 2020.06.08 by twkang [ADD] Output Item 들을 클릭했을 때 발생하는 이벤트.
        /// </summary>
        private void DoubleClick_OutputItem(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            int nColumnIndex = e.ColumnIndex;

            DataGridView dgViewList = null;
            dgViewList = GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.OUTPUT);

            if (nRowindex < 0 || nRowindex >= dgViewList.RowCount) { return; }

            int nIndexOfItem = int.Parse(dgViewList[0, nRowindex].Value.ToString());

            string strResult = string.Empty;
            bool bResult = false;
            int nResult = -1;
            double dbResult = 0;

            switch (nColumnIndex)
            {
                case COLUMN_INDEX_OF_ENABLE:
                    if (m_InstanceOfSelectionList.CreateForm(SelectionList.ENABLE
                        , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ENABLE_DISABLE
                        , dgViewList.Rows[nRowindex].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor == c_clrTrue ? SelectionList.ENABLE : SelectionList.DISABLE
                        , false
                        , SelectionList.DISABLE))
                    {
                        m_InstanceOfSelectionList.GetResult(ref strResult);
                        bResult = strResult.Equals(SelectionList.ENABLE);
                        if (m_InstanceOfAnalogIO.SetParameter(false, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.ENABLE, strResult))
                        {
                            dgViewList.Rows[nRowindex].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor = bResult ? c_clrTrue : c_clrFalse;
                            dgViewList.Rows[nRowindex].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor = bResult ? c_clrTrue : c_clrFalse;
                        }
                    }
                    break;
                case COLUMN_INDEX_OF_NAME:
                    if (m_InstanceOfKeyboard.CreateForm(dgViewList[COLUMN_INDEX_OF_NAME, nRowindex].Value.ToString()))
                    {
                        m_InstanceOfKeyboard.GetResult(ref strResult);
                        if (m_InstanceOfAnalogIO.SetParameter(false, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.NAME, strResult))
                        {
                            dgViewList[COLUMN_INDEX_OF_NAME, nRowindex].Value = strResult;
                        }
                    }
                    break;
                case COLUMN_INDEX_OF_TAGNUMER:
                    if (m_InstanceOfKeyboard.CreateForm(dgViewList[COLUMN_INDEX_OF_TAGNUMER, nRowindex].Value.ToString()))
                    {
                        m_InstanceOfKeyboard.GetResult(ref strResult);
                        if (m_InstanceOfAnalogIO.SetParameter(false, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.TAG_NO, strResult))
                        {
                            dgViewList[COLUMN_INDEX_OF_TAGNUMER, nRowindex].Value = strResult;
                        }
                    }
                    break;
                case COLUMN_INDEX_OF_TARGET:
                    if (m_InstanceOfCalculator.CreateForm(dgViewList[COLUMN_INDEX_OF_TARGET, nRowindex].Value.ToString()
                        , MIN_OF_TARGET
                        , m_nCountOfInputGroup.ToString()))
                    {
                        m_InstanceOfCalculator.GetResult(ref nResult);
                        if (m_InstanceOfAnalogIO.SetParameter(false, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.TARGET, nResult))
                        {
                            dgViewList[COLUMN_INDEX_OF_TARGET, nRowindex].Value = nResult.ToString();
                        }
                    }
                    break;
                case COLUMN_INDEX_OF_BIT:
                    if (m_InstanceOfCalculator.CreateForm(dgViewList[COLUMN_INDEX_OF_BIT, nRowindex].Value.ToString()
                        , MIN_OF_BIT
                        , MAX_OF_BIT))
                    {
                        m_InstanceOfCalculator.GetResult(ref nResult);
                        if (m_InstanceOfAnalogIO.SetParameter(false, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.BIT, nResult))
                        {
                            dgViewList[COLUMN_INDEX_OF_BIT, nRowindex].Value = nResult.ToString();
                        }
                    }
                    break;

                // 2022.11.04. by shkim. [ADD] Raw data type에 따른 계산과정 추가
                case COLUMN_INDEX_OF_DATA_TYPE:
                    AnalogIO_.RAW_DATA_TYPE enRawDataType = AnalogIO_.RAW_DATA_TYPE.SIGNED;
                    int nRawDataType = -1;
                    if (false == int.TryParse(dgViewList[COLUMN_INDEX_OF_DATA_TYPE, nRowindex].Value.ToString(), out nRawDataType))
                    {
                        enRawDataType = AnalogIO_.RAW_DATA_TYPE.SIGNED;
                    }
                    else
                    {
                        if (false == Enum.TryParse<AnalogIO_.RAW_DATA_TYPE>(nRawDataType.ToString(), out enRawDataType))
                        {
                            enRawDataType = AnalogIO_.RAW_DATA_TYPE.SIGNED;
                        }
                    }
                    if (m_InstanceOfSelectionList.CreateForm("ANALOG RAW DATA TYPE"
                                                            , Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.ANALOG_DATA_TYPE
                                                            , "PRE_VALUE"))
                    {
                        m_InstanceOfSelectionList.GetResult(ref nRawDataType);
                        if (m_InstanceOfAnalogIO.SetParameter(false, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.RAW_DATA_TYPE, nRawDataType))
                        {
                            dgViewList[COLUMN_INDEX_OF_DATA_TYPE, nRowindex].Value = nRawDataType.ToString();
                        }
                    }
                    break;
                // 2022.11.04. by shkim. [END]

                case COLUMN_INDEX_OF_MIN:
                    if (m_InstanceOfCalculator.CreateForm(dgViewList[COLUMN_INDEX_OF_MIN, nRowindex].Value.ToString()
                        , MIN_OF_VALUE
                        , MAX_OF_VALUE))
                    {
                        m_InstanceOfCalculator.GetResult(ref nResult);
                        if (m_InstanceOfAnalogIO.SetParameter(false, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.MIN, nResult))
                        {
                            dgViewList[COLUMN_INDEX_OF_MIN, nRowindex].Value = nResult.ToString();
                        }
                    }
                    break;
                case COLUMN_INDEX_OF_MAX:
                    if (m_InstanceOfCalculator.CreateForm(dgViewList[COLUMN_INDEX_OF_MAX, nRowindex].Value.ToString()
                        , MIN_OF_VALUE
                        , MAX_OF_VALUE))
                    {
                        m_InstanceOfCalculator.GetResult(ref nResult);
                        if (m_InstanceOfAnalogIO.SetParameter(false, nIndexOfItem, FrameOfSystem3.Config.ConfigAnalogIO.EN_PARAM_ANALOGIO.MAX, nResult))
                        {
                            dgViewList[COLUMN_INDEX_OF_MAX, nRowindex].Value = nResult.ToString();
                        }
                    }
                    break;
                case COLUMN_INDEX_OF_VOLT:
                    if (m_InstanceOfCalculator.CreateForm(dgViewList[COLUMN_INDEX_OF_VOLT, nRowindex].Value.ToString()
                        , dgViewList[COLUMN_INDEX_OF_MIN, nRowindex].Value.ToString()
                        , dgViewList[COLUMN_INDEX_OF_MAX, nRowindex].Value.ToString()))
                    {
                        m_InstanceOfCalculator.GetResult(ref dbResult);
                        m_InstanceOfAnalogIO.WriteOutputVolt(nIndexOfItem, dbResult);
                    }
                    break;
                case COLUMN_INDEX_OF_VALUE:
                    if (m_InstanceOfCalculator.CreateForm(dgViewList[COLUMN_INDEX_OF_TABLE_VALUE, nRowindex].Value.ToString()
                        , MIN_OF_VALUE
                        , MAX_OF_VALUE))
                    {
                        m_InstanceOfCalculator.GetResult(ref dbResult);
                        m_InstanceOfAnalogIO.WriteOutputValue(nIndexOfItem, dbResult);
                    }
                    break;
            }
            dgViewList.Rows[nRowindex].Selected = false;
        }
        /// <summary>
        /// 2020.06.08 by twkang [ADD] Output Lookup Table 을 클릭했을 때 발생하는 이벤트
        /// </summary>
        private void DoubleClick_Output_Table(object sender, DataGridViewCellEventArgs e)
        {
            int nRowindex = e.RowIndex;
            int nColumnIndex = e.ColumnIndex;

            if (nRowindex < 0 || nRowindex >= m_dgViewLookupOutput.RowCount) { return; }

            int nIndex = int.Parse(m_dgViewLookupOutput[COLUMN_INDEX_OF_INDEX, nRowindex].Value.ToString());

            double dbVolt = 0;
            double dbValue = 0;

            DataGridView dgViewList = null;
            dgViewList = GetDataGridViewByInOut((int)EN_SELECTED_ANALOG_TYPE.OUTPUT);

            switch (nColumnIndex)
            {
                case COLUMN_INDEX_OF_TABLE_VOLT:
                    if (m_InstanceOfCalculator.CreateForm(m_dgViewLookupOutput[COLUMN_INDEX_OF_TABLE_VOLT, nRowindex].Value.ToString()
                        , dgViewList[COLUMN_INDEX_OF_MIN, m_nIndexOfSelectedOfOutput].Value.ToString()
                        , dgViewList[COLUMN_INDEX_OF_MAX, m_nIndexOfSelectedOfOutput].Value.ToString()))
                    {
                        m_InstanceOfCalculator.GetResult(ref dbVolt);
                        if (m_InstanceOfAnalogIO.SetDataTable(false, m_nIndexOfSelectedOfOutput, nIndex, FrameOfSystem3.Config.ConfigAnalogIO.EN_TABLE_TYPE.VOLT, dbVolt))
                        {
                            m_dgViewLookupOutput[COLUMN_INDEX_OF_TABLE_VOLT, nRowindex].Value = dbVolt.ToString();
                        }
                    }
                    break;
                case COLUMN_INDEX_OF_TABLE_VALUE:
                    if (m_InstanceOfCalculator.CreateForm(m_dgViewLookupOutput[COLUMN_INDEX_OF_TABLE_VALUE, nRowindex].Value.ToString()
                        , MIN_OF_VALUE
                        , MAX_OF_VALUE))
                    {
                        m_InstanceOfCalculator.GetResult(ref dbValue);
                        if (m_InstanceOfAnalogIO.SetDataTable(false, m_nIndexOfSelectedOfOutput, nIndex, FrameOfSystem3.Config.ConfigAnalogIO.EN_TABLE_TYPE.VALUE, dbValue))
                        {
                            m_dgViewLookupOutput[COLUMN_INDEX_OF_TABLE_VALUE, nRowindex].Value = dbValue.ToString();
                        }
                    }
                    break;
            }
            m_dgViewLookupOutput.Rows[nRowindex].Selected = false;
        }


        #endregion

        #region 클래스

        private class DeviceInfo
        {
            private string m_strName = string.Empty;
            private int m_nIndex = -1;
            private bool m_bOn = false;
            private bool m_bIsVoltType = true;

            private RollingPointPairList m_PointList;
            private EN_TYPE m_enType = new EN_TYPE();

            public string Name { get { return m_strName; } set { m_strName = value; } }
            public int Index { get { return m_nIndex; } set { m_nIndex = value; } }
            public bool On { get { return m_bOn; } set { m_bOn = value; } }
            public bool IsVoltType { get { return m_bIsVoltType; } set { m_bIsVoltType = value; } }
            public EN_TYPE Type { get { return m_enType; } set { m_enType = value; } }
            public RollingPointPairList PorintList { get { return m_PointList; } }

            public DeviceInfo(EN_TYPE entype, string strName, int nIndex, RollingPointPairList pointList,bool bIsVoltType, bool bOn = false)
            {
                m_enType = entype;
                m_strName = strName;
                m_bOn = bOn;
                m_nIndex = nIndex;
                m_PointList = pointList;
                m_bIsVoltType = bIsVoltType;
            }

            public void AddValue(double dbTime, double dbValue)
            {
                m_PointList.Add(dbTime, dbValue);
            }
        }

        #endregion
    }
}
