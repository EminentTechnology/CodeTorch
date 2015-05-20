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
    public class NavigateToUrlCommand : ActionCommand
    {
        

       

        public override string Type
        {
            get
            {
                return "NavigateToUrlCommand";
            }
            set
            {
                base.Type = value;
            }
        }

   
        public string NavigateUrl { get; set; }
        public string Context { get; set; }

        public string EntityID { get; set; }



        

    }
}
