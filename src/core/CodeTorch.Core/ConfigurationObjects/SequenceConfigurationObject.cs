using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class SequenceConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "Sequences"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((Sequence)configurationItem).Name));
            Configuration.GetInstance().Sequences.Remove(((Sequence)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            Sequence.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return Sequence.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().Sequences.Clear();
        }
        
        public void Save(object configurationItem)
        {
            Sequence.Save((Sequence)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().Sequences.Cast<object>());
            }
            else
            {
                retVal.Add(Sequence.GetByName(name));
            }

            return retVal;
        }
    }
}
