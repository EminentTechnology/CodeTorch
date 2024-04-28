using CodeTorch.Core;
using CodeTorch.Core.Messages;
using System;

namespace CodeTorch.Web.ActionCommands
{
    public class InvokeSearchMessageActionCommand : IActionCommandStrategy
    {
        public Templates.BasePage Page { get; set; }
        public ActionCommand Command { get; set; }

        public bool ExecuteCommand()
        {
            bool success = true;
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                PerformSearchMessage search = new PerformSearchMessage();
                Page.MessageBus.Publish(search);
            }
            catch (Exception ex)
            {
                success = false;
                Page.DisplayErrorAlert(ex);

                log.Error(ex);
            }

            return success;
        }
    }
}
