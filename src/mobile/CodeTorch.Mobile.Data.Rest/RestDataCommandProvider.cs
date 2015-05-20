using System.Xml.Linq;
using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeTorch.Core;
using Xamarin.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace CodeTorch.Mobile.Data.Rest
{
   

    public class RestDataCommandProvider: IDataCommandProvider
    {
        IRestClient service = null;

        public void Initialize(List<CodeTorch.Core.Setting> settings)
        {
            // TODO: Implement this method
            // throw new NotImplementedException();
            service = DependencyService.Get<IRestClient>();
        }

        public Task<DataTable> GetData(
            CodeTorch.Core.DataConnection connection, 
            CodeTorch.Core.DataCommand dataCommand, 
            List<CodeTorch.Core.ScreenDataCommandParameter> parameters, 
            string commandText,
            Action<DataTable> successAction, 
            Action<Exception> errorAction
            )
        {
            return GetDataTable(connection, dataCommand, parameters, commandText,successAction,errorAction);
            
        }

        

        public Task<XDocument> GetXmlData(
            CodeTorch.Core.DataConnection connection, 
            CodeTorch.Core.DataCommand dataCommand, 
            List<CodeTorch.Core.ScreenDataCommandParameter> parameters, 
            string commandText,
            Action<DataTable> successAction, 
            Action<Exception> errorAction
            )
        {
            // TODO: Implement this method
             throw new NotImplementedException();
        }

        public Task<object> ExecuteCommand(
            CodeTorch.Core.DataConnection connection, 
            CodeTorch.Core.DataCommand dataCommand, 
            List<CodeTorch.Core.ScreenDataCommandParameter> parameters, 
            string commandText,
            Action<DataTable> successAction, 
            Action<Exception> errorAction
            )
        {
            
             return Exec(connection, dataCommand, parameters, commandText);
        }

        public void RefreshSchema(CodeTorch.Core.DataConnection connection, CodeTorch.Core.DataCommand dataCommand)
        {
            // TODO: Implement this method
            // throw new NotImplementedException();
        }

        #region common
        private string GetUrl(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters,  string commandText)
        {
            StringBuilder url = new StringBuilder();

            Setting baseUrlSetting = connection.Settings.Where(i => (i.Name.ToString().ToLower() == "baseurl")).SingleOrDefault<Setting>();
            if (baseUrlSetting != null)
            {
                url.Append(baseUrlSetting.Value);
            }

            

            //var segments = parameters.Where(p => ( p.InputType== ScreenInputType.Special && p.InputKey.ToLower()=="urlsegment")).ToList<ScreenDataCommandParameter>();
            //if (segments != null)
            //{
            //    foreach (ScreenDataCommandParameter segment in segments)
            //    { 
            //        url.Replace(String.Format("{{{0}}}", segment.Name), segment.Value.ToString());
            //    }
            //}


            return url.ToString();

        }    
        #endregion


        #region get datatable
        private async Task<DataTable> GetDataTable(
            DataConnection connection, 
            DataCommand dataCommand, 
            List<ScreenDataCommandParameter> parameters, 
            string commandText,
            Action<DataTable> successAction, 
            Action<Exception> errorAction
            )
        {
            DataTable retVal = null;

            RestRequest request = PrepareRequest(connection, dataCommand, parameters, commandText);

            await Task.Run(() => service.MakeAsyncRequest(request, result => 
                {
                    retVal = BuildDataTableFromText(dataCommand, retVal, result);


                    successAction(retVal);
                   
                },
                error => {
                   // what do we do if it fails - throw error message for display
                    errorAction(error);
                 
                }));

            return retVal;
        }

        private static DataTable BuildDataTableFromText(DataCommand dataCommand, DataTable retVal, RestResponse result)
        {
            retVal = new DataTable();

            List<DataRow> rows = new List<DataRow>();
            DataRow row = null;

            string lastColumnName = null;

            if (dataCommand.ReturnType == DataCommandReturnType.DataTable)
            {
                JsonTextReader reader = new JsonTextReader(new StringReader(result.Content));

                while (reader.Read())
                {

                    switch (reader.TokenType)
                    {

                        case JsonToken.StartObject:
                        //start new row
                            row = new DataRow();
                            break;
                        
                        case JsonToken.PropertyName:
                        //set current column (add if needed - can be optimized)
                            if (!retVal.Columns.Exists(col => col.ColumnName.ToLower() == reader.Value.ToString().ToLower()))
                            {   
                                //column does not exist so lets add column
                                DataColumn column = new DataColumn();
                                column.ColumnName = reader.Value.ToString();
                                //column.DataType = re

                                retVal.Columns.Add(column);
                            }

                            lastColumnName = reader.Value.ToString();
                            break;
                        case JsonToken.Null:
                        //handle null values
                            if ((row != null) && (!String.IsNullOrEmpty(lastColumnName)))
                            {
                                row[lastColumnName] = null;
                            }
                            break;
                        //store value for row column for the types we support
                        case JsonToken.Integer:
                        case JsonToken.Float:
                        case JsonToken.String:
                        case JsonToken.Boolean:
                        case JsonToken.Date:
                            if ((row != null) && (!String.IsNullOrEmpty(lastColumnName)))
                            {
                               row[lastColumnName] = reader.Value;
                            }

                            break;
                        case JsonToken.EndObject:
                        //close existing row
                            if (row != null)
                            {
                                retVal.Rows.Add(row);
                                row = null;
                                lastColumnName = null;
                            }
                            break;
                        //ignored tokens
                        case JsonToken.StartConstructor:
                        case JsonToken.Comment:
                        case JsonToken.Raw:
                        case JsonToken.Bytes:
                        case JsonToken.Undefined:
                        case JsonToken.EndConstructor:
                        case JsonToken.None:
                        case JsonToken.StartArray:
                        case JsonToken.EndArray:
                        //ignored tokens
                            
                            break;


                    }
                }

            }

            
            return retVal;
        }

        #endregion


        #region execute

        

        private async  Task<object> Exec(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            bool retVal = false;
            RestRequest request = PrepareRequest(connection, dataCommand, parameters, commandText);

           
            await Task.Run(() => service.MakeAsyncRequest(request, result => 
                {

                    //convert rest response to a datatable - somehow
                    var i = 1 + 2;

                    retVal = true;
                },
                error => {
                   // what do we do if it fails - throw error message for display
                    retVal = false;
                    throw error;
                }));

                //result => 
                //{

                //    convert rest response to a datatable - somehow
                //    var i = 1 + 2;


                //},
                //error => {
                //    what do we do if it fails - throw error message for display
                //    throw error;
                //});
            
            return true;
        }

        private RestRequest PrepareRequest(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            string url = GetUrl(connection, dataCommand, parameters, commandText);

            Method method = (Method)Enum.Parse(typeof(Method), dataCommand.Type, true);
            DataFormat format = DataFormat.Json;

            if (url.ToLower().Contains(".xml"))
            {
                format = DataFormat.Xml;
            }

            RestRequest request = new RestRequest(url, method, format);

            request.Resource = commandText;



            // specify stored procedure parameters
            //request.

            if (parameters != null)
            {
                foreach (DataCommandParameter p in dataCommand.Parameters)
                {
                    object value = null;
                    ScreenDataCommandParameter screenParam = parameters.Where(sp => sp.Name.ToLower() == p.Name.ToLower()).SingleOrDefault();

                    if (screenParam != null)
                    {
                        value = screenParam.Value;

                        switch (screenParam.InputType)
                        {
                            case ScreenInputType.AppSetting:
                                break;
                            case ScreenInputType.Control:
                                request.AddParameter(screenParam.Name, screenParam.Value.ToString(), ParameterType.GetOrPost);
                                break;
                            case ScreenInputType.ControlText:
                                request.AddParameter(screenParam.Name, screenParam.Value.ToString(), ParameterType.GetOrPost);
                                break;
                            case ScreenInputType.Cookie:
                                request.AddParameter(screenParam.Name, screenParam.Value.ToString(), ParameterType.Cookie);
                                break;
                            case ScreenInputType.File:

                                break;
                            case ScreenInputType.Form:
                                request.AddParameter(screenParam.Name, screenParam.Value.ToString(), ParameterType.GetOrPost);
                                break;
                            case ScreenInputType.Constant:
                                request.AddParameter(screenParam.Name, screenParam.Value.ToString(), ParameterType.GetOrPost);
                                break;
                            case ScreenInputType.QueryString:
                                request.AddParameter(screenParam.Name, screenParam.Value.ToString(), ParameterType.GetOrPost);

                                break;
                            case ScreenInputType.Session:
                                break;
                            case ScreenInputType.Special:
                                if (screenParam.InputKey.ToLower() == "header")
                                {
                                    request.AddParameter(screenParam.Name, screenParam.Value.ToString(), ParameterType.HttpHeader);

                                }
                                break;
                            case ScreenInputType.User:
                                break;
                            case ScreenInputType.DashboardSetting:
                                break;
                            case ScreenInputType.ServerVariables:
                                break;
                        }
                    }




                }
            }
            return request;
        }
        
        #endregion
    }
}
