using CodeTorch.Mobile.Data.Rest.Android;
using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using CodeTorch.Mobile.Data.Rest;
using Newtonsoft.Json;

[assembly: Xamarin.Forms.Dependency (typeof (AndroidRestClient))]
namespace CodeTorch.Mobile.Data.Rest.Android
{
    public class AndroidRestClient: IRestClient
    {
        public async void MakeAsyncRequest(RestRequest restRequest, Action<RestResponse> successAction, Action<Exception> errorAction)
        {
            try
            {
                //Build RestSharp request
                var restSharpRequest = BuildRestSharpRequest(restRequest);

                //Create the RestSharp client
                var restSharpClient = GetRestClient(restRequest.Url);

              

                restSharpClient.ExecuteAsync(restSharpRequest, (r, h) =>
                {
  

                    bool success = false;
                    Exception e = null;
                    var restResponse = BuildRestResponse(r);
                    var responseStatus = restResponse.ResponseStatus;
                    var responseStatusCode = restResponse.StatusCode;
                    var responseException = restResponse.ErrorException;
                    var responseErrorMessage = restResponse.ErrorMessage;
                    var responseContent = restResponse.Content;

                    HandleAsyncError(ref success, ref e, responseStatus, responseStatusCode, responseException, responseErrorMessage, responseContent);

                    if (success)
                    {
                        successAction(restResponse);
                    }
                    else
                    {
                        errorAction(e);
                    }
                    
                });

             
            }
            catch (Exception ex)
            {
                errorAction(ex);
            }
        }

        private static void HandleAsyncError(ref bool success, ref Exception e, ResponseStatus responseStatus, ResponseHttpStatusCode responseStatusCode, Exception responseException, string responseErrorMessage, string responseContent)
        {
            Exception inner = null;

            if (
                (responseStatus == ResponseStatus.Aborted) ||
                (responseStatus == ResponseStatus.Error) ||
                (responseStatus == ResponseStatus.TimedOut)
                )
            {
                //something bad happened with connection
                success = false;

                

                if (
                    (responseException != null) ||
                    (!String.IsNullOrEmpty(responseErrorMessage))
                    )
                {
                    if (responseException != null)
                    {
                        inner = responseException;
                    }
                    else
                    {
                        inner= new Exception(responseErrorMessage);
                    }
                }

       

                switch (responseStatus)
                {
                    case ResponseStatus.Aborted:
                        
                        e = new Exception("Error - Connection Aborted",inner);
                        
                        break;
                    case ResponseStatus.Error:
                        e = new Exception("Error - Connection Error", inner);
                        
                        break;
                    case ResponseStatus.TimedOut:
                        e = new Exception("Error - Connection Timed Out", inner);
                        
                        break;
                    default:
                        e = new Exception("Error - Connection - Error", inner);
                        break;
                }

              

            }
            else
            {
                

                //something bad happened with server side code from services
                if (Convert.ToInt32(responseStatusCode) >= 400) 
                {
                    success = false;

                    //attempt to serialize error
                    ServiceResponse errorResponse = SerializeServiceError(responseContent);
                    if(errorResponse == null)
                    {
                        e = new Exception(responseContent);
                    }
                    else
                    {
                        e = new Exception(errorResponse.Message);
                    }

                    switch (responseStatusCode)
                    {
                        case ResponseHttpStatusCode.BadGateway:
                            e = new Exception("Error - Bad Gateway", inner);
                            break;
                        case ResponseHttpStatusCode.BadRequest:
                            e = new Exception("Error - Bad Request", inner);
                            break;
                        case ResponseHttpStatusCode.Forbidden:
                            e = new Exception("Error - Forbidden", inner);
                            break;
                        case ResponseHttpStatusCode.GatewayTimeout:
                            e = new Exception("Error - Gateway Timeout", inner);
                            break;
                        case ResponseHttpStatusCode.HttpVersionNotSupported:
                            e = new Exception("Error - Http Version Not Supported", inner);
                            break;
                        case ResponseHttpStatusCode.MethodNotAllowed:
                            e = new Exception("Error - Method Not Allowed", inner);
                            break;
                        case ResponseHttpStatusCode.NotFound:
                            e = new Exception("Error - Not Found", inner);
                            break;
                        case ResponseHttpStatusCode.NotImplemented:
                            e = new Exception("Error - Not Implemented", inner);
                            break;
                        case ResponseHttpStatusCode.ProxyAuthenticationRequired:
                            e = new Exception("Error - Proxy Authentication Required", inner);
                            break;
                        case ResponseHttpStatusCode.RequestEntityTooLarge:
                            e = new Exception("Error - Request Entity Too Large", inner);
                            break;
                        case ResponseHttpStatusCode.RequestTimeout:
                            e = new Exception("Error - Request Timeout", inner);
                            break;
                        case ResponseHttpStatusCode.ServiceUnavailable:
                            e = new Exception("Error - Service Unavailable", inner);
                            break;
                        case ResponseHttpStatusCode.Unauthorized:
                            e = new Exception("Error - Unauthorized", inner);
                            break;
                        case ResponseHttpStatusCode.InternalServerError:
                            //special case to handle web service errors
                          
                            break;
                        default:
                            e = new Exception("Error - Server Error", inner);
                            break;
                    }

                }
                else
                {

                    //we are good
                    success = true;

                }
            }
        }

