using Microsoft.AspNetCore.Identity;

namespace FinancialApplication.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
}
