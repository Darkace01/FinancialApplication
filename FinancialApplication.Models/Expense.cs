namespace FinancialApplication.Models
{
    public class Expense: Entity
    {
        /// <summary>
        /// Expense title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Expense description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Expense amount
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Expense category id
        /// </summary>
        public int? CategoryId { get; set; }
        /// <summary>
        /// Expense category
        /// </summary>
        public Category Category { get; set; }
        /// <summary>
        /// User id of the expense
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// User of the expense
        /// </summary>
        public ApplicationUser User { get; set; }
    }
}
