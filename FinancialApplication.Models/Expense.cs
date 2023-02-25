namespace FinancialApplication.Models
{
    public class Transaction: Entity
    {
        /// <summary>
        /// Transaction title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Transaction description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Transaction amount
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Transaction category id
        /// </summary>
        public int? CategoryId { get; set; }
        /// <summary>
        /// Transaction category
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
