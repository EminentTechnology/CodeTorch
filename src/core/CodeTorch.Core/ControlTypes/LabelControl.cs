using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class LabelControl: Widget
    {

        public override string Type
        {
            get
            {
                return "Label";
            }
            set
            {
                base.Type = value;
            }
        }

        public string FormatString { get; set; }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, LabelControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add title
            //AddResourceKey(retVal, Screen, Control, Prefix, "FormatString", Control.FormatString);



            return retVal;
        }
    }
}
