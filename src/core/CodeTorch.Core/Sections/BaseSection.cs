using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing.Design;


namespace CodeTorch.Core
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class BaseSection
    {
        private bool _Visible = true;
        PermissionCheck _Permission = new PermissionCheck();
        SectionContainer _ContainerMode = SectionContainer.Panel;

        [Category("Common")]
        public virtual string ID { get; set; }

        [Category("Common")]
        public virtual string Name { get; set; }

        [Category("Common")]
        public virtual string IntroText { get; set; }

        [Category("Common")]
        public virtual string CssClass { get; set; }

        [Category("Common")]
        [ReadOnly(true)]
        public virtual string Type { get; set; }

        [Category("Common")]
        public virtual SectionMode Mode { get; set; }

        [Category("Common")]
        public SectionContainer ContainerMode
        {
            get { return _ContainerMode; }
            set { _ContainerMode = value; }
        }
        

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

        [Category("Common")]
        public string ContainerElement { get; set; }

        [Category("Common")]
        public string ContainerCssClass { get; set; }

        [Category("Common")]
        [TypeConverter("CodeTorch.Core.Design.SectionZoneTypeConverter,CodeTorch.Core.Design")]
        public string ContentPane { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public virtual string SelectDataCommand { get; set; }

        
        [Category("Security")]
        public PermissionCheck Permission
        {
            get { return _Permission; }
            set { _Permission = value; }
        }
        


        private List<BaseControl> _Controls = new List<BaseControl>();


        [XmlArray("Controls")]
        [XmlArrayItem(ElementName = "AutoCompleteBoxControl", Type = typeof(AutoCompleteBoxControl))]
        [XmlArrayItem(ElementName = "ButtonControl", Type = typeof(ButtonControl))]
        [XmlArrayItem(ElementName = "CheckBoxControl", Type = typeof(CheckBoxControl))]
        [XmlArrayItem(ElementName = "DatePickerControl", Type = typeof(DatePickerControl))]
        [XmlArrayItem(ElementName = "DropDownListControl", Type = typeof(DropDownListControl))]
        [XmlArrayItem(ElementName = "EditorControl", Type = typeof(EditorControl))]
        [XmlArrayItem(ElementName = "EmailAddressControl", Type = typeof(EmailAddressControl))]
        [XmlArrayItem(ElementName = "FileUploadControl", Type = typeof(FileUploadControl))]
        [XmlArrayItem(ElementName = "GenericControl", Type = typeof(GenericControl))]
        [XmlArrayItem(ElementName = "HyperLinkControl", Type = typeof(HyperLinkControl))]
        [XmlArrayItem(ElementName = "LabelControl", Type = typeof(LabelControl))]
        [XmlArrayItem(ElementName = "ListBoxControl", Type = typeof(ListBoxControl))]
        [XmlArrayItem(ElementName = "LookupDropDownListControl", Type = typeof(LookupDropDownListControl))]
        [XmlArrayItem(ElementName = "LookupListBoxControl", Type = typeof(LookupListBoxControl))]
        [XmlArrayItem(ElementName = "MultiComboDropDownListControl", Type = typeof(MultiComboDropDownListControl))]
        [XmlArrayItem(ElementName = "NumericTextBox", Type = typeof(NumericTextBoxControl))]
        [XmlArrayItem(ElementName = "PasswordControl", Type = typeof(PasswordControl))]
        [XmlArrayItem(ElementName = "PickerControl", Type = typeof(PickerControl))]
        [XmlArrayItem(ElementName = "SocialShareControl", Type = typeof(SocialShareControl))]
        [XmlArrayItem(ElementName = "TextAreaControl", Type = typeof(TextAreaControl))]
        [XmlArrayItem(ElementName = "TextBoxControl", Type = typeof(TextBoxControl))]
        [XmlArrayItem(ElementName = "TreeViewControl", Type = typeof(TreeViewControl))]
        [XmlArrayItem(ElementName = "WorkflowStatusControl", Type = typeof(WorkflowStatusControl))]
        [Category("Controls")]
        [Description("List of section controls")]
        [Editor("CodeTorch.Core.Design.ControlCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]  
        public virtual List<BaseControl> Controls
        {
            get
            {
                return _Controls;
            }
            set
            {
                _Controls = value;
            }

        }

        private Action _AfterPopulateSection = new Action();

        [Category("Actions")]
        public Action AfterPopulateSection
        {
            get
            {
                return _AfterPopulateSection;
            }
            set
            {
                _AfterPopulateSection = value;
            }

        }

        [Browsable(false)]
        [XmlIgnore()]
        public object Parent { get; set; }

        public override string ToString()
        {
            return "Section " + this.Name;
        }


        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, BaseSection Section, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            AddResourceKey(retVal, Screen, Section, Prefix, "Name", Section.Name);
            

            foreach (BaseControl control in Section.Controls)
            { 
                string ResourceKeyPrefix = String.Format("{0}.{1}", Prefix,Section.Name);
                retVal.AddRange(BaseControl.GetResourceKeys(Screen,control, ResourceKeyPrefix));
            }

            switch (Section.Type.ToLower())
            {
                //case "details":
                //    retVal.AddRange(DetailsSection.GetResourceKeys(Screen, ((DetailsSection)Section), Prefix));
                //    break;
                //case "edit":
                //    retVal.AddRange(EditSection.GetResourceKeys(Screen, ((EditSection)Section), Prefix));
                //    break;
                //case "fusionchart":
                //    retVal.AddRange(FusionChartSection.GetResourceKeys(Screen, ((FusionChartSection)Section), Prefix));
                //    break;
                //case "googlemap":
                //    retVal.AddRange(GoogleMapSection.GetResourceKeys(Screen, ((GoogleMapSection)Section), Prefix));
                //    break;
                case "grid":
                    retVal.AddRange(GridSection.GetResourceKeys(Screen, ((GridSection)Section), Prefix));
                    break;
                //case "image":
                //    retVal.AddRange(ImageSection.GetResourceKeys(Screen, ((ImageSection)Section), Prefix));
                //    break;
                
            }

            return retVal;
        }

        protected static void AddResourceKey(List<ResourceItem> keys, Screen screen, BaseSection section, string Prefix, string ResourceKey, string DefaultValue)
        {
            ResourceItem key = new Core.ResourceItem();

            key.ResourceSet = String.Format("App/{0}/{1}", screen.Folder, screen.Name);
            if (String.IsNullOrEmpty(Prefix))
            {
                key.Key = String.Format("{0}.{1}", section.Name, ResourceKey);
            }
            else
            {
                key.Key = String.Format("{0}.{1}.{2}", Prefix, section.Name, ResourceKey);
            }
            key.Value = DefaultValue;

            if (!String.IsNullOrEmpty(DefaultValue))
            {
                keys.Add(key);
            }
        }

        public static BaseSection GetNewSection(string type)
        {
            BaseSection retval = null;

            switch (type.ToLower())
            {
                case "alert":
                    retval = new AlertSection();
                    break;
                case "buttonlist":
                    retval = new ButtonListSection();
                    break;
                case "content":
                    retval = new ContentSection();
                    break;
                case "criteria":
                    retval = new CriteriaSection();
                    break;
                case "details":
                    retval = new DetailsSection();
                    break;
                case "edit":
                    retval = new EditSection();
                    break;
                case "editablegrid":
                    retval = new EditableGridSection();
                    break;
                case "grid":
                    retval = new GridSection();
                    break;
                case "image":
                    retval = new ImageSection();
                    break;
                case "linklist":
                    retval = new LinkListSection();
                    break;
                case "rdlcviewer":
                    retval = new RDLCViewerSection();
                    break;
                case "template":
                    retval = new TemplateSection();
                    break;
            }

            return retval;
        }
    }
}
