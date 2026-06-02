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
    public partial class Form_ColorPalette : Form
    {
        #region 상수
        const int c_nColorR = 127;
        const int c_nColorG = 127;
        const int c_nColorB = 127;

		// 미사용?
		//readonly string c_strGroupName  = "COLOR PALETTE";
		//readonly string c_strApply      = "APPLY";
		//readonly string c_strCancel     = "CANCEL";
        #endregion

        #region 변수
        private int m_nColorR = 0;
        private int m_nColorG = 0;
        private int m_nColorB = 0;

        private Form_Calculator m_InstanceCalculator    = Form_Calculator.GetInstance();

        // 2023.09.24. by shkim [ADD] Groupbox Event를 통한 Form 위치 이동 기능 추가
        Point _mouseDownPointOfFormGroupBox;
        // 2023.09.24. by shkim [END]
        #endregion

        #region 속성
        public Color ColorResult
        {
            get { return Color.FromArgb(m_nColorR, m_nColorG, m_nColorB); }
        }
        #endregion

        #region 싱글톤
        private Form_ColorPalette()
        {
            InitializeComponent();
        }
        private static Form_ColorPalette _Instance  = null;
        public static Form_ColorPalette GetInstance()
        {
            if(_Instance == null)
            {
                _Instance   = new Form_ColorPalette();
            }

            return _Instance;
        }
        #endregion

        #region 외부 인터페이스
        /// <summary>
        /// 2018.08.22 by yjlee [ADD] 컬러 파레트 폼을 생성한다.
        /// </summary>
        public bool CreateForm()
        {
            return CreateForm(c_nColorR, c_nColorG, c_nColorB);
        }
        /// <summary>
        /// 2018.08.22 by yjlee [ADD] 컬러 파레트 폼을 생성한다.
        /// </summary>
        public bool CreateForm(uint nR, uint nG, uint nB)
        {
            InitLanguage();

            InitForm(nR, nG, nB);

            this.ShowDialog();

            if(this.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 2018.08.22 by yjlee [ADD] 컬러 파레트 폼을 생성한다.
        /// </summary>
        public bool CreateForm(Color clr)
        {
            return CreateForm(clr.R, clr.G, clr.B);
        }
        #endregion

        #region 내부 인터페이스
        /// <summary>
        /// 2018.08.22 by yjlee [ADD] 값을 초기화한다.
        /// </summary>
        private void InitForm(uint nR, uint nG, uint nB)
        {
            m_nColorR = (int)nR;
            m_nColorG = (int)nG;
            m_nColorB = (int)nB;

            SetColorRed(nR);
            SetColorGreen(nG);
            SetColorBlue(nB);
        }
        /// <summary>
        /// 2018.08.22 by yjlee [ADD] 언어를 초기화한다.
        /// </summary>
        private void InitLanguage()
        {
			//Language InstanceLanguage   = Language.GetInstance();

			//m_groupColorPalette.Text    = InstanceLanguage.TranslateEnglishToRecentLanguage(c_strGroupName);

			//m_btnApply.Text             = InstanceLanguage.TranslateEnglishToRecentLanguage(c_strApply);
			//m_btnCancel.Text            = InstanceLanguage.TranslateEnglishToRecentLanguage(c_strCancel);
        }
        /// <summary>
        /// 2018.08.22 by yjlee [ADD] 빨간색 색 값을 설정한다.
        /// </summary>
        private void SetColorRed(uint nColor)
        {
            m_nColorR           = (int)nColor;
            m_pBarRed.Tick      = nColor;

            m_ledColor.ColorActive  = Color.FromArgb(m_nColorR, m_nColorG, m_nColorB);
        }
        /// <summary>
        /// 2018.08.22 by yjlee [ADD] 초록색 색 값을 설정한다.
        /// </summary>
        private void SetColorGreen(uint nColor)
        {
            m_nColorG           = (int)nColor;
            m_pBarGreen.Tick    = nColor;

            m_ledColor.ColorActive  = Color.FromArgb(m_nColorR, m_nColorG, m_nColorB);
        }
        /// <summary>
        /// 2018.08.22 by yjlee [ADD] 파란색 색 값을 설정한다.
        /// </summary>
        private void SetColorBlue(uint nColor)
        {
            m_nColorB           = (int)nColor;
            m_pBarBlue.Tick     = nColor;

            m_ledColor.ColorActive  = Color.FromArgb(m_nColorR, m_nColorG, m_nColorB);
        }
        #endregion

        #region UI 이벤트 처리
        /// <summary>
        /// 2018.08.22 by yjlee [ADD] 적용 버튼 클릭 시
        /// </summary>
        private void Click_Apply(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Close();
        }
        /// <summary>
        /// 2018.08.22 by yjlee [ADD] 취소 버튼 클릭 시
        /// </summary>
        private void ClIck_Cancel(object sender, EventArgs e)
        {
            m_nColorR = m_nColorG = m_nColorB = 0;

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.Close();
        }
        /// <summary>
        /// 2018.08.22 by yjlee [ADD] 마우스를 떼었을 때 이벤트 발생
        /// </summary>
        private void MouseUp_ProgressBar(object sender, MouseEventArgs e)
        {
            Control ctr = sender as Control;

            switch (ctr.TabIndex)
            {
                case 0: // RED
                    SetColorRed(m_pBarRed.Tick);
                    break;
                case 1: // GREEN
                    SetColorGreen(m_pBarGreen.Tick);
                    break;
                case 2: // BLUE
                    SetColorBlue(m_pBarBlue.Tick);
                    break;
            }
        }
        /// <summary>
        /// 2018.08.22 by yjlee [ADD] 라벨을 클릭했을 경우 발생
        /// </summary>
        private void Click_Label(object sender, EventArgs e)
        {
            Control ctr         = sender as Control;
            string strOldValue  = "";

            if(m_InstanceCalculator.CreateForm(strOldValue
                , "0"
                ,"255"))
            {
                // uint nResult     = (uint)m_InstanceCalculator.GetResultByInteger();

				int nResult = 0;
				m_InstanceCalculator.GetResult(ref nResult);

                switch (ctr.TabIndex)
                {
                    case 0: // RED
                        SetColorRed((uint)nResult);
                        break;
                    case 1: // GREEN
                        SetColorGreen((uint)nResult);
                        break;
                    case 2: // BLUE
                        SetColorBlue((uint)nResult);
                        break;
                }
            }
        }
        #endregion


        // 2023.09.24. by shkim [ADD] Groupbox Event를 통한 Form 위치 이동 기능 추가
        void OnMouseDown_FormGroupBox(object sender, MouseEventArgs e)
        {
            _mouseDownPointOfFormGroupBox = new Point(e.X, e.Y);
        }

        void OnMouseMove_FormGroupBox(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Location = new Point(this.Left - (_mouseDownPointOfFormGroupBox.X - e.X), this.Top - (_mouseDownPointOfFormGroupBox.Y - e.Y));
            }
        }
        // 2023.09.24. by shkim [END]
    }
}
