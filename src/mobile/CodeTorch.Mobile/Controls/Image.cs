using System;
using System.Linq;
using CodeTorch.Core;
using Xamarin.Forms;
using CodeTorch.Core.Platform;

namespace CodeTorch.Mobile
{

    public class Image : Xamarin.Forms.Image, IView
    {

        ImageControl _Me = null;
        public ImageControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (ImageControl)this.BaseControl;
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

            this.IsOpaque = Me.IsOpaque;

            Device.OnPlatform(
                Android: () =>
                {
                    this.Aspect = OnPlatformAspect.GetAndroidValue(Me.Aspect);
                    
                    this.Source = OnPlatformString.GetAndroidValue(Me.Source);

                },
                iOS: () =>
                {
                    this.Aspect = OnPlatformAspect.GetiOSValue(Me.Aspect);
                    this.Source = OnPlatformString.GetiOSValue(Me.Source);
                },
                WinPhone: () =>
                {
                    this.Aspect = OnPlatformAspect.GetWinPhoneValue(Me.Aspect);
                    this.Source = OnPlatformString.GetWinPhoneValue(Me.Source);
                }
          );
        }

        public Xamarin.Forms.View GetView()
        {
            return this as Xamarin.Forms.View;
        }

    }
}
