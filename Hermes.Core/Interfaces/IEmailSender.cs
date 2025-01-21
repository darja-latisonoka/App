using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

public class IEmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
{
    private readonly IConfiguration _configuration;

    public IEmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public IEmailSender() {}

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SendGridClient(_configuration["SendGrid:ApiKey"]);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("DuckChat.no.reply@gmail.com", "DuckChat"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(email));

        await client.SendEmailAsync(msg);
    }
}
