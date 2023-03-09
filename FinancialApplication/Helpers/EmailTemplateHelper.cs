namespace FinancialApplication.Helpers;

public class EmailTemplateHelper : IEmailTemplateHelper
{
    IWebHostEnvironment _env;

    public EmailTemplateHelper(IWebHostEnvironment env)
    {
        _env = env;
    }

    public string BuildEmailConfirmationTemplate(string firstName, string confirmationCode)
    {
        var rootPath = _env.ContentRootPath;
        var templatePath = Path.Combine(rootPath, $"EmailTemplates\\{AppConstant.EmailConfirmationTemplate}.html");
        var body = string.Empty;

        using (StreamReader reader = new(templatePath))
        {
            body = reader.ReadToEnd();
        }

        body = body.Replace("{firstName}", firstName)
                    .Replace("{confirmationCode}", confirmationCode)
                    .Replace("{companyName}", AppConstant.PublicAppName);

        return body;
    }

    public string BuildPasswordResetTemplate(string firstName, string confirmationCode)
    {
        var rootPath = _env.ContentRootPath;
        var templatePath = Path.Combine(rootPath, $"EmailTemplates\\{AppConstant.PasswordResetTemplate}.html");
        var body = string.Empty;

        using (StreamReader reader = new(templatePath))
        {
            body = reader.ReadToEnd();
        }

        body = body.Replace("{firstName}", firstName)
                    .Replace("{confirmationCode}", confirmationCode)
                    .Replace("{expirationTime}", DateTime.Now.AddMinutes(10).ToString("hh:mm tt", CultureInfo.InvariantCulture))
                    .Replace("{companyName}", AppConstant.PublicAppName);

        return body;
    }

    public string BuildPasswordResetConfirmationTemplate(string firstName)
    {
        var rootPath = _env.ContentRootPath;
        var templatePath = Path.Combine(rootPath, $"EmailTemplates\\{AppConstant.PasswordResetConfirmationTemplate}.html");
        var body = string.Empty;

        using (StreamReader reader = new(templatePath))
        {
            body = reader.ReadToEnd();
        }

        body = body.Replace("{firstName}", firstName)
                    .Replace("{companyName}", AppConstant.PublicAppName);

        return body;
    }
}
