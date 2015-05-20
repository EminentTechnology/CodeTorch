using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class MenuConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "Menus"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((Menu)configurationItem).Name));
            Configuration.GetInstance().Menus.Remove(((Menu)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            Menu.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return Menu.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().Menus.Clear();
        }
        
        public void Save(object configurationItem)
        {
            Menu.Save((Menu)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().Menus.Cast<object>());
            }
            else
            {
                retVal.Add(Menu.GetMenu(name));
            }

            return retVal;
        }
    }
}