        public static ServiceResponse SerializeServiceError(string json)
		{
            ServiceResponse error = null;

            try
            {
                error = JsonConvert.DeserializeObject<ServiceResponse>(json);
            }
            catch { }

            return error;

		}

        private RestResponse BuildRestResponse(RestSharp.IRestResponse restSharpResponse)
        {
            var restResponse = new RestResponse();

            restResponse.Content = restSharpResponse.Content;
            restResponse.ContentEncoding = restSharpResponse.ContentEncoding;
            restResponse.ContentLength = restSharpResponse.ContentLength;
            restResponse.ContentType = restSharpResponse.ContentType;
            //TODO: solve this mapping
            //toReturn.Cookies = restSharpResponse.Cookies;
            restResponse.ErrorException = restSharpResponse.ErrorException;
            restResponse.ErrorMessage = restSharpResponse.ErrorMessage;
            //TODO: solve this mapping
            //toReturn.Headers = restSharpResponse.Headers,
            restResponse.RawBytes = restSharpResponse.RawBytes;
            //TODO: solve this mapping
            //toReturn.Request = restRequest;
            restResponse.ResponseStatus = (ResponseStatus)Enum.Parse(typeof(ResponseStatus), restSharpResponse.ResponseStatus.ToString());
            restResponse.ResponseUri = restSharpResponse.ResponseUri;
            restResponse.Server = restSharpResponse.Server;
            restResponse.StatusCode = (ResponseHttpStatusCode)Enum.Parse(typeof(ResponseHttpStatusCode), restSharpResponse.StatusCode.ToString());
            restResponse.StatusDescription = restSharpResponse.StatusDescription;

            return restResponse;
        }


         private RestSharp.IRestClient GetRestClient(string url)
        {
            //TODO: maybe we could use allways the same instance...
            var restClient = new RestSharp.RestClient(url);

            return restClient;
        }

        private RestSharp.IRestRequest BuildRestSharpRequest(RestRequest restRequest)
        {
            var restSharpRequest = new RestSharp.RestRequest(restRequest.Resource);

            restSharpRequest.Method = (RestSharp.Method)Enum.Parse(typeof(RestSharp.Method), restRequest.Method.ToString());
            restSharpRequest.RequestFormat = (RestSharp.DataFormat)Enum.Parse(typeof(RestSharp.DataFormat), restRequest.RequestFormat.ToString());
            restSharpRequest.DateFormat = restRequest.DateFormat;
            
            //added by philip to force timeout within 15 seconds for connect
            restSharpRequest.Timeout = 15000; 

            //TODO: solve this mapping
            //restSharpRequest.Credentials = restRequest.Credentials;

            if (restRequest.Body != null)
                restSharpRequest.AddBody(restRequest.Body);

            foreach (var item in restRequest.Cookies)
                restSharpRequest.AddCookie(item.Key, item.Value);

            foreach (var item in restRequest.Files)
            {

                if (!string.IsNullOrEmpty(item.Path))
                {
                    restSharpRequest.AddFile(item.Name, item.Path);
                }
                else
                {
                    if (String.IsNullOrEmpty(item.ContentType))
                    {
                        restSharpRequest.AddFile(item.Name, item.Bytes, item.FileName);
                    }
                    else
                    {
                        restSharpRequest.AddFile(item.Name, item.Bytes, item.FileName, item.ContentType);
                    }

                }

            }

            foreach (var item in restRequest.Headers)
                restSharpRequest.AddHeader(item.Key, item.Value);

            foreach (var item in restRequest.UrlSegments)
                restSharpRequest.AddUrlSegment(item.Key, item.Value);

            foreach (var item in restRequest.Objects)
                restSharpRequest.AddObject(item.Key, item.Value);



            foreach (var item in restRequest.Parameters)
            {
                RestSharp.ParameterType t = (RestSharp.ParameterType) Enum.Parse(typeof(RestSharp.ParameterType), item.Type.ToString());

                restSharpRequest.AddParameter(
                    item.Name,
                    item.Value,
                    t
                );
            }


            return restSharpRequest;
        }
 
       
    }
}
