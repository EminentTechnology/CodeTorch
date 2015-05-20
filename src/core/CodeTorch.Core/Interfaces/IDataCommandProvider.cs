
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CodeTorch.Core.Interfaces
{
    public interface IDataCommandProvider
    {
        void Initialize(StringCollection settings);
        DataTable GetData(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText);

        XmlDocument GetXmlData(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText);

        object ExecuteCommand(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText);

        void RefreshSchema(DataConnection connection, DataCommand dataCommand);
        
    }
}
