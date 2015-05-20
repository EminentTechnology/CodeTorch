using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Mobile.Data.Rest
{
    public class RestResponse
    {
        public string Content { get; set; }
        public string ContentEncoding { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public Exception ErrorException { get; set; }
        public string ErrorMessage { get; set; }
        //public IList<AdditionalParameter> Headers { get; protected internal set; }
        public byte[] RawBytes { get; set; }
        public RestRequest Request { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public Uri ResponseUri { get; set; }
        public string Server { get; set; }
        public ResponseHttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }

        public RestResponse()
        {
        }
    }

    public class RestResponse<T> : RestResponse
    {
        public T Data { get; set; }
    }
}
