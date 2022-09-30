namespace FinancialApplication.Models
{
    public class Expense: Entity
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateAdded { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
