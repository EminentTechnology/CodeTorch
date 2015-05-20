using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace CodeTorch.Web
{
    public class CustomDataCommandValidator: CustomValidator
    {

        public string DataCommand
        {
            get { return (ViewState["DataCommand"]) == null ? null : ViewState["DataCommand"].ToString(); }
            set { ViewState["DataCommand"] = value; }
        }

        

        public string ValidationField
        {
            get { return (ViewState["ValidationField"]) == null ? null : ViewState["ValidationField"].ToString(); }
            set { ViewState["ValidationField"] = value; }
        }
       

        public bool UseValueParameter
        {
            get { return ((ViewState["UseValueParameter"] == null) ? false : Convert.ToBoolean(ViewState["UseValueParameter"])) ; }
            set { ViewState["UseValueParameter"] = value; }
        }
        

        public string ValueParameter
        {
            get { return (ViewState["ValueParameter"]) == null ? null : ViewState["ValueParameter"].ToString(); }
            set { ViewState["ValueParameter"] = value; }
        }
        

        public bool UseErrorMessageField
        {
            get { return ((ViewState["UseErrorMessageField"] == null) ? false : Convert.ToBoolean(ViewState["UseErrorMessageField"])); }
            set { ViewState["UseErrorMessageField"] = value; }
        }
        

        public string ErrorMessageField
        {
            get { return (ViewState["ErrorMessageField"]) == null ? null : ViewState["ErrorMessageField"].ToString(); }
            set { ViewState["ErrorMessageField"] = value; }
        }
    }
}
