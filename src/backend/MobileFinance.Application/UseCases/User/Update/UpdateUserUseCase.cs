using FluentValidation.Results;
using MobileFinance.Communication.Requests;
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.User;
using MobileFinance.Domain.Services;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.Application.UseCases.User.Update;
public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserReadOnlyRepository _readOnlyRespository;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserUseCase(
        ILoggedUser loggedUser,
        IUserReadOnlyRepository readOnlyRespository,
        IUserUpdateOnlyRepository updateOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _readOnlyRespository = readOnlyRespository;
        _updateOnlyRepository = updateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestUpdateUserJson request)
    {
        // Entity rescued w/ AsNoTracking(), so, is not recommended to update through it.
        var loggedUser = await _loggedUser.GetUser();

        await Validate(request, loggedUser.Email);

        // Entity to update. Rescued w/o AsNoTracking().
        var user = await _updateOnlyRepository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        _updateOnlyRepository.Update(user);
        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validationResult = new UpdateUserValidator().Validate(request);

        if(!currentEmail.Equals(request.Email))
        {
            var emailExist = await _readOnlyRespository.ExistActiveUserWithEmail(request.Email);

            if(emailExist)
                validationResult.Errors.Add(new ValidationFailure(string.Empty, ExceptionMessages.EMAIL_ALREADY_REGISTERED));
        }

        if(!validationResult.IsValid)
            throw new ErrorOnValidationException([..validationResult.Errors.Select(e => e.ErrorMessage)]);
    }
}
