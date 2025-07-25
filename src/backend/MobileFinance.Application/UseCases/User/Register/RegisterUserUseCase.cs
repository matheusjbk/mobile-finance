﻿using FluentValidation.Results;
using Mapster;
using MobileFinance.Application.Token;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.RefreshToken;
using MobileFinance.Domain.Repositories.User;
using MobileFinance.Domain.Security.Cryptography;
using MobileFinance.Domain.Security.Tokens;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.Application.UseCases.User.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITokenRepository _tokenRepository;

    public RegisterUserUseCase(
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IPasswordEncrypter passwordEncrypter,
        IUnitOfWork unitOfWork,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        ITokenRepository tokenRepository)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _passwordEncrypter = passwordEncrypter;
        _unitOfWork = unitOfWork;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _tokenRepository = tokenRepository;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = request.Adapt<Domain.Entities.User>();

        user.Password = _passwordEncrypter.Encrypt(request.Password);

        await _userWriteOnlyRepository.Add(user);
        await _unitOfWork.Commit();

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

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validationResult = new RegisterUserValidator().Validate(request);

        var emailExistsInDatabase = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

        if(emailExistsInDatabase)
            validationResult.Errors.Add(new ValidationFailure(string.Empty, ExceptionMessages.EMAIL_ALREADY_REGISTERED));

        if(!validationResult.IsValid)
            throw new ErrorOnValidationException([..validationResult.Errors.Select(e => e.ErrorMessage)]);
    }
}
