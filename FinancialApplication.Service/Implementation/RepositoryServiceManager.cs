namespace FinancialApplication.Service.Implementation;

public class RepositoryServiceManager : IRepositoryServiceManager
{
    private ITransactionService _transactionService;
    private ICategoryService _categoryService;
    private IEmailService _emailService;
    private IUserService _userService;
    private IFileStorageService _fileStorageService;
    private INotificationService _notificationService;


    private readonly FinancialApplicationDbContext _context;
    private readonly IConfiguration _config;
    private readonly Cloudinary _cloudinary;

    public RepositoryServiceManager(FinancialApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
        _cloudinary = new Cloudinary(_config["Cloudinary:Url"]);
    }

    public ITransactionService TransactionService
    {
        get
        {
            _transactionService ??= new TransactionService(_context);
            return _transactionService;
        }
    }

    public ICategoryService CategoryService
    {
        get
        {
            _categoryService ??= new CategoryService(_context);
            return _categoryService;
        }
    }

    public IEmailService EmailService
    {
        get
        {
            _emailService ??= new SMTPMailService(_config);
            return _emailService;
        }
    }

    public IUserService UserService
    {
        get
        {
            _userService ??= new UserService(_context);
            return _userService;
        }
    }

    public IFileStorageService FileStorageService
    {
        get
        {
            _fileStorageService ??= new FileStorageService(_cloudinary);
            return _fileStorageService;
        }
    }

    public INotificationService NotificationService
    {
        get
        {
            _notificationService ??= new NotificationService(_context);
            return _notificationService;
        }
    }
}
