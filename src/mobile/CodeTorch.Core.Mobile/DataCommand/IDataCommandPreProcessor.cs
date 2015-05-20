using System;
using System.Linq;

namespace CodeTorch.Core
{
    public interface  IDataCommandPreProcessor
    {
        void Process(DataCommandPreProcessorArgs args);
    }
}
