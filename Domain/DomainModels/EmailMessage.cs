using System.ComponentModel;

namespace Domain.DomainModels
{
    public class EmailMessage : BaseEntity
    {
        public EmailMessage(string? mailTo, string subject, string content, bool status)
        {
            MailTo = mailTo;
            Subject = subject;
            Content = content;
            Status = status;
        }

 
        public string? MailTo { get; set; }
        
     
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
    }
}