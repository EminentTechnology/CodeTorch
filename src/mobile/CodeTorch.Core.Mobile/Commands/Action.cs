


using CodeTorch.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace  CodeTorch.Core
{

    public class Action
    {
        private List<ActionCommand> _Commands = new List<ActionCommand>();

        

        [XmlArray("Commands")]
        //[XmlArrayItem(ElementName = "Command", Type = typeof(ActionCommand))]
        //[XmlArrayItem(ElementName = "DefaultOrPopulateScreenCommand", Type = typeof(DefaultOrPopulateScreenCommand))]
        //[XmlArrayItem(ElementName = "DownloadDocumentCommand", Type = typeof(DownloadDocumentCommand))]
        //[XmlArrayItem(ElementName = "ExecuteDataCommand", Type = typeof(ExecuteDataCommand))]
        //[XmlArrayItem(ElementName = "ExportRDLCCommand", Type = typeof(ExportRDLCCommand))]
        //[XmlArrayItem(ElementName = "InvokeSearchMessageCommand", Type = typeof(InvokeSearchMessageCommand))]
        [XmlArrayItem(ElementName = "InsertUpdateSaveCommand", Type = typeof(InsertUpdateSaveCommand))]
        [XmlArrayItem(ElementName = "LogoutCommand", Type = typeof(LogoutCommand))]
        [XmlArrayItem(ElementName = "NavigateToScreenCommand", Type = typeof(NavigateToScreenCommand))]
        //[XmlArrayItem(ElementName = "RedirectCommand", Type = typeof(RedirectCommand))]
        //[XmlArrayItem(ElementName = "RenderPageSectionsCommand", Type = typeof(RenderPageSectionsCommand))]
        //[XmlArrayItem(ElementName = "ResizePhotoCommand", Type = typeof(ResizePhotoCommand))]
        //[XmlArrayItem(ElementName = "SetControlPropertyCommand", Type = typeof(SetControlPropertyCommand))]
        //[XmlArrayItem(ElementName = "SetScreenObjectsDataCommand", Type = typeof(SetScreenObjectsDataCommand))]
        [XmlArrayItem(ElementName = "ValidateUserCommand", Type = typeof(ValidateUserCommand))]
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

        public Action Clone()
        {
           return (Action)this.MemberwiseClone();
        }

       
    }
}
