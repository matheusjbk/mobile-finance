using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.RefreshToken;
using MobileFinance.Domain.Repositories.User;
using MobileFinance.Domain.Security.Cryptography;
using MobileFinance.Domain.Security.Tokens;
using MobileFinance.Domain.Services;
using MobileFinance.Infra.DataAccess;
using MobileFinance.Infra.DataAccess.Repositories;
using MobileFinance.Infra.Extensions;
using MobileFinance.Infra.Security.Cryptography;
using MobileFinance.Infra.Security.Tokens.AccessToken.Generator;
using MobileFinance.Infra.Security.Tokens.AccessToken.Validator;
using MobileFinance.Infra.Security.Tokens.RefreshToken;
using MobileFinance.Infra.Services.LoggedUser;
using System.Reflection;

namespace MobileFinance.Infra;
public static class DependencyInjectionExtension
{
    public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddPasswordEncrypter(services);
        AddTokens(services, configuration);
        AddLoggedUser(services);

        if(configuration.IsTestEnvironment())
            return;

        AddDbContext_MySql(services, configuration);
        AddFluentMigrator_MySql(services, configuration);
    }

    private static void AddDbContext_MySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        services.AddDbContext<MobileFinanceDbContext>(options =>
        {
            options.UseMySql(connectionString, serverVersion);
        });
    }

    private static void AddFluentMigrator_MySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore()
            .ConfigureRunner(runner =>
            {
                runner
                .AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("MobileFinance.Infra")).For.All();
            });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddPasswordEncrypter(IServiceCollection services) => services.AddScoped<IPasswordEncrypter, BCryptNet>();

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTime = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeInMinutes")!;
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey")!;

        services.AddScoped<IAccessTokenGenerator>(provider => new JwtGenerator(expirationTime, signingKey));
        services.AddScoped<IAccessTokenValidator>(provider => new JwtValidator(signingKey));
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();
}
