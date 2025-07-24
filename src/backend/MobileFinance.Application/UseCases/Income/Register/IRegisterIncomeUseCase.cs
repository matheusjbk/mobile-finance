using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.Application.UseCases.Income.Register;
public interface IRegisterIncomeUseCase
{
    public Task<ResponseIncomeJson> Execute(RequestIncomeJson request);
}
