using CodeTorch.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodeTorch.Mobile.Templates
{
    public interface IMobilePage
    {
        Dictionary<string,Element> Elements { get; }
        MobileScreen Screen { get; set; }
        Page GetPage();

        void DisplayErrorAlert(string p);
    }
}
