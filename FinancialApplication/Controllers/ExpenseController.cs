namespace FinancialApplication.Controllers;

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

    [HttpGet(TransactionRoutes.GetByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TransactionDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsersTransactions(string startDateStr, string endDateStr, int take,string searchTerm)
    {
        try
        {
            var user = await GetUser();
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(startDateStr))
            {
                if (startDateStr.Contains("/"))
                {
                    _ = DateTime.TryParse(startDateStr, out startDate);
                }
            }
            if (!string.IsNullOrWhiteSpace(endDateStr))
            {
                if (endDateStr.Contains("/"))
                {
                    _ = DateTime.TryParse(endDateStr, out endDate);
                }
            }
            var total = take == 0 ? 50 : take;
            var transactions = await _repo.TransactionService.GetByUserWithParameters(user.Id, startDate, endDate, total,searchTerm);

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<TransactionDTO>>()
            {
                statusCode = StatusCodes.Status200OK,
                message = "Transactions retrieved successfully",
                data = transactions,
                hasError = false
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fetch users transactions");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status500InternalServerError,
                hasError = true,
                message = "An error occured while processing your request",
                data = null
            });
        }
    }

    [HttpPost(TransactionRoutes.CreateByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateNewUserTransaction(TransactionCreateDTO model)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Create new transaction");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status500InternalServerError,
                hasError = true,
                message = "An error occured while processing your request",
                data = null
            });
        }
    }
    
    [HttpPut(TransactionRoutes.UpdateByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUserTransaction(int transactionId, TransactionUpdateDTO model)
    {
        try
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

            transaction.Amount = model.Amount;
            transaction.CategoryId = model.CategoryId;
            transaction.Description = model.Description;

            await _repo.TransactionService.Update(transaction);

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = false,
                message = "Transaction updated successfully",
                data = null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update transaction");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status500InternalServerError,
                hasError = true,
                message = "An error occured while processing your request",
                data = null
            });
        }
    }

    [HttpDelete(TransactionRoutes.DeleteByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUserTransaction(int transactionId)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete transaction");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status500InternalServerError,
                hasError = true,
                message = "An error occured while processing your request",
                data = null
            });
        }
    }

    [HttpGet(TransactionRoutes.GetByTransactionIdandUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<TransactionDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserTransaction(int transactionId)
    {
        try
        {
            var user = await GetUser();
            var transaction = await _repo.TransactionService.GetByIdandUserId(transactionId, user.Id);

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Get transaction");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<TransactionDTO>()
            {
                statusCode = StatusCodes.Status500InternalServerError,
                hasError = true,
                message = "An error occured while processing your request",
                data = null
            });
        }
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
