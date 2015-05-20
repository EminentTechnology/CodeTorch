using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core.Platform
{
    public class OnPlatformDouble
    {
        public OnPlatformDouble()
        { }

        public OnPlatformDouble(double d)
        {
            this.Default = d;
        }

        public double Default { get; set; }
        public double Android { get; set; }
        public double iOS { get; set; }
        public double WinPhone { get; set; }


        public static double GetAndroidValue(OnPlatformDouble p)
        {
            return (p.Android == 0) ? p.Default : p.Android; 
        }

        public static double GetiOSValue(OnPlatformDouble p)
        {
            return (p.iOS == 0) ? p.Default : p.iOS;
        }

        public static double GetWinPhoneValue(OnPlatformDouble p)
        {
            return (p.WinPhone == 0) ? p.Default : p.WinPhone;
        }
    }
}
