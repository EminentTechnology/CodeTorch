using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    public enum ListBoxSelectionMode
    {

        // Summary:
        //     The default behaviour - only one Item can be selected at a time.
        Single = 0,
        //
        // Summary:
        //     Allows selection of multiple Items.
        Multiple = 1
    }

    [Serializable]
    public class ListBoxControl : Widget
    {
        public bool AutoPostBack { get; set; }
        public bool DisplayCheckBoxes { get; set; }
        public bool CausesValidation { get; set; }
        

        public string EmptyMessage { get; set; }

        public ListBoxSelectionMode SelectionMode { get; set; }

      


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

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataSortField { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataKeyField { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataCheckedField { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataCheckableField { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataImageUrlField { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataCssClassField { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataSelectedField { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataEnabledField { get; set; }



        [Category("Appearance")]
        public string Height { get; set; }



        [Category("Appearance")]
        public string Skin { get; set; }





        public override string Type
        {
            get
            {
                return "ListBox";
            }
            set
            {
                base.Type = value;
            }
        }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, ListBoxControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add title
            AddResourceKey(retVal, Screen, Control, Prefix, "EmptyMessage", Control.EmptyMessage);

          

            return retVal;
        }
    }
}
