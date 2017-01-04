using System;

using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CodeTorch.Core
{
    public class Common
    {
        public static void LogException(Exception ex)
        {
            LogException(ex, true);
        }


        public static void LogException(Exception ex, bool rethrow)
        {


            Abstractions.ILog log = Resolver.Resolve<Abstractions.ILogManager>().GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(ex);

            if (rethrow)
            {
                throw ex;
            }



        }

        public static void LogInfo(string Message)
        {
            Abstractions.ILog log = Resolver.Resolve<Abstractions.ILogManager>().GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Info(Message);

        }

        public static void LogDebug(string Message)
        {

            Abstractions.ILog log = Resolver.Resolve<Abstractions.ILogManager>().GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Debug(Message);

        }

        public static void LogWarn(string Message)
        {

            Abstractions.ILog log = Resolver.Resolve<Abstractions.ILogManager>().GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Warn(Message);

        }

 

        public static string ConvertWordToProperCase(string toConvert)
        {
            string reg = String.Format
                (
                    "{0}|{1}|{2}",
                    @"(?<=[A-Z])(?=[A-Z][a-z])",
                    @"(?<=[^A-Z])(?=[A-Z])",
                    @"(?<=[A-Za-z])(?=[^A-Za-z])"
                );

            Regex labelRegex = new Regex(reg);
            string newString = labelRegex.Replace(toConvert, " ");
            return newString;
        }
    }
}
