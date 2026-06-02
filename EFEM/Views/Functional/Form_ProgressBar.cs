using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Diagnostics;

using Sys3Controls;

namespace FrameOfSystem3.Views.Functional
{
    public partial class Form_ProgressBar : Form
    {
		private static Form_ProgressBar _Instance	= new Form_ProgressBar();
		public static Form_ProgressBar GetInstance()
		{
			return _Instance;
		}

        private Form_ProgressBar()
        {
			this.DoubleBuffered		= true;

			InitializeComponent();

			m_pTimer.Tick		+= new EventHandler(FunctionForTimerTick);
			m_pTimer.Interval	= TIMER_INTERVAL;
		}

		#region 상수
		private const int TIMER_INTERVAL	= 100;	

		#region 위치관련
		private	const int c_nFirstLabelPositionX				= 10;
		private const int c_nFirstLabelPositionY				= 60;

		private const int c_nLabelWidth							= 112;
		private const int c_nLabelHeight						= 50;

		private const int c_nFirstBarPositionX					= 130;
		private const int c_nFirstBarPositionY					= 60;

		private const int c_nBarWidth							= 563;
		private const int c_nBarHeight							= 50;

		private const int c_nPaddingY							= 10;
		private const int c_nOffsetForWindowMode				= 43;
		#endregion

		#region 컬러
		private readonly Color c_clrLabelBackGround	= Color.FromArgb(200, 200, 220);
		private readonly Color c_clrLabelFontColor	= Color.Black;

		private readonly Color c_clrBarFirst		= Color.White;
		private readonly Color c_clrBarSecond		= Color.DodgerBlue;
		#endregion

		#endregion

		#region 변수
		private int m_nCountOfItem								= 0;
		private bool m_bShown									= false;

		#region Data
		// Key : Progress Item 이름, Value : 해당 컨트롤
		private Dictionary<string, KeyValuePair<Sys3Label, Sys3ProgressBar>> m_dicForControl
				= new Dictionary<string, KeyValuePair<Sys3Label, Sys3ProgressBar>>();

		// Key : Progress Item 이름, Value : 해당 아이템의 현재스탭번호    // 스탭번호를 업데이트 해주기 위함
		private Dictionary<string, uint> m_dicForUpdate				= new Dictionary<string,uint>();

		// Control 을 생성해주기위한 변수	
		private Queue<KeyValuePair<string, uint>> m_queueForCreate	= new Queue<KeyValuePair<string,uint>>();
		#endregion

		private Timer m_pTimer		= new Timer();
		private System.Threading.ReaderWriterLockSlim m_RWLock	= new System.Threading.ReaderWriterLockSlim();

