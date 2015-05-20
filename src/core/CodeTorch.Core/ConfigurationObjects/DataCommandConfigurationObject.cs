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
    public class DataCommandConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "DataCommands"; } }

        public void Delete( object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((DataCommand)configurationItem).Name));
            Configuration.GetInstance().DataCommands.Remove(((DataCommand)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            DataCommand.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return DataCommand.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().DataCommands.Clear();
        }
        
        public void Save(object configurationItem)
        {
            DataCommand.Save((DataCommand)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().DataCommands.Cast<object>());
            }
            else
            {
                retVal.Add(DataCommand.GetDataCommand(name));
            }

            return retVal;
        }
    }
}
