using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;
using CodeTorch.Abstractions;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class RestServiceConfigurationObject: IConfigurationObject2, IConfigurationManager<RestService>
    {
        public string ConfigurationFolder { get { return "RestServices"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}\\{3}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((RestService)configurationItem).Folder, ((RestService)configurationItem).Name));
            Configuration.GetInstance().Screens.Remove(((Screen)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            RestService.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return RestService.GetItemCount(folder,name);
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

            if ((String.IsNullOrEmpty(folder)) && (String.IsNullOrEmpty(name)))
            {
                retVal = new List<object>(Configuration.GetInstance().RestServices.Cast<object>());
            }
            else
            {
                if ((!String.IsNullOrEmpty(folder)) && (String.IsNullOrEmpty(name)))
                {
                    retVal = new List<object>(RestService.GetByFolder(folder).Cast<object>());
                }
                else
                {
                    retVal.Add(RestService.GetByFolderAndName(folder, name));
                }
            }

            return retVal;
        }

        RestService IConfigurationManager<RestService>.Load(XDocument doc, string path)
        {
            return RestService.Load(doc);
        }
    }
}
