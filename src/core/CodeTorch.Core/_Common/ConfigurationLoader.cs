using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Xml.Linq;

using System.Xml;

using System.Web;

using System.Reflection;
using System.Deployment.Application;
using System.Windows.Forms;
using CodeTorch.Core.Interfaces;
using CodeTorch.Core.ConfigurationObjects;

namespace CodeTorch.Core
{

    public class ConfigurationLoader
    {
        public static App GetAppObject()
        {
            App retVal = null;
            string configMode = ConfigurationManager.AppSettings["APPBUILDER_CONFIG_MODE"];

            if ((!String.IsNullOrEmpty(configMode)) && (configMode.ToLower() == "folder"))
            {
                string configFolderPath = String.Format("", ConfigurationManager.AppSettings["APPBUILDER_CONFIG_FOLDER"]);
                if (!String.IsNullOrEmpty(configFolderPath))
                {
                    if (!configFolderPath.EndsWith("\\"))
                        configFolderPath += "\\";

                    string appFilePath = String.Format("{0}App\\App.xml", configFolderPath);
                    XDocument doc = XDocument.Load(appFilePath);
                    retVal = App.Populate(doc);
                }
                


            }
            else
            {
                //load config from dll - default behaviour
                string binPath = null;

                if (HttpContext.Current != null)
                {
                    binPath = HttpContext.Current.Server.MapPath(@"~/bin");
                }
                else
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();

                    if (ApplicationDeployment.IsNetworkDeployed)
                    {

                        binPath = String.Format("{0}\\", Application.UserAppDataPath);
                    }
                    else
                    {
                        binPath = String.Format("{0}\\", System.IO.Path.GetDirectoryName(assembly.Location));
                    }


                }

                string ConfigResourceDLL = String.Format("{1}", binPath, ConfigurationManager.AppSettings["APPBUILDER_CONFIG_DLL"]);
                string ConfigNamespace = ConfigurationManager.AppSettings["APPBUILDER_CONFIG_DLL_NAMESPACE"];

                if ((String.IsNullOrEmpty(ConfigResourceDLL)) || (String.IsNullOrEmpty(ConfigNamespace)))
                {
                    throw new ApplicationException("CodeTorch configuration has not been configured correctly");
                }

                System.Reflection.Assembly configAssembly = System.Reflection.Assembly.Load(ConfigResourceDLL);
                string item = String.Format("{0}.{1}.{2}", ConfigNamespace, "App", "App.xml");

                using (Stream fileStream = configAssembly.GetManifestResourceStream(item))
                {
                    using (XmlTextReader xreader = new XmlTextReader(fileStream))
                    {
                        XDocument doc = XDocument.Load(xreader);
                        
                        retVal = App.Populate(doc);
                    }
                }

                
            }

            return retVal;
        }

        public static void LoadWebConfiguration()
        {
           
                //default load is from resource dll - this is method called from 
                string configMode = ConfigurationManager.AppSettings["APPBUILDER_CONFIG_MODE"];

                if ((!String.IsNullOrEmpty(configMode)) && (configMode.ToLower() == "folder"))
                {
                    //Load config from folder
                    LoadFromConfigurationFolder(ConfigurationManager.AppSettings["APPBUILDER_CONFIG_FOLDER"]);
                }
                else
                {
                    //load config from dll - default behaviour
                    string binPath = null;

                    if (HttpContext.Current != null)
                    {
                        binPath = HttpContext.Current.Server.MapPath(@"~/bin");
                    }
                    else
                    {
                        Assembly assembly = Assembly.GetExecutingAssembly();

                        if (ApplicationDeployment.IsNetworkDeployed)
                        {

                            binPath = String.Format("{0}\\", Application.UserAppDataPath);
                        }
                        else
                        {
                            binPath = String.Format("{0}\\", System.IO.Path.GetDirectoryName(assembly.Location));
                        }


                    }




                    LoadFromConfigurationDLL
                        (
                            String.Format("{1}", binPath, ConfigurationManager.AppSettings["APPBUILDER_CONFIG_DLL"]),
                            ConfigurationManager.AppSettings["APPBUILDER_CONFIG_DLL_NAMESPACE"]
                        );
                }

          
        }

