namespace FinancialApplication.Service.Contract
{
    public interface IUserService
    {
        /// <summary>
        /// Generate a random 6 digit code for user confirmation
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GenerateUserConfirmationCode(string userId);

        /// <summary>
        /// Check if user confirmation code is valid. Returns true if the code is valid else it returns false
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<bool> VerifyUserConfirmationCode(string userId, string code);

        /// <summary>
        /// Verify User Email. Returns true if the code is valid else it returns false
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<bool> VerifyUserEmail(string userId, string code);
    }
}