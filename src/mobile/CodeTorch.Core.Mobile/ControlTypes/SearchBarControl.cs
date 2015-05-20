using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class SearchBarControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "SearchBar";
            }
            set
            {
                base.Type = value;
            }
        }

    }
}
