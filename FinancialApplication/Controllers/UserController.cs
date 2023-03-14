using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
