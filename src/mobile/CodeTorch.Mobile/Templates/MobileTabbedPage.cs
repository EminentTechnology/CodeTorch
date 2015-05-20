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
    public class MobileTabbedPage: TabbedPage, IMobilePage
    {
        public MobileScreen Screen { get; set; }

        public MobileTabbedScreen Me { get; set; }

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

        public MobileTabbedPage(string pageName)
        {
            Screen = MobileScreen.GetByName(pageName);
            Me = (MobileTabbedScreen)Screen;

            InitializeScreen();
        }

        public MobileTabbedPage(MobileScreen screen)
        {
            Screen = screen;
            Me = (MobileTabbedScreen)Screen;

            InitializeScreen();
        }

        private void InitializeScreen()
        {
            PageHelper.DefaultPageProperties(this, Screen);

            if (Me.Tabs.Count > 0)
            {
                foreach (string t in Me.Tabs)
                {
                    ProcessTabScreen(this, Me, t);
                }
            }
        }

        private void ProcessTabScreen(MobileTabbedPage page, MobileTabbedScreen screen, string tabScreen)
        {
           
            Page p = MobilePage.GetPage(tabScreen).GetPage();
            this.Children.Add(p);
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
