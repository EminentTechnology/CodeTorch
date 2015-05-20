using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.RichTextBox.FormatProviders.Txt;



namespace CodeTorch.Designer.Editors
{
    public partial class MarkupEditorForm : Form
    {
        public MarkupEditorForm()
        {
            InitializeComponent();
        }

        public string Content
        {
            get
            {
                
                TxtFormatProvider provider = new TxtFormatProvider();
                
                return provider.Export(markup.Document);
            }
            set
            {
                TxtFormatProvider provider = new TxtFormatProvider();
                markup.Document = provider.Import(value);
                //is.markup.Text = value;
            }
        }

        

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void markup_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
