using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Core.Services;
using CodeTorch.Web.Code;
using CodeTorch.Web.Data;
using CodeTorch.Web.UserControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeTorch.Web.ActionCommands
{
    public class ExportRDLCActionCommand : IActionCommandStrategy
    {
        public Templates.BasePage Page { get; set; }

  

        public ActionCommand Command { get; set; }

        ExportRDLCCommand Me = null;
        


        public void ExecuteCommand()
        {
            

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                if (Command != null)
                {
                    Me = (ExportRDLCCommand)Command;
                }

                RenderReport();

            }
            catch (Exception ex)
            {
                Page.DisplayErrorAlert(ex);

                log.Error(ex);
            }
            
        }

        protected virtual void RenderReport()
        {
            DataCommandService dataCommandDB = DataCommandService.GetInstance();
            PageDB pageDB = new PageDB();

            //get report data
            Hashtable dataItems = new Hashtable();
            foreach (CodeTorch.Core.ReportDataSource dataSource in Me.ReportDataSources)
            {
                DataTable data = GetData(dataCommandDB, pageDB, dataSource);
                dataItems.Add(dataSource.Name, data);
            }

            //render report
            string mime = null;
            string ext = null;
            byte[] report = ReportFunctions.RenderReport(Me, dataItems, out mime, out ext);

            Page.Response.Expires = 0;
            Page.Response.Buffer = true;
            Page.Response.Clear();
            Page.Response.ClearContent();
            Page.Response.ClearHeaders();
            Page.Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}.{1}", (String.IsNullOrEmpty(Me.ExportFileName) ? "export" : Me.ExportFileName), ext));
            Page.Response.ContentType = mime;
            Page.Response.BinaryWrite(report);
            Page.Response.End();

        }

        private DataTable GetData(DataCommandService dataCommandDB, PageDB pageDB, CodeTorch.Core.ReportDataSource dataSource)
        {
            List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(dataSource.ReportCommand, Page);
            DataTable data = dataCommandDB.GetDataForDataCommand(dataSource.ReportCommand, parameters);

            return data;
        }
        

      

        
    }
}
