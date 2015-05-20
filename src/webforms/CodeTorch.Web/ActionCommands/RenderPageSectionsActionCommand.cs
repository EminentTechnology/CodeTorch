using CodeTorch.Core;
using CodeTorch.Core.Commands;
using System;
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

        public void ExecuteCommand()
        {
            

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                if (Command != null)
                {
                    Me = (RenderPageSectionsCommand)Command;
                }

                switch (Me.Mode)
                { 
                    case RenderPageSectionsCommand.SectionRenderMode.InsertEdit:
                        PageMode = Page.DetermineMode(Me.EntityID, Me.EntityInputType);

                        switch (this.PageMode)
                        {
                            case FormViewMode.Insert:
                                Page.RenderPageSections(Page.Screen.SectionZoneLayout, Page.Screen, Page.Screen.Sections, false, SectionMode.Insert, "Screen.Sections");
                                break;
                            default:
                                Page.RenderPageSections(Page.Screen.SectionZoneLayout, Page.Screen, Page.Screen.Sections, false, SectionMode.Edit, "Screen.Sections");
                                break;
                        }
                        break;
                    default:
                        Page.RenderPageSections(Page.Screen.SectionZoneLayout, Page.Screen, Page.Screen.Sections, false);
                        break;

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
