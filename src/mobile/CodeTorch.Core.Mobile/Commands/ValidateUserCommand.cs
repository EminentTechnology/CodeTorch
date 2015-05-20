using System;
using System.Linq;

namespace CodeTorch.Core.Commands
{
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



        public string LoginCommand { get; set; }

        public string AfterLoginRedirectScreen { get; set; }

        public string AfterLoginSettings { get; set; }





        



    }
}
