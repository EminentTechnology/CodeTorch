using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodeTorch.Core.Platform
{
    public enum KeyboardType
    { 
        NotSet,
        Default,
        Chat,
        Email,
        Numeric,
        Telephone,
        Text, 
        Url
    }
    public class OnPlatformKeyboard
    {
        public OnPlatformKeyboard()
        { }

        public KeyboardType Default { get; set; }
        public KeyboardType Android { get; set; }
        public KeyboardType iOS { get; set; }
        public KeyboardType WinPhone { get; set; }

        public static KeyboardType GetAndroidValue(OnPlatformKeyboard p)
        {
            
            return (p.Android == KeyboardType.NotSet) ? p.Default : p.Android;
        }

        public static KeyboardType GetiOSValue(OnPlatformKeyboard p)
        {
            return (p.iOS == KeyboardType.NotSet) ? p.Default : p.iOS;
        }

        public static KeyboardType GetWinPhoneValue(OnPlatformKeyboard p)
        {
            return (p.WinPhone == KeyboardType.NotSet) ? p.Default : p.WinPhone;
        }
    }
}
