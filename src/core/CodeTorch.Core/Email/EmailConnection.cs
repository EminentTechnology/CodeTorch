using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    [Serializable]
    public class EmailConnection
    {
        

        [Category("Common")]
        [ReadOnly(true)]
        public string Name { get; set; }

        [TypeConverter("CodeTorch.Core.Design.EmailConnectionTypeTypeConverter,CodeTorch.Core.Design")]
        public string EmailConnectionType { get; set; }

        

        List<Setting> _settings = new List<Setting>();

        [XmlArray("Settings")]
        [XmlArrayItem("Setting")]
        public List<Setting> Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                _settings = value;
            }
        }

        public static void Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EmailConnection));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            EmailConnection item = null;

            try
            {
                item = (EmailConnection)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing EmailConnection - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            Configuration.GetInstance().EmailConnections.Add(item);

        }

        public static void Save(EmailConnection item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}EmailConnections", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}EmailConnections", ConfigPath));
            }

            string filePath = String.Format("{0}EmailConnections\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().EmailConnections.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().EmailConnections
                                .Where(c =>
                                    (
                                        (c.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static EmailConnection GetByName(string Name)
        {
            EmailConnection item = Configuration.GetInstance().EmailConnections
                            .Where(i =>
                                (
                                    (i.Name.ToLower() == Name.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return item;
        }

        public EmailConnectionType GetConnectionType()
        {
            EmailConnectionType retVal = null;

            if (!String.IsNullOrEmpty(this.EmailConnectionType))
            {
                retVal = CodeTorch.Core.EmailConnectionType.GetByName(this.EmailConnectionType);
            }

            return retVal;
        }
    }
}
