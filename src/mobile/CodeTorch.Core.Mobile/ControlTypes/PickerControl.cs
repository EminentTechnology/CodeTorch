using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class PickerControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "Picker";
            }
            set
            {
                base.Type = value;
            }
        }

    }
}
