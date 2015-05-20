using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class EditorControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "Editor";
            }
            set
            {
                base.Type = value;
            }
        }

    }
}
