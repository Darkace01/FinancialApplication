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
    /// List of user expenses
    /// </summary>
    public IEnumerable<Transaction> Transactions { get; set; }
}
