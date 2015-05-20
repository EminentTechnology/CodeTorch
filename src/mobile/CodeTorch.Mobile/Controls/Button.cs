using System;
using System.Linq;
using CodeTorch.Core;
using CodeTorch.Core.Platform;
using Xamarin.Forms;

namespace CodeTorch.Mobile
{

    public class Button : Xamarin.Forms.Button, IView
    {

        ButtonControl _Me = null;
        public ButtonControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (ButtonControl)this.BaseControl;
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
                   //this.BorderWidth = OnPlatformInt.GetAndroidValue(Me.BorderWidth);
                   //this.BorderRadius = OnPlatformInt.GetAndroidValue(Me.BorderRadius);

                   if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.BorderColor)))
                        this.BorderColor = Color.FromHex( OnPlatformString.GetAndroidValue(Me.BorderColor));

                   if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.Image)))
                   {
                       try
                       {
                           this.Image = ImageSource.FromFile(OnPlatformString.GetAndroidValue(Me.Image)) as FileImageSource;
                       }
                       catch (Exception ex)
                       { 
                            System.Diagnostics.Debug.WriteLine("Error while trying to load button image {1}: {0} " ,ex.Message, Me.Image );
                       }
                   }
          
                   
                   this.Text = OnPlatformString.GetAndroidValue(Me.Text);

                   if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.TextColor)))
                       this.TextColor = Color.FromHex(OnPlatformString.GetAndroidValue(Me.TextColor));
          
               },
               iOS: () =>
               {
               

                   if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.BorderColor)))
                        this.BorderColor = Color.FromHex( OnPlatformString.GetiOSValue(Me.BorderColor));

                   if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.Image)))
                   {
                       try
                       {
                           this.Image = ImageSource.FromFile(OnPlatformString.GetiOSValue(Me.Image)) as FileImageSource;
                       }
                       catch (Exception ex)
                       { 
                            System.Diagnostics.Debug.WriteLine("Error while trying to load button image {1}: {0} " ,ex.Message, Me.Image );
                       }
                   }
          
                   
                   this.Text = OnPlatformString.GetiOSValue(Me.Text);

                   if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.TextColor)))
                       this.TextColor = Color.FromHex(OnPlatformString.GetiOSValue(Me.TextColor));
               },
               WinPhone: () =>
               {
                  
                   if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.BorderColor)))
                        this.BorderColor = Color.FromHex( OnPlatformString.GetWinPhoneValue(Me.BorderColor));

                   if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.Image)))
                   {
                       try
                       {
                           this.Image = ImageSource.FromFile(OnPlatformString.GetWinPhoneValue(Me.Image)) as FileImageSource;
                       }
                       catch (Exception ex)
                       { 
                            System.Diagnostics.Debug.WriteLine("Error while trying to load button image {1}: {0} " ,ex.Message, Me.Image );
                       }
                   }
          
                   
                   this.Text = OnPlatformString.GetWinPhoneValue(Me.Text);

                   if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.TextColor)))
                       this.TextColor = Color.FromHex(OnPlatformString.GetWinPhoneValue(Me.TextColor));
               }

               
            );

            this.Clicked += Button_Clicked;
        }

        void Button_Clicked(object sender, EventArgs e)
        {
            //log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


            try
            {
                ActionRunner runner = new ActionRunner();
                Core.Action action = Me.OnClick.Clone();


                if (action != null)
                {
                    runner.Page = this.Page;
                    runner.Action = action;
                    runner.Execute();
                }

                
            }
            catch (Exception ex)
            {
                //log.Error(ex);
                //((BasePage)this.Page).DisplayErrorAlert(ex);

            }
        }

        public Xamarin.Forms.View GetView()
        {
            return this as Xamarin.Forms.View;
        }

    }
}