		public delegate void CallBackFuntion(bool bShow);
		public event CallBackFuntion evtProgressBar;
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.09.29 by twkang [ADD] 타이머에 의해 호출되는 함수
		/// </summary>
		private void FunctionForTimerTick(object sender, EventArgs e)
		{
			m_RWLock.EnterReadLock();
			int nCountOfQueue		= m_queueForCreate.Count;
			m_RWLock.ExitReadLock();

			for(int nIndex = 0; nIndex < nCountOfQueue; ++nIndex)
			{
				m_RWLock.EnterWriteLock();
				var kvp = m_queueForCreate.Dequeue();
				m_RWLock.ExitWriteLock();

				CreateControls(kvp.Key, kvp.Value);

				SetControlLocation(m_nCountOfItem - 1);
			}

			UpdateStep();

			bool bIsFinish	= true;

			foreach(var kvp in m_dicForControl.Values)
			{
				bIsFinish &= (kvp.Value.MaxTickCount == kvp.Value.Tick);
			}

			if(bIsFinish)
			{
				DisposeControl();
			}
		}
		/// <summary>
		/// 2020.09.29 by twkang [ADD] 컨트롤들을 생성한다.
		/// </summary>
		private void CreateControls(string strItemName, uint nMaxCountStep)
		{
			if(false == m_dicForControl.ContainsKey(strItemName))
			{
				Sys3Label label				= new Sys3Label();
				this.Controls.Add(label);

				label.Text					= strItemName;
				label.MainFont				= new System.Drawing.Font("맑은 고딕", 12, FontStyle.Bold);
				label.MainFontColor			= c_clrLabelFontColor;
				label.Height				= c_nLabelHeight;
				label.Width					= c_nLabelWidth;
				label.BackGroundColor		= c_clrLabelBackGround;
				
				Sys3ProgressBar ProgressBar	= new Sys3ProgressBar();
				this.Controls.Add(ProgressBar);

				ProgressBar.Tick			= 1;
				ProgressBar.FirstColor		= c_clrBarFirst;
				ProgressBar.SecondColor		= c_clrBarSecond;
				ProgressBar.MaxTickCount	= nMaxCountStep;
				ProgressBar.MinTickCount	= 0;
				ProgressBar.Width			= c_nBarWidth;
				ProgressBar.Height			= c_nBarHeight;
				ProgressBar.Enabled			= false;

				m_dicForControl.Add(strItemName, new KeyValuePair<Sys3Label,Sys3ProgressBar>(label, ProgressBar));
				m_nCountOfItem	= m_dicForControl.Count;

				this.Invalidate();
			}
		}
		/// <summary>
		/// 2020.09.29 by twkang [ADD] Label, ProgressBar 의 위치를 설정한다.
		/// </summary>
		private void SetControlLocation(int nIndex)
		{
			Point pt	= new Point();
			var kvp		= m_dicForControl.ElementAt(nIndex).Value;

			int nPadding			= c_nLabelHeight + c_nPaddingY;
			pt.X					= c_nFirstLabelPositionX;
			pt.Y					= c_nFirstLabelPositionY + (nPadding * nIndex);
			kvp.Key.Location		= pt;
			kvp.Key.BringToFront();

			pt.X					= c_nFirstBarPositionX;
			pt.Y					= c_nFirstBarPositionY + (nPadding * nIndex);
			kvp.Value.Location		= pt;
			kvp.Value.BringToFront();

			this.Height				= pt.Y + nPadding + c_nOffsetForWindowMode;
			m_groupTitle.Height		= this.Height;
			m_groupTitle.Invalidate();
			this.CenterToScreen();
			this.Invalidate();
		}
		/// <summary>
		/// 2021.06.28 by twkang [ADD] Step Update
		/// </summary>
		private void UpdateStep()
		{
			m_RWLock.EnterWriteLock();

			foreach(var kvp in m_dicForUpdate)
			{
				string strItem	= kvp.Key;
				uint uStep		= kvp.Value;

				if(false == m_dicForControl.ContainsKey(strItem))
					continue;

				Sys3ProgressBar pUpdatedProgressBar	= m_dicForControl[strItem].Value;

				pUpdatedProgressBar.Tick = uStep;

				if (pUpdatedProgressBar.MaxTickCount == uStep)
				{
					pUpdatedProgressBar.FirstColor = c_clrBarSecond;
				}
			}

			m_RWLock.ExitWriteLock();
			
			this.Invalidate();
		}
		/// <summary>
		/// 2020.09.29 by twkang [ADD] Label, ProgressBar 의 리소스를 해제한다.
		/// </summary>
		private void DisposeControl()
		{
			evtProgressBar(false);
		}
		#endregion

		#region 외부인터페이스
		/// <summary>
		/// 2020.09.29 by twkang [ADD] Progress 아이템을 추가한다.
		/// </summary>
		public void ShowForm(string strItemName, uint nMaxCountStep)
		{
			// 2022.02.17 by junho [ADD] Don't show at debugger
			if (System.Diagnostics.Debugger.IsAttached)
				return;

			m_RWLock.EnterWriteLock();

			m_queueForCreate.Enqueue(new KeyValuePair<string, uint>(strItemName, nMaxCountStep));

			m_RWLock.ExitWriteLock();

			evtProgressBar(true);
		}
        /// <summary>
        /// 2020.09.29 by twkang [ADD] 메인 폼에 의해 호출되는 함수, 폼을 생성한다.
        /// </summary>
        public void ShowByMainForm()
		{
			if(m_bShown)
				return;

			this.Show();

			m_bShown = true;

			m_pTimer.Start();
		}
		/// <summary>
		/// 2020.06.15 by twkang [ADD] 메인 폼에 의해 호출되는 함수, 폼을 닫는다.
		/// </summary>
		public void HideByMainForm()
		{
			if(false == m_bShown)
				return;

			m_pTimer.Stop();

			m_RWLock.EnterWriteLock();

			foreach (var kvp in m_dicForControl.Values)
			{
				kvp.Key.Dispose();
				kvp.Value.Dispose();
			}

			m_dicForControl.Clear();
			m_queueForCreate.Clear();
			m_dicForUpdate.Clear();

			m_nCountOfItem		= 0;
			
			m_RWLock.ExitWriteLock();

			this.Hide();
			m_bShown = false;
		}
		/// <summary>
		/// 2020.09.29 by twkang [ADD] 특정 아이템의 TIck 수를 바꾼다.
		/// </summary>
		public void UpdateStep(string strItemName, uint uStep)
		{
			m_RWLock.EnterWriteLock();

			if(false == m_dicForUpdate.ContainsKey(strItemName))
				m_dicForUpdate.Add(strItemName, uStep);
			else
				m_dicForUpdate[strItemName]	= uStep;

			m_RWLock.ExitWriteLock();
		}
		#endregion

		#region UI 이벤트
		/// <summary>
		/// 2021.06.15 by twkang [ADD] Close 버튼 클릭 이벤트
		/// </summary>
		private void Click_Close(object sender, EventArgs e)
		{
			DisposeControl();
		}
		#endregion
	}
}