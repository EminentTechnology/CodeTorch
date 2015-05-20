using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CodeTorch.Core
{

    public class LabelControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "Label";
            }
            set
            {
                base.Type = value;
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

        OnPlatformString _FormattedText = new OnPlatformString();
        public OnPlatformString FormattedText
        {
            get
            {
                return this._FormattedText;
            }
            set
            {
                this._FormattedText = value;
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

        public LineBreakMode LineBreakMode {get;set;}

        public TextAlignment XAlign { get; set; }
        public TextAlignment YAlign { get; set; }

    }
}
