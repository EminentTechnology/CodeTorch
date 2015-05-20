using System;

using System.Linq;


namespace CodeTorch.Core
{

    public class ActionCommand
    {
        public string Name { get; set; }
        public virtual string Type { get; set; }

        public virtual void Execute()
        { 
        }

        
    }
}
