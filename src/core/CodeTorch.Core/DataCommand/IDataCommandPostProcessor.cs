using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    public interface IDataCommandPostProcessor
    {
        void Process(DataCommandPostProcessorArgs args);
    }
}
