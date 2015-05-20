using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class FrameControl: BaseControl
    {

        public override string Type
        {
            get
            {
                return "Frame";
            }
            set
            {
                base.Type = value;
            }
        }

    }
}
