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
	/// <summary>
	/// 2018.06.09 by yjlee [MOD] 숫자 입력창(계산기) 리팩토링
	/// 2019.04.10 by yjlee [MOD] 레시피 관련 기능 확장
	/// </summary>
	public partial class Form_Calculator : Form
	{
		#region 싱글톤
		private static Form_Calculator _Instance = new Form_Calculator();
		public static Form_Calculator GetInstance()
		{
			return _Instance;
		}
		private Form_Calculator()
		{
			this.DoubleBuffered = true;
			
			InitializeComponent();

			#region KeyMapping
			m_dicOfButton.Add((int)Keys.NumPad0, sys3button15);
			m_dicOfButton.Add((int)Keys.NumPad1, sys3button11);
			m_dicOfButton.Add((int)Keys.NumPad2, sys3button12);
			m_dicOfButton.Add((int)Keys.NumPad3, sys3button13);
			m_dicOfButton.Add((int)Keys.NumPad4, sys3button9);
			m_dicOfButton.Add((int)Keys.NumPad5, sys3button8);
			m_dicOfButton.Add((int)Keys.NumPad6, sys3button7);
			m_dicOfButton.Add((int)Keys.NumPad7, sys3button2);
			m_dicOfButton.Add((int)Keys.NumPad8, sys3button3);
			m_dicOfButton.Add((int)Keys.NumPad9, sys3button4);
			m_dicOfButton.Add((int)Keys.D0, sys3button15);
			m_dicOfButton.Add((int)Keys.D1, sys3button11);
			m_dicOfButton.Add((int)Keys.D2, sys3button12);
			m_dicOfButton.Add((int)Keys.D3, sys3button13);
			m_dicOfButton.Add((int)Keys.D4, sys3button9);
			m_dicOfButton.Add((int)Keys.D5, sys3button8);
			m_dicOfButton.Add((int)Keys.D6, sys3button7);
			m_dicOfButton.Add((int)Keys.D7, sys3button2);
			m_dicOfButton.Add((int)Keys.D8, sys3button3);
			m_dicOfButton.Add((int)Keys.D9, sys3button4);
			m_dicOfButton.Add((int)Keys.Back, sys3button1);
			m_dicOfButton.Add((int)Keys.Multiply, sys3button5);
			m_dicOfButton.Add((int)Keys.Divide, sys3button10);
			m_dicOfButton.Add((int)Keys.Subtract, sys3button6);
			m_dicOfButton.Add((int)Keys.Add, sys3button14);
			m_dicOfButton.Add((int)Keys.Enter, sys3button18);
			m_dicOfButton.Add((int)Keys.Decimal, sys3button17);
			m_dicOfButton.Add((int)Keys.OemPeriod, sys3button17);
			#endregion
		}
		#endregion

		#region 상수
		private const int CLEAR_TABINDEX						= 0;
		private const int BACKSPACE_TABINDEX					= 1;

		private const int UNIT_LENGTH_MAX						= 10;
		#endregion

		#region 변수
		private int m_nResult			= 0;    // 식의 결과를 저장한다.
		private double m_dblResult		= 0.0;  // 식의 결과를 저장한다.

		private string m_strRecentValue		= null; // 현재 값

		private bool _isSetMinValue = false;
		private double m_dblMinValue    = 0;        // 최소값을 저장

		private bool _isSetMaxValue = false;
		private double m_dblMaxValue    = 0;        // 최대값을 저장

		private bool _isSetUnitValue = false;
		private string m_strUnit = "";				// 단위를 저장

		private Form_MessageBox m_InstanceMessageBox    = Form_MessageBox.GetInstance();
		private Form_Keyboard   m_InstanceKeyboard      = Form_Keyboard.GetInstance();

		#region GUI
		private bool m_bMouseDownTitle					= false;
		private Point m_pMouseDownCoordinate			= new Point();

		private bool m_bShift							= false;
		Dictionary<int, Sys3Controls.Sys3button> m_dicOfButton	= new Dictionary<int,Sys3Controls.Sys3button>();
		#endregion

		#region 레시피 관련
		// 2022.04.20 by junho [DEL] set min, max, unit
		//private string m_strRecipeItem                  = string.Empty;
		private Sys3Controls.Sys3Label m_selectedLabel  = null;
		#endregion

		#endregion

		#region 열거형
		private enum EN_OPERATOR
		{
			ADD			= 0,
			SUBTRACT	= 1,
			MULTIPLY	= 2,
			DIVIDE		= 3,
		}
		private enum EN_FUNCTIONAL
		{
			DOT				= 0,
			SWITHING_SIGN	= 1,
		}
		#endregion

		#region 외부 인터페이스
		/// <summary>
		/// 2025.06.13 by junho [ADD] previous value 추가
		/// 2018.06.09 by yjlee [ADD] 넘버링 폼을 생성한다.
		/// </summary>
		public bool CreateForm(string strPresent, string strMin, string strMax, string strUnit = "", string strTitle = "", string strPrevious = "", string strPreviousTime = "")
		{
			double dblPresentValue  = 0;
			double dblPreviousValue = 0;
			double dblMinValue  = 0;
			double dblMaxValue  = 0;
			DateTime tPreviousTime = DateTime.Now;

			double.TryParse(strPresent, out dblPresentValue);
			double.TryParse(strMin, out dblMinValue);
			double.TryParse(strMax, out dblMaxValue);

			bool isUsePrevious = double.TryParse(strPrevious, out dblPreviousValue) && DateTime.TryParse(strPreviousTime, out tPreviousTime);

			InitForm(dblPresentValue
				, dblPreviousValue
				, tPreviousTime
				, dblMinValue
				, dblMaxValue
				, strUnit
				, isUsePrevious);

			m_strUnit = strUnit;

			// 2023.02.07 by junho [ADD] title 표시 할 수 있도록 기능 추가
			m_TitleBar.Text = strTitle;

			ResetButtonClicked();

			SetLabels();

			// 2022.04.20 by junho [DEL] set min, max, unit
			//m_strRecipeItem = string.Empty;

			this.ShowDialog();

			if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
			{
				SetResult();

				return true;
			}

			return false;
		}

		/// <summary>
		/// 2020.06.29 by twkang [ADD] 수식의 값을 int 형으로 반환한다.
		/// </summary>
		public void GetResult(ref int nValue)
		{
			nValue	= m_nResult;
		}
		/// <summary>
		/// 2020.06.29 by twkang [ADD] 수식의 값을 double 형으로 반환한다.
		/// </summary>
		public void GetResult(ref double dbValue)
		{
			dbValue	= m_dblResult;
		}
		/// <summary>
		/// 2021.09.16 by twkang [ADD] 수식의 값을 String 형으로 반환.
		/// </summary>
		public void GetResult(ref string strValue)
		{
			strValue	= m_dblResult.ToString();
		}
		public bool GetMin(ref int value)
		{
			value = (int)m_dblMinValue;
			return _isSetMinValue;
		}
		public bool GetMin(ref double value)
		{
			value = m_dblMinValue;
			return _isSetMinValue;
		}
		public bool GetMax(ref int value)
		{
			value = (int)m_dblMaxValue;
			return _isSetMaxValue;
		}
		public bool GetMax(ref double value)
		{
			value = m_dblMaxValue;
			return _isSetMaxValue;
		}
		public bool GetUnit(ref string value)
		{
			value = m_strUnit;
			return _isSetUnitValue;
		}
		#endregion

		#region 버튼 클릭 이벤트
		/// <summary>
		/// 2018.06.09 by yjlee [ADD] 'APPLY' 또는 'CANCEL' 버튼을 눌렀을 경우의 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click_ApplyOrCancel(object sender, EventArgs e)
		{
			Control ctr = sender as Control;

			switch (ctr.TabIndex)
			{
				case 0: // APPLY
					CalculateExpression();
					ApplyCalculatedValue();
					break;
				case 1: // CANCEL
					// 선택된 라벨이 존재하지 않을 경우
					if (null == m_selectedLabel)
					{
						this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
					}
					else
					{
						SetSelectedSubDataLabel(m_selectedLabel, false);
					}
					break;
			}
		}

		/// <summary>
		/// 2018.06.09 by yjlee [ADD] 숫자 버튼을 눌렀을 경우의 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click_Number(object sender, EventArgs e)
		{
			Control ctr = sender as Control;

			DoNumbering(ctr.TabIndex);
		}
		/// <summary>
		/// 2019.01.23 by yjlee [ADD] 숫자 버튼을 두 번 눌렀을 경우 처리
		/// </summary>
		private void DoubleClick_Number(object sender, EventArgs e)
		{
			Control ctr = sender as Control;

			DoNumbering(ctr.TabIndex);
		}
		/// <summary>
		/// 2018.06.09 by yjlee [ADD] 숫자 버튼을 눌렀을 경우의 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DoNumbering(int nIndex)
		{
			if (m_strRecentValue.Equals("0"))
			{
				m_strRecentValue = nIndex.ToString();
			}
			else if (m_strRecentValue.Length < 20)
			{
				m_strRecentValue = m_strRecentValue + nIndex.ToString();
			}

			m_labelDisplay.Text = m_strRecentValue;
		}

		/// <summary>
		/// 2018.06.09 by yjlee [ADD] Clear 또는 BackSpace를 눌렀을 경우의 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click_ClearOrBackSpace(object sender, EventArgs e)
		{
			Control ctr = sender as Control;

			ClearOrBackspace(ctr.TabIndex);
		}

		/// <summary>
		/// 2018.06.09 by yjlee [ADD] Dot 또는 부호 변환 버튼을 눌렀을 경우의 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click_Functional(object sender, EventArgs e)
		{
			Control ctr = sender as Control;

			switch(ctr.TabIndex)
			{
				case 0: // DOT
					DoFunctional(EN_FUNCTIONAL.DOT);
					break;
				case 1:	// + -
					DoFunctional(EN_FUNCTIONAL.SWITHING_SIGN);
					break;
			}
		}

		/// <summary>
		/// 2018.06.11 by yjlee [ADD] 4칙 연산자 버튼을 눌렀을 경우의 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click_Operator(object sender, EventArgs e)
		{
			Control ctr = sender as Control;

			switch(ctr.TabIndex)
			{
				case 0: // +
					DoOperator(EN_OPERATOR.ADD);
					break;
				case 1: // -
					DoOperator(EN_OPERATOR.SUBTRACT);
					break;
				case 2: // *
					DoOperator(EN_OPERATOR.MULTIPLY);
					break;
				case 3:// /
					DoOperator(EN_OPERATOR.DIVIDE);
					break;
			}
		}

		/// <summary>
		/// 2018.06.11 by yjlee [ADD] 계산기에서 = 버튼을 눌렀을 경우의 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click_Calculation(object sender, EventArgs e)
		{
			CalculateExpression();
		}

		/// <summary>
		/// 2018.06.011 by yjlee [ADD] 키보드로 키를 눌렀을 경우 발생한다.
		/// 키보드로도 동일하게 사용가능하게 만들기 위한 이벤트
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form_EditNumber_KeyDown(object sender, KeyEventArgs e)
		{
			int nKeyCode    = (int)e.KeyCode;
			
			if(m_dicOfButton.ContainsKey(nKeyCode))
			{
				m_dicOfButton[nKeyCode].ButtonClicked	= true;
			}

			// 키패드 숫자 입력 시
			if (nKeyCode >= 96 && nKeyCode <= 105)
			{
				DoNumbering(nKeyCode - 96);
			}
			// 가로 키패드 숫자 입력 시
			else if (nKeyCode >= 48 && nKeyCode <= 57)
			{
				if(m_bShift && e.KeyCode == Keys.D8)
					DoOperator(EN_OPERATOR.MULTIPLY);
				else
					DoNumbering(nKeyCode - 48);
			}
			else
			{
				switch(e.KeyCode)
				{
					case Keys.OemQuestion:
						if(false == m_bShift)
							DoOperator(EN_OPERATOR.DIVIDE);
						break;
					case Keys.OemMinus: // -키 입력
						if(false == m_bShift)
							DoOperator(EN_OPERATOR.SUBTRACT);
						break;

					case Keys.Oemplus: // +키 또는 =키 입력
						if(m_bShift)
							DoOperator(EN_OPERATOR.ADD);
						else
							CalculateExpression();
						break;
					case Keys.ShiftKey:
						m_bShift = true;
						break;
					case Keys.Back: // 백스페이스 입력 시
						ClearOrBackspace(BACKSPACE_TABINDEX);
						break;
					case Keys.Delete: // Delete 입력 시
						ClearOrBackspace(CLEAR_TABINDEX);
						break;
					case Keys.Escape: // Esc 입력 시
						this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
						break;
					case Keys.Enter: // 엔터 입력 시
						// 계산할 값이 없을 경우 적용시킨다.
						if (string.IsNullOrEmpty(m_labelDisplay.SubText))
						{
							ApplyCalculatedValue();
						}
						else
						{
							CalculateExpression();
						}
						break;

						// 연산자 입력 시
					case Keys.Multiply: // *
						DoOperator(EN_OPERATOR.MULTIPLY);
						break;

					case Keys.Divide:
						DoOperator(EN_OPERATOR.DIVIDE);
						break;

					case Keys.Add: // +
						DoOperator(EN_OPERATOR.ADD);
						break;

					case Keys.Decimal:
					case Keys.OemPeriod:
						DoFunctional(EN_FUNCTIONAL.DOT);
						break;

					case Keys.Subtract: // -
						DoOperator(EN_OPERATOR.SUBTRACT);
						break;
					default:
						break;
				}
			}
		}
		/// <summary>
		/// 2019.04.11 by yjlee [ADD] MIN, MAX 라벨 클릭 시
		/// </summary>
		private void Click_MinMaxLabel(object sender, EventArgs e)
		{
			// 2022.04.20 by junho [DEL] set min, max, unit
			//if (string.IsNullOrEmpty(m_strRecipeItem)) { return; }

			// 이미 선택된 라벨이 존재할 경우
			if (null != m_selectedLabel) { return; }

			Sys3Controls.Sys3Label cLabel       = sender as Sys3Controls.Sys3Label;

			SetSelectedSubDataLabel(cLabel);
		}
		/// <summary>
		/// 2019.04.11 by yjlee [ADD] UNIT 라벨 클릭 시
		/// </summary>
		private void Click_UnitLabel(object sender, EventArgs e)
		{
			// 2022.04.20 by junho [DEL] set min, max, unit
			//if (string.IsNullOrEmpty(m_strRecipeItem)) { return; }

			Form_Keyboard instanceKeyBoard  = Form_Keyboard.GetInstance();

			if (instanceKeyBoard.CreateForm(m_labelUnit.Text, UNIT_LENGTH_MAX))
			{
				string strResult	= string.Empty;
				
				instanceKeyBoard.GetResult(ref strResult);

				m_labelUnit.Text	= strResult;

				m_strUnit = strResult;
				_isSetUnitValue = true;
				SetUnit(strResult);
			}
		}
		/// <summary>
		/// 2025.06.13 by junho [MOD] Previous 기능 추가로 해당 함수는 이전 값에 대한 내용으로 변경. 기존 코드는 Present로 이동
		/// 2021.05.26 by twkang [ADD] Previous 값 입력
		/// </summary>
		private void Click_Previous(object sender, EventArgs e)
		{
			string strPrevious	= m_labelPrevious.Text;

			m_strRecentValue	= strPrevious;

			m_labelDisplay.Text		= m_strRecentValue;
			m_labelDisplay.SubText	= string.Empty;
		}
		private void Click_Present(object sender, EventArgs e)
		{
			string strPresent = m_labelPresent.Text;

			m_strRecentValue = strPresent;

			m_labelDisplay.Text = m_strRecentValue;
			m_labelDisplay.SubText = string.Empty;
		}
		/// <summary>
		/// 2021.07.28 by twkang [ADD] TitleBar Mouse Down 이벤트
		/// </summary>
		private void MouseDown_Title(object sender, MouseEventArgs e)
		{
			m_bMouseDownTitle = true;
			m_pMouseDownCoordinate = e.Location;
		}
		/// <summary>
		/// 2021.07.28 by twkang [ADD] TitleBar Mouse Move 이벤트
		/// </summary>
		private void MouseMove_Title(object sender, MouseEventArgs e)
		{
			if (m_bMouseDownTitle)
			{
				this.SetDesktopLocation(MousePosition.X - m_pMouseDownCoordinate.X, MousePosition.Y - m_pMouseDownCoordinate.Y);
			}
		}
		/// <summary>
		/// 2021.07.28 by twkang [ADD] TitleBar Mouse Up 이벤트
		/// </summary>
		private void MouseUp_Title(object sender, MouseEventArgs e)
		{
			m_bMouseDownTitle = false;
		}
		#endregion

		#region 내부 인터페이스

		#region 초기화
		/// <summary>
		/// 2019.04.11 by yjlee [ADD] 라벨 초기 설정을 진행한다.
		/// </summary>
		private void SetLabels()
		{
			// 라벨 언어 설정
			m_labelPresent.SubText	= "PRESENT";
			m_labelPrevious.SubText = "PREVIOUS";
			m_labelMin.SubText		= "MIN";
			m_labelMax.SubText		= "MAX";
			m_labelUnit.SubText		= "UNIT";

			// 버튼 언어 설정
			m_btnClear.Text			= "CLEAR";
			m_btnApply.Text			= "APPLY";
			m_btnCancel.Text		= "CANCEL";
		}
		/// <summary>
		/// 2025.06.13 by junho [ADD] Previous value 추가
		/// private void InitForm(double dblOldValue, double dblMinValue, double dblMaxValue, string strUnit)
		/// 2019.04.11 by yjlee [ADD] 초기 값을 설정한다.
		/// </summary>
		private void InitForm(double present, double previous, DateTime previousTime, double min, double max, string unit, bool usePrevious)
		{
			// 데이터들을 초기화한다.
			m_labelUnit.Text	= unit;

			m_selectedLabel		= null;

			m_dblMinValue		= min;
			m_dblMaxValue		= max;
			
			m_labelPresent.Text		= present.ToString();

			if(usePrevious)
			{
				m_labelPrevious.Visible = true;
				m_labelPrevious.Text = previous.ToString();

				lbl_PreviousTime.Visible = true;
				lbl_PreviousTime.Text = previousTime.ToString("yyyy-MM-dd HH:mm:ss");
			}
			else
			{
				m_labelPrevious.Visible = false;
				lbl_PreviousTime.Visible = false;
			}

			m_labelMin.Text		= min.ToString();
			m_labelMax.Text		= max.ToString();

			SetUnit(unit);

			// 현재 값을 초기화한다.
			m_strRecentValue	= "0";
			m_labelDisplay.Text		= m_strRecentValue;
			m_labelDisplay.SubText	= string.Empty;
		}

		/// <summary>
		/// 2019.04.11 by yjlee [ADD] 단위를 설정한다.
		/// </summary>
		/// <param name="strUnit"></param>
		private void SetUnit(string strUnit)
		{
			m_labelPresent.UnitText = strUnit;
			m_labelPrevious.UnitText = strUnit;
			m_labelMin.UnitText = strUnit;
			m_labelMax.UnitText = strUnit;
		}
		/// <summary>
		/// 2020.10.22 by twkang [ADD] 버튼 클릭 상태를 초기화한다.
		/// </summary>
		private void ResetButtonClicked()
		{
			foreach(var kvp in m_dicOfButton)
			{
				kvp.Value.ButtonClicked		= false;
			}
		}
		#endregion

		#region 유효한 값 확인
		/// <summary>
		/// 2018.06.12 by yjlee [ADD] 입력한 값이 지정된 범위 내인지 판단한다.
		/// </summary>
		/// <returns></returns>
		private bool CheckValueIsCorrect()
		{
			double dblResult = 0.0;

			return double.TryParse(m_labelDisplay.Text, out dblResult);
		}
		#endregion

		#region 표기 변환 및 계산
		/// <summary>
		/// 2018.06.11 by yjlee [ADD] 중위식을 후위식으로 변경한다.
		/// </summary>
		/// <param name="strExp"></param>
		/// <returns></returns>
		private string ConvertToPostFix(string strExp)
		{
			string[] strTokens = strExp.Split(' '); // 중위식을 공백으로 나누어서 저장
			string[] strOps = new string[] { "+", "-", "*", "/" }; // 사용할 연산자 등록

			// 연산자 우선순위를 지정
			Dictionary<string, int> dicPrecs = new Dictionary<string, int>
            {
                {"+", 1},
                {"-", 1},
                {"*", 2},
                {"/", 2}
            };

			Stack<string> stackOp = new Stack<string>();  // 연산자를 저장할 스택
			List<string> listOutput = new List<string>(); // 후위식
			Stack<double> stackValue = new Stack<double>();  // 연산 결과를 저장

			foreach (string token in strTokens)
			{
				if (strOps.Contains(token))
				{
					while (stackOp.Count > 0)
					{
						// 현재 연산자보다, 스택의 연산자의 우선순위가 더 낮을 경우
						if (dicPrecs[stackOp.Peek()] < dicPrecs[token])
						{
							break;
						}

						// 스택에서 두 값을 가져온다.
						double dblResult = CalcTwoValue(stackOp.Pop(), stackValue.Pop(), stackValue.Pop());

						// 계산한 식을 다시 스택에 넣는다.
						stackValue.Push(dblResult);
					}

					// 현재 연산자를 스택에 추가한다.
					stackOp.Push(token);
				}
				// 피연산자일 경우
				else
				{
					// 피연산자를 저장한다.
					stackValue.Push(double.Parse(token));
				}
			}

			// 더 이상 읽을 것이 없을 경우, 모든 연산자를 스택에서 꺼내온다.
			while (stackOp.Count > 0)
			{
				// 스택에서 두 값을 가져온다.
				double dblResult = CalcTwoValue(stackOp.Pop(), stackValue.Pop(), stackValue.Pop());

				// 계산한 식을 다시 스택에 넣는다.
				stackValue.Push(dblResult);
			}

			// 스택에 마지막으로 남은 값(결과 값)을 반환한다.
			return stackValue.Pop().ToString();
		}
		/// <summary>
		/// 2018.06.11 by yjlee [ADD] 토큰(연산자)에 따른 수식을 계산한다.
		/// </summary>
		/// <param name="strOp">연산자</param>
		/// <param name="dblLhs">좌측 값</param>
		/// <param name="dblRhs">우측 값</param>
		/// <returns></returns>
		private double CalcTwoValue(string strOp, double dblRhs, double dblLhs)
		{
			// 스택에서 두 값을 가져온다.
			double dblResult = 0.0;

			// 연산자에 따른 식을 계산한다.
			switch (strOp)
			{
				case "+":
					dblResult = dblLhs + dblRhs;
					break;
				case "-":
					dblResult = dblLhs - dblRhs;
					break;
				case "*":
					dblResult = dblLhs * dblRhs;
					break;
				case "/":
					dblResult = dblLhs / dblRhs;
					break;
			}

			return dblResult;
		}
		#endregion

		#region 계산기 입력 처리
		/// <summary>
		/// 2019.04.11 by yjlee [ADD] 연산 결과를 설정한다.
		/// </summary>
		private void SetResult()
		{
			int nResult         = 0;
			double dblResult    = 0.0;

			// 각 형에 맞는 데이터로 파싱한다.
			if (int.TryParse(m_labelDisplay.Text, out nResult))
			{
				m_nResult	= nResult;
				m_dblResult = (double)m_nResult;
			}
			else if (double.TryParse(m_labelDisplay.Text, out dblResult))
			{
				m_dblResult = dblResult;
				m_nResult	= (int)m_dblResult;
			}
		}
		/// <summary>
		/// 2018.06.09 by yjlee [ADD] Clear 또는 BackSpace를 눌렀을 경우의 처리
		/// </summary>
		/// <param name="nIndex"></param>
		private void ClearOrBackspace(int nIndex)
		{
			switch (nIndex)
			{
				case CLEAR_TABINDEX: // CLEAR
					m_strRecentValue	= "0";
					m_labelDisplay.Text = "";
					break;
				case BACKSPACE_TABINDEX: // BACKSPACE
					int nLength = m_strRecentValue.Length;

					// 숫자가 하나일 경우
					if (nLength < 2)
					{
						m_strRecentValue = "0";
					}
					else
					{
						// 맨 마지막 글자 삭제
						m_strRecentValue = m_strRecentValue.Remove(nLength - 1);
					}
					break;
			}
			// 화면에 값을 갱신한다.
			m_labelDisplay.Text = m_strRecentValue;
		}

		/// <summary>
		/// 2018.06.09 by yjlee [ADD] Dot 또는 부호 변환 버튼을 눌렀을 경우의 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DoFunctional(EN_FUNCTIONAL enFunctional)
		{
			switch (enFunctional)
			{
				case EN_FUNCTIONAL.DOT: // DOT
					if (!m_strRecentValue.Contains("."))
					{
						m_strRecentValue = m_strRecentValue + ".";
					}
					break;
				case EN_FUNCTIONAL.SWITHING_SIGN: // +,-
					// - 부호일 경우, 부호를 제거한다.
					if (m_strRecentValue.Contains("-"))
					{
						m_strRecentValue = m_strRecentValue.Remove(0, 1);
					}
					else // 부호가 없을 경우 - 부호를 추가한다.
					{
						m_strRecentValue = m_strRecentValue.Insert(0, "-");
					}
					break;
			}
			// 화면에 값을 갱신한다.
			m_labelDisplay.Text = m_strRecentValue;
		}

		/// <summary>
		/// 2018.06.11 by yjlee [ADD] 4칙 연산자 버튼을 눌렀을 경우의 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DoOperator(EN_OPERATOR enOperator)
		{
			string strOperator		= "";

			switch (enOperator)
			{
				case EN_OPERATOR.ADD: // +
					strOperator = " + ";
					break;
				case EN_OPERATOR.SUBTRACT: // -
					strOperator = " - ";
					break;
				case EN_OPERATOR.MULTIPLY: // *
					strOperator = " * ";
					break;
				case EN_OPERATOR.DIVIDE: // /
					strOperator = " / ";
					break;
			}

			m_labelDisplay.SubText += m_strRecentValue + strOperator;

			m_strRecentValue	= "0";
			m_labelDisplay.Text = "0";
		}

		/// <summary>
		/// 2019.04.20 by yjlee [ADD] 계산된 값을 적용한다.
		/// </summary>
		private void ApplyCalculatedValue()
		{
			// 선택된 라벨이 존재하지 않을 경우
			if (null == m_selectedLabel)
			{
				if (CheckValueIsCorrect() == false)
				{
					return;
				}

				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
			else
			{
				SetMinMaxLabel();

				SetSelectedSubDataLabel(m_selectedLabel, false);
			}
		}

		/// <summary>
		/// 2018.06.11 by yjlee [ADD] 사용자가 입력한 수식을 계산한다.
		/// </summary>
		private void CalculateExpression()
		{
			// 총 수식을 구한다.
			string strValue		= m_labelDisplay.SubText + m_labelDisplay.Text;

			// 중위식을 후위식으로 변환하여 계산한다.
			string strResult	= ConvertToPostFix(strValue);

			m_labelDisplay.SubText = "";
			m_labelDisplay.Text = strResult;
			m_strRecentValue	= strResult;
		}

		#endregion

		#region 부가 정보 관련
		/// <summary>
		/// 2019.04.11 by yjlee [ADD] 라벨이 선택 되었다.
		/// </summary>
		private void SetSelectedSubDataLabel(Sys3Controls.Sys3Label cLabel, bool bSelected = true)
		{
			if (bSelected)
			{
				cLabel.BackGroundColor	= Color.White;
				cLabel.MainFontColor	= Color.Black;

				m_selectedLabel			= cLabel;

				m_labelDisplay.SubText	= string.Empty;
				m_labelDisplay.Text		= "0";
				m_strRecentValue		= "0";
			}
			else
			{
				cLabel.BackGroundColor	= Color.Silver;
				cLabel.MainFontColor	= Color.Black;

				m_selectedLabel			= null;

				m_labelDisplay.SubText	= string.Empty;

				// 2025.06.13 by junho [MOD] 초기값 "0"으로 설정되는데 이때에만 present 값으로 설정되어 통일성을 위해 변경
				//m_labelDisplay.Text		= m_labelPresent.Text;
				//m_strRecentValue		= m_labelPresent.Text;
				m_labelDisplay.Text = "0";
				m_strRecentValue = "0";
			}
		}
		/// <summary>
		/// 2019.04.11 by yjlee [ADD] MIN, MAX 라벨 값을 설정한다.
		/// </summary>
		private bool SetMinMaxLabel()
		{
			// 선택된 라벨이 존재하지 않는다.
			if (null == m_selectedLabel) { return false; }

			double dblRecentValue   = double.Parse(m_labelDisplay.Text);

			if (m_selectedLabel == m_labelMin)
			{
				m_dblMinValue = dblRecentValue;
				_isSetMinValue = true;
			}
			else if (m_selectedLabel == m_labelMax)
			{
				m_dblMaxValue = dblRecentValue;
				_isSetMaxValue = true;
			}

			m_selectedLabel.Text = m_labelDisplay.Text;
			return true;
		}
		#endregion

		private void Form_EditNumber_KeyUp(object sender, KeyEventArgs e)
		{
			int nKeyCode	= (int)e.KeyCode;

			if(e.KeyCode == Keys.ShiftKey)
				m_bShift	= false;

			if(m_dicOfButton.ContainsKey(nKeyCode))
			{
				m_dicOfButton[nKeyCode].ButtonClicked	= false;
			}
		}
		#endregion
	}
}
