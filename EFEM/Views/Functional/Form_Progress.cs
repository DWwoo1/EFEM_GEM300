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

namespace FrameOfSystem3.Views.Functional
{
    public partial class Form_Progress : Form
    {
        #region const
		private const int MAX_FORM_SIZE_Y			= 1024;

        private const int ROW_HEIGHT                = 30;
		private const int CONFIRM_BUTTON_LOCATION_X	= 330;
		private const int CELL_FONT_SIZE			= 15;

		private const int COLUMN_INDEX_OF_CONTENTS	= 0;
		private const int COLUMN_INDEX_OF_ENABLE	= 1;
        #endregion
        
        #region Variables
        private bool m_bLoad                        = false;
        private bool m_bSuccess                     = true;

        private int m_nNumberOfSuccess              = 0;
		private int m_nRecentRowIndex				= -1;

        private string m_strEndStep                 = string.Empty;
    
        private static Timer m_timerForUpdate       = new Timer();

        private ConcurrentQueue<KeyValuePair<bool, string>> m_queueForResult = new ConcurrentQueue<KeyValuePair<bool,string>>();
        #endregion

        #region Property
        #endregion

        #region Contructor & Destructor

        public Form_Progress(int nInterval)
        {            
            this.DoubleBuffered = true;
            
            m_timerForUpdate.Interval       = nInterval;
            m_timerForUpdate.Tick          += new EventHandler(CheckingQueue);
            m_timerForUpdate.Start();

            Shown += Form_Progress_Shown;

            InitializeComponent();

			m_dgViewProGress.RowTemplate.Height	= ROW_HEIGHT;

            // 2025.02.28. by shkim. [ADD] 프로그램 실행 상태 표시 추가(관리자 or 일반)
            m_groupTitle.Text = string.Format("{0} ({1})", Program.AssemblyName, Program.IsAdministratorMode ? "Admin Mode" : "Normal Mode");
            // 2025.02.28. by shkim. [END]
        }

        #endregion

        #region Internal Interface
        /// <summary>
        /// 2020.05.13 by twkang [ADD] 타이머에 의해 호출되는 함수이다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckingQueue(object sender, EventArgs e)
        {
            if (m_queueForResult.IsEmpty) { return; }
           
            UpdateGrid();
        }
        /// <summary>
        /// 2020.05.13 by twkang [ADD] Initialize 결과를 업데이트 한다.
        /// </summary>
        private void UpdateGrid()
        {
            var kvp         = DequeueResult();
			
            //being Init
            if(true == kvp.Key)
            {
                m_dgViewProGress.Rows.Add();
				m_nRecentRowIndex++;

				if(this.Height + ROW_HEIGHT < MAX_FORM_SIZE_Y)
				{
					m_groupTitle.Height += ROW_HEIGHT;
					m_dgViewProGress.Height += ROW_HEIGHT;

					m_groupTitle.Invalidate();
					this.Height += ROW_HEIGHT;
					this.CenterToScreen();
				}

                //Init 작업이 종료되었을 때
                if(m_strEndStep == kvp.Value)
                {
                    if (true == m_bSuccess)
                    {
                        this.Close();
                    }
                    else
                    {
                        string temp = string.Format("({0}/{1})", m_nNumberOfSuccess,  m_dgViewProGress.RowCount - 2);

                        m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_CONTENTS].Style.Font      = new System.Drawing.Font("맑은고딕", CELL_FONT_SIZE, FontStyle.Bold);
                        m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_CONTENTS].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_CONTENTS].Style.ForeColor = Color.Blue;
                        m_dgViewProGress[COLUMN_INDEX_OF_CONTENTS, m_nRecentRowIndex].Value = "Equipment is ready to be operated   " + temp;

						m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_CONTENTS].Style.SelectionBackColor	= Color.White;
						m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor		= Color.White;
						m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_CONTENTS].Style.SelectionForeColor	= Color.Blue;
						m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionForeColor		= Color.Blue;

