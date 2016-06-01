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

namespace CodeTorch.Data.Object
{
    public class ObjectDataCommandProvider : IDataCommandProvider
    {
        public void Initialize(StringCollection settings)
        {

        }

        public object ExecuteCommand(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            return Exec(connection, dataCommand, parameters, commandText);
        }

        public DataTable GetData(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            return ConvertToDataTable(Exec(connection, dataCommand, parameters, commandText));
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
                case DataCommandParameterType.Guid:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = DBNull.Value;
                        }
                        else
                        {
                            retVal = new Guid(Value.ToString());
                        }
                    }
                    break;
                case DataCommandParameterType.Int32:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = DBNull.Value;
                        }
                        else
                        {
                            retVal = Value;
                        }
                    }
                    break;
                default:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = DBNull.Value;
                        }
                        else
                        {
                            retVal = Value;
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

                if (assemblySetting != null && classSetting != null)
                {

                    assembly = Assembly.LoadFrom(assemblySetting.Value);

                    if (assembly != null)
                    {
                        Type type = assembly.GetType(classSetting.Value, true, true);

                        if (type != null)
                        {
                            MethodInfo method = type.GetMethod(command.Text);
                            int index = 0;

                            command.Parameters.Clear();

                            if (method != null)
                            {
                                while ((method.GetParameters().Length - 1) >= index)
                                {
                                    CodeTorch.Core.DataCommandParameter param = new CodeTorch.Core.DataCommandParameter();
                                    param.Name = "@" + method.GetParameters()[index].Name;
                                    var typeArray = method.GetParameters()[index].ParameterType.ToString().Split('.');
                                    var strtype = typeArray[typeArray.Count() - 1];

                                    param.Type = (DataCommandParameterType)Enum.Parse(typeof(DataCommandParameterType), strtype);
                                    param.Size = 5000;
                                    param.Direction = DataCommandParameterDirection.In;
                                    param.IsTableType = false;
                                    param.IsUserDefinedType = false;

                                    command.Parameters.Add(param);
                                    index++;
                                }
                            }

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

        private object Exec(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            object retVal = null;
            Assembly assembly = null;
            try
            {
                Setting assemblySetting = connection.Settings.Where(i => (i.Name.ToString().ToLower() == "assembly")).SingleOrDefault<Setting>();
                Setting classSetting = connection.Settings.Where(i => (i.Name.ToString().ToLower() == "classname")).SingleOrDefault<Setting>();

                if (assemblySetting != null && classSetting != null)
                {

                    assembly = Assembly.Load(assemblySetting.Value);

                    if (assembly != null)
                    {
                        Type type = assembly.GetType(classSetting.Value, true, true);

                        if (type != null)
                        {
                            MethodInfo method = type.GetMethod(commandText);

                            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                            object instance = constructor.Invoke(null);

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
                                    value = screenParam.Value;
                                    signature[index] = value;
                                }

                                index++;
                            }

                            try
                            {
                                retVal = method.Invoke(instance, signature);

                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    Common.LogException(ex, false);
                                    throw ex.InnerException;
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
