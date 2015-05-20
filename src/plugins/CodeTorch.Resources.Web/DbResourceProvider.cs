using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;
using System.Collections;
using System.Data;

using System.Globalization;
using System.Collections.Specialized;
using System.Resources;

using CodeTorch.Core;
using CodeTorch.Core.Services;


namespace CodeTorch.Resources.Web
{
    public class DbResourceProvider : IResourceProvider, IDbResourceProvider
    {
        private string _ResourceSetName;
        private IDictionary _resourceCache;
        private static object _SyncLock = new object();
        private DbResourceReader _ResourceReader = null;


        private DbResourceProvider()
        { }

        public DbResourceProvider(string virtualPath, string className)
        {
            _ResourceSetName = className;
            //TODO: wwDbResourceConfiguration.LoadedProviders.Add(this);
        }

        void IDbResourceProvider.ClearResourceCache()
        {
            this._resourceCache.Clear();
        }

        object IResourceProvider.GetObject(string ResourceKey, System.Globalization.CultureInfo Culture)
        {
            string CultureName = null;
            if (Culture != null)
                CultureName = Culture.Name;
            else
                CultureName = CultureInfo.CurrentUICulture.Name;

            return this.GetObjectInternal(ResourceKey, CultureName);
        }

        System.Resources.IResourceReader IResourceProvider.ResourceReader
        {
            get
            {
                if (this._ResourceReader != null)
                    return this._ResourceReader as IResourceReader;

                this._ResourceReader = new DbResourceReader(GetResourceCache(null));
                return this._ResourceReader as IResourceReader;
            }
        }

        private IDictionary GetResourceCache(string cultureName)
        {
            if (cultureName == null)
                cultureName = "";

            if (this._resourceCache == null)
                this._resourceCache = new ListDictionary();

            IDictionary Resources = this._resourceCache[cultureName] as IDictionary;
            if (Resources == null)
            {
                lock (_SyncLock)
                {
                    if (this._resourceCache.Contains(cultureName))
                        Resources = this._resourceCache[cultureName] as IDictionary;
                    else
                        Resources = GetResourceSet(cultureName as string, this._ResourceSetName);
                    this._resourceCache[cultureName] = Resources;
                }
            }

            return Resources;
        }

        private IDictionary GetResourceSet(string CultureName, string ResourceSet)
        {
            if (CultureName == null)
                CultureName = "";

            string ResourceFilter = "";
            

            Dictionary<string, object> hashTable = new Dictionary<string, object>();
            List<ResourceItem> items = null;

            CodeTorch.Core.Interfaces.IResourceProvider resource = ResourceService.GetInstance().ResourceProvider;

            if (string.IsNullOrEmpty(CultureName))
                items = resource.GetResourceItemsByResourceSet(ResourceSet);
            else
                items = resource.GetResourceItemsByResourceSetCulture(ResourceSet, CultureName);

            if (items == null)
                return hashTable as IDictionary;

            try
            {
                foreach (ResourceItem item in items)
                {
                    string resourceValue = item.Value;


                    if (String.IsNullOrEmpty(resourceValue))
                        resourceValue = String.Empty;

                    hashTable.Add(item.Key, resourceValue);
                }

               
            }
            catch { }
            

            return hashTable as IDictionary;
        }

        object GetObjectInternal(string ResourceKey, string CultureName)
        {
            IDictionary Resources = this.GetResourceCache(CultureName);

            object value = null;
            if (Resources == null)
                value = null;
            else
                value = Resources[ResourceKey];

            // *** If we're at a specific culture (en-Us) and there's no value fall back
            // *** to the generic culture (en)
            if (value == null && CultureName.Length > 3)
            {
                // *** try again with the 2 letter locale
                return GetObjectInternal(ResourceKey, CultureName.Substring(0, 2));
            }

            // *** If the value is still null get the invariant value
            if (value == null)
            {
                Resources = this.GetResourceCache("");
                if (Resources == null)
                    value = null;
                else
                    value = Resources[ResourceKey];
            }

            // *** If the value is still null and we're at the invariant culture
            // *** let's add a marker that the value is missing
            // *** this also allows the pre-compiler to work and never return null
            if (value == null && string.IsNullOrEmpty(CultureName))
            {
                // *** No entry there
                value = "";

                // *** DEPENDENCY HERE (#2): using wwDbResourceConfiguration and wwDbResourceDataManager to optionally
                //                           add missing resource keys

                // *** Add a key in the repository at least for the Invariant culture
                // *** Something's referencing but nothing's there
                //TODO - implement
                //if (wwDbResourceConfiguration.Current.AddMissingResources)
                //    new wwDbResourceDataManager().AddResource(ResourceKey, value.ToString(), "", this._ResourceSetName);

            }

            return value;
        }
    }
}
