using System.Collections.Generic;


namespace CodeTorch.Core.Interfaces
{
    public interface IDocumentProvider
    {
        void Initialize(List<Setting> settings);

        string Upload(Document doc);
    }
}
