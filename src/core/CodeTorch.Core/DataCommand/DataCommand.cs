using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing.Design;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace CodeTorch.Core
{
    [Serializable]
    public class DataCommand
    {
        string _Name = null;
        string _Text = null;

        List<DataCommandParameter> _parameters = new List<DataCommandParameter>();
        List<DataCommandColumn> _columns = new List<DataCommandColumn>();




        [Category("General")]
        [Description("The name of this data command")]
        [ReadOnly(true)]
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

        [Category("General")]
        [DisplayName("Connection")]
        [Description("The connection this data command will bind to")]
        [TypeConverter("CodeTorch.Core.Design.DataConnectionTypeConverter,CodeTorch.Core.Design")]
        public string DataConnection {get;set;}

        [Category("General")]
        [DisplayName("Type")]
        [Description("The type of command")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandCommandTypeTypeConverter,CodeTorch.Core.Design")]
        public string Type { get; set; }
        
        [Category("General")]
        [DisplayName("Text")]
        [Description("The text of the command - typically sql statement or db stored procedure name")]
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
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
        [Category("Parameters")]
        [Description("List of parameters used by this command")]
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
        [Category("Resultset")]
        [Description("List of columns returned in resultset")]
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

        [Category("Processing Hooks")]
        public string PreProcessingAssembly { get; set; }
        
        [Category("Processing Hooks")]
        public string PreProcessingClass { get; set; }
        
        [Category("Processing Hooks")]
        public string PostProcessingAssembly { get; set; }
        
        [Category("Processing Hooks")]
        public string PostProcessingClass { get; set; }

        public static void Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DataCommand));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            DataCommand command = (DataCommand)serializer.Deserialize(reader);

            Configuration.GetInstance().DataCommands.Add(command);

        }



        public static void Save(DataCommand item)
        {

            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();
            XmlSerializer x = new XmlSerializer(item.GetType());

            if (!Directory.Exists(String.Format("{0}DataCommands", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}DataCommands", ConfigPath));
            }

            string filePath = String.Format("{0}DataCommands\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

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
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().StartsWith("sequence contains more than one element"))
                {
                    throw new ApplicationException(String.Format("Error - Found multiple data commands with the same name in configuration - {0}", DataCommandName));
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
