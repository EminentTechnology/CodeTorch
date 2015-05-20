using System;

using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing.Design;
using System.Windows.Forms;

using System.Windows.Forms.Design;



namespace CodeTorch.Core.Design 
{
    public class ScreenDataCommandCollectionEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext tdcontext)
        {

            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object Value)
        {


            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            ScreenDataCommandCollectionForm editForm = new ScreenDataCommandCollectionForm();

            

            List<ScreenDataCommand> orig = (List<ScreenDataCommand>)Value;
            List<ScreenDataCommand> copy = ObjectCopier.Clone<List<ScreenDataCommand>>(orig);
            editForm.DataCommands = copy;
            DialogResult result = editorService.ShowDialog(editForm);

            if (result == DialogResult.OK)
            {
                Value = copy;
            }

            return Value;

        }
    }
}
