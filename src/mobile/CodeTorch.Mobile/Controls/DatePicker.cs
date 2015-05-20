using System;
using System.Linq;
using CodeTorch.Core;
using Xamarin.Forms;

namespace CodeTorch.Mobile
{

    public class DatePicker : Xamarin.Forms.DatePicker, IView
    {

        DatePickerControl _Me = null;
        public DatePickerControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (DatePickerControl)this.BaseControl;
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
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public Xamarin.Forms.View GetView()
        {
            return this as Xamarin.Forms.View;
        }

    }
}
