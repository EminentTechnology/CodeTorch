using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Core.Services;
using CodeTorch.Mobile.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;


namespace CodeTorch.Mobile.ActionCommands
{
    public class ValidateUserActionCommand : IActionCommandStrategy
    {
        public Page Page { get; set; }
        public IMobilePage IPage { get; set; }

  

        public ActionCommand Command { get; set; }

        ValidateUserCommand Me = null;

       

        public async void ExecuteCommand()
        {
            if (Command != null)
            {
                Me = (ValidateUserCommand)Command;
            }
            try
            {

                DataTable dt = null;
                DataCommandService dataCommandDB = DataCommandService.GetInstance();
                bool IsAuthenticated = false;

                //call data command

                //if we get datatable and row=1 store 

                List<ScreenDataCommandParameter> parameters = null;
                parameters = Common.GetPopulatedCommandParameters(Me.LoginCommand, IPage);

                DataCommand command = DataCommand.GetDataCommand(Me.LoginCommand);

                switch (command.ReturnType)
                {
                    case DataCommandReturnType.DataTable:
                        dt = await dataCommandDB.GetDataForDataCommand(Me.LoginCommand, parameters);
                        break;
                    case DataCommandReturnType.Integer:
                        throw new NotImplementedException();
                        break;
                    case DataCommandReturnType.Xml:
                        throw new NotImplementedException();
                        break;
                    case DataCommandReturnType.None:
                        throw new NotImplementedException();
                        break;
                }

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        Common.DataRowToSettings(row, Me.AfterLoginSettings);
                        IsAuthenticated = true;
                        if (!String.IsNullOrEmpty(Me.AfterLoginRedirectScreen))
                        {
                            Page p = MobilePage.GetPage(Me.AfterLoginRedirectScreen).GetPage();

                            Page.Navigation.PushAsync(p);
                        }
                    }
                }

                if (!IsAuthenticated)
                {
                    IPage.DisplayErrorAlert("You have entered an invalid username/password combination. Please try again.");
                }
            }
            catch (Exception ex)
            { 
                IPage.DisplayErrorAlert(ex.Message);
            }
        }

      

        
    }
}
