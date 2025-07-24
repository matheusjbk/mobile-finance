using Microsoft.EntityFrameworkCore;
using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.Income;

namespace MobileFinance.Infra.DataAccess.Repositories;
public class IncomeRepository : IIncomeWriteOnlyRepository, IIncomeReadOnlyRepository
{
    private readonly MobileFinanceDbContext _dbContext;

    public IncomeRepository(MobileFinanceDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Income income) => await _dbContext.Incomes.AddAsync(income);

    public async Task<Income?> GetById(User user, long incomeId) => 
        await _dbContext.Incomes.FirstOrDefaultAsync(income => income.Active && income.Id.Equals(incomeId) && income.UserId.Equals(user.Id));
}
