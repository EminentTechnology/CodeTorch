using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeTorch.Core
{

    public class DataCommandPreProcessorArgs
    {

        public DataCommand DataCommand { get; set; }
        public List<ScreenDataCommandParameter> Parameters { get; set; }
        private bool _SkipExecution = false;

        public bool SkipExecution
        {
            get { return _SkipExecution; }
            set { _SkipExecution = value; }
        }

        public object Data { get; set; }
    }
}
