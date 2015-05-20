using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Mobile.Data.Rest
{
    public enum Format
    {
        /// <summary>
        /// XML format
        /// </summary>
        Xml,

        /// <summary>
        /// JSON format
        /// </summary>
        Json,

        /// <summary>
        /// Protocol Buffer format
        /// </summary>
        ProtoBuf,

        /// <summary>
        /// Comma-separated format
        /// </summary>
        Csv,

        CustomBinary
    }

    public interface IRestClient
    {
        void MakeAsyncRequest(RestRequest restRequest, Action<RestResponse> successAction, Action<Exception> errorAction);

    }
}
