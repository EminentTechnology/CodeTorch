using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class BaseValidator
    {
        public string Name { get; set; }
        public string ErrorMessage { get; set; }

        [Browsable(false)]
        [XmlIgnore()]
        public object Parent { get; set; }
    }
}
