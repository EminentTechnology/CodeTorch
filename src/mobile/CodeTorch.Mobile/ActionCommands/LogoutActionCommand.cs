using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Mobile.Templates;
using System;
using System.Linq;
using Xamarin.Forms;

namespace CodeTorch.Mobile.ActionCommands
{
    public class LogoutActionCommand : IActionCommandStrategy
    {
        public Page Page { get; set; }
        public IMobilePage IPage { get; set; }

  
  

        public ActionCommand Command { get; set; }

        LogoutCommand Me = null;
        


        public void ExecuteCommand()
        {
            

           if (Command != null)
            {
                Me = (LogoutCommand)Command;
            }
            try
            {
                //clear login settings
                Common.RemoveSettings(Me.SettingsToRemove);

                //app start screen
                App app = Configuration.GetInstance().App;
                Page p = MobilePage.GetPage(app.DefaultScreen).GetPage();

                Page.Navigation.PushModalAsync(p);

            }
            catch (Exception ex)
            {
                IPage.DisplayErrorAlert(ex.Message);

                
            }
            
        }



        
    }
}
