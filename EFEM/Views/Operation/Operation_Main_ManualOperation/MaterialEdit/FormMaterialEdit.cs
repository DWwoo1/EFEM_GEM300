using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Threading.Tasks;

using EFEM.Modules;
using EFEM.Defines.MaterialTracking;
using EFEM.CustomizedByProcessType.PWA500Common;
using EFEM.MaterialTracking;

namespace FrameOfSystem3.Views.Functional
{    
    public partial class FormMaterialEdit : Form
    {
        #region <Constructors>
        public FormMaterialEdit()
        {
            DoubleBuffered = true;

            InitializeComponent();

            //SubstratePropertis property = new SubstratePropertis();
            ////EFEM.MaterialTracking.Substrate substrate = new EFEM.MaterialTracking.Substrate("");
            //pgSubstrateAttribute.SelectedObject = property;
            _formKeyboard = Form_Keyboard.GetInstance();
            _selectionList = Form_SelectionList.GetInstance();
            _calculator = Form_Calculator.GetInstance();
            //SelectionListPort,
            //SelectionListSlot,
            //SelectionListTransferState,
            //SelectionListProcessingState,
            //SelectionListSubstrateType,
        }
        #endregion </Constructors>

        #region <Types>
        enum ItemType
        {
            Keyboard,
            CalculatorSlot,
            CalculatorPort,
            //SelectionListPort,
            SelectionListTransferState,
            SelectionListProcessingState,
            SelectionListIdReadingState,
            SelectionListSubstrateType,
        }
        #endregion </Types>

        #region <Fields>
        private bool _isFirstSelected = false;
        private bool _isMouseDownAtTitle = false;
        private Point _mouseDownPoint = new Point();

        private GridItem _selectedGridItem = null;
        private string _selectedItemValue = string.Empty;

        private static Form_Keyboard _formKeyboard = null;
        private static Form_SelectionList _selectionList = null;
        private static Form_Calculator _calculator = null;
        private bool _isHandlingSelectedGridItemChanged = false;
        #endregion </Fields>

        #region <Methods>

