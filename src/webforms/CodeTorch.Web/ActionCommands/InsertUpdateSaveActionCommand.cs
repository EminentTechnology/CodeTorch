using CodeTorch.Abstractions;
using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using CodeTorch.Web.SectionControls;
using CodeTorch.Web.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeTorch.Web.ActionCommands
{
    public class InsertUpdateSaveActionCommand: IActionCommandStrategy
    {
        public Templates.BasePage Page { get; set; }

   

        public ActionCommand Command { get; set; }

        InsertUpdateSaveCommand Me = null;
        string EntityIDValue = null;
        FormViewMode PageMode;

        enum RedirectTrigger
        {
            New,
            Next,
            Update,
            Cancel
        }

        public bool ExecuteCommand()
        {
            bool success = true;

            if (Command != null)
            {
                Me = (InsertUpdateSaveCommand)Command;
            }

            PageMode = Page.DetermineMode(Me.EntityID, Me.EntityInputType);
            EntityIDValue = Page.GetEntityIDValue(Page.Screen, Me.EntityID, Me.EntityInputType);
            success = SaveEditForm();

            return success;
        }

       

       

        protected bool SaveEditForm()
        {
            DataCommandService dataCommandDB = DataCommandService.GetInstance();
            PageDB pageDB = new PageDB();

            bool SuccessIndicator = false;
            string retVal = null;

            ILog log = Resolver.Resolve<ILogManager>().GetLogger(this.GetType());


            if (Page.IsValid)
            {
                try
                {
                    object r = null;
                    List<ScreenDataCommandParameter> parameters = null;
                    switch (PageMode)
                    {
                        case FormViewMode.Insert:
                            log.Info("\r\n\r\nIn Insert Mode");
                            if (String.IsNullOrEmpty(Me.InsertCommand))
                            {
                                throw new ApplicationException("InsertCommand is invalid");
                            }
                            log.DebugFormat("InsertCommand:{0}", Me.InsertCommand);

                            parameters = pageDB.GetPopulatedCommandParameters(Me.InsertCommand, Page);
                            r = dataCommandDB.ExecuteDataCommand( Me.InsertCommand, parameters);

                            if (r != null)
                            {
                                retVal = r.ToString();
                                log.DebugFormat("Output Parameter:{0}", retVal);

                            }

                            log.DebugFormat("RedirectAfterInsert:{0}", Me.RedirectAfterInsert);
                            if (Me.RedirectAfterInsert)
                            {
                                string url = GetRedirectUrl(retVal);
                                log.DebugFormat("RedirectUrl:{0}", url);
                                Page.Response.Redirect(url,false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                            else
                            {


                                Page.DisplaySuccessAlert(String.Format(Me.AfterInsertConfirmationMessage, retVal));

                                RefreshSections();
                            }
                            break;
                        case FormViewMode.Edit:
                            log.Info("In Edit Mode");
                            if (String.IsNullOrEmpty(Me.UpdateCommand))
                            {
                                throw new ApplicationException("UpdateCommand is invalid");
                            }
                            log.DebugFormat("UpdateCommand:{0}", Me.UpdateCommand);

                            parameters = pageDB.GetPopulatedCommandParameters(Me.UpdateCommand, Page);
                            dataCommandDB.ExecuteDataCommand(Me.UpdateCommand, parameters);

                            log.DebugFormat("RedirectAfterUpdate:{0}", Me.RedirectAfterUpdate);
                            if (Me.RedirectAfterUpdate)
                            {
                                string url = GetRedirectUrl(Page.Request.QueryString[Me.EntityID]);
                                log.DebugFormat("RedirectUrl:{0}", url);
                                Page.Response.Redirect(url, false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                            else
                            {
                                Page.DisplaySuccessAlert(String.Format(Me.AfterUpdateConfirmationMessage, retVal));

                                RefreshSections();
                            }
                            break;
                    }
                    SuccessIndicator = true;
                }
                catch (Exception ex)
                {
                    SuccessIndicator = false;

                    Page.DisplayErrorAlert(ex);



                    log.Error(ex);
                }
            }

            return SuccessIndicator;
        }

        public string GetRedirectUrl(object DataCommandReturnValue)
        {
            string retVal = String.Empty;

            if (PageMode == FormViewMode.Insert)
            {

                if (String.IsNullOrEmpty(Me.AfterInsertRedirectUrl))
                {
                    throw new ApplicationException("AfterInsertRedirectUrl is invalid");
                }
                retVal = GetRedirectUrl(RedirectTrigger.New, DataCommandReturnValue);
            }
            else
            {
                if (String.IsNullOrEmpty(Me.AfterUpdateRedirectUrl))
                {
                    throw new ApplicationException("AfterUpdateRedirectUrl is invalid");
                }
                retVal = GetRedirectUrl(RedirectTrigger.Update, DataCommandReturnValue);
            }

            return retVal;
        }

        string GetRedirectUrl(RedirectTrigger trigger, object DataCommandReturnValue)
        {
            string retVal = String.Empty;

            switch (trigger)
            {
                case RedirectTrigger.New:
                    if (String.IsNullOrEmpty(Me.AfterInsertRedirectUrl.Trim()))
                    {
                        throw new ApplicationException("AfterInsertRedirectUrl is invalid");
                    }
                    retVal = Common.CreateUrlWithQueryStringContext(Me.AfterInsertRedirectUrl, Me.AfterInsertRedirectUrlContext);
                    retVal = String.Format(retVal, DataCommandReturnValue, Page.Request.QueryString[Me.EntityID]);
                    break;
                case RedirectTrigger.Update:
                    if (String.IsNullOrEmpty(Me.AfterUpdateRedirectUrl.Trim()))
                    {
                        throw new ApplicationException("AfterUpdateRedirectUrl is invalid");
                    }
                    retVal = Common.CreateUrlWithQueryStringContext(Me.AfterUpdateRedirectUrl, Me.AfterUpdateRedirectUrlContext);
                    retVal = String.Format(retVal, DataCommandReturnValue, Page.Request.QueryString[Me.EntityID]);
                    break;
                
            }

            return retVal;
        }

        private void RefreshSections()
        {
            ILog log = Resolver.Resolve<ILogManager>().GetLogger(this.GetType());

            try
            {
                foreach (BaseSectionControl section in Page.SectionControls)
                {
                    try
                    {
                        section.PopulateControl();

                        if (!(section.Section is EditSection))
                        {

                            section.BindDataSource();
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessageFormat = "<span class='ErrorMessages'>ERROR - {0} - {2} Section - {1})</span>";
                        string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, section.Section.Name, section.Section.Type);

                        section.Controls.Add(new LiteralControl(ErrorMessages));
                    }
                }


            }
            catch (Exception ex)
            {
                Page.DisplayErrorAlert(ex);

                log.Error(ex);
            }
        }
    }
}
