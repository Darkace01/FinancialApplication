using Microsoft.AspNetCore.Identity;

namespace FinancialApplication.Models;

public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// The first name of the user.
    /// </summary>
    public string FirstName { get; set; }
    /// <summary>
    /// The last name of the user
    /// </summary>
    public string LastName { get; set; }
    /// <summary>
    /// Profile picture url of the user
    /// </summary>
    public string ProfilePictureUrl { get; set; }
    /// <summary>
    /// Cloudinary public Id
    /// </summary>
    public string ProfilePictureId { get; set; }
    /// <summary>
    /// User has signed up with google
    /// </summary>
    public bool ExternalAuthInWithGoogle { get; set; }
    /// <summary>
    /// Expo notification token used for push notifications
    /// </summary>
    public string ExpoNotificationToken { get; set; }
    /// <summary>
    /// Received push notifications. If false, user will not receive push notifications.
    /// </summary>
    public bool RecievePushNotification { get; set; }
    /// <summary>
    /// List of user expenses
    /// </summary>
    public IEnumerable<Transaction> Transactions { get; set; }

    /// <summary>
    /// User confirmation code
    /// </summary>
    public IEnumerable<UserConfirmationCode> UserConfirmationCodes { get; set; }
}
