﻿using CodeTorch.Core;
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

        public bool ExecuteCommand()
        {

            bool success = true;
            Abstractions.ILog log = Resolver.Resolve<Abstractions.ILogManager>().GetLogger(this.GetType());

            try
            {
                if (Command != null)
                {
                    Me = (ValidateUserCommand)Command;
                }

                if (ValidateUser())
                {
                    HttpCookie ck = null;
                    ck = AuthenticationHelper.CreateFormAuthenticationCookie(LoginName, DateTime.Now.AddMinutes(Me.LogoutTimeout), false, FormsAuthentication.FormsCookieName, FormsAuthentication.FormsCookiePath, profile);

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
