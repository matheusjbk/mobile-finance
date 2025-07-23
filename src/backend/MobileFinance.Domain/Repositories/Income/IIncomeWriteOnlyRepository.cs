namespace MobileFinance.Domain.Repositories.Income;
public interface IIncomeWriteOnlyRepository
{
    public Task Add(Entities.Income income);
}
