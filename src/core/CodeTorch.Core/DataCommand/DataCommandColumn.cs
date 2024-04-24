using System;

namespace CodeTorch.Core
{
    [Serializable]
    public class DataCommandColumn
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
