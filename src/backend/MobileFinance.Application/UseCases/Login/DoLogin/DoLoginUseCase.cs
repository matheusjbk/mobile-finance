using MobileFinance.Application.Token;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.RefreshToken;
using MobileFinance.Domain.Repositories.User;
using MobileFinance.Domain.Security.Cryptography;
using MobileFinance.Domain.Security.Tokens;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.Application.UseCases.Login.DoLogin;
public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DoLoginUseCase(
        IUserReadOnlyRepository repository,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        ITokenRepository tokenRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _passwordEncrypter = passwordEncrypter;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _repository.GetByEmail(request.Email);

        if(user is null || !_passwordEncrypter.IsValid(request.Password, user.Password))
            throw new InvalidLoginException();

        var refreshTokenGeneration = new RefreshTokenGeneration(_refreshTokenGenerator, _tokenRepository, _unitOfWork);
        var refreshToken = await refreshTokenGeneration.GenerateAndSaveRefreshToken(user);

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier),
                RefreshToken = refreshToken
            }
        };
    }
}
