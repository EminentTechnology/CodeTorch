using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class SectionZoneLayoutConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "SectionZoneLayouts"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((SectionZoneLayout)configurationItem).Name));
            Configuration.GetInstance().SectionZoneLayouts.Remove(((SectionZoneLayout)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            SectionZoneLayout.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return SectionZoneLayout.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().SectionZoneLayouts.Clear();
        }
        
        public void Save(object configurationItem)
        {
            SectionZoneLayout.Save((SectionZoneLayout)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().SectionZoneLayouts.Cast<object>());
            }
            else
            {
                retVal.Add(SectionZoneLayout.GetByName(name));
            }

            return retVal;
        }
    }
}
