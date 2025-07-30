namespace MobileFinance.Domain.Repositories.Debit;
public interface IDebitUpdateOnlyRepository
{
    public Task<Entities.Debit?> GetById(Entities.User user, long debitId);
    public void Update(Entities.Debit debit);
}
