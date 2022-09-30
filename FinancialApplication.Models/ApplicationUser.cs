using Microsoft.AspNetCore.Identity;

namespace FinancialApplication.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public IEnumerable<Expense> Expenses { get; set; }
    public IEnumerable<Category> Categories { get; set; }
}
