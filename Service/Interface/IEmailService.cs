using Domain.DomainModels;

namespace Service.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(List<EmailMessage> emailMessages);
    }
}