namespace FinancialApplication.Service.Contract;

public interface IRepositoryServiceManager
{
    ICategoryService CategoryService { get; }
    IExpenseService ExpenseService { get; }
}