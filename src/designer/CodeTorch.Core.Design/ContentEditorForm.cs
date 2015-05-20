using System;
using System.Linq;
using System.Windows.Forms;
using Telerik.Windows.Documents.FormatProviders.Txt;



namespace CodeTorch.Core.Design
{
    public partial class ContentEditorForm : Form
    {
        public ContentEditorForm()
        {
            InitializeComponent();
        }

        public string Content
        {
            get
            {
                
                //TxtFormatProvider provider = new TxtFormatProvider();
             

                //return provider.Export(markup.Document);
                return markup.Text;
            }
            set
            {
                //TxtFormatProvider provider = new TxtFormatProvider();
                //markup.Document = provider.Import(value);
                markup.Text = value;
               
            }
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void markup_Click(object sender, EventArgs e)
        {

        }
    }
}
