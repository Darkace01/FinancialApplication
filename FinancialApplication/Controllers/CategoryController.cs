using Microsoft.AspNetCore.Authorization;

namespace FinancialApplication.Controllers;

[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/category")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IRepositoryServiceManager _repo;
    private readonly ILogger<CategoryController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public CategoryController(IRepositoryServiceManager repo, ILogger<CategoryController> logger, UserManager<ApplicationUser> userManager)
    {
        _repo = repo;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet(CategoryRoutes.GetByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<Category>>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> GetUsersCategories()
    {
        try
        {
            var user = await GetUser();
            var categories = await _repo.CategoryService.GetByUser(user.Id);
            return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<CategoryDTO>>()
            {
                statusCode = StatusCodes.Status200OK,
                message = "Categories retrieved successfully",
                data = categories,
                hasError = false
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Fetch users categories", ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status500InternalServerError,
                hasError = true,
                message = "An error occured while processing your request",
                data = null
            });
        }
    }

    [HttpPost(CategoryRoutes.CreateByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> CreateNewUserCategory(CategoryCreateDTO model)
    {
        try
        {
            if (model is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status400BadRequest,
                hasError = true,
                message = "Invalid authentication request",
                data = null
            });
            // TODO
            var user = await GetUser();

            var exists = await _repo.CategoryService.IsCategoryExistForUser(model.Title, user.Id);
            if (exists) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status400BadRequest,
                hasError = true,
                message = "Category already exists",
                data = null
            });

            var category = new Category()
            {
                Title = model.Title,
                UserId = user.Id,
                Description = model.Description,
                IsSubcategory = model.IsSubcategory
            };
            await _repo.CategoryService.Add(category);
            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                message = "Category created successfully",
                data = null,
                hasError = false
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Create new category");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status500InternalServerError,
                hasError = true,
                message = "An error occured while processing your request",
                data = null
            });
        }
    }

    [HttpDelete(CategoryRoutes.DeleteByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> DeleteUserCategory(int categoryId)
    {
        try
        {
            var user = await GetUser();
            await _repo.CategoryService.DeleteUserCategory(user.Id, categoryId);
            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                message = "Category deleted successfully",
                data = null,
                hasError = false
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete category");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status500InternalServerError,
                hasError = true,
                message = "An error occured while processing your request",
                data = null
            });
        }
    }

    [HttpPost(CategoryRoutes.UpdateByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> UpdateUserCategory(int categoryId, CategoryUpdateDTO model)
    {
        try
        {
            var user = await GetUser();
            var category = await _repo.CategoryService.GetByUserAndCategoryId(user.Id,categoryId);
            if (category is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status400BadRequest,
                hasError = true,
                message = "Category not found",
                data = null
            });
            await _repo.CategoryService.UpdateUserCategory(user.Id,model);
            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                message = "Category updated successfully",
                data = null,
                hasError = false
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update category");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
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
