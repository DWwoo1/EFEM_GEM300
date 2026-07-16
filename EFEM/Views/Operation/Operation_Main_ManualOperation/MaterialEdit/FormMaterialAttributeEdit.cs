using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Sys3Controls;

using EFEM.Modules;

namespace FrameOfSystem3.Views.Functional
{
    /// <summary>
    /// 자재(Substrate/Carrier) 속성 편집 폼. PropertyGrid 대신 Sys3Label 쌍(이름/값)을 런타임에 자동 배치한다.
    /// 카테고리는 접기/펼치기 가능(FlowLayoutPanel 자동 리플로우), 상단 검색으로 속성명 필터링을 지원한다.
    /// 표시할 필드/순서/카테고리/편집기는 호출부가 주입하는 <see cref="IMaterialFieldLayoutProvider"/> 가 결정한다.
    /// 입출력 계약: CreateEditForm(dict, provider) / GetResult(ref dict).
    /// </summary>
    public partial class FormMaterialAttributeEdit : Form
    {
        #region <Constructors>
        public FormMaterialAttributeEdit()
        {
            DoubleBuffered = true;

            InitializeComponent();

            _formKeyboard = Form_Keyboard.GetInstance();
            _selectionList = Form_SelectionList.GetInstance();
            _calculator = Form_Calculator.GetInstance();
        }
        #endregion </Constructors>

        #region <Fields>
        private bool _isMouseDownAtTitle = false;
        private Point _mouseDownPoint = new Point();

        private readonly Form_Keyboard _formKeyboard = null;
        private readonly Form_SelectionList _selectionList = null;
        private readonly Form_Calculator _calculator = null;

        // 편집 대상 작업 복사본. 화면 편집 시 즉시 갱신되고 GetResult 로 그대로 반환된다.
        private Dictionary<string, string> _working = new Dictionary<string, string>();

        // 필드 레이아웃 공급자(호출부 주입) — Substrate/Carrier 등 도메인 결정.
        private IMaterialFieldLayoutProvider _layoutProvider = null;

        // 카테고리 섹션 및 값 라벨 → 필드 매핑
        private readonly List<CategorySection> _sections = new List<CategorySection>();
        private readonly Dictionary<Sys3Label, CategorySection> _headerToSection = new Dictionary<Sys3Label, CategorySection>();
        private readonly Dictionary<Sys3Label, MaterialFieldDescriptor> _valueFields = new Dictionary<Sys3Label, MaterialFieldDescriptor>();

        private string _keyword = string.Empty;
        private bool _isEditingValue = false;
        #endregion </Fields>

        #region <Layout Constants>
        private const int FlowInnerWidth = 500;   // pnFields(532) - 세로 스크롤바/여백
        private const int RowHeight = 34;
        private const int HeaderHeight = 28;
        private const int NameWidth = 170;
        private const int Gap = 3;
        private static readonly int ValueWidthInRow = FlowInnerWidth - NameWidth - Gap;

        private static readonly Font NameFont = new Font("맑은 고딕", 9F, FontStyle.Bold);
        private static readonly Font ValueFont = new Font("맑은 고딕", 10F, FontStyle.Bold);
        private static readonly Font HeaderFont = new Font("맑은 고딕", 10F, FontStyle.Bold);

        private static readonly Color HeaderBackColor = Color.FromArgb(180, 190, 205);
        private static readonly Color NameBackColor = Color.Gainsboro;
        private static readonly Color ValueBackColor = Color.White;
        private static readonly Color ReadOnlyBackColor = Color.Silver;

        private const string ArrowExpanded = "▼ ";
        private const string ArrowCollapsed = "▶ ";
        private const string KeywordPlaceholder = "(전체)";
        #endregion </Layout Constants>

        #region <External>
        /// <summary>
        /// 편집 대상 속성 맵과 필드 레이아웃 공급자를 받아 폼을 구성하고 모달로 띄운다. OK 로 닫히면 true.
        /// </summary>
        public bool CreateEditForm(Dictionary<string, string> targetAttributes, IMaterialFieldLayoutProvider layoutProvider)
        {
            if (targetAttributes == null || layoutProvider == null)
                return false;

            _working = new Dictionary<string, string>(targetAttributes);
            _layoutProvider = layoutProvider;

            BuildSections();

            this.CenterToScreen();

            if (!this.Modal)
                this.ShowDialog();

            return this.DialogResult == DialogResult.OK;
        }

