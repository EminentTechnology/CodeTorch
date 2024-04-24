using System;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class PutRestServiceMethod : BaseRestServiceMethod
    {
        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string ResponseDataCommand { get; set; }
    }
}
