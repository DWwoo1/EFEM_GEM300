using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EFEM.Modules;
using EFEM.Defines.LoadPort;

namespace FrameOfSystem3.Views.Operation.SubPanelSummary.LoadPortSummary
{
    public partial class SummaryLoadPortState_PIO : ParameterPanel
    {
        #region <Constructors>
        public SummaryLoadPortState_PIO(int lpIndex)
        {
            InitializeComponent();
            
            MyIndex = lpIndex;

            _loadPortManager = LoadPortManager.Instance;

            _inputValues = new Dictionary<int, bool>();
            _outputValues = new Dictionary<int, bool>();
            _inputIndex = new Dictionary<int, int>();
            _outputIndex = new Dictionary<int, int>();
            InputControls = new List<Sys3Controls.Sys3LedLabel>();
            OutputControls = new List<Sys3Controls.Sys3LedLabel>();

            MappingSignalAndControls();

            this.Dock = DockStyle.Fill;
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly int MyIndex;

        private Dictionary<int, int> _inputIndex = null;        // Key : Address    Value : 순번
        private Dictionary<int, bool> _inputValues = null;      // Key : Address    Value : Value
        private Dictionary<int, int> _outputIndex = null;       // Key : Address    Value : 순번
        private Dictionary<int, bool> _outputValues = null;     // Key : Address    Value : Value
        private readonly List<Sys3Controls.Sys3LedLabel> InputControls = null;
        private readonly List<Sys3Controls.Sys3LedLabel> OutputControls = null;
        private AMHSInformation _amhsInformation = null;
        private static LoadPortManager _loadPortManager = null;
        private const int ParallelIOCount = 8;
        private const int DitigalInputRowIndex = 0;
        private const int DitigalOutputRowIndex = 1;
        #endregion </Fields>

        #region <Properties>
        #endregion </Properties>

        #region <Methods>

        #region <Override Methods>
        protected override void ProcessWhenActivation()
        {
            UpdateSignals();

            base.ProcessWhenActivation();
        }

        protected override void ProcessWhenDeactivation()
        {
            base.ProcessWhenDeactivation();
        }

        public override void CallFunctionByTimer()
        {
            UpdateParallelSignalValues();

            DisplayParallelSignalControls();

            base.CallFunctionByTimer();
        }
        #endregion </Override Methods>

        #region <Display Methods>
        private void MappingSignalAndControls()
        {
            int rowCount = pnLayout.RowCount;
            for (int i = 0; i < rowCount; ++i)
            {
                if (!(pnLayout.GetControlFromPosition(DitigalInputRowIndex, i) is Sys3Controls.Sys3LedLabel control))
                    continue;

                InputControls.Add(control);
            }

            for (int i = 0; i < rowCount; ++i)
            {
                if (!(pnLayout.GetControlFromPosition(DitigalOutputRowIndex, i) is Sys3Controls.Sys3LedLabel control))
                    continue;

                OutputControls.Add(control);
            }
        }
        private void UpdateSignals()
        {
            if (false == _loadPortManager.GetAMHSInformation(MyIndex, ref _amhsInformation))
                return;

            for (int i = 0; i < ParallelIOCount; ++i)
            {
                int inputIndex = _amhsInformation.DigitalInputs[i].Item1;
                int outputIndex = _amhsInformation.DigitalOutputs[i].Item1;

                _inputIndex[inputIndex] = i;
                _outputIndex[outputIndex] = i;

                _inputValues[inputIndex] = false;
                _outputValues[outputIndex] = false;
            }
        }
        private void UpdateParallelSignalValues()
        {
            _loadPortManager.GetAMHSSignalValues(MyIndex, ref _inputValues, ref _outputValues);
        }
        private void DisplayParallelSignalControls()
        {
            if (_inputValues == null)
            {
                for (int i = 0; i < InputControls.Count; ++i)
                {
                    InputControls[i].Active = false;
                }
            }
            else
            {
                foreach (var item in _inputValues)
                {
                    if (false == _inputIndex.TryGetValue(item.Key, out int index))
                        continue;

                    if (index < 0 || index >= InputControls.Count)
                        continue;

                    InputControls[index].Active = item.Value;
                }
            }

            if (_outputValues == null)
            {
                for (int i = 0; i < OutputControls.Count; ++i)
                {
                    OutputControls[i].Active = false;
                }
            }
            else
            {
                foreach (var item in _outputValues)
                {
                    if (false == _outputIndex.TryGetValue(item.Key, out int index))
                        continue;

                    if (index < 0 || index >= OutputControls.Count)
                        continue;

                    OutputControls[index].Active = item.Value;
                }
            }
        }
        #endregion </Display Methods>

        #endregion </Methods>
    }
}