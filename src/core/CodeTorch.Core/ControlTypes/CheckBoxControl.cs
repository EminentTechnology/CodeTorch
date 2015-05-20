using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class CheckBoxControl: BaseControl
    {
        public string CheckedValue { get; set; }
        public string UncheckedValue { get; set; }

        public string Text { get; set; }
        
        public override string Type
        {
            get
            {
                return "CheckBox";
            }
            set
            {
                base.Type = value;
            }
        }
    }
}
