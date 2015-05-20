using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Web.Templates
{
    public class DefaultAppPage: System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CodeTorch.Web.Common.RedirectToHomePage();
        }
    }
}
