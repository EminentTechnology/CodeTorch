using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
using CodeTorch.Core.Services;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace CodeTorch.Email
{
    public class DataCommandEmailProvider: IEmailProvider
    {
        DataCommandService sql = DataCommandService.GetInstance();

        private const string DataCommandWMailInsertMailMessage = "Mail_InsertMailMessage";
        private const string DataCommandMailInsertAttachment = "Mail_InsertAttachment";
        private const string DataCommandMailInsertAddress = "Mail_InsertAddress";

        private const string ParameterMailMessageID = "@MailMessageID";
        private const string ParameterMailSubject = "@MailSubject";
        private const string ParameterMailBody = "@MailBody";
        private const string ParameterPriority = "@Priority";
        private const string ParameterIsBodyHtml = "@IsBodyHtml";
        private const string ParameterTemplate = "@Template";
        private const string ParameterEntityType = "@EntityType";
        private const string ParameterEntityID = "@EntityID";
        private const string ParameterCreatedBy = "@CreatedBy";

        private const string ParameterMailAddressID = "@MailAddressID";
        private const string ParameterAddressType = "@AddressType";
        private const string ParameterEmailAddress = "@EmailAddress";
        private const string ParameterDisplayName = "@DisplayName";

        private const string ParameterMailMessageAttachmentID = "@MailMessageAttachmentID";
        private const string ParameterDocumentID = "@DocumentID";
        

        public void Initialize(List<CodeTorch.Core.Setting> settings)
        {
            
        }

        public void Send(EmailMessage message)
        {
            using (TransactionScope rootScope = TransactionUtils.CreateTransactionScope())
            {
                message.ID = Guid.NewGuid().ToString();

                InsertMailMessage(message);
                InsertAddress(message, message.From, "FROM");

                foreach (EmailAddress address in message.To)
                {
                    InsertAddress(message, address, "TO");
                }

                foreach (EmailAddress address in message.To)
                {
                    InsertAddress(message, address, "CC");
                }

                foreach (EmailAddress address in message.To)
                {
                    InsertAddress(message, address, "BCC");
                }

                foreach (EmailAddress address in message.ReplyTo)
                {
                    InsertAddress(message, address, "REPLYTO");
                }

                foreach (EmailAttachment attachment in message.Attachments)
                {
                    InsertAttachment(message, attachment);
                }

                rootScope.Complete();
            }
        }

        public void InsertMailMessage(EmailMessage message)
        {
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterMailMessageID, message.ID);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterMailSubject, message.Subject);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterMailBody, message.Body);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterPriority, message.Priority);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterIsBodyHtml, message.IsBodyHtml);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterTemplate, message.Template);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterEntityType, message.EntityType);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterEntityID, message.EntityID);
            parameters.Add(p);

            string userName;
            userName = UserIdentityService.GetInstance().IdentityProvider.GetUserName();
            p = new ScreenDataCommandParameter(ParameterCreatedBy, userName);
            parameters.Add(p);

            //get data from data command
            sql.ExecuteDataCommand(DataCommandWMailInsertMailMessage, parameters);
        }

        private void InsertAttachment(EmailMessage message, EmailAttachment attachment)
        {
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterMailMessageAttachmentID, attachment.ID);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterMailMessageID, message.ID);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterDocumentID, attachment.DocumentID);
            parameters.Add(p);

          


            //get data from data command
            sql.ExecuteDataCommand(DataCommandMailInsertAttachment, parameters);
        }

        private void InsertAddress(EmailMessage message, EmailAddress address, string AddressType)
        {
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterMailAddressID, address.ID);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterMailMessageID, message.ID);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterAddressType, AddressType);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterEmailAddress, address.Address);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterDisplayName, address.DisplayName);
            parameters.Add(p);

            //get data from data command
            sql.ExecuteDataCommand(DataCommandMailInsertAddress, parameters);
        }
    }
}