        public static void LoadFromConfigurationFolder(string ConfigurationPath)
        {
            //by default attempt to load Configuration dll

            if (!String.IsNullOrEmpty(ConfigurationPath))
            {
                if (!ConfigurationPath.EndsWith("\\"))
                    ConfigurationPath += "\\";

                Configuration.GetInstance().ConfigurationPath = ConfigurationPath;

                LoadFromFilePath(ConfigurationPath);
            }


        }

        private static void LoadFromConfigurationDLL(string fileName, string configNamespace)
        {
            try
            {
                if ((String.IsNullOrEmpty(fileName)) || (String.IsNullOrEmpty(configNamespace)))
                {
                    throw new ApplicationException("CodeTorch configuration has not been configured correctly");
                }
                else
                {
                    LoadConfigObjects(null, fileName, configNamespace);
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException(fileName + ": " + e.Message);
            }
        }

        private static void LoadItems(string Folder, string TypeName, string ConfigPath, string ConfigResourceDLL, string ConfigNamespace)
        {
            if (ConfigPath != null)
            {
                LoadDirectoryItems(ConfigPath + Folder, TypeName);
            }
            else
            {
                LoadResourceItems(Folder, TypeName, ConfigResourceDLL, ConfigNamespace);
            }
        }

        private static void LoadItem(string Folder, string FileName, string TypeName, string ConfigPath, string ConfigResourceDLL, string ConfigNamespace)
        {
            if (ConfigPath != null)
            {
                LoadFile(TypeName, String.Format("{0}{1}\\{2}",ConfigPath,Folder,FileName));
            }
            else
            {
                System.Reflection.Assembly configAssembly = System.Reflection.Assembly.Load(ConfigResourceDLL);
                string item = String.Format("{0}.{1}.{2}", ConfigNamespace, Folder, FileName);
                LoadResourceItem(Folder, TypeName, ConfigNamespace, configAssembly, item);
            }
        }

        private static void LoadResourceItems(string Folder, string entityName, string ConfigResourceDLL, string ConfigNamespace)
        {
            IConfigurationObject config = ConfigurationObjectFactory.CreateConfigurationObject(entityName);
            config.ClearAll();

            System.Reflection.Assembly configAssembly = System.Reflection.Assembly.Load(ConfigResourceDLL);

            var retVal = from item in configAssembly.GetManifestResourceNames()
                         where 
                         (
                            (item.ToLower().StartsWith(String.Format("{0}.{1}.", ConfigNamespace.ToLower(), Folder.ToLower()))) &&
                            (item.ToLower().EndsWith(".xml"))
                         )
                         orderby item
                         select item
                         ;

            List<string> items = retVal.ToList<string>();

            foreach (string item in items)
            {
                LoadResourceItem(Folder, entityName, ConfigNamespace, configAssembly, item);
            }

           
        }

        private static void LoadResourceItem(string Folder, string TypeName, string ConfigNamespace, System.Reflection.Assembly configAssembly, string item)
        {
            string itemName = item.Substring(ConfigNamespace.Length + 1);
            string[] tokens = itemName.Split('.');

            using (Stream fileStream = configAssembly.GetManifestResourceStream(item))
            {
                using (XmlTextReader xreader = new XmlTextReader(fileStream))
                {
                    XDocument doc = XDocument.Load(xreader);

                    IConfigurationObject config = ConfigurationObjectFactory.CreateConfigurationObject(TypeName);

                    if (tokens.Length == 3)
                    {
                        //simple folder/child relationship
                        config.Load(doc, Folder);
                    }
                    else
                    {
                        config.Load(doc, tokens[1]);

                    }

                    
                    

                }
            }
        }

        
       

        private static void LoadConfigObjects(string ConfigFolderPath, string ConfigResourceDLL, string ConfigNamespace)
        {
            LoadItem("App", "App.xml", "App", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);

            LoadItems("SectionTypes", "SectionType", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);

            LoadItems("ControlTypes", "ControlType", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("ScreenTypes", "ScreenType", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            //LoadItems("DashboardComponents", "DashboardComponent", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            //LoadItems("DashboardComponentTypes", "DashboardComponentType", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("DataCommands", "DataCommand", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("DataConnections", "DataConnection", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("DataConnectionTypes", "DataConnectionType", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            //TODO: Email Templates
            //LoadItems("EmailTemplates", "EmailTemplate", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            
            //LoadItems("Groups", "Group", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("Lookups", "Lookup", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("Menus", "Menu", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("Permissions", "Permission", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("Pickers", "Picker", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("RestServices", "RestService", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("Screens", "Screen", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            
            
            LoadItems("Sequences", "Sequence", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("Workflows", "Workflow", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("WorkflowTypes", "WorkflowType", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("Templates", "Template", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("PageTemplates", "PageTemplate", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("SectionZoneLayouts", "SectionZoneLayout", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("DocumentRepositories", "DocumentRepository", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("DocumentRepositoryTypes", "DocumentRepositoryType", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("EmailConnections", "EmailConnection", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            LoadItems("EmailConnectionTypes", "EmailConnectionType", ConfigFolderPath, ConfigResourceDLL, ConfigNamespace);
            
            
        }
        

        private static void LoadFromFilePath(string ConfigPath)
        {
            LoadConfigObjects(ConfigPath, null, null);
        }

        private static void LoadDirectoryItems(string FolderPath, string TypeName)
        {
            if (Directory.Exists(FolderPath))
            {
                LoadDirectoryItems(FolderPath, TypeName, true);
            }
        }

        private static void LoadDirectoryItems(string FolderPath, string entityName, bool ClearCollection)
        {
         
                string[] files = Directory.GetFiles(FolderPath, "*.xml");

                string[] childfolders = Directory.GetDirectories(FolderPath);

                if (ClearCollection)
                {
                    IConfigurationObject config = ConfigurationObjectFactory.CreateConfigurationObject(entityName);
                    config.ClearAll();
                }

                foreach (string dir in childfolders)
                {
                    LoadDirectoryItems(dir, entityName, false);
                }

                foreach (string file in files)
                {
                    LoadFile(entityName, file);
                }
            
        }

        private static void LoadFile(string entityName, string file)
        {
            try
            {
                XDocument doc = XDocument.Load(file);
                string folder = Path.GetDirectoryName(file).Substring(Path.GetDirectoryName(file).LastIndexOf("\\") + 1);
                
                IConfigurationObject config = ConfigurationObjectFactory.CreateConfigurationObject(entityName);
                config.Load(doc,folder);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error loading {0} configuration file: {1}", entityName, file), ex);
            }
        }

       

        public static void SerializeObjectToFile(Object item, string filePath, Type[] extraTypes = null)
        {

            System.Xml.Serialization.XmlSerializer x = null;

            if (extraTypes == null)
            {
                x = new System.Xml.Serialization.XmlSerializer(item.GetType());
            }
            else
            {
                x = new System.Xml.Serialization.XmlSerializer(item.GetType(), extraTypes); 
            }
            
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.CloseOutput = true;
            settings.NewLineHandling = NewLineHandling.Entitize;

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                x.Serialize(writer, item);
                writer.Close();
            }
        }



        public static string GetFileConfigurationPath()
        {
            string ConfigPath = Configuration.GetInstance().ConfigurationPath;
            return ConfigPath;
        }


      
        public static void ReloadConfigurationItems(string ConfigurationFolder, string ConfigurationItemType)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();
            LoadDirectoryItems(ConfigPath + ConfigurationFolder, ConfigurationItemType);
        }


    }


    
}

