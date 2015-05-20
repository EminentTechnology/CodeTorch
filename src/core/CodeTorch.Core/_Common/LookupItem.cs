using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class LookupItem
    {
        public string Value { get; set; }
        public string Description { get; set; }
        public string Sort { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
