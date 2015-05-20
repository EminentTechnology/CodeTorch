using System;
using System.Linq;
using CodeTorch.Core;
using Xamarin.Forms;


namespace CodeTorch.Mobile
{
    
    public interface IView 
    {

        BaseControl BaseControl { get; set; }
        string Name { get; set; }
        Page Page { get; set; }
        MobileScreen Screen { get; set; }
        string Value { get; set; }

        void Init();
        View GetView();
    }
}
