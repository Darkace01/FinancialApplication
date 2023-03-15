namespace FinancialApplication.Service.Implementation;

public class RepositoryServiceManager : IRepositoryServiceManager
{
    private ITransactionService _transactionService;
    private ICategoryService _categoryService;
    private IEmailService _emailService;
    private IUserService _userService;
    private IFileStorageService _fileStorageService;


    private readonly FinancialApplicationDbContext _context;
    private readonly IConfiguration _config;
    private readonly Cloudinary _cloudinary;

    public RepositoryServiceManager(FinancialApplicationDbContext context, IConfiguration config, Cloudinary cloudinary)
    {
        _context = context;
        _config = config;
        _cloudinary = cloudinary;
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

    public IEmailService EmailService
    {
        get
        {
            if (_emailService == null)
            {
                _emailService = new SMTPMailService(_config);
            }
            return _emailService;
        }
    }

    public IUserService UserService
    {
        get
        {
            if (_userService == null)
            {
                _userService = new UserService(_context);
            }
            return _userService;
        }
    }

    public IFileStorageService FileStorageService
    {
        get
        {
            if (_fileStorageService == null)
            {
                _fileStorageService = new FileStorageService(_cloudinary);
            }
            return _fileStorageService;
        }
    }
}
