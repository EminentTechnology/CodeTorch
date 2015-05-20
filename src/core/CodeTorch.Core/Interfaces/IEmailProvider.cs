using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core.Interfaces
{
    public interface IEmailProvider
    {
        void Initialize(List<Setting> settings);



        void Send(EmailMessage message);

     
    }
}
