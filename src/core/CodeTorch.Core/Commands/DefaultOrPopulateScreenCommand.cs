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
    public class DefaultOrPopulateScreenCommand : ActionCommand
    {

        ScreenInputType _EntityInputType = ScreenInputType.QueryString;
       

        public override string Type
        {
            get
            {
                return "DefaultOrPopulateScreenCommand";
            }
            set
            {
                base.Type = value;
            }
        }


        [Category("Entity")]
        public string EntityID { get; set; }



        [Category("Entity")]
        public ScreenInputType EntityInputType
        {
            get { return _EntityInputType; }
            set { _EntityInputType = value; }
        }

        

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string DefaultCommand { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string RetrieveCommand { get; set; }

        public bool ExecuteOnPostBack { get; set; }

    }
}
