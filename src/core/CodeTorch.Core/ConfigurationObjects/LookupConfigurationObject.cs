using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class LookupConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "Lookups"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((Lookup)configurationItem).Name));
            Configuration.GetInstance().Lookups.Remove(((Lookup)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            Lookup.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return Lookup.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().Lookups.Clear();
        }
        
        public void Save(object configurationItem)
        {
            Lookup.Save((Lookup)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().Lookups.Cast<object>());
            }
            else
            {
                retVal.Add(Lookup.GetByName(name));
            }

            return retVal;
        }
    }
}
