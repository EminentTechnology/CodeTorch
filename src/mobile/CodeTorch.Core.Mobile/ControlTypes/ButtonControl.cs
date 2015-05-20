using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{

    public class ButtonControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "Button";
            }
            set
            {
                base.Type = value;
            }
        }

        OnPlatformString _BorderColor = new OnPlatformString();
        public OnPlatformString BorderColor
        {
            get
            {
                return this._BorderColor;
            }
            set
            {
                this._BorderColor = value;
            }
        }

        OnPlatformInt _BorderRadius = new OnPlatformInt(5);
        public OnPlatformInt BorderRadius
        {
            get
            {
                return this._BorderRadius;
            }
            set
            {
                this._BorderRadius = value;
            }
        }

        OnPlatformInt _BorderWidth = new OnPlatformInt(0);
        public OnPlatformInt BorderWidth
        {
            get
            {
                return this._BorderWidth;
            }
            set
            {
                this._BorderWidth = value;
            }
        }

        OnPlatformFont _Font = new OnPlatformFont();
        public OnPlatformFont Font
        {
            get
            {
                return this._Font;
            }
            set
            {
                this._Font = value;
            }
        }

        OnPlatformString _Image = new OnPlatformString();
        public OnPlatformString Image
        {
            get
            {
                return this._Image;
            }
            set
            {
                this._Image = value;
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

        private Action _OnClick = new Action();
        public Action OnClick
        {
            get
            {
                return _OnClick;
            }
            set
            {
                _OnClick = value;
            }

        }

    }
}
