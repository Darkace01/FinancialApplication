﻿using Expo.Server.Client;
using Expo.Server.Models;
using Newtonsoft.Json.Linq;

namespace FinancialApplication.Helpers;

public class PushNotificationHelper
{
    private readonly IRepositoryServiceManager _repo;
    private readonly ILogger<PushNotificationHelper> _logger;

    public PushNotificationHelper(IRepositoryServiceManager repo)
    {
        _repo = repo;
    }

    public async Task SendUsersPushNotification()
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
                _logger.LogError(error.ErrorMessage);
            }
        }
    }
}