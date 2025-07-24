using MobileFinance.Communication.Requests;

namespace MobileFinance.Application.UseCases.Income.Update;
public interface IUpdateIncomeUseCase
{
    public Task Execute(RequestIncomeJson request, long incomeId);
}
