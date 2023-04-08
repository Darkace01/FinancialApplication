namespace FinancialApplication.Service.Contract
{
    public interface INotificationService
    {
        Task<List<string>> GetAllEnabledNotificationUsersNotificationTokens();
        Task<string> GetUserNotificationTokenByUserId(string userId);
        Task<bool> TurnOffReceivingNotificationForUser(string userId);
        Task<bool> TurnOnReceivingNotificationForUser(string userId);
        Task<bool> UpdateUserNotificationToken(string userId, string notificationToken);
    }
}