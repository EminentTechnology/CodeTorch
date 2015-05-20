using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class ContentDataItem
    {

        [Category("Data")]
        public string DataItem { get; set; }
        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string DataCommand { get; set; }

        public override string ToString()
        {
            return DataItem;
        }
    }
}
