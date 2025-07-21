using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MobileFinance.Application.UseCases.User.Register;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.Register;
public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase(refreshToken);

        var response = await useCase.Execute(request);

        response.ShouldNotBeNull();
        response.Name.ShouldBe(request.Name);
        response.Tokens.ShouldNotBeNull();
        response.Tokens.AccessToken.ShouldNotBeNullOrEmpty();
        response.Tokens.RefreshToken.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        (var user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase(refreshToken, request.Email);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.EMAIL_ALREADY_REGISTERED);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        (var user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        var useCase = CreateUseCase(refreshToken);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.EMPTY_NAME);
    }

    private static RegisterUserUseCase CreateUseCase(MobileFinance.Domain.Entities.RefreshToken refreshToken, string? email = null)
    {
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordEncrypter = PasswordEncrypterBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var accessTokenGenerator = AccessTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
        var tokenRepository = new TokenRepositoryBuilder().Get(refreshToken).Build();

        if(email is not null)
            userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(email);

        return new RegisterUserUseCase(
            userReadOnlyRepositoryBuilder.Build(),
            userWriteOnlyRepository,
            passwordEncrypter,
            unitOfWork,
            accessTokenGenerator,
            refreshTokenGenerator,
            tokenRepository);
    }
}
