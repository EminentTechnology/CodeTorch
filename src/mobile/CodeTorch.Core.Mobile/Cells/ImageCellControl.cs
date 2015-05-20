using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core
{
    public class ImageCellControl: TextCellControl
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
