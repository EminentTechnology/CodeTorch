using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data.Common;
using CodeTorch.Core.Services;

namespace CodeTorch.Core
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SMSMessage
    {
        public string PhoneNo { get; set; }
        public string Message { get; set; }
        public string Template { get; set; }
        public string EntityType { get; set; }
        public string EntityID { get; set; }

        public string SMSUserName { get; set; }
        public string SMSPassword { get; set; }
        public string SMSChannelID { get; set; }


       

       

        public void Send( string UserName)
        {
            

            SmsService smsDB = new SmsService();
            DateMode dateMode = Configuration.GetInstance().App.DateMode;

            string messageText = this.Message;
            while (messageText.Length > 0)
            {
                string messagePart;

                if (messageText.Length > 160)
                {
                    messagePart = messageText.Substring(0, 160);
                    messageText = messageText.Substring(160);
                }
                else
                {
                    messagePart = messageText;
                    messageText = String.Empty;
                }

                string MessageID = smsDB.InsertMessage(dateMode, this.PhoneNo, messagePart, this.Template, this.EntityType, this.EntityID, this.SMSUserName, this.SMSPassword, this.SMSChannelID, UserName);

            }

            

        }
    }
}
