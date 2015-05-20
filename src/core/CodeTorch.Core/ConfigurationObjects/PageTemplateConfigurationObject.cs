using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class PageTemplateConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "PageTemplates"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((PageTemplate)configurationItem).Name));
            Configuration.GetInstance().PageTemplates.Remove(((PageTemplate)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            PageTemplate.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return PageTemplate.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().PageTemplates.Clear();
        }
        
        public void Save(object configurationItem)
        {
            PageTemplate.Save((PageTemplate)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().PageTemplates.Cast<object>());
            }
            else
            {
                retVal.Add(PageTemplate.GetByName(name));
            }

            return retVal;
        }
    }
}
