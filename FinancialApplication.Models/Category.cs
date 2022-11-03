namespace FinancialApplication.Models;

public class Category : Entity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public bool IsSubcategory { get; set; } = false;
    public IEnumerable<Expense> Expenses { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
