using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using CodeTorch.Web.Data;
using CodeTorch.Core;
using System.Drawing;
namespace CodeTorch.Web.FieldTemplates
{
    public class Editor : BaseFieldTemplate
    {
        Telerik.Web.UI.RadEditor ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new Telerik.Web.UI.RadEditor();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }

        EditorControl _Me = null;
        public EditorControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (EditorControl)this.BaseControl;
                }
                return _Me;
            }
        }

        public override string Value
        {
            get
            {

                return ctrl.Content;
            }
            set
            {

                ctrl.Content = value;

            }
        }

        

        public override void InitControl(object sender, EventArgs e)
        {
            base.InitControl(sender, e);

            try
            {
                //ctrl = new Telerik.Web.UI.RadEditor();
                //ctrl.ID = "ctrl";

                if (!String.IsNullOrEmpty(Me.Width))
                {
                    ctrl.Width = new Unit(Me.Width);
                }

                if (!String.IsNullOrEmpty(Me.Height))
                {
                    ctrl.Height = new Unit(Me.Height);
                }

                if (!String.IsNullOrEmpty(Me.CssClass))
                {
                    ctrl.CssClass = Me.CssClass;
                }

                if (!String.IsNullOrEmpty(Me.SkinID))
                {
                    ctrl.SkinID = Me.SkinID;
                }

                if (!String.IsNullOrEmpty(Me.Skin))
                {
                    ctrl.Skin = Me.Skin;
                }

                ctrl.ContentAreaMode = Telerik.Web.UI.EditorContentAreaMode.Div;

                ctrl.AutoResizeHeight = Me.AutoResizeHeight;
                ctrl.MaxHtmlLength = Me.MaxHtmlLength;
                ctrl.MaxTextLength = Me.MaxTextLength;

                if (!String.IsNullOrEmpty(Me.EnabledContentFilters))
                {
                    string[] cf = Me.EnabledContentFilters.Split(',');
                    for (int i = 0; i < cf.Length; i++)
                    {
                        try
                        {
                            ctrl.EnableFilter((Telerik.Web.UI.EditorFilters)Enum.Parse(typeof(Telerik.Web.UI.EditorFilters), cf[i], false));
                        }
                        catch { }
                    }
                }

                if (!String.IsNullOrEmpty(Me.DisabledContentFilters))
                {
                    string[] cf = Me.DisabledContentFilters.Split(',');
                    for (int i = 0; i < cf.Length; i++)
                    {
                        try
                        {
                            ctrl.DisableFilter((Telerik.Web.UI.EditorFilters)Enum.Parse(typeof(Telerik.Web.UI.EditorFilters), cf[i], false));
                        }
                        catch { }
                    }
                }

                ctrl.ToolbarMode = (Telerik.Web.UI.EditorToolbarMode)Enum.Parse(typeof(Telerik.Web.UI.EditorToolbarMode), Me.ToolbarMode.ToString(), false);

                //editor modes
                SetupEditorModes();

                //this.Controls.Add(ctrl);

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.Text = ErrorMessages;
                this.ctrl.BackColor = Color.Red;

            }


        }

        private void SetupEditorModes()
        {
            if (!Me.EnableDesignMode && !Me.EnableHtmlMode && Me.EnablePreviewMode)
            {
                this.ctrl.EditModes = Telerik.Web.UI.EditModes.Preview;
            }

            if (!Me.EnableDesignMode && Me.EnableHtmlMode && !Me.EnablePreviewMode)
            {
                this.ctrl.EditModes = Telerik.Web.UI.EditModes.Html;
            }

            if (Me.EnableDesignMode && !Me.EnableHtmlMode && !Me.EnablePreviewMode)
            {
                this.ctrl.EditModes = Telerik.Web.UI.EditModes.Design;
            }

            if (Me.EnableDesignMode && !Me.EnableHtmlMode && Me.EnablePreviewMode)
            {
                this.ctrl.EditModes = Telerik.Web.UI.EditModes.Design | Telerik.Web.UI.EditModes.Preview;
            }

            if (Me.EnableDesignMode && Me.EnableHtmlMode && !Me.EnablePreviewMode)
            {
                this.ctrl.EditModes = Telerik.Web.UI.EditModes.Design | Telerik.Web.UI.EditModes.Html;
            }

            if (!Me.EnableDesignMode && Me.EnableHtmlMode && Me.EnablePreviewMode)
            {
                this.ctrl.EditModes = Telerik.Web.UI.EditModes.Html | Telerik.Web.UI.EditModes.Preview;
            }

            if (Me.EnableDesignMode && Me.EnableHtmlMode && Me.EnablePreviewMode)
            {
                this.ctrl.EditModes = Telerik.Web.UI.EditModes.All;
            }
        }

        public override string GetValidationControlIDSuffix()
        {
            return "$ctrl";
        }
    }
}
