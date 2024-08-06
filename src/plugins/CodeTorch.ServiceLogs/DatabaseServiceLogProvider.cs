using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace CodeTorch.ServiceLogs
{
    public class DatabaseServiceLogProvider : IServiceLogProvider
    {
        private string _connectionString;

        log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Initialize(string config)
        {
            string connectionStringKey = ConfigurationManager.AppSettings["ServiceLogConnectionStringKey"];
            if (!string.IsNullOrEmpty(connectionStringKey))
            {
                _connectionString = ConfigurationManager.ConnectionStrings[connectionStringKey]?.ConnectionString;
            }

            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"]?.ConnectionString;
            }

            log.Info("DatabaseServiceLogProvider - Initialized with connection string.");
        }

        public void Log(ServiceLogEntry entry)
        {
            try
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    entry.Enabled = false;
                    log.Error("DatabaseServiceLogProvider - Connection string is not set.");
                }

                if (entry.Enabled)
                {
                    entry.ServiceLogId = Guid.NewGuid().ToString();
                    InsertServiceLogEntry(entry);
                    log.Info("DatabaseServiceLogProvider - Log entry inserted successfully.");
                }
            }
            catch (Exception ex)
            {
                log.Error("DatabaseServiceLogProvider - Error while trying to log service logs to the database", ex);
            }
        }

        private void InsertServiceLogEntry(ServiceLogEntry entry)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("INSERT INTO ServiceLog (ApplicationName, TraceId, Component, EntityId, RequestUri, RequestIpAddress, RequestUserName, RequestDate, ResponseDate, RequestHeaders, ResponseHeaders, RequestData, ResponseData, ResponseCode, ResponseHttpCode, ExtraInfo, ErrorMessage, ServerHostName, ServerIpAddress, ServerUserName) VALUES (@ApplicationName, @TraceId, @Component, @EntityId, @RequestUri, @RequestIpAddress, @RequestUserName, @RequestDate, @ResponseDate, @RequestHeaders, @ResponseHeaders, @RequestData, @ResponseData, @ResponseCode, @ResponseHttpCode, @ExtraInfo, @ErrorMessage, @ServerHostName, @ServerIpAddress, @ServerUserName)", connection))
                {
                    command.Parameters.AddWithValue("@ApplicationName", entry.ApplicationName);
                    command.Parameters.AddWithValue("@TraceId", (object)entry.TraceId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Component", entry.Component);
                    command.Parameters.AddWithValue("@EntityId", (object)entry.EntityId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RequestUri", entry.RequestUri);
                    command.Parameters.AddWithValue("@RequestIpAddress", entry.RequestIpAddress);
                    command.Parameters.AddWithValue("@RequestUserName", entry.RequestUserName);
                    command.Parameters.AddWithValue("@RequestDate", entry.RequestDate);
                    command.Parameters.AddWithValue("@ResponseDate", (object)entry.ResponseDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RequestHeaders", (object)entry.RequestHeaders ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ResponseHeaders", (object)entry.ResponseHeaders ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RequestData", (object)entry.RequestData ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ResponseData", (object)entry.ResponseData ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ResponseCode", (object)entry.ResponseCode ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ResponseHttpCode", (object)entry.ResponseHttpCode ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ExtraInfo", (object)entry.ExtraInfo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ErrorMessage", (object)entry.ErrorMessage ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ServerHostName", entry.ServerHostName);
                    command.Parameters.AddWithValue("@ServerIpAddress", entry.ServerIpAddress);
                    command.Parameters.AddWithValue("@ServerUserName", entry.ServerUserName);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
