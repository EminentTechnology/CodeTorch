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
   
    public class PermissionManager
    {
        readonly IConfigurationStore Store;
        readonly ILog Log;

        public PermissionManager(IConfigurationStore store, ILogManager logger)
        {
            Store = store;
            Log = logger.GetLogger(typeof(DataCommandManager));
        }

        public async Task<List<Permission>> Search(string Category, string Name)
        {
            List<Permission> retVal = null;

            retVal = await Store.GetItems<Permission>();

            //filter by name
            if (!String.IsNullOrEmpty(Name))
            {
                retVal = retVal
                    .Where(l => l.Name.ToLower().Contains(Name.ToLower()))
                    .ToList<Permission>();
            }

            //filter by category
            if (!String.IsNullOrEmpty(Name))
            {
                retVal = retVal
                    .Where(l => l.Category.ToLower().Contains(Category.ToLower()))
                    .ToList<Permission>();
            }

            //order items by name
            retVal = retVal
                .OrderBy(l => l.Category)
                .ThenBy(l=>l.Name)
                .ToList<Permission>();

            return retVal;
        }

       

        public async Task Delete(string Name)
        {
            var item = await Store.GetItem<Permission>(Name);

            if (item != null)
            {
                await Store.Delete<Permission>(Name);
            }
            else
            {
                throw new Exception(String.Format("Permission {0} does not exist in configuration", Name));
            }


        }

        public async Task<string> Add
        (
            string Name, string Category, string Description

        )
        {
            Permission retVal = null;
           
            //check for existence
            if (await Store.Exists<Permission>(Name))
            {
                throw new Exception(String.Format("Permission {0} already exists", Name));
            }

            retVal = new Permission();

            
            retVal.Name = Name;
            retVal.Category = Category;
            retVal.Description = Description;
          


            retVal = await Store.Add(Name, retVal);

            return retVal.Name;
        }

        public async Task<Permission> GetByName(string Name)
        {
            Permission retVal = null;

            retVal = await Store.GetItem<Permission>(Name);

            return retVal;
        }

       

        public async Task Update(
            string Name, string Category, string Description
            )
        {
            var retVal = await Store.GetItem<Permission>(Name);

            if (retVal != null)
            {
                retVal.Category = Category;
                retVal.Description = Description;

                await Store.Save(Name, retVal);
            }
            else
            {
                throw new Exception(String.Format("Permission {0} does not exist in configuration", Name));
            }


        }

      

    }
}