        #region <Internal>
        private void InitFlags()
        {
            _isFirstSelected = false;
            _selectedGridItem = null;
            _isHandlingSelectedGridItemChanged = false;
        }
        private void InitFocus()
        {
            if (pgSubstrateAttribute.SelectedGridItem?.Parent != null)
            {
                pgSubstrateAttribute.SelectedGridItem = pgSubstrateAttribute.SelectedGridItem.Parent;
            }
        }
        private void ProcessingEvent(Keys enInputedKey)
        {
            switch (enInputedKey)
            {
                case Keys.Enter:
                    InitFlags();
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    break;
                case Keys.Escape:
                    InitFlags();
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    break;
                default:
                    return;
            }

            this.Close();
        }
        private ItemType ClassifyItemTypeByName(string name)
        {
            switch (name)
            {
                // 여기까지가 기본
                //case BaseSubstrateAttributeKeys.Name:
                //case BaseSubstrateAttributeKeys.LotId:
                //case BaseSubstrateAttributeKeys.Location:
                //case BaseSubstrateAttributeKeys.ProcessJobId:
                //case BaseSubstrateAttributeKeys.ControlJobId:
                //case BaseSubstrateAttributeKeys.RecipeId:
                //case BaseSubstrateAttributeKeys.Usage:
                //case BaseSubstrateAttributeKeys.DoNotProcessFlag:
                //    itemType = ItemType.Keyboard;
                //    break;

                case BaseSubstrateAttributeKeys.SourcePortId:
                case BaseSubstrateAttributeKeys.DestinationPortId:
                    return ItemType.CalculatorPort;

                case BaseSubstrateAttributeKeys.SourceSlot:
                case BaseSubstrateAttributeKeys.DestinationSlot:
                    return ItemType.CalculatorSlot;

                case BaseSubstrateAttributeKeys.TransPortState:
                    return ItemType.SelectionListTransferState;

                case BaseSubstrateAttributeKeys.ProcessingState:
                    return ItemType.SelectionListProcessingState;

                case BaseSubstrateAttributeKeys.IdReadingState:
                    return ItemType.SelectionListIdReadingState;

                ///////////////////////////////////
                case EFEM.CustomizedByProcessType.PWA500Common.PWA500SubstrateAttributes.SubstrateType:
                    return ItemType.SelectionListSubstrateType;

                default:
                    return ItemType.Keyboard;
            }
        }
        private bool ShowDialogByClassifiedItemType(ItemType itemType, string propertyName, string oldValue, ref string newValue)
        {
            switch (itemType)
            {
                case ItemType.Keyboard:
                    {
                        if (false == _formKeyboard.CreateForm(oldValue))    //string.Format("Edit {0} State", propertyName)))
                            return false;

                        _formKeyboard.GetResult(ref newValue);
                        return true;
                    }

                case ItemType.CalculatorPort:
                    {
                        int lpCount = LoadPortManager.Instance.Count;
                        if (false == _calculator.CreateForm(oldValue, "1", lpCount.ToString(), "", "Edit Port"))
                            return false;

                        int result = 0;
                        _calculator.GetResult(ref result);
                        newValue = result.ToString();

                        //int lpCount = LoadPortManager.Instance.Count;
                        //List<string> carriernames = new List<string>();
                        //for (int i = 0; i < lpCount; ++i)
                        //{
                        //    int portId = LoadPortManager.Instance.GetLoadPortPortId(i);
                        //    if (CarrierManagementServer.Instance.HasCarrier(portId))
                        //        carriernames.Add(CarrierManagementServer.Instance.GetCarrierId(portId));
                        //}

                        //if (carriernames.Count > 0)
                        //{
                        //    if (false == _selectionList.CreateForm("Edit Carrier Port",
                        //                                carriernames.ToArray(), oldValue))
                        //        return false;

                        //    _selectionList.GetResult(ref newValue);
                        //}
                        //else
                        //{

                        //    if (false == _calculator.CreateForm(oldValue, "1", lpCount.ToString(), "", "Edit Port"))
                        //        return false;

                        //    int result = 0;
                        //    _calculator.GetResult(ref result);
                        //    newValue = result.ToString();
                        //}
                        return true;
                    }

                case ItemType.CalculatorSlot:
                    {
                        if (false == _calculator.CreateForm(oldValue, "0", "24", "", "Edit Slot"))
                            return false;

                        int result = 0;
                        _calculator.GetResult(ref result);
                        newValue = result.ToString();

                        return true;
                    }

                case ItemType.SelectionListTransferState:
                    {
                        if (false == _selectionList.CreateForm("Edit Transfer State",
                            Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.SUBSTRATE_TRANSFER_STATE, oldValue))
                            return false;

                        _selectionList.GetResult(ref newValue);
                        return true;
                    }

                case ItemType.SelectionListProcessingState:
                    {
                        if (false == _selectionList.CreateForm("Edit Processing State",
                            Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.SUBSTRATE_PROCESSING_STATE, oldValue))
                            return false;

                        _selectionList.GetResult(ref newValue);
                        return true;
                    }

                case ItemType.SelectionListIdReadingState:
                    {
                        if (false == _selectionList.CreateForm("Edit Id Reading State",
                            Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.SUBSTRATE_ID_READING_STATE, oldValue))
                            return false;

                        _selectionList.GetResult(ref newValue);
                        return true;
                    }

                case ItemType.SelectionListSubstrateType:
                    {
                        if (false == _selectionList.CreateForm("Edit Substrate Type",
                            Define.DefineEnumProject.SelectionList.EN_SELECTIONLIST.SUBSTRATE_TYPE, oldValue))
                            return false;

                        _selectionList.GetResult(ref newValue);
                        return true;
                    }

                default:
                    return false;
            }
        }
        #endregion </Internal>

