﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeTorch.Core.Commands
{
    [Serializable]
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

   



        

    }
}
