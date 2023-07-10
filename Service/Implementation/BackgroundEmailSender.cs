
using ClassLibrary1.Interface;
using Domain.DomainModels;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation
{
    public class BackgroundEmailSender : IBackgroundEmailSender
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<EmailMessage> _emailRepository;

        public BackgroundEmailSender(IEmailService emailService, IRepository<EmailMessage> emailRepository)
        {
            _emailService = emailService;
            _emailRepository = emailRepository;
        }

        public async Task DoWork()
        {
            await _emailService.SendEmailAsync(_emailRepository.GetAll().Where(z => !z.Status).ToList());
        }
    }
}