using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.ComponentModel;
using CodeTorch.Core.Services;
using CodeTorch.Core.Interfaces;

namespace CodeTorch.Core
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class EmailMessage
    {
        public string ID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public MailPriority Priority { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Template { get; set; }
        public string EntityType { get; set; }
        public string EntityID { get; set; }

        [TypeConverter("CodeTorch.Core.Design.EmailConnectionTypeConverter,CodeTorch.Core.Design")]
        public string EmailConnection { get; set; }
        

        EmailAddress _From = new EmailAddress();
        List<EmailAddress> _To = new List<EmailAddress>();
        List<EmailAddress> _ReplyTo = new List<EmailAddress>();
        List<EmailAddress> _CC = new List<EmailAddress>();
        List<EmailAddress> _BCC = new List<EmailAddress>();
        List<EmailAttachment> _Attachments = new List<EmailAttachment>();

        public EmailAddress From
        {
            get { return _From; }
            set { _From = value; }
        }

        public List<EmailAddress> ReplyTo
        {
            get { return _ReplyTo; }
            set { _ReplyTo = value; }
        }


        public List<EmailAddress> To
        {
            get { return _To; }
            set { _To = value; }
        }

        public List<EmailAddress> CC
        {
            get { return _CC; }
            set { _CC = value; }
        }

        public List<EmailAddress> BCC
        {
            get { return _BCC    ; }
            set { _BCC = value; }
        }

        public List<EmailAttachment> Attachments
        {
            get { return _Attachments; }
            set { _Attachments = value; }
        }

       

        public void Send()
        {
            IEmailProvider email = EmailService.GetInstance().GetProvider(null);
            email.Send(this);
  
        }

        public EmailConnection GetConnection()
        {
            EmailConnection retVal = null;

            if (!String.IsNullOrEmpty(this.EmailConnection))
            {
                retVal = CodeTorch.Core.EmailConnection.GetByName(this.EmailConnection);
            }

            return retVal;
        }
        
    }
}
