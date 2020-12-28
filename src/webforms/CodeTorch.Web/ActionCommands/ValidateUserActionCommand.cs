using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CodeTorch.Web.ActionCommands
{
    public class ValidateUserActionCommand : IActionCommandStrategy
    {
        public Templates.BasePage Page { get; set; }

  

        public ActionCommand Command { get; set; }

        ValidateUserCommand Me = null;

        DataRow profile;

        string LoginName = String.Empty;
        bool RememberMe = false;

        public bool ExecuteCommand()
        {

            bool success = true;
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                if (Command != null)
                {
                    Me = (ValidateUserCommand)Command;
                   
                }

                if (Page.IsValid)
                {
                    //assumes page is valid prior to attempting login - relies on validation provided by client
                    RememberMe = Me.RememberMeDefault; //set to defalu value - maube overriden in ValidateUser()

                    if (ValidateUser())
                    {
                        HttpCookie ck = null;
                        ck = FormsAuthenticationMode.CreateFormAuthenticationCookie(LoginName, DateTime.Now.AddMinutes(Me.LogoutTimeout), RememberMe, FormsAuthentication.FormsCookieName, FormsAuthentication.FormsCookiePath, profile);

                        Page.Response.Cookies.Add(ck);

                        string strRedirect;
                        strRedirect = Page.Request["ReturnUrl"];
                        if (strRedirect == null)
                            strRedirect = FormsAuthentication.DefaultUrl;
                        Page.Response.Redirect(strRedirect, false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        Page.DisplayErrorAlert("You have entered an invalid username/password combination. Please try again.");
                    }
                }
                

            }
            catch (Exception ex)
            {
                success = false;
                Page.DisplayErrorAlert(ex);

                log.Error(ex);
            }

            return success;

        }

        private bool ValidateUser()
        {
            bool IsAuthenticated = false;

            DataCommandService dataCommandDB = DataCommandService.GetInstance();
            PageDB pageDB = new PageDB();
            DataTable data = null;
            string password = String.Empty;
            

            List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(Me.ProfileCommand, Page);

            foreach (ScreenDataCommandParameter p in parameters)
            {
                if (p.Name.ToLower() == Me.UserNameParameter.ToLower())
                {
                    LoginName = Page.GetEntityIDValue(Page.Screen, p.InputKey, p.InputType);
                    break;
                }

                
            }
            password = Page.GetEntityIDValue(Page.Screen, Me.PasswordEntityID, Me.PasswordEntityInputType);

            if (!String.IsNullOrEmpty(Me.RememberMeEntityID))
            {
                var rememberMeValue = Page.GetEntityIDValue(Page.Screen, Me.RememberMeEntityID, Me.RememberMeEntityInputType);
                bool tempRememberMe;
                if (!String.IsNullOrEmpty(rememberMeValue) && (bool.TryParse(rememberMeValue, out tempRememberMe)))
                {
                    RememberMe = tempRememberMe;
                }
            }
            

            data = dataCommandDB.GetDataForDataCommand(Me.ProfileCommand, parameters);

            if (data.Rows.Count == 1)
            {
                profile = data.Rows[0];
                string dbPassword = profile[Me.PasswordField].ToString();

                PasswordMode mode = Me.PasswordMode;

                if (!String.IsNullOrEmpty(dbPassword))
                {
                    switch (mode)
                    {
                        case PasswordMode.Hash:
                            if (Cryptographer.CompareHash(Me.PasswordAlgorithm, password, dbPassword))
                            {
                                IsAuthenticated = true;
                            }

                            

                            break;
                        case PasswordMode.Encrypted:
                            string decryptedPassword = Cryptographer.DecryptSymmetric(Me.PasswordAlgorithm, dbPassword);
                            if (decryptedPassword == password)
                            {
                                IsAuthenticated = true;
                            }
                            break;
                        case PasswordMode.PlainText:
                            if (dbPassword == password)
                            {
                                IsAuthenticated = true;
                            }
                            break;
                    }

                }


            }

            return IsAuthenticated;
        }

        
    }
}
