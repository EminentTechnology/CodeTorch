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
    public class DocumentRepository
    {
        private StringCollection _Settings = new StringCollection();

        [Category("Common")]
        [ReadOnly(true)]
        public string Name { get; set; }


        [TypeConverter("CodeTorch.Core.Design.DocumentRepositoryTypeTypeConverter,CodeTorch.Core.Design")]
        public string DocumentRepositoryType { get; set; }

        List<Setting> _settings = new List<Setting>();

        [XmlArray("Settings")]
        [XmlArrayItem("Setting")]
        [Description("List of repository settings")]
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

        public static void Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DocumentRepository));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            DocumentRepository item = null;

            try
            {
                item = (DocumentRepository)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing DocumentRepository - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            Configuration.GetInstance().DocumentRepositories.Add(item);

        }

        public static void Save(DocumentRepository item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}DocumentRepositories", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}DocumentRepositories", ConfigPath));
            }

            string filePath = String.Format("{0}DocumentRepositories\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().DocumentRepositories.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().DocumentRepositories
                                .Where(c =>
                                    (
                                        (c.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static DocumentRepository GetByName(string Name)
        {
            DocumentRepository item = Configuration.GetInstance().DocumentRepositories
                            .Where(i =>
                                (
                                    (i.Name.ToLower() == Name.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return item;
        }

        public DocumentRepositoryType GetRepositoryType()
        {
            DocumentRepositoryType retVal = null;

            if (!String.IsNullOrEmpty(this.DocumentRepositoryType))
            {
                retVal = CodeTorch.Core.DocumentRepositoryType.GetByName(this.DocumentRepositoryType);
            }

            return retVal;
        }
    }
}
