namespace MobileFinance.Domain.Repositories.Debit;
public interface IDebitReadOnlyRepository
{
    public Task<Entities.Debit?> GetById(Entities.User user, long debitId);
    public Task<IList<Entities.Debit>> GetByDayOfMonth(Entities.User user, DateTime date);
    public Task<IEnumerable<Entities.Debit>> GetByPeriod(Entities.User user, DateTime start, DateTime end);
}
