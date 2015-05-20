using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace CodeTorch.Designer.Editors
{
    public class MarkupEditor : PropertyGridItemElement
    {

        RadButtonElement button;

        string content = String.Empty;

        protected override void CreateChildElements()
        {
            base.CreateChildElements();

            this.button = new RadButtonElement();
            this.button.Text = "...";
            this.button.Click += new EventHandler(button_Click);
            this.button.StretchHorizontally = false;
            this.button.Margin = new Padding(0, 2, 0, 2);
            this.button.Alignment = System.Drawing.ContentAlignment.MiddleRight;

            this.ValueElement.Children.Add(this.button);
        }

        protected override void DisposeManagedResources()
        {
            this.button.Click -= button_Click;
            base.DisposeManagedResources();

        }

        private void button_Click(object sender, EventArgs e)
        {
            MarkupEditorForm editForm = new MarkupEditorForm();

            if (((PropertyGridItem)this.Data).Value != null)
            {
                editForm.Content = ((PropertyGridItem)this.Data).Value.ToString();
            }

            DialogResult result = editForm.ShowDialog();


            if (result == DialogResult.OK)
            {
                ((PropertyGridItem)this.Data).Value = editForm.Content;
            }


        }

        public override void AddEditor(IInputEditor editor)
        { }

        public override void RemoveEditor(IInputEditor editor)
        { }

        public override bool IsCompatible(PropertyGridItemBase data, object context)
        {
            PropertyGridItem item = data as PropertyGridItem;
            return item != null && item.PropertyType == typeof(string);
        }

        protected override Type ThemeEffectiveType
        {
            get
            {
                return typeof(PropertyGridItemElement);
            }
        }





    }
}
