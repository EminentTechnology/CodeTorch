
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eminent.AppBuilder.Core
{

    public class CodeTorchContainer
    {
        static readonly CodeTorchContainer instance = new CodeTorchContainer();

        public static CodeTorchContainer GetInstance()
        {
            return instance;
        }

        private CodeTorchContainer()
        {
           
        }

        public readonly Container Container = new Container();
    }
}
