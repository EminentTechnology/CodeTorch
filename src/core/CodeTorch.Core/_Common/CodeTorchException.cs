using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core
{
    public class CodeTorchException: Exception
    {
        public CodeTorchException()
		{
            
		}

		public CodeTorchException(string message): base(message)
		{
		}

		public CodeTorchException(string message,
			Exception innerException): base(message, innerException)
		{
		}

        protected CodeTorchException(SerializationInfo info,
			StreamingContext context): base(info, context)
		{
			if (info != null)
			{
                this.Status = info.GetInt32("Status");
                this.ErrorCode = info.GetString("ErrorCode");
                this.MoreInfo = info.GetString("MoreInfo");
			}
		}

        public int Status { get; set; }
        

        public string ErrorCode { get; set; }
        public string MoreInfo { get; set; }
    }
}
