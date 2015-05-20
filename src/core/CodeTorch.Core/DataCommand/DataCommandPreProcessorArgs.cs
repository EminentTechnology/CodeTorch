using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace CodeTorch.Core
{
    [Serializable]
    public class DataCommandPreProcessorArgs
    {
        public DbTransaction Transaction { get; set; }
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
