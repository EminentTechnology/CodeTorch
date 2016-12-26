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
using CodeTorch.Abstractions;
using System.Threading.Tasks;

namespace CodeTorch.Core
{

    public class ConfigurationLoader
    {

        public static IConfigurationStore Store { get; set; }

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


        public async static Task LoadConfiguration()
        {
            var config = Configuration.GetInstance();

            if (Store == null)
                throw new ArgumentNullException("Store");
            
            //load the special app object
            var app = await Store.GetItem<App>("App");
            config.App = app;


            //load all the other objects
            
            await LoadItems<SectionType>(config, config.SectionTypes);
            await LoadItems<ScreenType>(config, config.ScreenTypes);
            await LoadItems<Lookup>(config, config.Lookups);
            await LoadItems<ControlType>(config, config.ControlTypes);

            await LoadItems<DataCommand>(config, config.DataCommands);
            await LoadItems<DataConnection>(config, config.DataConnections);
            await LoadItems<DataConnectionType>(config, config.DataConnectionTypes);
            await LoadItems<Menu>(config, config.Menus);
            await LoadItems<Permission>(config, config.Permissions);

            await LoadItems<Picker>(config, config.Pickers);
            await LoadItems<RestService>(config, config.RestServices);
            await LoadItems<Screen>(config, config.Screens);
            await LoadItems<Sequence>(config, config.Sequences);
            await LoadItems<Workflow>(config, config.Workflows);
            await LoadItems<WorkflowType>(config, config.WorkflowTypes);


            await LoadItems<PageTemplate>(config, config.PageTemplates);
            await LoadItems<SectionZoneLayout>(config, config.SectionZoneLayouts);
            await LoadItems<DocumentRepository>(config, config.DocumentRepositories);
            await LoadItems<DocumentRepositoryType>(config, config.DocumentRepositoryTypes);
            await LoadItems<EmailConnection>(config, config.EmailConnections);
            await LoadItems<EmailConnectionType>(config, config.EmailConnectionTypes);


        }

        static async Task LoadItems<T>(Configuration config, List<T> data)
        {
            
            var items = await Store.GetItems<T>();
            data.AddRange(items);
        }

        [Obsolete("LoadWebConfiguration is deprecated, please use LoadConfiguration instead.")]
        public static void LoadWebConfiguration()
        {
            LoadConfiguration();
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
            throw new NotImplementedException();
        }

       
        

       

       
        

       

        private static void LoadDirectoryItems(string FolderPath, string entityName, bool ClearCollection)
        {
         
                string[] files = Directory.GetFiles(FolderPath, "*.xml");

                string[] childfolders = Directory.GetDirectories(FolderPath);

                if (ClearCollection)
                {
                    IConfigurationObject2 config = ConfigurationObjectFactory.CreateConfigurationObject(entityName);
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
                
                IConfigurationObject2 config = ConfigurationObjectFactory.CreateConfigurationObject(entityName);
                config.Load(doc,folder);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error loading {0} configuration file: {1}", entityName, file), ex);
            }
        }

        public static void LoadFromConfigurationFolder(string configurationPath)
        {
            throw new NotImplementedException();
        }
    }


    
}

