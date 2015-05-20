using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Web
{
    public class RestServiceException
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public string ErrorCode { get; set; }
        public string MoreInfo { get; set; }


    }
}
