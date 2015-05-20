using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public enum GridCsvDelimiter
    {
        NewLine = 0,
        Semicolon = 1,
        Colon = 2,
        Comma = 3,
        Tab = 4,
        VerticalBar = 5
    }
}
