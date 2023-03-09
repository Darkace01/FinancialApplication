namespace FinancialApplication.Service.Contract
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string message);
    }
}