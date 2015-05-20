using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Core.Interfaces
{
    public interface IConfigurationObject
    {
        string ConfigurationFolder { get; }

        void ClearAll();
        void Load(XDocument doc, string folder);
        //int GetCounts(string folder, string name);
        void Save(object configurationItem);
        void Delete(object configurationItem);
       // List<object> GetItems(string folder, string name);
    }
}
