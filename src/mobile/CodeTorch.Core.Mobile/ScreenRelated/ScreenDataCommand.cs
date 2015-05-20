using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace CodeTorch.Core
{

    public class ScreenDataCommand
    {
        public string Name { get; set; }

        List<ScreenDataCommandParameter> _parameters = new List<ScreenDataCommandParameter>();

        [XmlArray("Parameters")]
        [XmlArrayItem("Parameter")]
        public List<ScreenDataCommandParameter> Parameters
        {
            get
            {
                return _parameters;
            }

        }

        public override string ToString()
        {
            return Name;
        }

        public ScreenDataCommand()
        { }

        public ScreenDataCommand(string Name)
        {
            this.Name = Name;
        }

        public static ScreenDataCommand GetDataCommand(List<ScreenDataCommand> datacommands, string DataCommandName)
        {
            ScreenDataCommand command = datacommands
                            .Where(d =>
                                (
                                    (d.Name.ToLower() == DataCommandName.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return command;
        }

    }

   
}


