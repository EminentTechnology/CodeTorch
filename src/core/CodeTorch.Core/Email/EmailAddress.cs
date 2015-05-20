using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class EmailAddress
    {
        public string ID { get; set; }

        public string Address { get; set; }
        public string DisplayName { get; set; }

        public override string ToString()
        {
            string retVal = DisplayName;

            if (String.IsNullOrEmpty(retVal))
            {
                retVal = Address;

                if (String.IsNullOrEmpty(retVal))
                {
                    retVal = base.ToString();
                }
            }

            return retVal;
        }
    }
}
