
using Domain.Interface;
using Domain.Model;
using Domain.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;


namespace EmailConfiguration
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings smtpSettings;
        public EmailSender(IOptions<SmtpSettings> options)
        {
            smtpSettings = options.Value;
        }
        public async Task SendCustomerCredentialsEmail(string toEmail,string confirmationLink)
        {
            var fromAddress = new MailAddress(smtpSettings.SenderAddress, "Bad Wolf");
            var toAddress = new MailAddress(toEmail);
            var smtp = new SmtpClient
            {
                Host = smtpSettings.Host,
                Port = smtpSettings.Port,
                EnableSsl = smtpSettings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = smtpSettings.UseDefaultCredentials,
                Credentials = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password)
            };
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = "Confirm your email",
                Body = $"Please confirm your account by clicking <a href='{confirmationLink}'>here</a>.",
                IsBodyHtml = smtpSettings.IsBodyHtml
            };

            await smtp.SendMailAsync(message);
        }
        private async Task SendMailAsync(EmailOptions emailOptions)
        {
            var mail = new MailMessage
            {
                Subject = emailOptions.Subject,
                Body = emailOptions.Body,
                From = new MailAddress(smtpSettings.SenderAddress),
                IsBodyHtml = smtpSettings.IsBodyHtml,

            };
            foreach (var toEmail in emailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }
            var networkCredential = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password);

            var smtpClient = new SmtpClient
            {
                Host = smtpSettings.Host,
                Port = smtpSettings.Port,
                EnableSsl = smtpSettings.EnableSsl,
                UseDefaultCredentials = smtpSettings.UseDefaultCredentials,
                Credentials = networkCredential
            };
            mail.BodyEncoding = Encoding.Default;
            await smtpClient.SendMailAsync(mail);
        }
    }
}
