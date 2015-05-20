using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ScreenMenu
    {
        [TypeConverter("CodeTorch.Core.Design.MenuTypeConverter,CodeTorch.Core.Design")]
        public string Name { get; set; }
        public string SelectedPrimaryMenu { get; set; }

        public bool DisplayMenu { get; set; }

        public override string ToString()
        {
            string retVal = Name;

            if (!DisplayMenu)
                retVal = "Not Displayed";

            return retVal;
        }
    }
}
