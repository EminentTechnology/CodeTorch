using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    public interface  IDataCommandPreProcessor
    {
        void Process(DataCommandPreProcessorArgs args);
    }
}
