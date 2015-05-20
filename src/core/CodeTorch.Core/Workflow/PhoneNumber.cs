using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PhoneNumber
    {
        public string Phone { get; set; }

        public override string ToString()
        {
            string retVal = Phone;

            if (String.IsNullOrEmpty(retVal))
            {
                retVal = base.ToString();
            }

            return retVal;
        }
    }
}
