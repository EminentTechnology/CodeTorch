using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeTorch.Web.UserControls
{
    public class SectionPanelTemplate : UserControl
    {
        public string IntroText { get; set; }
        public string HeadingTitle { get; set; }
        public string SectionCssClass { get; set; }

        protected Literal IntroTextLiteral;
        protected Literal HeadingTitleLiteral;
        protected PlaceHolder Body;

        internal void AddBody(PlaceHolder body)
        {
            if (this.Body != null)
            {
                this.Body.Controls.Add(body);
            }
        }

        internal void Update()
        {
            if (this.HeadingTitleLiteral != null)
            {
                this.HeadingTitleLiteral.Text = this.HeadingTitle;
            }

            if (this.IntroTextLiteral != null)
            {
                this.IntroTextLiteral.Text = this.IntroText;
            }
        }
    }
}
