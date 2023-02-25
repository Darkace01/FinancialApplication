namespace FinancialApplication.DTO;

public class TransactionDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime DateAdded { get; set; }
    public string DateAddedFormatted { get {return DateAdded.ToString("dd/MM/yyyy"); } }
    
    public int? CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string UserId { get; set; }
}

public class TransactionCreateDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public int? CategoryId { get; set; }
    public string UserId { get; set; }
}

public class TransactionUpdateDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public int? CategoryId { get; set; }
    public string UserId { get; set; }
}
