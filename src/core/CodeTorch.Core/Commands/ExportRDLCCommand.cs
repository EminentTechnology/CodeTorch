using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace CodeTorch.Core.Commands
{
    [Serializable]
    public class ExportRDLCCommand : ActionCommand
    {

        string _PageWidth = "8.5in";
        string _PageHeight = "11in";
        string _MarginTop = "1.25in";
        string _MarginLeft = "1in";
        string _MarginRight = "1in";
        string _MarginBottom = "1.25in";
       

        public override string Type
        {
            get
            {
                return "ExportRDLCCommand";
            }
            set
            {
                base.Type = value;
            }
        }

        List<ReportDataSource> _datasources = new List<ReportDataSource>();

        



        [Category("Report")]
        public string ReportName { get; set; }

        [Category("Report")]
        public string ExportFileName { get; set; }

        [Category("Report")]
        public bool EnableHyperlinks { get; set; }

        [Category("Report")]
        public ReportType ReportType { get; set; }

        [Category("Output")]
        public string PageWidth { get; set; }

        [Category("Output")]
        public string PageHeight { get; set; }

        [Category("Output")]
        public string MarginTop { get; set; }

        [Category("Output")]
        public string MarginLeft { get; set; }

        [Category("Output")]
        public string MarginRight { get; set; }

        [Category("Output")]
        public string MarginBottom { get; set; }


        [Category("Report")]
        public bool EnableLocalization { get; set; }


        [Category("Data")]
        [XmlArray("ReportDataSources")]
        [XmlArrayItem("ReportDataSource")]
        public List<ReportDataSource> ReportDataSources
        {
            get
            {
                return _datasources;
            }
            set
            {
                _datasources = value;
            }
        }

       

        List<ReportParameter> _parameters = new List<ReportParameter>();

        [XmlArray("Parameters")]
        [XmlArrayItem("Parameter")]
        [Category("Report")]
        public List<ReportParameter> Parameters
        {
            get
            {
                return _parameters;
            }

        }



        

    }
}
