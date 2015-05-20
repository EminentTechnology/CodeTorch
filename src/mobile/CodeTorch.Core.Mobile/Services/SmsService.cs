using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

using System.Data;
using CodeTorch.Core;

namespace CodeTorch.Core.Services
{
    public class SmsService
    {
        public string InsertMessage( 
            DateMode DateMode, string PhoneNo, string MessageText, string Template, 
            string EntityType, string EntityID, string UserName, string Password, 
            string ChannelID, string CreatedBy)
        {

            string MessageID = Guid.NewGuid().ToString();

            DataCommandService dataCommandDB = DataCommandService.GetInstance(); ;

            


            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();



            ScreenDataCommandParameter p = null;

            
            p = new ScreenDataCommandParameter();
            p.Name = "@MessageID";
            p.Value = MessageID;
            parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@PhoneNo";
            p.Value = PhoneNo;
            parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@MessageText";
            p.Value = MessageText;
            parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@Template";
            p.Value = Template;
            parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@EntityType";
            p.Value = EntityType;
            parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@EntityID";
            p.Value = EntityID;
            parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@UserName";
            p.Value = UserName;
            parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@Password";
            p.Value = Password;
            parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@ChannelID";
            p.Value = ChannelID;
            parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@CreatedOn";
            switch (DateMode)
            {
                case DateMode.LocalDate:
                    p.Value = DateTime.Now;
                    break;
                case DateMode.UniversalDate:
                    p.Value = DateTime.UtcNow;
                    break;
            }
            
            parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@CreatedBy";
            p.Value = CreatedBy;
            parameters.Add(p);

            dataCommandDB.ExecuteDataCommand(Configuration.GetInstance().App.SaveSequenceDataCommand, parameters);



            return MessageID;


        }
    }
}
