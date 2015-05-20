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
    public class RedirectCommand : ActionCommand
    {

        public enum RedirectModeEnum
        {
            Constant,
            DataCommand,
            Referrer
        }
       

        public override string Type
        {
            get
            {
                return "RedirectCommand";
            }
            set
            {
                base.Type = value;
            }
        }


        

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string DataCommand { get; set; }


        [Category("Redirect")]
        public string RedirectUrl { get; set; }

        [Category("Redirect")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string RedirectUrlField { get; set; }

        [Category("Redirect")]
        public RedirectModeEnum RedirectMode { get; set; }


        

    }
}
