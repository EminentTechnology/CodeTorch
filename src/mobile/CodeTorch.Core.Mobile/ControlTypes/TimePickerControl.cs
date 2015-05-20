using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class TimePickerControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "TimePicker";
            }
            set
            {
                base.Type = value;
            }
        }

    }
}
