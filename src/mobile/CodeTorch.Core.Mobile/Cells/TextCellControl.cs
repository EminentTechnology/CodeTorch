using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core
{
    public class TextCellControl: BaseCell
    {
        public override string Type
        {
            get
            {
                return "Text";
            }
            set
            {
                base.Type = value;
            }
        }

        OnPlatformString _Detail = new OnPlatformString();
        public OnPlatformString Detail
        {
            get
            {
                return this._Detail;
            }
            set
            {
                this._Detail = value;
            }
        }

        OnPlatformString _DetailColor = new OnPlatformString();
        public OnPlatformString DetailColor
        {
            get
            {
                return this._DetailColor;
            }
            set
            {
                this._DetailColor = value;
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
