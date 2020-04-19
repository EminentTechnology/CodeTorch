using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core
{
    public enum ValidationDataType
    {
        //
        // Summary:
        //     A string data type. The value is treated as a System.String.
        String = 0,
        //
        // Summary:
        //     A 32-bit signed integer data type. The value is treated as a System.Int32.
        Integer = 1,
        //
        // Summary:
        //     A double precision floating point number data type. The value is treated as a
        //     System.Double.
        Double = 2,
        //
        // Summary:
        //     A date data type. Only numeric dates are allowed. The time portion cannot be
        //     specified.
        Date = 3,
        //
        // Summary:
        //     A monetary data type. The value is treated as a System.Decimal. However, currency
        //     and grouping symbols are still allowed.
        Currency = 4
    }
}
