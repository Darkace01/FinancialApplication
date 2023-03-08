using FinancialApplication.Service.Contract;
using FinancialApplication.Service.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace FinancialApplication.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureRepository(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryServiceManager, RepositoryServiceManager>();
        services.AddScoped<IJWTHelper, JWTHelper>();
        services.AddScoped<IEmailTemplateHelper, EmailTemplateHelper>();
    }

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

    public static void ConfigureIdentity(this IServiceCollection services) => services.AddIdentity<ApplicationUser, IdentityRole>(
                                options =>
                                {
                                    options.User.RequireUniqueEmail = true;
                                    options.Password.RequiredLength = 6;
                                })
                                .AddEntityFrameworkStores<FinancialApplicationDbContext>()
                                .AddDefaultTokenProviders();


    public static void ConfigureVersioning(this IServiceCollection services) => services.AddApiVersioning(options =>
                                {
                                    options.ReportApiVersions = true;
                                    options.AssumeDefaultVersionWhenUnspecified = true;
                                    options.DefaultApiVersion = new ApiVersion(1, 0);
                                });
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

    public static void ConfigureExternalAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication().AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
        });
    }
}
