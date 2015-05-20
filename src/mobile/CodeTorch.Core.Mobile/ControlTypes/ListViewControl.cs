using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class ListViewControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "ListView";
            }
            set
            {
                base.Type = value;
            }
        }

    }
}
