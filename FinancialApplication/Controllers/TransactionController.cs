﻿namespace FinancialApplication.Controllers;

[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/transactions")]
[ApiController]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly IRepositoryServiceManager _repo;
    private readonly ILogger<TransactionController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public TransactionController(UserManager<ApplicationUser> userManager, ILogger<TransactionController> logger, IRepositoryServiceManager repo)
    {
        _userManager = userManager;
        _logger = logger;
        _repo = repo;
    }

    [HttpGet(TransactionRoutes._getByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TransactionDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsersTransactions(string startDateStr, string endDateStr, int take, string searchTerm)
    {
        var user = await GetUser();
        var startDate = DateTime.MinValue;
        var endDate = DateTime.MaxValue;
        if (!string.IsNullOrWhiteSpace(startDateStr))
        {
            if (startDateStr.Contains('/'))
            {
                startDate = CommonHelpers.ConvertToDate(startDateStr);
            }
        }
        if (!string.IsNullOrWhiteSpace(endDateStr))
        {
            if (endDateStr.Contains('/'))
            {
                endDate = CommonHelpers.ConvertToDate(endDateStr);
            }
        }
        var total = take == 0 ? 50 : take;
        var transactions = await _repo.TransactionService.GetByUserWithParameters(user.Id, startDate, endDate, total, searchTerm);

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<TransactionDTO>>()
        {
            statusCode = StatusCodes.Status200OK,
            message = $"Transactions retrieved successfully {searchTerm}",
            data = transactions,
            hasError = false
        });
    }

    [HttpPost(TransactionRoutes._createByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateNewUserTransaction(TransactionCreateDTO model)
    {
        if (model is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = true,
            message = "Invalid request",
            data = null
        });

        if (string.IsNullOrWhiteSpace(model.Title)) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = true,
            message = "Title is required",
            data = null
        });

        if(string.IsNullOrWhiteSpace(model.DateAdded)) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = true,
            message = "Date is required",
            data = null
        });

        if (!model.DateAdded.Contains('/')) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = true,
            message = "Incorrect Date Format. Expected dd/MM/yyyy",
            data = null
        });

        if (model.Amount <= 0) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = true,
            message = "Amount is required",
            data = null
        });

        var user = await GetUser();
        model.UserId = user.Id;
        await _repo.TransactionService.Add(model);
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = false,
            message = "Transaction created successfully",
            data = null
        });
    }

    [HttpPut(TransactionRoutes._updateByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUserTransaction(int transactionId, TransactionUpdateDTO model)
    {
        if (model is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = true,
            message = "Invalid request",
            data = null
        });

        var user = await GetUser();
        var transaction = await _repo.TransactionService.GetByIdandUserId(transactionId, user.Id);

        if (transaction is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = true,
            message = "Transaction not found",
            data = null
        });

        if (transaction.UserId != user.Id) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = true,
            message = "You are not authorized to perform this action",
            data = null
        });
        
        model.UserId = user.Id;

        await _repo.TransactionService.Update(model);

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = false,
            message = "Transaction updated successfully",
            data = null
        });
    }

    [HttpDelete(TransactionRoutes._deleteByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUserTransaction(int transactionId)
    {
        var user = await GetUser();
        var transaction = await _repo.TransactionService.GetByIdandUserId(transactionId, user.Id);

        if (transaction is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = true,
            message = "Transaction not found",
            data = null
        });

        if (transaction.UserId != user.Id) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = true,
            message = "You are not authorized to perform this action",
            data = null
        });

        await _repo.TransactionService.Delete(transactionId);

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = false,
            message = "Transaction deleted successfully",
            data = null
        });
    }

    [HttpGet(TransactionRoutes._getByTransactionIdandUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<TransactionDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserTransaction(int transactionId, CancellationToken cancellationToken)
    {
        var user = await GetUser();
        var transaction = await _repo.TransactionService.GetByIdandUserId(transactionId, user.Id,cancellationToken);

        if (transaction is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<TransactionDTO>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = true,
            message = "Transaction not found",
            data = null
        });

        if (transaction.UserId != user.Id) return StatusCode(StatusCodes.Status200OK, new ApiResponse<TransactionDTO>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = true,
            message = "You are not authorized to perform this action",
            data = null
        });

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<TransactionDTO>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = false,
            message = "Transaction retrieved successfully",
            data = transaction
        });
    }

    [HttpGet(TransactionRoutes._getUserTransactionBalance)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<ClientTransactionBalance>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserTransactionBalance(CancellationToken cancellationToken)
    {
        var user = await GetUser();
        var clientTransactionBalance = await _repo.TransactionService.GetUserBalanceForTheMonth(user.Id, DateTime.Now, cancellationToken);

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<ClientTransactionBalance>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = false,
            message = "Transaction balance retrieved successfully",
            data = clientTransactionBalance
        });
    }

    [HttpGet(TransactionRoutes._getUserTransactionMonthlyBalance)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<List<ClientTransactionMonthlyBalance>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserBalanceForEveryMonthFromJanuaryToDecember(CancellationToken cancellationToken)
    {
        var user = await GetUser();
        var userMonthlyTransactionBalance = await _repo.TransactionService.GetUserBalanceForEveryMonthFromJanuaryToDecember(user.Id,cancellationToken);

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<List<ClientTransactionMonthlyBalance>>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = false,
            message = "Transaction monthly balance retrieved successfully",
            data = userMonthlyTransactionBalance
        });
    }

    [HttpGet(TransactionRoutes._getUserDashboard)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<List<DashboardTransactionandBalance>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserDashboard(CancellationToken cancellationToken)
    {
        var user = await GetUser();
        var startDate = DateTime.MinValue;
        var endDate = DateTime.MaxValue;
        var total = 5;
        var transactions = await _repo.TransactionService.GetByUserWithParameters(user.Id, startDate, endDate, total, "");
        var clientBalance = await _repo.TransactionService.GetUserBalanceForTheMonth(user.Id, DateTime.Now, cancellationToken);
        var userMonthlyTransactionBalance = await _repo.TransactionService.GetUserBalanceForEveryMonthFromJanuaryToDecember(user.Id,cancellationToken);

        var dashboardTransactionandBalance = new DashboardTransactionandBalance()
        {
            Transactions = transactions.ToList(),
            Balance = clientBalance,
            MonthlyBalance = userMonthlyTransactionBalance
        };

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<DashboardTransactionandBalance>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = false,
            message = "Dashboard retrieved successfully",
            data = dashboardTransactionandBalance
        });

    }

    #region Helpers
    private async Task<ApplicationUser> GetUser()
    {
        var username = User.Identity.Name;
        var user = await _userManager.FindByNameAsync(username);
        return user;
    }
    #endregion
}
