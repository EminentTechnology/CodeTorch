using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class GenericControl: BaseControl
    {

        public override string Type
        {
            get
            {
                return "Generic";
            }
            set
            {
                base.Type = value;
            }
        }

        public string Path { get; set; }
    }
}
