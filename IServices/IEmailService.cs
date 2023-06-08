using IPMO.Models.EmailService;

namespace IPMO.IServices
{
    public interface IEmailService
    {
        bool SendEmail(Message message);

        // This method is for forgot password 
       
    }
}
