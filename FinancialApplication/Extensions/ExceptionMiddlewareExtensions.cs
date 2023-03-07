using FinancialApplication.DTO;
using System.Text.Json;

namespace FinancialApplication.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger, IConfiguration configuration)
    {
        _ = app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                var config = context.Features.Get<IConfiguration>();
                var loggedInUser = string.IsNullOrWhiteSpace(context.Request.HttpContext.User.Identity.Name) ? "Anonymous" : context.Request.HttpContext.User.Identity.Name;
                if (contextFeature != null)
                {
                    logger.LogError(contextFeature.Error, $"{contextFeature.Endpoint} Something went wrong: {contextFeature.Error}");
                    var response = JsonSerializer.Serialize(new ApiResponse<string>()
                    {
                        statusCode = context.Response.StatusCode,
                        message = $"Internal Server Error: {contextFeature.Error}",
                        hasError = true,
                    });
                    await context.Response.WriteAsync(response);
                }
            });
        });
    }
}
