
namespace FinancialApplication.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiversion}/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJWTHelper _jWTHelper;
        private readonly IRepositoryServiceManager _repo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(IJWTHelper jWTHelper, IRepositoryServiceManager repo, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _jWTHelper = jWTHelper;
            _repo = repo;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost(AuthRoutes.Login)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                if (model is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status401Unauthorized,
                    hasError = true,
                    message = "Invalid authentication request",
                    data = null
                });
                var user = await _userManager.FindByNameAsync(model.username);
                if (user == null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status401Unauthorized,
                    hasError = true,
                    message = "Invalid username or password",
                    data = null
                });

                var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.password);
                if(isPasswordValid != true) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status401Unauthorized,
                    hasError = true,
                    message = "Invalid username or password",
                    data = null
                });

                var userRoles = await _userManager.GetRolesAsync(user);
                var token = _jWTHelper.GenerateToken(user, userRoles);

                return StatusCode(StatusCodes.Status200OK, new ApiResponse<LoginResponseDTO>()
                {
                    statusCode = StatusCodes.Status200OK,
                    hasError = false,
                    message = "Authrorized",
                    data = new LoginResponseDTO()
                    {
                        accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        emailAddress = user.Email,
                        fullName = user.FullName,
                        userId = user.Id
                    }
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    hasError = true,
                    message = "An error occured while processing your request",
                    data = null
                });
            }
        }

        [HttpPost(AuthRoutes.Register)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            try
            {
                if (model is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status401Unauthorized,
                    hasError = true,
                    message = "Invalid authentication request",
                    data = null
                });
                var userExists = await _userManager.FindByNameAsync(model.username);
                if (userExists is not null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status401Unauthorized,
                    hasError = true,
                    message = "User already exists",
                    data = null
                });

                ApplicationUser user = new()
                {
                    Email = model.email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.username,
                    FullName = model.fullName
                };
                if (!await _roleManager.RoleExistsAsync(AppConstant.PublicUserRole))
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppConstant.PublicUserRole));
                }               

                var result = await _userManager.CreateAsync(user, model.password);
                if (result.Succeeded != true) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status401Unauthorized,
                    hasError = true,
                    message = "User creation failed. Please check user details and try again",
                    data = null
                });

                await _userManager.AddToRoleAsync(user, AppConstant.PublicUserRole);

                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status200OK,
                    hasError = false,
                    message = "User created successfully",
                    data = null
                });
            }
            catch (Exception)
            {
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
}
