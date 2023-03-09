namespace FinancialApplication.Service.Contract
{
    public interface IEmailService
    {
        /// <summary>
        /// Send email using SendGrid
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<bool> SendEmailAsync(string toEmail, string subject, string message);
    }
}