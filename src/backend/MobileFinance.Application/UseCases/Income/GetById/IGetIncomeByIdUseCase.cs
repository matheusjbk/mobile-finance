using MobileFinance.Communication.Responses;

namespace MobileFinance.Application.UseCases.Income.GetById;
public interface IGetIncomeByIdUseCase
{
    public Task<ResponseIncomeJson> Execute(long incomeId);
}
