using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public enum ScreenInputType
    {
        AppSetting,
        Constant,
        Control,
        ControlText,
        Cookie,
        File,
        Form,
        Header,
        QueryString,
        ServerVariables,
        Session,
        Special,
        User
        
    }
}
