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

        public void Execute()
        {
            bool SupportsTransactions = true;

            if (Page.Screen != null)
                SupportsTransactions = (Page.Screen.SupportsTransactions);


            if (SupportsTransactions)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    ExecuteActions();

                    scope.Complete();
                }
            }
            else
            {
                ExecuteActions();
            }


        }

        private void ExecuteActions()
        {
            foreach (ActionCommand command in Action.Commands)
            {
                IActionCommandStrategy s = ActionCommandStrategyFactory.CreateFromType(command.Type);
                s.Page = this.Page;
                s.Command = command;
                // s.Transaction = tran;

                bool success = s.ExecuteCommand();

                if (!success && !command.ContinueOnError)
                {
                    //don't continue if we have an error - depending on setting set in config

                    

                    break;
                }




            }
        }


    }
}
