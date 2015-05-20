using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public enum DataCommandParameterType
    {
        AnsiString,
        Binary,
        Byte,
        Boolean,
        Currency,
        Date,
        DateTime,
        Decimal,
        Double,
        Guid,
        Int16,
        Int32,
        Int64,
        Object,
        SByte,
        Single,
        String,
        Time,
        UInt16,
        UInt32,
        UInt64,
        VarNumeric,
        AnsiStringFixedLength,
        StringFixedLength,
        Xml,
        DateTime2,
        DateTimeOffset,
        Structured
    }
}
