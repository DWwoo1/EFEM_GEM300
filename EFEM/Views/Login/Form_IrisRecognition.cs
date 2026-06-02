using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using IrisysSDK_;

namespace FrameOfSystem3.Views.Login
{
    public partial class Form_IrisRecognition : Form
    {
        #region Contants
        private const int INTERVAL_TIMER                = 100;
        private const int MAX_COUNT_DOD                 = 5;

        private readonly string MESSAGE_DOD             = "...";
        private readonly string MESSAGE_INIT_SDK        = "INITIALIZE THE SDK";
        private readonly string MESSAGE_INIT_CAMERA     = "INITIALIZE THE CAMERA";

        private readonly string MESSAGE_ENROLL          = "ENROLL YOUR IRIS";
        private readonly string MESSAGE_ENROLL_FINISH   = "ENROLLED!!";

        private readonly string MESSAGE_AUTHO           = "AUTHORIZE YOUR IRIS";
        private readonly string MESSAGE_AUTHO_FINISH    = "AUTHORIZED!!";
        #endregion End - Contants

        #region Enumerable
        private enum EN_IRIS_STEP
        {
            INIT_SDK        = 0,
            INIT_CAMERA,

            ENROLL_IRIS,
            ENROLL_FINISH,

            AUTHO_IRIS,
            AUTHO_FINISH,
        }
        #endregion End - Enumerable

        #region Variables
        private Timer m_timerForPreview     = new Timer();

        private IrisysSDK m_instanceSDK     = null;

        private bool m_bEnrollMode          = false;
        private EN_IRIS_STEP m_enStep       = EN_IRIS_STEP.INIT_SDK;

        private int m_nWidthOfView          = 0;
        private int m_nHeightOfView         = 0;
        private byte[][] m_arBuffer         = null;

        private byte[] m_arCodeLeft         = null;
        private byte[] m_arCodeRight        = null;

        private int m_nCountOfDod           = 0;
        #endregion End - Variables

        public Form_IrisRecognition(bool bEnrollMode)
        {
            InitializeComponent();

            // 1. Initialize the variables.
            m_instanceSDK           = IrisysSDK.GetInstance();

            m_bEnrollMode           = bEnrollMode;

            m_nWidthOfView          = m_picturePreview.Width;
            m_nHeightOfView         = m_picturePreview.Height;

            m_timerForPreview.Tick      += CallByTimerToRecogize;
            m_timerForPreview.Interval  = INTERVAL_TIMER;
            m_timerForPreview.Start();
        }

        #region Internal Interface
        /// <summary>
        /// 2021.04.07 by yjlee [ADD] Update the preview to recognize.
        /// </summary>
        private bool UpdatePreview()
        {
            if(m_instanceSDK.GetPreview(m_nWidthOfView, m_nHeightOfView, ref m_arBuffer))
            {
                Bitmap bmp          = new Bitmap(m_nWidthOfView, m_nHeightOfView);

                var pBuffer         = m_arBuffer[0];

                for(int nCol = 0 ; nCol < m_nHeightOfView ; ++ nCol)
                {
                    for(int nRow = 0 ; nRow < m_nWidthOfView ; ++ nRow)
                    {
                        int nIndexOfPixel       = (nCol * m_nWidthOfView * 3) + nRow * 3;

                        bmp.SetPixel(nRow, nCol, Color.FromArgb(pBuffer[nIndexOfPixel], pBuffer[nIndexOfPixel + 1], pBuffer[nIndexOfPixel + 2]));
                    }
                }

                m_picturePreview.Image      = bmp;
                m_picturePreview.Invalidate();

                return true;
            }

            return false;
        }
        #endregion End - Internal Interface

        #region Timer
        /// <summary>
        /// 2021.04.07 by yjlee [ADD] Update the preview and enroll or authorize a user iris.
        /// </summary>
        private void CallByTimerToRecogize(object sender, EventArgs e)
        {
            string strMessage       = string.Empty;

            // 1. Action something to recognize the iris.
            switch(m_enStep)
            {
                case EN_IRIS_STEP.INIT_SDK:
                    strMessage      = MESSAGE_INIT_SDK;

                    if(m_instanceSDK.Init())
                    {
                        m_nCountOfDod   = 0;

                        m_enStep        = EN_IRIS_STEP.INIT_CAMERA;
                    }
                    break;

                case EN_IRIS_STEP.INIT_CAMERA:
                    strMessage      = MESSAGE_INIT_CAMERA;

                    if(m_instanceSDK.StartIrisRecognition())
                    {
                        m_nCountOfDod   = 0;

                        m_enStep        = m_bEnrollMode ? EN_IRIS_STEP.ENROLL_IRIS : EN_IRIS_STEP.AUTHO_IRIS;
                    }
                    break;

                case EN_IRIS_STEP.ENROLL_IRIS:
                    if(UpdatePreview())
                    {
                        if(m_instanceSDK.EnrollIris(ref m_arCodeLeft, ref m_arCodeRight))
                        {
                            m_enStep        = EN_IRIS_STEP.ENROLL_FINISH;
                        }
                        else
                        {
                            strMessage      = MESSAGE_ENROLL;
                        }
                    }
                    break;

                case EN_IRIS_STEP.ENROLL_FINISH:
                    strMessage      = MESSAGE_ENROLL_FINISH;
                    m_nCountOfDod   = 0;
                    break;

                case EN_IRIS_STEP.AUTHO_IRIS:
                    if (UpdatePreview())
                    {
                        if(m_instanceSDK.AuthorizeIris(ref m_arCodeLeft, ref m_arCodeRight))
                        {
                            m_enStep        = EN_IRIS_STEP.AUTHO_FINISH;
                        }
                        else
                        { 
                            strMessage      = MESSAGE_AUTHO;
                        }
                    }
                    break;

                case EN_IRIS_STEP.AUTHO_FINISH:
                    strMessage      = MESSAGE_AUTHO_FINISH;
                    m_nCountOfDod   = 0;
                    break;
            }

            // 2. Composite the message to display.
            for (int nDod = 0; nDod < m_nCountOfDod; ++nDod)
            {
                strMessage += MESSAGE_DOD;
            }

            m_nCountOfDod   = (m_nCountOfDod + 1) % MAX_COUNT_DOD;

            m_labelComment.Text     = strMessage;
        }
        #endregion End - Timer

        #region GUI Events
        /// <summary>
        /// 2021.04.07 by yjlee [ADD] Click the exit button to close this.
        /// </summary>
        private void Click_Exit(object sender, EventArgs e)
        {
            m_instanceSDK.Exit();

            Close();
        }
        #endregion End - GUI Events

        #region External Interface
        /// <summary>
        /// 2021.04.07 by yjlee [ADD] Get the iris code.
        /// </summary>
        public void GetIrisCode(ref byte[] arCodeLeft, ref byte[] arCodeRight)
        {
            if(null != m_arCodeLeft)
            {
                arCodeLeft      = new byte[m_arCodeLeft.Length];
                m_arCodeLeft.CopyTo(arCodeLeft, 0);
            }
            
            if(null != m_arCodeRight)
            {
                arCodeRight     = new byte[m_arCodeRight.Length];
                m_arCodeRight.CopyTo(arCodeRight, 0);
            }
        }
        #endregion End - External Interface
    }
}