using MobileFinance.Communication.Responses;

namespace MobileFinance.Application.UseCases.Debit.GetById;
public interface IGetDebitByIdUseCase
{
    public Task<ResponseDebitJson> Execute(long debitId);
}
