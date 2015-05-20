using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class DatePickerControl: BaseControl
    {
        


        public override string Type
        {
            get
            {
                return "DatePicker";
            }
            set
            {
                base.Type = value;
            }
        }

        string _DateFormat = "dd-MM-yyyy";
        string _DisplayDateFormat=  "dd MMM yyyy";

        public string DateFormat
        {
            get { return _DateFormat; }
            set { _DateFormat = value; }
        }
        

        public string DisplayDateFormat
        {
            get { return _DisplayDateFormat; }
            set { _DisplayDateFormat = value; }
        }

        DateTime _MinDate = DateTime.MinValue;

        public DateTime MinDate
        {
            get { return _MinDate; }
            set { _MinDate = value; }
        }

        DateTime _MaxDate = DateTime.MaxValue;

        public DateTime MaxDate
        {
            get { return _MaxDate; }
            set { _MaxDate = value; }
        }

        [Category("Appearance")]
        public string Skin { get; set; }
    }
}
