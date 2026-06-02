using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FrameOfSystem3.Views.Functional
{
	// 2025.05.22. by shkim [MOD] 기능개선 : Selection Item의 Text의 맞게 폼의 크기 변경되도록 개선 (아이템 글자 잘림 방지)
    public partial class Form_SelectionList : Form
    {
		#region constructor
		private Form_SelectionList()
		{
			InitializeComponent();

			DEFAULT_FORM_HEIGHT = this.Height;
			DEFUALT_FORM_WIDTH = this.Width;
            DEFAULT_GROUPBOX_WIDTH = _gbSelectionItems.Width;
			GROUP_BOX_LABEL_HEADER_HEIGHT = _gbSelectionItems.LabelHeight;

            /*	기본 버튼의 크기 (DEFAULT_BUTTON_WIDTH
			 *	= DEFAULT_GROUPBOX_WIDTH - (2 * Padding + LED 너비) + 1
			 */
            DEFAULT_BUTTON_WIDTH = DEFAULT_GROUPBOX_WIDTH - (2 * c_Padding + c_LedWidth) + 1;
			_calculatedButtonWidth = DEFAULT_BUTTON_WIDTH;

            /*	필요한 그룹박스 너비 (Selected or Selection 한 쪽 기준)
             *	= 2 * PADDING + LED 너비 + BUTTON 너비 - 1
             *	
             *	추가해야할 Form의 너비
             *	= 추가로 필요한 그룹박스 너비 * 2
			 */
        }

        private static Form_SelectionList _Instance = new Form_SelectionList();
        public static Form_SelectionList GetInstance()
        {
            return _Instance;
        }
        #endregion /constructor

        /// <summary>
        /// 추가될 컨트롤의 수량에 맞춰 폼의 높이를 조정하게 된다.
        /// </summary>
        readonly int DEFAULT_FORM_HEIGHT = 0;
		readonly int DEFUALT_FORM_WIDTH = 0;

		readonly int DEFAULT_GROUPBOX_WIDTH = 0;
        readonly int DEFAULT_BUTTON_WIDTH = 0; // 버튼의 너비
		readonly int GROUP_BOX_LABEL_HEADER_HEIGHT = 0; // 그룹박스 헤더 높이
        #region const

        private const int c_MaxCountOfDisplay = 10;     // 한 화면에 출력되는 열 갯수
        private const int c_LedWidth = 30;				// LED 컨트롤 너비
        private const int c_LedHeight = 45;             // LED 컨트롤 높이
		private const int c_ButtonHeight = 45;//46;			// 버튼 높이 // 초기에 왜 LED를 높이를 다르게 했었지?

        private const int c_Padding = 10;        // 각 컨트롤간의 간격

		private const int c_buttonPaddingForText = 20;	// 버튼의 텍스트와 오른쪽 Padding

        private const string	PREVALUE				= "Prevalue.";
		private const string	DEFAULT_VALUE			= "DefaultIndex.";
		#endregion /const

		#region 변수

		private List<int> m_ListSelected				= new List<int>();

		int _maxStringDesignWidth = 0;    // 최대 문자열 길이
		int _calculatedButtonWidth = 0;	// 버튼의 너비
        #endregion

        #region field
        Dictionary<int, SelectionItem> _totalItemList = new Dictionary<int, SelectionItem>();	// reference

		Dictionary<int, DisplayItem> _choiceList = new Dictionary<int, DisplayItem>();		// 선택지
		Dictionary<int, DisplayItem> _selectedList = new Dictionary<int, DisplayItem>();	// 선택한 것

		int _currentChoicePage = 0;
		int _currentSelectedPage = 0;

		string _filterKeyword = string.Empty;
		bool _isUseMultiSelection = false;

		string _defaultValue = string.Empty;
		int _defaultIndex = -1;

		string _resultString = string.Empty;
		int _resultIntager = -1;
		int[] _resultArrayIntager = null;

		bool _isMouseDownTitle = false;
		Point _mouseDownCoordinate = new Point();
		// 2025.06.13 by junho [ADD] Previous value 추가
		string _previousValue = string.Empty;
		#endregion /field


        /// <summary>
        /// 2020.05.26 by twkang [ADD] 폼이 생성될 때 초기화 작업을 수행한다.
        /// </summary>
		private void InitializeForm(ref string strTitle, ref string[] arrItems, int[] arrIndex, ref string strPreValue, bool bMultiSelection, ref string strDefaultValue, string strPreviousValue = "", string strPreviousTime = "")
		{
			_lbl_Title.Text = strTitle;
			// _gbSelectionItems.Text = strTitle;

            _isUseMultiSelection	= bMultiSelection;
			btn_AllSelect.Visible = _isUseMultiSelection;
			btn_AllUnSelect.Visible = _isUseMultiSelection;

			//Default Value
			if (strDefaultValue.Contains(DEFAULT_VALUE))
			{
				string[] temp = strDefaultValue.Split('.');
				int.TryParse(temp[1], out _defaultIndex);
				_defaultValue = string.Empty;
			}
			else
			{
				_defaultIndex = -1;
				_defaultValue = strDefaultValue;
			}

			// make reference list
			_totalItemList.Clear();
			if(arrIndex == null)
			{
				arrIndex = new int[arrItems.Length];
				for(int i = 0; i<arrItems.Length; ++i)
				{
					arrIndex[i] = i;
				}
			}

            _maxStringDesignWidth = 0;
            using (Graphics g = this.CreateGraphics())
			{
				using (Font font = new Font("맑은 고딕", 12, FontStyle.Bold))
				{
					for (int i = 0; i < arrItems.Length; ++i)
					{
						_totalItemList.Add(i, new SelectionItem(i, arrItems[i], arrIndex[i]));

						SizeF size = g.MeasureString(arrItems[i], font);

                        if (size.Width > _maxStringDesignWidth)
                        {
                            _maxStringDesignWidth = (int)size.Width;
                        }
                    }
				}
            }

			_calculatedButtonWidth = _maxStringDesignWidth + c_buttonPaddingForText;
			if (DEFAULT_BUTTON_WIDTH > _calculatedButtonWidth)
			{
				_calculatedButtonWidth = DEFAULT_BUTTON_WIDTH;
			}
			this.Width = DEFUALT_FORM_WIDTH + (_calculatedButtonWidth - DEFAULT_BUTTON_WIDTH) * 2;

            // Apply pre selected values
            if (strPreValue.Contains(PREVALUE))
			{
				int preSelectNumber = -1;
				int.TryParse(strPreValue.Split('.')[1], out preSelectNumber);
				for (int i = 0; i < _totalItemList.Count; ++i)
				{
					if(_totalItemList[i].Number == preSelectNumber)
					{
						_totalItemList[i].Selected = true;
					}
				}
			}
			else
			{
				string[] preSelectNames = strPreValue.Replace(" ", "").Split(',');

				if (false == _isUseMultiSelection && preSelectNames.Length > 1)
					Array.Resize(ref preSelectNames, 1);

				if (false == preSelectNames.Contains(""))
				{
					for (int i = 0; i < preSelectNames.Length; ++i)
					{
						foreach(var item in _totalItemList.Values)
						{
							if (item.Name.Replace(" ", "") == preSelectNames[i])
								item.Selected = true;
						}
					}
				}
			}

			_currentChoicePage = 0;
			_currentSelectedPage = 0;
			_filterKeyword = string.Empty;
			lbl_Filtering.Text = _filterKeyword;

			// 2025.06.13 by junho [ADD] Previous value 추가
			_previousValue = strPreviousValue;
			DateTime previousTime;
			if(DateTime.TryParse(strPreviousTime, out previousTime) && false == string.IsNullOrWhiteSpace(_previousValue))
			{
				btn_PreviousSet.Visible = true;
				btn_PreviousSet.SubText = previousTime.ToString("yyyy-MM-dd HH:mm:ss");
			}
			else
			{
				btn_PreviousSet.Visible = false;
			}

			UpdateUi();

			this.CenterToScreen();
		}

        #region interface

		#region Create Form
		/// <summary>
        /// 2020.05.28 by twkang [ADD] SelectionList Form 을 생성한다.
        /// </summary>
        public bool CreateForm(string strTitle, string[] arrItems, string strPreValue, bool bMultiSelection = false, string strDefaultValue = "", string strPreviousValue = "", string strPreviousTime = "")
        {
            if (null == arrItems) { return false;}

			if(string.IsNullOrEmpty(strDefaultValue))
			{
				strDefaultValue = strPreValue;
			}

            InitializeForm(ref strTitle, ref arrItems, null, ref strPreValue, bMultiSelection,  ref strDefaultValue, strPreviousValue, strPreviousTime);

            this.ShowDialog();

            if(this.DialogResult==System.Windows.Forms.DialogResult.OK)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 2020.05.28 by twkang [ADD] SelectionList Form 을 생성한다.
        /// </summary>
        public bool CreateForm(string strTitle, string[] arrItems, int[] arrIndex, int nPreValueIndex, bool bMultiSelection = false, int nDefaultValue = -1)
        {
            if(null == arrIndex)
            {
                return false;
            }

			string strDefaultValue	= DEFAULT_VALUE + nDefaultValue.ToString();
			string strPreValue		= string.Empty;

			if(nPreValueIndex > -1 || nPreValueIndex < arrIndex.Length)
			{
				strPreValue = PREVALUE + nPreValueIndex.ToString();
			}
			else
			{
				strPreValue	= Define.DefineConstant.Common.NONE;
			}

            InitializeForm(ref strTitle, ref arrItems, arrIndex, ref strPreValue, bMultiSelection, ref strDefaultValue);
            
			this.ShowDialog();
            
            if(this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                return true;
            }

            return false;
        }
		/// <summary>
		/// 2020.06.05 by twkang [ADD] SelectionList Form 을 생성한다.
		/// </summary>
		public bool CreateForm(string strTitle, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST enList, string strPreValue, bool bMultiSelection = false, string strDefaultValue = "")
		{
			if(string.IsNullOrEmpty(strDefaultValue))
			{
				strDefaultValue	= strPreValue;
			}

			string[] strArr = null;

			if(FrameOfSystem3.Functional.SelectionList.GetInstance().GetList(enList, ref strArr))
			{
				return CreateForm(strTitle, strArr, strPreValue, bMultiSelection, strDefaultValue);
			}
			
			return false;
		}
		public bool CreateForm(string strTitel, string[] arItems, int[] arIndex, string[] arPreValue, bool bMultiSelection = false, string strDefaultValue = "")
		{
			string strPreValue	= string.Empty;
			if(arPreValue != null)
			{
				strPreValue		= string.Join(", ", arPreValue);
			}

			InitializeForm(ref strTitel, ref arItems, arIndex, ref strPreValue, bMultiSelection, ref strDefaultValue);

			this.ShowDialog();

			if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
			{
				return true;
			}

			return false;
		}

		// 2025.06.13 by junho [ADD] Previous value용 추가
		public bool CreateForm(string strTitle, Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST enList, string presentValue, string previousValue, string previousTime)
		{
			string[] strArr = null;

			if (FrameOfSystem3.Functional.SelectionList.GetInstance().GetList(enList, ref strArr))
			{
				if (false == strArr.Contains(previousValue))
					previousValue = "";

				return CreateForm(strTitle, strArr, presentValue, strPreviousValue: previousValue, strPreviousTime: previousTime);
			}

			return false;
		}
		#endregion

		#region GetData
		/// <summary>
        /// 2022.03.26 by shkim [MOD] SelectionList 결과값 선택 순서대로 받환 받는 옵션 추가
     	/// 2020.05.28 by twkang [ADD] SelectionList 결과값을 반환한다.
     	/// </summary>
		public void GetResult(ref int[] nArrResult, bool bUseOrder = false)
		{
            if(false == bUseOrder)
            {
                nArrResult = _resultArrayIntager;
            }
            else
            {
				nArrResult = _resultArrayIntager.Cast<int>().ToArray();
            }
		}
		/// <summary>
		/// 2020.05.28 by twkang [ADD] SelectionList 결과값을 반환한다.
		/// </summary>
		public void GetResult(ref string strResult)
		{
			strResult = _resultString;
		}
		/// <summary>
		/// /// 2020.05.28 by twkang [ADD] SelectionList 결과값을 반환한다.
		/// </summary>
		public void GetResult(ref int nIndex)
		{
			nIndex = _resultIntager;
		}
		#endregion
		
        #endregion /interface

        #region ui event
        /// <summary>
        /// 2020.05.28 by twkang [ADD] Apply or Cancel 버튼을 눌렀을 경우의 이벤트 처리
        /// </summary>
        private void Click_ApplyOrCancel(object sender, EventArgs e)
        {
            DestroyControls();

            Control ctr = sender as Control;

            switch(ctr.TabIndex)
            {
                case 0: // Apply
                    MakeResult();
                    this.DialogResult       = System.Windows.Forms.DialogResult.OK;
                    break;
                case 1: // Cancel
                    this.DialogResult       = System.Windows.Forms.DialogResult.Cancel;
                    break;
            }
            this.Close();
        }
        /// <summary>
        /// 2020.05.28 by twkang [ADD] 페이지 이동 버튼을 눌렀을 때 이벤트처리
        /// </summary>
        private void Click_MovePage(object sender, EventArgs e)
        {
			if (sender == btn_ChoiceNext)
			{
				++_currentChoicePage;
			}
			else if (sender == btn_ChoicePrev)
			{
				--_currentChoicePage;
			}
			else if (sender == btn_SelectedNext)
			{
				++_currentSelectedPage;
			}
			else if (sender == btn_SelectedPrev)
			{
				--_currentSelectedPage;
			}
			else if (sender == btn_ChoicePage)
			{
				var calc = Form_Calculator.GetInstance();
				if (false == calc.CreateForm(_currentChoicePage.ToString(), "1", ((int)((_choiceList.Count) / c_MaxCountOfDisplay) + 1).ToString(), "page", "Select Page"))
					return;

				calc.GetResult(ref _currentChoicePage);
				--_currentChoicePage;
			}
			else if(sender == btn_SelectedPage)
			{
				var calc = Form_Calculator.GetInstance();
				if (false == calc.CreateForm(_currentSelectedPage.ToString(), "1", ((int)((_selectedList.Count) / c_MaxCountOfDisplay) + 1).ToString(), "page", "Select Page"))
					return;

				calc.GetResult(ref _currentSelectedPage);
				--_currentSelectedPage;
			}
			else return;

			UpdateUi();
        }

		private void Form_SelectionList_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				DestroyControls();

				this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			}
		}

		/// <summary>
		/// 2021.07.28 by twkang [ADD] All Select, UnSelect 버튼 클릭 이벤트
		/// </summary>
		private void Click_SelectButton(object sender, EventArgs e)
		{
			bool isEnable;
			if (sender == btn_AllSelect)
			{
				isEnable = true;
			}
			else if (sender == btn_AllUnSelect)
			{
				isEnable = false;
			}
			else return;

			foreach(var item in _totalItemList.Values)
			{
				item.Selected = isEnable;
			}
			UpdateUi();
		}
		private void Click_PreviousSet(object sender, EventArgs e)
		{
			foreach(var item in _totalItemList.Values)
			{
				item.Selected = item.Name == _previousValue;
			}
			UpdateUi();
		}

		/// <summary>
		/// 2021.07.28 by twkang [ADD] TitleBar Mouse Down 이벤트
		/// </summary>
		private void MouseDown_Title(object sender, MouseEventArgs e)
		{
			_isMouseDownTitle = true;
			_mouseDownCoordinate = e.Location;
		}
		/// <summary>
		/// 2021.07.28 by twkang [ADD] TitleBar Mouse Move 이벤트
		/// </summary>
		private void MouseMove_Title(object sender, MouseEventArgs e)
		{
			if (_isMouseDownTitle)
			{
				this.SetDesktopLocation(MousePosition.X - _mouseDownCoordinate.X, MousePosition.Y - _mouseDownCoordinate.Y);
			}
		}
		/// <summary>
		/// 2021.07.28 by twkang [ADD] TitleBar Mouse Up 이벤트
		/// </summary>
		private void MouseUp_Title(object sender, MouseEventArgs e)
		{
			_isMouseDownTitle = false;
		}
        #endregion /ui event

		#region method
		private void UpdateUi()
		{
			DestroyControls();
			foreach(var kvpItem in _totalItemList)
			{
				// 2024.11.11 by junho [MOD] 대소문자 구분하지 않도록 변경
				//if (_filterKeyword == string.Empty || kvpItem.Value.Name.Contains(_filterKeyword))
				if (_filterKeyword == string.Empty || kvpItem.Value.Name.IndexOf(_filterKeyword, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					var item = new DisplayItem(_choiceList.Count, kvpItem.Value, false, _calculatedButtonWidth, GROUP_BOX_LABEL_HEADER_HEIGHT);
					_choiceList.Add(_choiceList.Count, item);
                    _gbSelectionItems.Controls.Add(item.ButtonControl);
					_gbSelectionItems.Controls.Add(item.LedControl);

					item.ItemClicked += ButtonClicked;
				}

				if(kvpItem.Value.Selected)
				{
					var item = new DisplayItem(_selectedList.Count, kvpItem.Value, true, _calculatedButtonWidth, GROUP_BOX_LABEL_HEADER_HEIGHT);
					_selectedList.Add(_selectedList.Count, item);
                    _gbSelectedItems.Controls.Add(item.ButtonControl);
					_gbSelectedItems.Controls.Add(item.LedControl);

					item.ItemClicked += ButtonClicked;
				}
			}

			#region form resizing

			int oneLineArea = c_LedHeight + c_Padding;
			int maxDisplayCount = Math.Max(_choiceList.Count, _selectedList.Count);
			if (maxDisplayCount > c_MaxCountOfDisplay)
				maxDisplayCount = c_MaxCountOfDisplay;

			int totalItemHeight = oneLineArea * maxDisplayCount;
			this.Height = DEFAULT_FORM_HEIGHT + totalItemHeight;

            #endregion /form resizing

            #region page information
            int maxChoicePage = (int)((_choiceList.Count) / c_MaxCountOfDisplay);
			int maxSelectedPage = (int)((_selectedList.Count) / c_MaxCountOfDisplay);

			if (_currentChoicePage < 0)						_currentChoicePage = 0;
			else if (_currentChoicePage > maxChoicePage)	_currentChoicePage = maxChoicePage;

			if (_currentSelectedPage < 0)						_currentSelectedPage = 0;
			else if (_currentSelectedPage > maxSelectedPage)	_currentSelectedPage = maxSelectedPage;

			btn_ChoicePage.Text = (_currentChoicePage + 1).ToString();
			btn_ChoicePage.SubText = string.Format("/ {0}", maxChoicePage + 1);

			btn_SelectedPage.Text = (_currentSelectedPage + 1).ToString();
			btn_SelectedPage.SubText = string.Format("/ {0}", maxSelectedPage + 1);
			#endregion /page information

			int startIndex, endIndex;
			startIndex = _currentChoicePage * c_MaxCountOfDisplay;
			endIndex = startIndex + c_MaxCountOfDisplay;
			if (endIndex >= _choiceList.Count) endIndex = _choiceList.Count;
			for (int i = startIndex; i < endIndex; ++i)
			{
				_choiceList[i].ShowHide(true);
			}

			startIndex = _currentSelectedPage * c_MaxCountOfDisplay;
			endIndex = startIndex + c_MaxCountOfDisplay;
			if (endIndex >= _selectedList.Count) endIndex = _selectedList.Count;
			for (int i = startIndex; i < endIndex; ++i)
			{
				_selectedList[i].ShowHide(true);
			}


            Refresh();
		}
		private void ButtonClicked(int index)
		{
			if (_totalItemList.Count <= index)
				return;

			bool isTurnOn = (false == _totalItemList[index].Selected);
			if(isTurnOn && false == _isUseMultiSelection)
			{
				foreach (var item in _totalItemList.Values)
				{
					item.Selected = false;
				}
			}

			_totalItemList[index].Selected = isTurnOn;
			UpdateUi();
		}

		/// <summary>
		/// 2020.05.28 by twkang [ADD] 생성한 컨트롤들을 제거한다.
		/// </summary>
		private void DestroyControls()
		{
			foreach (var item in _choiceList.Values)
			{
				_gbSelectionItems.Controls.Remove(item.LedControl);
                _gbSelectionItems.Controls.Remove(item.ButtonControl);
				item.Dispose();
			}
			foreach (var item in _selectedList.Values)
			{
				_gbSelectedItems.Controls.Remove(item.LedControl);
                _gbSelectedItems.Controls.Remove(item.ButtonControl);
				item.Dispose();
			}

			_choiceList.Clear();
			_selectedList.Clear();
		}
		/// <summary>
		/// 2020.05.28 by twkang [ADD] 선택된 아이템으로 결과값을 생성한다.
		/// </summary>
		private void MakeResult()
		{
			List<int> resultNumbers = new List<int>();
			List<string> resultNames = new List<string>();
			foreach (var item in _totalItemList.Values)
			{
				if (item.Selected)
				{
					resultNumbers.Add(item.Number);
					resultNames.Add(item.Name);
				}
			}
			if (resultNumbers.Count < 1)
			{
				_resultString = _defaultValue;
				_resultIntager = _defaultIndex;
				_resultArrayIntager = new int[1] { _defaultIndex };
			}
			else
			{
				_resultArrayIntager = resultNumbers.ToArray();
				_resultIntager = resultNumbers[0];
				_resultString = string.Join(", ", resultNames.ToArray());
			}
			return;
		}

		#endregion /method

		private class SelectionItem
		{
			string _itemName;
			int _itemIndex;
			int _itemNumber;
			bool _isSelected = false;

			public SelectionItem(int index, string name, int number)
			{
				_itemName = name;
				_itemIndex = index;
				_itemNumber = number;
			}
			public string Name { get { return _itemName; } }
			public int Number { get { return _itemNumber; } }
			public int Index { get { return _itemIndex; } }
			public bool Selected { get { return _isSelected; } set { _isSelected = value; } }
		}
		private class DisplayItem
		{
			int _index;			// 몇번째 item인지
			int _originIndex;	// totalList에서의 index
			int _displayIndex;
			bool _isSelectedList;

			Sys3Controls.Sys3button _button;
			Sys3Controls.Sys3LedLabel _led;

			int _yPositionOffset = 0;

			public DisplayItem(int index, SelectionItem data, bool isSelected, int buttonWidth, int yPositionOffset)
			{
				_yPositionOffset = yPositionOffset;

                _index = index;
				_originIndex = data.Index;
				_displayIndex = _index - ((int)(_index / c_MaxCountOfDisplay) * c_MaxCountOfDisplay);

				_led = new Sys3Controls.Sys3LedLabel();
				_led.Height = c_LedHeight;
				_led.Width = c_LedWidth;

				_isSelectedList = isSelected;
				if (_isSelectedList)
				{
					_led.Active = true;
				}
				else
				{
					_led.Active = data.Selected;
				}

				_button = new Sys3Controls.Sys3button();
				_button.Text = data.Name;
				_button.SubText = string.Format("[ {0} ]", data.Number.ToString());

				_button.Height = c_ButtonHeight;
				_button.Width = buttonWidth;

				_button.UseSubFont = true;
				_button.TextAlignSub = Sys3Controls.EN_TEXTALIGN.TOP_LEFT;
				_button.SubFont = new Font("맑은 고딕", 10, FontStyle.Bold);
				_button.SubFontColor = Color.Black;

				_button.TextAlignMain = Sys3Controls.EN_TEXTALIGN.BOTTOM_RIGHT;
				_button.MainFontColor = Color.Black;
				_button.MainFont = new Font("맑은 고딕", 12, FontStyle.Bold);

				_button.GradientFirstColor = Color.FromArgb(235, 235, 235);
				_button.GradientSecondColor = Color.FromArgb(180, 186, 193);
				_button.GradientAngle = 85;

				_button.BorderWidth = 1;
				_button.MouseClick += ButtonClicked;

				RePositioning();
				ShowHide(false);
			}
			~DisplayItem()
			{
				_button.MouseClick -= ButtonClicked;
				ItemClicked = null;
			}
			
			public void Dispose()
			{
				_led.Dispose();
				_button.Dispose();
			}
			public Control LedControl { get { return _led; } }
			public Control ButtonControl { get { return _button; } }

            public void RePositioning()
            {
                Point pt = new Point();

                int nHeightPadding = c_LedHeight + c_Padding;
                pt.Y = c_Padding + (_displayIndex * nHeightPadding) + _yPositionOffset;

				// 버튼 위치
				pt.X = c_Padding + c_LedWidth - 1;
                _button.Location = pt;

                // LED 위치
                pt.X = c_Padding;
                _led.Location = pt;
            }
            public void ShowHide(bool isShow)
			{
				if(isShow)
				{
					_led.Show();
					_button.Show();
				}
				else
				{
					_led.Hide();
					_button.Hide();
				}
			}

			public event Action<int> ItemClicked = null;

			private void ButtonClicked(object sender, EventArgs e)
			{
				if (ItemClicked != null)
					ItemClicked(_originIndex);
			}
		}

		private void lbl_Filtering_Click(object sender, EventArgs e)
		{
			var keyboard = Form_Keyboard.GetInstance();
			if (false == keyboard.CreateForm(_filterKeyword))
				return;

			keyboard.GetResult(ref _filterKeyword);
			UpdateUi();
			lbl_Filtering.Text = _filterKeyword;
		}
    }
}
