using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class DocumentRepositoryConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "DocumentRepositories"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((DocumentRepository)configurationItem).Name));
            Configuration.GetInstance().DocumentRepositories.Remove(((DocumentRepository)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            DocumentRepository.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return DocumentRepository.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().DocumentRepositories.Clear();
        }
        
        public void Save(object configurationItem)
        {
            DocumentRepository.Save((DocumentRepository)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().DocumentRepositories.Cast<object>());
            }
            else
            {
                retVal.Add(DocumentRepository.GetByName(name));
            }

            return retVal;
        }
    }
}
