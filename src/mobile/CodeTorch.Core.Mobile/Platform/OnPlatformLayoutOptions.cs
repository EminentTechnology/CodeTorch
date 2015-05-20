using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodeTorch.Core.Platform
{
    public enum FormLayoutOptions
    { 
        NotSet,
        Center,
        CenterAndExpand,
        End,
        EndAndExpand,
        Fill, 
        FillAndExpand,
        Start,
        StartAndExpand
    }
    public class OnPlatformLayoutOptions
    {
        public OnPlatformLayoutOptions()
        { }

        public FormLayoutOptions Default { get; set; }
        public FormLayoutOptions Android { get; set; }
        public FormLayoutOptions iOS { get; set; }
        public FormLayoutOptions WinPhone { get; set; }

        public static FormLayoutOptions GetAndroidValue(OnPlatformLayoutOptions p)
        {
            
            return (p.Android == FormLayoutOptions.NotSet) ? p.Default : p.Android;
        }

        public static FormLayoutOptions GetiOSValue(OnPlatformLayoutOptions p)
        {
            return (p.iOS == FormLayoutOptions.NotSet) ? p.Default : p.iOS;
        }

        public static FormLayoutOptions GetWinPhoneValue(OnPlatformLayoutOptions p)
        {
            return (p.WinPhone == FormLayoutOptions.NotSet) ? p.Default : p.WinPhone;
        }
    }
}
