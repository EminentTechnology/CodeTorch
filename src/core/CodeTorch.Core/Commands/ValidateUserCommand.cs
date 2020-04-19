using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeTorch.Core.Commands
{
    [Serializable]
    public class ValidateUserCommand : ActionCommand
    {
        

       

        public override string Type
        {
            get
            {
                return "ValidateUserCommand";
            }
            set
            {
                base.Type = value;
            }
        }


        int _LogoutTimeout = 30;
        string _PasswordAlgorithm = "HashProvider";
        PasswordMode _PasswordMode = PasswordMode.Hash;
        ScreenInputType _PasswordEntityInputType = ScreenInputType.Control;
        ScreenInputType _RememberMeEntityID = ScreenInputType.Control;

        [Category("Entity")]
        public string PasswordEntityID { get; set; }



        [Category("Entity")]
        public ScreenInputType PasswordEntityInputType
        {
            get { return _PasswordEntityInputType; }
            set { _PasswordEntityInputType = value; }
        }

        [Category("Entity")]
        public string RememberMeEntityID { get; set; }

        [Category("Entity")]
        public ScreenInputType RememberMeEntityInputType
        {
            get { return _RememberMeEntityID; }
            set { _RememberMeEntityID = value; }
        }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string ProfileCommand { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string PasswordField { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string UserNameParameter { get; set; }


        [Category("Security")]
        public string PasswordAlgorithm
        {
            get { return _PasswordAlgorithm; }
            set { _PasswordAlgorithm = value; }
        }

        [Category("Security")]
        public bool RememberMeDefault { get; set; }

        [Category("Security")]
        public PasswordMode PasswordMode
        {
            get { return _PasswordMode; }
            set { _PasswordMode = value; }
        }


        [Category("Security")]
        public int LogoutTimeout
        {
            get
            {
                return _LogoutTimeout;
            }
            set
            {
                _LogoutTimeout = value;
            }
        }



    }
}
