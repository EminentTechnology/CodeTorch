using CodeTorch.Core;
using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodeTorch.Mobile
{
    public class ImageCell: Xamarin.Forms.ImageCell, ICell
    {
        public BaseControl BaseControl { get; set; }
        public Page Page { get; set; }
        public BaseCell Cell { get; set; }
        public BaseSection Section { get; set; }

        ImageCellControl _Me = null;
        public ImageCellControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (ImageCellControl)this.Cell;
                }
                return _Me;
            }
        }


        public void Init()
        {
            CellHelper.DefaultViewProperties(this);

            Device.OnPlatform(
                Android: () =>
                {

                    
                   if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.Text)))
                        this.Text =  OnPlatformString.GetAndroidValue(Me.Text);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.TextColor)))
                        this.TextColor = Color.FromHex( OnPlatformString.GetAndroidValue(Me.TextColor));

                    if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.Detail)))
                        this.Detail =  OnPlatformString.GetAndroidValue(Me.Detail);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.DetailColor)))
                        this.DetailColor = Color.FromHex( OnPlatformString.GetAndroidValue(Me.DetailColor));

                    this.ImageSource = OnPlatformString.GetAndroidValue(Me.Source);

                },
                iOS: () =>
                {
                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.Text)))
                        this.Text =  OnPlatformString.GetiOSValue(Me.Text);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.TextColor)))
                        this.TextColor = Color.FromHex( OnPlatformString.GetiOSValue(Me.TextColor));

                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.Detail)))
                        this.Detail =  OnPlatformString.GetiOSValue(Me.Detail);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.DetailColor)))
                        this.DetailColor = Color.FromHex( OnPlatformString.GetiOSValue(Me.DetailColor));

                    this.ImageSource = OnPlatformString.GetiOSValue(Me.Source);
            
                 
                },
                WinPhone: () =>
                {
                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.Text)))
                        this.Text =  OnPlatformString.GetWinPhoneValue(Me.Text);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.TextColor)))
                        this.TextColor = Color.FromHex( OnPlatformString.GetWinPhoneValue(Me.TextColor));

                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.Detail)))
                        this.Detail =  OnPlatformString.GetWinPhoneValue(Me.Detail);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.DetailColor)))
                        this.DetailColor = Color.FromHex( OnPlatformString.GetWinPhoneValue(Me.DetailColor));

                    this.ImageSource = OnPlatformString.GetWinPhoneValue(Me.Source);
            
                   
                }
          );

     
    }

        public Xamarin.Forms.Cell GetCell()
        {
            return this as Xamarin.Forms.Cell;
        }

        #region events
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ActionRunner.ExecuteAction(this.Page, Me.OnAppearing);
        }

        protected override void OnLongPressed()
        {
            base.OnLongPressed();
            ActionRunner.ExecuteAction(this.Page, Me.OnLongPressed);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ActionRunner.ExecuteAction(this.Page, Me.OnDisappearing);
        }

        protected override void OnTapped()
        {
            base.OnTapped();
            ActionRunner.ExecuteAction(this.Page, Me.OnTapped);
        }
        #endregion 
    }
}
