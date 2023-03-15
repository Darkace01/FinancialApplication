namespace FinancialApplication.Service.Contract;

public interface IRepositoryServiceManager
{
    ICategoryService CategoryService { get; }
    ITransactionService TransactionService { get; }
    IEmailService EmailService { get; }
    IUserService UserService { get; }
    IFileStorageService FileStorageService { get; }
}