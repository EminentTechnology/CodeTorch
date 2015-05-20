using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core.Platform
{
    public class OnPlatformFont
    {
         public OnPlatformFont()
        { }

        

        public string Default { get; set; }
        public string Android { get; set; }
        public string iOS { get; set; }
        public string WinPhone { get; set; }
    }
}
