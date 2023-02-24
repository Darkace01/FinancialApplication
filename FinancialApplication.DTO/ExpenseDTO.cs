namespace FinancialApplication.DTO;

public class ExpenseDTO
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

public class ExpenseCreateDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public int? CategoryId { get; set; }
    public string UserId { get; set; }
}

public class ExpenseUpdateDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public int? CategoryId { get; set; }
    public string UserId { get; set; }
}
