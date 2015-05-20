using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class DashboardComponentConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "ScreenTypes"; } }

        public void Delete( object configurationItem)
        {
            throw new NotImplementedException();
        }

        public void Load(XDocument doc, string folder)
        {
            throw new NotImplementedException();
        }

        public int GetCounts(string folder, string name)
        {
             throw new NotImplementedException();
        }

        public void ClearAll()
        {
            throw new NotImplementedException();
        }
        
        public void Save(object configurationItem)
        {
            throw new NotImplementedException();
        }

        public List<object> GetItems(string folder, string name)
        {
            throw new NotImplementedException();
        }
    }
}
