using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.Application.UseCases.Debit.Register;
public interface IRegisterDebitUseCase
{
    public Task<ResponseRegisteredDebitJson> Execute(RequestDebitJson request);
}
