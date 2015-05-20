using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class Setting
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Setting()
        { 
        
        }

        public Setting(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

    }
}
