namespace FinancialApplication.Models;

public class Entity
{
    public int Id { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DateTime DateModified { get; set; } = DateTime.Now;
}
