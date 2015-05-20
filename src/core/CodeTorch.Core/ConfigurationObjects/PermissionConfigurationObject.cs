using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class PermissionConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "Permissions"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((Permission)configurationItem).Name));
            Configuration.GetInstance().Permissions.Remove(((Permission)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            Permission.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return Permission.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().Permissions.Clear();
        }
        
        public void Save(object configurationItem)
        {
            Permission.Save((Permission)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().Permissions.Cast<object>());
            }
            else
            {
                retVal.Add(Permission.GetByName(name));
            }

            return retVal;
        }
    }
}
