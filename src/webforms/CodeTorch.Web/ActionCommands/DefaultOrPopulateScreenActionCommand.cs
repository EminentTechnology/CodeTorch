using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using CodeTorch.Web.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeTorch.Web.ActionCommands
{
    public class DefaultOrPopulateScreenActionCommand : IActionCommandStrategy
    {
        public Templates.BasePage Page { get; set; }



        public ActionCommand Command { get; set; }

        DefaultOrPopulateScreenCommand Me = null;

        //string EntityIDValue = null;
        FormViewMode PageMode;

        public bool ExecuteCommand()
        {

            bool success = true;
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                if (Command != null)
                {
                    Me = (DefaultOrPopulateScreenCommand)Command;
                }


                bool CanExecute = Me.ExecuteOnPostBack ? true : (!Page.IsPostBack);


                if (CanExecute)
                {
                    if (!String.IsNullOrEmpty(Me.EntityID))
                    {
                        PageMode = Page.DetermineMode(Me.EntityID, Me.EntityInputType);

                        if (PageMode == FormViewMode.Insert)
                        {
                            DefaultForm();
                        }
                        else
                        {
                            PopulateForm();
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Me.DefaultCommand))
                        {
                            DefaultForm();
                        }

                        if (!String.IsNullOrEmpty(Me.RetrieveCommand))
                        {
                            PopulateForm();
                        }
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


        private void DefaultForm()
        {

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
            
                DataCommandService dataCommandDB = DataCommandService.GetInstance();
                PageDB pageDB = new PageDB();

                if (!String.IsNullOrEmpty(Me.DefaultCommand))
                {
                    List<ScreenDataCommandParameter> parameters = null;
                    parameters = pageDB.GetPopulatedCommandParameters(Me.DefaultCommand, Page);
                    DataTable retVal = dataCommandDB.GetDataForDataCommand(Me.DefaultCommand, parameters);

                    foreach (Section section in Page.Screen.Sections)
                    {
                        Page.PopulateFormByDataTable(section.Widgets, retVal);
                    }
                }
            }
            catch (Exception ex)
            {
                Page.DisplayErrorAlert(ex);

                log.Error(ex);
            }
            
        }

        private void PopulateForm()
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                DataCommandService dataCommandDB = DataCommandService.GetInstance();
                PageDB pageDB = new PageDB();

                if (!String.IsNullOrEmpty(Me.RetrieveCommand))
                {
                    List<ScreenDataCommandParameter> parameters = null;

                    parameters = pageDB.GetPopulatedCommandParameters(Me.RetrieveCommand, Page);
                    DataTable retVal = dataCommandDB.GetDataForDataCommand(Me.RetrieveCommand, parameters);

                    foreach (Section section in Page.Screen.Sections)
                    {
                        Page.PopulateFormByDataTable(section.Widgets, retVal);


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
