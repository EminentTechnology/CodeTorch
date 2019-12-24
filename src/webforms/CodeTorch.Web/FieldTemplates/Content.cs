using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeTorch.Core;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Data;

namespace CodeTorch.Web.FieldTemplates
{
    public class Content: BaseFieldTemplate
    {
        System.Web.UI.WebControls.Literal ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new System.Web.UI.WebControls.Literal();
            Controls.Add(ctrl);
        }

        ContentControl _Me = null;
        public ContentControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (ContentControl)this.Widget;
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
                if (!String.IsNullOrEmpty(Me.SkinID))
                {
                    ctrl.SkinID = Me.SkinID;
                }

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
            base.LoadControl(sender, e);

            try
            {
                DataRow row = null;

                if ((this.RecordObject != null) && (this.RecordObject is DataRow))
                {
                    row = (DataRow)this.RecordObject;
                }

                string content = Me.Content;
                if (String.IsNullOrEmpty(content))
                    content = String.Empty;

                ctrl.Text = PopulateContent(content, row);

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

        private string PopulateContent(string text, DataRow dataRow)
        {
            if (dataRow != null)
            {
                foreach (DataColumn c in dataRow.Table.Columns)
                {
                    text = Common.ReplaceText(text, String.Format("{{{0}}}", c.ColumnName), String.Format("{0}", dataRow[c.ColumnName]), StringComparison.InvariantCultureIgnoreCase);
                }
            }

            return text;
        }
    }
}
