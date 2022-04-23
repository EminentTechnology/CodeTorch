using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Abstractions
{
    public interface IConfigurationManager<T>
    {
        T Load(XDocument doc, string path);


    }
}
