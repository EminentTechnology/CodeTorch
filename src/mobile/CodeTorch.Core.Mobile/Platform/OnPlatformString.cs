using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core.Platform
{
    public class OnPlatformString
    {
        public OnPlatformString()
        { }

        public string Default { get; set; }
        public string Android { get; set; }
        public string iOS { get; set; }
        public string WinPhone { get; set; }

        public static string GetAndroidValue(OnPlatformString p)
        {
            return (String.IsNullOrEmpty(p.Android)) ? p.Default : p.Android;
        }

        public static string GetiOSValue(OnPlatformString p)
        {
            return (String.IsNullOrEmpty(p.iOS)) ? p.Default : p.iOS;
        }

        public static string GetWinPhoneValue(OnPlatformString p)
        {
            return (String.IsNullOrEmpty(p.WinPhone)) ? p.Default : p.WinPhone;
        }
    }
}
