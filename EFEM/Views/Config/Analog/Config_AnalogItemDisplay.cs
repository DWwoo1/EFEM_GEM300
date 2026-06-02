using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameOfSystem3.Views.Config.Analog
{
    public partial class Config_AnalogItemDisplay : UserControl
    {
        public DataGridView ItemDataGridView
        {
            get
            {
                return m_dgView;
            }
        }

        public Config_AnalogItemDisplay()
        {
            InitializeComponent();
        }
    }
}
