using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]

    public enum WindowsAuthenticationUserNameFormat
    { 
        UserNameOnly = 0,
        FullUserName
    }

    public class WindowsAuthenticationMode : BaseAuthenticationMode
    {
        public override string Type
        {
            get
            {
                return "Windows";
            }
            set
            {
                base.Type = value;
            }
        }

        public WindowsAuthenticationUserNameFormat UserNameFormat { get; set; } 
    }
}
