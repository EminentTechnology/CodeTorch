using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    [XmlInclude(typeof(FormsAuthenticationMode))]
    [XmlInclude(typeof(WindowsAuthenticationMode))]
    public class BaseAuthenticationMode
    {

        [ReadOnly(true)]
        public virtual string Type { get; set; }

        

        public virtual bool Authenticate()
        {
            return false;
        }
    }
}
