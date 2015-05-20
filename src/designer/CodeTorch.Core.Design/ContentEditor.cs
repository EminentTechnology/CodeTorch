using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.ComponentModel;
using System.Windows.Forms.Design;
using CodeTorch.Core;


namespace CodeTorch.Core.Design
{
    public class ContentEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext tdcontext)
        {

            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object Value)
        {


            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            ContentEditorForm editForm = new ContentEditorForm();

            if (Value != null)
                editForm.Content = Value.ToString();
            else
                editForm.Content = String.Empty;

            DialogResult result = editorService.ShowDialog(editForm);

            if (result == DialogResult.OK)
            {
                Value = editForm.Content;
            }

            return Value;

        }
    }
}
