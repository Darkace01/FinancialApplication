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
        private readonly ILogger<AuthController> _logger;

        public AuthController(IJWTHelper jWTHelper, IRepositoryServiceManager repo, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AuthController> logger)
        {
            _jWTHelper = jWTHelper;
            _repo = repo;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpPost(AuthRoutes.Login)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
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
                var user = await _userManager.FindByNameAsync(model.username);
                user ??= await _userManager.FindByEmailAsync(model.username);
                if (user == null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "Invalid username or password",
                    data = null
                });

                var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.password);
                if (isPasswordValid != true) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
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
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        phoneNumber = user.PhoneNumber,
                        fullName = $"{user.FirstName} {user.LastName}",
                        userId = user.Id
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login");
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
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
                var userExists = await _userManager.FindByNameAsync(model.username);
                userExists ??= await _userManager.FindByEmailAsync(model.username);
                if (userExists is not null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "User already exists",
                    data = null
                });

                ApplicationUser user = new()
                {
                    Email = model.email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.username,
                    FirstName = model.firstName,
                    LastName = model.lastName,
                    PhoneNumber = model.phoneNumber,
                };
                if (!await _roleManager.RoleExistsAsync(AppConstant.PublicUserRole))
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppConstant.PublicUserRole));
                }

                var result = await _userManager.CreateAsync(user, model.password);
                if (result.Succeeded != true) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    hasError = true,
                    message = "An error occured while processing your request",
                    data = null
                });
            }
        }

        [HttpPost(AuthRoutes.ChangePassword)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
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

                var userExists = await _userManager.FindByNameAsync(model.username);
                if (userExists is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "User doesn't exists",
                    data = null
                });

                if (string.Compare(model.currentPassword, model.newPassword) == 0) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "Both passwords are the same",
                    data = null
                });

                var isPasswordValid = await _userManager.CheckPasswordAsync(userExists, model.currentPassword);
                if (isPasswordValid != true) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "Invalid current password",
                    data = null
                });

                var result = await _userManager.ChangePasswordAsync(userExists, model.currentPassword, model.newPassword);
                if (result.Succeeded != true) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "Password change failed. Please check user details and try again",
                    data = null
                });

                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status200OK,
                    hasError = false,
                    message = "Password changed successfully",
                    data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangePassword");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    hasError = true,
                    message = "An error occured while processing your request",
                    data = null
                });
            }
        }

        [HttpPost(AuthRoutes.ResetPasswordRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RequestPasswordResetCode(PasswordRequestDTO model)
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

                var userExist = await _userManager.FindByEmailAsync(model.email);
                if (userExist is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "User doesn't exists",
                    data = null
                });

                var code = await _userManager.GeneratePasswordResetTokenAsync(userExist);
                // Send Code later
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status200OK,
                    hasError = false,
                    message = "Password reset code sent successfully",
                    data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError( ex, "RequestPasswordResetCode");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    hasError = true,
                    message = "An error occured while processing your request",
                    data = null
                });
            }
        }

        [HttpPost(AuthRoutes.ResetPassword)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPassword(PasswordRequestCodeDTO model)
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

                var userExist = await _userManager.FindByEmailAsync(model.email);
                if (userExist is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "User doesn't exists",
                    data = null
                });

                var resetPassword = await _userManager.ResetPasswordAsync(userExist, model.code, model.password);
                if (resetPassword.Succeeded != true) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "Password reset failed. Please check user details and try again",
                    data = null
                });

                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status200OK,
                    hasError = false,
                    message = "Password reset successfully",
                    data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError( ex, "ResetPassword");
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