                        m_btnConfirm.Location       = new Point(CONFIRM_BUTTON_LOCATION_X, this.Height - (m_btnConfirm.Size.Height + 10));
                        m_btnConfirm.Visible        = true;
                        m_btnConfirm.BringToFront();
                        m_btnConfirm.Invalidate();
                    }
                }
                else
                {
                    m_dgViewProGress[COLUMN_INDEX_OF_CONTENTS, m_nRecentRowIndex].Value							= kvp.Value;
                    m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_CONTENTS].Style.ForeColor	= Color.Blue;
                    m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor		= Color.Blue;

					m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_CONTENTS].Style.SelectionBackColor	= Color.White;
					m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor		= Color.Blue;
					m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_CONTENTS].Style.SelectionForeColor	= Color.Blue;
                }
            }
            //현재 인스턴스의 Initialize 결과를 업데이트 한다.
            else
            {
                bool bInitResult = kvp.Value == "True" ? true : false;
                m_bSuccess &= bInitResult;
				
                Color cColor = bInitResult == true ? Color.FromArgb(24, 243, 1) : Color.Red;

                if (false == bInitResult)
                {
                    m_dgViewProGress[COLUMN_INDEX_OF_CONTENTS, m_nRecentRowIndex].Value += Define.DefineConstant.Common.FAIL;
                    m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_CONTENTS].Style.ForeColor			= cColor;
					m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_CONTENTS].Style.SelectionForeColor	= cColor;
                    
                }
                else
                {
                    m_dgViewProGress[COLUMN_INDEX_OF_CONTENTS, m_nRecentRowIndex].Value += Define.DefineConstant.Common.SUCCESS;
                    m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_CONTENTS].Style.ForeColor			= Color.Green;
					m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_CONTENTS].Style.SelectionForeColor	= Color.Green;
                    ++m_nNumberOfSuccess;
                }

                m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_ENABLE].Style.BackColor			= cColor;
				m_dgViewProGress.Rows[m_nRecentRowIndex].Cells[COLUMN_INDEX_OF_ENABLE].Style.SelectionBackColor = cColor;
            }

        }
        #endregion

        #region External Interface
        /// <summary>
        /// 2020.05.18 by twkang [ADD] 작업 결과를 Enqueue 한다.
        /// </summary>        
        public void EnqueueResult(bool bRowAdd, ref string strResult)
        {
            m_queueForResult.Enqueue(new KeyValuePair<bool, string>(bRowAdd, strResult));
        }
        public void EnqueueResult(bool bRowAdd, ref bool strResult)
        {			
            m_queueForResult.Enqueue(new KeyValuePair<bool, string>(bRowAdd, strResult.ToString()));
        }
        /// <summary>
        /// 2020.05.18 by twkang [ADD] 작업 결과를 Dequeue 한다.
        /// </summary>
        public KeyValuePair<bool, string> DequeueResult()
        {
            KeyValuePair<bool, string> temp;
            m_queueForResult.TryDequeue(out temp);
            return temp;
        }
       
        /// <summary>
        /// 2020.05.18 by twkang [ADD] 초기화 작업의 개수를 저장한다.
        /// </summary>        
        public void SetEndStep(int nEndStep)
        {
            m_strEndStep  = (nEndStep).ToString();
        }
		public bool IsFormLoad()
		{
			return m_bLoad;
		}
        #endregion

        #region UI_Event
        /// <summary>
        /// 2020.05.13 by twkang [ADD] Form 이 처음 표시될 때 발생하는 이벤트.
        /// </summary>
        private void Form_Progress_Shown(object sender, EventArgs e)
        {
            m_bLoad = true;
        }
        /// <summary>
        /// 2020.05.13 by twkang [ADD] Confirm 버튼을 눌렀을때 발생하는 이벤트이다.
        /// </summary>
        private void m_btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
            m_bLoad = false;
        }
        #endregion
    }
}
