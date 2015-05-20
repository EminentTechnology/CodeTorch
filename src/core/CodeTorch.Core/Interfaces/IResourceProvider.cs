using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core.Interfaces
{
    public interface IResourceProvider
    {
        void Initialize(string config);
        List<ResourceItem> GetResourceItemsByResourceSet(string resourceSet);
        List<ResourceItem> GetResourceItemsByCulture(string cultureCode);
        List<ResourceItem> GetResourceItemsByResourceSetCulture(string resourceSet, string cultureCode);
        bool Save(ResourceItem item);
        bool Save(List<ResourceItem> items, bool updateExistingItems);
        
    }
}
