using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CodeTorch.Core
{

    public class DataConnection
    {
        private List<Setting> _Settings = new List<Setting>();


        public string Name { get; set; }


        public string DataConnectionType { get; set; }

        List<Setting> _settings = new List<Setting>();

        [XmlArray("Settings")]
        [XmlArrayItem("Setting")]
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
                throw new Exception(String.Format("Error occurred while processing DataConnection - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            Configuration.GetInstance().DataConnections.Add(item);

            return item;

        }

        public static DataConnection Load(string Name)
        {

            DataConnection retVal = null;
            string item = String.Format("{0}.{1}.{2}.xml", ConfigurationLoader.ConfigAssemblyName, "DataConnections", Name);
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

            if (item == null)
            { 
                //attempt to load screen from config
                item = Load(Name);
                
            }

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
