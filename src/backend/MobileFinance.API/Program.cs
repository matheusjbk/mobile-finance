using Microsoft.AspNetCore.Authentication.Cookies;
using MobileFinance.API.BackgroundServices;
using MobileFinance.API.Converters;
using MobileFinance.API.Filters;
using MobileFinance.API.Middlewares;
using MobileFinance.API.Token;
using MobileFinance.Application;
using MobileFinance.Domain.Security.Tokens;
using MobileFinance.Infra;
using MobileFinance.Infra.Extensions;
using MobileFinance.Infra.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter)))
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddInfra(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

if(!builder.Configuration.IsTestEnvironment())
{
    builder.Services.AddHostedService<DeleteUserService>();
    AddGoogleAuthentication();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

await app.RunAsync();

void MigrateDatabase()
{
    if(builder.Configuration.IsTestEnvironment()) return;

    var connectionString = builder.Configuration.ConnectionString();

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DatabaseMigration.Migrate(connectionString, serviceScope.ServiceProvider);
}

void AddGoogleAuthentication()
{
    var clientId = builder.Configuration.GetValue<string>("Settings:Google:ClientId")!;
    var clientSecret = builder.Configuration.GetValue<string>("Settings:Google:ClientSecret")!;

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    }).AddCookie()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = clientId;
        googleOptions.ClientSecret = clientSecret;
    });
}

// This class is required for the WebApplicationFactory to work correctly in tests.
public partial class Program 
{
    protected Program() { }
}