using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing.Design;

namespace CodeTorch.Core
{
    public enum LabelRenderingMode
    {

        Left = 0,
        Top = 1,
        None = 2
    }

    [Serializable]
    [XmlInclude(typeof(ButtonControl))]
    [XmlInclude(typeof(CheckBoxControl))]
    [XmlInclude(typeof(DatePickerControl))]
    [XmlInclude(typeof(DropDownListControl))]
    [XmlInclude(typeof(EditorControl))]
    [XmlInclude(typeof(EmailAddressControl))]
    [XmlInclude(typeof(FileUploadControl))]
    [XmlInclude(typeof(GenericControl))]
    [XmlInclude(typeof(HyperLinkControl))]
    [XmlInclude(typeof(LabelControl))]
    [XmlInclude(typeof(ListBoxControl))]
    [XmlInclude(typeof(LookupDropDownListControl))]
    [XmlInclude(typeof(LookupListBoxControl))]
    [XmlInclude(typeof(MultiComboDropDownListControl))]
    [XmlInclude(typeof(NumericTextBoxControl))]
    [XmlInclude(typeof(PasswordControl))]
    [XmlInclude(typeof(PhotoPickerControl))]
    [XmlInclude(typeof(PickerControl))]
    [XmlInclude(typeof(SocialShareControl))]
    [XmlInclude(typeof(TextAreaControl))]
    [XmlInclude(typeof(TextBoxControl))]
    [XmlInclude(typeof(TreeViewControl))]
    [XmlInclude(typeof(WorkflowStatusControl))]
    public class BaseControl
    {
        private bool _Visible = true;
        private bool _LabelRendersBeforeControl = true;

        [Category("Common")]
        public string Name { get; set; }

        
        [Category("Style")]
        public string ControlCssClass { get; set; }

        [Category("Style")]
        public string ControlContainerElement { get; set; }

        [Category("Style")]
        public string ControlContainerCssClass { get; set; }

        [Category("Common")]
        public string Label { get; set; }

        [Category("Style")]
        public string LabelCssClass { get; set; }

        [Category("Style")]
        public bool LabelWrapsControl { get; set; }

        [Category("Style")]
        public bool LabelRendersBeforeControl
        {
            get
            {
                return _LabelRendersBeforeControl;
            }
            set
            {
                _LabelRendersBeforeControl = value;
            }
        }


        [Category("Style")]
        public string LabelContainerElement { get; set; }

        [Category("Style")]
        public string LabelContainerCssClass { get; set; }

        [Category("Style")]
        public string ControlGroupElement { get; set; }

        [Category("Style")]
        public string ControlGroupCssClass { get; set; }


        [Category("Common")]
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        public string HelpText { get; set; }

        [Category("Style")]
        public string HelpTextElement { get; set; }

        [Category("Style")]
        public string HelpTextCssClass { get; set; }

        [Category("Common")]
        [ReadOnly(true)]
        public virtual string Type { get; set; }

        [Category("Common")]
        public bool IsRequired { get; set; }

        [Category("Common")]
        public bool Visible 
        { 
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
            }
        }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataField { get; set; }



        [Category("Style")]
        public virtual string Width { get; set; }

        [Category("Style")]
        [Description("CSS Class attached to this control")]
        public string CssClass { get; set; }



        [Category("Style")]
        public string SkinID { get; set; }

        


        private List<BaseValidator> _Validators = new List<BaseValidator>();


