﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace CodeTorch.Core.Commands
{
#if !( PORTABLE)
    [Serializable]
#endif
    public class SetScreenObjectsDataCommand : ActionCommand
    {
        

       

        public override string Type
        {
            get
            {
                return "SetScreenObjectsDataCommand";
            }
            set
            {
                base.Type = value;
            }
        }

   
        public string DataCommand { get; set; }



        

    }
}
