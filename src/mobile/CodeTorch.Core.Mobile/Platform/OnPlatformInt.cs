using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core.Platform
{
    public class OnPlatformInt
    {
        public OnPlatformInt()
        { }

        public OnPlatformInt(int d)
        {
            this.Default = d;
            
        }

        
        public int Default { get; set; }
        public int Android { get; set; }
        public int iOS { get; set; }
        public int WinPhone { get; set; }


        public static int GetAndroidValue(OnPlatformInt p)
        {
            return (p.Android == 0) ? p.Default : p.Android; 
        }

        public static int GetiOSValue(OnPlatformInt p)
        {
            return (p.iOS == 0) ? p.Default : p.iOS;
        }

        public static int GetWinPhoneValue(OnPlatformInt p)
        {
            return (p.WinPhone == 0) ? p.Default : p.WinPhone;
        }
    }
}
