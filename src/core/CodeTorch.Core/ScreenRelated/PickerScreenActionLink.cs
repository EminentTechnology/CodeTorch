using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PickerScreenActionLink
    {
        PermissionCheck _Permission = new PermissionCheck();

        public string Text { get; set; }
        public bool ShowLink { get; set; }

        public PermissionCheck Permission { get { return _Permission; } set { _Permission = value; } }

        public override string ToString()
        {
            string retVal = Text;

            if (!ShowLink)
            {
                retVal = "Not Displayed";
            }
            else
            {
                if (String.IsNullOrEmpty(Text))
                    retVal = "No Link Text";
            }

            return retVal;
        }
    }
}
