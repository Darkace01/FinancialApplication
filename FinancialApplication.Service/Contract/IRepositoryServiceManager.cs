namespace FinancialApplication.Service.Contract;

public interface IRepositoryServiceManager
{
    ICategoryService CategoryService { get; }
    ITransactionService TransactionService { get; }
}