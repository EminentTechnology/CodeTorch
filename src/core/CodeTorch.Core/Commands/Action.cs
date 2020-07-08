
using CodeTorch.Core.Commands;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using System.Xml.Serialization;

namespace  CodeTorch.Core
{
    [Serializable]
    [System.ComponentModel.TypeConverter(typeof(ExpandableObjectConverter))]
    public class Action
    {
        private List<ActionCommand> _Commands = new List<ActionCommand>();

        

        [XmlArray("Commands")]
        //[XmlArrayItem("Command")]
        [XmlArrayItem(ElementName = "Command", Type = typeof(ActionCommand))]
        [XmlArrayItem(ElementName = "DefaultOrPopulateScreenCommand", Type = typeof(DefaultOrPopulateScreenCommand))]
        [XmlArrayItem(ElementName = "DownloadDocumentCommand", Type = typeof(DownloadDocumentCommand))]
        [XmlArrayItem(ElementName = "ExecuteDataCommand", Type = typeof(ExecuteDataCommand))]
        [XmlArrayItem(ElementName = "ExportRDLCCommand", Type = typeof(ExportRDLCCommand))]
        [XmlArrayItem(ElementName = "InvokeSearchMessageCommand", Type = typeof(InvokeSearchMessageCommand))]
        [XmlArrayItem(ElementName = "InsertUpdateSaveCommand", Type = typeof(InsertUpdateSaveCommand))]
        [XmlArrayItem(ElementName = "LogoutCommand", Type = typeof(LogoutCommand))]
        
        [XmlArrayItem(ElementName = "NavigateToUrlCommand", Type = typeof(NavigateToUrlCommand))]
        [XmlArrayItem(ElementName = "RedirectCommand", Type = typeof(RedirectCommand))]
        [XmlArrayItem(ElementName = "RenderPageSectionsCommand", Type = typeof(RenderPageSectionsCommand))]
        [XmlArrayItem(ElementName = "ResizePhotoCommand", Type = typeof(ResizePhotoCommand))]
        [XmlArrayItem(ElementName = "SetControlPropertyCommand", Type = typeof(SetControlPropertyCommand))]
        [XmlArrayItem(ElementName = "SetScreenObjectsDataCommand", Type = typeof(SetScreenObjectsDataCommand))]
        [XmlArrayItem(ElementName = "ValidateUserCommand", Type = typeof(ValidateUserCommand))]
#if NETFRAMEWORK
        [Editor("CodeTorch.Core.Design.ActionCommandCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]
#endif
        public List<ActionCommand> Commands
        {
            get
            {
                return _Commands;
            }
            set
            {
                _Commands = value;
            }

        }

        public override string ToString()
        {
            return String.Format("{0} Commands", _Commands.Count);
        }
    }
}
