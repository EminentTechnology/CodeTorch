using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;
using CodeTorch.Abstractions;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class ScreenTypeConfigurationObject: IConfigurationObject2, IConfigurationManager<ScreenType>
    {
        public string ConfigurationFolder { get { return "ScreenTypes"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((ScreenType)configurationItem).Name));
            Configuration.GetInstance().ScreenTypes.Remove(((ScreenType)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            ScreenType.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return ScreenType.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().ScreenTypes.Clear();
        }
        
        public void Save(object configurationItem)
        {
            ScreenType.Save((ScreenType)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().ScreenTypes.Cast<object>());
            }
            else
            {
                retVal.Add(ScreenType.GetByName(name));
            }

            return retVal;
        }

        ScreenType IConfigurationManager<ScreenType>.Load(XDocument doc, string path)
        {
            return ScreenType.Load(doc);
        }
    }
}
