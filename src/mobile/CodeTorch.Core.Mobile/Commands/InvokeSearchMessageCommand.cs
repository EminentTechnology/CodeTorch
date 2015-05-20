using System;
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
    public class InvokeSearchMessageCommand : ActionCommand
    {
        

       

        public override string Type
        {
            get
            {
                return "InvokeSearchMessageCommand";
            }
            set
            {
                base.Type = value;
            }
        }

   



        

    }
}
