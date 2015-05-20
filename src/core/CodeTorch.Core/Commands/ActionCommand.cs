using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    [Serializable]
    public class ActionCommand
    {
        public string Name { get; set; }
        public virtual string Type { get; set; }

        

        public virtual void Execute()
        { 
        }

        
    }
}
