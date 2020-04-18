﻿using CodeTorch.Abstractions;
using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Core.Messages;
using System;
using System.Linq;

namespace CodeTorch.Web.ActionCommands
{
    public class InvokeSearchMessageActionCommand : IActionCommandStrategy
    {
        public Templates.BasePage Page { get; set; }

  

        public ActionCommand Command { get; set; }

        InvokeSearchMessageCommand Me = null;
        


        public bool ExecuteCommand()
        {
            bool success = true;
            ILog log = Resolver.Resolve<ILogManager>().GetLogger(this.GetType());

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
