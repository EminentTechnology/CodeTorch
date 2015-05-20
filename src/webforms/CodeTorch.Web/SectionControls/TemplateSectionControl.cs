using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

using System.Web.UI.WebControls;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using CodeTorch.Core;
using CodeTorch.Web.Templates;
using System.Collections;
using System.Web;

namespace CodeTorch.Web.SectionControls
{
    public class TemplateSectionControl : BaseSectionControl
    {
        System.Web.UI.WebControls.Literal Content;
        BasePage page;

        TemplateSection _Me = null;
        public TemplateSection Me
        {
            get
            {
                if ((_Me == null) && (this.Section != null) && (this.Section is TemplateSection))
                {
                    _Me = (TemplateSection)this.Section;
                }
                return _Me;
            }
        }




        public override void RenderControl()
        {
            page = HttpContext.Current.Handler as BasePage;
            base.RenderControl();

            Content = new Literal();
            this.ContentPlaceHolder.Controls.Add(Content);

           

        }

        public override void PopulateControl()
        {
            base.PopulateControl();

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


            try
            {
                if (!this.Page.IsPostBack)
                {
                    RenderContent();
                }
            }
            catch (Exception ex)
            {

                page.DisplayErrorAlert(ex);
                log.Error(ex);
            }

            

        }

        private Template GetTemplate(DataTable data)
        {
            Template retVal = null;
            string TemplateName = null;

            if (Me.TemplateSelectionMode == DynamicMode.Static)
            {
                TemplateName = Me.ContentTemplate;
            }
            else
            {

                if (data.Rows.Count > 0)
                {
                    if (String.IsNullOrEmpty(Me.ContentTemplateField))
                    {
                        throw new ApplicationException("Configuration Error - ContentTemplateField is empty");
                    }


                    if (data.Columns.Contains(Me.ContentTemplateField))
                    {
                        TemplateName = data.Rows[0][Me.ContentTemplateField].ToString();
                    }
                }

            }

            if (!String.IsNullOrEmpty(TemplateName))
            {
                retVal = Template.GetByName(TemplateName);
            }

            return retVal;
        }

        private DataTable GetContentTemplateData()
        {
            DataCommandService dataCommandDB = DataCommandService.GetInstance();
            PageDB pageDB = new PageDB();

            if (String.IsNullOrEmpty(Me.ContentTemplateDataCommand))
            {
                throw new ApplicationException("ContentTemplateDataCommand is not configured");
            }

            List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(Me.ContentTemplateDataCommand, page);
            DataTable data = dataCommandDB.GetDataForDataCommand(Me.ContentTemplateDataCommand, parameters);

            return data;
        }

        private Hashtable GetContentItems(Template contentTemplate)
        {
            DataCommandService dataCommandDB = DataCommandService.GetInstance();
            PageDB pageDB = new PageDB();
            Hashtable retVal = new Hashtable();

            var dataItems = from t in contentTemplate.DataItems
                            join c in Me.DataItems
                                on t.Name.ToLower() equals c.DataItem.ToLower()
                            select new
                            {
                                t.Name,
                                Item = c
                            };

            foreach (var dataItem in dataItems)
            {
                List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(dataItem.Item.DataCommand, page);
                DataTable data = dataCommandDB.GetDataForDataCommand(dataItem.Item.DataCommand, parameters);


                retVal.Add(dataItem.Name, data);
            }



            return retVal;
        }


        public void RenderContent()
        {
            DataTable data = null;

            if (
                (Me.TemplateSelectionMode == DynamicMode.Dynamic) ||
                (Me.ContentSelectionMode == DynamicMode.Dynamic)
              )
            {
                data = GetContentTemplateData();
            }
            //select template
            Template contentTemplate = GetTemplate(data);

            if (contentTemplate != null)
            {
                Hashtable contentItems = GetContentItems(contentTemplate);
                string content = String.Empty;

                if (Me.ContentSelectionMode == DynamicMode.Static)
                {

                    content = Template.RenderContent(contentTemplate, contentItems);
                }
                else
                {
                    if (String.IsNullOrEmpty(Me.ContentField))
                    {
                        throw new ApplicationException("Configuration Error - ContentField is empty");
                    }

                    if ((data != null) && (data.Rows.Count > 0))
                    {
                        content = Template.RenderContent(contentTemplate, data.Rows[0][Me.ContentField].ToString(), contentItems);
                    }
                }

                this.Content.Text = content;
            }
            else
            {
                throw new ApplicationException("No Template found");
            }

        }

    }
}
