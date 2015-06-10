using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class LookupListBoxControl : Widget
    {
        public bool AutoPostBack { get; set; }
        public bool DisplayCheckBoxes { get; set; }
        public string EmptyMessage { get; set; }
        public bool CausesValidation { get; set; }

        public ListBoxSelectionMode SelectionMode { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.LookupTypeConverter,CodeTorch.Core.Design")]
        public string LookupType { get; set; }

        [Category("Appearance")]
        public string Height { get; set; }

        [Category("Appearance")]
        public string Skin { get; set; }

        public string RelatedControl { get; set; }




        public override string Type
        {
            get
            {
                return "LookupListBox";
            }
            set
            {
                base.Type = value;
            }
        }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, LookupListBoxControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add title
            AddResourceKey(retVal, Screen, Control, Prefix, "EmptyMessage", Control.EmptyMessage);



            return retVal;
        }
    }
}
