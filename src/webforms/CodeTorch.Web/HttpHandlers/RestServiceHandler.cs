using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using CodeTorch.Core;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Xml;
using System.Web.Security;
using System.Configuration;
using CodeTorch.Core.Services;
using CodeTorch.Core.Interfaces;
using System.Xml.Serialization;

namespace CodeTorch.Web.HttpHandlers
{
    public class RestServiceHandler : IHttpHandler
    {
        /// <summary>
        /// Optionally store RouteData on this handler
        /// so we can access it internally
        /// </summary>
        public RouteData RouteData { get; set; }

        public RestService Me { get; set; }

        string format = "json";

        public string Format
        {
            get { return format; }
            set { format = value; }
        }

        public string EntityID { get; set; }
        public string EntityIDParameterName { get; set; }

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            App app = CodeTorch.Core.Configuration.GetInstance().App;
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);
            
            try
            {
                if (Me == null)
                {
                    if (context.Request.Path.ToLower().EndsWith("xml"))
                        Format = "xml";

                    throw new ApplicationException("No service has been configured for this path");
                }


                DataCommand command = null;
                RestServiceMethodReturnTypeEnum returnType = RestServiceMethodReturnTypeEnum.None;
                

                //collect parameters

                DataTable dt = null;
                XmlDocument doc = null;
                context.Response.TrySkipIisCustomErrors = true;

                if (Format.ToLower() == "json")
                {
                    context.Response.ContentType = "application/json";
                }
                else
                {
                    context.Response.ContentType = "text/xml";
                }

                BaseRestServiceMethod method = Me.Methods.Where(i => (i.Action.ToString() == HttpContext.Current.Request.HttpMethod)).SingleOrDefault();

                if (method != null)
                {
                    //pass parameters - minus dbcommand parameters to datacommand
                    DataCommandService dataCommandDB = DataCommandService.GetInstance();

                    if (String.IsNullOrEmpty(method.RequestDataCommand))
                    { 
                        throw new ApplicationException(String.Format("Request Data Command has not been configured for this service - {0}", Me.Name));
                    }

                    command = DataCommand.GetDataCommand(method.RequestDataCommand);

                    if (command == null)
                    {
                        throw new ApplicationException(String.Format("Request Data Command - {0} - could not be found in configuration", method.RequestDataCommand));
                    }

                    List<ScreenDataCommandParameter> parameters = GetPopulatedCommandParameters(method.RequestDataCommand, method.DataCommands, null);
                    returnType = method.ReturnType;
                    //execute request datacommand and return data if applicable
                    switch (command.ReturnType)
                    {
                        case DataCommandReturnType.Integer:
                            object newID = dataCommandDB.ExecuteDataCommand(method.RequestDataCommand, parameters);

                            if (method is PostRestServiceMethod)
                            {
                                DataCommand postCommand = null;

                                

                                PostRestServiceMethod postMethod = (PostRestServiceMethod)method;
                                returnType = postMethod.ReturnType;
                                if (!String.IsNullOrEmpty(postMethod.ResponseDataCommand))
                                {
                                    postCommand = DataCommand.GetDataCommand(postMethod.ResponseDataCommand);

                                    if (postCommand == null)
                                    {
                                        throw new ApplicationException(String.Format("Response Data Command - {0} - could not be found in configuration", postMethod.ResponseDataCommand));
                                    }

                                    parameters = GetPopulatedCommandParameters(postMethod.ResponseDataCommand, method.DataCommands, newID);


                                    switch (returnType)
                                    {
                                        case RestServiceMethodReturnTypeEnum.DataRow:
                                        case RestServiceMethodReturnTypeEnum.DataTable:
                                            dt = dataCommandDB.GetDataForDataCommand(postMethod.ResponseDataCommand, parameters);
                                            break;
                                        case RestServiceMethodReturnTypeEnum.Xml:
                                            doc = dataCommandDB.GetXmlDataForDataCommand(postMethod.ResponseDataCommand, parameters);
                                            break;
                                    }

                                    
                                }
                            }

                            if (method is PutRestServiceMethod)
                            {
                                PutRestServiceMethod putMethod = (PutRestServiceMethod)method;
                                returnType = putMethod.ReturnType;
                                if (!String.IsNullOrEmpty(putMethod.ResponseDataCommand))
                                {
                                    parameters = GetPopulatedCommandParameters(putMethod.ResponseDataCommand, method.DataCommands, newID);

                                    dt = dataCommandDB.GetDataForDataCommand(putMethod.ResponseDataCommand, parameters);
                                }
                            }
                            
                            break;
                        case DataCommandReturnType.Xml:
                            doc = dataCommandDB.GetXmlDataForDataCommand(method.RequestDataCommand, parameters);
                            break;
                        default:
                            dt = dataCommandDB.GetDataForDataCommand(method.RequestDataCommand, parameters);
                            break;
                    }

                    //in certain cases execute response datacommand


                }
                else
                {
                    throw new NotSupportedException();
                }



                //get data if any
                if (
                    ((dt != null) && (returnType == RestServiceMethodReturnTypeEnum.DataTable)) ||
                    ((dt != null) && (returnType == RestServiceMethodReturnTypeEnum.DataRow)) ||
                    ((doc != null) && (returnType == RestServiceMethodReturnTypeEnum.Xml))
                   )
                {
                    if (Format.ToLower() == "json")
                    {
                        using (JsonWriter json = new JsonTextWriter(writer))
                        {
                            DataColumnCollection columns = null; 




                            switch (returnType)
                            {
                                case RestServiceMethodReturnTypeEnum.DataTable:
                                    columns = dt.Columns;
                                    BuildJsonArrayResponse(app, context, method, command, dt, json, columns);
                                    break;
                                case RestServiceMethodReturnTypeEnum.DataRow:
                                    columns = dt.Columns;
                                    BuildJsonObjectResponse(app, context, method, command, dt, json, columns);
                                    break;
                                case RestServiceMethodReturnTypeEnum.Xml:
                                    BuildJsonXmlObjectResponse(app, method, builder, doc);
                                    break;

                            }




                        }
                    }
                    else
                    {
                        //xml
                        using (XmlWriter xml = new XmlTextWriter(writer))
                        {
                            DataColumnCollection columns = null;




                            switch (method.ReturnType)
                            {
                                case RestServiceMethodReturnTypeEnum.DataTable:
                                    columns = dt.Columns;
                                    BuildXmlListResponse(app, context,method, command, dt, xml, columns);
                                    break;
                                case RestServiceMethodReturnTypeEnum.DataRow:
                                    columns = dt.Columns;
                                    BuildXmlItemResponse(app,context, method, command, dt, xml, columns);
                                    break;
                                case RestServiceMethodReturnTypeEnum.Xml:
                                    BuildXmlObjectResponse(app, method, doc, xml);
                                    break;
                            }
                        }
                    }
                }

                //perform any special post processing



                //string url = ((System.Web.Routing.Route)(RouteData.Route)).Url;
                //context.Response.Write("From the handler at " + DateTime.Now + " - " + Me.Name + " - " + url + " - " + HttpContext.Current.Request.HttpMethod);

                
                //context.Response.ContentType = "text/xml";
                //context.Response.Write(dt.DataSet.GetXml());


                context.Response.StatusCode = (int)HttpStatusCode.OK;

                context.Response.Write(builder.ToString());

            }
            catch (CodeTorchException cex)
            {
                builder = new StringBuilder();
                RestServiceException exception = new RestServiceException();

                exception.Status = (int)HttpStatusCode.InternalServerError;
                exception.Message = cex.Message;
                exception.MoreInfo = cex.MoreInfo;
                exception.ErrorCode = cex.ErrorCode;

                context.Response.TrySkipIisCustomErrors = true;
                context.Response.StatusCode = exception.Status;
                SerializeRestException(app, context, builder, writer, exception);
            }
            catch (Exception ex)
            {
                builder = new StringBuilder();
                RestServiceException exception = new RestServiceException();

                exception.Status = (int)HttpStatusCode.InternalServerError;
                exception.Message = ex.Message;

                context.Response.TrySkipIisCustomErrors = true;
                context.Response.StatusCode = exception.Status;
                SerializeRestException(app, context, builder, writer, exception);
            }
        }

        

        private void SerializeRestException(App app, HttpContext context, StringBuilder builder, StringWriter writer, RestServiceException exception)
        {
            if (Format.ToLower() == "json")
            {
                if (app.RestServiceResponseMode == RestServiceResponseMode.Simple)
                {
                    context.Response.Write(JsonConvert.SerializeObject(exception));
                }
                else
                {
                    StructuredError err = new StructuredError
                    {
                        Error = exception
                    };

                    context.Response.Write(JsonConvert.SerializeObject(err));
                }
            }
            else
            {
                System.Xml.Serialization.XmlSerializer x = null;

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.CloseOutput = true;

                if (app.RestServiceResponseMode == RestServiceResponseMode.Simple)
                {
                    x = new System.Xml.Serialization.XmlSerializer(exception.GetType());

                    using (XmlWriter xmlwriter = XmlWriter.Create(builder, settings))
                    {
                        x.Serialize(xmlwriter, exception);
                        writer.Close();
                    }
                }
                else
                {
                    StructuredError err = new StructuredError
                    {
                        Error = exception
                    };
                    x = new System.Xml.Serialization.XmlSerializer(err.GetType());

                    using (XmlWriter xmlwriter = XmlWriter.Create(builder, settings))
                    {
                        x.Serialize(xmlwriter, err);
                        writer.Close();
                    }
                }
                
                
                context.Response.Write(builder.ToString());
            }
        }

        private static void WriteJsonValue(DataCommand command, JsonWriter json, DataRow row, DataColumn col)
        {
            DataCommandColumn commandCol = command.Columns.Where(c => c.Name.ToLower() == col.ColumnName.ToLower()).SingleOrDefault();

            if (commandCol != null)
            {
                object rawValue = row[col.ColumnName];
                //commandCol.Type
                if (rawValue is DBNull)
                {
                    json.WriteNull();
                }
                else
                {
                    Type t = Type.GetType("System." + commandCol.Type);
                    object v = Convert.ChangeType(rawValue, t);
                    json.WriteValue(v);
                }
                
            }
            else
            {
                json.WriteValue(row[col].ToString());
            }
        }

        private static void WriteXmlValue(DataCommand command, XmlWriter xml, DataRow row, DataColumn col)
        {
            DataCommandColumn commandCol = command.Columns.Where(c => c.Name.ToLower() == col.ColumnName.ToLower()).SingleOrDefault();

            if (commandCol != null)
            {
                object rawValue = row[col.ColumnName];
                //commandCol.Type
                if (rawValue is DBNull)
                {
                    
                }
                else
                {
                    

                    Type t = Type.GetType("System." + commandCol.Type);
                    object v = Convert.ChangeType(rawValue, t);

                    switch (commandCol.Type.ToLower())
                    { 
                        case "guid":
                            xml.WriteValue(v.ToString());
                            break;
                        default:
                            xml.WriteValue(v);
                            break;
                    }

                   
                }

            }
            else
            {
                xml.WriteValue(row[col].ToString());
            }
        }

      

      

        public List<ScreenDataCommandParameter> GetPopulatedCommandParameters(string DataCommandName,  List<ScreenDataCommand> datacommands, object NewID)
        {
            string ErrorFormat = "Invalid {0} propery for Data Command {1} Parameter {2} - {3}";
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();

            ScreenDataCommand screenCommand = ScreenDataCommand.GetDataCommand(datacommands, DataCommandName);
            if ((screenCommand != null) && (screenCommand.Parameters != null))
            {
                parameters = ObjectCopier.Clone<List<ScreenDataCommandParameter>>(screenCommand.Parameters);

                foreach (ScreenDataCommandParameter p in parameters)
                {
                    try
                    {
                        p.Value = GetParameterInputValue(p, NewID);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(
                            String.Format(ErrorFormat, "Value", DataCommandName, p.Name, ex.Message),
                            ex);
                    }

                }
            }




            return parameters;

        }

        private object GetParameterInputValue( IScreenParameter parameter, object newID)
        {
            object retVal = null;
            App app = CodeTorch.Core.Configuration.GetInstance().App;
            CodeTorch.Web.FieldTemplates.BaseFieldTemplate f = null;

            switch (parameter.InputType)
            {
                case ScreenInputType.AppSetting:
                    retVal = ConfigurationManager.AppSettings[parameter.InputKey];
                    break;
                
                case ScreenInputType.Control:
                    throw new NotSupportedException();

                    //if (container == null)
                    //{
                    //    f = page.FindFieldRecursive(parameter.InputKey);
                    //}
                    //else
                    //{
                    //    f = page.FindFieldRecursive(container, parameter.InputKey);
                    //}

                    //if (f != null)
                    //{
                    //    retVal = f.Value;
                    //}

                    break;
                case ScreenInputType.ControlText:

                    //if (container == null)
                    //{
                    //    f = page.FindFieldRecursive(parameter.InputKey);
                    //}
                    //else
                    //{
                    //    f = page.FindFieldRecursive(container, parameter.InputKey);
                    //}

                    //if (f != null)
                    //{
                    //    retVal = f.DisplayText;
                    //}
                    throw new NotSupportedException();

                    break;
                case ScreenInputType.Cookie:
                    retVal = HttpContext.Current.Request.Cookies[parameter.InputKey].Value;
                    break;
                case ScreenInputType.File:
                    //currently onlu supports storage to database
                    if (HttpContext.Current.Request.ContentType.ToLower().Contains("multipart"))
                    {
                        HttpPostedFile file = null;

                        
                        if (HttpContext.Current.Request.Files.Count == 1)
                        {
                            //for refit support
                            file = HttpContext.Current.Request.Files[0] as HttpPostedFile;
                        }
                        else
                        {
                            file = HttpContext.Current.Request.Files[parameter.InputKey] as HttpPostedFile;
                        }
                        file = HttpContext.Current.Request.Files[parameter.InputKey] as HttpPostedFile;

                        if (file != null)
                        {
                            if (file.ContentLength > 0)
                            {
                                DocumentService documentService = DocumentService.GetInstance();

                                if (String.IsNullOrEmpty(parameter.Default))
                                {
                                    retVal = ReadFully(file.InputStream);
                                }
                                else
                                {
                                    DocumentRepository repo = DocumentRepository.GetByName(parameter.Default);

                                    if (repo == null)
                                    {
                                        throw new Exception(String.Format("Parameter {0} is assigned to a missing document repository  - {1}. Please check configuration.", parameter.Name, parameter.Default));
                                    }

                                    IDocumentProvider documentProvider = documentService.GetProvider(repo);
                                    if (documentProvider == null)
                                    {
                                        throw new Exception(String.Format("Parameter {0} is assigned to document repository  - {1}. The provider for this repository could not be found. Please check configuration", parameter.Name, parameter.Default));
                                    }

                                    // AppendDocumentID(DocumentID);
                                    Document document = new Document();//need to clone from config

                                    document.FileName = file.FileName;
                                    document.ContentType = file.ContentType;

                                    if (String.IsNullOrEmpty(document.ContentType))
                                    {
                                        document.ContentType = "application / octet - stream";
                                    }

                                    document.Size = Convert.ToInt32(file.ContentLength);
                                    document.Stream = file.InputStream;
                                    document.EntityID = "TEMP";
                                    document.EntityType = "TEMP";


                                    document.Settings.Add(new Setting("ModifiedBy", "SYSTEM"));

                                    //perform actual upload
                                    document.ID = documentProvider.Upload(document);


                                    retVal = document.ID;
                                }

                                
                            }
                        }
                        
                        
                    }
                    
                    break;
                case ScreenInputType.Form:
                    retVal = HttpContext.Current.Request.Form[parameter.InputKey];
                    break;
                case ScreenInputType.Header:
                    retVal = HttpContext.Current.Request.Headers[parameter.InputKey];
                    break;
                case ScreenInputType.QueryString:
                    retVal = HttpContext.Current.Request.QueryString[parameter.InputKey];
                    break;

                case ScreenInputType.Session:
                    retVal = HttpContext.Current.Session[parameter.InputKey];
                    break;
                case ScreenInputType.Special:
                    switch (parameter.InputKey.ToLower())
                    {

                        case "null":
                            retVal = null;
                            break;
                        case "newid":
                            retVal = newID;
                            break;
                        case "dbnull":
                            retVal = DBNull.Value;
                            break;
                        case "username":

                            retVal = UserIdentityService.GetInstance().IdentityProvider.GetUserName();

                            break;
                        case "hostheader":
                            retVal = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
                            break;
                        case "applicationpath":
                            retVal = HttpContext.Current.Request.ApplicationPath;
                            break;
                        case "urlsegment":
                            try
                            {
                                retVal = this.RouteData.Values[parameter.Default];
                            }
                            catch { }
                            break;
                        case "absoluteapplicationpath":
                            retVal = String.Format("{0}://{1}{2}",
                                HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.ServerVariables["HTTP_HOST"],
                                ((HttpContext.Current.Request.ApplicationPath == "/") ? String.Empty : HttpContext.Current.Request.ApplicationPath));

                            break;

                    }
                    break;

                case ScreenInputType.User:
                    try
                    {
                        List<string> profileProperties = CodeTorch.Core.Configuration.GetInstance().App.ProfileProperties;
                        int propertyIndex = Enumerable.Range(0, profileProperties.Count).First(i => profileProperties[i].ToLower() == parameter.InputKey.ToLower());

                        FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = identity.Ticket;

                        retVal = ticket.UserData.Split('|')[propertyIndex];

                    }
                    catch { }
                    break;
                case ScreenInputType.Constant:
                    retVal = parameter.InputKey;
                    break;
                case ScreenInputType.ServerVariables:
                    retVal = HttpContext.Current.Request.ServerVariables[parameter.InputKey];
                    break;
            }

            if (
                (parameter.InputType != ScreenInputType.Special) &&
                (parameter.InputType != ScreenInputType.File)
               )
            {
                if (retVal == null)
                {
                    retVal = parameter.Default;
                }
            }

            return retVal;
        }

        static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private void BuildXmlObjectResponse(App app, BaseRestServiceMethod method, XmlDocument doc, XmlWriter xml)
        {
            if (app.RestServiceResponseMode == RestServiceResponseMode.IncludeMetaAndError)
            {
                string xmlStructure = @"<Response><Data/><Meta><Status>Success</Status></Meta></Response>";

                XmlDocument structure = new XmlDocument();
                structure.LoadXml(xmlStructure);

                XmlElement data = (XmlElement)structure.SelectSingleNode("/Response/Data");

                data.AppendChild(structure.ImportNode(doc.DocumentElement, true));
                data.WriteTo(xml);
            }
            else
            {
                doc.WriteTo(xml);
            }
        }

        private void BuildXmlItemResponse(App app, HttpContext context, BaseRestServiceMethod method, DataCommand command, DataTable dt, XmlWriter xml, DataColumnCollection commandColumns)
        {
            if (app.RestServiceResponseMode == RestServiceResponseMode.IncludeMetaAndError)
            {
                xml.WriteStartElement("Response");
                xml.WriteStartElement("Data");
            }

            if (dt.Rows.Count > 0)
            {
                DataRow record = dt.Rows[0];
                xml.WriteStartElement(Me.EntityName);

                List<DataColumn> columns = GetColumnsForResponse(context, method, commandColumns);

                foreach (DataColumn col in columns)
                {
                    xml.WriteStartElement(col.ColumnName);
                    WriteXmlValue(command, xml, record, col);
                    xml.WriteEndElement();
                }

                xml.WriteEndElement();
            }
            else
            {

            }

            if (app.RestServiceResponseMode == RestServiceResponseMode.IncludeMetaAndError)
            {
                xml.WriteEndElement(); //data

                //now write meta 

                xml.WriteStartElement("Meta");

                    xml.WriteStartElement("Status");
                    xml.WriteValue("Success");
                    xml.WriteEndElement();

             
                    
                xml.WriteEndElement(); //meta

                xml.WriteEndElement(); //response
            }
        }

        private void BuildXmlListResponse(App app, HttpContext context, BaseRestServiceMethod method, DataCommand command, DataTable dt, XmlWriter xml, DataColumnCollection commandColumns)
        {
            if (app.RestServiceResponseMode == RestServiceResponseMode.IncludeMetaAndError)
            {
                xml.WriteStartElement("Response");
                xml.WriteStartElement("Data");
            }

            xml.WriteStartElement(Me.EntityCollectionName);

            int offset = 0; int limit = 0;
            List<DataRow> rows = GetRowsForResponse(context, method, dt, ref offset, ref limit);

            foreach (DataRow row in rows)
            {
                xml.WriteStartElement(Me.EntityName);

                List<DataColumn> columns = GetColumnsForResponse(context, method, commandColumns);

                foreach (DataColumn col in columns)
                {
                    xml.WriteStartElement(col.ColumnName);
                    WriteXmlValue(command, xml, row, col);
                    xml.WriteEndElement();
                }

                xml.WriteEndElement();
            }
            xml.WriteEndElement();

            if (app.RestServiceResponseMode == RestServiceResponseMode.IncludeMetaAndError)
            {
                xml.WriteEndElement(); //data

                //now write meta 

                xml.WriteStartElement("Meta");

                    xml.WriteStartElement("Status");
                    xml.WriteValue("Success");
                    xml.WriteEndElement();

                    if (method.EnablePaging)
                    {
                        xml.WriteStartElement("Limit");
                        xml.WriteValue(limit);
                        xml.WriteEndElement();

                        xml.WriteStartElement("Offset");
                        xml.WriteValue(offset);
                        xml.WriteEndElement();
                    }

                    xml.WriteStartElement("TotalResults");
                    xml.WriteValue(dt.Rows.Count);
                    xml.WriteEndElement();
                    
                xml.WriteEndElement(); //meta

                xml.WriteEndElement(); //response
            }
        }

        

        private void BuildJsonXmlObjectResponse(App app, BaseRestServiceMethod method, StringBuilder builder, XmlDocument doc)
        {
            if (app.RestServiceResponseMode == RestServiceResponseMode.IncludeMetaAndError)
            {
                string xmlStructure = @"<Response><Data/><Meta><Status>Success</Status></Meta></Response>";

                XmlDocument structure = new XmlDocument();
                structure.LoadXml(xmlStructure);

                XmlElement data = (XmlElement)structure.SelectSingleNode("/Response/Data");

                data.AppendChild(structure.ImportNode(doc.DocumentElement, true));
                
                builder.Append(JsonConvert.SerializeXmlNode(data));
            }
            else
            {
                builder.Append(JsonConvert.SerializeXmlNode(doc));
            }

            
        }

        private void BuildJsonObjectResponse(App app, HttpContext context, BaseRestServiceMethod method, DataCommand command, DataTable dt, JsonWriter json, DataColumnCollection commandColumns)
        {
            if (app.RestServiceResponseMode == RestServiceResponseMode.IncludeMetaAndError)
            {
                json.WriteStartObject(); //root
                json.WritePropertyName("Data");
            }

            if (dt.Rows.Count > 0)
            {
                DataRow record = dt.Rows[0];
                json.WriteStartObject();

                List<DataColumn> columns = GetColumnsForResponse(context, method, commandColumns);

                foreach (DataColumn col in columns)
                {
                    json.WritePropertyName(col.ColumnName);
                    WriteJsonValue(command, json, record, col);
                }

                json.WriteEndObject();
            }
            else
            { 
                //no value to write
                if (app.RestServiceResponseMode == RestServiceResponseMode.IncludeMetaAndError)
                {
                    json.WriteNull(); //end data if no value present
                }
            }

            if (app.RestServiceResponseMode == RestServiceResponseMode.IncludeMetaAndError)
            {
                

                //now write meta 
                json.WritePropertyName("Meta");
                json.WriteStartObject();

                    json.WritePropertyName("Status");
                    json.WriteValue("Success");

                json.WriteEndObject();

                

                json.WriteEndObject(); //end root
            }
        }

        private  void BuildJsonArrayResponse(App app,HttpContext context, BaseRestServiceMethod method, DataCommand command, DataTable dt, JsonWriter json, DataColumnCollection commandColumns)
        {
            if (app.RestServiceResponseMode == RestServiceResponseMode.IncludeMetaAndError)
            {
                json.WriteStartObject(); //root
                json.WritePropertyName("Data");
            }

            json.WriteStartArray();

            int offset = 0;
            int limit = 0;
            List<DataRow> rows = GetRowsForResponse(context, method, dt, ref offset, ref limit);

            foreach (DataRow row in rows)
            {
                json.WriteStartObject();

                List<DataColumn> columns = GetColumnsForResponse(context, method, commandColumns);

                foreach (DataColumn col in columns)
                {
                    json.WritePropertyName(col.ColumnName);
                    WriteJsonValue(command, json, row, col);
                }

                json.WriteEndObject();
            }
            json.WriteEndArray();

            if (app.RestServiceResponseMode == RestServiceResponseMode.IncludeMetaAndError)
            {
                

                //now write meta 
                json.WritePropertyName("Meta");
                json.WriteStartObject();

                    json.WritePropertyName("Status");
                    json.WriteValue("Success");

                    if (method.EnablePaging)
                    {
                        json.WritePropertyName("Limit");
                        json.WriteValue(limit);

                        json.WritePropertyName("Offset");
                        json.WriteValue(offset);
                        
                    }

                    json.WritePropertyName("TotalResults");
                    json.WriteValue(dt.Rows.Count);

                json.WriteEndObject();

                

                json.WriteEndObject(); //end root
            }
        }

        private static List<DataRow> GetRowsForResponse(HttpContext context, BaseRestServiceMethod method, DataTable dt, ref int offset, ref int limit)
        {
            List<DataRow> rows = new List<DataRow>();

            if (method.EnablePaging)
            {
                offset = 0;
                limit = dt.Rows.Count;

                if (method.EnableDefaultLimit)
                {
                    limit = method.PagingLimitDefault;
                }


                //if we have a parameter we can use for offset
                if (!String.IsNullOrEmpty(method.PagingOffsetParameter))
                {
                    //set offset from request
                    string offsetValue = context.Request[method.PagingOffsetParameter];
                    if (!String.IsNullOrEmpty(offsetValue))
                    {
                        int.TryParse(offsetValue, out offset);
                    }
                }

                string limitValue = null;
                //if we have a parameter we can use for limit
                if (!String.IsNullOrEmpty(method.PagingLimitParameter))
                {
                    //set limit from request
                    limitValue = context.Request[method.PagingLimitParameter];
                    if (!String.IsNullOrEmpty(limitValue))
                    {
                        int.TryParse(limitValue, out limit);

                    }

                }

                if (offset < 0)
                {
                    offset = 0;
                }

                if (limit < 0)
                {
                    limit = dt.Rows.Count - offset;
                }

                if ((limit + offset) > dt.Rows.Count)
                {
                    limit = dt.Rows.Count - offset;

                    if (limit < 0)
                    {
                        limit = 0;
                    }
                }

                //now that we have limits and offsets
                rows = dt.Select().ToList<DataRow>().Skip(offset).Take(limit).ToList<DataRow>();

            }
            else
            {
                rows = dt.Rows.Cast<DataRow>().ToList<DataRow>();

            }
            return rows;
        }

        private static List<DataColumn> GetColumnsForResponse(HttpContext context, BaseRestServiceMethod method, DataColumnCollection commandColumns)
        {
            List<DataColumn> columns = new List<DataColumn>();

            if (method.EnablePartialResponse)
            {
                string fieldsValue = null;

                //if we have a parameter we can use for partial response
                if (!String.IsNullOrEmpty(method.PartialResponseParameter))
                {
                    //set fields for partial response from request
                    fieldsValue = context.Request[method.PartialResponseParameter];
                }

                if (String.IsNullOrEmpty(fieldsValue))
                {
                    //this means either we dont have parameter enabled for partial response
                    //or no partial response fields were passed in
                    //so lets see if we have any defaults 
                    fieldsValue = method.PartialResponseDefaultFields;
                }

                if (String.IsNullOrEmpty(fieldsValue))
                {
                    //we still don't have any fields to use for partial response
                    //so return all fields from data source
                    columns = commandColumns.Cast<DataColumn>().ToList<DataColumn>();
                }
                else
                {
                    //we do have fields to use for partial response
                    //lets split them by commas
                    //and return only those fields
                    string[] fields = fieldsValue.Split(',');

                    for (int i = 0; i < fields.Length; i++)
                    {
                        string columnName = fields[i];
                        if (commandColumns.Contains(columnName))
                        {
                            DataColumn col = commandColumns[columnName];
                            columns.Add(col);
                        }

                    }
                }


            }
            else
            {
                //partial response not supported - return all fields from data source
                columns = commandColumns.Cast<DataColumn>().ToList<DataColumn>();
            }
            return columns;
        }

        [XmlRoot("Response")]
        public class StructuredError
        {
            public RestServiceException Error { get; set; }
        }
    }
}
