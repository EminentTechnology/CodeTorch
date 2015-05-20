using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    [Serializable]
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

        [ReadOnly(true)]
        public string Name { get; set; }

        public ScreenInputType InputType { get; set; }

        [TypeConverter("CodeTorch.Core.Design.InputKeyTypeConverter,CodeTorch.Core.Design")]
        public string InputKey { get; set; }

        public string Default { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public object Value { get; set; }

        
    }
}
