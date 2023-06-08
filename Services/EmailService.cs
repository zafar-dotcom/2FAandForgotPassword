using IPMO.IServices;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;
using IPMO.Models.EmailService;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System.Net;

namespace IPMO.Services
{
    public class EmailService :IEmailService
    {

        private readonly EmailConfiguration _emailConfig;
        public EmailService(EmailConfiguration emailConfig) => _emailConfig = emailConfig;
        public bool SendEmail(Message message)
        {
            try
            {
                var emailMessage = CreateEmailMessage(message);
                Send(emailMessage);
                return true;
            }
            catch (Exception)
            {
                throw;
                return false;
            }
        }

     



        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                client.Send(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception or both.
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

      
    }
}
