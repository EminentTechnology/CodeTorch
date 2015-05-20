using CodeTorch.Core;
using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodeTorch.Mobile.Templates
{
    public class MobileNavigationPage: NavigationPage, IMobilePage
    {
        public MobileScreen Screen { get; set; }


        public MobileNavigationScreen Me { get; set; }

        Dictionary<string, Element> _Elements = null;
        public Dictionary<string, Element> Elements
        {
            get
            {
                if (_Elements == null)
                {
                    _Elements = new Dictionary<string, Element>();
                }
                return _Elements;
            }
        }

        public MobileNavigationPage(string pageName, Page root): base(root)
        {
            Screen = MobileScreen.GetByName(pageName);
            Me = (MobileNavigationScreen)Screen;

            InitializeScreen();
        }

        public MobileNavigationPage(MobileScreen screen, Page root): base(root)
        {
            Screen = screen;
            Me = (MobileNavigationScreen)Screen;

            InitializeScreen();
        }

        private void InitializeScreen()
        {
            PageHelper.DefaultPageProperties(this, Screen);

            //set navigation screen properties
            Device.OnPlatform(
                Android: () =>
                {
                    if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.BarBackgroundColor)))
                        this.BarBackgroundColor = Color.FromHex(OnPlatformString.GetAndroidValue(Me.BarBackgroundColor));

                    if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.BarTextColor)))
                        this.BarTextColor = Color.FromHex(OnPlatformString.GetAndroidValue(Me.BarTextColor));

                    

                },
                iOS: () =>
                {
                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.BarBackgroundColor)))
                        this.BarBackgroundColor = Color.FromHex(OnPlatformString.GetiOSValue(Me.BarBackgroundColor));

                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.BarTextColor)))
                        this.BarTextColor = Color.FromHex(OnPlatformString.GetiOSValue(Me.BarTextColor));
                },
                WinPhone: () =>
                {
                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.BarBackgroundColor)))
                        this.BarBackgroundColor = Color.FromHex(OnPlatformString.GetWinPhoneValue(Me.BarBackgroundColor));

                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.BarTextColor)))
                        this.BarTextColor = Color.FromHex(OnPlatformString.GetWinPhoneValue(Me.BarTextColor));
                }
          );

            
        }

        public Xamarin.Forms.Page GetPage()
        {
            return this as Xamarin.Forms.Page;
        }
        
        public void DisplayErrorAlert(string message)
        {
            PageHelper.DisplayErrorAlert(this, message);
        }

        
    }
}
