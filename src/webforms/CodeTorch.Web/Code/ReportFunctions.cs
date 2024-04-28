using CodeTorch.Core;
using CodeTorch.Core.Commands;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;

namespace CodeTorch.Web.Code
{
    public class ReportFunctions
    {
        public static byte[] RenderReport(ExportRDLCCommand reportDefinition, Hashtable dataItems, out string mime, out string ext)
        {
            byte[] retVal;

            LocalReport localReport = new LocalReport();
            localReport.DataSources.Clear();
            LoadReportDefinition(localReport, reportDefinition);

            foreach (CodeTorch.Core.ReportDataSource dataSource in reportDefinition.ReportDataSources)
            {
                DataTable data = (DataTable) dataItems[dataSource.Name];

                Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource(dataSource.Name, data);
                localReport.DataSources.Add(rds);
            }

            localReport.Refresh();
            retVal = Export(localReport, reportDefinition, out mime, out ext);

            return retVal;
        }

        private static void LoadReportDefinition(LocalReport localReport, ExportRDLCCommand reportDefinition)
        {
            string ConfigResourceDLL = ConfigurationManager.AppSettings["APPBUILDER_CONFIG_DLL_NAMESPACE"];
            string configNamespace = ConfigurationManager.AppSettings["APPBUILDER_CONFIG_DLL_NAMESPACE"];

            Assembly assembly = Assembly.Load(ConfigResourceDLL);
            string reportPath = GetReportPath(reportDefinition.ReportName, reportDefinition.EnableLocalization, configNamespace);
            Stream stream = assembly.GetManifestResourceStream(reportPath);

            if (stream != null)
            {
                localReport.LoadReportDefinition(stream);
                localReport.EnableHyperlinks = reportDefinition.EnableHyperlinks;
                localReport.DisplayName = "Export";
            }
            else
            {
                throw new ApplicationException(String.Format("Report {0} could not be found.", reportPath));
            }
        }

        internal static string GetReportPath(string ReportName, bool EnableLocalization, string configNamespace)
        {
            string cultureSuffix = String.Empty;
            string reportPath = String.Format("{0}.Reports.{1}", configNamespace, ReportName);

            if (EnableLocalization)
            {
                cultureSuffix = Common.CultureCode;
                if (reportPath.ToLower().EndsWith(".rdlc"))
                {
                    reportPath = String.Format("{0}.{1}.rdlc", reportPath.Substring(0, reportPath.Length - 5), cultureSuffix);
                }
            }
            return reportPath;
        }

        private static byte[] Export(LocalReport report, ExportRDLCCommand reportDefinition, out string mime, out string ext)
        {
            Warning[] warnings;
            byte[] b;
            string deviceInfo = null;


            if ((reportDefinition.ReportType == ReportType.PDF))
            {
                deviceInfo = String.Format("<DeviceInfo><OutputFormat>{0}</OutputFormat>", reportDefinition.ReportType);

                if (!String.IsNullOrEmpty(reportDefinition.PageWidth))
                    deviceInfo += String.Format("<PageWidth>{0}</PageWidth>", reportDefinition.PageWidth);

                if (!String.IsNullOrEmpty(reportDefinition.PageHeight))
                    deviceInfo += String.Format("<PageHeight>{0}</PageHeight>", reportDefinition.PageHeight);

                if (!String.IsNullOrEmpty(reportDefinition.MarginTop))
                    deviceInfo += String.Format("<MarginTop>{0}</MarginTop>", reportDefinition.MarginTop);

                if (!String.IsNullOrEmpty(reportDefinition.MarginLeft))
                    deviceInfo += String.Format("<MarginLeft>{0}</MarginLeft>", reportDefinition.MarginLeft);

                if (!String.IsNullOrEmpty(reportDefinition.MarginRight))
                    deviceInfo += String.Format("<MarginRight>{0}</MarginRight>", reportDefinition.MarginRight);

                if (!String.IsNullOrEmpty(reportDefinition.MarginBottom))
                    deviceInfo += String.Format("<MarginBottom>{0}</MarginBottom>", reportDefinition.MarginBottom);

                deviceInfo += "</DeviceInfo>";
            }

            string[] streams = null;
            string encoding = null;
            mime = null;
            ext = null;

            b = report.Render(reportDefinition.ReportType.ToString(), deviceInfo, out mime, out encoding, out ext, out streams, out warnings);

            return b;
        }
    }
}
