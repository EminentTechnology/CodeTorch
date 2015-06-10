using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core
{
    [Serializable]
    public class TreeViewControl : Widget
    {

        public bool DisplayCheckBoxes { get; set; }
        public bool CheckChildNodes { get; set; }
        public bool CausesValidation { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string SelectDataCommand { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataFieldID { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataFieldParentID { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataTextField { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataValueField { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataNavigateUrlField { get; set; }


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

        [Category("Behavior")]
        public bool EnableDragAndDrop { get; set; }

        [Category("Behavior")]
        public bool EnableDragAndDropBetweenNodes { get; set; }

        public string OnClientMouseOver { get; set; }
        public string OnClientMouseOut { get; set; }
        


        public override string Type
        {
            get
            {
                return "TreeView";
            }
            set
            {
                base.Type = value;
            }
        }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, TreeViewControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add title
            //AddResourceKey(retVal, Screen, Control, Prefix, "EmptyMessage", Control.EmptyMessage);



            return retVal;
        }
    }
}
