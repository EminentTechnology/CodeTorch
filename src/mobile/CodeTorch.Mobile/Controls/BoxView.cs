using System;
using System.Linq;
using CodeTorch.Core;
using Xamarin.Forms;
using CodeTorch.Core.Platform;

namespace CodeTorch.Mobile
{

    public class BoxView : Xamarin.Forms.BoxView, IView
    {

        BoxViewControl _Me = null;
        public BoxViewControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (BoxViewControl)this.BaseControl;
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

            Device.OnPlatform(
               Android: () =>
               {

                   if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.Color)))
                       this.Color = Color.FromHex(OnPlatformString.GetAndroidValue(Me.Color));



               },
               iOS: () =>
               {
                   if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.Color)))
                       this.Color = Color.FromHex(OnPlatformString.GetAndroidValue(Me.Color));

               },
               WinPhone: () =>
               {
                   if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.Color)))
                       this.Color = Color.FromHex(OnPlatformString.GetAndroidValue(Me.Color));

               });
        }

        public Xamarin.Forms.View GetView()
        {
            return this as Xamarin.Forms.View;
        }

    }
}
