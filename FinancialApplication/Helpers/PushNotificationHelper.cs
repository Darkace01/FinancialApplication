using Expo.Server.Client;
using Expo.Server.Models;
using System.Text.Json;

namespace FinancialApplication.Helpers;

public class PushNotificationHelper : IPushNotificationHelper
{
    private readonly IRepositoryServiceManager _repo;

    public PushNotificationHelper(IRepositoryServiceManager repo)
    {
        _repo = repo;
    }

    public async Task<string> SendUsersTestPushNotification()
    {
        var expoSDKClient = new PushApiClient();
        var usersNotificationToken = await _repo.NotificationService.GetAllEnabledNotificationUsersNotificationTokens();
        var messages = new PushTicketRequest()
        {
            PushTo = usersNotificationToken,
            PushTitle = "Test Notification",
            PushBody = "This is a test notification",
            PushSound = "default",
        };

        var result = await expoSDKClient.PushSendAsync(messages);
        //Check if error occurred and log it
        if (result?.PushTicketErrors?.Count > 0)
        {
            foreach (var error in result.PushTicketErrors)
            {
            }
        }

        var responseString = JsonSerializer.Serialize(result);
        return responseString;
    }

    public async Task Send9amPushNotification()
    {
        var expoSDKClient = new PushApiClient();
        var usersNotificationToken = await _repo.NotificationService.GetAllEnabledNotificationUsersNotificationTokens();
        var messages = new PushTicketRequest()
        {
            PushTo = usersNotificationToken,
            PushTitle = "Good morning 🫣",
            PushBody = "Spent today? Log it now.",
            PushSound = "default",
        };

        _ = await expoSDKClient.PushSendAsync(messages);
    }

    public async Task Send12pmPushNotification()
    {
        var expoSDKClient = new PushApiClient();
        var usersNotificationToken = await _repo.NotificationService.GetAllEnabledNotificationUsersNotificationTokens();
        var messages = new PushTicketRequest()
        {
            PushTo = usersNotificationToken,
            PushTitle = "Good afternoon 🫣",
            PushBody = "Spent today? Log it now.",
            PushSound = "default",
        };

        _ = await expoSDKClient.PushSendAsync(messages);
    }

    public async Task Send3pmPushNotification()
    {
        var expoSDKClient = new PushApiClient();
        var usersNotificationToken = await _repo.NotificationService.GetAllEnabledNotificationUsersNotificationTokens();
        var messages = new PushTicketRequest()
        {
            PushTo = usersNotificationToken,
            PushTitle = "Good afternoon 🫣",
            PushBody = "Spent today? Log it now.",
            PushSound = "default",
        };

        _ = await expoSDKClient.PushSendAsync(messages);
    }

    public async Task Send6pmPushNotification()
    {
        var expoSDKClient = new PushApiClient();
        var usersNotificationToken = await _repo.NotificationService.GetAllEnabledNotificationUsersNotificationTokens();
        var messages = new PushTicketRequest()
        {
            PushTo = usersNotificationToken,
            PushTitle = "Good evening 🫣",
            PushBody = "Spent today? Log it now.",
            PushSound = "default",
        };

        _ = await expoSDKClient.PushSendAsync(messages);
    }
}
