using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core.Design
{
    public class DataCommandCommandTypeTypeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            //this means a standard list of values are supported
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            string DataConnectionName = null;

            StandardValuesCollection list = null;

            if (context.Instance is DataCommand)
            {
                DataCommand command = (DataCommand)context.Instance;

                if(String.IsNullOrEmpty(command.DataConnection))
                {
                    DataConnectionName = Configuration.GetInstance().App.DefaultConnection;
                }
                else
                {
                    DataConnectionName = command.DataConnection;
                }

                if(!String.IsNullOrEmpty(DataConnectionName))
                {
                    DataConnection connection = Configuration.GetInstance().DataConnections.Where(c => c.Name.ToLower() == DataConnectionName.ToLower()).SingleOrDefault();

                    if(connection != null)
                    {
                        DataConnectionType connectionType  = Configuration.GetInstance().DataConnectionTypes.Where(c => c.Name.ToLower() == connection.DataConnectionType.ToLower()).SingleOrDefault();
                        
                        if(connectionType != null)
                        {
                            

                            list = new StandardValuesCollection(connectionType.CommandTypes);
                            
                           
                        }
                    }
                }
 
            }

            return list;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            
            return false;
        }
    }
}
