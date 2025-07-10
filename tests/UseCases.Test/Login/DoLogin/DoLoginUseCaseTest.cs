using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MobileFinance.Application.UseCases.Login.DoLogin;
using MobileFinance.Communication.Requests;
using MobileFinance.Domain.Entities;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Login.DoLogin;
public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var password) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);
        var useCase = CreateUseCase(refreshToken, user);

        var response = await useCase.Execute(new RequestLoginJson
        {
            Email = user.Email,
            Password = password
        });

        response.ShouldNotBeNull();
        response.Name.ShouldBe(user.Name);
        response.Tokens.ShouldNotBeNull();
        response.Tokens.AccessToken.ShouldNotBeNullOrEmpty();
        response.Tokens.RefreshToken.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_Invalid_User()
    {
        (var user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);
        var request = RequestLoginJsonBuilder.Build();
        var useCase = CreateUseCase(refreshToken);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<InvalidLoginException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.INVALID_EMAIL_OR_PASSWORD);
    }

    private static DoLoginUseCase CreateUseCase(RefreshToken refreshToken, MobileFinance.Domain.Entities.User? user = null)
    {
        var repository = new UserReadOnlyRepositoryBuilder().GetByEmail(user).Build();
        var passwordEncrypter = PasswordEncrypterBuilder.Build();
        var accessTokenGenerator = AccessTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
        var tokenRepository = new TokenRepositoryBuilder().Get(refreshToken).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new DoLoginUseCase(
            repository,
            passwordEncrypter,
            accessTokenGenerator,
            refreshTokenGenerator,
            tokenRepository,
            unitOfWork);
    }
}
