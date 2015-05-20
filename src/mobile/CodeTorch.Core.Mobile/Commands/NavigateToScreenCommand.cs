using System;

using System.Linq;


namespace CodeTorch.Core.Commands
{

    public class NavigateToScreenCommand : ActionCommand
    {
        

       

        public override string Type
        {
            get
            {
                return "NavigateToScreenCommand";
            }
            set
            {
                base.Type = value;
            }
        }

   
        public string NavigateScreen { get; set; }
        public string Context { get; set; }

        public string EntityID { get; set; }



        

    }
}
