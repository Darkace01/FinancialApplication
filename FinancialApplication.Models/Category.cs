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
    /// List of expense in the category
    /// </summary>
    public IEnumerable<Transaction> Transactions { get; set; }
}
