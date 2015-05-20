using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class Script
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Assembly { get; set; }
    }
}
