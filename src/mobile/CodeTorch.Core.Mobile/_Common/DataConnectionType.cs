using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CodeTorch.Core
{

    public class DataConnectionType
    {
        private List<Setting> _Settings = new List<Setting>();
        private List<String> _CommandTypes = new List<String>();


        public string Name { get; set; }


        public string Assembly { get; set; }
        public string Class { get; set; }

        [XmlArray("CommandTypes")]
        [XmlArrayItem("CommandType")]
        public List<String> CommandTypes
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
        public List<Setting> Settings
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
                throw new Exception(String.Format("Error occurred while processing DataConnectionType - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            Configuration.GetInstance().DataConnectionTypes.Add(item);

            return item;

        }

        public static DataConnectionType Load(string Name)
        {

            DataConnectionType retVal = null;
            string item = String.Format("{0}.{1}.{2}.xml", ConfigurationLoader.ConfigAssemblyName, "DataConnectionTypes", Name);
            using (Stream fileStream = ConfigurationLoader.ConfigAssembly.GetManifestResourceStream(item))
            {
                using (XmlReader xreader = XmlReader.Create(fileStream))
                {
                    XDocument doc = XDocument.Load(xreader);
                    retVal = Load(doc);

                }
            }

            return retVal;

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

            if (item == null)
            { 
                //attempt to load screen from config
                item = Load(Name);
                
            }

            return item;
        }
    }
}
