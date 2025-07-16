using MobileFinance.Communication.Requests;
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.User;
using MobileFinance.Domain.Security.Cryptography;
using MobileFinance.Domain.Services.LoggedUser;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.Application.UseCases.User.CreatePassword;
public class CreateUserPasswordUseCase : ICreateUserPasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncrypter _passwordEncrypter;

    public CreateUserPasswordUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IPasswordEncrypter passwordEncrypter)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _passwordEncrypter = passwordEncrypter;
    }

    public async Task Execute(RequestCreateUserPasswordJson request)
    {
        var loggedUser = await _loggedUser.GetUser();

        Validate(request);

        var user = await _repository.GetById(loggedUser.Id);

        user.Password = _passwordEncrypter.Encrypt(request.Password);

        _repository.Update(user);
        await _unitOfWork.Commit();
    }

    private static void Validate(RequestCreateUserPasswordJson request)
    {
        var validationResult = new CreateUserPasswordValidator().Validate(request);

        if(!validationResult.IsValid)
            throw new ErrorOnValidationException([..validationResult.Errors.Select(e => e.ErrorMessage)]);
    }
}
