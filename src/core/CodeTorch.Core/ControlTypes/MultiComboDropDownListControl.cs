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
    public class MultiComboDropDownListControl : Widget
    {
        public bool AutoPostBack { get; set; }
        public bool IncludeAdditionalListItem { get; set; }
        public bool CausesValidation { get; set; }

        public string EmptyMessage { get; set; }
        public string AdditionalListItemText { get; set; }
        public string AdditionalListItemValue { get; set; }

        bool _ShowToggleImage = true;
        public bool ShowToggleImage
        {
            get
            {
                return _ShowToggleImage;
            }
            set
            {
                _ShowToggleImage = value;
            }
        }


         [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string SelectDataCommand { get; set; }

        public string RelatedControl { get; set; }

         [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataTextField { get; set; }

         [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataValueField { get; set; }

        [Category("Load On Demand")]
        public bool EnableLoadOnDemand { get; set; }


        [Category("Load On Demand")]
        public bool EnableItemCaching { get; set; }
        [Category("Load On Demand")]
        public bool EnableVirtualScrolling { get; set; }

        public bool MarkFirstMatch { get; set; }
        public bool IsCaseSensitive { get; set; }
        public DropDownListFilterMode FilterMode { get; set; }
        public DropDownListRenderingMode RenderingMode { get; set; }
        public string DropDownWidth { get; set; }

        [Category("Appearance")]
        public string Skin { get; set; }



        public override string Type
        {
            get
            {
                return "MultiComboDropDownList";
            }
            set
            {
                base.Type = value;
            }
        }

        List<MultiComboDropDownListColumn> _Columns = new List<MultiComboDropDownListColumn>();

        [Category("Data")]
        [XmlArray("Columns")]
        [XmlArrayItem("Column")]
        [Description("List of dropdown columns for multi combo")]
        public List<MultiComboDropDownListColumn> Columns
        {
            get
            {
                return _Columns;
            }
            set
            {
                _Columns = value;
            }

        }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, MultiComboDropDownListControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add title
            AddResourceKey(retVal, Screen, Control, Prefix, "EmptyMessage", Control.EmptyMessage);
            AddResourceKey(retVal, Screen, Control, Prefix, "AdditionalListItemText", Control.AdditionalListItemText);

            foreach (MultiComboDropDownListColumn column in Control.Columns)
            {
                string resourceKey = String.Format("Columns.{0}.HeaderText", column.DataField);
                AddResourceKey(retVal, Screen, Control, Prefix, resourceKey, column.HeaderText);
            }


            return retVal;
        }
    }
}
