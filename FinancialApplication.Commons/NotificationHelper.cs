using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialApplication.Commons;

public static class NotificationHelper
{
    public static List<TimeSpan> GetNotificationIntervals()
    {
        List<TimeSpan> notificationIntervals = new()
        {
            TimeSpan.FromHours(8),   // 8am
            TimeSpan.FromHours(10),  // 10am
            TimeSpan.FromHours(15),  // 3pm
            TimeSpan.FromHours(18),  // 6pm
        };
        return notificationIntervals;
    }
    // create cron expression for 8am, 10am, 3pm and 6pm
    public static string GetCronExpressionForNotificationIntervals()
    {
        var notificationIntervals = GetNotificationIntervals();
        var cronExpression = string.Empty;
        foreach (var interval in notificationIntervals)
        {
            cronExpression += $"{interval.Minutes} {interval.Hours} * * *";
        }
        return cronExpression;
    }
}
