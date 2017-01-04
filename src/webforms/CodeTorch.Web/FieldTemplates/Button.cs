using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeTorch.Core;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using CodeTorch.Web.Templates;
using System.Data;


namespace CodeTorch.Web.FieldTemplates
{
    public class Button: BaseFieldTemplate
    {
        System.Web.UI.WebControls.Button ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new System.Web.UI.WebControls.Button();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }
        
        ButtonControl _Me = null;
        public ButtonControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (ButtonControl)this.Widget;
                }
                return _Me;
            }
        }

        public override string Value
        {
            get
            {
                return (ViewState["Value"] == null) ? String.Empty : ViewState["Value"].ToString();
            }
            set
            {
                ViewState["Value"] = value;
                
                
            }
        }

    

        

        public override void InitControl(object sender, EventArgs e)
        {
            base.InitControl(sender, e);


            try
            {

                //ctrl = new System.Web.UI.WebControls.Button();

                
                ctrl.CausesValidation = Me.CausesValidation;
                ctrl.OnClientClick = Me.OnClientClick;
                ctrl.CommandName = Me.CommandName;
                ctrl.CommandArgument = Me.CommandArgument;
                ctrl.CssClass = Me.CssClass;
                if (!String.IsNullOrEmpty(Me.Width))
                {
                    ctrl.Attributes.Add("style", String.Format("width: {0};", Me.Width));
                }



                if (!String.IsNullOrEmpty(Me.SkinID))
                {
                    ctrl.SkinID = Me.SkinID;
                }

                ctrl.Click += button_Click;
                

                

               // this.Widgets.Add(ctrl);

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.Controls.Add(new LiteralControl(ErrorMessages));
                

            }
        }

        public override void LoadControl(object sender, EventArgs e)
        {

            try
            {
                DataRow row = null;

                if ((this.RecordObject != null) && (this.RecordObject is DataRow))
                {
                    row = (DataRow)this.RecordObject;
                }

                if (String.IsNullOrEmpty(Me.Text))
                {
                    if (!String.IsNullOrEmpty(Me.DataTextField))
                    {
                        if (row != null)
                        {
                            string format = "{0}";

                            if (!String.IsNullOrEmpty(Me.DataTextFormatString))
                                format = Me.DataTextFormatString;

                            ctrl.Text = String.Format(format, row[Me.DataTextField]);
                        }
                    }
                }
                else
                {
                    ctrl.Text = GetGlobalResourceString("Text", Me.Text);
                }

                


            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);

                this.Controls.Add(new LiteralControl(ErrorMessages));

            }
        }

        public override bool SupportsValidation()
        {
            return false;
        }

       

        void button_Click(object sender, EventArgs e)
        {
            Abstractions.ILog log = Resolver.Resolve<Abstractions.ILogManager>().GetLogger(this.GetType());


            try
            {
                ActionRunner runner = new ActionRunner();
                Core.Action action = ObjectCopier.Clone<Core.Action>(Me.OnClick);


                if (action != null)
                {
                    runner.Page = (BasePage)this.Page;
                    runner.Action = action;
                    runner.Execute();
                }

                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ((BasePage)this.Page).DisplayErrorAlert(ex);

            }

        }
    }
}
