namespace MobileFinance.Domain.Repositories.Debit;
public interface IDebitReadOnlyRepository
{
    public Task<Entities.Debit?> GetById(Entities.User user, long debitId); 
}
