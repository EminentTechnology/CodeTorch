using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeTorch.Core.ConfigurationObjects
{
    public class AppConfigurationObject: IConfigurationObject
    {
        public string ConfigurationFolder { get { return "App"; } }

        public void Delete(object configurationItem)
        {
            throw new NotImplementedException();
        }

        public void Load(XDocument doc, string folder)
        {
            App.Load(doc);
        }

        public int GetCounts(string folder, string name)
        {
            return App.GetItemCount();
        }
        
        public void ClearAll()
        {
            //no collection to clear
        }
        
        public void Save(object configurationItem)
        {
            App.Save((App)configurationItem);
        }
        

        public List<object> GetItems(string folder, string name)
        {
            List<object> retVal = new List<object>();

            retVal.Add(Configuration.GetInstance().App);

            return retVal;
        }
        

        
    }
}
