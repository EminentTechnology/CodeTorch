using CodeTorch.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodeTorch.Mobile
{
    public interface ICell
    {
        BaseControl BaseControl { get; set; }
        Page Page { get; set; }
        BaseCell Cell { get; set; }
        BaseSection Section { get; set; }
         

        void Init();
        Cell GetCell();
    }
}