        /// <summary>
        /// 편집 결과를 반환한다. Provider 가 다루지 않은 키도 작업 복사본에 그대로 남아 손실되지 않는다.
        /// </summary>
        public void GetResult(ref Dictionary<string, string> attributeResults)
        {
            attributeResults = new Dictionary<string, string>(_working);
        }

        public void DisposeControls()
        {
            Dispose();
        }
        #endregion </External>

        #region <Build / Destroy>
        private void BuildSections()
        {
            DestroyRows();

            IReadOnlyList<MaterialFieldDescriptor> fields = _layoutProvider.GetFields();

            pnFields.SuspendLayout();

            // 카테고리(최초 등장 순서)별로 섹션을 만든다.
            var sectionByCategory = new Dictionary<string, CategorySection>(StringComparer.Ordinal);

            foreach (var field in fields)
            {
                if (field == null || string.IsNullOrEmpty(field.Key))
                    continue;

                string category = string.IsNullOrEmpty(field.Category) ? SubstrateFieldLayoutCommon.CategoryEtc : field.Category;

                if (false == sectionByCategory.TryGetValue(category, out var section))
                {
                    section = CreateSection(category);
                    sectionByCategory[category] = section;
                    _sections.Add(section);
                }

                string value = _working.TryGetValue(field.Key, out var v) ? (v ?? string.Empty) : string.Empty;
                AddFieldRow(section, field, value);
            }

            // 초기 상태: 전체 펼침
            foreach (var section in _sections)
            {
                ApplyVisibility(section);
            }

            pnFields.ResumeLayout(true);
        }

        private CategorySection CreateSection(string category)
        {
            var header = new Sys3Label
            {
                MainFont = HeaderFont,
                MainFontColor = Color.Black,
                TextAlignMain = EN_TEXTALIGN.MIDDLE_LEFT,
                BackGroundColor = HeaderBackColor,
                UseBorder = true,
                BorderStroke = 1,
                BorderStyle = ButtonBorderStyle.Solid,
                Description = "",
                UseImage = false,
                UseSubFont = false,
                Margin = new Padding(0, 2, 0, 0),
                Size = new Size(FlowInnerWidth, HeaderHeight),
            };

            var body = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.WhiteSmoke,
                Margin = new Padding(0, 0, 0, 4),
                Padding = new Padding(0),
            };

            var section = new CategorySection(category, header, body);

            header.Click += HeaderClicked;
            _headerToSection[header] = section;

            pnFields.Controls.Add(header);
            pnFields.Controls.Add(body);

            return section;
        }

        private void AddFieldRow(CategorySection section, MaterialFieldDescriptor field, string value)
        {
            var row = new Panel
            {
                Size = new Size(FlowInnerWidth, RowHeight),
                Margin = new Padding(0, 0, 0, 2),
                BackColor = Color.WhiteSmoke,
            };

            var nameLabel = new Sys3Label
            {
                Text = field.DisplayName,
                MainFont = NameFont,
                MainFontColor = Color.Black,
                TextAlignMain = EN_TEXTALIGN.MIDDLE_LEFT,
                BackGroundColor = NameBackColor,
                UseBorder = true,
                BorderStroke = 1,
                BorderStyle = ButtonBorderStyle.Solid,
                Description = "",
                UseImage = false,
                UseSubFont = false,
                Location = new Point(0, 0),
                Size = new Size(NameWidth, RowHeight),
            };

            bool isReadOnly = field.Editor == MaterialFieldEditorKind.ReadOnly;
            var valueLabel = new Sys3Label
            {
                Text = DisplayTextFor(field, value),
                MainFont = ValueFont,
                MainFontColor = isReadOnly ? Color.DimGray : Color.Black,
                TextAlignMain = EN_TEXTALIGN.MIDDLE_LEFT,
                BackGroundColor = isReadOnly ? ReadOnlyBackColor : ValueBackColor,
                UseBorder = true,
                BorderStroke = 1,
                BorderStyle = ButtonBorderStyle.Solid,
                Description = "",
                UseImage = false,
                UseSubFont = false,
                Location = new Point(NameWidth + Gap, 0),
                Size = new Size(ValueWidthInRow, RowHeight),
            };

            row.Controls.Add(nameLabel);
            row.Controls.Add(valueLabel);
            section.Body.Controls.Add(row);

            section.Rows.Add(new FieldRow(row, field));

            if (false == isReadOnly)
            {
                _valueFields[valueLabel] = field;
                valueLabel.Click += ValueLabelClicked;
            }
        }

