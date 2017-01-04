using CodeTorch.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Logger.Log4Net
{
    public class Log4NetLogManager : ILogManager
    {
        public ILog GetLogger(string namedLogger)
        {
            var logger = log4net.LogManager.GetLogger(namedLogger);
            return new Log4NetLogger(logger);
        }

        public ILog GetLogger(Type type)
        {
            var logger = log4net.LogManager.GetLogger(type);
            return new Log4NetLogger(logger);
        }
    }
}
