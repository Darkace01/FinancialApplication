namespace FinancialApplication.Service.Contract
{
    public interface IUserService
    {
        Task<string> GenerateUserConfirmationCode(string userId);
        Task<bool> VerifyUserConfirmationCode(string userId, string code);
        Task<bool> VerifyUserEmail(string userId, string code);
    }
}