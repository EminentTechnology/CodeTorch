using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTorch.Core
{
    public class SectionViewModel
    {
        public SectionViewModel(IPageModel pageModel, Section section)
        {
            PageModel = pageModel;
            Section = section;
        }

        public IPageModel PageModel { get; private set; }
        public Section Section { get; private set; }
    }
}
