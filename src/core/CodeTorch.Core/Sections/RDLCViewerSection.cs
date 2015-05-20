using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    [Serializable]
    public class RDLCViewerSection : BaseSection
    {
        public override string Type
        {
            get
            {
                return "RDLCViewer";
            }
            set
            {
                base.Type = value;
            }
        }

        List<ReportDataSource> _datasources = new List<ReportDataSource>();
       
        bool _LoadDataOnPageLoad = false;


       

        [Category("Report")]
        public string ReportName { get; set; }

        [Category("Report")]
        public bool EnableHyperlinks { get; set; }

        [Category("Report")]
        public bool EnableLocalization { get; set; }


        

        [Category("Report")]
        public string HelpText { get; set; }

        

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

        

        [Category("Data")]
        public bool LoadDataOnPageLoad { get { return _LoadDataOnPageLoad; } set { _LoadDataOnPageLoad = value; } }

     

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


        #region Hidden Overrides
        [Browsable(false)]
        public override List<BaseControl> Controls
        {
            get
            {
                return base.Controls;
            }
            set
            {
                base.Controls = value;
            }
        }
        #endregion
    }
}
