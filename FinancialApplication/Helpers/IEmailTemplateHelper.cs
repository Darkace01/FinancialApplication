namespace FinancialApplication.Helpers
{
    public interface IEmailTemplateHelper
    {
        string BuildEmailConfirmationTemplate(string firstName, string confirmationCode);
    }
}