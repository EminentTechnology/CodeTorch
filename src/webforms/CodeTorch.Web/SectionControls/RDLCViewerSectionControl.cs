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
using CodeTorch.Web.Templates;
using System.Web.Security;
using Telerik.Web.UI;
using CodeTorch.Core;
using CodeTorch.Core.Messages;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.Reflection;
using System.IO;
using CodeTorch.Web.Code;

namespace CodeTorch.Web.SectionControls
{
    public class RDLCViewerSectionControl: BaseSectionControl
    {


        ReportViewer viewer;




        RDLCViewerSection _Me = null;
        public RDLCViewerSection Me
        {
            get
            {
                if ((_Me == null) && (this.Section != null))
                {
                    _Me = (RDLCViewerSection)this.Section;
                }
                return _Me;
            }
        }


        BasePage page;
       
       

        public override void RenderControl()
        {
            page = HttpContext.Current.Handler as BasePage;
            base.RenderControl();

            viewer = new ReportViewer();
            viewer.Width = new Unit("100%");
            this.ContentPlaceHolder.Controls.Add(viewer);

            page.MessageBus.Subscribe<PerformSearchMessage>(RenderReport);

           
            this.Visible = Me.LoadDataOnPageLoad;

           
        }

        
        

        public override void PopulateControl()
        {
            base.PopulateControl();

            if (!Page.IsPostBack)
            {
                if (this.Visible)
                {
                    RenderReport();
                }
            }
        }


        public override void BindDataSource()
        {
            base.BindDataSource();
            RenderReport();
        }

        private void RenderReport(PerformSearchMessage message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                RenderReport();
            }
            catch (Exception ex)
            {
                page.DisplayErrorAlert(ex);
                log.Error(ex);
            }
        }

        protected void RenderReport()
        {
            try
            {
                DataCommandService dataCommandDB = DataCommandService.GetInstance();
                PageDB pageDB = new PageDB();

                viewer.Reset();

                LoadReportDefinition(viewer.LocalReport);
                viewer.LocalReport.DataSources.Clear();

                foreach (CodeTorch.Core.ReportParameter p in GetReportParameters())
                {
                    string parameterValue = null;

                    if (p.Value != null)
                        parameterValue = p.Value.ToString();

                    Microsoft.Reporting.WebForms.ReportParameter rp = new Microsoft.Reporting.WebForms.ReportParameter(p.Name, parameterValue);
                    viewer.LocalReport.SetParameters(rp);
                }

                foreach (CodeTorch.Core.ReportDataSource dataSource in Me.ReportDataSources)
                {

                    DataTable data = GetData(dataCommandDB, pageDB, dataSource);

                    Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource(dataSource.Name, data);
                    viewer.LocalReport.DataSources.Add(rds);

                }

                viewer.LocalReport.DisplayName = "Report";

                viewer.LocalReport.Refresh();
                viewer.Visible = true;

                this.Visible = true;
            }
            catch (Exception ex)
            {
                Common.LogException(ex, false);


                page.DisplayErrorAlert(String.Format("<strong>The following error(s) occurred:</strong>{0}", Common.GetExceptionMessage(ex, 4)));

            }
        }



        private void LoadReportDefinition(LocalReport localReport)
        {
            string ConfigResourceDLL = ConfigurationManager.AppSettings["APPBUILDER_CONFIG_DLL_NAMESPACE"];
            string configNamespace = ConfigurationManager.AppSettings["APPBUILDER_CONFIG_DLL_NAMESPACE"];

            Assembly assembly = Assembly.Load(ConfigResourceDLL);

            string reportPath = ReportFunctions.GetReportPath(Me.ReportName, Me.EnableLocalization, configNamespace);

            Stream stream = assembly.GetManifestResourceStream(reportPath);
            if (stream != null)
            {
                localReport.LoadReportDefinition(stream);
                localReport.EnableHyperlinks = Me.EnableHyperlinks;
            }
            else
            {
                throw new ApplicationException(String.Format("Report {0} could not be found.", reportPath));
            }
        }

        private DataTable GetData(DataCommandService dataCommandDB, PageDB pageDB, CodeTorch.Core.ReportDataSource dataSource)
        {
            List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(dataSource.ReportCommand, page);
            DataTable data = dataCommandDB.GetDataForDataCommand(dataSource.ReportCommand, parameters);

            return data;
        }

        private List<CodeTorch.Core.ReportParameter> GetReportParameters()
        {
            PageDB pageDB = new PageDB();
            List<CodeTorch.Core.ReportParameter> parameters = new List<Core.ReportParameter>();

            if (Me.Parameters != null)
            {
                parameters = pageDB.GetPopulatedReportParameters(page, Me.Parameters);
            }

            return parameters;
        }
        
        
    }
}
