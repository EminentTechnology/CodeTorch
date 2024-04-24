using System;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class PostRestServiceMethod : BaseRestServiceMethod
    {
        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string ResponseDataCommand { get; set; }
    }
}
