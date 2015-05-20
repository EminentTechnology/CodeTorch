
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace CodeTorch.Core.Interfaces
{
    

    public interface IDocumentProvider
    {
        void Initialize(List<Setting> settings);

        string Upload(Document doc);

       
    }
}
