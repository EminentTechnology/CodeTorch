using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    public class PutRestServiceMethod : BaseRestServiceMethod
    {
        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string ResponseDataCommand { get; set; }
    }
}
