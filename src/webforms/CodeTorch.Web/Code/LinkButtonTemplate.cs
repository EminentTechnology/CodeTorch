using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI.Grid;
using Telerik.Web.UI;
using System.Data;


namespace CodeTorch.Web.Code
{
  

    public class LinkButtonTemplate : ITemplate
    {
        private string _DisplayField = string.Empty;
        private string _ValueField = string.Empty;

        public LinkButtonTemplate(string DisplayField, string ValueField)
        {
            _DisplayField = DisplayField;
            _ValueField = ValueField;
        }





        // Override the ITemplate.InstantiateIn method to ensure 
        // that the templates are created in a Literal control and
        // that the Literal object's DataBinding event is associated
        // with the BindData method.
        public void InstantiateIn(Control container)
        {
            LinkButton ctrl = new LinkButton();
            ctrl.DataBinding += new EventHandler(this.BindData);
            container.Controls.Add(ctrl);
        }
        // Create a public method that will handle the
        // DataBinding event called in the InstantiateIn method.
        public void BindData(object sender, EventArgs e)
        {
            LinkButton ctrl = (LinkButton)sender;

            GridDataItem container = (GridDataItem)ctrl.NamingContainer;

            
            //l.Text = ((DataRowView)container.DataItem)[column].ToString();
            DataRowView val = ((DataRowView)container.DataItem);

            string field = String.Empty;
            string fieldValue = String.Empty;

            if (HttpContext.Current != null)
            {
                field = HttpContext.Current.Request.QueryString["field"];
                fieldValue = HttpContext.Current.Request.QueryString["fieldValue"];
                ctrl.Text = val[_DisplayField].ToString();
                ctrl.OnClientClick = string.Format("parent.document.getElementById('{0}').value='{1}';parent.document.getElementById('{2}').value='{3}';parent.$.fancybox.close();", field, val[_DisplayField], fieldValue, val[_ValueField] );

            }
            else
            {
                ctrl.Text = "Missing picker parameters";
            }

            
            

        }


    }
}
