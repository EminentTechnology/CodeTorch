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
    public class EntryCell: Xamarin.Forms.EntryCell, ICell
    {
        public BaseControl BaseControl { get; set; }
        public Page Page { get; set; }
        public BaseCell Cell { get; set; }
        public BaseSection Section { get; set; }

        EntryCellControl _Me = null;
        public EntryCellControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (EntryCellControl)this.Cell;
                }
                return _Me;
            }
        }

        public void Init()
        {

            CellHelper.DefaultViewProperties(this);

            this.Keyboard = CellHelper.GetKeyboard(Me.Keyboard);
            this.XAlign = CellHelper.GetTextAlignment(Me.XAlign);

            Device.OnPlatform(
                Android: () =>
                {
                     if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.Label)))
                        this.Label =  OnPlatformString.GetAndroidValue(Me.Label);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.LabelColor)))
                        this.LabelColor = Color.FromHex( OnPlatformString.GetAndroidValue(Me.LabelColor));

                    if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.Text)))
                        this.Text =  OnPlatformString.GetAndroidValue(Me.Text);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(Me.Placeholder)))
                        this.Placeholder =  OnPlatformString.GetAndroidValue(Me.Placeholder);

                },
                iOS: () =>
                {
                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.Label)))
                        this.Label =  OnPlatformString.GetiOSValue(Me.Label);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.LabelColor)))
                        this.LabelColor = Color.FromHex( OnPlatformString.GetiOSValue(Me.LabelColor));

                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.Text)))
                        this.Text =  OnPlatformString.GetiOSValue(Me.Text);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(Me.Placeholder)))
                        this.Placeholder =  OnPlatformString.GetiOSValue(Me.Placeholder);
                },
                WinPhone: () =>
                {
                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.Label)))
                        this.Label =  OnPlatformString.GetWinPhoneValue(Me.Label);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.LabelColor)))
                        this.LabelColor = Color.FromHex( OnPlatformString.GetWinPhoneValue(Me.LabelColor));

                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.Text)))
                        this.Text =  OnPlatformString.GetWinPhoneValue(Me.Text);

                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(Me.Placeholder)))
                        this.Placeholder =  OnPlatformString.GetWinPhoneValue(Me.Placeholder);
                }
            );

            this.Completed += EntryCell_Completed;
        }

        void EntryCell_Completed(object sender, EventArgs e)
        {
            ActionRunner.ExecuteAction(this.Page, Me.OnCompleted);
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
