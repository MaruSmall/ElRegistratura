using System.Threading.Tasks;

namespace ElRegistratura.Email
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
