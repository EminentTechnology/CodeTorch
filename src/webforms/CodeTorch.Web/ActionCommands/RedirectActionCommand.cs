using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CodeTorch.Web.ActionCommands
{
    public class RedirectActionCommand : IActionCommandStrategy
    {
        public Templates.BasePage Page { get; set; }

  

        public ActionCommand Command { get; set; }

        RedirectCommand Me = null;
        


        public bool ExecuteCommand()
        {
            bool success = true;


            Abstractions.ILog log = Resolver.Resolve<Abstractions.ILogManager>().GetLogger(this.GetType());

            try
            {
                if (Command != null)
                {
                    Me = (RedirectCommand)Command;
                }

                Page.Response.Clear();
                Page.Response.Redirect(ProcessRedirect(),false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                success = false;
                Page.DisplayErrorAlert(ex);

                log.Error(ex);
                
                if ((Me != null) && (!String.IsNullOrWhiteSpace(Me.OnErrorRedirectUrl)))
                {
                    string url = Common.CreateUrlWithQueryStringContext(Me.OnErrorRedirectUrl, Me.Context);
                    Page.Response.Clear();
                    Page.Response.Redirect(url, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            return success;
        }

        private string ProcessRedirect()
        {

            string retVal = null;

            

            

            switch (Me.RedirectMode)
            {
                case RedirectCommand.RedirectModeEnum.Constant:

                    //if there is a data command - execute it then redirect to the url listed below
                    if (!String.IsNullOrEmpty(Me.DataCommand))
                    {
                        retVal = ExecuteDataCommand(retVal);
                    }

                    retVal = Me.RedirectUrl;

                    break;
                case RedirectCommand.RedirectModeEnum.DataCommand:
                    retVal = ExecuteDataCommand(retVal);

                    if (String.IsNullOrEmpty(retVal))
                    {
                        retVal = Me.RedirectUrl;
                    }
                    break;
                case RedirectCommand.RedirectModeEnum.Referrer:
                    retVal = ExecuteDataCommand(retVal);
                    if (Page.Request.UrlReferrer != null)
                    {
                        retVal = Page.Request.UrlReferrer.AbsoluteUri;
                    }
                    else
                    {
                        retVal = Me.RedirectUrl;
                    }
                    break;
            }


            retVal = Common.CreateUrlWithQueryStringContext(retVal, Me.Context);

            return retVal;

        }

        private string ExecuteDataCommand(string retVal)
        {
            DataCommandService dataCommandDB = DataCommandService.GetInstance();
            PageDB pageDB = new PageDB();

            DataCommand command = DataCommand.GetDataCommand(Me.DataCommand);
            List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(Me.DataCommand, Page);
            DataTable data = null;
            object commandRetVal = null;
            if (command == null)
            {
                throw new ApplicationException(String.Format("DataCommand {0} could not be found in configuration", Me.DataCommand));
            }

            if (command.ReturnType == DataCommandReturnType.DataTable)
            {
                data = dataCommandDB.GetDataForDataCommand(Me.DataCommand, parameters);
            }
            else
            {
                commandRetVal = dataCommandDB.ExecuteDataCommand(Me.DataCommand, parameters);
            }

            if (command.ReturnType == DataCommandReturnType.DataTable)
            {
                if (data != null)
                {
                    if (!String.IsNullOrEmpty(Me.RedirectUrlField))
                    {
                        if (data.Rows.Count == 1)
                        {
                            retVal = data.Rows[0][Me.RedirectUrlField].ToString();
                        }
                        else
                        {
                            throw new ApplicationException("Invalid number of rows returned - " + data.Rows.Count);
                        }
                    }

                }
            }
            else
            {
                if (commandRetVal != null)
                {
                    retVal = commandRetVal.ToString();
                }
            }
            return retVal;
        }



        
    }
}
