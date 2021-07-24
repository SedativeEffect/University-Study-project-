using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using module_10.BLL.Exceptions;
using module_10.BLL.Models;
using module_10.BLL.Utils;
using module_10.DL.Interfaces;

namespace module_10.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly MailSettings _mailSettings;

        public EmailService(ILogger<EmailService> logger, IOptions<MailSettings> mailOptions)
        {
            _logger = logger;
            _mailSettings = mailOptions.Value;
        }
        public async Task NotifyByEmail(string email, string subject, string message)
        {
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            msg.To.Add(new MailboxAddress(email, email));
            msg.Subject = subject;
            msg.Body = new TextPart("plain")
            {
                Text = message
            };
            using var mailClient = new SmtpClient();
            try
            {
                await mailClient.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await mailClient.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                await mailClient.SendAsync(msg);
            }
            catch (Exception ex)
            {
                _logger.LogAndThrow(new SendMailException(ex.Message), LogLevel.Error);
            }
            finally
            {
                await mailClient.DisconnectAsync(true);
            }

        }

    }
}
