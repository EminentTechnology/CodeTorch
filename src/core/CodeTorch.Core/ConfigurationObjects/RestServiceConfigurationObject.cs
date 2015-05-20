using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class RestServiceConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "RestServices"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((RestService)configurationItem).Name));
            Configuration.GetInstance().RestServices.Remove(((RestService)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            RestService.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return RestService.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().RestServices.Clear();
        }
        
        public void Save(object configurationItem)
        {
            RestService.Save((RestService)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().RestServices.Cast<object>());
            }
            else
            {
                retVal.Add(RestService.GetByName(name));
            }

            return retVal;
        }
    }
}
