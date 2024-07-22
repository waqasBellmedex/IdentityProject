
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
        private readonly SmtpSettings _smtpSettings;
        public EmailSender(IOptions<SmtpSettings> options)
        {
            _smtpSettings = options.Value;
        }
        public async Task SendCustomerCredentialsEmail(string toEmail, string confirmationLink)
        {
            var emailOptions = new EmailOptions
            {
                Subject = "Confirm your email",
                Body = $"Please confirm your account by clicking <a href='{confirmationLink}'>here</a>.",
                ToEmail = toEmail
            };

            try
            {
                await SendMailAsync(emailOptions);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        private async Task SendMailAsync(EmailOptions emailOptions)
        {
            var mail = new MailMessage
            {
                Subject = emailOptions.Subject,
                Body = emailOptions.Body,
                From = new MailAddress(_smtpSettings.SenderAddress),
                IsBodyHtml = _smtpSettings.IsBodyHtml,
                BodyEncoding = Encoding.Default
            };

            mail.To.Add(emailOptions.ToEmail);

            var networkCredential = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);

            var smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Host,
                Port = _smtpSettings.Port,
                EnableSsl = _smtpSettings.EnableSsl,
                UseDefaultCredentials = _smtpSettings.UseDefaultCredentials,
                Credentials = networkCredential
            };

            try
            {
                await smtpClient.SendMailAsync(mail);
                Console.WriteLine("Email sent.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

    }
}
