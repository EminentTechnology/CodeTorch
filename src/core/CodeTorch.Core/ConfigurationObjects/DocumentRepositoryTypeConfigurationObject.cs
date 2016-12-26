using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;
using CodeTorch.Abstractions;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class DocumentRepositoryTypeConfigurationObject: IConfigurationObject2, IConfigurationManager<DocumentRepositoryType>
    {
        public string ConfigurationFolder { get { return "DocumentRepositoryTypes"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((DocumentRepositoryType)configurationItem).Name));
            Configuration.GetInstance().DocumentRepositoryTypes.Remove(((DocumentRepositoryType)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            DocumentRepositoryType.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return DocumentRepositoryType.GetItemCount(name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().DocumentRepositoryTypes.Clear();
        }
        
        public void Save(object configurationItem)
        {
            DocumentRepositoryType.Save((DocumentRepositoryType)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if (String.IsNullOrEmpty(name))
            {
                retVal = new List<object>(Configuration.GetInstance().DocumentRepositoryTypes.Cast<object>());
            }
            else
            {
                retVal.Add(DocumentRepositoryType.GetByName(name));
            }

            return retVal;
        }

        DocumentRepositoryType IConfigurationManager<DocumentRepositoryType>.Load(XDocument doc, string path)
        {
            return DocumentRepositoryType.Load(doc);
        }
    }
}
