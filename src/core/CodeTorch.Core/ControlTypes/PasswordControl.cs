using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class PasswordControl: BaseControl
    {
        string _PasswordAlgorithm = "HashProvider";

        public string PasswordAlgorithm
        {
            get { return _PasswordAlgorithm; }
            set { _PasswordAlgorithm = value; }
        }

        PasswordMode _PasswordMode = PasswordMode.Hash;

        public PasswordMode PasswordMode
        {
            get { return _PasswordMode; }
            set { _PasswordMode = value; }
        }

        public override string Type
        {
            get
            {
                return "Password";
            }
            set
            {
                base.Type = value;
            }
        }

       

        
    }
}
