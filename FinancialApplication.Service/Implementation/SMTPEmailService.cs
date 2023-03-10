using System.Net.Mail;
using System.Net;

public class SMTPDetails
{
    public string SMTPHost { get; set; }

    public int SMTPPort { get; set; }

    public bool EnableSSL { get; set; }

    public bool UseDefaultCredentials { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }
    public string FromEmail { get; set; }
    public string FromDisplayName { get; set; }

}
public class SMTPMailService : IEmailService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initiate Sendgrid email notifications
    /// </summary>
    public SMTPMailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    /// <summary>
    /// Send mail via SMTP to a single recipient
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="toEmail"></param>
    /// <param name="body"></param>
    /// <param name="attachment"></param>
    /// <param name="cc"></param>
    /// <param name="bcc"></param>
    public async Task<bool> SendEmailAsync(string toEmail, string subject, string message)
    {
        var mailMessage = new MailMessage()
        {
            From = new MailAddress(SMPTPInforFromConfig.FromEmail, SMPTPInforFromConfig.FromDisplayName),
            Subject = subject,
            IsBodyHtml = true,
            Body = message
        };
        mailMessage.To.Add(new MailAddress(toEmail));
        var smtpClient = new SmtpClient()
        {
            Port = SMPTPInforFromConfig.SMTPPort,
            Host = SMPTPInforFromConfig.SMTPHost,
            EnableSsl = SMPTPInforFromConfig.EnableSSL,
            UseDefaultCredentials = SMPTPInforFromConfig.UseDefaultCredentials,
        };

        if (SMPTPInforFromConfig.UseDefaultCredentials == false)
        {
            smtpClient.Credentials = new NetworkCredential(SMPTPInforFromConfig.Username, SMPTPInforFromConfig.Password);
        }
        await smtpClient.SendMailAsync(mailMessage);
        return true;
    }

    private SMTPDetails SMPTPInforFromConfig
    {
        get
        {
            _ = int.TryParse(_configuration["SMTP:Port"], out int smtpPort);
            _ = bool.TryParse(_configuration["SMTP:EnableSSL"], out bool enableSSL);
            SMTPDetails smtpDetails = new()
            {
                SMTPHost = _configuration["SMTP:Host"],
                UseDefaultCredentials = false,
                SMTPPort = smtpPort,
                EnableSSL = enableSSL,
                Username = _configuration["SMTP:username"],
                Password = _configuration["SMTP:password"],
                FromDisplayName = _configuration["SMTP:DisplayName"],
                FromEmail = _configuration["SMTP:From"]
            };
            return smtpDetails;
        }
    }
}