        private void DestroyRows()
        {
            foreach (var pair in _valueFields)
                pair.Key.Click -= ValueLabelClicked;
            _valueFields.Clear();

            foreach (var pair in _headerToSection)
                pair.Key.Click -= HeaderClicked;
            _headerToSection.Clear();

            foreach (var section in _sections)
            {
                if (pnFields != null)
                {
                    pnFields.Controls.Remove(section.Header);
                    pnFields.Controls.Remove(section.Body);
                }
                section.Dispose();
            }
            _sections.Clear();
        }
        #endregion </Build / Destroy>

        #region <Collapse / Filter>
        private void HeaderClicked(object sender, EventArgs e)
        {
            if (!(sender is Sys3Label header))
                return;

            if (false == _headerToSection.TryGetValue(header, out var section))
                return;

            section.Expanded = !section.Expanded;
            pnFields.SuspendLayout();
            ApplyVisibility(section);
            pnFields.ResumeLayout(true);
        }

        private void ApplyFilterAll()
        {
            pnFields.SuspendLayout();
            foreach (var section in _sections)
                ApplyVisibility(section);
            pnFields.ResumeLayout(true);
        }

        /// <summary>
        /// 현재 검색어(_keyword)와 섹션의 펼침 상태에 맞춰 헤더/본문/행의 표시 여부를 갱신한다.
        /// 검색어가 있으면 매칭 섹션은 강제로 펼치고, 비매칭 섹션은 통째로 숨긴다.
        /// </summary>
        private void ApplyVisibility(CategorySection section)
        {
            bool filterActive = false == string.IsNullOrEmpty(_keyword);
            int visibleRows = 0;

            foreach (var fieldRow in section.Rows)
            {
                bool visible = false == filterActive || IsMatch(fieldRow.Field, _keyword);
                fieldRow.Row.Visible = visible;
                if (visible)
                    visibleRows++;
            }

            bool anyVisible = visibleRows > 0;

            if (filterActive)
            {
                section.Header.Visible = anyVisible;
                section.Body.Visible = anyVisible;    // 매칭 섹션은 강제 펼침
            }
            else
            {
                section.Header.Visible = true;
                section.Body.Visible = section.Expanded;
            }

            bool showAsExpanded = filterActive ? anyVisible : section.Expanded;
            section.Header.Text = (showAsExpanded ? ArrowExpanded : ArrowCollapsed) + section.Category;
        }

