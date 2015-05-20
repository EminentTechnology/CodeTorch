using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeTorch.Core
{

    public class DataCommandPostProcessorArgs
    {

        public DataCommand DataCommand { get; set; }
        public List<ScreenDataCommandParameter> Parameters { get; set; }
        public object Data { get; set; }
    }
}
