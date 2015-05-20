using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class ControlTypeConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "ControlTypes"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(),ConfigurationFolder, ((ControlType)configurationItem).Name));
            Configuration.GetInstance().ControlTypes.Remove(((ControlType)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            ControlType.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return ControlType.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().ControlTypes.Clear();
        }
        
        public void Save(object configurationItem)
        {
            ControlType.Save((ControlType)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().ControlTypes.Cast<object>());
            }
            else
            {
                retVal.Add(ControlType.GetControlType(name));
            }

            return retVal;
        }
    }
}
