namespace FinancialApplication.Service.Implementation;

public class RepositoryServiceManager : IRepositoryServiceManager
{
    private IExpenseService _expenseService;
    private ICategoryService _categoryService;
    private readonly FinancialApplicationDbContext _context;

    public RepositoryServiceManager(FinancialApplicationDbContext context)
    {
        _context = context;
    }

    public IExpenseService ExpenseService
    {
        get
        {
            if (_expenseService == null)
            {
                _expenseService = new ExpenseService(_context);
            }
            return _expenseService;
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
