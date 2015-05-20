using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class RegularExpressionValidator : BaseValidator
    {
        public string ValidationExpression { get; set; }
    }
}
