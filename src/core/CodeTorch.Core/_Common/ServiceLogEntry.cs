using System;

namespace CodeTorch.Core
{
    public enum ServiceLogResponseCodeType
    {
        Unknown = 0,
        Successful = 1,
        BadRequest = 2,
        NotFound = 3,
        Unauthorized = 4,
        InternalServerError = 5,
        NetworkError = 6,
        VendorError = 7
    }

    public class ServiceLogEntry
    {
        public bool Enabled { get; set; } = true;
        public bool LogRequestHeaders { get; set; } = true;
        public bool LogRequestData { get; set; } = true;
        public bool LogResponseHeaders { get; set; } = true;
        public bool LogResponseData { get; set; } = true;

        public string ServiceLogId { get; set; }
        public string ApplicationName { get; set; }
        public string TraceId { get; set; }
        public string Component { get; set; } 
        public string EntityId { get; set; }
        public string RequestUri { get; set; }
        public string RequestIpAddress { get; set; } 
        public string RequestUserName { get; set; }

        public DateTime RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string RequestHeaders { get; set; }
        public string ResponseHeaders { get; set; }
        public string RequestData { get; set; }
        public string ResponseData { get; set; }
        public ServiceLogResponseCodeType ResponseCode { get; set; }
        public int? ResponseHttpCode { get; set; }
        public string ExtraInfo { get; set; }
        public string ErrorMessage { get; set; }
        public string ServerHostName { get; set; } 
        public string ServerIpAddress { get; set; }
        public string ServerUserName { get; set; }
    }
}
