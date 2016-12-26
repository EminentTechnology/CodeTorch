using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;
using CodeTorch.Abstractions;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class WorkflowConfigurationObject: IConfigurationObject2, IConfigurationManager<Workflow>
    {
        public string ConfigurationFolder { get { return "Workflows"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((Workflow)configurationItem).Name));
            Configuration.GetInstance().Workflows.Remove(((Workflow)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            Workflow.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return Workflow.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().Workflows.Clear();
        }
        
        public void Save(object configurationItem)
        {
            Workflow.Save((Workflow)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().Workflows.Cast<object>());
            }
            else
            {
                retVal.Add(Workflow.GetWorkflowByCode(name));
            }

            return retVal;
        }

        Workflow IConfigurationManager<Workflow>.Load(XDocument doc, string path)
        {
            return Workflow.Load(doc);
        }
    }
}
