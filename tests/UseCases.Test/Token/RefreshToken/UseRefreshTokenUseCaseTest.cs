using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Tokens;
using MobileFinance.Application.UseCases.Token.RefreshToken;
using MobileFinance.Communication.Requests;
using MobileFinance.Domain.ValueObjects;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Token.RefreshToken;
public class UseRefreshTokenUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);
        var request = new RequestNewTokenJson
        {
            RefreshToken = refreshToken.Value
        };
        var useCase = CreateUseCase(refreshToken);

        var response = await useCase.Execute(request);

        response.ShouldNotBeNull();
        response.AccessToken.ShouldNotBeNullOrWhiteSpace();
        response.RefreshToken.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_RefreshToken_NotFound()
    {
        (var user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);
        var request = new RequestNewTokenJson
        {
            RefreshToken = refreshToken.Value
        };
        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<RefreshTokenNotFoundException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.INVALID_SESSION);
    }

    [Fact]
    public async Task Error_Expired_RefreshToken()
    {
        (var user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);
        refreshToken.CreatedOn = DateTime.UtcNow.AddDays(-MobileFinanceRuleConstants.REFRESH_TOKEN_VALIDITY_IN_DAYS - 1);
        var request = new RequestNewTokenJson
        {
            RefreshToken = refreshToken.Value
        };
        var useCase = CreateUseCase(refreshToken);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<RefreshTokenExpiredException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.EXPIRED_SESSION);
    }

    private static UseRefreshTokenUseCase CreateUseCase(MobileFinance.Domain.Entities.RefreshToken? refreshToken = null)
    {
        var tokenRepositoryBuilder = new TokenRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var accessTokenGenerator = AccessTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();

        if(refreshToken is not null)
            tokenRepositoryBuilder.Get(refreshToken);

        return new UseRefreshTokenUseCase(
            tokenRepositoryBuilder.Build(),
            unitOfWork,
            accessTokenGenerator,
            refreshTokenGenerator);
    }
}
