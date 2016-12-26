using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;
using CodeTorch.Abstractions;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class WorkflowTypeConfigurationObject: IConfigurationObject2, IConfigurationManager<WorkflowType>
    {
        public string ConfigurationFolder { get { return "WorkflowTypes"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((WorkflowType)configurationItem).Name));
            Configuration.GetInstance().WorkflowTypes.Remove(((WorkflowType)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            WorkflowType.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return WorkflowType.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().WorkflowTypes.Clear();
        }
        
        public void Save(object configurationItem)
        {
            WorkflowType.Save((WorkflowType)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().WorkflowTypes.Cast<object>());
            }
            else
            {
                retVal.Add(WorkflowType.GetByName(name));
            }

            return retVal;
        }

        WorkflowType IConfigurationManager<WorkflowType>.Load(XDocument doc, string path)
        {
            return WorkflowType.Load(doc);
        }
    }
}
