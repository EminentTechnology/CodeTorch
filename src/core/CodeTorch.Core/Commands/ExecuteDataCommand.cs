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
    public class ExecuteDataCommand : ActionCommand
    {
        

       

        public override string Type
        {
            get
            {
                return "ExecuteDataCommand";
            }
            set
            {
                base.Type = value;
            }
        }

   
        public string DataCommand { get; set; }



        

    }
}
