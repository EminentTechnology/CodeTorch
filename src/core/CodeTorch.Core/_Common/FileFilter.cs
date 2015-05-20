using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class FileFilter
    {
        public string Description { get; set; }
        public string Extensions { get; set; }
    }
}
