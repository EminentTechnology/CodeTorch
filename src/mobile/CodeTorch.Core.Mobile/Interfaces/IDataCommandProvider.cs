using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Core.Interfaces
{
    public interface IDataCommandProvider
    {
        void Initialize(List<Setting> settings);
        Task<DataTable> GetData(
            DataConnection connection, 
            DataCommand dataCommand, 
            List<ScreenDataCommandParameter> parameters, 
            string commandText,
            Action<DataTable> successAction, 
            Action<Exception> errorAction
            );

        Task<XDocument> GetXmlData(
            DataConnection connection, 
            DataCommand dataCommand, 
            List<ScreenDataCommandParameter> parameters, 
            string commandText,
            Action<DataTable> successAction, 
            Action<Exception> errorAction
            
            );

        Task<object> ExecuteCommand(
            DataConnection connection, 
            DataCommand dataCommand, 
            List<ScreenDataCommandParameter> parameters, 
            string commandText,
            Action<DataTable> successAction, 
            Action<Exception> errorAction);

        void RefreshSchema(DataConnection connection, DataCommand dataCommand);
    }
}
