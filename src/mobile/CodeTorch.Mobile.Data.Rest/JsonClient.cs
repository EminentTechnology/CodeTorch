using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Mobile.Data.Rest
{
    public class JsonClient: IRestClient
    {
        

        public TimeSpan Timeout
        {
            get
            {
                // TODO: Implement this property getter
                throw new NotImplementedException();
            }
            set
            {
                // TODO: Implement this property setter
                throw new NotImplementedException();
            }
        }

        public void AddHeader(string key, string value)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void RemoveHeader(string key)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<T>> PostAsync<T>(string address, object dto, Format format)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<T>> GetAsync<T>(string address, Format format)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<T>> GetAsync<T>(string address, Dictionary<string, string> values, Format format)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
    }
}
