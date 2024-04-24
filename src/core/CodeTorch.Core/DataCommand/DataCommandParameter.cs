using System;

namespace CodeTorch.Core
{
    [Serializable]
    public class DataCommandParameter
    {
        public string Name { get; set; }
        public DataCommandParameterType Type { get; set; }
        public int Size { get; set; }
        public DataCommandParameterDirection Direction { get; set; }
        public string TypeName { get; set; }
        public bool IsUserDefinedType { get; set; }
        public bool IsTableType { get; set; }
        public string Description { get; set; }
    }
}
