using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class StepperControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "Stepper";
            }
            set
            {
                base.Type = value;
            }
        }

    }
}
