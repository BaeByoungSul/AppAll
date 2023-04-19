
using Models.Email;

namespace Services.EmailService;

public interface IEmailService
{
    void SendEmail(EmailDto request);
    void SendDbEmail(EmailDto request);
}
