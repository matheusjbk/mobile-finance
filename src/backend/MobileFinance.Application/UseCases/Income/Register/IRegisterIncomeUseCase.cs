using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.Application.UseCases.Income.Register;
public interface IRegisterIncomeUseCase
{
    public Task<ResponseRegisteredIncomeJson> Execute(RequestIncomeJson request);
}
