using FinancialApplication.Commons;

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
        private readonly IEmailTemplateHelper _emailTemplate;

        public AuthController(IJWTHelper jWTHelper, IRepositoryServiceManager repo, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AuthController> logger, IEmailTemplateHelper emailTemplate)
        {
            _jWTHelper = jWTHelper;
            _repo = repo;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailTemplate = emailTemplate;
        }

        [HttpPost(AuthRoutes.Login)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
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

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (isEmailConfirmed != true) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status400BadRequest,
                hasError = true,
                message = "Email not confirmed",
                data = null
            });

            var userRoles = await _userManager.GetRolesAsync(user);
            var token = _jWTHelper.GenerateToken(user, userRoles);
            var clientBalance = await _repo.TransactionService.GetUserBalanceForTheMonth(user.Id, DateTime.Now);

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
                    profilePictureUrl = user.ProfilePictureUrl,
                    userId = user.Id,
                    ClientBalance = clientBalance,
                    ProfilePictureId = user.ProfilePictureId,
                }
            });
        }

        [HttpPost(AuthRoutes.Register)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (model is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status400BadRequest,
                hasError = true,
                message = "Invalid authentication request",
                data = null
            });

            if (!ModelState.IsValid) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
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
                EmailConfirmed = false
            };
            if (!await _roleManager.RoleExistsAsync(AppConstant.PublicUserRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(AppConstant.PublicUserRole));
            }

            var result = await _userManager.CreateAsync(user, model.password);
            if (result.Succeeded != true)
            {
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = $"User creation failed. {result?.Errors?.FirstOrDefault().Description}",
                    data = null
                });
            }

            var code = await _repo.UserService.GenerateUserConfirmationCode(user.Id);
            var emailBody = _emailTemplate.BuildEmailConfirmationTemplate(user.FirstName, code);

            var mailSent = await _repo.EmailService.SendEmailAsync(user.Email, "Account Confirmation", emailBody);

            if (mailSent != true)
            {
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "User created successfully but email confirmation failed",
                    data = null
                });
            }

            await _userManager.AddToRoleAsync(user, AppConstant.PublicUserRole);

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = false,
                message = "User created successfully",
                data = null
            });
        }

        [HttpPost(AuthRoutes.ChangePassword)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
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
            if (result.Succeeded == false)
            {
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "Password change failed. Please check user details and try again",
                    data = null
                });
            }

            var mailBody = _emailTemplate.BuildPasswordResetConfirmationTemplate(userExists.FirstName);
            var mailSent = await _repo.EmailService.SendEmailAsync(userExists.Email, "Password Change Confirmation", mailBody);
            if (mailSent != true)
            {
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "Password changed successfully but email confirmation failed",
                    data = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = false,
                message = "Password changed successfully",
                data = null
            });
        }

        [HttpPost(AuthRoutes.ResetPasswordRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RequestPasswordResetCode(PasswordRequestDTO model)
        {
            if (model is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status400BadRequest,
                hasError = true,
                message = "Invalid authentication request",
                data = null
            });

            var userExist = await _userManager.FindByEmailAsync(model.email);
            // Ensure the api is not used to test out multiple users
            if (userExist is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = false,
                message = "Password reset code sent successfully",
                data = null
            });

            var code = await _repo.UserService.GenerateUserConfirmationCode(userExist.Id);
            var emailBody = _emailTemplate.BuildPasswordResetTemplate(userExist.FirstName, code);
            var mailSent = await _repo.EmailService.SendEmailAsync(userExist.Email, "Password Reset", emailBody);
            if (mailSent != true)
            {
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "Password reset code sent successfully but email confirmation failed",
                    data = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = false,
                message = "Password reset code sent successfully",
                data = null
            });
        }

        [HttpPost(AuthRoutes.ResetPassword)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPassword(PasswordRequestCodeDTO model)
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

            var isValidCode = await _repo.UserService.VerifyUserConfirmationCode(userExist.Id, model.code);

            if (isValidCode == false)
            {
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "Invalid Code",
                    data = null
                });
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(userExist);
            var resetPassword = await _userManager.ResetPasswordAsync(userExist, token, model.password);
            if (resetPassword.Succeeded != true) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status400BadRequest,
                hasError = true,
                message = "Password reset failed. Please check user details and try again",
                data = null
            });

            var mailBody = _emailTemplate.BuildPasswordResetConfirmationTemplate(userExist.FirstName);
            var mailSent = await _repo.EmailService.SendEmailAsync(userExist.Email, "Password Reset Confirmation", mailBody);
            if (mailSent != true)
            {
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "Password reset successfully but email confirmation failed",
                    data = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = false,
                message = "Password reset successfully",
                data = null
            });
        }

        [HttpPost(AuthRoutes.ResendConfirmationEmail)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResendEmailConfirmationCode([FromBody] RequestEmailConfirmationDTO model)
        {
            if(model == null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status400BadRequest,
                hasError = true,
                message = "Invalid authentication request",
                data = null
            });

            var userExist = await _userManager.FindByEmailAsync(model.username);
            userExist ??= await _userManager.FindByNameAsync(model.username);
            if (userExist is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status400BadRequest,
                hasError = true,
                message = "User doesn't exists",
                data = null
            });

            var code = await _repo.UserService.GenerateUserConfirmationCode(userExist.Id);
            var emailBody = _emailTemplate.BuildEmailConfirmationTemplate(userExist.FirstName, code);
            var mailSent = await _repo.EmailService.SendEmailAsync(userExist.Email, "Email Confirmation", emailBody);
            if (mailSent != true)
            {
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    hasError = true,
                    message = "Email confirmation code sent successfully but email confirmation failed",
                    data = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = false,
                message = "Email confirmation code sent successfully",
                data = null
            });
        }

        [HttpPost(AuthRoutes.ConfirmEmail)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> VerifyEmailConfirmationCode([FromBody] EmailConfirmationDTO model)
        {
            if(model  == null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status400BadRequest,
                hasError = true,
                message = "Invalid payload",
                data = null
            });

            if (string.IsNullOrWhiteSpace(model.username) || string.IsNullOrWhiteSpace(model.code)) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status400BadRequest,
                hasError = true,
                message = "Invalid payload",
                data = null
            });
            var userExist = await _userManager.FindByEmailAsync(model.username);
            userExist ??= await _userManager.FindByNameAsync(model.username);
            if (userExist is null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status400BadRequest,
                hasError = true,
                message = "User doesn't exists",
                data = null
            });

            var isCodeValid = await _repo.UserService.VerifyUserEmail(userExist.Id, model.code);
            if(isCodeValid == false) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status400BadRequest,
                hasError = true,
                message = "Invalid code.",
                data = null
            });

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
            {
                statusCode = StatusCodes.Status200OK,
                hasError = false,
                message = "Email Confirmed.",
                data = null
            });
        }
    }
}
