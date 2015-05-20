using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core.Attributes
{

    [AttributeUsage(AttributeTargets.All)]
    public class PropertyValueChangedCommandAttribute : Attribute
    {
        public string PropertyName { get; set; }
        public string Command { get; set; }

        public PropertyValueChangedCommandAttribute(string PropertyName, string Command)
        {
            this.PropertyName = PropertyName;
            this.Command = Command;
        }
    }
}
