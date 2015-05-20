using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class ProgressBarControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "ProgressBar";
            }
            set
            {
                base.Type = value;
            }
        }

    }
}
