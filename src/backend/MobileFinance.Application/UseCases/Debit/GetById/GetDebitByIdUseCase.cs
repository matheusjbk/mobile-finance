using Mapster;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Repositories.Debit;
using MobileFinance.Domain.Services.LoggedUser;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.Application.UseCases.Debit.GetById;
public class GetDebitByIdUseCase : IGetDebitByIdUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IDebitReadOnlyRepository _repository;

    public GetDebitByIdUseCase(ILoggedUser loggedUser, IDebitReadOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }

    public async Task<ResponseDebitJson> Execute(long debitId)
    {
        var loggedUser = await _loggedUser.GetUser();

        var debit = await _repository.GetById(loggedUser, debitId)
            ?? throw new NotFoundException(ExceptionMessages.DEBIT_NOT_FOUND);

        return debit.Adapt<ResponseDebitJson>();
    }
}
