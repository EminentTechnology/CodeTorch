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

    public class WidgetViewModel
    {
        public WidgetViewModel(IPageModel pageModel, Section section, Widget widget)
        {
            PageModel = pageModel;
            Section = section;
            Widget = widget;
        }

        public IPageModel PageModel { get; private set; }
        public Section Section { get; private set; }
        public Widget Widget { get; private set; }
    }
}
