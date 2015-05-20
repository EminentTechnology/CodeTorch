using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Mobile;
using CodeTorch.Mobile.Templates;
using System;
using System.Linq;
using Xamarin.Forms;


namespace CodeTorch.Mobile.ActionCommands
{
    public class NavigateToScreenActionCommand : IActionCommandStrategy
    {
        public Page Page { get; set; }
        public IMobilePage IPage { get; set; }

  

        public ActionCommand Command { get; set; }

        NavigateToScreenCommand Me = null;
        


        public void ExecuteCommand()
        {
            if (Command != null)
            {
                Me = (NavigateToScreenCommand)Command;
            }

            //log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                Page p = MobilePage.GetPage(Me.NavigateScreen).GetPage();

                Page.Navigation.PushAsync(p);

                //if (Command != null)
                //{
                //    Me = (NavigateToUrlCommand)Command;
                //}

                //if (String.IsNullOrEmpty(Me.NavigateUrl))
                //{
                //    throw new ApplicationException("NavigateUrl is invalid");
                //}
                //string url = Common.CreateUrlWithQueryStringContext(Me.NavigateUrl, Me.Context);
                //url = String.Format(url, Page.Request.QueryString[Me.EntityID]);

                //log.DebugFormat("RedirectUrl:{0}", url);
                //Page.Response.Redirect(url,false);
                //HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                //Page.DisplayErrorAlert(ex);

                //log.Error(ex);
            }
            
        }



        
    }
}
