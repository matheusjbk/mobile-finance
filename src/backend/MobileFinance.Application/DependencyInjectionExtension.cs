using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobileFinance.Application.Services.Mapster;
using MobileFinance.Application.UseCases.Income.GetById;
using MobileFinance.Application.UseCases.Income.Register;
using MobileFinance.Application.UseCases.Login.DoLogin;
using MobileFinance.Application.UseCases.Login.External;
using MobileFinance.Application.UseCases.Token.RefreshToken;
using MobileFinance.Application.UseCases.User.ChangePassword;
using MobileFinance.Application.UseCases.User.CreatePassword;
using MobileFinance.Application.UseCases.User.Delete.Delete;
using MobileFinance.Application.UseCases.User.Delete.Request;
using MobileFinance.Application.UseCases.User.Profile;
using MobileFinance.Application.UseCases.User.Register;
using MobileFinance.Application.UseCases.User.Update;
using Sqids;

namespace MobileFinance.Application;
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddIdEncoder(services, configuration);
        AddMapster(services, configuration);
        AddUseCases(services);
    }

    private static void AddIdEncoder(IServiceCollection services, IConfiguration configuration)
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
        });

        services.AddSingleton(sqids);
    }

    private static void AddMapster(IServiceCollection services, IConfiguration configuration)
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
        });

        var mapsterConfiguration = new MapsterConfiguration(sqids);

        services.AddMapster();
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IChangeUserPasswordUseCase, ChangeUserPasswordUseCase>();
        services.AddScoped<IExternalLoginUseCase, ExternalLoginUseCase>();
        services.AddScoped<ICreateUserPasswordUseCase, CreateUserPasswordUseCase>();
        services.AddScoped<IRequestDeleteUserUseCase, RequestDeleteUserUseCase>();
        services.AddScoped<IDeleteUserAccountUseCase, DeleteUserAccountUseCase>();
        services.AddScoped<IUseRefreshTokenUseCase, UseRefreshTokenUseCase>();
        services.AddScoped<IRegisterIncomeUseCase, RegisterIncomeUseCase>();
        services.AddScoped<IGetIncomeByIdUseCase, GetIncomeByIdUseCase>();
    }
}
