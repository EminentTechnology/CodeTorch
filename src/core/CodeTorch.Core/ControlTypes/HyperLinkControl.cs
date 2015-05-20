using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class HyperLinkControl: BaseControl
    {

        public override string Type
        {
            get
            {
                return "HyperLink";
            }
            set
            {
                base.Type = value;
            }
        }

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DataNavigateUrlFields { get; set; }

        [Category("Data")]
        public string DataNavigateUrlFormatString { get; set; }


        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DataTextField { get; set; }

        [Category("Data")]
        public string DataTextFormatString { get; set; }

        [Category("Data")]
        public string Text { get; set; }

        [Category("Data")]
        public string Url { get; set; }

        [Category("Data")]
        public string Target { get; set; }

        public string Relationship { get; set; }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, HyperLinkControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add title
            AddResourceKey(retVal, Screen, Control, Prefix, "Text", Control.Text);
            


            return retVal;
        }
    }
}
