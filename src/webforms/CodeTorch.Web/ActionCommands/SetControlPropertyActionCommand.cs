using CodeTorch.Core;
using CodeTorch.Core.Commands;
using System;
using System.Linq;
using System.Reflection;

namespace CodeTorch.Web.ActionCommands
{
    public class SetControlPropertyActionCommand : IActionCommandStrategy
    {
        public Templates.BasePage Page { get; set; }



        public ActionCommand Command { get; set; }

        SetControlPropertyCommand Me = null;
        


        public void ExecuteCommand()
        {


            Abstractions.ILog log = Resolver.Resolve<Abstractions.ILogManager>().GetLogger(this.GetType());

            try
            {
                if (Command != null)
                {
                    Me = (SetControlPropertyCommand)Command;
                }

                //first find control
                object control = Page.FindControlRecursive(Me.ControlName);

                //then attempt to set propery
                if (control != null)
                {
                    PropertyInfo propertyInfo = control.GetType().GetProperty(Me.PropertyName);

                    if (propertyInfo != null)
                    {
                        object controlValue = Convert.ChangeType(Me.PropertyValue, propertyInfo.PropertyType);

                        propertyInfo.SetValue(control, controlValue);
                    }
                
                }

                

            }
            catch (Exception ex)
            {
                log.Error(ex.Message,ex);
                throw ex;
            }
            
        }



        
    }
}
