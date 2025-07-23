using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.Income;

namespace MobileFinance.Infra.DataAccess.Repositories;
public class IncomeRepository : IIncomeWriteOnlyRepository
{
    private readonly MobileFinanceDbContext _dbContext;

    public IncomeRepository(MobileFinanceDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Income income) => await _dbContext.Incomes.AddAsync(income);
}
