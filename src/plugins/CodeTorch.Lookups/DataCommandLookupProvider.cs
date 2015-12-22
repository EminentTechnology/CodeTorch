using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeTorch.Core.Services;

namespace CodeTorch.Lookups
{
    public class DataCommandLookupProvider : ILookupProvider
    {
        DataCommandService sql = DataCommandService.GetInstance();

        private const string DataCommandLookupGetItems = "Lookup_GetItems";
        private const string DataCommandLookupGetTypes = "Lookup_GetTypes";
        private const string DataCommandLookupGetItemsWithCulture = "Lookup_GetItemsWithCulture";
        private const string DataCommandLookupGetActiveItems = "Lookup_GetActiveItems";
        private const string DataCommandLookupGetActiveItemsWithCulture = "Lookup_GetActiveItemsWithCulture";
        private const string DataCommandLookupSave = "Lookup_Save";
        private const string DataCommandLookupDeactivate = "Lookup_Deactivate";

        private const string ParameterCultureCode = "@CultureCode";
        private const string ParameterLookupType = "@LookupType";
        private const string ParameterLookupDescription = "@LookupDescription";
        private const string ParameterLookupValue = "@LookupValue";
        private const string ParameterSort = "@LookupSort";

        private const string ColumnLookupType = "LookupType";
        private const string ColumnLookupValue = "LookupValue";
        private const string ColumnLookupDescription = "LookupDescription";
        private const string ColumnLookupSort = "LookupSort";

        public DataConnection Connection { get; set; }

        public void Initialize(string config)
        {
            //read any special config needed for specific implementation
        }

        public CodeTorch.Core.Lookup GetLookupItems(string lookupType, string lookupDescription)
        {
            Lookup retVal = null;

            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterLookupType, lookupType);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterLookupDescription, lookupDescription);
            parameters.Add(p);

            //get data from data command
            DataTable dt = GetDataForDataCommand(DataCommandLookupGetItems, parameters);

            retVal = PopulateLookup(retVal, dt);

            return retVal;
        }


        public CodeTorch.Core.Lookup GetLookupItems(string cultureCode, string lookupType, string lookupDescription)
        {
            Lookup retVal = null;

            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterCultureCode, cultureCode);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterLookupType, lookupType);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterLookupDescription, lookupDescription);
            parameters.Add(p);

            //get data from data command
            DataTable dt = GetDataForDataCommand(DataCommandLookupGetItemsWithCulture, parameters);

            retVal = PopulateLookup(retVal, dt);

            return retVal;
        }

        public CodeTorch.Core.Lookup GetActiveLookupItems(string lookupType, string lookupDescription, string lookupValue)
        {
            Lookup retVal = null;

            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterLookupType, lookupType);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterLookupDescription, lookupDescription);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterLookupValue, lookupValue);
            parameters.Add(p);

            //get data from data command
            DataTable dt = GetDataForDataCommand(DataCommandLookupGetActiveItems, parameters);

            retVal = PopulateLookup(retVal, dt);

            return retVal;
        }

        public CodeTorch.Core.Lookup GetActiveLookupItems(string cultureCode, string lookupType, string lookupDescription, string lookupValue)
        {
            Lookup retVal = null;

            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterCultureCode, cultureCode);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterLookupType, lookupType);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterLookupDescription, lookupDescription);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterLookupValue, lookupValue);
            parameters.Add(p);

            //get data from data command
            DataTable dt = GetDataForDataCommand(DataCommandLookupGetActiveItemsWithCulture, parameters);

            retVal = PopulateLookup(retVal, dt);

            return retVal;
        }

        public List<Lookup> GetLookupTypes()
        {
            List<Lookup> retVal = new List<Lookup>();

            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            

            //get data from data command
            DataTable dt = GetDataForDataCommand(DataCommandLookupGetTypes, parameters);

            if (dt.Rows.Count > 0)
            {
                
                foreach (DataRow row in dt.Rows)
                {
                    Lookup item = new Lookup();

                    item.Name = row[ColumnLookupType].ToString();
                   

                    retVal.Add(item);
                }
            }

            return retVal;
        }

        public void Save(CodeTorch.Core.Lookup lookup)
        {
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;


            p = new ScreenDataCommandParameter(ParameterLookupType, lookup.Name);
            parameters.Add(p);

            //deactivate all lookup items
            ExecuteDataCommand(DataCommandLookupDeactivate, parameters);

            foreach (LookupItem item in lookup.Items)
            {
                //reactivate lookups items 1 by 1
                parameters = new List<ScreenDataCommandParameter>();
                p = null;

                

                p = new ScreenDataCommandParameter(ParameterLookupType, lookup.Name);
                parameters.Add(p);

                p = new ScreenDataCommandParameter(ParameterLookupValue, item.Value);
                parameters.Add(p);

                p = new ScreenDataCommandParameter(ParameterLookupDescription, item.Description);
                parameters.Add(p);

                p = new ScreenDataCommandParameter(ParameterSort, item.Sort);
                parameters.Add(p);

                //save lookups to db 
                ExecuteDataCommand(DataCommandLookupSave, parameters);
            }
        }

       
        private static Lookup PopulateLookup(Lookup retVal, DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                retVal = new Lookup();
                retVal.Name = dt.Rows[0][ColumnLookupType].ToString();

                foreach (DataRow row in dt.Rows)
                {
                    LookupItem item = new LookupItem();

                    item.Value = row[ColumnLookupValue].ToString();
                    item.Description = row[ColumnLookupDescription].ToString();
                    item.Sort = row[ColumnLookupSort].ToString();

                    retVal.Items.Add(item);
                }
            }
            return retVal;
        }

        private void ExecuteDataCommand(string dataCommand, List<ScreenDataCommandParameter> parameters)
        {
            if (Connection == null)
            {
                sql.ExecuteDataCommand(dataCommand, parameters);
            }
            else
            {
                DataCommand command = DataCommand.GetDataCommand(dataCommand);

                if (command == null)
                    throw new Exception(String.Format("DataCommand {0} could not be found in configuration", dataCommand));

                sql.ExecuteCommand(null, Connection, command, parameters, command.Text);
            }
        }

        private DataTable GetDataForDataCommand(string dataCommand, List<ScreenDataCommandParameter> parameters)
        {
            DataTable retVal = null;

            if (Connection == null)
            {
                retVal = sql.GetDataForDataCommand(dataCommand, parameters);
            }
            else
            {
                DataCommand command = DataCommand.GetDataCommand(dataCommand);

                if (command == null)
                    throw new Exception(String.Format("DataCommand {0} could not be found in configuration", dataCommand));

                retVal = sql.GetData(null, Connection, command, parameters, command.Text);
            }

            return retVal;
        }
    }
}
