using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class NumericTextBoxControl: Widget
    {

        public override string Type
        {
            get
            {
                return "NumericTextBox";
            }
            set
            {
                base.Type = value;
            }
        }

        [Category("Appearance")]
        public string Skin { get; set; }

        

        double _MinValue = double.MinValue;

        public double MinValue
        {
            get { return _MinValue; }
            set { _MinValue = value; }
        }

        double _MaxValue = double.MaxValue;

        public double MaxValue
        {
            get { return _MaxValue; }
            set { _MaxValue = value; }
        }

        int _DecimalDigits = 2;

        public int DecimalDigits
        {
            get { return _DecimalDigits; }
            set { _DecimalDigits = value; }
        }

        string _DecimalSeparator = ".";

        public string DecimalSeparator
        {
            get { return _DecimalSeparator; }
            set { _DecimalSeparator = value; }
        }

        string _GroupSeparator = ",";

        public string GroupSeparator
        {
            get { return _GroupSeparator; }
            set { _GroupSeparator = value; }
        }

        int _GroupSizes = 3;

        public int GroupSizes
        {
            get { return _GroupSizes; }
            set { _GroupSizes = value; }
        }


        
    }
}
