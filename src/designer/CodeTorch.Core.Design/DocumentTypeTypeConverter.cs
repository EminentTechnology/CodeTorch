using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Specialized;
using CodeTorch.Core.Interfaces;
using CodeTorch.Core.Services;

namespace CodeTorch.Core.Design
{
    public class DocumentTypeTypeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            //this means a standard list of values are supported
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            //the actual list of standard items to return
            StandardValuesCollection list = null;

            ILookupProvider lookupDB = LookupService.GetInstance().LookupProvider;

            Lookup data = lookupDB.GetLookupItems("DOCUMENT_TYPE", null);

            StringCollection collection = new StringCollection();

            foreach (LookupItem record in data.Items)
            {
                collection.Add(record.Value);
            }

            list = new StandardValuesCollection(collection);


            return list;


        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
