using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;
using CodeTorch.Abstractions;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class DataConnectionTypeConfigurationObject: IConfigurationObject2, IConfigurationManager<DataConnectionType>
    {
        public string ConfigurationFolder { get { return "DataConnectionTypes"; } }

        public void Delete( object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((DataConnectionType)configurationItem).Name));
            Configuration.GetInstance().DataConnectionTypes.Remove(((DataConnectionType)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            DataConnectionType.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return DataConnectionType.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().DataConnectionTypes.Clear();
        }
        
        public void Save(object configurationItem)
        {
            DataConnectionType.Save((DataConnectionType)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().DataConnectionTypes.Cast<object>());
            }
            else
            {
                retVal.Add(DataConnectionType.GetByName(name));
            }

            return retVal;
        }

        DataConnectionType IConfigurationManager<DataConnectionType>.Load(XDocument doc, string path)
        {
            return DataConnectionType.Load(doc);
        }
    }
}
