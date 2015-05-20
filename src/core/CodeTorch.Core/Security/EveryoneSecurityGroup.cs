using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class EveryoneSecurityGroup: BaseSecurityGroup
    {
        public override string Type
        {
            get
            {
                return "Everyone";
            }
            set
            {
                base.Type = value;
            }
        }
    }
}
