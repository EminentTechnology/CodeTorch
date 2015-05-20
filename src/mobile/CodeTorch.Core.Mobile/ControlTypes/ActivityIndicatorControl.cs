using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class ActivityIndicatorControl: BaseControl
    {

        public override string Type
        {
            get
            {
                return "ActivityIndicator";
            }
            set
            {
                base.Type = value;
            }
        }

    }
}
