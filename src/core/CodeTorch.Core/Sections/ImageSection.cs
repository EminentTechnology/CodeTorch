using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class ImageSection: Section
    {
        public override string Type
        {
            get
            {
                return "Image";
            }
            set
            {
                base.Type = value;
            }
        }
    }
}
