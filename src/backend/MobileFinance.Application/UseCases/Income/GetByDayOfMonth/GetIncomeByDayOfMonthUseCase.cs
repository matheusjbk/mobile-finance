using Mapster;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Repositories.Income;
using MobileFinance.Domain.Services.LoggedUser;

namespace MobileFinance.Application.UseCases.Income.GetByDayOfMonth;
public class GetIncomeByDayOfMonthUseCase : IGetIncomeByDayOfMonthUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IIncomeReadOnlyRepository _repository;

    public GetIncomeByDayOfMonthUseCase(ILoggedUser loggedUser, IIncomeReadOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }

    public async Task<ResponseIncomesJson> Execute(RequestFilterIncomesJson request)
    {
        var loggedUser = await _loggedUser.GetUser();

        var incomes = await _repository.GetByDayOfMonth(loggedUser, request.FilterDate);

        return new ResponseIncomesJson
        {
            Incomes = incomes.Adapt<IList<ResponseShortIncomeJson>>()
        };
    }
}