        private static bool IsMatch(MaterialFieldDescriptor field, string keyword)
        {
            if (field.DisplayName != null && field.DisplayName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            if (field.Key != null && field.Key.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            return false;
        }

        // lblKeyword 클릭 → 검색어 입력(키보드) 후 즉시 필터 적용.
        private void EditKeywordClicked(object sender, EventArgs e)
        {
            if (_isEditingValue)
                return;

            try
            {
                _isEditingValue = true;

                if (false == _formKeyboard.CreateForm(_keyword))
                    return;

                string result = string.Empty;
                _formKeyboard.GetResult(ref result);
                _keyword = result == null ? string.Empty : result.Trim();
                UpdateKeywordLabel();
                ApplyFilterAll();   // 입력 즉시 필터 적용
            }
            finally
            {
                _isEditingValue = false;
            }
        }

        // btnSearch 클릭 → 현재 입력된 검색어로 필터 재실행(재적용).
        private void BtnSearchClicked(object sender, EventArgs e)
        {
            ApplyFilterAll();
        }

        // 전체 펼침: 필터 해제 후 모든 카테고리를 펼친다.
        private void BtnExpandAllClicked(object sender, EventArgs e)
        {
            SetAllExpanded(true);
        }

        // 전체 접기: 필터 해제 후 모든 카테고리를 접는다.
        private void BtnCollapseAllClicked(object sender, EventArgs e)
        {
            SetAllExpanded(false);
        }

        private void SetAllExpanded(bool expanded)
        {
            _keyword = string.Empty;
            UpdateKeywordLabel();

            foreach (var section in _sections)
                section.Expanded = expanded;

            ApplyFilterAll();
        }

        private void UpdateKeywordLabel()
        {
            lblKeyword.Text = string.IsNullOrEmpty(_keyword) ? KeywordPlaceholder : _keyword;
            lblKeyword.MainFontColor = string.IsNullOrEmpty(_keyword) ? Color.Gray : Color.Black;
        }
        #endregion </Collapse / Filter>

        #region <Value Editing>
        private void ValueLabelClicked(object sender, EventArgs e)
        {
            if (_isEditingValue)
                return;

            if (!(sender is Sys3Label valueLabel))
                return;

            if (false == _valueFields.TryGetValue(valueLabel, out var field))
                return;

            try
            {
                _isEditingValue = true;

                // 편집기에는 "저장된 값"(int 모드면 정수 문자열)을 넘겨야 preselect/파싱이 맞는다.
                string oldValue = _working.TryGetValue(field.Key, out var stored) ? (stored ?? string.Empty) : string.Empty;
                if (EditValueByKind(field, oldValue, out var newValue))
                {
                    _working[field.Key] = newValue;                       // 저장은 원본 값(int 모드면 int)
                    valueLabel.Text = DisplayTextFor(field, newValue);    // 표시는 이름으로 매핑
                }
            }
            finally
            {
                _isEditingValue = false;
            }
        }

        /// <summary>
        /// 라벨에 표시할 텍스트를 만든다. int 모드(UseSelectionValue) SelectionList 는 저장값(정수)을
        /// 짝지어진 항목 이름으로 매핑해 보여준다. 그 외에는 저장값 그대로 표시한다.
        /// </summary>
        private static string DisplayTextFor(MaterialFieldDescriptor field, string storedValue)
        {
            if (field.Editor == MaterialFieldEditorKind.SelectionList
                && field.UseSelectionValue
                && field.SelectionItems != null
                && field.SelectionIndices != null
                && field.SelectionItems.Length == field.SelectionIndices.Length
                && int.TryParse(storedValue, out int v))
            {
                for (int i = 0; i < field.SelectionIndices.Length; ++i)
                {
                    if (field.SelectionIndices[i] == v)
                        return field.SelectionItems[i];
                }
            }

            return storedValue ?? string.Empty;
        }

        private bool EditValueByKind(MaterialFieldDescriptor field, string oldValue, out string newValue)
        {
            newValue = oldValue;

            switch (field.Editor)
            {
                case MaterialFieldEditorKind.Keyboard:
                    {
                        if (false == _formKeyboard.CreateForm(oldValue))
                            return false;

                        _formKeyboard.GetResult(ref newValue);
                        return true;
                    }

                case MaterialFieldEditorKind.CalculatorPort:
                    {
                        int max = field.CalcMax > 0 ? field.CalcMax : LoadPortManager.Instance.Count;
                        int min = field.CalcMin > 0 ? field.CalcMin : 1;
                        if (false == _calculator.CreateForm(oldValue, min.ToString(), max.ToString(), "", "Edit Port"))
                            return false;

                        int result = 0;
                        _calculator.GetResult(ref result);
                        newValue = result.ToString();
                        return true;
                    }

                case MaterialFieldEditorKind.CalculatorSlot:
                    {
                        int max = field.CalcMax > 0 ? field.CalcMax : 24;
                        int min = field.CalcMin;
                        if (false == _calculator.CreateForm(oldValue, min.ToString(), max.ToString(), "", "Edit Slot"))
                            return false;

                        int result = 0;
                        _calculator.GetResult(ref result);
                        newValue = result.ToString();
                        return true;
                    }

                case MaterialFieldEditorKind.SelectionList:
                    {
                        string title = string.Format("Edit {0}", field.DisplayName);

                        // 배열이 지정되면 EN_SELECTIONLIST 등록 없이 즉석 배열 오버로드를 사용한다.
                        if (field.SelectionItems != null && field.SelectionItems.Length > 0)
                        {
                            // (A) int 값 저장 모드: 항목=표시 텍스트, 저장=짝지어진 정수값(예: ((int)enum).ToString()).
                            if (field.UseSelectionValue &&
                                field.SelectionIndices != null &&
                                field.SelectionIndices.Length == field.SelectionItems.Length)
                            {
                                if (false == int.TryParse(oldValue, out int preInt))
                                    preInt = -1;

                                if (false == _selectionList.CreateForm(title, field.SelectionItems, field.SelectionIndices, preInt))
                                    return false;

                                int selectedValue = 0;
                                _selectionList.GetResult(ref selectedValue);
                                newValue = selectedValue.ToString();
                                return true;
                            }

                            // (B) 문자열 저장 모드: 선택한 항목 텍스트 자체가 저장값.
                            int[] indices = field.SelectionIndices;
                            if (indices == null || indices.Length != field.SelectionItems.Length)
                            {
                                indices = new int[field.SelectionItems.Length];
                                for (int i = 0; i < indices.Length; ++i)
                                    indices[i] = i;
                            }

                            string[] preValue = new[] { oldValue };
                            if (false == _selectionList.CreateForm(title, field.SelectionItems, indices, preValue))
                                return false;

                            _selectionList.GetResult(ref newValue);
                            return true;
                        }

                        // (C) EN_SELECTIONLIST 레지스트리 폴백(문자열).
                        if (false == _selectionList.CreateForm(title, field.SelectionListType, oldValue))
                            return false;

                        _selectionList.GetResult(ref newValue);
                        return true;
                    }

                default:
                    return false;
            }
        }
        #endregion </Value Editing>

        #region <UI Event>
        // Ctrl+F → 검색 키워드 입력(검색 라벨 클릭과 동일), Esc → 저장 없이 닫기.
        // 자식 컨트롤 포커스와 무관하게 잡기 위해 ProcessCmdKey 에서 처리한다.
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_isEditingValue)
                return base.ProcessCmdKey(ref msg, keyData);

            if (keyData == (Keys.Control | Keys.F))
            {
                EditKeywordClicked(this, EventArgs.Empty);
                return true;
            }

            if (keyData == Keys.Escape)
            {
                // 1단계: 검색어가 있으면 먼저 해제 + 전체 펼치기
                if (false == string.IsNullOrEmpty(_keyword))
                {
                    SetAllExpanded(true);
                    return true;
                }

                // 2단계: 검색어가 없으면 저장 없이 닫기
                ProcessingEvent(Keys.Escape);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void ProcessingEvent(Keys enInputedKey)
        {
            switch (enInputedKey)
            {
                case Keys.Enter:
                    DialogResult = DialogResult.OK;
                    break;
                case Keys.Escape:
                    DialogResult = DialogResult.Cancel;
                    break;
                default:
                    return;
            }

            this.Close();
        }

        private void BtnOkorCancelClicked(object sender, EventArgs e)
        {
            Control ctr = sender as Control;
            if (ctr == null)
                return;

            switch (ctr.TabIndex)
            {
                case 0: // OK
                    ProcessingEvent(Keys.Enter);
                    break;
                case 1: // CANCEL
                    ProcessingEvent(Keys.Escape);
                    break;
            }
        }

        private void MouseDown_Title(object sender, MouseEventArgs e)
        {
            _isMouseDownAtTitle = true;
            _mouseDownPoint = e.Location;
        }
        private void MouseMove_Title(object sender, MouseEventArgs e)
        {
            if (_isMouseDownAtTitle)
            {
                this.SetDesktopLocation(MousePosition.X - _mouseDownPoint.X, MousePosition.Y - _mouseDownPoint.Y);
            }
        }
        private void MouseUp_Title(object sender, MouseEventArgs e)
        {
            _isMouseDownAtTitle = false;
        }
        #endregion </UI Event>

        #region <Nested Types>
        /// <summary>런타임 생성된 한 개 필드 행(패널 + 서술자).</summary>
        private sealed class FieldRow
        {
            public FieldRow(Panel row, MaterialFieldDescriptor field)
            {
                Row = row;
                Field = field;
            }
            public Panel Row { get; }
            public MaterialFieldDescriptor Field { get; }
        }

        /// <summary>접기/펼치기 가능한 한 개 카테고리 섹션(헤더 + 본문 + 행 목록).</summary>
        private sealed class CategorySection
        {
            public CategorySection(string category, Sys3Label header, FlowLayoutPanel body)
            {
                Category = category;
                Header = header;
                Body = body;
                Expanded = true;
                Rows = new List<FieldRow>();
            }

            public string Category { get; }
            public Sys3Label Header { get; }
            public FlowLayoutPanel Body { get; }
            public bool Expanded { get; set; }
            public List<FieldRow> Rows { get; }

            public void Dispose()
            {
                foreach (var fieldRow in Rows)
                {
                    Body.Controls.Remove(fieldRow.Row);
                    fieldRow.Row.Dispose();   // 자식 라벨도 함께 해제
                }
                Rows.Clear();

                Header.Dispose();
                Body.Dispose();
            }
        }
        #endregion </Nested Types>
    }
}
