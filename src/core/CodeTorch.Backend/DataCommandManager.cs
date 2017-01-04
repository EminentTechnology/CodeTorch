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

    public class DataCommandManager
    {
        readonly IConfigurationStore Store;
        readonly ILog Log;

        public DataCommandManager(IConfigurationStore store, ILogManager logger)
        {
            Store = store;
            Log = logger.GetLogger(typeof(DataCommandManager));
        }

        public async Task<string> Add
        (
            string name, string connectionName, string type, string text, string returnType,
            string preProcessingAssembly, string preProcessingClass,
            string postProcessingAssembly, string postProcessingClass
        )
        {
            DataCommand retVal = null;
            DataCommandReturnType dataCommandReturnType = DataCommandReturnType.Integer;

            //check for existence
            if (await Store.Exists<DataCommand>(name))
            {
                throw new Exception(String.Format("Data Command {0} already exists", name));
            }

            retVal = new DataCommand();

            DataConnection connection = await Store.GetItem<DataConnection>(connectionName);
            if(connection == null)
            {
                throw new Exception(String.Format("Data Connection {0} does not exist", connectionName));
            }

            retVal.DataConnection = connectionName;
            retVal.Name = name;
            retVal.Type = type;
            retVal.Text = text;

            Enum.TryParse<DataCommandReturnType>(returnType, out dataCommandReturnType);
            retVal.ReturnType = dataCommandReturnType;

            retVal.PreProcessingAssembly = preProcessingAssembly;
            retVal.PreProcessingClass = preProcessingClass;

            retVal.PostProcessingAssembly = postProcessingAssembly;
            retVal.PostProcessingClass = postProcessingClass;

            retVal = await Store.Add<DataCommand>(name, retVal);

            return retVal.Name;
        }

        public async Task Update
        (
            string name, string connectionName, string type, string text, string returnType,
            string preProcessingAssembly, string preProcessingClass,
            string postProcessingAssembly, string postProcessingClass
        )
        {
            DataCommand retVal = null;
            DataCommandReturnType dataCommandReturnType = DataCommandReturnType.Integer;

            retVal = await Store.GetItem<DataCommand>(name); 

            //check for existence
            if (retVal == null)
            {
                throw new Exception(String.Format("Data Command {0} does not exist in configuration", name));
            }

            DataConnection connection = await Store.GetItem<DataConnection>(connectionName);
            if (connection == null)
            {
                throw new Exception(String.Format("Data Connection {0} does not exist", connectionName));
            }

            retVal.DataConnection = connectionName;
            retVal.Type = type;
            retVal.Text = text;

            Enum.TryParse<DataCommandReturnType>(returnType, out dataCommandReturnType);
            retVal.ReturnType = dataCommandReturnType;

            retVal.PreProcessingAssembly = preProcessingAssembly;
            retVal.PreProcessingClass = preProcessingClass;

            retVal.PostProcessingAssembly = postProcessingAssembly;
            retVal.PostProcessingClass = postProcessingClass;

            retVal = await Store.Update<DataCommand>(name, retVal);

            
        }

        public async Task<List<LookupItem>> GetReturnTypes()
        {
            List<LookupItem> retVal = new List<LookupItem>();

            Type t = typeof(DataCommandReturnType);
            var items = Enum.GetValues(t);

            foreach (int item in items)
            {
                var description = Enum.GetName(t, item);
                retVal.Add(new LookupItem { Value = item.ToString(), Sort = description, Description = description });
            };

            
            return await Task.FromResult(retVal);
        }

        public async Task<List<LookupItem>> GetParameterTypes()
        {
            List<LookupItem> retVal = new List<LookupItem>();

            Type t = typeof(DataCommandParameterType);
            var items = Enum.GetValues(t);
           
            foreach (int item in items)
            {
                var description = Enum.GetName(t, item);
                retVal.Add(new LookupItem { Value = item.ToString(), Sort = description, Description = description });
            };

            return await Task.FromResult(retVal);
        }

        public async Task<List<LookupItem>> GetParameterDirections()
        {
            List<LookupItem> retVal = new List<LookupItem>();

            Type t = typeof(DataCommandParameterDirection);
            var items = Enum.GetValues(t);

            foreach (int item in items)
            {
                var description = Enum.GetName(t, item);
                retVal.Add(new LookupItem { Value = item.ToString(), Sort = description, Description = description });
            };

            return await Task.FromResult(retVal);
        }


        public async Task<List<LookupItem>> GetDataCommandTypes(string connectionName)
        {

            List<LookupItem> retVal = new List<LookupItem>();

            DataConnection connection = await Store.GetItem<DataConnection>(connectionName);

            if (connection != null)
            {

                DataConnectionType connectionType = connection.GetConnectionType();

                if (connectionType != null)
                {
                    foreach (var item in connectionType.CommandTypes)
                    {
                        retVal.Add(new LookupItem { Value = item, Sort = item, Description = item });
                    };
                }
            }
            
            return await Task.FromResult(retVal);
        }

        public async Task<List<DataConnection>> GetConnections()
        {
            List<DataConnection> retVal = null;

            retVal = await Store.GetItems<DataConnection>();
            
            return retVal;
        }

        public async Task<List<DataCommand>> Search(string name)
        {
            List<DataCommand> retVal = null;

            retVal = await Store.GetItems<DataCommand>();

            //filter by name
            if (!String.IsNullOrEmpty(name))
            {
                retVal = retVal
                    .Where(c => c.Name.ToLower().Contains(name.ToLower()))
                    .ToList<DataCommand>();
            }

            //order items by name
            retVal = retVal
                .OrderBy(c => c.Name)
                .ToList<DataCommand>();

            return retVal;
        }

        public async Task<List<DataCommand>> SearchDataTableReturningCommands(string name)
        {
            List<DataCommand> retVal = null;

            retVal = await Store.GetItems<DataCommand>();

            //filter by name
            if (!String.IsNullOrEmpty(name))
            {
                retVal = retVal
                    .Where(c => c.Name.ToLower().Contains(name.ToLower()))
                    .ToList<DataCommand>();
            }

            //order items by name
            retVal = retVal
                .Where(c=>c.ReturnType == DataCommandReturnType.DataTable)
                .OrderBy(c => c.Name)
                .ToList<DataCommand>();

            return retVal;
        }

        public async Task<List<DataCommand>> SearchIntegerReturningCommands(string name)
        {
            List<DataCommand> retVal = null;

            retVal = await Store.GetItems<DataCommand>();

            //filter by name
            if (!String.IsNullOrEmpty(name))
            {
                retVal = retVal
                    .Where(c => c.Name.ToLower().Contains(name.ToLower()))
                    .ToList<DataCommand>();
            }

            //order items by name
            retVal = retVal
                .Where(c => c.ReturnType == DataCommandReturnType.Integer)
                .OrderBy(c => c.Name)
                .ToList<DataCommand>();

            return retVal;
        }

        public async Task<List<DataCommand>> SearchXmlReturningCommands(string name)
        {
            List<DataCommand> retVal = null;

            retVal = await Store.GetItems<DataCommand>();

            //filter by name
            if (!String.IsNullOrEmpty(name))
            {
                retVal = retVal
                    .Where(c => c.Name.ToLower().Contains(name.ToLower()))
                    .ToList<DataCommand>();
            }

            //order items by name
            retVal = retVal
                .Where(c => c.ReturnType == DataCommandReturnType.Xml)
                .OrderBy(c => c.Name)
                .ToList<DataCommand>();

            return retVal;
        }

        public async Task<dynamic> GetByName(string name)
        {
            dynamic retVal = null;

            retVal = await Store.GetItem<DataCommand>(name);

            if (retVal != null)
            {
                retVal = new
                {
                    Name = retVal.Name,
                    DataConnection = retVal.DataConnection,
                    PostProcessingAssembly = retVal.PostProcessingAssembly,
                    PostProcessingClass = retVal.PostProcessingClass,
                    PreProcessingClass = retVal.PreProcessingClass,
                    PreProcessingAssembly = retVal.PreProcessingAssembly,
                    ReturnType = Enum.GetName(typeof(DataCommandReturnType), retVal.ReturnType),
                    Text = retVal.Text,
                    Type = retVal.Type.ToString(),
                };
            }

            return retVal;
        }


        public async Task<List<dynamic>> GetParameters(string Command)
        {
            List<dynamic> retVal = new List<dynamic>();

            var command = await Store.GetItem<DataCommand>(Command);

            if (command != null)
            {
                retVal = command.Parameters.Select(p=> new
                {
                    Name = p.Name,

                    Type = p.Type,
                    TypeDisplay = p.Type.ToString(),

                    Size = p.Size,

                    Direction = p.Direction,
                    DirectionName = p.Direction.ToString(),

                    TypeName = p.TypeName,

                    IsUserDefinedType = p.IsUserDefinedType,
                    IsTableType = p.IsTableType
                })
                .ToList<dynamic>();
            }

            return retVal;
        }

        public async Task<DataCommandParameter> GetParameterByName(string Command, string Name)
        {
            DataCommandParameter retVal = null;

            var command = await Store.GetItem<DataCommand>(Command);

            if (command != null)
            {
                retVal = command.Parameters.SingleOrDefault(p=>p.Name.ToLower()==Name);
            }

            return retVal;
        }

        public async Task AddParameter(string Command, string Name, string Type, int Size, string Direction, string TypeName, bool IsUserDefinedType, bool IsTableType)
        {
            DataCommandParameter retVal = null;

            var command = await Store.GetItem<DataCommand>(Command);

            if (command != null)
            {
                retVal = command.Parameters.SingleOrDefault(p => p.Name.ToLower() == Name.ToLower());

                if (retVal != null)
                {
                    throw new Exception(String.Format("Data Command Parameter {0} already exists", Name));
                }

                retVal = new DataCommandParameter();

                retVal.Name = Name;

                DataCommandParameterType parameterType = DataCommandParameterType.String;
                Enum.TryParse<DataCommandParameterType>(Type, out parameterType);
                retVal.Type = parameterType;

                retVal.Size = Size;

                DataCommandParameterDirection parameterDirection = DataCommandParameterDirection.In;
                Enum.TryParse<DataCommandParameterDirection>(Direction, out parameterDirection);
                retVal.Direction = parameterDirection;

                retVal.TypeName = TypeName;
                retVal.IsUserDefinedType = IsUserDefinedType;
                retVal.IsTableType = IsTableType;

                command.Parameters.Add(retVal);

                await Store.Save<DataCommand>(Command, command);
            }
            else
            {
                throw new Exception(String.Format("Data Command {0} does not exist in configuration", Command));
            }

            
        }

        public async Task UpdateParameter(string Command, string Name, string Type, int Size, string Direction, string TypeName, bool IsUserDefinedType, bool IsTableType)
        {
            DataCommandParameter retVal = null;

            var command = await Store.GetItem<DataCommand>(Command);

            if (command != null)
            {
                retVal = command.Parameters.SingleOrDefault(p => p.Name.ToLower() == Name);

                if (retVal == null)
                {
                    throw new Exception(String.Format("Data Command Parameter {0} does not exist", Name));
                }

                retVal.Name = Name;

                DataCommandParameterType parameterType = DataCommandParameterType.String;
                Enum.TryParse<DataCommandParameterType>(Type, out parameterType);
                retVal.Type = parameterType;

                retVal.Size = Size;

                DataCommandParameterDirection parameterDirection = DataCommandParameterDirection.In;
                Enum.TryParse<DataCommandParameterDirection>(Direction, out parameterDirection);
                retVal.Direction = parameterDirection;

                retVal.TypeName = TypeName;
                retVal.IsUserDefinedType = IsUserDefinedType;
                retVal.IsTableType = IsTableType;


                await Store.Save<DataCommand>(Command, command);
            }
            else
            {
                throw new Exception(String.Format("Data Command {0} does not exist in configuration", Command));
            }


        }

        public async Task DeleteParameter(string Command, string Name)
        {
            DataCommandParameter retVal = null;

            var command = await Store.GetItem<DataCommand>(Command);

            if (command != null)
            {
                retVal = command.Parameters.SingleOrDefault(p => p.Name.ToLower() == Name.ToLower());

                if (retVal == null)
                {
                    throw new Exception(String.Format("Data Command Parameter {0} does not exist", Name));
                }

                command.Parameters.Remove(retVal);


                await Store.Save<DataCommand>(Command, command);
            }
            else
            {
                throw new Exception(String.Format("Data Command {0} does not exist in configuration", Command));
            }


        }


        public async Task<List<DataCommandColumn>> GetColumns(string Command)
        {
            List<DataCommandColumn> retVal = new List<DataCommandColumn>();

            var command = await Store.GetItem<DataCommand>(Command);

            if (command != null)
            {
                retVal = command.Columns;
            }

            return retVal;
        }

       

        public async Task AddColumn(string Command, string Name, string Type)
        {
            DataCommandColumn retVal = null;

            var command = await Store.GetItem<DataCommand>(Command);

            if (command != null)
            {
                retVal = command.Columns.SingleOrDefault(p => p.Name.ToLower() == Name.ToLower());

                if (retVal != null)
                {
                    throw new Exception(String.Format("Data Command Column {0} already exists", Name));
                }

                retVal = new DataCommandColumn();

                retVal.Name = Name;
                retVal.Type = Type;
                
                command.Columns.Add(retVal);

                await Store.Save(Command, command);
            }
            else
            {
                throw new Exception(String.Format("Data Command {0} does not exist in configuration", Command));
            }


        }

        public async Task UpdateColumn(string Command, string Name, string Type)
        {
            DataCommandColumn retVal = null;

            var command = await Store.GetItem<DataCommand>(Command);

            if (command != null)
            {
                retVal = command.Columns.SingleOrDefault(p => p.Name.ToLower() == Name.ToLower());

                if (retVal == null)
                {
                    throw new Exception(String.Format("Data Command Column {0} does not exist", Name));
                }

                
                retVal.Type = Type;

                await Store.Save(Command, command);
            }
            else
            {
                throw new Exception(String.Format("Data Command {0} does not exist in configuration", Command));
            }


        }

        public async Task DeleteColumn(string Command, string Name)
        {
            DataCommandColumn retVal = null;

            var command = await Store.GetItem<DataCommand>(Command);

            if (command != null)
            {
                retVal = command.Columns.SingleOrDefault(p => p.Name.ToLower() == Name.ToLower());

                if (retVal == null)
                {
                    throw new Exception(String.Format("Data Command Column {0} does not exist", Name));
                }

                command.Columns.Remove(retVal);


                await Store.Save(Command, command);
            }
            else
            {
                throw new Exception(String.Format("Data Command {0} does not exist in configuration", Command));
            }


        }

        public async Task RefreshSchema(string Command)
        {
            DataCommandColumn retVal = null;

            var command = await Store.GetItem<DataCommand>(Command);

            if (command != null)
            {
                try
                {
                    DataConnection connection = command.GetDataConnection();
                    IDataCommandProvider DataSource = DataCommandService.GetInstance().GetProvider(connection);
                    DataSource.RefreshSchema(connection, command);

                    await Store.Save(Command, command);
                }
                catch (Exception ex)
                {
                    Log.Error("Error refreshing schema", ex);
                }

            }
            else
            {
                throw new Exception(String.Format("Data Command {0} does not exist in configuration", Command));
            }


        }

        public async Task Delete(string Name)
        {
            var item = await Store.GetItem<DataCommand>(Name);

            if (item != null)
            {
                await Store.Delete<DataCommand>(Name);
            }
            else
            {
                throw new Exception(String.Format("Data Command {0} does not exist in configuration", Name));
            }


        }
    }

}
