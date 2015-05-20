using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CodeTorch.Core
{

    public class StackLayoutControl: BaseControl
    {

        public override string Type
        {
            get
            {
                return "StackLayout";
            }
            set
            {
                base.Type = value;
            }
        }

        public StackOrientation Orientation = StackOrientation.Vertical;

        OnPlatformDouble _Spacing = new OnPlatformDouble(6);
        public OnPlatformDouble Spacing
        {
            get
            {
                return this._Spacing;
            }
            set
            {
                this._Spacing = value;
            }
        }
    }
}
