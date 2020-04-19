using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeTorch.Core.Commands
{
    [Serializable]
    public class DownloadDocumentCommand : ActionCommand
    {

        

        public override string Type
        {
            get
            {
                return "DownloadDocumentCommand";
            }
            set
            {
                base.Type = value;
            }
        }


        

        public string RetrieveCommand { get; set; }




        bool _ForceDownloadWithContentDisposition = true;
        public bool ForceDownloadWithContentDisposition
        {
            get
            {
                return _ForceDownloadWithContentDisposition;
            }
            set
            {
                _ForceDownloadWithContentDisposition = value;
            }
        }

    }
}
