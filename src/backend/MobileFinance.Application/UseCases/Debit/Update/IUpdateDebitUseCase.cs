using MobileFinance.Communication.Requests;

namespace MobileFinance.Application.UseCases.Debit.Update;
public interface IUpdateDebitUseCase
{
    public Task Execute(RequestDebitJson request, long debitId);
}
