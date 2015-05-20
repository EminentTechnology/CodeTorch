using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeTorch.Core.Interfaces
{
    public interface ILookupProvider
    {
        void Initialize(string config);

        Lookup GetLookupItems(string lookupType, string lookupDescription);

        Lookup GetLookupItems(string cultureCode, string lookupType, string lookupDescription);

        Lookup GetActiveLookupItems(string lookupType, string lookupDescription, string lookupValue);

        Lookup GetActiveLookupItems(string cultureCode, string lookupType, string lookupDescription, string lookupValue);

        List<Lookup> GetLookupTypes();

        void Save(Lookup lookup);
        void Save(DataConnection connection, Lookup lookup);

       
        
    }
}
