using System;
using System.Linq;

namespace CodeTorch.Core
{
    public enum RestServiceMethodActionEnum
    { 
        GET,
        POST,
        PUT,
        DELETE
    }

    public enum RestServiceMethodReturnTypeEnum
    {
        None,
        DataTable,
        DataRow,
        Xml,
        Raw
    }
}
