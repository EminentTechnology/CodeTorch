using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class EntryControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "Entry";
            }
            set
            {
                base.Type = value;
            }
        }

        public bool IsPassword { get; set; }

        OnPlatformString _Placeholder = new OnPlatformString();
        public OnPlatformString Placeholder
        {
            get
            {
                return this._Placeholder;
            }
            set
            {
                this._Placeholder = value;
            }
        }

        OnPlatformString _Text = new OnPlatformString();
        public OnPlatformString Text
        {
            get
            {
                return this._Text;
            }
            set
            {
                this._Text = value;
            }
        }

        OnPlatformString _TextColor = new OnPlatformString();
        public OnPlatformString TextColor
        {
            get
            {
                return this._TextColor;
            }
            set
            {
                this._TextColor = value;
            }
        }

    }
}
