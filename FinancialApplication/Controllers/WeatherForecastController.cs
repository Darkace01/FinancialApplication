using FinancialApplication.DTO;
using FinancialApplication.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace FinancialApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IJWTHelper _jWTHelper;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IJWTHelper jWTHelper)
    {
        _logger = logger;
        _jWTHelper = jWTHelper;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
        ApplicationUser user = new();
        user.UserName = "test";
        List<string> roles = new();
        roles.Add("Admin");

        var token = _jWTHelper.GenerateToken(user, roles);
        return StatusCode(StatusCodes.Status200OK, new ApiResponse()
        {
            statusCode = StatusCodes.Status200OK,
            hasError = false,
            message = "Authrorized",
            data = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            }
        });
    }
}