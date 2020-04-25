using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTorch.Blazor.Sections
{
    public class ButtonsBase: ComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment<RenderFragment> Template { get; set; }

      
    }
}
