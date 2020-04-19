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
    public class DataConnectionType
    {
        private StringCollection _Settings = new StringCollection();
        private StringCollection _CommandTypes = new StringCollection();

        [Category("Common")]
        [ReadOnly(true)]
        public string Name { get; set; }


        public string Assembly { get; set; }
        public string Class { get; set; }

        [XmlArray("CommandTypes")]
        [XmlArrayItem("CommandType")]
#if NETFRAMEWORK
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
#endif
        public StringCollection CommandTypes
        {
            get
            {
                return _CommandTypes;
            }
            set
            {
                _CommandTypes = value;
            }

        }

        [XmlArray("Settings")]
        [XmlArrayItem("Setting")]
#if NETFRAMEWORK
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
#endif
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

        public static DataConnectionType Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DataConnectionType));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            DataConnectionType item = null;

            try
            {
                item = (DataConnectionType)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing DataConnectionType - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            return item;

        }

        public static void Save(DataConnectionType item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}DataConnectionTypes", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}DataConnectionTypes", ConfigPath));
            }

            string filePath = String.Format("{0}DataConnectionTypes\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().DataConnectionTypes.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().DataConnectionTypes
                                .Where(c =>
                                    (
                                        (c.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static DataConnectionType GetByName(string Name)
        {
            DataConnectionType item = Configuration.GetInstance().DataConnectionTypes
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
