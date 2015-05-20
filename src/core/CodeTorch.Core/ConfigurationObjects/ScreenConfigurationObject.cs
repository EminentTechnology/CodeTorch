using System;
using System.Linq;
using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class ScreenConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "Screens"; } }

        public void Delete(object configurationItem)
        {
            File.Delete(String.Format("{0}{1}\\{2}\\{3}.xml", ConfigurationLoader.GetFileConfigurationPath(), ConfigurationFolder, ((Screen)configurationItem).Folder, ((Screen)configurationItem).Name));
            Configuration.GetInstance().Screens.Remove(((Screen)configurationItem));
        }

        public void Load(XDocument doc, string folder)
        {
            Screen.Load(doc, folder);
        }

        public int GetCounts(string folder, string name)
        {
            return Screen.GetItemCount(folder,name);
        }

        public void ClearAll()
        {
            Configuration.GetInstance().Screens.Clear();
        }
        
        public void Save(object configurationItem)
        {
            Screen.Save((Screen)configurationItem);
        }

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            if ((String.IsNullOrEmpty(folder)) && (String.IsNullOrEmpty(name)))
            {
                retVal = new List<object>(Configuration.GetInstance().Screens.Cast<object>());
            }
            else
            {   
                if((!String.IsNullOrEmpty(folder)) && (String.IsNullOrEmpty(name)))
                {
                    retVal = new List<object>(Screen.GetByFolder(folder).Cast<object>()); 
                }
                else
                {
                    retVal.Add(Screen.GetByFolderAndName(folder, name));
                }
            }

            return retVal;
        }
    }
}
