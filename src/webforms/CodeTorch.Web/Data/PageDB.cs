using System;
using System.Collections.Generic;
using System.Linq;
using CodeTorch.Web.Templates;
using CodeTorch.Core;
using System.Web.UI;

namespace CodeTorch.Web.Data
{
    public class PageDB
    {
     
 
      

        public List<ScreenDataCommandParameter> GetPopulatedCommandParameters(string DataCommandName, BasePage page)
        {
            return GetPopulatedCommandParameters(DataCommandName, page, null);
        }

        public List<ScreenDataCommandParameter> GetPopulatedCommandParameters(string DataCommandName, BasePage page, Control Container)
        {
            return GetPopulatedCommandParameters(DataCommandName, page, null, page.Screen.DataCommands);
            
        }

        public List<ScreenDataCommandParameter> GetPopulatedCommandParameters(string DataCommandName, BasePage page, Control Container, List<ScreenDataCommand> datacommands)
        {
            string ErrorFormat = "Invalid {0} propery for Data Command {1} Parameter {2} - {3}";
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();

            ScreenDataCommand screenCommand = ScreenDataCommand.GetDataCommand(datacommands, DataCommandName);
            if ((screenCommand != null) && (screenCommand.Parameters != null))
            {
                parameters = ObjectCopier.Clone<List<ScreenDataCommandParameter>>(screenCommand.Parameters);

                foreach (ScreenDataCommandParameter p in parameters)
                {
                    try
                    {
                        p.Value = Common.GetParameterInputValue(page, p, Container);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(
                            String.Format(ErrorFormat, "Value", DataCommandName, p.Name, ex.Message),
                            ex);
                    }

                }
            }




            return parameters;

        }


      
        public List<ReportParameter> GetPopulatedReportParameters(BasePage page, List<ReportParameter> reportRarameters)
        {
            return GetPopulatedReportParameters(page, reportRarameters, null);
        }

        public List<ReportParameter> GetPopulatedReportParameters(BasePage page, List<ReportParameter> reportRarameters, Control Container)
        {
            string lastParameterName = null;
            string ErrorFormat = "Invalid {0} propery for Report Parameter {1} - {2}";
            List<ReportParameter> retVal = null;


            try
            {
                if(reportRarameters != null)
                    retVal = ObjectCopier.Clone<List<ReportParameter>>(reportRarameters);

                foreach (ReportParameter p in retVal)
                {
                    lastParameterName = p.Name;
                    p.Value = Common.GetParameterInputValue(page, p, Container);
                }
                
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    String.Format(ErrorFormat, "Value", lastParameterName, ex.Message),
                    ex);
            }

            return retVal;
            
        }
    }
}
