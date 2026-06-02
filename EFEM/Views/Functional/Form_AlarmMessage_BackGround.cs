using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameOfSystem3.Views.Functional
{
	public partial class Form_AlarmMessage_BackGround : Form
	{
		#region 싱글톤
		private static Form_AlarmMessage_BackGround _Instance	= new Form_AlarmMessage_BackGround();
		public static Form_AlarmMessage_BackGround GetInstance()
		{
			return _Instance;
		}
		#endregion

		private Form_AlarmMessage_BackGround()
		{
			InitializeComponent();
			this.DoubleBuffered = true;
		}

		#region 상수
		private const int INTERNAL_OF_TIMER					= 10;
		private const int START_POS_X_OF_PATTERN			= 128;

		private const string STATUS_NOTICE					= "NOTICE";
		private const string STATUS_ERROR					= "ERROR";
		private const string STATUS_WARNING					= "WARNING";
		
		private const int STATE_NOTICE						= 1;
		private const int STATE_WARNING						= 2;
		private const int STATE_ERROR						= 3;
		#endregion
		
		#region 변수
		private static Timer m_timerForUpdate				= null;
		private Point pt									= new Point();

		private int m_nCountOfMove							= 0;	
		private double m_dOpacity							= 0;

		private List<TestLabel> m_listTopLabelPattern		= new List<TestLabel>();
		private List<TestLabel> m_listBottomLabelpattern	= new List<TestLabel>();
		#endregion

		#region 내부인터페이스
		/// <summary>
		/// 2020.06.01 by twkang [ADD] 타이머 처리를 위한 함수로, UI를 갱신한다.
		/// </summary>
		private void FunctionForTimerTick(object sender, EventArgs e)
		{
			if (m_nCountOfMove > 128)
			{
				m_nCountOfMove = 0;
			}

			for(int nIndex = 0, nEnd = m_listTopLabelPattern.Count; nIndex < nEnd; nIndex++)
			{
				pt	 = m_listTopLabelPattern[nIndex].Location;
				pt.X = (START_POS_X_OF_PATTERN * nIndex) - m_nCountOfMove;
				m_listTopLabelPattern[nIndex].Location	= pt;
			}

			for (int nIndex = 0, nEnd = m_listBottomLabelpattern.Count; nIndex < nEnd; nIndex++)
			{
				pt = m_listBottomLabelpattern[nIndex].Location;
				pt.X = START_POS_X_OF_PATTERN * nIndex - m_nCountOfMove;
				m_listBottomLabelpattern[nIndex].Location = pt;
			}

			m_nCountOfMove	+= 2;

			if (this.Opacity == 1)
			{
				m_dOpacity = -0.01;
			}
			else if (this.Opacity < 0.3)
			{
				m_dOpacity = 0.015;
			}
			this.Opacity += m_dOpacity;

		}

		/// <summary>
		/// 2020.06.01 by twkang [ADD} 색상과 패턴을 설정한다.
		/// </summary>
		private void SetLayout(ref int nGrade)
		{
			Color c1		= new Color();

			switch(nGrade)
			{
				case STATE_NOTICE:
					c1 = Color.LimeGreen;
					m_lblAlarmStatus.Text	= STATUS_NOTICE;					
					break;
				case STATE_WARNING:
					c1 = Color.Orange;
					m_lblAlarmStatus.Text	= STATUS_WARNING;					
					break;
				case STATE_ERROR:
					c1 = Color.Red;
					m_lblAlarmStatus.Text	= STATUS_ERROR;
					break;
			}

			m_lblAlarmStatus.MainFontColor	= c1;

			m_LabelForPattern1.BackGroundColor	= c1;
			m_LabelForPattern2.BackGroundColor	= c1;
			m_LabelForPattern3.BackGroundColor	= c1;
			m_LabelForPattern4.BackGroundColor	= c1;

			for(int nIndex = 0, nEnd = m_listBottomLabelpattern.Count; nIndex < nEnd; nIndex++)
			{
				m_listTopLabelPattern[nIndex].Color_Left = c1;
				m_listBottomLabelpattern[nIndex].Color_Right = c1;
			}
		}
		#endregion
				
		#region 외부인터페이스
		/// <summary>
		/// 2020.06.01 by twkang [ADD] AlarmMessage BackGround 폼을 생성한다.
		/// </summary>
		public bool CreateForm(int nGrade)
		{
			m_nCountOfMove	= 0;
			m_dOpacity		= 0.015;

			#region Mapping Table
			m_listTopLabelPattern.Clear();
			m_listBottomLabelpattern.Clear();

			m_listTopLabelPattern.Add(m_AlarmPattern1);
			m_listTopLabelPattern.Add(m_AlarmPattern2);
			m_listTopLabelPattern.Add(m_AlarmPattern3);
			m_listTopLabelPattern.Add(m_AlarmPattern4);
			m_listTopLabelPattern.Add(m_AlarmPattern5);
			m_listTopLabelPattern.Add(m_AlarmPattern6);
			m_listTopLabelPattern.Add(m_AlarmPattern7);
			m_listTopLabelPattern.Add(m_AlarmPattern8);
			m_listTopLabelPattern.Add(m_AlarmPattern9);
			m_listTopLabelPattern.Add(m_AlarmPattern10);
			m_listBottomLabelpattern.Add(m_AlarmPattern11);
			m_listBottomLabelpattern.Add(m_AlarmPattern12);
			m_listBottomLabelpattern.Add(m_AlarmPattern13);
			m_listBottomLabelpattern.Add(m_AlarmPattern14);
			m_listBottomLabelpattern.Add(m_AlarmPattern15);
			m_listBottomLabelpattern.Add(m_AlarmPattern16);
			m_listBottomLabelpattern.Add(m_AlarmPattern17);
			m_listBottomLabelpattern.Add(m_AlarmPattern18);
			m_listBottomLabelpattern.Add(m_AlarmPattern19);
			m_listBottomLabelpattern.Add(m_AlarmPattern20);
			#endregion

			SetLayout(ref nGrade);
			
			if(m_timerForUpdate != null)
			{
				m_timerForUpdate.Stop();
			}

			m_timerForUpdate			= new Timer();
			m_timerForUpdate.Interval	= INTERNAL_OF_TIMER;
			m_timerForUpdate.Tick		+= new EventHandler(FunctionForTimerTick);
			m_timerForUpdate.Start();

			this.Show();

			return true;
		}
		/// <summary>
		/// 2020.06.01 by twkang [ADD] 폼을 종료시킨다.
		/// </summary>
		public void CloseBackGroundForm()
		{
			// 2024.02.14 by junho [ADD] Null exception 발생 하여 예외처리
            if (m_timerForUpdate != null)
			m_timerForUpdate.Stop();

			this.Hide();
		}
		#endregion
	}

	#region Control
	/// <summary>
	/// 2020.06.01 by twkang [ADD] 사선 모양의 Label Control 클래스 이다.
	/// </summary>
	public class TestLabel : Control
	{
		Point p1	= new Point();
		Point p2	= new Point();
		Point p3	= new Point();
		Point p4	= new Point();

		Point p5	= new Point();
		Point p6	= new Point();
		Point p7	= new Point();
		Point p8	= new Point();

		Color c1	= new Color();
		Color c2	= new Color();

		public TestLabel()
		{
			c1 = Color.Black;
			c2 = Color.Black;
		}

		[Category("Left Color")]
		public Color Color_Left
		{
			get { return c1; }
			set
			{
				c1 = value;
				this.Invalidate();
			}
		}
		[Category("Top Right")]
		public Color Color_Right
		{
			get { return c2; }
			set
			{
				c2 = value;
				this.Invalidate();
			}
		}
		/// <summary>
		/// 2020.06.01 by twkang [ADD] Control 이 그려질 때 호출되는 함수이다.
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			SolidBrush s1		= new SolidBrush(c1);
			SolidBrush s2		= new SolidBrush(c2);

			while (this.Width % 4 != 0)
			{
				++this.Width;
			}

			int nWidth	= this.Width / 4;

			p1.X = nWidth;
			p1.Y = 0;

			p2.X = nWidth << 1;
			p2.Y = 0;

			p3.X = nWidth;
			p3.Y = this.Height;

			p4.X = 0;
			p4.Y = this.Height;

			p5.X = p2.X + nWidth;
			p5.Y = 0;

			p6.X = this.Width;
			p6.Y = 0;

			p7.X = p5.X;
			p7.Y = this.Height;

			p8.X = p2.X;
			p8.Y = this.Height;

			PointF[] Point1	= { p1, p2, p3, p4 };
			PointF[] Point2	= { p5, p6, p7, p8 };
			e.Graphics.FillPolygon(s1, Point1);
			e.Graphics.FillPolygon(s2, Point2);

		}
	}
	#endregion
}
