using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.Application.UseCases.Debit.GetByDayOfMonth;
public interface IGetDebitByDayOfMonthUseCase
{
    public Task<ResponseDebitsJson> Execute(RequestFilterDebitsJson request);
}
