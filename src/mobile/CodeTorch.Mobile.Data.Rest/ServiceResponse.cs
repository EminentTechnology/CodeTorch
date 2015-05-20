using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Mobile.Data.Rest
{
    public class ServiceResponse
	{
		public ServiceResponse ()
		{
		}
		public int Status { get; set; }
		public string Message { get; set; }
        public string ErrorCode { get; set; }
        public string MoreInfo { get; set; }
		public override string ToString ()
		{
			return Message.ToString();
		}
	}
}
