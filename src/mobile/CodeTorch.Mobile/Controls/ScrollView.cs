using System;
using System.Linq;
using CodeTorch.Core;
using Xamarin.Forms;
using CodeTorch.Mobile.Templates;

namespace CodeTorch.Mobile
{

    public class ScrollView : Xamarin.Forms.ScrollView, IView
    {

        ScrollViewControl _Me = null;
        public ScrollViewControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (ScrollViewControl)this.BaseControl;
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
            ViewHelper.DefaultViewProperties(this);

            this.Orientation = Me.Orientation;

            

            if (Me.Controls.Count > 0)
            {
                ProcessControl(Me.Controls[0]);
            }
        }

        private void ProcessControl(BaseControl control)
        {

            IView view = GetView(this.Page, Screen, control);
            
            ViewHelper.SetupView(Page as IMobilePage, Screen, control, view);
            view.Init();

            this.Content = view.GetView();


        }

        private IView GetView(Page page, MobileScreen screen, BaseControl control)
        {
            return ViewHelper.GetView(page, screen, control);
        }

        public Xamarin.Forms.View GetView()
        {
            return this as Xamarin.Forms.View;
        }

    }
}
