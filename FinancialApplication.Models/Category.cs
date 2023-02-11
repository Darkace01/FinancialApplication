namespace FinancialApplication.Models;

public class Category : Entity
{
    /// <summary>
    /// Category title
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// Category description
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Icon
    /// </summary>
    public string Icon { get; set; }
    /// <summary>
    /// Is Sub Category, if true the category is a sub category else it's a primary category
    /// </summary>
    public bool IsSubcategory { get; set; } = false;
    /// <summary>
    /// List of expense in the category
    /// </summary>
    public IEnumerable<Expense> Expenses { get; set; }
    /// <summary>
    /// User id of the category
    /// </summary>
    public string UserId { get; set; }
    /// <summary>
    /// User of the categpry
    /// </summary>
    public ApplicationUser User { get; set; }
}
