using System.Collections.Generic;

namespace CodeTorch.Core
{
    public class DataRow
    {
        Dictionary<string, object> _values = new Dictionary<string, object>();

        public object this[string columnName]
        {
            get
            {
                return _values[columnName];
            }
            set
            {
                _values[columnName] = value;
            }
        }
    }
}