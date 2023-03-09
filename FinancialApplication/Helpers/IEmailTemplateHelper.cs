namespace FinancialApplication.Helpers
{
    public interface IEmailTemplateHelper
    {
        string BuildEmailConfirmationTemplate(string firstName, string confirmationCode);
        string BuildPasswordResetConfirmationTemplate(string firstName);
        string BuildPasswordResetTemplate(string firstName, string confirmationCode);
    }
}