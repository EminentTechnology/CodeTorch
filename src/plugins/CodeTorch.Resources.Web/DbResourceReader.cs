using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Collections;

namespace CodeTorch.Resources.Web
{
    public class DbResourceReader: IResourceReader
    {
        private IDictionary _resources;

        public DbResourceReader(IDictionary resources)
        {
            _resources = resources;
        }

        void IResourceReader.Close()
        {
        }

        System.Collections.IDictionaryEnumerator IResourceReader.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        void IDisposable.Dispose()
        {
        }
    }
}
