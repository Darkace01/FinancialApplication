﻿namespace FinancialApplication.Controllers;

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
    public async Task<IActionResult> GetUsersExpenses()
    {
        try
        {
            var user = await GetUser();
            var expenses = await _repo.ExpenseService.GetByUser(user.Id);
            return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<ExpenseDTO>>()
            {
                statusCode = StatusCodes.Status200OK,
                message = "Expenses retrieved successfully",
                data = expenses.Select(x => new ExpenseDTO()
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    CategoryId = x.CategoryId,
                    DateAdded = x.DateAdded,
                    DateAddedFormatted = x.DateAdded.ToString("dd/MM/yyyy"),
                    Description = x.Description,
                    UserId = x.UserId,
                    CategoryName = x.Category?.Title
                }),
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

            var user = await GetUser();
            var expense = new Expense()
            {
                Amount = model.Amount,
                CategoryId = model.CategoryId,
                DateAdded = DateTime.Now,
                Description = model.Description,
                UserId = user.Id
            };

            await _repo.ExpenseService.Add(expense);
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
    public async Task<IActionResult> UpdateUserExpense(int expenseId,ExpenseUpdateDTO model)
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

    [HttpGet(ExpenseRoutes.GetByUser)]
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

            var expenseDTO = new ExpenseDTO()
            {
                Amount = expense.Amount,
                CategoryId = expense.CategoryId,
                CategoryName = expense.Category?.Title,
                DateAdded = expense.DateAdded,
                Description = expense.Description,
                Id = expense.Id,
                DateAddedFormatted = expense.DateAdded.ToString("dd/MM/yyyy"),
                UserId = expense.UserId
            };

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<ExpenseDTO>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = false,
                message = "Expense retrieved successfully",
                data = expenseDTO
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