using Microsoft.AspNetCore.Authorization;

namespace FinancialApplication.Controllers;

[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/category")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IRepositoryServiceManager _repo;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(IRepositoryServiceManager repo, ILogger<CategoryController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [HttpGet(CategoryRoutes.GetByUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<Category>>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> GetUsersCategories()
    {
        try
        {
            var user = User.Identity.Name;
            var categories = await _repo.CategoryService.GetByUser(user);
            return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<Category>>()
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
            
            var user = User.Identity.Name;
            var category = new Category()
            {
                Title = model.Title,
                UserId = user,
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
            _logger.LogError("Create new category", ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status500InternalServerError,
                hasError = true,
                message = "An error occured while processing your request",
                data = null
            });
        }
    }
}
