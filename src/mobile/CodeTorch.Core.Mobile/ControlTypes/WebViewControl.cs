using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class WebViewControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "WebView";
            }
            set
            {
                base.Type = value;
            }
        }

    }
}
