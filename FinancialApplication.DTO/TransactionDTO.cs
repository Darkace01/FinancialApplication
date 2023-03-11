﻿namespace FinancialApplication.DTO;

public class TransactionDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime DateAdded { get; set; }
    public string DateAddedFormatted { get {return DateAdded.ToString("dd/MM/yyyy"); } }
    
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string CategoryIcon { get; set; }
    public string UserId { get; set; }
    public bool InFlow { get; set; }
}

public class TransactionCreateDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public int CategoryId { get; set; }
    public string UserId { get; set; }
    public bool InFlow { get; set; }
    public string DateAdded { get; set; }
}

public class TransactionUpdateDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public int CategoryId { get; set; }
    public string UserId { get; set; }
    public DateTime? DateAdded { get; set; }
}


public class ClientTransactionBalance
{
    public decimal TotalInflow { get; set; }
    public decimal TotalOutflow { get; set; }
    public decimal Balance { get; set; }
    public decimal Percentage { get; set; }
}

public class ClientTransactionMonthlyBalance
{
    public string Month { get; set; }
    public decimal Balance { get; set; }
    public decimal Percentage { get; set; }
}

public class DashboardTransactionandBalance
{
    public List<TransactionDTO> Transactions { get; set; }
    public ClientTransactionBalance Balance { get; set; }
    public List<ClientTransactionMonthlyBalance> MonthlyBalance { get; set; }
}