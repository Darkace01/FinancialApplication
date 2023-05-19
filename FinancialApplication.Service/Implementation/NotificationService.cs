namespace FinancialApplication.Service.Implementation;

public class NotificationService : INotificationService
{
    private readonly FinancialApplicationDbContext _context;

    public NotificationService(FinancialApplicationDbContext context)
    {
        _context = context;
    }


    /// <summary>
    /// updates a user expo notification token
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="notificationToken"></param>
    /// <returns>Returns true if the operation was successful else it returns false</returns>
    public async Task<bool> UpdateUserNotificationToken(string userId, string notificationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user != null)
        {
            user.ExpoNotificationToken = notificationToken;
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
        return false;
    }
    /// <summary>
    /// Get all users notification token who have accepted to receive notification
    /// </summary>
    /// <returns>List of notification tokens</returns>
    public async Task<List<string>> GetAllEnabledNotificationUsersNotificationTokens()
    {
        var users = await _context.Users.AsNoTracking().Where(x => x.ReceivePushNotification == true).ToListAsync();
        return users.Select(x => x.ExpoNotificationToken).ToList();
    }

    /// <summary>
    /// Get a user notification who has accepted to receive notification
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<string> GetUserNotificationTokenByUserId(string userId)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId && x.ReceivePushNotification == true);
        if (user != null)
        {
            return user.ExpoNotificationToken;
        }
        return null;
    }

    /// <summary>
    /// Turn on receiving notification for a user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<bool> TurnOnReceivingNotificationForUser(string userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user != null)
        {
            user.ReceivePushNotification = true;
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
        return false;
    }

    /// <summary>
    /// Turn off receiving notification for a user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<bool> TurnOffReceivingNotificationForUser(string userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user != null)
        {
            user.ReceivePushNotification = false;
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
        return false;
    }
}
