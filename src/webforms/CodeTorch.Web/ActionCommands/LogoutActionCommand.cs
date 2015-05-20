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
        


        public void ExecuteCommand()
        {
            

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                Page.DisplayErrorAlert(ex);

                log.Error(ex);
            }
            
        }



        
    }
}
