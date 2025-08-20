namespace MobileFinance.Domain.Repositories.Income;
public interface IIncomeReadOnlyRepository
{
    public Task<Entities.Income?> GetById(Entities.User user, long incomeId);
    public Task<IList<Entities.Income>> GetByDayOfMonth(Entities.User user, DateTime date);
    public Task<IEnumerable<Entities.Income>> GetByPeriod(Entities.User user, DateTime start, DateTime end);
}
