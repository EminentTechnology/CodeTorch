using CodeTorch.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Mobile.ActionCommands
{
    public static class ActionCommandStrategyFactory
    {

        public const string NavigateToScreenCommandType = "NavigateToScreenCommand";
        public const string InsertUpdateSaveCommandType = "InsertUpdateSaveCommand";
        public const string ValidateUserCommandType = "ValidateUserCommand";
        public const string LogoutCommandType = "LogoutCommand";
        
    

        private static Dictionary<string, Func<IActionCommandStrategy>> strategyList =
            new Dictionary<string, Func<IActionCommandStrategy>>()
            {
                {NavigateToScreenCommandType, () => { return new NavigateToScreenActionCommand(); }},
                {InsertUpdateSaveCommandType, () => { return new InsertUpdateSaveActionCommand(); }},
                {ValidateUserCommandType, () => { return new ValidateUserActionCommand(); }},
                {LogoutCommandType, () => { return new LogoutActionCommand(); }}
        
            };

        public static IActionCommandStrategy CreateFromType(string type)
        {
            return strategyList[type]();
        }
    }
}
