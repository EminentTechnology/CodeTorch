using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CodeTorch.Core;
using CodeTorch.Web.Templates;
using CodeTorch.Core.Messages;

namespace CodeTorch.Web.SectionControls
{
    public class AlertSectionControl : BaseSectionControl
    {
        
        System.Web.UI.WebControls.PlaceHolder AlertHolder;
        BasePage page = null;

        AlertSection _Me = null;
        public AlertSection Me
        {
            get
            {
                if ((_Me == null) && (this.Section != null) && (this.Section is AlertSection))
                {
                    _Me = (AlertSection)this.Section;
                }
                return _Me;
            }
        }




       

        public override void RenderControl()
        {
            page = HttpContext.Current.Handler as BasePage;
            base.RenderControl();

            page.MessageBus.Subscribe<DisplayAlertMessage>(RenderAlert);

            AlertHolder = new PlaceHolder();
            this.ContentPlaceHolder.Controls.Add(AlertHolder);

            if (Me.IncludeValidationSummary)
            {
                ValidationSummary summary = new ValidationSummary();
                summary.HeaderText = "The following error(s) occurred:";
                summary.DisplayMode = ValidationSummaryDisplayMode.List;
                summary.EnableClientScript = true;
                summary.CssClass = "alert alert-danger";

                this.ContentPlaceHolder.Controls.Add(summary);

            }
            

            


        }

        public override void PopulateControl()
        {
            base.PopulateControl();

            

        }

        private void RenderAlert(DisplayAlertMessage message)
        {
            Abstractions.ILog log = Resolver.Resolve<Abstractions.ILogManager>().GetLogger(this.GetType());

            try
            {
                //AlertHolder.Widgets.Clear();

                string alertClass = "alert";
                string alertTypeClass = null; 

                

                switch (message.AlertType)
                { 
                    case DisplayAlertMessage.ALERT_DANGER:
                        alertTypeClass = "alert-danger";
                        break;
                    case DisplayAlertMessage.ALERT_WARNING:
                        alertTypeClass = "alert-warning";
                        break;
                    case DisplayAlertMessage.ALERT_SUCCESS:
                        alertTypeClass = "alert-success";
                        break;
                    default:
                        alertTypeClass = "alert-info";
                        break;
                }

                alertClass += " " + alertTypeClass;

                if (message.IsDismissable)
                {
                    alertClass += " alert-dismissable";
                }

                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Attributes.Add("class", alertClass);

                if (message.IsDismissable)
                {
                    HtmlGenericControl button = new HtmlGenericControl("button");
                    button.Attributes.Add("type", "button");
                    button.Attributes.Add("class", "close");
                    button.Attributes.Add("data-dismiss", "alert");
                    button.Attributes.Add("aria-hidden", "true");
                    button.InnerHtml = "&times;";

                    div.Controls.Add(button);

                }

                if (!String.IsNullOrEmpty(message.Text))
                {
                    div.Controls.Add(new LiteralControl(message.Text));
                }
                

                AlertHolder.Controls.Add(div);
                
                this.Visible = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        





    }
}
