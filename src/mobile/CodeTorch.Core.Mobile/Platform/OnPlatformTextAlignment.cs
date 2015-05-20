using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodeTorch.Core.Platform
{
    public enum TextAlignmentType
    { 
        NotSet,
        Start,
        Center,
        End
        
    }
    public class OnPlatformTextAlignment
    {
        public OnPlatformTextAlignment()
        { }

        public TextAlignmentType Default { get; set; }
        public TextAlignmentType Android { get; set; }
        public TextAlignmentType iOS { get; set; }
        public TextAlignmentType WinPhone { get; set; }

        public static TextAlignmentType GetAndroidValue(OnPlatformTextAlignment p)
        {
            
            return (p.Android == TextAlignmentType.NotSet) ? p.Default : p.Android;
        }

        public static TextAlignmentType GetiOSValue(OnPlatformTextAlignment p)
        {
            return (p.iOS == TextAlignmentType.NotSet) ? p.Default : p.iOS;
        }

        public static TextAlignmentType GetWinPhoneValue(OnPlatformTextAlignment p)
        {
            return (p.WinPhone == TextAlignmentType.NotSet) ? p.Default : p.WinPhone;
        }
    }
}
