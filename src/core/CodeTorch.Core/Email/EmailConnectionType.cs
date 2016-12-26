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
    public class EmailConnectionType
    {
        

        [Category("Common")]
        [ReadOnly(true)]
        public string Name { get; set; }


        public string Assembly { get; set; }
        public string Class { get; set; }



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

        public static EmailConnectionType Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EmailConnectionType));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            EmailConnectionType item = null;

            try
            {
                item = (EmailConnectionType)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing EmailConnectionType - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            return item;

        }

        public static void Save(EmailConnectionType item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}EmailConnectionTypes", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}EmailConnectionTypes", ConfigPath));
            }

            string filePath = String.Format("{0}EmailConnectionTypes\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().EmailConnectionTypes.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().EmailConnectionTypes
                                .Where(c =>
                                    (
                                        (c.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static EmailConnectionType GetByName(string Name)
        {
            EmailConnectionType item = Configuration.GetInstance().EmailConnectionTypes
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
