using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialApplication.Controllers;

[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/notifications")]
[ApiController]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly IRepositoryServiceManager _repo;
    private readonly UserManager<ApplicationUser> _userManager;

    public NotificationController(UserManager<ApplicationUser> userManager, IRepositoryServiceManager repo)
    {
        _userManager = userManager;
        _repo = repo;
    }

    [HttpPost(NotificationRoutes._saveUserNotificationToken)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SaveUserNotificationToken(SaveUserNotificationTokenDTO model)
    {
        if (model == null) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status400BadRequest,
            hasError = true,
            message = "Invalid payload",
            data = null
        });

        if (string.IsNullOrWhiteSpace(model.token)) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status400BadRequest,
            hasError = true,
            message = "Invalid payload",
            data = null
        });

        var user = await GetUser();
        var isSaved = await _repo.NotificationService.UpdateUserNotificationToken(user.Id, model.token);

        if (isSaved == false) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status400BadRequest,
            hasError = true,
            message = "Unable to save token. Please try again later",
            data = null
        });

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = false,
            message = "Successfully saved token.",
            data = null
        });
    }

    [HttpPost(NotificationRoutes._turnOffNotification)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> TurnOffNotificationForUser()
    {
        var user = await GetUser();
        var isSaved = await _repo.NotificationService.TurnOffReceivingNotificationForUser(user.Id);

        if (isSaved == false) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status400BadRequest,
            hasError = true,
            message = "Unable to turn this feature off at the moment. Please try again later",
            data = null
        });

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = false,
            message = "Turned off successfully.",
            data = null
        });
    }

    [HttpPost(NotificationRoutes._turnOnNotification)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> TurnOnNotificationForUser()
    {
        var user = await GetUser();
        var isSaved = await _repo.NotificationService.TurnOnReceivingNotificationForUser(user.Id);

        if (isSaved == false) return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status400BadRequest,
            hasError = true,
            message = "Unable to turn this feature on at the moment. Please try again later",
            data = null
        });

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = false,
            message = "Turned on successfully.",
            data = null
        });
    }

    [HttpGet(NotificationRoutes._sendTestNotificationToAllUsers)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SendTestNotificationToAllUsers()
    {
        PushNotificationHelper pushNotificationHelper = new(_repo);
        var response = await pushNotificationHelper.SendUsersPushNotification();
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = false,
            message = "Successfully sent notification",
            data = response
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
