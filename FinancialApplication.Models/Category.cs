namespace FinancialApplication.Models;

public class Category : Entity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsSubcategory { get; set; } = false;
    public IEnumerable<Expense> Expenses { get; set; }
}
