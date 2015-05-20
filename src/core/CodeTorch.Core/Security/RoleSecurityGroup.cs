using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class RoleSecurityGroup: BaseSecurityGroup
    {
        public override string Type
        {
            get
            {
                return "Role";
            }
            set
            {
                base.Type = value;
            }
        }

        public string Role { get; set; }
    }
}
