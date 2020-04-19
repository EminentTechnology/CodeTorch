using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeTorch.Core.Commands
{
    [Serializable]
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
