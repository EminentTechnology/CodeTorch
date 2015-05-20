using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class DatePickerControl : BaseControl
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

    }
}
