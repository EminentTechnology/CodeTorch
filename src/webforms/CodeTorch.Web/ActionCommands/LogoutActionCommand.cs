using CodeTorch.Abstractions;
using CodeTorch.Core;
using CodeTorch.Core.Commands;
using System;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CodeTorch.Web.ActionCommands
{
    public class LogoutActionCommand : IActionCommandStrategy
    {
        public Templates.BasePage Page { get; set; }

  

        public ActionCommand Command { get; set; }

        LogoutCommand Me = null;
        


        public bool ExecuteCommand()
        {
            bool success = true;
            ILog log = Resolver.Resolve<ILogManager>().GetLogger(this.GetType());

            try
            {
                Page.Session.Abandon();
                FormsAuthentication.SignOut();
                Page.Session.Clear();

                

                if (String.IsNullOrEmpty(Page.Request.QueryString["RedirUrl"]))
                {
                    Page.Response.Redirect("~/default.aspx", false);
                }
                else
                {
                    Page.Response.Redirect(Page.Request.QueryString["RedirUrl"], false);
                }
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                success = false;
                Page.DisplayErrorAlert(ex);

                log.Error(ex);
            }

            return success;

        }



        
    }
}
