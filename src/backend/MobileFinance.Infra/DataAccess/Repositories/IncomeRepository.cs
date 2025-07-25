using Microsoft.EntityFrameworkCore;
using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.Income;

namespace MobileFinance.Infra.DataAccess.Repositories;
public class IncomeRepository : IIncomeWriteOnlyRepository, IIncomeReadOnlyRepository, IIncomeUpdateOnlyRepository
{
    private readonly MobileFinanceDbContext _dbContext;

    public IncomeRepository(MobileFinanceDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Income income) => await _dbContext.Incomes.AddAsync(income);

    public async Task Delete(long incomeId)
    {
        var income = await _dbContext.Incomes.FirstAsync(income => income.Active && income.Id.Equals(incomeId));
        _dbContext.Incomes.Remove(income);
    }

    async Task<Income?> IIncomeReadOnlyRepository.GetById(User user, long incomeId) => await GetFullIncome(user, incomeId, trackQuery: false);

    async Task<Income?> IIncomeUpdateOnlyRepository.GetById(User user, long incomeId) => await GetFullIncome(user, incomeId, trackQuery: true);

    public void Update(Income income) => _dbContext.Incomes.Update(income);

    private async Task<Income?> GetFullIncome(User user, long incomeId, bool trackQuery)
    {
        var query = _dbContext.Incomes.AsQueryable();

        if(!trackQuery)
            query = query.AsNoTracking();

        var income = await query.FirstOrDefaultAsync(income => income.Active && income.Id.Equals(incomeId) && income.UserId.Equals(user.Id));

        return income;
    }
}
