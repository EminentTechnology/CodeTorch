using CodeTorch.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CodeTorch.Mobile
{
    public class AbsoluteLayout : Xamarin.Forms.AbsoluteLayout, IView
    {
        AbsoluteLayoutControl _Me = null;
        public AbsoluteLayoutControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (AbsoluteLayoutControl)this.BaseControl;
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
