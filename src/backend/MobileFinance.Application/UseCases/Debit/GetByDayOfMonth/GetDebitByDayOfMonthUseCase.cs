using Mapster;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Repositories.Debit;
using MobileFinance.Domain.Services.LoggedUser;

namespace MobileFinance.Application.UseCases.Debit.GetByDayOfMonth;
public class GetDebitByDayOfMonthUseCase : IGetDebitByDayOfMonthUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IDebitReadOnlyRepository _repository;

    public GetDebitByDayOfMonthUseCase(ILoggedUser loggedUser, IDebitReadOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }

    public async Task<ResponseDebitsJson> Execute(RequestFilterDebitsJson request)
    {
        var loggedUser = await _loggedUser.GetUser();

        var debits = await _repository.GetByDayOfMonth(loggedUser, request.FilterDate);

        return new ResponseDebitsJson
        {
            Debits = debits.Adapt<IList<ResponseShortDebitJson>>()
        };
    }
}
