using Mapster;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.Debit;
using MobileFinance.Domain.Services.LoggedUser;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.Application.UseCases.Debit.Register;
public class RegisterDebitUseCase : IRegisterDebitUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IDebitWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterDebitUseCase(
        ILoggedUser loggedUser,
        IDebitWriteOnlyRepository repository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisteredDebitJson> Execute(RequestDebitJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.GetUser();

        var debit = request.Adapt<Domain.Entities.Debit>();
        debit.UserId = loggedUser.Id;

        await _repository.Add(debit);
        await _unitOfWork.Commit();

        return debit.Adapt<ResponseRegisteredDebitJson>();
    }

    private static void Validate(RequestDebitJson request)
    {
        var validationResult = new DebitValidator().Validate(request);

        if(!validationResult.IsValid)
            throw new ErrorOnValidationException([.. validationResult.Errors.Select(e => e.ErrorMessage)]);
    }
}
