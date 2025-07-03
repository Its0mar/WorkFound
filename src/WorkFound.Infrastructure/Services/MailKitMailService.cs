using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Common.Settings;

namespace WorkFound.Infrastructure.Services;

public class MailKitMailService : IMailService
{
    private readonly EmailSettings _settings;

    public MailKitMailService(IOptions<EmailSettings> emailSettings)
    {
        _settings = emailSettings.Value;
    }
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = body };
        message.Body = bodyBuilder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_settings.Host, int.Parse(_settings.Port), SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}