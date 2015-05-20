using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core
{
    public class EntryCellControl: BaseCell
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

        OnPlatformKeyboard _Keyboard = new OnPlatformKeyboard();
        public OnPlatformKeyboard Keyboard
        {
            get
            {
                return this._Keyboard;
            }
            set
            {
                this._Keyboard = value;
            }
        }

        OnPlatformString _Label = new OnPlatformString();
        public OnPlatformString Label
        {
            get
            {
                return this._Label;
            }
            set
            {
                this._Label = value;
            }
        }

        OnPlatformString _LabelColor = new OnPlatformString();
        public OnPlatformString LabelColor
        {
            get
            {
                return this._LabelColor;
            }
            set
            {
                this._LabelColor = value;
            }
        }

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

        OnPlatformTextAlignment _XAlign = new OnPlatformTextAlignment();
        public OnPlatformTextAlignment XAlign
        {
            get
            {
                return this._XAlign;
            }
            set
            {
                this._XAlign = value;
            }
        }

        private Action _OnCompleted = new Action();
        public Action OnCompleted
        {
            get
            {
                return _OnCompleted;
            }
            set
            {
                _OnCompleted = value;
            }

        }
    }
}
