using FluentValidation.Results;
using MobileFinance.Communication.Requests;
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.User;
using MobileFinance.Domain.Security.Cryptography;
using MobileFinance.Domain.Services;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.Application.UseCases.User.ChangePassword;
public class ChangeUserPasswordUseCase : IChangeUserPasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeUserPasswordUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository repository,
        IPasswordEncrypter passwordEncrypter,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _passwordEncrypter = passwordEncrypter;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        // Entity rescued w/ AsNoTracking(), so, is not recommended to update through it.
        var loggedUser = await _loggedUser.GetUser();

        Validate(request, loggedUser);

        // Entity to update. Rescued w/o AsNoTracking().
        var user = await _repository.GetById(loggedUser.Id);

        user.Password = _passwordEncrypter.Encrypt(request.NewPassword);

        _repository.Update(user);
        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User user)
    {
        var validationResult = new ChangeUserPasswordValidator().Validate(request);

        if(!_passwordEncrypter.IsValid(request.CurrentPassword, user.Password))
            validationResult.Errors.Add(new ValidationFailure(string.Empty, ExceptionMessages.DIFFERENT_CURRENT_PASSWORD));

        if(!validationResult.IsValid)
            throw new ErrorOnValidationException([..validationResult.Errors.Select(e => e.ErrorMessage)]);
    }
}
