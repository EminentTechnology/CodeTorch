using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class ImageControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "Image";
            }
            set
            {
                base.Type = value;
            }
        }

        OnPlatformAspect _Aspect = new OnPlatformAspect();
        public OnPlatformAspect Aspect
        {
            get
            {
                return this._Aspect;
            }
            set
            {
                this._Aspect = value;
            }
        }

        public bool IsOpaque { get; set; }

        OnPlatformString _Source = new OnPlatformString();
        public OnPlatformString Source
        {
            get
            {
                return this._Source;
            }
            set
            {
                this._Source = value;
            }
        }

    }
}
