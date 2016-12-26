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
    public class DataConnection
    {
        private StringCollection _Settings = new StringCollection();

        [Category("Common")]
        [ReadOnly(true)]
        public string Name { get; set; }

        [TypeConverter("CodeTorch.Core.Design.DataConnectionTypeTypeConverter,CodeTorch.Core.Design")]
        public string DataConnectionType { get; set; }

        List<Setting> _settings = new List<Setting>();

        [XmlArray("Settings")]
        [XmlArrayItem("Setting")]
        [Description("List of data connections")]
        [Category("Settings")]
        public  List<Setting> Settings
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

        public static DataConnection Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DataConnection));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            DataConnection item = null;

            try
            {
                item = (DataConnection)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing DataConnection - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            return item;

        }

        public static void Save(DataConnection item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}DataConnections", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}DataConnections", ConfigPath));
            }

            string filePath = String.Format("{0}DataConnections\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().DataConnections.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().DataConnections
                                .Where(c =>
                                    (
                                        (c.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static DataConnection GetByName(string Name)
        {
            DataConnection item = Configuration.GetInstance().DataConnections
                            .Where(i =>
                                (
                                    (i.Name.ToLower() == Name.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return item;
        }

        public DataConnectionType GetConnectionType()
        {
            DataConnectionType retVal = null;

            if (!String.IsNullOrEmpty(this.DataConnectionType))
            {
                retVal = CodeTorch.Core.DataConnectionType.GetByName(this.DataConnectionType);
            }

            return retVal;
        }
    }
}
