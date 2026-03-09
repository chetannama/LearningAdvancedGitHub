using EmailSpamDetectionService.Model;

namespace EmailSpamDetectionService.Services.Interfaces
{
    public interface IEmailSpamService
    {
        public Task<bool> SendEmailAsync(EmailRequest email);
    }
}
