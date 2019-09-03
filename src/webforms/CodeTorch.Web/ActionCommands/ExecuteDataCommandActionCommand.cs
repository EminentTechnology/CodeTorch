using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using CodeTorch.Web.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeTorch.Web.ActionCommands
{
    public class ExecuteDataCommandActionCommand : IActionCommandStrategy
    {
        public Templates.BasePage Page { get; set; }

  

        public ActionCommand Command { get; set; }

        ExecuteDataCommand Me = null;
        


        public bool ExecuteCommand()
        {
            bool success = true;
            DataCommandService dataCommandDB = DataCommandService.GetInstance();
            PageDB pageDB = new PageDB();
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                if (Command != null)
                {
                    Me = (ExecuteDataCommand)Command;
                }

                
                if (String.IsNullOrEmpty(Me.DataCommand))
                {
                    throw new ApplicationException(String.Format("Command {0} - ExecuteDataCommand - DataCommand is invalid", Me.Name));
                }
                log.DebugFormat("DataCommand:{0}", Me.DataCommand);

                List<ScreenDataCommandParameter> parameters = null;
                parameters = pageDB.GetPopulatedCommandParameters(Me.DataCommand, Page);
                dataCommandDB.ExecuteDataCommand(Me.DataCommand, parameters);

                

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
