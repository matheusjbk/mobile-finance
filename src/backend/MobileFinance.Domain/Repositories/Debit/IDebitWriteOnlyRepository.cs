namespace MobileFinance.Domain.Repositories.Debit;
public interface IDebitWriteOnlyRepository
{
    public Task Add(Entities.Debit debit);
    public Task Delete(long debitId);
}
