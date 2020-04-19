using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class CompareValidator: BaseValidator
    {
        public ValidationDataType Type { get; set; }

        public ValidationCompareOperator Operator { get; set; }

        public string ControlToCompare { get; set; }
        
        public string ValueToCompare { get; set; }

    }
}
