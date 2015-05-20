using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class EmailConnectionConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "EmailConnections"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((EmailConnection)configurationItem).Name));
            Configuration.GetInstance().EmailConnections.Remove(((EmailConnection)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            EmailConnection.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return EmailConnection.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().EmailConnections.Clear();
        }
        
        public void Save(object configurationItem)
        {
            EmailConnection.Save((EmailConnection)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().EmailConnections.Cast<object>());
            }
            else
            {
                retVal.Add(EmailConnection.GetByName(name));
            }

            return retVal;
        }
    }
}
