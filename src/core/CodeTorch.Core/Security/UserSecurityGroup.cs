using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class UserSecurityGroup: BaseSecurityGroup
    {
        public override string Type
        {
            get
            {
                return "User";
            }
            set
            {
                base.Type = value;
            }
        }

        public string User { get; set; }
    }
}
