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
    public class WorkflowType
    {
        private StringCollection _Settings = new StringCollection();

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

        public static WorkflowType Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(WorkflowType));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            WorkflowType item = null;

            try
            {
                item = (WorkflowType)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing WorkflowType - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            return item;

        }

        public static void Save(WorkflowType item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}WorkflowTypes", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}WorkflowTypes", ConfigPath));
            }

            string filePath = String.Format("{0}WorkflowTypes\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().WorkflowTypes.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().WorkflowTypes
                                .Where(c =>
                                    (
                                        (c.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static WorkflowType GetByName(string Name)
        {
            WorkflowType item = Configuration.GetInstance().WorkflowTypes
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
