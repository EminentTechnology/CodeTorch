using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class DetailsSection: BaseSection
    {
        public override string Type
        {
            get
            {
                return "Details";
            }
            set
            {
                base.Type = value;
            }
        }

        public string LabelWidth { get; set; }



        
    }
}
