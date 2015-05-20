using System;
using System.Linq;
using CodeTorch.Core;
using Xamarin.Forms;
using CodeTorch.Core.Platform;

namespace CodeTorch.Mobile
{

    public class Label : Xamarin.Forms.Label, IView
    {

        LabelControl _Me = null;
        public LabelControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (LabelControl)this.BaseControl;
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
                    this.Text = OnPlatformString.GetAndroidValue(Me.Text);
                },
                iOS: () =>
                {
                    this.Text = OnPlatformString.GetiOSValue(Me.Text);
                },
                WinPhone: () =>
                {
                    this.Text = OnPlatformString.GetWinPhoneValue(Me.Text);
                }
          );
        }

        public Xamarin.Forms.View GetView()
        {
            return this as Xamarin.Forms.View;
        }

    }
}
