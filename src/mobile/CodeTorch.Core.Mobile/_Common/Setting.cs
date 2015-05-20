using System;
using System.Linq;

namespace CodeTorch.Core
{

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
