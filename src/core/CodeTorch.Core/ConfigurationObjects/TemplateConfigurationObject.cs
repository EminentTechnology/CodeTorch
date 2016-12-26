using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;
using CodeTorch.Abstractions;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class TemplateConfigurationObject: IConfigurationObject2, IConfigurationManager<Template>
    {
        public string ConfigurationFolder { get { return "Templates"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((Template)configurationItem).Name));
            Configuration.GetInstance().Templates.Remove(((Template)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            Template.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return Template.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().Templates.Clear();
        }
        
        public void Save(object configurationItem)
        {
            Template.Save((Template)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().Templates.Cast<object>());
            }
            else
            {
                retVal.Add(Template.GetByName(name));
            }

            return retVal;
        }

        Template IConfigurationManager<Template>.Load(XDocument doc, string path)
        {
            return Template.Load(doc);
        }
    }
}
