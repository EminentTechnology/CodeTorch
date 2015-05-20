using CodeTorch.Core;
using CodeTorch.Web.Templates;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Web
{
    public interface IActionCommandStrategy
    {
        BasePage Page { get; set; }
       // DbTransaction Transaction { get; set; }
        ActionCommand Command { get; set; }

        void ExecuteCommand();
    }
}
