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
    public class DocumentRepositoryType
    {
        private StringCollection _Settings = new StringCollection();
        
        [Category("Common")]
        [ReadOnly(true)]
        public string Name { get; set; }


        public string Assembly { get; set; }
        public string Class { get; set; }

        

        [XmlArray("Settings")]
        [XmlArrayItem("Setting")]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        public StringCollection Settings
        {
            get
            {
                return _Settings;
            }
            set
            {
                _Settings = value;
            }

        }

        public static void Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DocumentRepositoryType));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            DocumentRepositoryType item = null;

            try
            {
                item = (DocumentRepositoryType)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing DocumentRepositoryType - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            Configuration.GetInstance().DocumentRepositoryTypes.Add(item);

        }

        public static void Save(DocumentRepositoryType item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}DocumentRepositoryTypes", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}DocumentRepositoryTypes", ConfigPath));
            }

            string filePath = String.Format("{0}DocumentRepositoryTypes\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().DocumentRepositoryTypes.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().DocumentRepositoryTypes
                                .Where(c =>
                                    (
                                        (c.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static DocumentRepositoryType GetByName(string Name)
        {
            DocumentRepositoryType item = Configuration.GetInstance().DocumentRepositoryTypes
                            .Where(i =>
                                (
                                    (i.Name.ToLower() == Name.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return item;
        }
    }
}
