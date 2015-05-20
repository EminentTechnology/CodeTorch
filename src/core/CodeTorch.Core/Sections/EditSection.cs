using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class EditSection: BaseSection
    {

        public override string Type
        {
            get
            {
                return "Edit";
            }
            set
            {
                base.Type = value;
            }
        }

        public string LabelWidth { get; set; }

        

       
    }
}
