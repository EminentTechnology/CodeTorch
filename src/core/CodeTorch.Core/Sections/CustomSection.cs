using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core
{
    [Serializable]
    public class CustomSection: BaseSection
    {
        public override string Type
        {
            get
            {
                return "Custom";
            }
            set
            {
                base.Type = value;
            }
        }

        public string UserControlPath { get; set; }
        public string Assembly { get; set; }
        public string Class { get; set; }
    }
}
