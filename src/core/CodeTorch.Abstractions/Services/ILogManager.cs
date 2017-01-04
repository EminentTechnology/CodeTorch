using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Abstractions
{
    public interface ILogManager
    {
        ILog GetLogger(string namedLogger);
        ILog GetLogger(Type type);
    }
}
