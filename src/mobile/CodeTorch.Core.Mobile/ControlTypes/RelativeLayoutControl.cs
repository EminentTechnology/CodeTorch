using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class RelativeLayoutControl: BaseControl
    {

        public override string Type
        {
            get
            {
                return "RelativeLayout";
            }
            set
            {
                base.Type = value;
            }
        }

    }
}
