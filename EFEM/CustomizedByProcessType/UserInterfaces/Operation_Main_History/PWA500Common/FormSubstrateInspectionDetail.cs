using System;
using System.Drawing;
using System.Windows.Forms;

using EFEM.MaterialTracking;

namespace EFEM.CustomizedByProcessType.UserInterface.OperationMainHistory.PWA500Common
{
    /// <summary>
    /// 2026.07.10. jhlim [ADD] DB 조회 페이지 - 캐리어 상세의 "안착 기판" 리스트를 더블클릭했을 때 표시하는
    /// 읽기 전용 기판 상세 팝업.
    ///
    /// FormMaterialAttributeEdit 는 도메인 Substrate 객체가 아니라 문자열 딕셔너리 + IMaterialFieldLayoutProvider 를
    /// 받는 편집 폼(편집 가능 필드는 클릭 시 팝업으로 값을 바꾸고, 실제 저장은 호출측이 SubstrateManager.Set*ByKey +
    /// SaveDataByKey 로 수행)이라, DB 조회 DTO(SubstrateItem, 라이브 자재가 아닐 수 있음)를 그대로 넣기 부적합하고
    /// 필드가 편집 가능한 것처럼 보이면 "저장이 실제로 안 됨" 혼선이 생긴다. 그래서 이 페이지 전용의 단순
    /// 읽기 전용 폼을 새로 만든다(저장/편집 기능 없음). 표시 방식은 이 서브뷰의 기존 상세 그리드 패턴
    /// (Sys3DoubleBufferedDataGridView key-value)을 그대로 따른다.
    /// </summary>
    public sealed class FormSubstrateInspectionDetail : Form
    {
        #region <Constructors>
        public FormSubstrateInspectionDetail()
        {
            Text = "Substrate Detail (Read-Only)";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            ClientSize = new Size(560, 640);

            _gvInfo = CreateGrid();
            AddColumns(_gvInfo, "Item", "Value");
            _gvExtra = CreateGrid();
            AddColumns(_gvExtra, "Extra Key", "Value");

            var content = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            content.Controls.Add(WithHeader(_gvInfo, "Substrate Info"), 0, 0);
            content.Controls.Add(WithHeader(_gvExtra, "Extra"), 0, 1);

            var btnClose = CreateButton("Close", Color.LightGray, Color.Gray, (s, e) => Close());
            var buttonPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(6) };
            buttonPanel.Controls.Add(btnClose);

            var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            root.Controls.Add(content, 0, 0);
            root.Controls.Add(buttonPanel, 0, 1);

            Controls.Add(root);
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly DataGridView _gvInfo;
        private readonly DataGridView _gvExtra;
        #endregion </Fields>

        #region <Externals>
        /// <summary>기판 상세를 채우고 모달로 표시한다.</summary>
        public void ShowSubstrate(SubstrateItem item)
        {
            _gvInfo.Rows.Clear();
            _gvExtra.Rows.Clear();

            if (item != null)
            {
                AddKeyValue(_gvInfo, "UniqueKey", item.UniqueKey);
                AddKeyValue(_gvInfo, "Name", item.Name);
                AddKeyValue(_gvInfo, "Origin Name", item.OriginName);
                AddKeyValue(_gvInfo, "Location", item.LocationId);
                AddKeyValue(_gvInfo, "Source Port", item.SourcePortId.ToString());
                AddKeyValue(_gvInfo, "Source Slot", item.SourceSlot.ToString());
                AddKeyValue(_gvInfo, "Source Carrier", item.SourceCarrierId);
                AddKeyValue(_gvInfo, "Current Carrier Key", item.CurrentCarrierKey);
                AddKeyValue(_gvInfo, "Dest Port", item.DestinationPortId.ToString());
                AddKeyValue(_gvInfo, "Dest Slot", item.DestinationSlot.ToString());
                AddKeyValue(_gvInfo, "Lot ID", item.LotId);
                AddKeyValue(_gvInfo, "Recipe ID", item.RecipeId);
                AddKeyValue(_gvInfo, "Process Job", item.ProcessJobId);
                AddKeyValue(_gvInfo, "Control Job", item.ControlJobId);
                AddKeyValue(_gvInfo, "Transport Status", item.TransportStatus.ToString());
                AddKeyValue(_gvInfo, "Processing Status", item.ProcessingStatus.ToString());
                AddKeyValue(_gvInfo, "Id Reading Status", item.IdReadingStatus.ToString());
                AddKeyValue(_gvInfo, "DoNotProcess", item.DoNotProcessFlag.ToString());
                AddKeyValue(_gvInfo, "Usage", item.Usage.ToString());

                if (item.Extra != null)
                {
                    foreach (var kv in item.Extra)
                        _gvExtra.Rows.Add(kv.Key, kv.Value);
                }
            }

            ShowDialog();
        }
        #endregion </Externals>

