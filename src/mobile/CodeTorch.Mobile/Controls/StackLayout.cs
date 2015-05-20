using System;
using System.Linq;
using CodeTorch.Core;
using CodeTorch.Core.Platform;
using Xamarin.Forms;
using CodeTorch.Mobile.Templates;

namespace CodeTorch.Mobile
{

    public class StackLayout : Xamarin.Forms.StackLayout, IView
    {

        StackLayoutControl _Me = null;
        

        public StackLayoutControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (StackLayoutControl)this.BaseControl;
                }
                return _Me;
            }
        }

        public BaseControl BaseControl { get; set; }
        public Page Page { get; set; }
        public MobileScreen Screen { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        

        public void Init()
        {
            
          ViewHelper.DefaultViewProperties(this);

            this.Orientation = Me.Orientation;

            Device.OnPlatform(
                Android: () =>
                {
                    this.Spacing = OnPlatformDouble.GetAndroidValue(Me.Spacing); 
                },
                iOS: () =>
                {
                    this.Spacing = OnPlatformDouble.GetiOSValue(Me.Spacing); 
                },
                WinPhone: () =>
                {
                    this.Spacing = OnPlatformDouble.GetWinPhoneValue(Me.Spacing); 
                }
            );

            if (Me.Controls.Count > 0)
            {
                foreach (BaseControl control in Me.Controls)
                {
                    ProcessControl( control);
                }
            }
        }

        private void ProcessControl(BaseControl control)
        {

            IView view = GetView(this.Page, Screen, control);
            ViewHelper.SetupView(this.Page as IMobilePage, this.Screen, control, view);
            view.Init();

            this.Children.Add(view.GetView());
            

        }

        private IView GetView(Page page, MobileScreen screen, BaseControl control)
        {
            return ViewHelper.GetView(page, screen, control);
        }


        public Xamarin.Forms.View GetView()
        {
            return this as Xamarin.Forms.View;
        }

    }
}
