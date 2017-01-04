using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace CodeTorch.Web.Backend
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            CodeTorch.Web.Common.LogException(ex);
        }
    }
}