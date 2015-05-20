using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using CodeTorch.Web.UserControls;
using CodeTorch.Web.FieldTemplates;
using System.Web.Security;
using Telerik.Web.UI;
using CodeTorch.Core;
using CodeTorch.Web.Templates;

namespace CodeTorch.Web.SectionControls
{
    public class ContentSectionControl: BaseSectionControl
    {
        System.Web.UI.WebControls.Literal Content;

        ContentSection _Me = null;
        public ContentSection Me
        {
            get
            {
                if ((_Me == null) && (this.Section != null) && (this.Section is ContentSection))
                {
                    _Me = (ContentSection)this.Section;
                }
                return _Me;
            }
        }


        

        public override void RenderControl()
        {
            base.RenderControl();

            Content = new Literal();
            this.ContentPlaceHolder.Controls.Add(Content);


        }

        public override void PopulateControl()
        {
            base.PopulateControl();

            if (!String.IsNullOrEmpty(Me.SelectDataCommand))
            {

                PageDB pageDB = new PageDB();
                List<ScreenDataCommandParameter> parameters;
                BasePage page = ((BasePage)this.Page);
                DataCommandService dataCommand = DataCommandService.GetInstance();

                parameters = pageDB.GetPopulatedCommandParameters(Me.SelectDataCommand, (BasePage)this.Page);

                DataTable data = dataCommand.GetDataForDataCommand(Me.SelectDataCommand, parameters);
                if (data.Rows.Count > 0)
                {
                    this.Content.Text = PopulateContent(Me.Content, data.Rows[0]);
                }
                else
                {
                    this.Content.Text = Me.Content;
                }
            }
            else 
            {
                this.Content.Text = Me.Content;
            }
            
        }

        private string PopulateContent(string text, DataRow dataRow)
        {

            foreach (DataColumn c in dataRow.Table.Columns)
            {
                text = Common.ReplaceText(text, String.Format("{{{0}}}", c.ColumnName), String.Format("{0}", dataRow[c.ColumnName]), StringComparison.InvariantCultureIgnoreCase);
            }

            return text;
        }

      

    }
}
