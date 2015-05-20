using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class BoxViewControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "BoxView";
            }
            set
            {
                base.Type = value;
            }
        }

        OnPlatformString _Color = new OnPlatformString();
        public OnPlatformString Color
        {
            get
            {
                return this._Color;
            }
            set
            {
                this._Color = value;
            }
        }

    }
}
