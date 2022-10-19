using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public EmailSettings _emailSettings { get; }
        public ILogger<EmailService> _logger { get; }

        public async Task<bool> SendEmail(Application.Models.Email email)
        {
            var client = new SendGridClient(_emailSettings.ApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_emailSettings.FromAddress, _emailSettings.FromName),
                Subject = email.Subject,
                PlainTextContent = email.Body
            };
            msg.AddTo(new EmailAddress(email.To, email.FullName));
            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"There was an error sending an email to: {email.To}. Reason: {response.StatusCode.ToString()}");
            }

            return response.IsSuccessStatusCode;
        }
    }
}