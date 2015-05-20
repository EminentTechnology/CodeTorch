using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
using CodeTorch.Core.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Resources.Web
{
    public class DataCommandResourceProvider: IResourceProvider
    {
        DataCommandService sql = DataCommandService.GetInstance();

        private const string DataCommandResourceGetByResourceSet = "Resource_GetByResourceSet";
        private const string DataCommandResourceGetByCulture = "Resource_GetByCulture";
        private const string DataCommandResourceGetByResourceSetCulture = "Resource_GetByResourceSetCulture";
        private const string DataCommandResourceSave = "Resource_Save";


        private const string ParameterResourceSet = "@ResourceSet";
        private const string ParameterCultureCode = "@CultureCode";
        private const string ParameterResourceID = "@ResourceID";
        private const string ParameterResourceKey = "@ResourceKey";
        private const string ParameterResourceValue = "@ResourceValue";

        private const string ColumnResourceSet = "ResourceSet";
        private const string ColumnCultureCode = "CultureCode";
        private const string ColumnResourceID = "ResourceID";
        private const string ColumnResourceKey = "ResourceKey";
        private const string ColumnResourceValue = "ResourceValue";

        public void Initialize(string config)
        {
            
        }

        public List<CodeTorch.Core.ResourceItem> GetResourceItemsByResourceSet(string resourceSet)
        {
            
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterResourceSet, resourceSet);
            parameters.Add(p);

            //get data from data command
            DataTable dt = sql.GetDataForDataCommand(DataCommandResourceGetByResourceSet, parameters);

            List<CodeTorch.Core.ResourceItem> retVal = PopulateResourceItems(dt);
            return retVal;
        }

        public List<CodeTorch.Core.ResourceItem> GetResourceItemsByCulture(string cultureCode)
        {
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterCultureCode, cultureCode);
            parameters.Add(p);

            //get data from data command
            DataTable dt = sql.GetDataForDataCommand(DataCommandResourceGetByCulture, parameters);

            List<CodeTorch.Core.ResourceItem> retVal = PopulateResourceItems(dt);
            return retVal;
        }

        public List<CodeTorch.Core.ResourceItem> GetResourceItemsByResourceSetCulture(string resourceSet, string cultureCode)
        {
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterResourceSet, resourceSet);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterCultureCode, cultureCode);
            parameters.Add(p);

            //get data from data command
            DataTable dt = sql.GetDataForDataCommand(DataCommandResourceGetByResourceSetCulture, parameters);

            List<CodeTorch.Core.ResourceItem> retVal = PopulateResourceItems(dt);
            return retVal;
        }

        public bool Save(CodeTorch.Core.ResourceItem item)
        {
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            item.ID = Guid.NewGuid().ToString();

            p = new ScreenDataCommandParameter(ParameterResourceID, item.ID);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterResourceSet, item.ResourceSet);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterCultureCode, item.CultureCode);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterResourceKey, item.Key);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterResourceValue, item.Value);
            parameters.Add(p);

            //get data from data command
            sql.ExecuteDataCommand(DataCommandResourceSave, parameters);

            

            return true;
        }

        public bool Save(List<CodeTorch.Core.ResourceItem> items, bool updateExistingItems)
        {
            foreach (CodeTorch.Core.ResourceItem item in items)
            {
                Save(item);
            }

            return true;
        }

        private static List<CodeTorch.Core.ResourceItem> PopulateResourceItems(DataTable dt)
        {
            List<CodeTorch.Core.ResourceItem> retVal = new List<ResourceItem>();

            foreach (DataRow row in dt.Rows)
            {
                CodeTorch.Core.ResourceItem item = new CodeTorch.Core.ResourceItem();

                item.ID = row[ColumnResourceID].ToString();
                item.CultureCode = row[ColumnCultureCode].ToString();
                item.Key = row[ColumnResourceKey].ToString();
                item.ResourceSet = row[ColumnResourceSet].ToString();
                item.Value = row[ColumnResourceValue].ToString();

                retVal.Add(item);
            }


            return retVal;
        }
    }
}
