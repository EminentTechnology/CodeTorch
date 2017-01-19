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
   
    public class ScreenManager
    {
        readonly IConfigurationStore Store;
        readonly ILog Log;

        public ScreenManager(IConfigurationStore store, ILogManager logger)
        {
            Store = store;
            Log = logger.GetLogger(typeof(DataCommandManager));
        }

        public async Task<List<Screen>> Search(string Name)
        {
            List<Screen> retVal = null;

            retVal = await Store.GetItems<Screen>();

            //filter by name
            if (!String.IsNullOrEmpty(Name))
            {
                retVal = retVal
                    .Where(l => l.Name.ToLower().Contains(Name.ToLower()))
                    .ToList<Screen>();
            }

            //order items by name
            retVal = retVal
                .OrderBy(l => l.Name)
                .ToList<Screen>();

            return retVal;
        }

        public async Task Delete(string Name)
        {
            var item = await Store.GetItem<Screen>(Name);

            if (item != null)
            {
                await Store.Delete<Screen>(Name);
            }
            else
            {
                throw new Exception(String.Format("Screen {0} does not exist in configuration", Name));
            }


        }

        public async Task<string> Add
        (
            string Name, string Url, string DisplayField, string ValueField,
            string DataCommand, string DataCommandParameter,
            int Height, int Width

        )
        {
            Screen retVal = null;
           
            //check for existence
            if (await Store.Exists<Screen>(Name))
            {
                throw new Exception(String.Format("Screen {0} already exists", Name));
            }

            retVal = new Screen();

            if ((Height == 0) && (Width == 0))
            {
                //default 
                Width = 512;
                Height = 512;
            }
            
            //retVal.Name = Name;
            //retVal.Url = Url;
            //retVal.DisplayField = DisplayField;
            //retVal.ValueField = ValueField;
            //retVal.DataCommand = DataCommand;
            //retVal.DataCommandParameter = DataCommandParameter;
            //retVal.Height = Height;
            //retVal.Width = Width;


            retVal = await Store.Add(Name, retVal);

            return retVal.Name;
        }

        public async Task<Screen> GetByName(string Name)
        {
            Screen retVal = null;

            retVal = await Store.GetItem<Screen>(Name);

            return retVal;
        }

        public async Task<Screen> GetSections(string Name)
        {
            Screen retVal = null;

            retVal = await Store.GetItem<Screen>(Name);

            return retVal;
        }



        public async Task Update(
            string Name, string Url, string DisplayField, string ValueField,
            string DataCommand, string DataCommandParameter,
            int Height, int Width
            )
        {
            var retVal = await Store.GetItem<Screen>(Name);

            if (retVal != null)
            {
               
                //retVal.Name = Name;
                //retVal.Url = Url;
                //retVal.DisplayField = DisplayField;
                //retVal.ValueField = ValueField;
                //retVal.DataCommand = DataCommand;
                //retVal.DataCommandParameter = DataCommandParameter;
                //retVal.Height = Height;
                //retVal.Width = Width;

                await Store.Save(Name, retVal);
            }
            else
            {
                throw new Exception(String.Format("Screen {0} does not exist in configuration", Name));
            }


        }

      

    }
}
