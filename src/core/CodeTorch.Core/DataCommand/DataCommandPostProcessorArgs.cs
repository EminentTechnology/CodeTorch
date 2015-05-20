using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace CodeTorch.Core
{
    [Serializable]
    public class DataCommandPostProcessorArgs
    {
        public DbTransaction Transaction { get; set; }
        public DataCommand DataCommand { get; set; }
        public List<ScreenDataCommandParameter> Parameters { get; set; }
        public object Data { get; set; }
    }
}
