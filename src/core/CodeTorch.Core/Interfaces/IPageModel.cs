using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTorch.Core
{
    public interface IPageModel
    {
        Screen Screen { get; set; }

        //TODO: get current page title to use in layouts
    }
}
