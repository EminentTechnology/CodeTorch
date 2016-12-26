using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Core.Interfaces
{










    public interface IConfigurationObject2
    {
        string ConfigurationFolder { get; }

        void ClearAll();
        void Load(XDocument doc, string folder);
        
        void Save(object configurationItem);
        void Delete(object configurationItem);
       
    }
}
