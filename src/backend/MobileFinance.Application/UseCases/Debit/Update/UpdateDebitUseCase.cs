using Mapster;
using MobileFinance.Communication.Requests;
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.Debit;
using MobileFinance.Domain.Services.LoggedUser;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.Application.UseCases.Debit.Update;
public class UpdateDebitUseCase : IUpdateDebitUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IDebitUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDebitUseCase(
        ILoggedUser loggedUser,
        IDebitUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestDebitJson request, long debitId)
    {
        Validate(request);

        var loggedUser = await _loggedUser.GetUser();

        var debit = await _repository.GetById(loggedUser, debitId)
            ?? throw new NotFoundException(ExceptionMessages.DEBIT_NOT_FOUND);

        request.Adapt(debit);

        _repository.Update(debit);

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestDebitJson request)
    {
        var validationResult = new DebitValidator().Validate(request);

        if(!validationResult.IsValid)
            throw new ErrorOnValidationException([..validationResult.Errors.Select(e => e.ErrorMessage)]);
    }
}
