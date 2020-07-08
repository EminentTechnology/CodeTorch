using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;


namespace CodeTorch.Core
{
   

    [Serializable]
    public class Widget
    {
        [Category("Common")]
        public string Name { get; set; }

        [Category("Common")]
        public bool RenderLabel { get; set; } = true;


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
        public bool LabelRendersBeforeControl { get; set; } = true;


        [Category("Style")]
        public string LabelContainerElement { get; set; }

        [Category("Style")]
        public string LabelContainerCssClass { get; set; }

        [Category("Style")]
        public string ControlGroupElement { get; set; }

        [Category("Style")]
        public string ControlGroupCssClass { get; set; }


        [Category("Common")]
#if NETFRAMEWORK
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
#endif
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
        public bool Visible { get; set; } = true;

        [Category("Data")]
#if NETFRAMEWORK
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
#endif
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
#if NETFRAMEWORK
        [Editor("CodeTorch.Core.Design.ValidatorCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]
#endif
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


        public static Widget GetNewControl(string type)
        {
            ControlType widgetType = ControlType.GetControlType(type);
            string assemblyQualifiedName = String.Format("{0}, {1}", widgetType.AbstractionClass, widgetType.AbstractionAssembly);

            Type t = System.Type.GetType(assemblyQualifiedName, false, true);
            Widget retval = (Widget)Activator.CreateInstance(t);

            

            return retval;
        }

        public static object ConvertToSpecificControl(Widget control)
        {
            object retVal = null;

            ControlType widgetType = ControlType.GetControlType(control.GetType().Name);
            string assemblyQualifiedName = String.Format("{0}, {1}", widgetType.AbstractionClass, widgetType.AbstractionAssembly);

            Type t = System.Type.GetType(assemblyQualifiedName, false, true);
            retVal = Convert.ChangeType(control, t);



            return retVal;
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

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, Widget Control, String Prefix)
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

        protected static void AddResourceKey(List<ResourceItem> keys, Screen screen, Widget control, string Prefix, string ResourceKey, string DefaultValue)
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
