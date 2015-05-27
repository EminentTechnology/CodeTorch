using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing.Design;

namespace CodeTorch.Core
{
    [Serializable]
    public class EditableGridSection: GridSection
    {
        ScreenInputType _EntityInputType = ScreenInputType.QueryString;
        private List<Section> _Sections = new List<Section>();

        public override string Type
        {
            get
            {
                return "EditableGrid";
            }
            set
            {
                base.Type = value;

                
            }
        }

        

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string InsertCommand { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string UpdateCommand { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string DefaultCommand { get; set; }

        [Category("Sections")]
        [Description("List of page sections")]
        [XmlArray("Sections")]
        //[XmlArrayItem(ElementName = "AlertSection", Type = typeof(AlertSection))]
        //[XmlArrayItem(ElementName = "ButtonListSection", Type = typeof(ButtonListSection))]
        //[XmlArrayItem(ElementName = "ContentSection", Type = typeof(ContentSection))]
        //[XmlArrayItem(ElementName = "CriteriaSection", Type = typeof(CriteriaSection))]
        //[XmlArrayItem(ElementName = "CustomSection", Type = typeof(CustomSection))]
        //[XmlArrayItem(ElementName = "DetailsSection", Type = typeof(DetailsSection))]
        //[XmlArrayItem(ElementName = "EditableGridSection", Type = typeof(EditableGridSection))]
        //[XmlArrayItem(ElementName = "EditSection", Type = typeof(EditSection))]
        //[XmlArrayItem(ElementName = "GridSection", Type = typeof(GridSection))]
        //[XmlArrayItem(ElementName = "ImageSection", Type = typeof(ImageSection))]
        //[XmlArrayItem(ElementName = "LinkListSection", Type = typeof(LinkListSection))]
        //[XmlArrayItem(ElementName = "RDLCViewerSection", Type = typeof(RDLCViewerSection))]
        //[XmlArrayItem(ElementName = "TemplateSection", Type = typeof(TemplateSection))]
        [Editor("CodeTorch.Core.Design.SectionCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]
        public List<Section> Sections
        {
            get
            {
                return _Sections;
            }
            set
            {
                _Sections = value;
            }

        }

        [Category("Sections")]
        [TypeConverter("CodeTorch.Core.Design.SectionZoneLayoutTypeConverter,CodeTorch.Core.Design")]
        public string SectionZoneLayout { get; set; }

        

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, GridSection Section, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            AddResourceKey(retVal, Screen, Section, Prefix, "Grid.HelpText", Section.Grid.HelpText);

            foreach (GridColumn column in Section.Grid.Columns)
            {
                string ResourceKeyPrefix = String.Format("{0}.{1}.Grid.Columns", Prefix, Section.Name);
                retVal.AddRange(GridColumn.GetResourceKeys(Screen, column, ResourceKeyPrefix));
            }

           

            return retVal;
        }

        
    }
}
