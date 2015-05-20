using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class SliderControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "Slider";
            }
            set
            {
                base.Type = value;
            }
        }

    }
}
