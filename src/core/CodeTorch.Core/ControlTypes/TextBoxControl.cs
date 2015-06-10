using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class TextBoxControl: Widget
    {

        public override string Type
        {
            get
            {
                return "TextBox";
            }
            set
            {
                base.Type = value;
            }
        }

     
    }
}
