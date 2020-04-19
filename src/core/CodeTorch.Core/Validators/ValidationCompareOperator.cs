using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core
{
    //
    // Summary:
    //     Specifies the validation comparison operators used by the System.Web.UI.WebControls.CompareValidator
    //     control.
    public enum ValidationCompareOperator
    {
        //
        // Summary:
        //     A comparison for equality.
        Equal = 0,
        //
        // Summary:
        //     A comparison for inequality.
        NotEqual = 1,
        //
        // Summary:
        //     A comparison for greater than.
        GreaterThan = 2,
        //
        // Summary:
        //     A comparison for greater than or equal to.
        GreaterThanEqual = 3,
        //
        // Summary:
        //     A comparison for less than.
        LessThan = 4,
        //
        // Summary:
        //     A comparison for less than or equal to.
        LessThanEqual = 5,
        //
        // Summary:
        //     A comparison for data type only.
        DataTypeCheck = 6
    }
}
