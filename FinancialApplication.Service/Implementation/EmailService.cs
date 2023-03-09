namespace FinancialApplication.Service.Implementation;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Send email using SendGrid
    /// </summary>
    /// <param name="toEmail"></param>
    /// <param name="subject"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public Task<bool> SendEmailAsync(string toEmail, string subject, string message)
    {
        string apiKey = _config["SendGrid:ApiSecret"];
        string fromEmail = _config["SendGrid:FromEmail"];
        string displayName = _config["SendGrid:DisplayName"];
        return Execute(apiKey, subject, message, toEmail, fromEmail, displayName);
    }

    /// <summary>
    /// Send email using SendGrid
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="subject"></param>
    /// <param name="message"></param>
    /// <param name="email"></param>
    /// <param name="fromEmail"></param>
    /// <param name="displayName"></param>
    /// <returns></returns>
    public static async Task<bool> Execute(string apiKey, string subject, string message, string email, string fromEmail, string displayName)
    {
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress(fromEmail, displayName),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(email));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);

        var response = await client.SendEmailAsync(msg);
        return response.StatusCode == System.Net.HttpStatusCode.Accepted;

    }
}
