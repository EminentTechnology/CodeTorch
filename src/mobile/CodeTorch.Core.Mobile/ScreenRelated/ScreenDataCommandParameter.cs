using System;
using System.Linq;
using System.Xml.Serialization;

namespace CodeTorch.Core
{

    public class ScreenDataCommandParameter : IScreenParameter
    {
        public ScreenDataCommandParameter()
        {

        }

        public ScreenDataCommandParameter(string Key, ScreenInputType Type)
        {
            InputKey = Key;
            InputType = Type;
        }

        public ScreenDataCommandParameter(string Name, object Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public ScreenDataCommandParameter(string Key, object Value, ScreenInputType Type)
        {
            InputKey = Key;
            InputType = Type;
            this.Value = Value;
        }


        public string Name { get; set; }

        public ScreenInputType InputType { get; set; }


        public string InputKey { get; set; }

        public string Default { get; set; }

        [XmlIgnore]
        public object Value { get; set; }

        public ScreenDataCommandParameter Clone()
        {
           return (ScreenDataCommandParameter)this.MemberwiseClone();
        }
        
    }
}
