using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    public class ResourceItem
    {
        public string ResourceSet { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }


        public string ID { get; set; }
        public string CultureCode { get; set; }
    }
}
