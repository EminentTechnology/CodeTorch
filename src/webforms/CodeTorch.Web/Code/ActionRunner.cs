using CodeTorch.Core;
using CodeTorch.Web.ActionCommands;
using CodeTorch.Web.Templates;
using System;
using System.Linq;
using System.Transactions;

namespace CodeTorch.Web
{
    public class ActionRunner
    {
        public CodeTorch.Core.Action Action { get; set; }
        public BasePage Page { get; set; }

        internal void Execute()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {

                foreach (ActionCommand command in Action.Commands)
                {
                    IActionCommandStrategy s = ActionCommandStrategyFactory.CreateFromType(command.Type);
                    s.Page = this.Page;
                    s.Command = command;
                    // s.Transaction = tran;

                    s.ExecuteCommand();

                    


                }

                scope.Complete();
            }

           
        }

   
        
    }
}
