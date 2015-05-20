using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using CodeTorch.Core.Commands;

namespace CodeTorch.Core.Design
{
    public class ActionCommandCollectionEditor : CollectionEditor
    {
        public ActionCommandCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type[] CreateNewItemTypes()
        {
            return new Type[]
            {
                typeof(DefaultOrPopulateScreenCommand),
                typeof(DownloadDocumentCommand),
                typeof(ExecuteDataCommand),
                typeof(ExportRDLCCommand),
                typeof(InvokeSearchMessageCommand),
                typeof(InsertUpdateSaveCommand),
                typeof(LogoutCommand),
                typeof(NavigateToUrlCommand),
                typeof(RedirectCommand),
                typeof(RenderPageSectionsCommand),
                typeof(ResizePhotoCommand),
                typeof(SetControlPropertyCommand),
                typeof(SetScreenObjectsDataCommand),
                typeof(ValidateUserCommand)

            
            };
        }

        protected override string GetDisplayText(object value)
        {
            ActionCommand command = (ActionCommand)value;

            string retVal = command.Name;

            return base.GetDisplayText(retVal);
        }

        //protected override object CreateInstance(Type itemType)
        //{
        //    Screen screen = (Screen)Context.Instance;

        //    object retVal = base.CreateInstance(itemType);
        //    ((ActionCommand)retVal).Parent = screen;

        //    return retVal;
        //}
    }
}
