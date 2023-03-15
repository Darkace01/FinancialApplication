namespace FinancialApplication.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiversion}/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryServiceManager _repo;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager, ILogger<UserController> logger, IRepositoryServiceManager repo)
        {
            _userManager = userManager;
            _logger = logger;
            _repo = repo;
        }

        [HttpPost(UserRoutes._uploadUserProfilePicture)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<FileStorageDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadUserProfilePicture([FromForm] IFormFile file)
        {
            if(file == null || file.Length == 0)
            {
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = "No file selected",
                    data = null,
                    hasError = true
                });
            }
            var user = await GetUser();

            if (user.ProfilePictureId != null)
            {
                var deleteResult = await _repo.FileStorageService.DeleteFile(user.ProfilePictureId);
                if (!deleteResult)
                {
                    return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                    {
                        statusCode = StatusCodes.Status500InternalServerError,
                        message = "Error deleting old profile picture",
                        data = null,
                        hasError = true
                    });
                }
            }
            var fileStorage = await _repo.FileStorageService.SaveFile(file, "user_profile_picture");
            user.ProfilePictureUrl = fileStorage.FileUrl;
            user.ProfilePictureId = fileStorage.PublicId;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    message = "Error uploading profile picture",
                    data = null,
                    hasError = true
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ApiResponse<FileStorageDTO>()
            {
                statusCode = StatusCodes.Status200OK,
                message = "Profile picture uploaded successfully",
                hasError = false,
                data = fileStorage
            });
        }

        [HttpPost(UserRoutes._userBasicDetails)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<UserBasicDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserBasicDetails()
        {
            var user = await GetUser();

            if (user == null)return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = "Invalid user",
                    hasError = true
                });

            UserBasicDetail userDetail = new()
            {
                emailAddress = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName,
                phoneNumber = user.PhoneNumber,
                ProfilePictureId = user.ProfilePictureId,
                profilePictureUrl = user.ProfilePictureUrl,
                userId = user.Id
            };

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<UserBasicDetail>()
            {
                statusCode = StatusCodes.Status200OK,
                message = "User details",
                hasError = false,
                data = userDetail
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
}