        #region <Build helpers>
        private static void AddKeyValue(DataGridView grid, string key, string value)
        {
            grid.Rows.Add(key, value ?? string.Empty);
        }
        private static Control WithHeader(Control content, string header)
        {
            var panel = new Panel { Dock = DockStyle.Fill };
            var label = new Sys3Controls.Sys3Label();
            label.BackGroundColor = Color.LightSteelBlue;
            label.BorderStroke = 1;
            label.BorderStyle = ButtonBorderStyle.Solid;
            label.Description = "";
            label.DisabledColor = Color.DarkGray;
            label.Dock = DockStyle.Top;
            label.EdgeRadius = 1;
            label.Height = 24;
            label.LoadImage = null;
            label.MainFont = new Font("맑은 고딕", 9F, FontStyle.Bold);
            label.MainFontColor = Color.Black;
            label.SubText = "";
            label.Text = header;
            label.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_LEFT;
            label.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            label.TextAlignUnit = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            label.ThemeIndex = 0;
            label.UnitText = "";
            label.UseBorder = true;
            label.UseEdgeRadius = false;
            label.UseImage = false;
            label.UseSubFont = false;
            label.UseUnitFont = false;
            panel.Controls.Add(content);
            panel.Controls.Add(label);
            return panel;
        }
        private static DataGridView CreateGrid()
        {
            return new Sys3Controls.Sys3DoubleBufferedDataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                MultiSelect = false,
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                EditMode = DataGridViewEditMode.EditProgrammatically,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                AllowUserToResizeRows = false
            };
        }
        private static void AddColumns(DataGridView grid, params string[] headers)
        {
            foreach (var h in headers)
            {
                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = h,
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
            }
        }
        private static Sys3Controls.Sys3button CreateButton(string text, Color grad1, Color grad2, EventHandler onClick)
        {
            var b = new Sys3Controls.Sys3button();
            b.BorderWidth = 2;
            b.ButtonClicked = false;
            b.ClickedEmphasizeTextColor = Color.White;
            b.CustomClickedGradientFirstColor = Color.BlanchedAlmond;
            b.CustomClickedGradientSecondColor = Color.Gold;
            b.Description = "";
            b.DisabledColor = Color.Silver;
            b.Dock = DockStyle.Right;
            b.EdgeRadius = 5;
            b.GradientAngle = 60F;
            b.GradientFirstColor = grad1;
            b.GradientSecondColor = grad2;
            b.HoverEmphasizeCustomColor = Color.Firebrick;
            b.LoadImage = null;
            b.MainFont = new Font("맑은 고딕", 11F, FontStyle.Bold);
            b.MainFontColor = Color.Black;
            b.Size = new Size(120, 36);
            b.SubFont = new Font("맑은 고딕", 9F);
            b.SubFontColor = Color.Black;
            b.SubText = "";
            b.Text = text;
            b.TextAlignMain = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            b.TextAlignSub = Sys3Controls.EN_TEXTALIGN.MIDDLE_CENTER;
            b.ThemeIndex = 0;
            b.UseBorder = true;
            b.UseClickedEmphasizeTextColor = false;
            b.UseCustomizeClickedColor = true;
            b.UseEdge = true;
            b.UseHoverEmphasizeCustomColor = true;
            b.UseImage = false;
            b.UserHoverEmpahsize = true;
            b.UseSubFont = false;
            if (onClick != null)
                b.Click += onClick;
            return b;
        }
        #endregion </Build helpers>
    }
}
