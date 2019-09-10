using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace CodeTorch.Web.ActionCommands
{
    public class RenderPageSectionsActionCommand : IActionCommandStrategy
    {
        public Templates.BasePage Page { get; set; }



        public ActionCommand Command { get; set; }

        RenderPageSectionsCommand Me = null;


        FormViewMode PageMode;

        public bool ExecuteCommand()
        {
            bool success = true;

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                if (Command != null)
                {
                    Me = (RenderPageSectionsCommand)Command;
                }

                //identify section template to load
                string SectionZoneLayoutToLoad = Page.Screen.SectionZoneLayout;
                if (Page.Screen.SectionZoneLayoutMode == SectionZoneLayoutMode.Static)
                {
                    SectionZoneLayoutToLoad = Page.Screen.SectionZoneLayout;
                }
                else
                {
                    SectionZoneLayoutToLoad = GetDynamicSectionZoneLayout();
                }

                



                switch (Me.Mode)
                { 
                    case RenderPageSectionsCommand.SectionRenderMode.InsertEdit:
                        PageMode = Page.DetermineMode(Me.EntityID, Me.EntityInputType);

                        switch (this.PageMode)
                        {
                            case FormViewMode.Insert:
                                Page.RenderPageSections(SectionZoneLayoutToLoad, Page.Screen, Page.Screen.Sections, false, SectionMode.Insert, "Screen.Sections");
                                break;
                            default:
                                Page.RenderPageSections(SectionZoneLayoutToLoad, Page.Screen, Page.Screen.Sections, false, SectionMode.Edit, "Screen.Sections");
                                break;
                        }
                        break;
                    default:
                        Page.RenderPageSections(SectionZoneLayoutToLoad, Page.Screen, Page.Screen.Sections, false);
                        break;

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

        private string GetDynamicSectionZoneLayout()
        {
            string retVal = null;
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            try
            {
                DataCommandService dataCommandDB = DataCommandService.GetInstance();
                PageDB pageDB = new PageDB();

                List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(Page.Screen.SectionZoneLayoutDataCommand, Page);

                DataTable dt = dataCommandDB.GetDataForDataCommand(Page.Screen.SectionZoneLayoutDataCommand, parameters);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains(Page.Screen.SectionZoneLayoutDataField))
                    {
                        retVal = dt.Rows[0][Page.Screen.SectionZoneLayoutDataField].ToString();
                    }
                }


            }
            catch (Exception ex)
            {
                Page.DisplayErrorAlert(ex);

                log.Error(ex);
            }

            return retVal;
        }
    }
}
