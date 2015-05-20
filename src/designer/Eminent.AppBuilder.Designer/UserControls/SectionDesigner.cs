using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeTorch.Core;

namespace CodeTorch.Designer.UserControls
{
    public partial class SectionDesigner : UserControl
    {
        public CodeTorch.Core.Screen Screen;
        public SectionDivider Zone;
        public BaseSection Section;
        

        public SectionDesigner()
        {
            InitializeComponent();
        }

        public void InitControl()
        {
            SectionLabel.DataBindings.Add("Text", Section, "Name");

            foreach (BaseControl control in Section.Controls)
            {
                AddControls(control);
            }
        }

        private void AddControls(BaseControl control)
        {
            //throw new NotImplementedException();
        }
    }
}
