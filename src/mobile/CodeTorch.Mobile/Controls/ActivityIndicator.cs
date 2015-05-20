using System;
using System.Linq;
using CodeTorch.Core;
using Xamarin.Forms;

namespace CodeTorch.Mobile
{

    public class ActivityIndicator : Xamarin.Forms.ActivityIndicator, IView
    {

        ActivityIndicatorControl _Me = null;
        public ActivityIndicatorControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (ActivityIndicatorControl)this.BaseControl;
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
