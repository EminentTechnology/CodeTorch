using System;
using System.Collections.Generic;
using System.Linq;
using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Core.Services;
using CodeTorch.Mobile.Templates;
using Xamarin.Forms;

namespace CodeTorch.Mobile.ActionCommands
{
    public class InsertUpdateSaveActionCommand: IActionCommandStrategy
    {
        public Page Page { get; set; }
        public IMobilePage IPage { get; set; }

   

        public ActionCommand Command { get; set; }

        InsertUpdateSaveCommand Me = null;
        string EntityIDValue = null;
        FormViewMode PageMode;

        enum FormViewMode
        {
            View,
            Insert,
            Edit
        }

        enum RedirectTrigger
        {
            New,
            Next,
            Update,
            Cancel
        }

        public void ExecuteCommand()
        {
            if (Command != null)
            {
                Me = (InsertUpdateSaveCommand)Command;
            }

            //PageMode = Page.DetermineMode(Me.EntityID, Me.EntityInputType);
            //EntityIDValue = Page.GetEntityIDValue(Page.Screen, Me.EntityID, Me.EntityInputType);
            SaveEditForm();
        }

       

       

        protected async void SaveEditForm()
        {
            DataCommandService dataCommandDB = DataCommandService.GetInstance();

            bool SuccessIndicator = false;
            string retVal = null;

            if (true)//Page.IsValid)
            {
                try
                {
                    object r = null;
                    List<ScreenDataCommandParameter> parameters = null;
                    switch (PageMode)
                    {
                        case FormViewMode.View:
                            CodeTorch.Core.Common.LogInfo("\r\n\r\nIn Insert Mode");
                            
                            if (String.IsNullOrEmpty(Me.InsertCommand))
                            {
                                throw new Exception("InsertCommand is invalid");
                            }
                            CodeTorch.Core.Common.LogDebug(String.Format("InsertCommand:{0}", Me.InsertCommand));

                            parameters = Common.GetPopulatedCommandParameters(Me.InsertCommand, IPage);

                            DataCommand command = DataCommand.GetDataCommand(Me.InsertCommand);

                            switch (command.ReturnType)
                            {
                                case DataCommandReturnType.DataTable:
                                    r = await dataCommandDB.GetDataForDataCommand(Me.InsertCommand, parameters);
                                    break;
                                case DataCommandReturnType.Integer:
                                    r = await  dataCommandDB.ExecuteDataCommand(Me.InsertCommand, parameters);
                                    break;
                                case DataCommandReturnType.Xml:
                                    throw new NotImplementedException();
                                    break;
                                case DataCommandReturnType.None:
                                    break;
                            }
                            

                            if (r != null)
                            {
                                if (!String.IsNullOrEmpty(Me.AfterInsertSettings))
                                {
                                    if (r is DataTable)
                                    {
                                        DataTable dt = r as DataTable;
                                        if (dt.Rows.Count > 0)
                                        {
                                            DataRow row = dt.Rows[0];
                                            Common.DataRowToSettings(row, Me.AfterInsertSettings);
                                        }
                                        
                                    
                                    }
                                }
                                //if r is datatable - see if any of the values need to be stored in settings
                                //retVal = r.ToString();
                                //CodeTorch.Core.Common.LogDebug(String.Format("Output Parameter:{0}", retVal));

                            }

                            CodeTorch.Core.Common.LogDebug(String.Format("RedirectAfterInsert:{0}", Me.RedirectAfterInsert));
                            if (Me.RedirectAfterInsert)
                            {
                                if (!String.IsNullOrEmpty(Me.AfterInsertRedirectScreen))
                                {
                                    Page p = MobilePage.GetPage(Me.AfterInsertRedirectScreen).GetPage();

                                    Page.Navigation.PushAsync(p);
                                }
                                else
                                { 
                                    //TODO: do what -tell about config error
                                }

                                //string url = GetRedirectUrl(retVal);
                                //log.DebugFormat("RedirectUrl:{0}", url);
                                //Page.Response.Redirect(url,false);
                                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                            else
                            {
                                Page.DisplayAlert ("Alert", Me.AfterInsertConfirmationMessage, "OK");

                                //Page.DisplaySuccessAlert(String.Format(Me.AfterInsertConfirmationMessage, retVal));

                                //RefreshSections();
                            }
                            break;
                        //case FormViewMode.Edit:
                        //    log.Info("In Edit Mode");
                        //    if (String.IsNullOrEmpty(Me.UpdateCommand))
                        //    {
                        //        throw new ApplicationException("UpdateCommand is invalid");
                        //    }
                        //    log.DebugFormat("UpdateCommand:{0}", Me.UpdateCommand);

                        //    parameters = pageDB.GetPopulatedCommandParameters(Me.UpdateCommand, Page);
                        //    dataCommandDB.ExecuteDataCommand(Me.UpdateCommand, parameters);

                        //    log.DebugFormat("RedirectAfterUpdate:{0}", Me.RedirectAfterUpdate);
                        //    if (Me.RedirectAfterUpdate)
                        //    {
                        //        string url = GetRedirectUrl(Page.Request.QueryString[Me.EntityID]);
                        //        log.DebugFormat("RedirectUrl:{0}", url);
                        //        Page.Response.Redirect(url, false);
                        //        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        //    }
                        //    else
                        //    {
                        //        Page.DisplaySuccessAlert(String.Format(Me.AfterUpdateConfirmationMessage, retVal));

                        //        RefreshSections();
                        //    }
                        //    break;
                    }
                    SuccessIndicator = true;
                }
                catch (Exception ex)
                {
                    SuccessIndicator = false;

                    IPage.DisplayErrorAlert(ex.Message);



                    //log.Error(ex);
                }
            }

            //return SuccessIndicator;
        }

        

        

        public string GetRedirectUrl(object DataCommandReturnValue)
        {
            string retVal = String.Empty;

            if (PageMode == FormViewMode.Insert)
            {

                if (String.IsNullOrEmpty(Me.AfterInsertRedirectScreen))
                {
                    throw new Exception("AfterInsertRedirectUrl is invalid");
                }
                retVal = GetRedirectUrl(RedirectTrigger.New, DataCommandReturnValue);
            }
            else
            {
                if (String.IsNullOrEmpty(Me.AfterUpdateRedirectScreen))
                {
                    throw new Exception("AfterUpdateRedirectUrl is invalid");
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
                    if (String.IsNullOrEmpty(Me.AfterInsertRedirectScreen.Trim()))
                    {
                        throw new Exception("AfterInsertRedirectUrl is invalid");
                    }
                    //retVal = Common.CreateUrlWithQueryStringContext(Me.AfterInsertRedirectUrl, Me.AfterInsertRedirectUrlContext);
                    //retVal = String.Format(retVal, DataCommandReturnValue, Page.Request.QueryString[Me.EntityID]);
                    retVal = Me.AfterInsertRedirectScreen;
                    break;
                case RedirectTrigger.Update:
                    if (String.IsNullOrEmpty(Me.AfterUpdateRedirectScreen.Trim()))
                    {
                        throw new Exception("AfterUpdateRedirectUrl is invalid");
                    }
                    //retVal = Common.CreateUrlWithQueryStringContext(Me.AfterUpdateRedirectScreen, Me.AfterUpdateRedirectUrlContext);
                    //retVal = String.Format(retVal, DataCommandReturnValue, Page.Request.QueryString[Me.EntityID]);
                    retVal = Me.AfterUpdateRedirectScreen;
                    break;
                
            }

            return retVal;
        }

        private void RefreshSections()
        {
            //log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            //try
            //{
            //    foreach (BaseSectionControl section in Page.SectionControls)
            //    {
            //        try
            //        {
            //            section.PopulateControl();

            //            if (!(section.Section is EditSection))
            //            {

            //                section.BindDataSource();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            string ErrorMessageFormat = "<span class='ErrorMessages'>ERROR - {0} - {2} Section - {1})</span>";
            //            string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, section.Section.Name, section.Section.Type);

            //            section.Controls.Add(new LiteralControl(ErrorMessages));
            //        }
            //    }


            //}
            //catch (Exception ex)
            //{
            //    Page.DisplayErrorAlert(ex);

            //    log.Error(ex);
            //}
        }
    }
}
