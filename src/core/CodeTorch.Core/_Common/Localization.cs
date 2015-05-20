using System;
using System.Collections.Generic;
using System.Linq;

using CodeTorch.Core.Interfaces;
using CodeTorch.Core.Services;

namespace CodeTorch.Core
{
    public class Localization
    {
        public static List<ResourceItem> GetResourceKeys()
        {
            return PopulateResourceKeys();
        
        }

        private static List<ResourceItem> TestPopulateResourceKeys()
        {
            List<ResourceItem> retVal = new List<ResourceItem>();
            //specific screen
            //Screen s = Screen.GetByFolderAndName("Report", "HeadCountByDepartment.aspx");
            //retVal.AddRange(Screen.GetResourceKeys(s));

            //only menus
                //foreach (Menu menu in Configuration.GetInstance().Menus)
                //{
                //    retVal.AddRange(Menu.GetResourceKeys(menu));
                //}

            //only edit and list edit pages
            //foreach (Screen screen in Configuration.GetInstance().Screens)
            //{
            //    if((screen.Type.ToLower() == "edit") || (screen.Type.ToLower() == "listedit"))
            //    retVal.AddRange(Screen.GetResourceKeys(screen));
            //}

            //only pages in specific folder
            string folder = "Report";
            foreach (Screen screen in Configuration.GetInstance().Screens)
            {
                if ((screen.Folder.ToLower() == folder.ToLower()))
                    retVal.AddRange(Screen.GetResourceKeys(screen));
            }

            return retVal;
        }

        private static List<ResourceItem> PopulateResourceKeys()
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            foreach (Menu menu in Configuration.GetInstance().Menus)
            {
                retVal.AddRange(Menu.GetResourceKeys(menu));
            }

            foreach (Screen screen in Configuration.GetInstance().Screens)
            {
                retVal.AddRange(Screen.GetResourceKeys(screen));
            }

            foreach (Lookup lookup in Configuration.GetInstance().Lookups)
            {
                ILookupProvider lookupDB = LookupService.GetInstance().LookupProvider;

                

                Lookup data = lookupDB.GetLookupItems( lookup.Name, null);

                foreach (LookupItem row in data.Items)
                {
                    ResourceItem key = new ResourceItem();

                    key.ResourceSet = String.Format("Lookup.{0}", data.Name);
                    key.Key = row.Value;
                    key.Value = row.Description;

                    retVal.Add(key);
                }
            }

            return retVal;
        }

        public static List<ResourceItem> GetResourceKeysFromDB(string cultureCode)
        {
            ResourceService resource = ResourceService.GetInstance();
            List<ResourceItem> retVal = resource.ResourceProvider.GetResourceItemsByCulture(cultureCode);

            return retVal;

        }
    }
}
