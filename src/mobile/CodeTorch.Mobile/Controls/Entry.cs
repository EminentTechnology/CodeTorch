using System;
using System.Linq;
using CodeTorch.Core;
using Xamarin.Forms;
using CodeTorch.Core.Platform;

namespace CodeTorch.Mobile
{

    public class Entry : Xamarin.Forms.Entry, IView
    {

        EntryControl _Me = null;
        public EntryControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (EntryControl)this.BaseControl;
                }
                return _Me;
            }
        }

        public BaseControl BaseControl { get; set; }
        public Page Page { get; set; }
        public MobileScreen Screen { get; set; }
        public string Name { get; set; }
        public string Value 
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        public void Init()
        {
            ViewHelper.DefaultViewProperties(this);

            this.IsPassword = Me.IsPassword;

            Device.OnPlatform(
                Android: () =>
                {


                    
                    this.Text = OnPlatformString.GetAndroidValue(Me.Text);
                    this.Placeholder = OnPlatformString.GetAndroidValue(Me.Placeholder);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.TextColor)))
                        this.TextColor = Color.FromHex(OnPlatformString.GetAndroidValue(Me.TextColor));

                },
                iOS: () =>
                {
                    

                    this.Text = OnPlatformString.GetiOSValue(Me.Text);
                    this.Placeholder = OnPlatformString.GetiOSValue(Me.Placeholder);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.TextColor)))
                        this.TextColor = Color.FromHex(OnPlatformString.GetiOSValue(Me.TextColor));
                },
                WinPhone: () =>
                {
                    

                    this.Text = OnPlatformString.GetWinPhoneValue(Me.Text);
                    this.Placeholder = OnPlatformString.GetWinPhoneValue(Me.Placeholder);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.TextColor)))
                        this.TextColor = Color.FromHex(OnPlatformString.GetWinPhoneValue(Me.TextColor));
                }
          );
        }

        public Xamarin.Forms.View GetView()
        {
            return this as Xamarin.Forms.View;
        }

    }
}
