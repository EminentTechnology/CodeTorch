using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    //meaningful only when used as part of SelectFields collection of a group expression
    [Serializable]
    public enum GridAggregateFunction
    {
        None=0,	
        Sum=1,	
        Min=2,
        Max=3,
        Last=4,
        First=5,
        Count=6,
        Avg=7,
        CountDistinct=8,
        Custom=9
    }
}
