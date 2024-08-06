using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public enum DateMode
    { 
        LocalDate,
        UniversalDate
    }

    [Serializable]
    public enum RestServiceResponseMode
    { 
        Default,
        Simple,
        IncludeMetaAndError
    }

    [Serializable]
    public class App
    {
        private int schemaVersion = 1;
        private List<ServiceLogProviderItem> _serviceLogProviders = new List<ServiceLogProviderItem>();

        public int SchemaVersion 
        { 
            get
            {
                return schemaVersion;
            } 
            set 
            {
                schemaVersion = value;
            } 
        }

        public bool SupportExtensionlessScreens { get; set; }
        public string Name { get; set; }
        public string CopyrightCompanyName { get; set; }
        public string DefaultScreen { get; set; }
        public string LoginScreen { get; set; }
        public string DefaultPageTemplate { get; set; }
        public string DefaultConnection { get; set; }
        public string DefaultMenu { get; set; }

        public string DefaultErrorMessageFormatString { get; set; }
        public string DefaultZoneLayout { get; set; }
        public string AdminRole { get; set; }
        public string DevelopmentBaseUrl { get; set; }
        public string DashboardPageTemplate { get; set; }
        public bool EnableLocalization { get; set; }
        public DateMode DateMode { get; set; }

        public RestServiceResponseMode RestServiceResponseMode { get; set; }

        public string SaveSequenceDataCommand { get; set; }

        bool _EnableLicensingForWeb = true;

        public bool EnableLicensingForWeb
        {
            get { return _EnableLicensingForWeb; }
            set { _EnableLicensingForWeb = value; }

        }

        public string LookupProviderAssembly { get; set; }
        public string LookupProviderClass { get; set; }
        public string LookupProviderConfig { get; set; }

        public string AuthorizationProviderAssembly { get; set; }
        public string AuthorizationProviderClass { get; set; }
        public string AuthorizationProviderConfig { get; set; }

        public string UserIdentityProviderAssembly { get; set; }
        public string UserIdentityProviderClass { get; set; }
        public string UserIdentityProviderConfig { get; set; }

        public string ResourceProviderAssembly { get; set; }
        public string ResourceProviderClass { get; set; }
        public string ResourceProviderConfig { get; set; }

        

        [XmlArray("ServiceLogProviders")]
        [XmlArrayItem("ServiceLogProvider")]
        public List<ServiceLogProviderItem> ServiceLogProviders
        {
            get
            {
                return _serviceLogProviders;
            }
            set
            {
                _serviceLogProviders = value;
            }
        }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string LocalizationCommand { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string LocalizationCultureField { get; set; }
        public string LocalizationDefaultCulture { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string LocalizationUserNameParameter { get; set; }

        BaseAuthenticationMode _AuthenticationMode = new FormsAuthenticationMode();


        [XmlElement(ElementName = "WindowsAuthenticationMode", Type = typeof(WindowsAuthenticationMode))]
        [XmlElement(ElementName = "FormsAuthenticationMode", Type = typeof(FormsAuthenticationMode))]
        public BaseAuthenticationMode AuthenticationMode
        {
            get { return _AuthenticationMode; }
            set { _AuthenticationMode = value; }
        }

        

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string ProfileCommand { get; set; }

        [XmlArray("Profile")]
        [XmlArrayItem("Property")]
        public List<String> ProfileProperties;

        public static void Load(XDocument doc)
        {
            App item = Populate(doc);

            Configuration.GetInstance().App = item;

          

        }

        public static App Populate(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(App));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            App item = null;

            try
            {
                item = (App)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing App - {0}", doc.Root.FirstNode.ToString()), ex);
            }
            return item;
        }

        public static void Save(App item)
        {

            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();
            XmlSerializer x = new XmlSerializer(item.GetType());

            if (!Directory.Exists(String.Format("{0}App", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}App", ConfigPath));
            }

            string filePath = String.Format("{0}App\\App.xml", ConfigPath);

            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }


        internal static int GetItemCount()
        {
            return 1;
        }

        public string GetDefaultValue(string key)
        {
            string retVal = null;

            if(this.Defaults != null)
            {
                AppDefaultItem item = this.Defaults.FirstOrDefault(i => i.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
                if(item != null)
                {
                    retVal = item.Value;
                }
            }

            return retVal;
        }

        [XmlArray("Defaults")]
        [XmlArrayItem("Item")]
        public List<AppDefaultItem> Defaults { get; set; } = new List<AppDefaultItem>();
    }

    [Serializable]
    public class  AppDefaultItem
    {
        [XmlAttribute("Key")]
        public string Key { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    [Serializable]
    public class ServiceLogProviderItem
    {
        public string Assembly { get; set; }
        public string Class { get; set; }
        public string Config { get; set; }
    }
}
