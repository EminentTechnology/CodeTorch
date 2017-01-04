using CodeTorch.Abstractions;
using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
using CodeTorch.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Backend
{
   
    public class LookupManager
    {
        readonly IConfigurationStore Store;
        readonly ILog Log;

        public LookupManager(IConfigurationStore store, ILogManager logger)
        {
            Store = store;
            Log = logger.GetLogger(typeof(DataCommandManager));
        }

        public async Task<List<Lookup>> Search(string Name)
        {
            List<Lookup> retVal = null;

            retVal = await Store.GetItems<Lookup>();

            //filter by name
            if (!String.IsNullOrEmpty(Name))
            {
                retVal = retVal
                    .Where(l => l.Name.ToLower().Contains(Name.ToLower()))
                    .ToList<Lookup>();
            }

            //order items by name
            retVal = retVal
                .OrderBy(l => l.Name)
                .ToList<Lookup>();

            return retVal;
        }

        public async Task Delete(string Name)
        {
            var item = await Store.GetItem<Lookup>(Name);

            if (item != null)
            {
                await Store.Delete<Lookup>(Name);
            }
            else
            {
                throw new Exception(String.Format("Lookup {0} does not exist in configuration", Name));
            }


        }

        public async Task<string> Create
        (
            string Name
        )
        {
            Lookup retVal = null;
           
            //check for existence
            if (await Store.Exists<Lookup>(Name))
            {
                throw new Exception(String.Format("Lookup {0} already exists", Name));
            }

            retVal = new Lookup();

            
            retVal.Name = Name;
            

            retVal = await Store.Add(Name, retVal);

            return retVal.Name;
        }

        public async Task<Lookup> GetByName(string Name)
        {
            Lookup retVal = null;

            retVal = await Store.GetItem<Lookup>(Name);

            return retVal;
        }

        public async Task<List<LookupItem>> GetItemsByName(string Name)
        {
            List<LookupItem> retVal = new List<LookupItem>();

            var lookup = await Store.GetItem<Lookup>(Name);

            if (lookup != null)
            {
                retVal = lookup.Items;
            }

            return retVal;
        }

        public async Task AddLookupItem(string Name, string Value, string Description, string Sort)
        {
            LookupItem retVal = null;
            
            var lookup = await Store.GetItem<Lookup>(Name);

            if (lookup != null)
            {
                retVal = lookup.Items.SingleOrDefault(i => i.Value.ToLower() == Value.ToLower());

                if (retVal != null)
                {
                    throw new Exception(String.Format("Lookup Item {0}-{1} already exists", Name,Value));
                }

                retVal = new LookupItem();

                retVal.Value = Value;
                retVal.Description = Description;
                retVal.Sort = (String.IsNullOrEmpty(Sort) ? Description : Sort);

                lookup.Items.Add(retVal);

                await Store.Save(Name, lookup);
            }
            else
            {
                throw new Exception(String.Format("Lookup {0} does not exist in configuration", Name));
            }


        }

        public async Task UpdateLookupItem(string Name, string Value, string Description, string Sort)
        {
            LookupItem retVal = null;

            var lookup = await Store.GetItem<Lookup>(Name);

            if (lookup != null)
            {
                retVal = lookup.Items.SingleOrDefault(i => i.Value.ToLower() == Value.ToLower());

                if (retVal == null)
                {
                    throw new Exception(String.Format("Lookup Item {0}-{1} does noe exist", Name, Value));
                }

                retVal.Description = Description;
                retVal.Sort = (String.IsNullOrEmpty(Sort) ? Description : Sort);

                await Store.Save(Name, lookup);
            }
            else
            {
                throw new Exception(String.Format("Lookup {0} does not exist in configuration", Name));
            }


        }

        public async Task DeleteLookupItem(string Name, string Value)
        {
            LookupItem retVal = null;

            var lookup = await Store.GetItem<Lookup>(Name);

            if (lookup != null)
            {
                retVal = lookup.Items.SingleOrDefault(i => i.Value.ToLower() == Value.ToLower());

                if (retVal == null)
                {
                    throw new Exception(String.Format("Lookup Item {0}-{1} does noe exist", Name, Value));
                }

                lookup.Items.Remove(retVal);

                await Store.Save(Name, lookup);
            }
            else
            {
                throw new Exception(String.Format("Lookup {0} does not exist in configuration", Name));
            }


        }

        public async Task UpdateProvider(string Name)
        {
            try
            {
                ILookupProvider lookup = LookupService.GetInstance().LookupProvider;

                if (String.IsNullOrEmpty(Name))
                {

                    var items = await Store.GetItems<Lookup>();



                    foreach (var item in items)
                    {
                        lookup.Save(item);
                    }
                }
                else
                {
                    var item = await Store.GetItem<Lookup>(Name);

                    if (item != null)
                    {
                        lookup.Save(item);
                    }
                    else
                    {
                        throw new Exception(String.Format("Lookup {0} does not exist in configuration", Name));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error Updating Lookup Provider", ex);
            }
            
        }

        public async Task UpdateFromProvider(string Name)
        {
            try
            {
                ILookupProvider lookup = LookupService.GetInstance().LookupProvider;

                if (String.IsNullOrEmpty(Name))
                {

                    var items = lookup.GetLookupTypes();

                    foreach (var item in items)
                    {
                        await Store.Save(item.Name, item);
                    }
                }
                else
                {
                    var item = lookup.GetActiveLookupItems(Name, null, null);

                    if (item != null)
                    {
                        await Store.Save(item.Name, item);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error Updating Lookup From Provider", ex);
            }
            
        }

    }
}
