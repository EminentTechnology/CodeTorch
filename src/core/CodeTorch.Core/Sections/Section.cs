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
    public class Section
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
        


        private List<Widget> _Controls = new List<Widget>();


        [XmlArray("Widgets")]
        [Category("Widgets")]
        [Description("List of section widgets")]
#if NETFRAMEWORK
        [Editor("CodeTorch.Core.Design.ControlCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]  
#endif
        public virtual List<Widget> Widgets
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


        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, Section Section, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            AddResourceKey(retVal, Screen, Section, Prefix, "Name", Section.Name);
            

            foreach (Widget control in Section.Widgets)
            { 
                string ResourceKeyPrefix = String.Format("{0}.{1}", Prefix,Section.Name);
                retVal.AddRange(Widget.GetResourceKeys(Screen,control, ResourceKeyPrefix));
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

        protected static void AddResourceKey(List<ResourceItem> keys, Screen screen, Section section, string Prefix, string ResourceKey, string DefaultValue)
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

        public static Section GetNewSection(string type)
        {
            SectionType sectionType = SectionType.GetByName(type);
            string assemblyQualifiedName = String.Format("{0}, {1}", sectionType.AbstractionClass, sectionType.AbstractionAssembly);

            Type t = System.Type.GetType(assemblyQualifiedName, false, true);
            Section retval = (Section)Activator.CreateInstance(t);
            
            return retval;
        }
    }
}
