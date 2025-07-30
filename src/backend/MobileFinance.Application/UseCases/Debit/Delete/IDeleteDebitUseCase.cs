namespace MobileFinance.Application.UseCases.Debit.Delete;
public interface IDeleteDebitUseCase
{
    public Task Execute(long debitId);
}
