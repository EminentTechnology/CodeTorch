using CodeTorch.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CodeTorch.Designer.Code
{
    public enum ProjectType
    { 
        Web
    }


    [Serializable]
    public class Project
    {
        public string Name { get; set; }
        public string RootNamespace { get; set; }
        public ProjectType ProjectType { get; set; }
        public string ConfigurationFolder { get; set; }

        public string DatabaseProjectFolder { get; set; }

        List<String> _OutputLocations = new List<string>();
        public List<String> OutputLocations
        {
            get { return _OutputLocations; }
            set { _OutputLocations = value; }
        }


        List<DataConnection> _Connections = new List<DataConnection>();
        public List<DataConnection> Connections
        {
            get { return _Connections; }
            set { _Connections = value; }
        }


        public static Project Load(string fileName)
        {
            Project item = null;
            XDocument doc = XDocument.Load(fileName);
           
            XmlSerializer serializer = new XmlSerializer(typeof(Project));
           
            if (serializer != null)
            {
                XmlReader reader = doc.CreateReader();
                reader.MoveToContent();

                

                try
                {

                    item = (Project)serializer.Deserialize(reader);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(String.Format("Error occurred while processing Screen - {0}", doc.Root.FirstNode.ToString()), ex);
                }


            }

            return item;

        }

        public DataConnection GetDataConnection(DataCommand command)
        {
            DataConnection retVal = null;

            if (!String.IsNullOrEmpty(command.DataConnection))
            {
                retVal = this.Connections.Where(x => x.Name == command.DataConnection).SingleOrDefault<DataConnection>();
            }
            else
            {
                if (!String.IsNullOrEmpty(Configuration.GetInstance().App.DefaultConnection))
                {
                    retVal = this.Connections.Where(x => x.Name == Configuration.GetInstance().App.DefaultConnection).SingleOrDefault<DataConnection>();
                }
            }

            return retVal;
        }

        public DataConnection GetDefaultDataConnection()
        {
            DataConnection retVal = null;

            if (!String.IsNullOrEmpty(Configuration.GetInstance().App.DefaultConnection))
                {
                    retVal = this.Connections.Where(x => x.Name == Configuration.GetInstance().App.DefaultConnection).SingleOrDefault<DataConnection>();
                }

            return retVal;
        }
    }
}
