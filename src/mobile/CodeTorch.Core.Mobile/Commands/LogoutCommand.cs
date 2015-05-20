using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;



namespace CodeTorch.Core.Commands
{

    public class LogoutCommand : ActionCommand
    {

        
       

        public override string Type
        {
            get
            {
                return "LogoutCommand";
            }
            set
            {
                base.Type = value;
            }
        }

   

        public string SettingsToRemove { get; set; }

        

    }
}
