using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class PickerConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "Pickers"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((Picker)configurationItem).Name));
            Configuration.GetInstance().Pickers.Remove(((Picker)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            Picker.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return Picker.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().Pickers.Clear();
        }
        
        public void Save(object configurationItem)
        {
            Picker.Save((Picker)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().Pickers.Cast<object>());
            }
            else
            {
                retVal.Add(Picker.GetPicker(name));
            }

            return retVal;
        }
    }
}
