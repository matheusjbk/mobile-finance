using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Tokens;
using MobileFinance.Application.UseCases.Login.External;
using Shouldly;

namespace UseCases.Test.Login.External;
public class ExternalLoginUseCaseTest
{
    [Fact]
    public async Task Success_User_Exists()
    {
        (var user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var token = await useCase.Execute(user.Name, user.Email);

        token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Success_User_Dont_Exists()
    {
        (var user, _) = UserBuilder.Build();
        var useCase = CreateUseCase();

        var token = await useCase.Execute(user.Name, user.Email);

        token.ShouldNotBeNullOrWhiteSpace();
    }

    private static ExternalLoginUseCase CreateUseCase(MobileFinance.Domain.Entities.User? user = null)
    {
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder().GetByEmail(user).Build();
        var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var accessTokenGenerator = AccessTokenGeneratorBuilder.Build();

        return new ExternalLoginUseCase(
            readOnlyRepository,
            writeOnlyRepository,
            unitOfWork,
            accessTokenGenerator);
    }
}