        #region <External>
        public void GetResult(ref Dictionary<string, string> attributeResults)
        {
            attributeResults = new Dictionary<string, string>();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(pgSubstrateAttribute.SelectedObject);
            foreach (PropertyDescriptor prop in properties)
            {
                if (prop == null)
                    continue;
                object value = prop.GetValue(pgSubstrateAttribute.SelectedObject);
                attributeResults[prop.Name] = value.ToString();
            }
        }
        public bool CreateEditForm(Dictionary<string, string> targetAttributes)
        {
            if (targetAttributes == null)
                return false;

            Dictionary<string, string> dataToEdit = new Dictionary<string, string>(targetAttributes);
            pgSubstrateAttribute.SelectedObject = new DictionaryPropertyGridAdapter(dataToEdit);
            InitFocus();

            this.CenterToScreen();

            if (!this.Modal)
                this.ShowDialog();

            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                return true;
            }

            return false;
        }
        public void DisposeControls()
        {
            Dispose();
        }
        #endregion </External>

        #region <UI Event>
        private void BtnOkorCancelClicked(object sender, EventArgs e)
        {
            Control ctr = sender as Control;

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
        private void PgItemSelected(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (false == _isFirstSelected)
            {
                _isFirstSelected = true;
                return;
            }

            if (_selectedGridItem != null && e.NewSelection.PropertyDescriptor == null)
                return;

            _selectedGridItem = pgSubstrateAttribute.SelectedGridItem;
            if (_selectedGridItem == null)
                return;

            if (_isHandlingSelectedGridItemChanged)
                return;

            if (_selectedGridItem.PropertyDescriptor == null)
                return;

            try
            {
                _isHandlingSelectedGridItemChanged = true;

                _selectedItemValue = _selectedGridItem.PropertyDescriptor.GetValue(pgSubstrateAttribute.SelectedObject).ToString();

                string newValue = string.Empty;
                string itemName = _selectedGridItem.PropertyDescriptor.Name;
                ItemType itemType = ClassifyItemTypeByName(itemName);
                if (ShowDialogByClassifiedItemType(itemType, itemName, _selectedItemValue, ref newValue))
                {
                    _selectedGridItem.PropertyDescriptor.SetValue(pgSubstrateAttribute.SelectedObject, newValue);
                    pgSubstrateAttribute.Refresh();
                    InitFocus();
                }

            }
            finally
            {
                _isHandlingSelectedGridItemChanged = false;
            }
        }
        #endregion </UI Event>

        #endregion </Methods>
    }

    public class DictionaryPropertyGridAdapter : ICustomTypeDescriptor
    {
        private IDictionary _dictionary;

        public DictionaryPropertyGridAdapter(IDictionary dictionary)
        {
            _dictionary = dictionary;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptor[] properties = new PropertyDescriptor[_dictionary.Count];
            int i = 0;
            foreach (DictionaryEntry entry in _dictionary)
            {
                if (entry.Value == null)
                    continue;
                properties[i] = new DictionaryPropertyDescriptor(_dictionary, entry.Key);
                i++;
            }
            return new PropertyDescriptorCollection(properties);
        }

        #region ICustomTypeDescriptor 구현 (생략 가능한 메서드들)
        public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(this, true);
        public string GetClassName() => TypeDescriptor.GetClassName(this, true);
        public string GetComponentName() => TypeDescriptor.GetComponentName(this, true);
        public TypeConverter GetConverter() => TypeDescriptor.GetConverter(this, true);
        public EventDescriptor GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(this, true);
        public PropertyDescriptor GetDefaultProperty() => null;
        public object GetEditor(Type editorBaseType) => TypeDescriptor.GetEditor(this, editorBaseType, true);
        public EventDescriptorCollection GetEvents(Attribute[] attributes) => TypeDescriptor.GetEvents(this, attributes, true);
        public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents(this, true);
        public PropertyDescriptorCollection GetProperties() => GetProperties(new Attribute[0]);
        public object GetPropertyOwner(PropertyDescriptor pd) => this;
        #endregion
    }

