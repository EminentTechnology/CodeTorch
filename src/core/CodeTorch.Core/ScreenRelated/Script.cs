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

        public bool RenderAtTopOfPage { get; set; } = true;
        public bool IsOnSubmitScript { get; set; } = false;

        public string Path { get; set; }
        public string Assembly { get; set; }

        //used only with bottom scripts or submit scripts
        public string Contents { get; set; }
    }
}
