using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class SwitchControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "Switch";
            }
            set
            {
                base.Type = value;
            }
        }

    }
}
