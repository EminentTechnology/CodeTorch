using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodeTorch.Core.Platform
{
    public class OnPlatformAspect
    {
        public OnPlatformAspect()
        { }

        public Aspect Default { get; set; }
        public Aspect Android { get; set; }
        public Aspect iOS { get; set; }
        public Aspect WinPhone { get; set; }

        public static Aspect GetAndroidValue(OnPlatformAspect p)
        {
            return (p.Android == Aspect.AspectFit) ? p.Default : p.Android;
        }

        public static Aspect GetiOSValue(OnPlatformAspect p)
        {
            return (p.iOS == Aspect.AspectFit) ? p.Default : p.iOS;
        }

        public static Aspect GetWinPhoneValue(OnPlatformAspect p)
        {
            return (p.WinPhone == Aspect.AspectFit) ? p.Default : p.WinPhone;
        }
    }
}
