using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.Application.UseCases.Income.GetByDayOfMonth;
public interface IGetIncomeByDayOfMonthUseCase
{
    public Task<ResponseIncomesJson> Execute(RequestFilterIncomesJson request);
}
