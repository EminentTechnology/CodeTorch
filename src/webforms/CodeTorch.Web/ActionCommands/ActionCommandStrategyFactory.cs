using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Web.ActionCommands
{
    public static class ActionCommandStrategyFactory
    {

        public const string InsertUpdateSaveCommandType = "InsertUpdateSaveCommand";
        public const string DefaultOrPopulateScreenCommandType = "DefaultOrPopulateScreenCommand";
        public const string NavigateToUrlCommandType = "NavigateToUrlCommand";
        public const string RenderPageSectionsCommandType = "RenderPageSectionsCommand";
        public const string SetControlPropertyCommandType = "SetControlPropertyCommand";
        public const string RedirectCommandType = "RedirectCommand";
        public const string LogoutCommandType = "LogoutCommand";
        public const string ExportRDLCCommandType = "ExportRDLCCommand";
        public const string DownloadDocumentCommandType = "DownloadDocumentCommand";
        public const string ValidateUserCommandType = "ValidateUserCommand";
        public const string ExecuteDataCommandType = "ExecuteDataCommand";
        public const string SetScreenObjectsDataCommandType = "SetScreenObjectsDataCommand";
        public const string InvokeSearchMessageCommandType = "InvokeSearchMessageCommand";
        public const string ResizePhotoCommandType = "ResizePhotoCommand";
        
    

        private static Dictionary<string, Func<IActionCommandStrategy>> strategyList =
            new Dictionary<string, Func<IActionCommandStrategy>>()
            {
                {InsertUpdateSaveCommandType, () => { return new InsertUpdateSaveActionCommand(); }},
                {DefaultOrPopulateScreenCommandType, () => { return new DefaultOrPopulateScreenActionCommand(); }},
                {NavigateToUrlCommandType, () => { return new NavigateToUrlActionCommand(); }},
                {RenderPageSectionsCommandType, () => { return new RenderPageSectionsActionCommand(); }},
                {SetControlPropertyCommandType, () => { return new SetControlPropertyActionCommand(); }},
                {RedirectCommandType, () => { return new RedirectActionCommand(); }},
                {LogoutCommandType, () => { return new LogoutActionCommand(); }},
                {ExportRDLCCommandType, () => { return new  ExportRDLCActionCommand(); }},
                {DownloadDocumentCommandType, () => { return new  DownloadDocumentActionCommand(); }},
                {ValidateUserCommandType, () => { return new  ValidateUserActionCommand(); }},
                {ExecuteDataCommandType, () => { return new  ExecuteDataCommandActionCommand(); }},
                {SetScreenObjectsDataCommandType, () => { return new  SetScreenObjectsDataCommandActionCommand(); }},
                {InvokeSearchMessageCommandType, () => { return new  InvokeSearchMessageActionCommand(); }},
                {ResizePhotoCommandType, () => { return new  ResizePhotoActionCommand(); }}
        
            };

        public static IActionCommandStrategy CreateFromType(string type)
        {
            return strategyList[type]();
        }
    }
}
