using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CodeTorch.Core
{

    public class DataCommand
    {
        string _Name = null;
        string _Text = null;

        List<DataCommandParameter> _parameters = new List<DataCommandParameter>();
        List<DataCommandColumn> _columns = new List<DataCommandColumn>();




        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {

                _Name = value;

            }
        }


        public string DataConnection {get;set;}


        public string Type { get; set; }
        

        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {

                _Text = value;
            }
        }
        public DataCommandReturnType ReturnType { get; set; }

        [XmlArray("Parameters")]
        [XmlArrayItem("Parameter")]
        public List<DataCommandParameter> Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                _parameters = value;
            }
        }

        [XmlArray("Columns")]
        [XmlArrayItem("Column")]
        public List<DataCommandColumn> Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
            }

        }


        public string PreProcessingAssembly { get; set; }
        

        public string PreProcessingClass { get; set; }
        

        public string PostProcessingAssembly { get; set; }
        

        public string PostProcessingClass { get; set; }

        public static DataCommand Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DataCommand));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            DataCommand command = (DataCommand)serializer.Deserialize(reader);

            Configuration.GetInstance().DataCommands.Add(command);

            return command;

        }

        public static DataCommand Load(string Name)
        {

            DataCommand retVal = null;
            string item = String.Format("{0}.{1}.{2}.xml", ConfigurationLoader.ConfigAssemblyName, "DataCommands", Name);
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



        public static DataCommand GetDataCommand(string DataCommandName)
        {
            DataCommand command = null;

            try
            {
                command = Configuration.GetInstance().DataCommands
                                .Where(d =>
                                    (
                                        (d.Name.ToLower() == DataCommandName.ToLower())
                                    )
                                )
                                .SingleOrDefault();

                if (command == null)
                { 
                    //attempt to load screen from config
                    command = Load(DataCommandName);
                
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().StartsWith("sequence contains more than one element"))
                {
                    throw new Exception(String.Format("Error - Found multiple data commands with the same name in configuration - {0}", DataCommandName));
                }
                else
                {
                    throw ex;
                }
            }

            return command;
        }



        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().DataCommands.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().DataCommands
                                .Where(i =>
                                    (
                                        (i.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public DataConnection GetDataConnection()
        {
            DataConnection retVal = null;

            if (!String.IsNullOrEmpty(this.DataConnection))
            {
                retVal = CodeTorch.Core.DataConnection.GetByName(this.DataConnection);
            }
            else
            { 
                if(!String.IsNullOrEmpty(Configuration.GetInstance().App.DefaultConnection))
                {
                    retVal = CodeTorch.Core.DataConnection.GetByName(Configuration.GetInstance().App.DefaultConnection);
                }
            }

            return retVal;
        }
    }
}
