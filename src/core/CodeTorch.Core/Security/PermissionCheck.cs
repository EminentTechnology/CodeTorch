using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class PermissionCheck
    {
        public PermissionCheck()
        { }

        public PermissionCheck(bool CheckPermission)
        {
            _CheckPermission = CheckPermission;
        }


        [TypeConverter("CodeTorch.Core.Design.PermissionTypeConverter,CodeTorch.Core.Design")]
        public string Name { get; set; }

        private bool _CheckPermission = false;


        public bool CheckPermission
        {
            get
            {
                return _CheckPermission;
            }
            set
            {
                _CheckPermission = value;
            }
        }

        public override string ToString()
        {
            string retVal = Name;

            if (!CheckPermission)
            {
                retVal = "Not Checked";
            }
            else
            {
                if (String.IsNullOrEmpty(Name))
                    retVal = "None Selected";
            }

            return retVal;
        }

    }
}
