using MobileFinance.API.Converters;
using MobileFinance.API.Filters;
using MobileFinance.Application;
using MobileFinance.Infra;
using MobileFinance.Infra.Extensions;
using MobileFinance.Infra.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter)))
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfra(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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

// This class is required for the WebApplicationFactory to work correctly in tests.
public partial class Program 
{
    protected Program() { }
}