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
        Control,
        ControlText,
        Cookie,
        File,
        Form,
        Constant,
        QueryString,
        Session,
        Special,
        User,
        DashboardSetting,
        ServerVariables
    }
}
