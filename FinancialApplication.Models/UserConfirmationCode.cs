namespace FinancialApplication.Models;

public class UserConfirmationCode : Entity
{
    public string Code { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsUsed { get; set; } = false;

}
