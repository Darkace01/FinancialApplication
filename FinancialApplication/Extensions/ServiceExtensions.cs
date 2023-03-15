using CloudinaryDotNet;
using FinancialApplication.Service.Contract;
using FinancialApplication.Service.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace FinancialApplication.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// Configure Dependency Injection
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureRepository(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryServiceManager, RepositoryServiceManager>();
        services.AddScoped<IJWTHelper, JWTHelper>();
        services.AddScoped<IEmailTemplateHelper, EmailTemplateHelper>();
    }

    /// <summary>
    /// Configure JWT Authentication
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void ConfigureAuthenticationWithJWT(this IServiceCollection services, IConfiguration configuration) => services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        // Adding Jwt Bearer  
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
            };
        });

    /// <summary>
    /// Configure Identity
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureIdentity(this IServiceCollection services) => services.AddIdentity<ApplicationUser, IdentityRole>(
                                options =>
                                {
                                    options.User.RequireUniqueEmail = true;
                                    options.Password.RequiredLength = 6;
                                })
                                .AddEntityFrameworkStores<FinancialApplicationDbContext>()
                                .AddDefaultTokenProviders();

    /// <summary>
    /// Configure API Versioning
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureVersioning(this IServiceCollection services) => services.AddApiVersioning(options =>
                                {
                                    options.ReportApiVersions = true;
                                    options.AssumeDefaultVersionWhenUnspecified = true;
                                    options.DefaultApiVersion = new ApiVersion(1, 0);
                                });
    /// <summary>
    /// Configure Swagger
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Financial Application",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Name = "Kazeem Quadri",
                    Email = "quadrikazeem01@gmail.com",
                    Url = new Uri("https://kaz.com.ng"),
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://github.com/DarkAce01/financialapplication/blob/main/LICENSE"),
                }
            });

            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
            });
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            swagger.OperationFilter<SwaggerFileOperationFilter>();
        });
    }

    /// <summary>
    /// Configure External Authentication. Currently only Google Authentication
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void ConfigureExternalAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication().AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
        });
    }
    /// <summary>
    /// Configure cloudinary for image upload
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void ConfigureCloudinary(this IServiceCollection services, IConfiguration configuration)
    {
        var cloudName = configuration["Cloudinary:CloudName"];
        var apiKey = configuration["Cloudinary:ApiKey"];
        var apiSecret = configuration["Cloudinary:ApiSecret"];
        Console.WriteLine(cloudName ?? "n");
        Console.WriteLine(apiKey ?? "k");
        Console.WriteLine(apiSecret ?? "s");

        if (new[] { cloudName, apiKey, apiSecret }.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("Please specify Cloudinary account details!");
        }

        services.AddSingleton(new Cloudinary(new Account(cloudName, apiKey, apiSecret)));
    }
}
