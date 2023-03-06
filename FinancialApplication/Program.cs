using Azure.Identity;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FinancialApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Identity Dependency Injection
builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
//Identity
builder.Services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
builder.Services.ConfigureRepository();
builder.Services.ConfigureIdentity();

//JWT
builder.Services.ConfigureAuthenticationWithJWT(builder.Configuration);

builder.Services.AddHealthChecks();

//Version
builder.Services.ConfigureVersioning();

var app = builder.Build();

//CORS
app.UseCors(x => x
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .SetIsOriginAllowed(origin => true)
                 .AllowCredentials());

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//}
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinancialApplication v1"));

app.ConfigureExceptionHandler(app.Logger, app.Configuration);

app.UseHealthChecks("/app/health");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