        [XmlArray("Validators")]
        [XmlArrayItem(ElementName = "CompareValidator", Type = typeof(CompareValidator))]
        [XmlArrayItem(ElementName = "DataCommandValidator", Type = typeof(DataCommandValidator))]
        [XmlArrayItem(ElementName = "RangeValidator", Type = typeof(RangeValidator))]
        [XmlArrayItem(ElementName = "RegularExpressionValidator", Type = typeof(RegularExpressionValidator))]
        [Category("Validators")]
        [Description("List of control validators")]
        [Editor("CodeTorch.Core.Design.ValidatorCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]
        public virtual List<BaseValidator> Validators
        {
            get
            {
                return _Validators;
            }
            set
            {
                _Validators = value;
            }

        }

        

        [Browsable(false)]
        [XmlIgnore()]
        public object Parent { get; set; }


        public static BaseControl GetNewControl(string type)
        {
            BaseControl retval = null;

            switch (type.ToLower())
            {
                case "autocompletebox":
                    retval = new AutoCompleteBoxControl();
                    break;
                case "button":
                    retval = new ButtonControl();
                    break;
                case "checkbox":
                    retval = new CheckBoxControl();
                    break;
                case "datepicker":
                    retval = new DatePickerControl();
                    break;
                case "dropdownlist":
                    retval = new DropDownListControl();
                    break;
                case "emailaddress":
                    retval = new EmailAddressControl();
                    break;
                case "editor":
                    retval = new EditorControl();
                    break;
                case "fileupload":
                    retval = new FileUploadControl();
                    break;
                case "hyperlink":
                    retval = new HyperLinkControl();
                    break;
                case "label":
                    retval = new LabelControl();
                    break;
                case "listbox":
                    retval = new ListBoxControl();
                    break;
                case "lookupdropdownlist":
                    retval = new LookupDropDownListControl();
                    break;
                case "lookuplistbox":
                    retval = new LookupListBoxControl();
                    break;
                case "multicombodropdownlist":
                    retval = new MultiComboDropDownListControl();
                    break;
                case "numerictextbox":
                    retval = new NumericTextBoxControl();
                    break;
                case "password":
                    retval = new PasswordControl();
                    break;
                case "photopicker":
                    retval = new PhotoPickerControl();
                    break;
                case "picker":
                    retval = new PickerControl();
                    break;
                case "socialshare":
                    retval = new SocialShareControl();
                    break;
                case "textarea":
                    retval = new TextAreaControl();
                    break;
                case "textbox":
                    retval = new TextBoxControl();
                    break;
                case "treeview":
                    retval = new TreeViewControl();
                    break;
                case "workflowstatus":
                    retval = new WorkflowStatusControl();
                    break;
            }

            return retval;
        }

        public static object ConvertToSpecificControl(BaseControl control)
        {
            object retval = null;

          

            switch (control.Type.ToLower())
            {
                case "button":
                    retval = (ButtonControl) control;
                    break;
                case "checkbox":
                    retval = (CheckBoxControl) control;
                    break;
                case "datepicker":
                    retval = (DatePickerControl) control;
                    break;
                case "dropdownlist":
                    retval = (DropDownListControl) control;
                    break;
                case "emailaddress":
                    retval = (EmailAddressControl) control;
                    break;
                case "editor":
                    retval = (EditorControl) control;
                    break;
                case "fileupload":
                    retval = (FileUploadControl) control;
                    break;
                case "hyperlink":
                    retval = (HyperLinkControl) control;
                    break;
                case "label":
                    retval = (LabelControl) control;
                    break;
                case "listbox":
                    retval = (ListBoxControl) control;
                    break;
                case "lookupdropdownlist":
                    retval = (LookupDropDownListControl) control;
                    break;
                case "lookuplistbox":
                    retval = (LookupListBoxControl) control;
                    break;
                case "multicombodropdownlist":
                    retval = (MultiComboDropDownListControl) control;
                    break;
                case "numerictextbox":
                    retval = (NumericTextBoxControl) control;
                    break;
                case "password":
                    retval = (PasswordControl) control;
                    break;
                case "photopicker":
                    retval = (PhotoPickerControl) control;
                    break;
                case "picker":
                    retval = (PickerControl) control;
                    break;
                case "socialshare":
                    retval = (SocialShareControl) control;
                    break;
                case "textarea":
                    retval = (TextAreaControl) control;
                    break;
                case "textbox":
                    retval = (TextBoxControl) control;
                    break;
                case "treeview":
                    retval = (TreeViewControl) control;
                    break;
                case "workflowstatus":
                    retval = (WorkflowStatusControl) control;
                    break;
            }

            return retval;
        }

        PermissionCheck _VisiblePermission = new PermissionCheck();
        [Category("Security")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public PermissionCheck VisiblePermission
        {
            get { return _VisiblePermission; }
            set { _VisiblePermission = value; }
        }

        PermissionCheck _EditPermission = new PermissionCheck();
        [Category("Security")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public PermissionCheck EditPermission
        {
            get { return _EditPermission; }
            set { _EditPermission = value; }
        }


        [Category("Security")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string ReadOnlyDataField { get; set; }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, BaseControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add title
            AddResourceKey(retVal, Screen, Control, Prefix, "Label", Control.Label);
            //sub titles
            if (Control.IsRequired)
            { 
                AddResourceKey(retVal, Screen, Control, Prefix, "IsRequired.ErrorMessage", String.Format("{0} is required", Control.Label));
                
            }

            foreach (BaseValidator v in Control.Validators)
            {
                AddResourceKey(retVal, Screen, Control, Prefix, String.Format("Validators.{0}.ErrorMessage", v.Name), v.ErrorMessage);
            }
            

            //screens with action links
            //EmailAddress
            //HyperLink
            //Label
            //ListBox
            //LookupDropDownList
            //LookupListBox
            //MultiComboDropDownList
            //PickerControl
            //WorkflowStatus
            switch (Control.Type.ToLower())
            {
                case "dropdownlist":
                    retVal.AddRange(DropDownListControl.GetResourceKeys(Screen, ((DropDownListControl) Control), Prefix));
                    break;
                case "emailaddress":
                    retVal.AddRange(EmailAddressControl.GetResourceKeys(Screen, ((EmailAddressControl) Control), Prefix));
                    break;
                case "hyperlink":
                    retVal.AddRange(HyperLinkControl.GetResourceKeys(Screen, ((HyperLinkControl) Control), Prefix));
                    break;
                case "label":
                    retVal.AddRange(LabelControl.GetResourceKeys(Screen, ((LabelControl) Control), Prefix));
                    break;
                case "listbox":
                    retVal.AddRange(ListBoxControl.GetResourceKeys(Screen, ((ListBoxControl) Control), Prefix));
                    break;
                case "lookupdropdownlist":
                    retVal.AddRange(LookupDropDownListControl.GetResourceKeys(Screen, ((LookupDropDownListControl) Control), Prefix));
                    break;
                case "lookuplistbox":
                    retVal.AddRange(LookupListBoxControl.GetResourceKeys(Screen, ((LookupListBoxControl) Control), Prefix));
                    break;
                case "multicombodropdownlist":
                    retVal.AddRange(MultiComboDropDownListControl.GetResourceKeys(Screen, ((MultiComboDropDownListControl) Control), Prefix));
                    break;
                case "pickercontrol":
                    retVal.AddRange(PickerControl.GetResourceKeys(Screen, ((PickerControl) Control), Prefix));
                    break;
                case "treeview":
                    retVal.AddRange(TreeViewControl.GetResourceKeys(Screen, ((TreeViewControl)Control), Prefix));
                    break;
                case "workflowstatus":
                    retVal.AddRange(WorkflowStatusControl.GetResourceKeys(Screen, ((WorkflowStatusControl) Control), Prefix));
                    break;
            }

            return retVal;
        }

        protected static void AddResourceKey(List<ResourceItem> keys, Screen screen, BaseControl control, string Prefix, string ResourceKey, string DefaultValue)
        {
            ResourceItem key = new Core.ResourceItem();

            key.ResourceSet = String.Format("App/{0}/{1}", screen.Folder, screen.Name);
            if (String.IsNullOrEmpty(Prefix))
            {
                key.Key = String.Format("Control.{0}.{1}",  control.Name, ResourceKey);
            }
            else
            {
                key.Key = String.Format("{0}.Control.{1}.{2}", Prefix, control.Name, ResourceKey);
            }
            key.Value = DefaultValue;

            if (!String.IsNullOrEmpty(DefaultValue))
            {
                keys.Add(key);
            }
        }
    }
}
