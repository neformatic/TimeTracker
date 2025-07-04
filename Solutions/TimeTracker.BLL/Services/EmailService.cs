using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using TimeTracker.BLL.Models.Email;
using TimeTracker.BLL.Services.Interfaces;
using TimeTracker.Common.Models.Settings;

namespace TimeTracker.BLL.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendAsync(string email, string subject, string message)
    {
        await SendAsync([email], subject, message);
    }

    public async Task SendAsync(List<string> emails, string subject, string message)
    {
        using var client = new SmtpClient();

        if (_emailSettings.UseSsl)
        {
            await client.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort);
        }
        else
        {
            await client.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.None);
        }

        if (!string.IsNullOrEmpty(_emailSettings.Password))
        {
            await client.AuthenticateAsync(_emailSettings.Address, _emailSettings.Password);
        }

        var emailDetails = new EmailDetailsModel
        {
            Message = message,
            Recipients = emails,
            SenderEmail = _emailSettings.Address,
            SenderName = _emailSettings.SenderName,
            Subject = subject
        };

        var emailMessage = CreateEmail(emailDetails);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }

    private MimeMessage CreateEmail(EmailDetailsModel emailDetails)
    {
        var emailMessage = new MimeMessage();
        var toAddresses = emailDetails.Recipients.Select(r => new MailboxAddress(string.Empty, r)).ToList();

        emailMessage.From.Add(new MailboxAddress(emailDetails.SenderName, emailDetails.SenderEmail));
        emailMessage.To.AddRange(toAddresses);
        emailMessage.Subject = emailDetails.Subject;

        var builder = new BodyBuilder
        {
            HtmlBody = emailDetails.Message
        };

        emailMessage.Body = builder.ToMessageBody();

        return emailMessage;
    }
}