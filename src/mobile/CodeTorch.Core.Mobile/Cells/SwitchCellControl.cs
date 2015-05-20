using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core
{
    public class SwitchCellControl: BaseCell
    {
        public override string Type
        {
            get
            {
                return "Switch";
            }
            set
            {
                base.Type = value;
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

        OnPlatformBool _On = new OnPlatformBool();
        public OnPlatformBool On
        {
            get
            {
                return this._On;
            }
            set
            {
                this._On = value;
            }
        }

        private Action _OnChanged = new Action();
        public Action OnChanged
        {
            get
            {
                return _OnChanged;
            }
            set
            {
                _OnChanged = value;
            }

        }
    }
}
