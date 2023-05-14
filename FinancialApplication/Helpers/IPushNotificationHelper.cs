namespace FinancialApplication.Helpers
{
    public interface IPushNotificationHelper
    {
        Task Send12pmPushNotification();
        Task Send3pmPushNotification();
        Task Send6pmPushNotification();
        Task Send9amPushNotification();
        Task<string> SendUsersTestPushNotification();
    }
}