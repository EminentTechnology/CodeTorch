using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeTorch.Web.Data;
using System.Web.Security;
using System.Management;

namespace CodeTorch.Web.Templates
{
    public class PageNotFound : BasePage
    {
        public override bool PerformScreenAuthenticationCheck()
        {
            return false;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            this.Title = "Page Not Found";
            this.SubTitle = "";

            
        }
    }
}
