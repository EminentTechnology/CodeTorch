using CodeTorch.Core;
using CodeTorch.Mobile.ActionCommands;
using CodeTorch.Mobile.Templates;
using System;
using System.Linq;
using Xamarin.Forms;


namespace CodeTorch.Mobile
{
    public class ActionRunner
    {
        public CodeTorch.Core.Action Action { get; set; }
        public Page Page { get; set; }

        internal void Execute()
        {
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            //{

                foreach (ActionCommand command in Action.Commands)
                {
                    IActionCommandStrategy s = ActionCommandStrategyFactory.CreateFromType(command.Type);
                    s.Page = this.Page;
                    s.IPage = this.Page as IMobilePage;
                    s.Command = command;
                   // s.Page = this.Page;
                   // s.Command = command;
                    // s.Transaction = tran;

                    s.ExecuteCommand();

                    


                }

            //    scope.Complete();
            //}

           
        }

        public static void ExecuteAction(Page page, Core.Action action)
        {
            try
            {
                if (action != null)
                {
                    ActionRunner runner = new ActionRunner();
                    runner.Page = page;
                    runner.Action = action.Clone();
                    runner.Execute();
                }
            }
            catch (Exception ex)
            {
                //log.Error(ex);
                //((BasePage)this.Page).DisplayErrorAlert(ex);

            }
        }
   
        
    }
}
