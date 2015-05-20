using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class AbsoluteLayoutControl: BaseControl
    {

        public override string Type
        {
            get
            {
                return "AbsoluteLayout";
            }
            set
            {
                base.Type = value;
            }
        }

        

    }
}
