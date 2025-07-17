using MobileFinance.Application.Token;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.RefreshToken;
using MobileFinance.Domain.Security.Tokens;
using MobileFinance.Domain.ValueObjects;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.Application.UseCases.Token.RefreshToken;
public class UseRefreshTokenUseCase : IUseRefreshTokenUseCase
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public UseRefreshTokenUseCase(
        ITokenRepository tokenRepository,
        IUnitOfWork unitOfWork,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<ResponseTokensJson> Execute(RequestNewTokenJson request)
    {
        var refreshToken = await _tokenRepository.Get(request.RefreshToken)
            ?? throw new RefreshTokenNotFoundException();

        var refreshTokenValidUntil = refreshToken.CreatedOn.AddDays(MobileFinanceRuleConstants.REFRESH_TOKEN_VALIDITY_IN_DAYS);

        if(DateTime.Compare(refreshTokenValidUntil, DateTime.UtcNow) < 0)
            throw new RefreshTokenExpiredException();

        var newRefreshToken = new Domain.Entities.RefreshToken
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = refreshToken.UserId
        };

        await _tokenRepository.SaveNewRefreshToken(newRefreshToken);

        await _unitOfWork.Commit();

        return new ResponseTokensJson
        {
            AccessToken = _accessTokenGenerator.Generate(refreshToken.User.UserIdentifier),
            RefreshToken = newRefreshToken.Value
        };
    }
}
