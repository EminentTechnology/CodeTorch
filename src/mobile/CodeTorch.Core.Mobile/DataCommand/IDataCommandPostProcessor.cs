using System;
using System.Linq;

namespace CodeTorch.Core
{
    public interface IDataCommandPostProcessor
    {
        void Process(DataCommandPostProcessorArgs args);
    }
}
