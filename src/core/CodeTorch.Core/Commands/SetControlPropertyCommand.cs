using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace CodeTorch.Core.Commands
{
    [Serializable]
    public class SetControlPropertyCommand : ActionCommand
    {
        

       

        public override string Type
        {
            get
            {
                return "SetControlPropertyCommand";
            }
            set
            {
                base.Type = value;
            }
        }

   
        public string ControlName { get; set; }
        public string PropertyName { get; set; }

        public string PropertyValue { get; set; }



        

    }
}
