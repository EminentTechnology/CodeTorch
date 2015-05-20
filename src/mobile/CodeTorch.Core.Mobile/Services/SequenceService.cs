using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Data.Common;
using CodeTorch.Core;

namespace CodeTorch.Core.Services
{
    public class SequenceService 
    {
        public void Save(Sequence sequence)
        {
            int retVal;

            DataCommandService dataCommandDB = DataCommandService.GetInstance();

            

            
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();

            

            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter();
            p.Name = "@SequenceName";
            p.Value = sequence.Name;
            parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@SequencePrefix";
            p.Value = sequence.Prefix;
            parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@SeedValue";
            p.Value = sequence.SeedValue;
            parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@Increment";
            p.Value = sequence.Increment;
            parameters.Add(p);

            dataCommandDB.ExecuteDataCommand(Configuration.GetInstance().App.SaveSequenceDataCommand, parameters);


            


        }

        
    }
}
