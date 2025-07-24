namespace MobileFinance.Domain.Repositories.Income;
public interface IIncomeUpdateOnlyRepository
{
    public Task<Entities.Income?> GetById(Entities.User user, long incomeId);
    public void Update(Entities.Income income);
}
