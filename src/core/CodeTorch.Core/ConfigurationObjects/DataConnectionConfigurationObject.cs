using CodeTorch.Abstractions;
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
    public class DataConnectionConfigurationObject: IConfigurationObject2, IConfigurationManager<DataConnection>
    {
        public string ConfigurationFolder { get { return "DataConnections"; } }

        public void Delete( object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((DataConnection)configurationItem).Name));
            Configuration.GetInstance().DataConnections.Remove(((DataConnection)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            DataConnection.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return DataConnection.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().DataConnections.Clear();
        }
        
        public void Save(object configurationItem)
        {
            DataConnection.Save((DataConnection)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().DataConnections.Cast<object>());
            }
            else
            {
                retVal.Add(DataConnection.GetByName(name));
            }

            return retVal;
        }

        DataConnection IConfigurationManager<DataConnection>.Load(XDocument doc, string path)
        {
            return DataConnection.Load(doc);
        }
    }
}
