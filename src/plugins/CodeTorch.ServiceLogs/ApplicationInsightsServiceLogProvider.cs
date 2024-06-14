using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace CodeTorch.ServiceLogs
{
    public class ApplicationInsightsServiceLogProvider : IServiceLogProvider
    {
        private readonly TelemetryClient _telemetryClient;
        const string intentionallyNotLogged = "Intentionally Excluded From Logs";

        public ApplicationInsightsServiceLogProvider()
        {
            _telemetryClient = new TelemetryClient();
        }

        public void Initialize(string config)
        {
            //ignores settings and looks to config file for application insights key
            _telemetryClient.Context.InstrumentationKey = ConfigurationManager.AppSettings["ai:InstrumentationKey"];
            
        }

        public void Log(ServiceLogEntry entry)
        {
            try
            {
                if (string.IsNullOrEmpty(_telemetryClient.Context.InstrumentationKey))
                {
                    entry.Enabled = false;
                }

                if (entry.Enabled)
                {
                    entry.ServiceLogId = Guid.NewGuid().ToString();

                    var properties = new Dictionary<string, string>
                    {
                        { "ServiceLogId", entry.ServiceLogId },
                        { "ApplicationName", entry.ApplicationName },
                        { "TraceId", entry.TraceId },
                        { "Component", entry.Component },
                        { "EntityId", entry.EntityId },
                        { "RequestUri", entry.RequestUri },
                        { "RequestIpAddress", entry.RequestIpAddress },
                        { "RequestUserName", entry.RequestUserName },
                        { "RequestDate", entry.RequestDate.ToString("o") },
                        { "ResponseDate", entry.ResponseDate?.ToString("o") },
                        { "ResponseCode", entry.ResponseCode.ToString() },
                        { "ResponseHttpCode", entry.ResponseHttpCode?.ToString() },
                        { "ExtraInfo", entry.ExtraInfo },
                        { "ErrorMessage", entry.ErrorMessage },
                        { "ServerHostName", entry.ServerHostName },
                        { "ServerIpAddress", entry.ServerIpAddress },
                        { "ServerUserName", entry.ServerUserName }
                    };

                    if (entry.LogRequestHeaders)
                    {
                        properties.Add("RequestHeaders", entry.RequestHeaders);
                    }
                    else
                    {
                        properties.Add("RequestHeaders", intentionallyNotLogged);
                    }

                    if (entry.LogResponseHeaders)
                    {
                        properties.Add("ResponseHeaders", entry.ResponseHeaders);
                    }
                    else
                    {
                        properties.Add("ResponseHeaders", intentionallyNotLogged);
                    }

                    if (entry.LogRequestData)
                    {
                        properties.Add("RequestData", entry.RequestData);
                    }
                    else
                    {
                        properties.Add("RequestData", intentionallyNotLogged);
                    }

                    if (entry.LogResponseData)
                    {
                        properties.Add("ResponseData", entry.ResponseData);
                    }
                    else
                    {
                        properties.Add("ResponseData", intentionallyNotLogged);
                    }

                    var telemetry = new EventTelemetry("ServiceLog")
                    {
                        Timestamp = entry.RequestDate,
                    };

                    foreach (var property in properties)
                    {
                        telemetry.Properties.Add(property.Key, property.Value);
                    }

                    _telemetryClient.TrackEvent(telemetry);

                    TimeSpan duration = entry.ResponseDate.HasValue ? entry.ResponseDate.Value - entry.RequestDate : TimeSpan.Zero;
                    var success = entry.ResponseCode == ServiceLogResponseCodeType.Successful;

                    _telemetryClient.TrackRequest(entry.RequestUri, entry.RequestDate, duration, entry.ResponseHttpCode?.ToString(), success);
                }
            }
            catch (Exception ex)
            {
                //TODO: log error
            }
        }
    }
}
