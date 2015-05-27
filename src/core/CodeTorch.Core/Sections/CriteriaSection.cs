

using System;
using System.ComponentModel;
using System.Linq;


namespace CodeTorch.Core
{
    [Serializable]
    public class CriteriaSection : Section
    {
        public override string Type
        {
            get
            {
                return "Criteria";
            }
            set
            {
                base.Type = value;
            }
        }

        int _ControlsPerRow = 5;
        string _ActionButtonText = "Search";
        //string _SectionHeader = "ENTITY Search Criteria";
        string _HelpText = "Locate any ENTITY in the system by entering data into the fields below:";
        string _ContainerWidth = "90%";

        public int ControlsPerRow { get { return _ControlsPerRow; } set { _ControlsPerRow = value; } }

        public string ActionButtonText { get { return _ActionButtonText; } set { _ActionButtonText = value; } }

        [Category("Common")]
        public string RowElement { get; set; }

        [Category("Common")]
        public string RowCssClass { get; set; }

        [Category("Common")]
        public string ColumnElement { get; set; }

        [Category("Common")]
        public string ColumnCssClass { get; set; }

       
        public string HelpText { get { return _HelpText; } set { _HelpText = value; } }

        public string ContainerWidth { get { return _ContainerWidth; } set { _ContainerWidth = value; } }


        

        public override string ToString()
        {
            return String.Format("{0} - {1} Controls", Name, this.Controls.Count);
        }

        

        
    }
}
