using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;
using CodeTorch.Abstractions;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class SectionTypeConfigurationObject: IConfigurationObject2, IConfigurationManager<SectionType>
    {
        public string ConfigurationFolder { get { return "SectionTypes"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((SectionType)configurationItem).Name));
            Configuration.GetInstance().SectionTypes.Remove(((SectionType)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            SectionType.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return SectionType.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().SectionTypes.Clear();
        }
        
        public void Save(object configurationItem)
        {
            SectionType.Save((SectionType)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().SectionTypes.Cast<object>());
            }
            else
            {
                retVal.Add(SectionType.GetByName(name));
            }

            return retVal;
        }

        SectionType IConfigurationManager<SectionType>.Load(XDocument doc, string path)
        {
            return SectionType.Load(doc);
        }
    }
}