    public class DictionaryPropertyDescriptor : PropertyDescriptor
    {
        private IDictionary _dictionary;
        private object _key;

        public DictionaryPropertyDescriptor(IDictionary dictionary, object key)
        : base(key.ToString(), new Attribute[] { new CategoryAttribute(GetCategory(key)), new ReadOnlyAttribute(true) })
        {
            _dictionary = dictionary;
            _key = key;
        }

        private static string GetCategory(object key)
        {
            string keyString = key.ToString();

            switch (keyString)
            {
                // 여기까지가 기본
                case BaseSubstrateAttributeKeys.UniqueKey:
                case BaseSubstrateAttributeKeys.Name:
                case BaseSubstrateAttributeKeys.LotId:
                case BaseSubstrateAttributeKeys.Location:
                    return "1.Basic";

                case BaseSubstrateAttributeKeys.SourcePortId:
                case BaseSubstrateAttributeKeys.SourceSlot:
                case BaseSubstrateAttributeKeys.DestinationPortId:
                case BaseSubstrateAttributeKeys.DestinationSlot:
                    return "2.Slots";

                case BaseSubstrateAttributeKeys.ProcessJobId:
                case BaseSubstrateAttributeKeys.ControlJobId:
                case BaseSubstrateAttributeKeys.RecipeId:
                    return "5.Jobs";

                case BaseSubstrateAttributeKeys.TransPortState:
                case BaseSubstrateAttributeKeys.ProcessingState:
                case BaseSubstrateAttributeKeys.IdReadingState:
                    return "3.Status";

                case BaseSubstrateAttributeKeys.Usage:
                case BaseSubstrateAttributeKeys.DoNotProcessFlag:
                    return "6.Usage";
                ///////////////////////////////////

                default:
                    return "4.ETC";
            }
        }
        public override Type PropertyType => _dictionary[_key].GetType();
        public override void SetValue(object component, object value) => _dictionary[_key] = value;
        public override object GetValue(object component)
        {
            string keyString = _key.ToString();

            switch (keyString)
            {
                case BaseSubstrateAttributeKeys.SourcePortId:
                case BaseSubstrateAttributeKeys.DestinationPortId:
                    {
                        if (false == int.TryParse(_dictionary[_key].ToString(), out int portId))
                            return string.Empty;

                        return portId;
                        //if (false == CarrierManagementServer.Instance.HasCarrier(portId))
                        //    return string.Empty;

                        //return CarrierManagementServer.Instance.GetCarrierId(portId);
                    }
                    
                //case BaseSubstrateAttributeKeys.TransPortState:
                //case BaseSubstrateAttributeKeys.ProcessingState:
                //case BaseSubstrateAttributeKeys.SourceSlot:
                //case BaseSubstrateAttributeKeys.DestinationSlot:
                //case EFEM.Defines.ProcessTypes.PWA500BIN.SubstrateType.:                   
                //case BaseSubstrateAttributeKeys.Name:
                //case BaseSubstrateAttributeKeys.LotId:
                //case BaseSubstrateAttributeKeys.Location:
                //case BaseSubstrateAttributeKeys.ProcessJobId:
                //case BaseSubstrateAttributeKeys.ControlJobId:
                //case BaseSubstrateAttributeKeys.RecipeId:
                //case BaseSubstrateAttributeKeys.Usage:
                //case BaseSubstrateAttributeKeys.DoNotProcessFlag:

                default:
                    return _dictionary[_key];
            }
        }
        public override bool IsReadOnly => true;
        public override Type ComponentType => null;
        public override bool CanResetValue(object component) => false;
        public override void ResetValue(object component) { }
        public override bool ShouldSerializeValue(object component) => true;
    }
}
