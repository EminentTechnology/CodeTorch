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
    public class MobileContentPage: ContentPage, IMobilePage
    {
        public MobileScreen Screen { get; set; }


        public MobileContentScreen Me { get; set; }

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

        public MobileContentPage(string pageName)
        {
            Screen = MobileScreen.GetByName(pageName);
            Me = (MobileContentScreen)Screen;

            InitializeScreen();

            
        }

        public MobileContentPage(MobileScreen screen)
        {
            Screen = screen;
            Me = (MobileContentScreen)Screen;

            InitializeScreen();
        }

        private void InitializeScreen()
        {
            //set screen level properties

            

            PageHelper.DefaultPageProperties(this, Screen);
           

            if (Me.Controls.Count > 0)
            {
                //only 1st control for content
                ProcessControl(this, Me, Me.Controls[0]);
            }
        }

        private void ProcessControl(MobileContentPage page, MobileContentScreen screen, BaseControl control)
        {
            //create control
            //View 

            //initalize controls

            IView view = GetView(page, screen, control);

            ViewHelper.SetupView(page, screen, control, view);

            view.Init();

            this.Content = view.GetView();
            
        }

        

        private IView GetView(Page page, MobileScreen screen, BaseControl control)
        {
            return ViewHelper.GetView(page, screen, control);
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
