using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class RangeValidator : BaseValidator
    {
        public ValidationDataType Type { get; set; }

        public string MaximumValue { get; set; }

        public string MinimumValue { get; set; }
    }
}
