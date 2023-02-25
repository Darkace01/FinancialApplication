namespace FinancialApplication.Controllers;

[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/expenses")]
[ApiController]
[Authorize]
public class ExpenseController : ControllerBase
{
    private readonly IRepositoryServiceManager _repo;
    private readonly ILogger<ExpenseController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public ExpenseController(UserManager<ApplicationUser> userManager, ILogger<ExpenseController> logger, IRepositoryServiceManager repo)
    {
        _userManager = userManager;
        _logger = logger;
        _repo = repo;
    }

    [HttpGet(ExpenseRoutes.GetByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExpenseDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsersExpenses(string startDateStr, string endDateStr, int take,string searchTerm)
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
            var expenses = await _repo.ExpenseService.GetByUserWithParameters(user.Id, startDate, endDate, total,searchTerm);

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<ExpenseDTO>>()
            {
                statusCode = StatusCodes.Status200OK,
                message = "Expenses retrieved successfully",
                data = expenses,
                hasError = false
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fetch users expenses");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status500InternalServerError,
                hasError = true,
                message = "An error occured while processing your request",
                data = null
            });
        }
    }

    [HttpPost(ExpenseRoutes.CreateByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateNewUserExpense(ExpenseCreateDTO model)
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
            await _repo.ExpenseService.Add(model);
            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = false,
                message = "Expense created successfully",
                data = null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Create new expense");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status500InternalServerError,
                hasError = true,
                message = "An error occured while processing your request",
                data = null
            });
        }
    }
    
    [HttpPut(ExpenseRoutes.UpdateByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUserExpense(int expenseId, ExpenseUpdateDTO model)
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
            var expense = await _repo.ExpenseService.GetByIdandUserId(expenseId, user.Id);

            if (expense is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = true,
                message = "Expense not found",
                data = null
            });

            if (expense.UserId != user.Id) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = true,
                message = "You are not authorized to perform this action",
                data = null
            });

            expense.Amount = model.Amount;
            expense.CategoryId = model.CategoryId;
            expense.Description = model.Description;

            await _repo.ExpenseService.Update(expense);

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = false,
                message = "Expense updated successfully",
                data = null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update expense");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status500InternalServerError,
                hasError = true,
                message = "An error occured while processing your request",
                data = null
            });
        }
    }

    [HttpDelete(ExpenseRoutes.DeleteByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUserExpense(int expenseId)
    {
        try
        {
            var user = await GetUser();
            var expense = await _repo.ExpenseService.GetByIdandUserId(expenseId, user.Id);

            if (expense is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = true,
                message = "Expense not found",
                data = null
            });

            if (expense.UserId != user.Id) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = true,
                message = "You are not authorized to perform this action",
                data = null
            });

            await _repo.ExpenseService.Delete(expenseId);

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = false,
                message = "Expense deleted successfully",
                data = null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete expense");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status500InternalServerError,
                hasError = true,
                message = "An error occured while processing your request",
                data = null
            });
        }
    }

    [HttpGet(ExpenseRoutes.GetByExpenseIdandUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<ExpenseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserExpense(int expenseId)
    {
        try
        {
            var user = await GetUser();
            var expense = await _repo.ExpenseService.GetByIdandUserId(expenseId, user.Id);

            if (expense is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<ExpenseDTO>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = true,
                message = "Expense not found",
                data = null
            });

            if (expense.UserId != user.Id) return StatusCode(StatusCodes.Status200OK, new ApiResponse<ExpenseDTO>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = true,
                message = "You are not authorized to perform this action",
                data = null
            });

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<ExpenseDTO>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = false,
                message = "Expense retrieved successfully",
                data = expense
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Get expense");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<ExpenseDTO>()
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
