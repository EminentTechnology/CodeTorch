using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Data.SqlServer
{
    public class ConnectionInfo
    {
        public string DataSource = String.Empty;
        public string InitialCatalog = String.Empty;
        public string UserID = String.Empty;
        public string Password = String.Empty;
        


        public string GetConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = DataSource;
            builder.InitialCatalog = InitialCatalog;
            builder.UserID = UserID;
            builder.Password = Password;

            return builder.ToString();

        }
    }
}
