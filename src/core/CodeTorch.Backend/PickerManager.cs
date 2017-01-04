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
   
    public class PickerManager
    {
        readonly IConfigurationStore Store;
        readonly ILog Log;

        public PickerManager(IConfigurationStore store, ILogManager logger)
        {
            Store = store;
            Log = logger.GetLogger(typeof(DataCommandManager));
        }

        public async Task<List<Picker>> Search(string Name)
        {
            List<Picker> retVal = null;

            retVal = await Store.GetItems<Picker>();

            //filter by name
            if (!String.IsNullOrEmpty(Name))
            {
                retVal = retVal
                    .Where(l => l.Name.ToLower().Contains(Name.ToLower()))
                    .ToList<Picker>();
            }

            //order items by name
            retVal = retVal
                .OrderBy(l => l.Name)
                .ToList<Picker>();

            return retVal;
        }

        public async Task Delete(string Name)
        {
            var item = await Store.GetItem<Picker>(Name);

            if (item != null)
            {
                await Store.Delete<Picker>(Name);
            }
            else
            {
                throw new Exception(String.Format("Picker {0} does not exist in configuration", Name));
            }


        }

        public async Task<string> Add
        (
            string Name, string Url, string DisplayField, string ValueField,
            string DataCommand, string DataCommandParameter,
            int Height, int Width

        )
        {
            Picker retVal = null;
           
            //check for existence
            if (await Store.Exists<Picker>(Name))
            {
                throw new Exception(String.Format("Picker {0} already exists", Name));
            }

            retVal = new Picker();

            if ((Height == 0) && (Width == 0))
            {
                //default 
                Width = 512;
                Height = 512;
            }
            
            retVal.Name = Name;
            retVal.Url = Url;
            retVal.DisplayField = DisplayField;
            retVal.ValueField = ValueField;
            retVal.DataCommand = DataCommand;
            retVal.DataCommandParameter = DataCommandParameter;
            retVal.Height = Height;
            retVal.Width = Width;


            retVal = await Store.Add(Name, retVal);

            return retVal.Name;
        }

        public async Task<Picker> GetByName(string Name)
        {
            Picker retVal = null;

            retVal = await Store.GetItem<Picker>(Name);

            return retVal;
        }

       

        public async Task Update(
            string Name, string Url, string DisplayField, string ValueField,
            string DataCommand, string DataCommandParameter,
            int Height, int Width
            )
        {
            var retVal = await Store.GetItem<Picker>(Name);

            if (retVal != null)
            {
               
                retVal.Name = Name;
                retVal.Url = Url;
                retVal.DisplayField = DisplayField;
                retVal.ValueField = ValueField;
                retVal.DataCommand = DataCommand;
                retVal.DataCommandParameter = DataCommandParameter;
                retVal.Height = Height;
                retVal.Width = Width;

                await Store.Save(Name, retVal);
            }
            else
            {
                throw new Exception(String.Format("Picker {0} does not exist in configuration", Name));
            }


        }

      

    }
}
