using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class ReportDataSource
    {
        public string Name { get; set; }

        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string ReportCommand { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
