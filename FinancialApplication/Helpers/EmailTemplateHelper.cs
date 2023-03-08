using FinancialApplication.Commons;

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

        using (StreamReader reader = new StreamReader(templatePath))
        {
            body = reader.ReadToEnd();
        }

        body = body.Replace("{firstName}", firstName)
                    .Replace("{confirmationCode}", confirmationCode)
                    .Replace("{companyName}", AppConstant.PublicAppName);

        return body;
    }
}
