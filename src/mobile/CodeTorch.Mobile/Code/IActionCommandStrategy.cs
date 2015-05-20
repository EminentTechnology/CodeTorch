using System;
using System.Linq;
using CodeTorch.Core;
using Xamarin.Forms;
using CodeTorch.Mobile.Templates;

namespace CodeTorch.Mobile
{
    public interface IActionCommandStrategy
    {
        IMobilePage IPage { get; set; }
        Page Page { get; set; }
       // DbTransaction Transaction { get; set; }
        ActionCommand Command { get; set; }

        void ExecuteCommand();
    }
}
