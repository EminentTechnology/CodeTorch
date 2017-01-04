using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Xml;

using System.Text;
using CodeTorch.Core.Interfaces;
using CodeTorch.Core;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CodeTorch.Data.Object
{
    public class ObjectDataCommandProvider : IDataCommandProvider
    {
        public void Initialize(StringCollection settings)
        {

        }

        public object ExecuteCommand(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            var t = Exec(connection, dataCommand, parameters, commandText);
            return t.Result;
        }

        public DataTable GetData(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            var t = Exec(connection, dataCommand, parameters, commandText);
            return ConvertToDataTable(t.Result);
        }

        public XmlDocument GetXmlData(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            throw new NotImplementedException();
        }



        public void RefreshSchema(DataConnection connection, DataCommand dataCommand)
        {
            RefreshSchemaDetails(connection, dataCommand);
        }

        private object GetValue(DataCommandParameter p, object Value)
        {
            object retVal = null;

            switch (p.Type)
            {
                case DataCommandParameterType.Boolean:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = null;
                        }
                        else
                        {
                            if (Value is String)
                            {
                                retVal = Boolean.Parse(Value.ToString());
                            }
                            else
                            {
                                retVal = (bool)Value;
                            }

                        }
                    }
                    break;
                case DataCommandParameterType.Date:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = null;
                        }
                        else
                        {
                            if (Value is String)
                            {
                                retVal = (DateTime.Parse(Value.ToString())).Date;
                            }
                            else
                            {
                                retVal = ((DateTime)Value).Date;
                            }


                        }
                    }
                    break;
                case DataCommandParameterType.DateTime:
                case DataCommandParameterType.DateTime2:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = null;
                        }
                        else
                        {
                            if (Value is String)
                            {
                                retVal = DateTime.Parse(Value.ToString());
                            }
                            else
                            {
                                retVal = (DateTime)Value;
                            }
                        }
                    }
                    break;
                case DataCommandParameterType.DateTimeOffset:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = null;
                        }
                        else
                        {
                            if (Value is String)
                            {
                                retVal = DateTimeOffset.Parse(Value.ToString());
                            }
                            else
                            {
                                retVal = (DateTimeOffset)Value;
                            }
                        }
                    }
                    break;
                case DataCommandParameterType.Decimal:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = null;
                        }
                        else
                        {
                            if (Value is string)
                            {
                                retVal = decimal.Parse(Value.ToString());
                            }
                            else
                            {
                                retVal = (decimal)Value;
                            }

                        }
                    }
                    break;
                case DataCommandParameterType.Double:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = null;
                        }
                        else
                        {
                            if (Value is string)
                            {
                                retVal = double.Parse(Value.ToString());
                            }
                            else
                            {
                                retVal = (double)Value;
                            }

                        }
                    }
                    break;
                case DataCommandParameterType.Enum:
                    if (Value != null)
                    {
                        retVal = Enum.Parse(Type.GetType(p.TypeName), Value.ToString(), true);
                    }
                    break;
                case DataCommandParameterType.Guid:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = null;
                        }
                        else
                        {
                            retVal = new Guid(Value.ToString());
                        }
                    }
                    break;
                case DataCommandParameterType.Int16:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = null;
                        }
                        else
                        {
                            if (Value is String)
                            {
                                retVal = short.Parse(Value.ToString());
                            }
                            else
                            {
                                retVal = (short)Value;
                            }
                        }
                    }
                    break;
                case DataCommandParameterType.Int32:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = null;
                        }
                        else
                        {
                            if (Value is String)
                            {
                                retVal = int.Parse(Value.ToString());
                            }
                            else
                            {
                                retVal = (int)Value;
                            }
                        }
                    }
                    break;
                case DataCommandParameterType.Int64:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = null;
                        }
                        else
                        {
                            if (Value is String)
                            {
                                retVal = long.Parse(Value.ToString());
                            }
                            else
                            {
                                retVal = (long)Value;
                            }
                        }
                    }
                    break;
                case DataCommandParameterType.Single:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = null;
                        }
                        else
                        {
                            if (Value is String)
                            {
                                retVal = float.Parse(Value.ToString());
                            }
                            else
                            {
                                retVal = (float)Value;
                            }
                        }
                    }
                    break;
                default:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = null;
                        }
                        else
                        {
                            retVal =  Value;
                        }
                    }
                    break;
            }

            return retVal;
        }

        private void RefreshSchemaDetails(DataConnection connection, DataCommand command)
        {
            //get parameters
            Assembly assembly = null;
            try
            {
                Setting assemblySetting = connection.Settings.Where(i => (i.Name.ToString().ToLower() == "assembly_fullpath")).SingleOrDefault<Setting>();
                Setting classSetting = connection.Settings.Where(i => (i.Name.ToString().ToLower() == "classname")).SingleOrDefault<Setting>();
                Setting resolverSetting = connection.Settings.Where(i => (i.Name.ToString().ToLower() == "useresolver")).SingleOrDefault<Setting>();

                if (assemblySetting != null && classSetting != null)
                {
                    bool UseResolver = false;

                    if (resolverSetting != null)
                    {
                        bool.TryParse(resolverSetting.Value, out UseResolver);
                    }

                    assembly = Assembly.LoadFrom(assemblySetting.Value);

                    if (assembly != null)
                    {
                        Type type = assembly.GetType(classSetting.Value, true, true);

                        if (type != null)
                        {
                            MethodInfo method = type.GetMethod(command.Text);
                            int index = 0;

                            if (method != null)
                            {
                                command.Parameters.Clear();

                                while ((method.GetParameters().Length - 1) >= index)
                                {
                                    CodeTorch.Core.DataCommandParameter param = new CodeTorch.Core.DataCommandParameter();
                                    param.Name = method.GetParameters()[index].Name;
                                    var typeArray = method.GetParameters()[index].ParameterType.ToString().Split('.');
                                    var strtype = typeArray[typeArray.Count() - 1];

                                    //TODO - need mapping from common types to  DataCommandParameterTypes
                                    param.Type = (DataCommandParameterType)Enum.Parse(typeof(DataCommandParameterType), strtype);

                                    //TODO - need mapping from common types to standard sizes - although not really needed for this provider
                                    param.Size = 5000;

                                    //TODO - should we check to out type to map to out
                                    param.Direction = DataCommandParameterDirection.In;


                                    param.IsTableType = false;
                                    param.IsUserDefinedType = false;

                                    command.Parameters.Add(param);
                                    index++;
                                }
                            }

                            //TODO: need to take into account Task/TaskEnumerable/Async - similar to Exec method
                            if (method.ReturnType.Name.ToLower() != "void")
                            {
                                var entity = Activator.CreateInstance(method.ReturnType, false);
                                command.Columns.Clear();
                                var enumerable = entity as IEnumerable<object>;

                                if (enumerable != null)
                                {
                                    entity = Activator.CreateInstance(enumerable.GetType().GetGenericArguments()[0], false);
                                }

                                foreach (PropertyInfo property in entity?.GetType().GetProperties())
                                {
                                    CodeTorch.Core.DataCommandColumn column = new CodeTorch.Core.DataCommandColumn();

                                    column.Name = property.Name;
                                    column.Type = property.PropertyType.ToString();

                                    command.Columns.Add(column);

                                }

                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    Common.LogException(e, false);
                    throw e.InnerException;
                }
                else
                {
                    Common.LogException(e);
                }
            }
            finally
            {
                if (assembly != null)
                    assembly = null;
            }
        }

        private static bool IsAsyncMethod(MethodInfo method)
        {
            
            Type attType = typeof(AsyncStateMachineAttribute);

            // Obtain the custom attribute for the method. 
            // The value returned contains the StateMachineType property. 
            // Null is returned if the attribute isn't present for the method. 
            var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);

            return (attrib != null);
        }
        private async Task<object> Exec(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            object retVal = null;
            Assembly assembly = null;
            

            try
            {
                Setting assemblySetting = connection.Settings.Where(i => (i.Name.ToString().ToLower() == "assembly")).SingleOrDefault<Setting>();
                Setting classSetting = connection.Settings.Where(i => (i.Name.ToString().ToLower() == "classname")).SingleOrDefault<Setting>();
                Setting resolverSetting = connection.Settings.Where(i => (i.Name.ToString().ToLower() == "useresolver")).SingleOrDefault<Setting>();

                if (assemblySetting != null && classSetting != null)
                {
                    bool UseResolver = false;

                    if (resolverSetting != null)
                    {
                        bool.TryParse(resolverSetting.Value, out UseResolver);
                    }

                    assembly = Assembly.Load(assemblySetting.Value);

                    if (assembly != null)
                    {
                        Type type = assembly.GetType(classSetting.Value, true, true);

                        if (type != null)
                        {
                            MethodInfo method = type.GetMethod(commandText);

                            if(method == null)
                            throw new Exception(String.Format("Unable to find method {0} in Data Command - {1}", commandText, dataCommand.Name ));

                            object instance = null;

                            if (UseResolver)
                            {
                                instance = Resolver.Resolve(type);
                            }
                            else
                            {
                                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                                instance = constructor.Invoke(null);
                            }
                            

                            object[] signature = new object[parameters.Count()];
                            int index = 0;

                            foreach (DataCommandParameter p in dataCommand.Parameters)
                            {
                                object value = null;
                                ScreenDataCommandParameter screenParam;

                                try
                                {
                                    screenParam = parameters.Where(sp => sp.Name.ToLower() == p.Name.ToLower()).SingleOrDefault();
                                }
                                catch (Exception ex)
                                {
                                    throw new ApplicationException("Screen Parameter is null when value is expected", ex);
                                }

                                if (screenParam != null)
                                {
                                    try
                                    {
                                        value = GetValue(p, screenParam.Value);
                                        signature[index] = value;

                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception(string.Format("Error trying to set parameter {0} - {1}", p.Name, ex.Message), ex);
                                    }
                                    
                                }

                                index++;
                            }

                            try
                            {
                                if (IsAsyncMethod(method))
                                {
                                    if (method.ReturnType == typeof(Task))
                                    {
                                        await (dynamic)method.Invoke(instance, signature);
                                    }
                                    else
                                    {
                                        retVal = await (dynamic)method.Invoke(instance, signature);
                                    }
                                    
                                    
                                }
                                else
                                {
                                    retVal = method.Invoke(instance, signature);
                                }

                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    Common.LogException(ex, false);
                                    throw new Exception(String.Format("{0} - {1}", ex.Message, ex.InnerException.Message), ex);
                                }
                                else
                                {
                                    Common.LogException(ex, false);
                                    throw ex;
                                }
                            }


                        }
                    }

                }


            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    Common.LogException(e, false);
                    throw e.InnerException;
                }
                else
                {
                    Common.LogException(e);
                }
            }
            finally
            {
                if (assembly != null)
                    assembly = null;
            }


            return retVal;

        }


        private static DataTable ConvertToDataTable(object entity)
        {
            DataTable retVal = null;
            var enumerable = entity as IEnumerable<object>;

            if (enumerable != null)
            {
                retVal = ResolveListToDataTable(enumerable);
            }
            else
            {
                if (entity != null)
                    retVal = ResolveToDataTable(entity);
            }

            return retVal;

        }

        private static DataTable ResolveToDataTable(object entity)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo property in entity?.GetType().GetProperties())
            {
                dt.Columns.Add(new DataColumn(property.Name, property.PropertyType));
            }

            DataRow newRow = dt.NewRow();

            foreach (PropertyInfo property in entity?.GetType().GetProperties())
            {
                newRow[property.Name] = entity.GetType().GetProperty(property.Name).GetValue(entity, null);
            }

            dt.Rows.Add(newRow);

            return dt;
        }

        private static DataTable ResolveListToDataTable(IEnumerable<object> entities)
        {
            var dt = new DataTable();

            if (entities != null)
            {
                if (entities.Count() > 0)
                {

                    foreach (PropertyInfo property in entities.ElementAt(0).GetType().GetProperties())
                    {
                        dt.Columns.Add(new DataColumn(property.Name, property.PropertyType));
                    }

                    foreach (object entity in entities)
                    {
                        DataRow newRow = dt.NewRow();

                        foreach (PropertyInfo property in entity.GetType().GetProperties())
                        {
                            newRow[property.Name] = entity.GetType().GetProperty(property.Name).GetValue(entity, null);
                        }

                        dt.Rows.Add(newRow);
                    }
                }
            }

            return dt;
        }

    }
}
