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
    public class SwitchCell: Xamarin.Forms.SwitchCell, ICell
    {
        public BaseControl BaseControl { get; set; }
        public Page Page { get; set; }


        public BaseCell Cell { get; set; }
        public BaseSection Section { get; set; }
        
        SwitchCellControl _Me = null;
        public SwitchCellControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (SwitchCellControl)this.Cell;
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
                    this.On =  OnPlatformBool.GetAndroidValue(Me.On);
                    
                     if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.Text)))
                        this.Text =  OnPlatformString.GetAndroidValue(Me.Text);

                },
                iOS: () =>
                {
                   this.On =  OnPlatformBool.GetiOSValue(Me.On);
                    
                     if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.Text)))
                        this.Text =  OnPlatformString.GetiOSValue(Me.Text);
                },
                WinPhone: () =>
                {
            
                     this.On =  OnPlatformBool.GetWinPhoneValue(Me.On);
                    
                     if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.Text)))
                        this.Text =  OnPlatformString.GetWinPhoneValue(Me.Text);
                }
          );

            this.OnChanged += SwitchCell_OnChanged;
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

        void SwitchCell_OnChanged(object sender, ToggledEventArgs e)
        {
            ActionRunner.ExecuteAction(this.Page, Me.OnChanged);
        }
        #endregion 
    }
}
