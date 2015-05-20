using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class EmailConnectionTypeConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "EmailConnectionTypes"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml",ConfigurationLoader.GetFileConfigurationPath(),ConfigurationFolder, ((EmailConnectionType)configurationItem).Name));
            Configuration.GetInstance().EmailConnectionTypes.Remove(((EmailConnectionType)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            EmailConnectionType.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return EmailConnectionType.GetItemCount(name);
        }

        public void ClearAll()
        {
           Configuration.GetInstance().EmailConnectionTypes.Clear();  
        }
        
        public void Save(object configurationItem)
        {
            EmailConnectionType.Save((EmailConnectionType)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().EmailConnectionTypes.Cast<object>());
            }
            else
            {
                retVal.Add(EmailConnectionType.GetByName(name));
            }

            return retVal;
        }
    }
}
