namespace FinancialApplication.Controllers;

[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/categories")]
[ApiController]
[Authorize]
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

    [HttpGet(CategoryRoutes._getAllCategories)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _repo.CategoryService.GetAll();
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<CategoryDTO>>()
        {
            statusCode = StatusCodes.Status200OK,
            message = "Categories retrieved successfully",
            data = categories,
            hasError = false
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
