using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Xml.Linq;

using System.Xml;



using System.Reflection;





namespace CodeTorch.Core
{

    public class ConfigurationLoader
    {
        static Assembly configAssembly = null;
        static string configAssemblyName = null;

        public static string ConfigAssemblyName
        {
            get
            {
                return configAssemblyName;
            }
            
        }

        public static Assembly ConfigAssembly
        {
            get
            {
                return configAssembly;
            }
           
        }

        public static void LoadMobileConfiguration(string assemblyName)
        {
            //TODO: mobile licensing
            //License.GetInstance().LoadServerLicenseFile();
            configAssemblyName = assemblyName;
            AssemblyName a = new AssemblyName(assemblyName);
            configAssembly = Assembly.Load(a);

            foreach (var res in configAssembly.GetManifestResourceNames())
                System.Diagnostics.Debug.WriteLine("found resource: " + res);

            Configuration.GetInstance().App = GetAppObject();
        }

        public static App GetAppObject()
        {
            App retVal = null;
            string configMode = null;

            string item = String.Format("{0}.{1}.{2}", configAssemblyName, "App", "App.xml");
            using (Stream fileStream = configAssembly.GetManifestResourceStream(item))
            {
                using (XmlReader xreader = XmlReader.Create(fileStream))
                {
                    XDocument doc = XDocument.Load(xreader);

                    retVal = App.Populate(doc);
                }
            }

            return retVal;
        }

    }



    
}

