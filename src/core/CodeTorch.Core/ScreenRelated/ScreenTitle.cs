using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ScreenTitle
    {
        public string Name { get; set; }
        public string FormattedName { get; set; }
        public bool UseCommand { get; set; }
        public string CommandFormatString { get; set; }
        public string FormattedCommandFormatString { get; set; }

        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string CommandName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
