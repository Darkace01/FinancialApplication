namespace FinancialApplication.Service.Implementation;

public class RepositoryServiceManager : IRepositoryServiceManager
{
    private ITransactionService _transactionService;
    private ICategoryService _categoryService;
    private readonly FinancialApplicationDbContext _context;

    public RepositoryServiceManager(FinancialApplicationDbContext context)
    {
        _context = context;
    }

    public ITransactionService TransactionService
    {
        get
        {
            if (_transactionService == null)
            {
                _transactionService = new TransactionService(_context);
            }
            return _transactionService;
        }
    }

    public ICategoryService CategoryService
    {
        get
        {
            if (_categoryService == null)
            {
                _categoryService = new CategoryService(_context);
            }
            return _categoryService;
        }
    }
}
