using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Enums;
using MobileFinance.Infra.DataAccess;

namespace WebApi.Test;
// Configures the web host to use an in-memory database and seed test data for integration tests.
public class MobileFinanceWebApplicationFactory : WebApplicationFactory<Program>
{
    private MobileFinance.Domain.Entities.User _user = default!;
    private string _password = string.Empty;
    private RefreshToken _refreshToken = default!;
    private MobileFinance.Domain.Entities.Income _income = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MobileFinanceDbContext>));

                if(descriptor is not null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<MobileFinanceDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                    options.UseInternalServiceProvider(provider);
                });

                using var scope = services.BuildServiceProvider().CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<MobileFinanceDbContext>();

                StartDatabase(dbContext);
            });
    }

    public string GetUserName() => _user.Name;
    public string GetUserEmail() => _user.Email;
    public Guid GetUserIdentifier() => _user.UserIdentifier;
    public string GetUserPassword() => _password;

    public RefreshToken GetRefreshToken() => _refreshToken;

    public string GetIncomeId() => IdEncoderBuilder.Build().Encode(_income.Id);
    public string GetIncomeTitle() => _income.Title;
    public IncomeType GetIncomeType() => _income.IncomeType;

    private void StartDatabase(MobileFinanceDbContext dbContext)
    {
        (_user, _password) = UserBuilder.Build();
        _refreshToken = RefreshTokenBuilder.Build(_user);
        _income = IncomeBuilder.Build(_user);


        dbContext.Database.EnsureDeleted();

        dbContext.Users.Add(_user);
        dbContext.RefreshTokens.Add(_refreshToken);
        dbContext.Incomes.Add(_income);

        dbContext.SaveChanges();
    }
}
